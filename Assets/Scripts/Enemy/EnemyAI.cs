using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] public Animator animator;

    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float wanderInterval = 3f;

    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float timeCoolDown = 1.5f;
    [SerializeField] public GameObject weaponThrow;
    [SerializeField] private Transform posStartThrow;
    [SerializeField] private GameObject weapon;

    private GameManager gameManager;
    private bool isAttacking = false;

    private Transform target;
    private EnemiesHealth health;
    private Vector3 randomPoint;
    private List<Transform> enemiesInRange = new List<Transform>();

    public bool iswinning = false;
    public GameObject indicatorPrefab;
    public Transform canvasTransform;
    private GameObject indicator;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    private bool dontMove = false;

    private void Awake()
    {
        canvasTransform = GameObject.FindGameObjectWithTag("CanvasOverlay").transform;
        gameManager = FindFirstObjectByType<GameManager>();
        health = GetComponentInChildren<EnemiesHealth>();

    }
    private void Start()
    {
        InvokeRepeating(nameof(Wander), 0f, wanderInterval);
        Indicator();
    }

    private void Indicator()
    {
        indicator = Instantiate(indicatorPrefab, canvasTransform);
        indicator.GetComponent<Image>().color = skinnedMeshRenderer.material.color;
        //        Debug.Log(skinnedMeshRenderer.material.color);
        //       Debug.Log(indicator.GetComponent<Image>().color);
        indicator.GetComponent<OffScreenIndicator>().target = enemy.transform;
        indicator.GetComponent<OffScreenIndicator>().mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!health.isAlive || iswinning)
        {
            if (!dontMove)
            {
                dontMove = true;
                CancelInvoke(nameof(Wander));

            }
            return;
        }
        if (randomPoint == transform.position)
        {
            animator.SetBool("IsIdle", true);
        }
        if (target == null) FindNearestTarget();

        if (target != null && Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            CancelInvoke(nameof(Wander));
            StopAndAttack();
        }
        else
        {
            if (!IsInvoking(nameof(Wander)))
            {
                InvokeRepeating(nameof(Wander), 0f, wanderInterval);
            }
        }
    }
    private void FindNearestTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var col in colliders)
        {
            if ((col.CompareTag("Player") || col.CompareTag("Enemy")) && col.transform.gameObject != transform.GetChild(1).gameObject)
            {
                float dist = Vector3.Distance(transform.position, col.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = col.transform;
                }
            }
        }

        target = nearest;
    }
    /*    private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, attackRange);
        }*/
    private void StopAndAttack()
    {
        enemy.isStopped = true;
        transform.LookAt(target);

        if (!isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        weapon.SetActive(false);
        animator.SetBool("IsAttack", true);
        //        yield return new WaitForSeconds(0.2f);


        GameObject throwWeapon = Instantiate(weaponThrow, posStartThrow.position, Quaternion.identity);
        throwWeapon.GetComponent<ThrowWeapon>().who_throw_obj = transform.GetChild(1).gameObject;
        throwWeapon.GetComponent<ThrowWeapon>().currentlevelObject = GetComponent<LevelManager>();
        throwWeapon.GetComponent<ThrowWeapon>().who_throw = "Enemy";
        throwWeapon.GetComponent<ThrowWeapon>().target = target.GetComponentInChildren<TargetPos>().transform.position;

        yield return new WaitForSeconds(timeCoolDown / 2);
        weapon.SetActive(true);
        yield return new WaitForSeconds(timeCoolDown / 2);
        target = null;
        FindNearestTarget();
        isAttacking = false;
        animator.SetBool("IsAttack", false);
    }

    private void Wander()
    {
        if (!health.isAlive || iswinning) { return; }
        if (enemy.isStopped) enemy.isStopped = false;

        //       else
        //        {
        animator.SetBool("IsIdle", false);
        enemy.SetDestination(GetRandomNavMeshPosition(transform.position, wanderRadius));
        //       }
    }
    /*    private IEnumerator WaitForSetNewDes()
        {
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("IsIdle", false);
            enemy.SetDestination(GetRandomNavMeshPosition(transform.position, wanderRadius));
        }*/
    private Vector3 GetRandomNavMeshPosition(Vector3 origin, float radius)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius + origin;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                randomPoint = hit.position;
                return hit.position;
            }
        }
        return origin;
    }
    private void OnDisable()
    {
        if (indicator)
        {
            Destroy(indicator);
        }
        if (gameManager)
            gameManager.MinusEnemy();
    }
}
