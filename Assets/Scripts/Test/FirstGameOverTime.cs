using UnityEngine;

public class FirstGameOverTime : MonoBehaviour
{
    [SerializeField] private GameObject[] item;

    private void OnEnable()
    {
        foreach (var item1 in item)
        {
            Destroy(item1);
        }
    }
}
