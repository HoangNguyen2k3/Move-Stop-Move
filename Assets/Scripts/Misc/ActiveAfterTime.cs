using System.Threading.Tasks;
using UnityEngine;

public class ActiveAfterTime : MonoBehaviour
{
    [SerializeField] private float timeDelay = 1.0f;
    [SerializeField] private GameObject[] item;
    private void Start()
    {
        SetActiveItem();
    }

    private async void SetActiveItem()
    {
        await Task.Delay((int)timeDelay * 1000);
        for (int i = 0; i < item.Length; i++)
        {
            if (item[i] != null)
                item[i].SetActive(true);
        }

    }
}
