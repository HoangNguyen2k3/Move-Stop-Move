using UnityEngine;

public class CheckDistance : MonoBehaviour
{
    public bool isTouchPlayer = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ApplicationVariable.PLAYER_TAG))
        {
            isTouchPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ApplicationVariable.PLAYER_TAG))
        {
            isTouchPlayer = false;
        }
    }
}
