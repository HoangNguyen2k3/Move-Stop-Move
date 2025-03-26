using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinZombieMode : MonoBehaviour
{
    [SerializeField] private GameObject[] day_bar;
    [SerializeField] private TextMeshProUGUI day;
    [SerializeField] private SaveDayZombieMode saveDayZombieMode;
    [SerializeField] private Color done_color;
    [SerializeField] private Color notdone_color;
    [Header("Sound")]
    [SerializeField] private AudioClip sound_win;
    private void OnEnable()
    {
        SoundManager.Instance.PlaySFXSound(sound_win);
        day.text = "you survived day " + saveDayZombieMode.current_day.ToString() + "!";
        for (int i = 0; i < day_bar.Length; i++)
        {
            if (i < saveDayZombieMode.current_day)
            {
                day_bar[i].GetComponent<Image>().color = done_color;
            }
            else
            {
                day_bar[i].GetComponent<Image>().color = notdone_color;
            }
        }
    }

}
