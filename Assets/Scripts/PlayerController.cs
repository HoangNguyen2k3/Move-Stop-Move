using System.Collections;
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

    private string IDLE_STATE = "IsIdle";
    private string ATTACK_STATE = "IsAttack";
    private string ENEMY_TAG = "Enemy";

    private bool isCoolDown = false;
    private Animator animator;
    private Vector3 direct;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(ENEMY_TAG) && !isCoolDown)
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
        GameObject throwWeaponPrefab = Instantiate(throwWeapon, posStart.position, transform.rotation);
        throwWeaponPrefab.GetComponent<ThrowWeapon>().target = target;
    }
}
