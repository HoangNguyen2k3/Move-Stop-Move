using UnityEngine;

public class CheckStatusGame : MonoBehaviour
{
    void Update()
    {
        if (GameManager.Instance.iswinning || GameManager.Instance.islose)
        {
            Destroy(gameObject);
        }
    }
}
