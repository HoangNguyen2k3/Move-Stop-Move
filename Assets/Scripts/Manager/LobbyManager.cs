using Unity.Cinemachine;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{
    [SerializeField] private GameObject[] UIInGame;
    [SerializeField] private GameObject GameManager;
    [SerializeField] private GameObject UILobbyGame;
    [SerializeField] private CinemachineCamera cam;
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
    public void InGame()
    {
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
