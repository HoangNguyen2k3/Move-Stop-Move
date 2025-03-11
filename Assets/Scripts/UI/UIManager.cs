using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public string nameEnemyWin;
    private GameObject player;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private TextMeshProUGUI earnCoinlose;
    [SerializeField] private TextMeshProUGUI textNameEnemy;
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
            textNameEnemy.text = nameEnemyWin;
            GameManager.Instance.islose = true;
            earnCoinlose.text = GameManager.Instance.num_coin.ToString();
        }
    }
    public void RestartScene()
    {
        GameManager.Instance.gameObject.GetComponent<CoinManager>().AddingCoin();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
