using UnityEngine;

public class EnemiesHealth : MonoBehaviour
{
    [SerializeField] private ParticleSystem take_damage_FX;
    private bool isAlive = true;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ThrowWeapon>() && isAlive)
        {
            Instantiate(take_damage_FX, other.transform.position, Quaternion.identity);
            isAlive = false;
            Die();
        }
    }

    private void Die()
    {
        if (!take_damage_FX.isPlaying && !isAlive)
        {
            animator.SetBool("IsDead", true);
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
        Destroy(gameObject);
    }
}
