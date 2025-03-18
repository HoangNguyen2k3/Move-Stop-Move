using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public void PauseCurrentGame()
    {
        Time.timeScale = 0f;
    }
    public void ContinueGame()
    {
        Time.timeScale = 1f;
    }
}
