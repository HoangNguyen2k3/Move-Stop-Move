using UnityEngine;
using UnityEngine.AI;

public class ZombieEnemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Animator animator;
    private NavMeshAgent agent;
    private string PLAYER_STRING = "Player";
    private ZombieManager manager;
    private bool dance = false;
    private void Awake()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        manager = FindFirstObjectByType<ZombieManager>();
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (!target && dance == false)
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().isDead = true;
            ZombieManager.Instance.islose = true;
        }
    }
    private void OnDestroy()
    {
        manager.MinusEnemy();
    }
}
