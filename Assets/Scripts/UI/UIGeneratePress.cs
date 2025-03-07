using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGeneratePress : MonoBehaviour
{
    [SerializeField] private GameObject[] showGameObject;
    [SerializeField] private GameObject[] hiddenGameObject;
    public void ShowGameObject()
    {
        for (int i = 0; i < showGameObject.Length; i++)
        {
            showGameObject[i].SetActive(true);
        }
    }
    public void HiddenGameObject()
    {
        for (int i = 0; i < hiddenGameObject.Length; i++)
        {
            hiddenGameObject[i].SetActive(false);
        }
    }
    public void ShowAndHiddenGameObject()
    {
        for (int i = 0; i < showGameObject.Length; i++)
        {
            showGameObject[i].SetActive(true);
        }
        for (int i = 0; i < hiddenGameObject.Length; i++)
        {
            hiddenGameObject[i].SetActive(false);
        }
    }
    public void ReturnToHome()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
