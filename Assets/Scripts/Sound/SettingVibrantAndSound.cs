using UnityEngine;

public class SettingVibrantAndSound : MonoBehaviour
{
    [SerializeField] private GameObject vibrant;
    [SerializeField] private GameObject unvibrant;
    [SerializeField] private GameObject sound;
    [SerializeField] private GameObject unsound;

    private void Start()
    {
        CheckStatus();
    }
    public void CheckStatus()
    {
        if (PlayerPrefs.GetInt("Vibrant", 0) == 0)
        {
            vibrant.SetActive(false);
            unvibrant.SetActive(true);
            //            Handheld.();
        }
        else
        {
            vibrant.SetActive(true);
            unvibrant.SetActive(false);
            //            Handheld.Vibrate();
        }
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            SoundManager.Instance.SFXSound.mute = false;
            sound.SetActive(true);
            unsound.SetActive(false);
        }
        else
        {
            SoundManager.Instance.SFXSound.mute = true;
            sound.SetActive(false);
            unsound.SetActive(true);
        }
    }
    public void ClickToVibrant()
    {
        vibrant.SetActive(true);
        unvibrant.SetActive(false);
        PlayerPrefs.SetInt("Vibrant", 1);
    }
    public void ClickToUnVibrant()
    {
        vibrant.SetActive(false);
        unvibrant.SetActive(true);
        PlayerPrefs.SetInt("Vibrant", 0);
    }
    public void OpenSound()
    {
        SoundManager.Instance.SFXSound.mute = false;
        sound.SetActive(true);
        unsound.SetActive(false);
        PlayerPrefs.SetInt("Sound", 1);
    }
    public void UnSound()
    {
        SoundManager.Instance.SFXSound.mute = true;
        sound.SetActive(false);
        unsound.SetActive(true);
        PlayerPrefs.SetInt("Sound", 0);
    }



}
