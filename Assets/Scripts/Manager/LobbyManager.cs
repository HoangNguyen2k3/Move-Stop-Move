using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{
    [SerializeField] private GameObject[] UIInGame;
    [SerializeField] private GameObject GameManager;
    [SerializeField] private GameObject UILobbyGame;
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private ScreenShotAndShow show;
    public bool currentinLobby = true;

    private PlayerController playerControl;
    private void Start()
    {
        playerControl = FindFirstObjectByType<PlayerController>();
        InLobby();
    }
    public void InLobby()
    {
        currentinLobby = true;
        cam.GetComponent<CinemachineCamera>().Priority = 1;
        if (playerControl.animator)
        {
            playerControl.animator.SetBool(ApplicationVariable.IDLE_PLAYER_STATE, true);
        }
        SetActiveFalseGameUI();
        UILobbyGame.SetActive(true);
    }
    public async void InGame()
    {
        show.TakePictureAndShow();
        await Task.Delay(50);
        currentinLobby = false;
        cam.GetComponent<CinemachineCamera>().Priority = -1;
        SetActiveTrueGameUI();
        UILobbyGame.SetActive(false);
    }
    public void SetActiveFalseGameUI()
    {
        for (int i = 0; i < UIInGame.Length; i++)
        {
            UIInGame[i].SetActive(false);
        }
    }
    public void SetActiveTrueGameUI()
    {
        for (int i = 0; i < UIInGame.Length; i++)
        {
            UIInGame[i].SetActive(true);
        }
    }
}
