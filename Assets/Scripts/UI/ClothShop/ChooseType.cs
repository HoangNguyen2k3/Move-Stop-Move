using System;
using UnityEngine;
using UnityEngine.UI;

public class ChooseType : MonoBehaviour
{
    [SerializeField] private Button[] button;
    [SerializeField] private GameObject[] list_item;
    public static EventHandler<int> OnChangeTypeClothes;

    [SerializeField] private GameObject buttonPurchase;
    [SerializeField] private GameObject buttonAds;
    private Vector3 begin;
    [SerializeField] private RectTransform center;
    private void OnEnable()
    {
        SetActiveChooseWeaponType(0);
    }
    private void Start()
    {
        begin = buttonPurchase.GetComponent<RectTransform>().anchoredPosition;
        SetActiveChooseWeaponType(0);
        for (int i = 0; i < button.Length; i++)
        {
            int index = i;
            button[i].onClick.AddListener(() =>
            {
                SettingButton(index);
                SetActiveChooseWeaponType(index);
                OnChangeTypeClothes?.Invoke(null, index);

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
    public void SettingButton(int index)
    {
        if (index == 3)
        {
            buttonPurchase.GetComponent<RectTransform>().anchoredPosition = center.anchoredPosition;
            buttonAds.SetActive(false);
        }
        else
        {
            buttonPurchase.GetComponent<RectTransform>().anchoredPosition = begin;
            buttonAds.SetActive(true);
        }
    }
}
