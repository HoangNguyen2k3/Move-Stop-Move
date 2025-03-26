using UnityEngine;

public class CheckStatusIndicator : MonoBehaviour
{
    private LobbyManager lobby;
    [SerializeField] private GameObject child;

    public bool inZombieMode = false;
    private void Start()
    {
        if (!inZombieMode)
            lobby = FindFirstObjectByType<LobbyManager>();
    }
    void Update()
    {
        if (!inZombieMode)
        {
            if (GameManager.Instance.iswinning || GameManager.Instance.islose)
            {
                Destroy(gameObject);
            }
            if (lobby.currentinLobby && child.activeSelf == true)
            {
                child.SetActive(false);
            }

        }
        else
        {
            if (ZombieManager.Instance.currentInLobbyZombie)
            {
                child.SetActive(false);
            }
            else
            {
                child.SetActive(true);
            }
        }

    }
}
