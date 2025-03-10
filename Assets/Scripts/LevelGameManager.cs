using TMPro;
using UnityEngine;

public class LevelGameManager : MonoBehaviour
{
    public float enemy_level_easy = 20;
    public float enemy_level_medium = 35;
    public float enemy_level_hard = 50;
    [SerializeField] private TextMeshProUGUI[] text;
    [SerializeField] private TextMeshProUGUI num_enemy;
    string temp = "CURRENT LEVEL: ";

    private void Start()
    {
        if (!PlayerPrefs.HasKey("LevelGame"))
        {
            PlayerPrefs.SetString("LevelGame", "EASY");
            ChangeGameLevel_Easy();
        }
        else
        {
            switch (PlayerPrefs.GetString("LevelGame"))
            {
                case "EASY":
                    ChangeGameLevel_Easy();
                    break;
                case "MEDIUM":
                    ChangeGameLevel_Medium();
                    break;
                case "HARD":
                    ChangeGameLevel_Hard();
                    break;
            }
        }
    }
    public void ChangeGameLevel_Medium()
    {
        GameManager.Instance.SettingEnemyMaxCount(enemy_level_medium);
        ChangeTextLevel(temp + "MEDIUM");
        PlayerPrefs.SetString("LevelGame", "MEDIUM");
        PlayerPrefs.SetFloat("num_enemy_level", 35);
        num_enemy.text = "ALIVE: " + enemy_level_medium.ToString();
    }
    public void ChangeGameLevel_Easy()
    {
        GameManager.Instance.SettingEnemyMaxCount(enemy_level_easy);
        ChangeTextLevel(temp + "EASY");
        PlayerPrefs.SetString("LevelGame", "EASY");
        PlayerPrefs.SetFloat("num_enemy_level", 20);
        num_enemy.text = "ALIVE: " + enemy_level_easy.ToString();
    }
    public void ChangeGameLevel_Hard()
    {
        GameManager.Instance.SettingEnemyMaxCount(enemy_level_hard);
        ChangeTextLevel(temp + "HARD");
        PlayerPrefs.SetString("LevelGame", "HARD");
        PlayerPrefs.SetFloat("num_enemy_level", 50);
        num_enemy.text = "ALIVE: " + enemy_level_hard.ToString();
    }
    public void ChangeTextLevel(string current_level)
    {
        foreach (TextMeshProUGUI text in text)
        {
            text.text = current_level;
        }
    }
}
