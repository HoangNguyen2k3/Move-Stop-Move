using UnityEngine;

public class EnemiesHealth : MonoBehaviour
{
    [SerializeField] private ParticleSystem take_damage_FX;
    [SerializeField] private GameObject enemy;
    [SerializeField] private SkinnedMeshRenderer current_Mesh;
    [SerializeField] private Transform pos_particle;
    private Collider currentCollider;
    public bool isAlive = true;
    private Animator animator;
    private void Start()
    {
        currentCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ThrowWeapon>() && isAlive)
        {
            if (other.GetComponent<ThrowWeapon>().who_throw == "Enemy") { return; }
            //   ParticleSystem temp = Instantiate(take_damage_FX, other.transform.position, Quaternion.identity);
            ParticleSystem temp = Instantiate(take_damage_FX, pos_particle.position, Quaternion.identity);
            temp.GetComponent<ParticleSystemRenderer>().material = current_Mesh.material;
            isAlive = false;
            Die();
        }
    }

    public void Die()
    {
        Destroy(currentCollider);
        if (!take_damage_FX.isPlaying && !isAlive)
        {
            animator.SetBool(ApplicationVariable.IS_DEAD_STATE, true);
            Invoke("DestroyEnemy", 1.0f);
        }
    }

    private void DestroyEnemy()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.GetComponent<PlayerController>().RemoveEnemyFromList(transform);
        }
        if (enemy.gameObject)
        {
            Destroy(enemy.gameObject);
        }
    }
}
