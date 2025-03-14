using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public string nameEnemyWin;
    private GameObject player;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private TextMeshProUGUI[] earnCoinlose;
    [SerializeField] private TextMeshProUGUI[] textNameEnemy;
    [SerializeField] private TextMeshProUGUI[] num_rank_lose_txt;
    [SerializeField] private TextMeshProUGUI[] name_player_txt;

    private bool iswin = false;
    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>().gameObject;
    }
    private void Update()
    {
        if (GameManager.Instance.name_enemy_win != "")
        {
            nameEnemyWin = GameManager.Instance.name_enemy_win;
        }
        if (player == null && gameOver.activeSelf == false && !iswin)
        {
            gameOver.SetActive(true);
            iswin = true;
            GetComponent<UIGeneratePress>().ShowAndHiddenGameObject();
            GameManager.Instance.islose = true;
            ProcessEndGame();
        }
    }

    public void ProcessEndGame()
    {
        for (int i = 0; i < textNameEnemy.Length; i++)
        {
            textNameEnemy[i].text = nameEnemyWin;
        }

        for (int i = 0; i < earnCoinlose.Length; i++)
        {
            earnCoinlose[i].text = GameManager.Instance.num_coin.ToString();
        }
        for (int i = 0; i < num_rank_lose_txt.Length; i++)
        {
            float temp = GameManager.Instance.enemy_remain;
            temp += 1;
            num_rank_lose_txt[i].text = "#" + temp.ToString();
        }
        for (int i = 0; i < name_player_txt.Length; i++)
        {
            name_player_txt[i].text = PlayerPrefs.GetString("NamePlayer", "YOU");
        }
    }

    public void RestartScene()
    {
        GameManager.Instance.gameObject.GetComponent<CoinManager>().AddingCoin();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
