using UnityEngine;
using UnityEngine.UI;

public class ChooseType : MonoBehaviour
{
    [SerializeField] private Button[] button;
    [SerializeField] private GameObject[] list_item;

    private void Start()
    {
        SetActiveChooseWeaponType(0);
        for (int i = 0; i < button.Length; i++)
        {
            int index = i;
            button[i].onClick.AddListener(() =>
            {
                SetActiveChooseWeaponType(index);
            });
        }

    }
    public void SetActiveChooseWeaponType(int index)
    {
        for (int i = 0; i < button.Length; i++)
        {
            if (i == index)
            {
                button[i].interactable = false;
                list_item[i].SetActive(true);
            }
            else
            {
                button[i].interactable = true;
                list_item[i].SetActive(false);
            }
        }
    }
}
