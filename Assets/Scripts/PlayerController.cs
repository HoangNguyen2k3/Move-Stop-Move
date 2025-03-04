using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float angle = 5f;
    [SerializeField] private Joystick my_joyStick;
    [SerializeField] private GameObject weapon;
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private GameObject throwWeapon;
    [SerializeField] private Transform posStart;

    [SerializeField] private GameObject circleTarget;

    private string IDLE_STATE = "IsIdle";
    private string ATTACK_STATE = "IsAttack";
    private string ENEMY_TAG = "Enemy";

    private bool isCoolDown = false;
    private Animator animator;
    private Vector3 direct;
    private Rigidbody rb;

    private Transform firstEnemy = null;
    private List<Transform> enemiesInRange = new List<Transform>();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        circleTarget.SetActive(false);
    }

    private void Update()
    {
        direct.x = my_joyStick.Horizontal;
        direct.z = my_joyStick.Vertical;
        RotateCharacter();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (direct != Vector3.zero)
        {
            rb.position += direct.normalized * speed * Time.fixedDeltaTime;
            animator.SetBool(IDLE_STATE, false);
        }
        else
        {
            animator.SetBool(IDLE_STATE, true);
        }
    }

    private void RotateCharacter()
    {
        if (direct == Vector3.zero)
            return;
        Quaternion rot = Quaternion.LookRotation(direct.normalized);
        transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rot, angle * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ENEMY_TAG))
        {
            Transform enemy = other.transform;
            if (!enemiesInRange.Contains(enemy))
                enemiesInRange.Add(enemy);

            if (firstEnemy == null)
            {
                firstEnemy = enemy;
                UpdateCircleTarget();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ENEMY_TAG))
        {
            Transform enemy = other.transform;
            enemiesInRange.Remove(enemy);

            if (enemy == firstEnemy)
            {
                firstEnemy = enemiesInRange.Count > 0 ? enemiesInRange[0] : null;
                UpdateCircleTarget();
            }
        }
    }

    private void UpdateCircleTarget()
    {
        if (firstEnemy != null)
        {
            circleTarget.SetActive(true);
            circleTarget.transform.position = firstEnemy.position;
        }
        else
        {
            circleTarget.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(ENEMY_TAG) && !isCoolDown && firstEnemy == other.transform && direct == Vector3.zero)
        {
            StartCoroutine(Attack(other.transform));
            ThrowWeapon(other.transform.position);
        }
    }

    private IEnumerator Attack(Transform target)
    {
        isCoolDown = true;
        weapon.SetActive(false);
        animator.SetBool(ATTACK_STATE, true);
        yield return new WaitForSeconds(cooldown / 5);
        animator.SetBool(ATTACK_STATE, false);
        yield return new WaitForSeconds(cooldown / 2);
        weapon.SetActive(true);
        yield return new WaitForSeconds(cooldown / 2);
        isCoolDown = false;
    }

    public void ThrowWeapon(Vector3 target)
    {
        if (firstEnemy != null)
        {
            Vector3 directionToEnemy = firstEnemy.position - transform.position;
            directionToEnemy.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angle * 10 * Time.deltaTime);
        }

        GameObject throwWeaponPrefab = Instantiate(throwWeapon, posStart.position, Quaternion.identity);
        target.y = posStart.position.y;
        throwWeaponPrefab.GetComponent<ThrowWeapon>().target = target;
    }
    public void RemoveEnemyFromList(Transform enemy)
    {
        if (enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);

            if (enemy == firstEnemy)
            {
                firstEnemy = enemiesInRange.Count > 0 ? enemiesInRange[0] : null;
                UpdateCircleTarget();
            }
        }
    }

}
