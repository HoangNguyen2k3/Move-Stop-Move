using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private TextMeshProUGUI enemy_alive;
    [SerializeField] private GameObject winningGame;
    [SerializeField] private GameObject[] enemy;

    [SerializeField] private float enemy_spawn_pertime;
    [SerializeField] public float enemy_remain; //Tong quai can danh hien tai ke ca chua spawn
    public PlayerController playerController;
    private float enemy_not_spawn_num;
    [HideInInspector] public string name_enemy_win;
    [HideInInspector] public float num_coin = 0;
    [SerializeField] private TextMeshProUGUI earnCoinwin;
    public bool iswinning = false;
    public bool islose = false;
    private bool firstTime = true;
    private string enemy_text = "ALIVE: ";
    private Vector3 randomPoint;
    private UIGeneratePress ui_generate;
    [SerializeField] private UIManager uiManager;

    private float rangeSpawn = 30f;
    [SerializeField] private CinemachineCamera winningCam;
    [SerializeField] private CoinManager coinManager;
    private void Start()
    {
        coinManager = GetComponent<CoinManager>();
        ui_generate = GetComponent<UIGeneratePress>();
        if (PlayerPrefs.HasKey("num_enemy_level"))
        {
            enemy_remain = PlayerPrefs.GetFloat("num_enemy_level");
        }
        enemy_not_spawn_num = enemy_remain;
        enemy_alive.text = quickAddText(enemy_remain);
        InvokeRepeating(nameof(SpawnEnemy), 0, 2.5f);
    }
    private void Update()
    {

        if (enemy_remain <= 0 && !iswinning)
        {
            SoundManager.Instance.PlaySFXSound(SoundManager.Instance.win_sound);
            iswinning = true;
            enemy_alive.text = quickAddText(0);
            uiManager.ProcessEndGame();
            winningCam.Priority = 10;
            winningGame.SetActive(true);
            earnCoinwin.text = num_coin.ToString();
            playerController.animator.SetBool("IsWin", true);
            ui_generate.ShowAndHiddenGameObject();
            playerController.isWinning = true;
            LobbyManager.Instance.currentinLobby = true;
        }
        if (playerController == null && enemy_remain == 1 && !iswinning)
        {
            iswinning = true;
            EnemyAI enemy_win = FindFirstObjectByType<EnemyAI>();
            enemy_win.animator.SetBool("IsWin", true);
            enemy_win.iswinning = true;
        }
    }
    public void Revive()
    {
        playerController.RevivePlayer();
    }
    public void DestroyPlayer()
    {
        Destroy(playerController.gameObject);
    }
    public void LoopSpawn()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0, 2.5f);
    }
    private string quickAddText(float num)
    {
        return enemy_text + num.ToString();
    }
    public void MinusEnemy()
    {
        enemy_remain--;
        enemy_alive.text = quickAddText(enemy_remain);
    }
    private void SpawnEnemy()
    {
        if (LobbyManager.Instance.currentinLobby && firstTime == true)
        {
            SpawnEnemyFirstTime();
        }
        if (LobbyManager.Instance.currentinLobby) { return; }
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
    private void SpawnEnemyFirstTime()
    {
        firstTime = false;
        rangeSpawn = 7f;
        SpawnEnemyPerTime(2);
        enemy_not_spawn_num -= 2;
        rangeSpawn = 25f;
        CancelInvoke(nameof(SpawnEnemy));
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
                if (playerController && Vector3.Distance(randomPoint, playerController.gameObject.transform.position) < 6.1f)
                {
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
    public Vector3 GetRandomNavMeshPositionPlayer(float radius)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius + transform.position;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {

                randomPoint = hit.position;
                if (CheckReviveCondition(randomPoint, 10f))
                {
                    continue;
                }
                return hit.position;
            }
        }
        return transform.position;
    }
    public void SettingEnemyMaxCount(float num)
    {
        enemy_remain = num;
        enemy_not_spawn_num = enemy_remain;
    }
    public void Earnx3Gold()
    {
        coinManager.AddingCoinXn(2);
    }
}
