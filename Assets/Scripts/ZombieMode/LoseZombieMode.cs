using UnityEngine;
using UnityEngine.UI;

public class LoseZombieMode : MonoBehaviour
{
    [SerializeField] private GameObject[] day_bar;
    [SerializeField] private GameObject icon;
    [SerializeField] private SaveDayZombieMode saveDayZombieMode;
    [SerializeField] private Color done_color;
    [SerializeField] private Color notdone_color;
    [SerializeField] private Color current_color;

    private void OnEnable()
    {
        if (SoundManager.Instance)
            SoundManager.Instance.PlaySFXSound(SoundManager.Instance.lose_sound);
        icon.transform.position = day_bar[saveDayZombieMode.current_day - 1].transform.position;
        for (int i = 0; i < day_bar.Length; i++)
        {
            if (i == (saveDayZombieMode.current_day - 1))
            {
                day_bar[i].GetComponent<Image>().color = current_color;
            }
            else if (i < (saveDayZombieMode.current_day - 1))
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
