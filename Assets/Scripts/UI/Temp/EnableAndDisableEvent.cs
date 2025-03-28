using UnityEngine;

public class EnableAndDisableEvent : MonoBehaviour
{
    [SerializeField] private bool isZombieMap;

    private void OnEnable()
    {
        if (isZombieMap)
        {
            ZombieManager.Instance.currentInLobbyZombie = true;
        }
        else
        {
            LobbyManager.Instance.setOffMoveEnemy_indicator = true;
        }

    }
    private void OnDisable()
    {
        if (isZombieMap)
        {
            ZombieManager.Instance.currentInLobbyZombie = false;
        }
        else
        {
            LobbyManager.Instance.setOffMoveEnemy_indicator = false;
        }
    }
}
