using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject gameOver;
    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>().gameObject;
    }
    private void Update()
    {
        if (player == null && gameOver.activeSelf == false)
        {
            gameOver.SetActive(true);
        }
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
