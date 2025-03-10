using Unity.Cinemachine;
using UnityEngine;

public class ChangeCamAndAnimPlayer : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cam;
    private PlayerController playerController;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }
    public void ChangeCamPriority()
    {
        playerController.animator.SetBool("IsDance", true);
        cam.GetComponent<CinemachineCamera>().Priority = 10;
    }
    public void ReturnCamPriority()
    {
        playerController.animator.SetBool("IsDance", false);
        cam.GetComponent<CinemachineCamera>().Priority = -1;
    }
}
