using System.Collections;
using UnityEngine;

public class LobbyAnimUI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject lobby;
    [SerializeField] private LobbyManager lobby_manage;
    public void EnableLobby()
    {
        lobby.SetActive(true);
        animator.Play("Lobby");
        //  StartCoroutine(WaitForAnimation());
    }

    public void DisableLobby()
    {
        StartCoroutine(DisableLobbyCoroutine());
    }

    private IEnumerator DisableLobbyCoroutine()
    {
        animator.Play("Lobby");
        //yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        yield return new WaitForSeconds(0.3f);
        lobby_manage.InGame();
        //     lobby.SetActive(false);
    }
}
