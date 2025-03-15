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
    [SerializeField] private ChooseClother[] chooseClother;
    [SerializeField] private Button button_Purchase;
    private int num;

    [SerializeField] private GameObject textWarning;
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
        button_Purchase.onClick.AddListener(() =>
        {
            PurchaseOrSelectWeapon();
        });

    }
    public void PurchaseOrSelectWeapon()
    {
        chooseClother[num].num_page = num;
        if (!chooseClother[num].PurchaseOrSelectWeapon())
        {
            Instantiate(textWarning, gameObject.transform.parent);
            Debug.Log("hehe");
        }
        else
        {
            Debug.Log("haha");
        }
        //chooseClother[num].PurchaseOrSelectWeapon();
    }
    public void SetActiveChooseWeaponType(int index)
    {
        num = index;
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
