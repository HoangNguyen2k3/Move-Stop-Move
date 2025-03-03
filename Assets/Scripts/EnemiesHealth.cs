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

        }
        if (!take_damage_FX.isPlaying && !isAlive)
        {
            animator.SetBool("IsDead", true);
            Invoke("DestroyEnemy", 1.25f);
        }
    }
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
