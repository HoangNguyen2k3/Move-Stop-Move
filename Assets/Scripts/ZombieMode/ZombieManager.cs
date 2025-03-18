using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : Singleton<ZombieManager>
{
    [SerializeField] private TextMeshProUGUI enemy_alive;
    [SerializeField] private GameObject winningGame;
    [SerializeField] private GameObject[] enemy;
    [SerializeField] private GameObject loseGame;

    [SerializeField] private float enemy_spawn_pertime;
    [SerializeField] public float enemy_remain;
    public PlayerController playerController;
    private float enemy_not_spawn_num;
    [HideInInspector] public string name_enemy_win;
    [HideInInspector] public float num_coin = 0;
    public bool iswinning = false;
    public bool islose = false;
    private bool temp = false;
    private Vector3 randomPoint;

    private float rangeSpawn = 25f;
    private void Start()
    {
        enemy_not_spawn_num = enemy_remain;
        enemy_alive.text = quickAddText(enemy_remain);
        InvokeRepeating(nameof(SpawnEnemy), 0, 2.5f);
    }
    private void Update()
    {

        if (islose && !temp)
        {
            temp = true;
            loseGame.SetActive(true);
            CancelInvoke(nameof(SpawnEnemy));
        }
        if (islose) { return; }
        if (enemy_remain <= 0 && !iswinning)
        {
            iswinning = true;
            enemy_alive.text = quickAddText(0);
            winningGame.SetActive(true);
            //    earnCoinwin.text = num_coin.ToString();
            playerController.animator.SetBool("IsWin", true);
            playerController.isWinning = true;
        }

    }
    private string quickAddText(float num)
    {
        return num.ToString();
    }
    public void MinusEnemy()
    {
        enemy_remain--;
        enemy_alive.text = quickAddText(enemy_remain);
    }
    private void SpawnEnemy()
    {
        if (enemy_not_spawn_num == 0)
        {
            return;
        }
        if (enemy_remain <= 0) { enemy_alive.text = quickAddText(0); return; }
        if (enemy_not_spawn_num > enemy_spawn_pertime)
        {
            SpawnEnemyPerTime(enemy_spawn_pertime);
            enemy_not_spawn_num -= enemy_spawn_pertime;
        }
        else
        {
            SpawnEnemyPerTime(enemy_not_spawn_num);
            enemy_not_spawn_num = 0;
        }
    }
    private void SpawnEnemyPerTime(float a)
    {
        for (int i = 0; i < a; i++)
        {
            int random_enemy = Random.Range(0, enemy.Length);
            Instantiate(enemy[random_enemy], GetRandomNavMeshPosition(transform.position, rangeSpawn), Quaternion.identity);
        }
    }
    private Vector3 GetRandomNavMeshPosition(Vector3 origin, float radius)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius + origin;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                randomPoint = hit.position;
                if (Vector3.Distance(randomPoint, playerController.gameObject.transform.position) < 6.1f)
                {
                    continue;
                }
                return hit.position;
            }
        }
        return origin;
    }
    public void SettingEnemyMaxCount(float num)
    {
        enemy_remain = num;
        enemy_not_spawn_num = enemy_remain;
    }
}
