using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseClother : MonoBehaviour
{
    [SerializeField] private Button[] button;
    [SerializeField] private GameObject[] activeButton;
    [SerializeField] private ClotherShop[] clothShop;
    [SerializeField] private PlayerController player;

    [SerializeField] private Button button_Purchase;
    [SerializeField] private Button exit_button;
    [SerializeField] private TextMeshProUGUI text_button;
    [SerializeField] private Image coinImage;

    [SerializeField] private CoinManager coinManager;
    private int current_index = 0;
    private void OnEnable()
    {
        ChooseType.OnChangeTypeClothes += ChangeTyeClother;
    }

    private void ChangeTyeClother(object sender, int e)
    {
        CheckButtonStatus(0);
        SetActiveCurrentClothes(0);
        SetTempPlayerSkin(0);
    }

    private void Start()
    {
        CheckButtonStatus(0);
        SetActiveCurrentClothes(0);
        SetTempPlayerSkin(0);
        for (int i = 0; i < button.Length; i++)
        {
            int index = i;
            button[i].onClick.AddListener(() =>
            {
                SetActiveCurrentClothes(index);
                SetTempPlayerSkin(index);
                CheckButtonStatus(index);
            });
        }
        button_Purchase.onClick.AddListener(() =>
        {
            PurchaseOrSelectWeapon();
        });
        exit_button.onClick.AddListener(() =>
        {
            ExitBtn();
        });
    }

    public void ExitBtn()
    {
        player.TakeInfoCloth();
    }
    public void PurchaseOrSelectWeapon()
    {
        if (clothShop[current_index].status == "NotPurchase")
        {
            if (coinManager.PurchaseSomething(clothShop[current_index].price))
            {
                SetRemainClothes();
                clothShop[current_index].status = "Selected";
                player.characterPlayer.skinClother[clothShop[current_index].clothType] = clothShop[current_index];
                player.TakeInfoCloth();
                CheckButtonStatus(current_index);
                SetClotherStatus();
            }
        }
        else if (clothShop[current_index].status == "Purchase")
        {
            SetRemainClothes();
            clothShop[current_index].status = "Selected";
            player.characterPlayer.skinClother[clothShop[current_index].clothType] = clothShop[current_index];
            player.TakeInfoCloth();
            CheckButtonStatus(current_index);
            SetClotherStatus();
        }
    }
    public void SetRemainClothes()
    {
        if (player.characterPlayer.skinClother[clothShop[current_index].clothType] != null)
            player.characterPlayer.skinClother[clothShop[current_index].clothType].status = "Purchase";
    }
    public void SetClotherStatus()
    {
        if (player.characterPlayer.skinClother[clothShop[current_index].clothType] != null)
            player.characterPlayer.skinClother[clothShop[current_index].clothType].status = "Selected";
    }
    public void SetActiveCurrentClothes(int index)
    {
        current_index = index;
        for (int i = 0; i < button.Length; i++)
        {
            if (i == index)
            {
                activeButton[i].SetActive(true);
            }
            else
            {
                activeButton[i].SetActive(false);
            }
        }
    }
    private void CheckButtonStatus(int index)
    {
        if (clothShop[index].status == "NotPurchase")
        {
            SetButtonStatus(true, clothShop[index].price.ToString(), true);
        }
        else if (clothShop[index].status == "Purchase")
        {
            SetButtonStatus(true, "SELECT", false);
        }
        else if (clothShop[index].status == "Selected")
        {
            SetButtonStatus(false, "SELECTED", false);
        }
    }
    private void SetButtonStatus(bool status_btn, string status, bool coin_status)
    {
        button_Purchase.interactable = status_btn;
        text_button.text = status;
        coinImage.gameObject.SetActive(coin_status);
    }
    public void SetTempPlayerSkin(int index)
    {
        ClotherShop skin = clothShop[index];
        player.SettingSkin(skin, skin.clothType);
    }
    private void OnDisable()
    {
        ChooseType.OnChangeTypeClothes -= ChangeTyeClother;

    }
}
