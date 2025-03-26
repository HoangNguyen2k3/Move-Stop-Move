using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource music;
    [SerializeField] public AudioSource SFXSound;
    //[SerializeField] private AudioClip BG_music;
    [Header("-------------------Sound List----------------------")]
    public AudioClip win_sound;
    public AudioClip lose_sound;
    public AudioClip level_up;
    public AudioClip throwWeapon;
    public AudioClip hit_something;
    public AudioClip dead;
    public AudioClip button_click;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(Instance.gameObject);
    }
    private void Start()
    {
        //PlayMusic(BG_music);
    }
    public void PlayMusic(AudioClip clip)
    {
        music.PlayOneShot(clip);
    }
    public void PlaySFXSound(AudioClip clip)
    {
        SFXSound.PlayOneShot(clip);
    }
}
