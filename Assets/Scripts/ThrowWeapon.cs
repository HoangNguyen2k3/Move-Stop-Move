using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    [SerializeField] private WeaponObject weapon;

    [HideInInspector] public GameObject who_throw_obj;
    [HideInInspector] public string who_throw = "Player";
    [HideInInspector] public LevelManager currentlevelObject;
    [HideInInspector] public Vector3 target;
    private Vector3 startPosition;
    private void Start()
    {
        if (!weapon.isTurning)
        {
            transform.LookAt(target);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - 90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        startPosition = transform.position;
    }
    private void Update()
    {
        if (currentlevelObject == null) { Destroy(gameObject); }
        Vector3 newPosition = FindNewPosition();
        transform.position = newPosition;
        if (Vector3.Distance(startPosition, transform.position) >= weapon.range)
        {
            Destroy(gameObject);
        }
    }

    private Vector3 FindNewPosition()
    {
        if (weapon.isTurning)
        {
            transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y + (weapon.speedRotate * Time.deltaTime), 0f);
        }
        Vector3 direction = (target - startPosition).normalized;
        Vector3 newPosition = transform.position + direction * weapon.speedMove * Time.deltaTime;

        return newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ThrowWeapon>()) { return; }
        if (who_throw == ApplicationVariable.PLAYER_TAG)
        {
            if (other.gameObject.CompareTag(ApplicationVariable.IGNORE_TAG)) { return; }
            if (other.gameObject.GetComponentInChildren<EnemiesHealth>())
            {
                currentlevelObject.AddLevel();
                Destroy(gameObject);
            }
            else
            {
                if (!other.gameObject.CompareTag(ApplicationVariable.PLAYER_TAG))
                {
                    Destroy(gameObject);
                }
                else if (!other.gameObject.GetComponentInChildren<EnemiesHealth>() && !other.gameObject.CompareTag(ApplicationVariable.PLAYER_TAG))
                {
                    Instantiate(weapon.touchSomething, transform.position, Quaternion.identity);
                }
            }
        }
        else
        {
            if (other.gameObject.CompareTag(ApplicationVariable.IGNORE_TAG)) { return; }
            if (other.gameObject.GetComponentInChildren<PlayerController>())
            {
                if (other.isTrigger) { return; }
                currentlevelObject.AddLevel();
                GameManager.Instance.name_enemy_win = who_throw_obj.GetComponentInParent<GenerateEnemyType>().nameEnemy.text;
                other.gameObject.GetComponentInChildren<PlayerController>().isDead = true;

                Destroy(gameObject);
            }
            else if (other.gameObject != who_throw_obj && other.gameObject.CompareTag(ApplicationVariable.ENEMY_TAG) && who_throw_obj != null)
            {
                other.gameObject.GetComponent<EnemiesHealth>().isAlive = false;
                currentlevelObject.AddLevel();
                other.gameObject.GetComponent<EnemiesHealth>().Die();
                Destroy(gameObject);
            }
            else
            {
                if (!other.gameObject.GetComponentInChildren<PlayerController>() && !other.gameObject.CompareTag(ApplicationVariable.ENEMY_TAG))
                {
                    Instantiate(weapon.touchSomething, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }

    }
}
