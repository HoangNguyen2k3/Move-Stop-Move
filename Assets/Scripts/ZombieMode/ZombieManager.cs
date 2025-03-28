using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : Singleton<ZombieManager>
{
    [SerializeField] private TextMeshProUGUI enemy_alive;
    [SerializeField] private GameObject winningGame;
    [SerializeField] private List<GameObject> enemy;
    [SerializeField] private GameObject loseGame;
    [SerializeField] private GameObject loseReal;

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
    public bool currentInLobbyZombie = true;
    [Header("Coin Manager")]
    [HideInInspector] public float num_coin = 0;
    [SerializeField] private TextMeshProUGUI coin_win;
    [SerializeField] private TextMeshProUGUI coin_lose;
    [SerializeField] private TextMeshProUGUI coin_win_x3;
    [SerializeField] private TextMeshProUGUI coin_lose_x3;
    private CoinManager coinManager;
    public bool x2GoldAbilities = false;

    public GameObject floatingTextPlayer;
    public GameObject CanvasIndicator;

    [SerializeField] private bool isMapBoss = false;
    private int maxEnemyType;


    [SerializeField] private SaveDayZombieMode saveDayZombieMode;

    [SerializeField] private TextMeshProUGUI day_zombie;
    private void Start()
    {
        saveDayZombieMode.current_day = PlayerPrefs.GetInt("DayZombieMode", 1);
        day_zombie.text = "DAY " + saveDayZombieMode.current_day.ToString();
        if (saveDayZombieMode.current_day == 5)
        {
            isMapBoss = true;
        }
        enemy_remain = saveDayZombieMode.num_enemy_day[saveDayZombieMode.current_day - 1];
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

            currentInLobbyZombie = true;
            //            floatingTextPlayer.SetActive(false);
            CanvasIndicator.SetActive(false);
            SetUpCoin();
            temp = true;
            loseReal.SetActive(true);
            //           loseGame.SetActive(true);
            CancelInvoke(nameof(SpawnEnemy));
        }
        if (islose) { return; }
        if (enemy_remain <= 0 && !iswinning)
        {
            if (saveDayZombieMode.current_day < 5)
            {
                saveDayZombieMode.current_day += 1;
                PlayerPrefs.SetInt("DayZombieMode", saveDayZombieMode.current_day);
            }
            currentInLobbyZombie = true;
            CanvasIndicator.SetActive(false);
            floatingTextPlayer.SetActive(false);
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
    public void RevivePlayer()
    {
        playerController.RevivePlayer();
    }
    public void CanRevive()
    {
        loseGame.SetActive(true);
    }
    public void DeadPlayer()
    {
        playerController.DeadPlayer();
    }
    public void ChangeInShop()
    {
        currentInLobbyZombie = false;
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
        if (isMapBoss)
        {
            for (int i = 0; i < a; i++)
            {
                int random_enemy = Random.Range(0, enemy.Count);
                Instantiate(enemy[random_enemy], GetRandomNavMeshPosition(transform.position, rangeSpawn), Quaternion.identity);
                if (isMapBoss)
                {
                    if (enemy[random_enemy].GetComponent<EnemiesHealth>().isBoss)
                    {
                        enemy.Remove(enemy[random_enemy]);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < a; i++)
            {
                int random_enemy = Random.Range(0, enemy.Count - 1);
                Instantiate(enemy[random_enemy], GetRandomNavMeshPosition(transform.position, rangeSpawn), Quaternion.identity);
            }
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
                    if (randomPoint.y > 0f) { continue; }
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
                if (CheckReviveCondition(randomPoint, 12f))
                {
                    if (randomPoint.y > 0f) { continue; }
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
    public void StopSpawn()
    {
        CancelInvoke(nameof(SpawnEnemy));
    }
    public void ContinueSpawn()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0, 2.5f);
    }
}
