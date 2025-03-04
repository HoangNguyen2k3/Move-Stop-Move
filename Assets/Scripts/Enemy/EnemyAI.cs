using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] private Animator animator;

    private Transform target;
    private EnemiesHealth health;
    private CheckDistance checkDistance;
    private float dis_Detech = 15f;
    private void Start()
    {
        checkDistance = GetComponentInChildren<CheckDistance>();
        health = GetComponentInChildren<EnemiesHealth>();
        target = FindFirstObjectByType<PlayerController>().transform;
    }
    private void Update()
    {
        if (!health.isAlive)
        {
            return;
        }

        float current_dis = Vector3.Distance(target.position, transform.position);
        if (current_dis <= dis_Detech && checkDistance == true)
        {
            animator.SetBool(ApplicationVariable.IDLE_PLAYER_STATE, false);
            enemy.SetDestination(target.position);
        }
        else
        {
            enemy.SetDestination(transform.position);
            animator.SetBool(ApplicationVariable.IDLE_PLAYER_STATE, true);
        }
    }
}
