using UnityEngine;

public class CheckStatusGame : MonoBehaviour
{
    private LobbyManager lobby;
    [SerializeField] private GameObject child;
    private void Start()
    {
        lobby = FindFirstObjectByType<LobbyManager>();
    }
    void Update()
    {
        if (GameManager.Instance.iswinning || GameManager.Instance.islose)
        {
            Destroy(gameObject);
        }
        if (lobby.currentinLobby && child.activeSelf == true)
        {
            child.SetActive(false);
        }
        /*        else if (!lobby.currentinLobby && child.activeSelf == false)
                {
                    child.SetActive(true);
                }*/

    }
}
