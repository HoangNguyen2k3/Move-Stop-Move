using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterToOtherScene : MonoBehaviour
{
    [SerializeField] private int num_Secene = 1;
    public void EnterToScene()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        SceneManager.LoadScene(num_Secene);
    }
}
