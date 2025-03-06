using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemy_alive;
    [SerializeField] private GameObject winningGame;
    [SerializeField] private GameObject[] enemy;

    [SerializeField] private float enemy_spawn_pertime;
    [SerializeField] private float enemy_remain; //Tong quai can danh hien tai ke ca chua spawn
    private float enemy_not_spawn_num;


    private bool iswinning = false;

    private string enemy_text = "ALIVE: ";
    private Vector3 randomPoint;
    private void Start()
    {
        enemy_not_spawn_num = enemy_remain;
        enemy_alive.text = quickAddText(enemy_remain);
        InvokeRepeating(nameof(SpawnEnemy), 0, 2.5f);
    }
    private void Update()
    {
        if (enemy_remain <= 0 && !iswinning)
        {
            iswinning = true;
            enemy_alive.text = quickAddText(0);
            winningGame.SetActive(true);
        }
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
        //        Debug.Log(enemy_spawn_pertime + "+" + enemy_remain + "+" + enemy_not_spawn_num);
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
        for (int i = 0; i < enemy_spawn_pertime; i++)
        {
            int random_enemy = Random.Range(0, enemy.Length);
            Instantiate(enemy[random_enemy], GetRandomNavMeshPosition(transform.position, 100f), Quaternion.identity);
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
                return hit.position;
            }
        }
        return origin;
    }
}
