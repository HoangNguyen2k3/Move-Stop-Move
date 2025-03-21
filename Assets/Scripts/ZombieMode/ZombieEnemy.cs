using UnityEngine;
using UnityEngine.AI;

public class ZombieEnemy : MonoBehaviour
{
    private Transform target;
    [SerializeField] private Animator animator;
    private NavMeshAgent agent;
    private string PLAYER_STRING = "Player";
    private ZombieManager manager;
    private bool dance = false;
    private bool isExistPlayer = true;

    public GameObject indicatorPrefab;
    private GameObject indicator;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    private Transform canvasTransform;
    [SerializeField] private Transform posStartThrow;

    public float num_alive_player = 1;
    public bool canAttack = true;
    private void Awake()
    {
        if (target == null)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
                isExistPlayer = false;
            }
        }
        manager = FindFirstObjectByType<ZombieManager>();
        //   levelManager = GetComponent<LevelManager>();
    }
    private void Start()
    {
        canvasTransform = GameObject.FindGameObjectWithTag("CanvasOverlay").transform;
        agent = GetComponent<NavMeshAgent>();
        Indicator();
    }
    private void Indicator()
    {
        indicator = Instantiate(indicatorPrefab, canvasTransform);
        indicator.GetComponent<OffScreenIndicatorZombie>().arrow.color = skinnedMeshRenderer.material.color;
        indicator.GetComponent<OffScreenIndicatorZombie>().target = posStartThrow;
        indicator.GetComponent<OffScreenIndicatorZombie>().mainCamera = Camera.main;
    }
    private void Update()
    {
        if (target == null && isExistPlayer)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                animator.SetBool("isWalk", false);
                target = GameObject.FindGameObjectWithTag("Player").transform;
                isExistPlayer = false;
            }
            else
            {
                animator.SetBool("isWalk", true);
            }
        }
        if (!target && dance == false && !isExistPlayer)
        {
            dance = true;
            agent.isStopped = true;
            animator.SetBool("IsWin", true);
            return;
        }
        if (!target) { return; }
        agent.SetDestination(target.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_STRING))
        {
        }
    }
    /*    private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider && collision.gameObject.CompareTag("Player") && num_alive_player == 1 && canAttack)
            {
                collision.gameObject.GetComponent<PlayerZombie>().isDead = true;
                ZombieManager.Instance.islose = true;
            }
            else if (collision.collider && collision.gameObject.CompareTag("Player") && num_alive_player > 1 && canAttack)
            {
                WaitToPlayerShield();
            }
        }
        public async void WaitToPlayerShield()
        {
            canAttack = false;
            await Task.Delay(2000);
            canAttack = true;
            num_alive_player--;
        }*/
    private void OnDestroy()
    {
        manager.MinusEnemy();
    }
}
