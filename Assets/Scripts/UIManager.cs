using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public string nameEnemyWin;
    private GameObject player;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private TextMeshProUGUI textNameEnemy;
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
        if (player == null && gameOver.activeSelf == false)
        {
            gameOver.SetActive(true);
            textNameEnemy.text = nameEnemyWin;
        }
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
