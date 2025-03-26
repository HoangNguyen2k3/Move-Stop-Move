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
    [Header("---------------Zombie Mode------------------")]
    public bool isZombie = false;
    public bool isBoss = false;
    private int hpBoss = 10;
    public bool isScore = false;
    private void Start()
    {
        currentCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isZombie)
        {
            if (other.GetComponent<ThrowWeapon>() && isAlive)
            {
                if (other.GetComponent<ThrowWeapon>().who_throw == "Enemy") { return; }
                //  if (other.gameObject == enemy) { return; }
                //   ParticleSystem temp = Instantiate(take_damage_FX, other.transform.position, Quaternion.identity);
                ParticleSystem temp = Instantiate(take_damage_FX, pos_particle.position, Quaternion.identity);
                isAlive = false;
                temp.GetComponent<ParticleSystemRenderer>().material = current_Mesh.material;
                Die();
            }
            return;
        }
        if ((other.CompareTag("ThrowWeapon") || other.GetComponent<ThrowWeapon>()) && isAlive)
        {

            //  if (other.gameObject == enemy) { return; }
            //   ParticleSystem temp = Instantiate(take_damage_FX, other.transform.position, Quaternion.identity);
            ParticleSystem temp = Instantiate(take_damage_FX, pos_particle.position, Quaternion.identity);
            if (isBoss)
            {
                TakedDamageBoss();
            }
            else
            {
                isScore = true;
                isAlive = false;
                ZombieEnemy();
            }
            temp.GetComponentInChildren<ParticleSystemRenderer>().material = current_Mesh.material;
        }
    }
    private void TakedDamageBoss()
    {
        hpBoss -= 1;
        if (hpBoss <= 0)
        {
            isAlive = false;
            CircleRange player = FindFirstObjectByType<CircleRange>();
            if (player != null)
            {
                player.GetComponent<CircleRange>().RemoveEnemyFromList(transform);
            }
            PlayerZombie player_z = FindFirstObjectByType<PlayerZombie>();
            player_z.gameObject.GetComponent<LevelManager>().AddLevel();
            Destroy(currentCollider); Destroy(gameObject);
        }
    }
    public void TakeColorMaterial()
    {
        ParticleSystem temp = Instantiate(take_damage_FX, pos_particle.position, Quaternion.identity);
        isAlive = false;
        temp.GetComponent<ParticleSystemRenderer>().material = current_Mesh.material;
        //Die();
    }
    private void ZombieEnemy()
    {
        CircleRange player = FindFirstObjectByType<CircleRange>();
        if (player != null)
        {
            player.GetComponent<CircleRange>().RemoveEnemyFromList(transform);
        }
        Destroy(currentCollider); Destroy(gameObject);
    }

    public void Die()
    {
        Destroy(currentCollider);
        if (!take_damage_FX.isPlaying && !isAlive)
        {
            SoundManager.Instance?.PlaySFXSound(SoundManager.Instance.dead);
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
