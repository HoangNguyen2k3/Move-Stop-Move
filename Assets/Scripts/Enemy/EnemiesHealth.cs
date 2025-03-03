using UnityEngine;

public class EnemiesHealth : MonoBehaviour
{
    [SerializeField] private ParticleSystem take_damage_FX;
    [SerializeField] private GameObject enemy;
    [SerializeField] private SkinnedMeshRenderer current_Mesh;

    public bool isAlive = true;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ThrowWeapon>() && isAlive)
        {
            ParticleSystem temp = Instantiate(take_damage_FX, other.transform.position, Quaternion.identity);
            temp.GetComponent<ParticleSystemRenderer>().material = current_Mesh.material;
            isAlive = false;
            Die();
        }
    }

    private void Die()
    {
        if (!take_damage_FX.isPlaying && !isAlive)
        {
            animator.SetBool(ApplicationVariable.IS_DEAD_STATE, true);
            Invoke("DestroyEnemy", 1.25f);
        }
    }

    private void DestroyEnemy()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.GetComponent<PlayerController>().RemoveEnemyFromList(transform);
        }
        Destroy(enemy.gameObject);
    }
}
