using TMPro;
using Unity.Cinemachine;
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
    public PlayerZombie playerController;
    private float enemy_not_spawn_num;
    [HideInInspector] public string name_enemy_win;
    public bool iswinning = false;
    public bool islose = false;
    private bool temp = false;
    private bool startingGame = false;
    private Vector3 randomPoint;

    private float rangeSpawn = 25f;
    [Header("ZombieMode")]
    [SerializeField] private CinemachineCamera camWinning;
    public bool currentInLobbyZombie = false;
    [Header("Coin Manager")]
    [HideInInspector] public float num_coin = 0;
    [SerializeField] private TextMeshProUGUI coin_win;
    [SerializeField] private TextMeshProUGUI coin_lose;
    [SerializeField] private TextMeshProUGUI coin_win_x3;
    [SerializeField] private TextMeshProUGUI coin_lose_x3;
    private CoinManager coinManager;
    public bool x2GoldAbilities = false;
    private void Start()
    {
        coinManager = GetComponent<CoinManager>();
        enemy_not_spawn_num = enemy_remain;
        enemy_alive.text = quickAddText(enemy_remain);
        //InvokeRepeating(nameof(SpawnEnemy), 0, 2.5f);
        SpawnEnemy();
    }
    private void Update()
    {
        if (!startingGame && playerController.gameObject.activeSelf == true)
        {
            startingGame = true;
            InvokeRepeating(nameof(SpawnEnemy), 0, 2.5f);
        }
        if (islose && !temp)
        {
            SetUpCoin();
            temp = true;
            loseGame.SetActive(true);
            CancelInvoke(nameof(SpawnEnemy));
        }
        if (islose) { return; }
        if (enemy_remain <= 0 && !iswinning)
        {
            iswinning = true;
            enemy_alive.text = quickAddText(0);
            SetUpCoin();
            winningGame.SetActive(true);
            camWinning.Priority = 10;
            //    earnCoinwin.text = num_coin.ToString();
            playerController.animator.SetBool("IsWin", true);
            playerController.isWinning = true;
        }

    }
    public void SetUpCoin()
    {
        if (x2GoldAbilities) { num_coin *= 2; }
        coin_win.text = num_coin.ToString();
        coin_lose.text = num_coin.ToString();
        coin_win_x3.text = (num_coin * 3).ToString();
        coin_lose_x3.text = (num_coin * 3).ToString();
    }
    public void EarnCoin()
    {
        coinManager.AddingCoin();
    }
    public void EarnCoinX3()
    {
        coinManager.AddingCoinXn(3);
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
                    if (randomPoint.y > 0.5f) { continue; }
                    continue;
                }
                return hit.position;
            }
        }
        return origin;
    }
    private bool CheckReviveCondition(Vector3 posCheck, float distance)
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var item in enemy)
        {
            float dis = Vector3.Distance(item.transform.position, posCheck);
            if (dis < distance)
            {
                return false;
            }
        }
        return true;
    }
    public Vector3 GetRandomPositionRevivePlayer(float radius)
    {
        Vector3 origin = transform.position;
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius + origin;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                randomPoint = hit.position;
                if (CheckReviveCondition(randomPoint, 10f))
                {
                    if (randomPoint.y > 0.5f) { continue; }
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
