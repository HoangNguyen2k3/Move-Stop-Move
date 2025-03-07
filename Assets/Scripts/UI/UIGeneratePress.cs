using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGeneratePress : MonoBehaviour
{
    [SerializeField] private GameObject showGameObject;

    public void ShowGameObject()
    {
        showGameObject.SetActive(true);
    }
    public void HiddenGameObject()
    {
        showGameObject.SetActive(false);
    }
    public void ReturnToHome()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
