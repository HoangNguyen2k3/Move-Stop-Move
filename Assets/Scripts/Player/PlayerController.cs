using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
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
    [SerializeField] private CinemachineCamera cam_end;

    [SerializeField] private ParticleSystem take_damage_FX;
    [SerializeField] private SkinnedMeshRenderer current_Mesh;

    public float addingScale = 0;
    private bool isCoolDown = false;
    private bool isEnemyInRange = false;
    private Animator animator;
    private Vector3 direct;
    private Rigidbody rb;
    private Transform firstEnemy = null;
    private List<Transform> enemiesInRange = new List<Transform>();

    public bool isDead = false;
    private bool isAnimationDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        circleTarget.SetActive(false);
    }

    private void Update()
    {
        if (isDead && !isAnimationDead) { StartCoroutine(DiePlayer()); }
        direct.x = my_joyStick.Horizontal;
        direct.z = my_joyStick.Vertical;
        RotateCharacter();
        if (isEnemyInRange)
        {
            if (firstEnemy)
            {
                circleTarget.transform.position = firstEnemy.position;
            }

        }
    }

    private void FixedUpdate()
    {
        Movement();
    }
    private IEnumerator DiePlayer()
    {
        ParticleSystem temp = Instantiate(take_damage_FX, transform.position, Quaternion.identity);
        temp.GetComponent<ParticleSystemRenderer>().material = current_Mesh.material;
        isAnimationDead = true;
        animator.SetBool(ApplicationVariable.IS_DEAD_STATE, true);
        yield return new WaitForSeconds(0.3f);
        cam_end.Priority = 10;
        Destroy(gameObject);
    }
    private void Movement()
    {
        if (direct != Vector3.zero)
        {
            rb.position += direct.normalized * speed * Time.fixedDeltaTime;
            animator.SetBool(ApplicationVariable.IDLE_PLAYER_STATE, false);
        }
        else
        {
            animator.SetBool(ApplicationVariable.IDLE_PLAYER_STATE, true);
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
        if (other.CompareTag(ApplicationVariable.ENEMY_TAG))
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
        if (other.CompareTag(ApplicationVariable.ENEMY_TAG))
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
            if (firstEnemy.GetComponent<EnemiesHealth>().isAlive == false)
            {
                isEnemyInRange = false;
                circleTarget.SetActive(false);
                return;
            }
            circleTarget.SetActive(true);
            isEnemyInRange = true;
            circleTarget.transform.position = firstEnemy.position;
        }
        else
        {
            isEnemyInRange = false;
            circleTarget.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(ApplicationVariable.ENEMY_TAG) && !isCoolDown && firstEnemy == other.transform && direct == Vector3.zero)
        {
            StartCoroutine(Attack());
            if (other.gameObject.GetComponentInChildren<TargetPos>())
            {
                ThrowWeapon(other.gameObject.GetComponentInChildren<TargetPos>().transform.position);
            }
        }
    }

    private IEnumerator Attack()
    {
        isCoolDown = true;
        weapon.SetActive(false);
        animator.SetBool(ApplicationVariable.ATTACK_PLAYER_STATE, true);
        yield return new WaitForSeconds(cooldown / 5);
        animator.SetBool(ApplicationVariable.ATTACK_PLAYER_STATE, false);
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
        throwWeaponPrefab.transform.localScale += Vector3.one * addingScale;
        throwWeaponPrefab.GetComponent<ThrowWeapon>().currentlevelObject = GetComponent<LevelManager>();
        //  target.y = posStart.position.y;
        throwWeaponPrefab.GetComponent<ThrowWeapon>().target = target;
        // throwWeaponPrefab.GetComponent<ThrowWeapon>().target = target.GetComponentInChildren<TargetPos>().transform.position;
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
    private void OnDestroy()
    {
        Destroy(circleTarget);
    }
}
