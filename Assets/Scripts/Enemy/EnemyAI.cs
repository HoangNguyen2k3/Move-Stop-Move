using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private LevelManager levelManager;

    private bool dontMove = false;

    private void Awake()
    {
        canvasTransform = GameObject.FindGameObjectWithTag("CanvasOverlay").transform;
        health = GetComponentInChildren<EnemiesHealth>();
        levelManager = GetComponent<LevelManager>();

    }
    private void Start()
    {
        InvokeRepeating(nameof(Wander), 0f, wanderInterval);
        Indicator();
    }

    private void Indicator()
    {
        indicator = Instantiate(indicatorPrefab, canvasTransform);
        indicator.GetComponent<IndicatorObj>().arrow.color = skinnedMeshRenderer.material.color;
        indicator.GetComponent<IndicatorObj>().backGround.color = skinnedMeshRenderer.material.color;
        indicator.GetComponent<IndicatorObj>().numEnemyLevel.text = levelManager.current_level.ToString();
        //       indicator.GetComponent<Image>().color = skinnedMeshRenderer.material.color;
        //        Debug.Log(skinnedMeshRenderer.material.color);
        //       Debug.Log(indicator.GetComponent<Image>().color);
        indicator.GetComponent<OffScreenIndicator>().target = enemy.transform;
        indicator.GetComponent<OffScreenIndicator>().mainCamera = Camera.main;
    }

    private void Update()
    {
        if (LobbyManager.Instance.currentinLobby)
        {
            return;
        }
        indicator.GetComponent<IndicatorObj>().numEnemyLevel.text = levelManager.current_level.ToString();
        if (!health.isAlive || iswinning)
        {
            enemy.isStopped = true;
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
            if (!IsInvoking(nameof(Wander)) && enemy.isStopped == false)
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
        transform.LookAt(target);

        //        enemy.ResetPath();

        if (!isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        //        enemy.isStopped = true;
        enemy.isStopped = true;
        isAttacking = true;
        weapon.SetActive(false);
        animator.SetBool("IsAttack", true);


        GameObject throwWeapon = Instantiate(weaponThrow, posStartThrow.position, Quaternion.identity);
        throwWeapon.GetComponent<ThrowWeapon>().who_throw_obj = transform.GetChild(1).gameObject;
        throwWeapon.GetComponent<ThrowWeapon>().currentlevelObject = GetComponent<LevelManager>();
        throwWeapon.GetComponent<ThrowWeapon>().who_throw = "Enemy";
        throwWeapon.GetComponent<ThrowWeapon>().target = target.GetComponentInChildren<TargetPos>().transform.position;

        //        yield return new WaitForSeconds(timeCoolDown / 2.5f);
        yield return new WaitForSeconds(1f);
        enemy.isStopped = false;
        weapon.SetActive(true);
        animator.SetBool("IsAttack", false);
        //        enemy.isStopped = false;
        yield return new WaitForSeconds(timeCoolDown / 5);
        target = null;
        FindNearestTarget();
        isAttacking = false;
    }

    private void Wander()
    {
        if (!health.isAlive || iswinning || LobbyManager.Instance.currentinLobby) { return; }
        if (enemy.isStopped) { enemy.isStopped = false; }

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
        if (GameManager.Instance)
        {
            GameManager.Instance.MinusEnemy();
        }
    }
}
