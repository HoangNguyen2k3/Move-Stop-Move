using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    [SerializeField] private WeaponObject weapon;

    [HideInInspector] public GameObject who_throw_obj;
    [HideInInspector] public string who_throw = "Player";
    [HideInInspector] public LevelManager currentlevelObject;
    [HideInInspector] public Vector3 target;
    private Vector3 startDir;
    private Vector3 startPosition;
    public bool isZombieMode = false;
    [HideInInspector] public Vector3 dir;

    [HideInInspector] public Transform target_transform;
    private bool check = false;

    public bool upScaleWeapon = false;
    private float scaleSpeed = 3f;
    private float maxScale = 9f;
    public bool throwEnemy = false;
    public bool throwWall = false;
    private float range_attack;
    //    [Header("Sound")]
    //    [SerializeField] private AudioClip touchSmt;
    private void Start()
    {
        scaleSpeed *= transform.localScale.x;
        maxScale *= transform.localScale.x;
        //        dir.y = 0.5f;
        if (!weapon.isTurning)
        {
            if (!isZombieMode)
                transform.LookAt(target);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - 90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        startPosition = transform.position;
        startDir = (target - transform.position).normalized;
        range_attack = weapon.range;
        if (upScaleWeapon)
        {
            range_attack += 10f;
        }
    }
    private void Update()
    {
        if (target_transform != null)
        {
            check = true;
        }
        if (currentlevelObject == null) { Destroy(gameObject); }

        if (upScaleWeapon)
        {
            MainWeapon();
            IncreaseScale();
            return;
        }
        if (target_transform == null && !check)
        {
            MainWeapon();
        }
        else
        {
            ChaseWeapon();
        }

    }
    private void ChaseWeapon()
    {
        if (target_transform == null)
        {
            FindNearestTarget();
            if (target_transform == null) return;
        }
        if (!weapon.isTurning)
        {
            transform.LookAt(target_transform);
            transform.rotation = Quaternion.Euler(-90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        if (weapon.isTurning)
        {
            transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y + (weapon.speedRotate * Time.deltaTime), 0f);
        }
        Vector3 targetPosition = target_transform.position;
        targetPosition.y = who_throw_obj.transform.position.y + 0.6f;
        float step = weapon.speedMove * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        if (Vector3.Distance(startPosition, transform.position) >= range_attack)
        {
            Destroy(gameObject);
        }
    }
    private void IncreaseScale()
    {
        if (transform.localScale.x < maxScale)
        {
            float newScale = transform.localScale.x + (scaleSpeed * Time.deltaTime);
            newScale = Mathf.Min(newScale, maxScale);
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }
    private void FindNearestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        target_transform = closestEnemy;
    }
    private void MainWeapon()
    {
        Vector3 newPosition = FindNewPosition();
        transform.position = newPosition;
        if (Vector3.Distance(startPosition, transform.position) >= range_attack)
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
        Vector3 newPosition = transform.position + dir * weapon.speedMove * Time.deltaTime;
        if (!isZombieMode)
        {
            newPosition = transform.position + startDir * weapon.speedMove * Time.deltaTime;
        }
        return newPosition;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ThrowWeapon>() || other.CompareTag("ThrowWeapon") || other.gameObject.layer == LayerMask.NameToLayer("Road")) { return; }
        if (who_throw == ApplicationVariable.PLAYER_TAG)
        {
            if (other.gameObject.CompareTag(ApplicationVariable.IGNORE_TAG)) { return; }
            if (other.gameObject.GetComponentInChildren<EnemiesHealth>() &&
                other.gameObject.GetComponentInChildren<EnemiesHealth>().isBoss == false
                && other.gameObject.GetComponentInChildren<EnemiesHealth>().isScore == false)
            {
                currentlevelObject.AddLevel();
                if (!throwEnemy)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (!other.gameObject.CompareTag(ApplicationVariable.PLAYER_TAG) && !throwWall)
                {
                    Instantiate(weapon.touchSomething, transform.position, Quaternion.identity);
                    Debug.Log(other.gameObject.name);
                    Destroy(gameObject);
                }
                else if (!other.gameObject.GetComponentInChildren<EnemiesHealth>() && !other.gameObject.CompareTag(ApplicationVariable.PLAYER_TAG) && !throwWall)
                {
                    Instantiate(weapon.touchSomething, transform.position, Quaternion.identity);
                    Destroy(gameObject);
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
                other.gameObject.GetComponent<EnemiesHealth>().TakeColorMaterial();
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
    private void OnDisable()
    {
        if (SoundManager.Instance)
            SoundManager.Instance.PlaySFXSound(SoundManager.Instance.hit_something);
    }
}
