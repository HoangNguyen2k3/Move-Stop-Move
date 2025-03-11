using TMPro;
using UnityEngine;

public class ReviveGame : MonoBehaviour
{
    [SerializeField] private UIGeneratePress currentManager;
    [SerializeField] private TextMeshProUGUI time_text;
    public int time_wait = 5;
    public void takeASecond()
    {
        time_wait--;
        time_text.text = time_wait.ToString();
        if (time_wait == 0)
        {
            currentManager.ShowAndHiddenGameObject();
        }
    }
    private void OnEnable()
    {
        time_wait = 5;
        time_text.text = "5";
    }
}
