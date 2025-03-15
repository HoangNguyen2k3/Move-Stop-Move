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
    [SerializeField] private TextMeshProUGUI paramText;
    private int current_index = 0;
    public int num_page = 0;
    public int current_num_page;
    [SerializeField] private FullSkinObject[] fullSkinShop;
    private void OnEnable()
    {
        ChooseType.OnChangeTypeClothes += ChangeTyeClother;
    }

    private void ChangeTyeClother(object sender, int e)
    {
        num_page = e;
        CheckButtonStatus(0);
        SetActiveCurrentClothes(0);
        if (!CheckCurrentFullSkin())
        {
            SetTempPlayerSkin(0);
        }
        SetParamSkin(0);
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
                SetParamSkin(index);
            });
        }
        exit_button.onClick.AddListener(() =>
        {
            ExitBtn();
        });
    }
    public bool CheckCurrentFullSkin()
    {
        if (player.characterPlayer.fullSkinPlayer == null)
        {
            return false;
        }
        return true;
    }
    public void ExitBtn()
    {
        if (CheckCurrentFullSkin())
        {
            player.TakeInfoFullSkin();
        }
        else
        {
            player.TakeInfoCloth();
        }
    }
    public bool PurchaseOrSelectWeapon()
    {
        Debug.Log(num_page);
        if (num_page != 3 && CheckCurrentFullSkin())
        {
            return false;
        }
        if (num_page != 3 && !CheckCurrentFullSkin())
        {
            if (clothShop[current_index].status == "NotPurchase")
            {
                if (coinManager.PurchaseSomething(clothShop[current_index].price))
                {
                    SetRemainClothes("Purchase");
                    clothShop[current_index].status = "Selected";
                    player.characterPlayer.skinClother[clothShop[current_index].clothType] = clothShop[current_index];
                    player.TakeInfoCloth();
                    CheckButtonStatus(current_index);
                    SetClotherStatus("Selected");
                }
            }
            else if (clothShop[current_index].status == "Purchase")
            {
                SetRemainClothes("Purchase");
                clothShop[current_index].status = "Selected";
                player.characterPlayer.skinClother[clothShop[current_index].clothType] = clothShop[current_index];
                player.TakeInfoCloth();
                CheckButtonStatus(current_index);
                SetClotherStatus("Selected");
            }

        }
        else if (num_page == 3)
        {
            PurchaseOrSelectFullSkin();
        }
        return true;
    }
    public void PurchaseOrSelectFullSkin()
    {
        if (fullSkinShop[current_index].status == "NotPurchase")
        {
            if (coinManager.PurchaseSomething(fullSkinShop[current_index].price))
            {
                SetRemainClothes("Purchase");
                fullSkinShop[current_index].status = "Selected";
                player.characterPlayer.fullSkinPlayer = fullSkinShop[current_index];
                player.TakeInfoFullSkin();
                CheckButtonStatus(current_index);
                SetClotherStatus("Selected");
            }
        }
        else if (fullSkinShop[current_index].status == "Purchase")
        {
            SetRemainClothes("Purchase");
            fullSkinShop[current_index].status = "Selected";
            player.characterPlayer.fullSkinPlayer = fullSkinShop[current_index];
            player.TakeInfoFullSkin();
            CheckButtonStatus(current_index);
            SetClotherStatus("Selected");
        }
        else if (fullSkinShop[current_index].status == "Selected" && CheckCurrentFullSkin())
        {
            SetRemainClothes("Purchase");
            fullSkinShop[current_index].status = "Purchase";
            player.characterPlayer.fullSkinPlayer = null;
            player.TakeInfoCloth();
            CheckButtonStatus(current_index);
            SetRemainClothes("Purchase");
        }
    }
    public void SetRemainClothes(string _status)
    {
        if (num_page != 3)
        {
            if (player.characterPlayer.skinClother[clothShop[current_index].clothType] != null)
                player.characterPlayer.skinClother[clothShop[current_index].clothType].status = _status;
        }
        else
        {
            if (player.characterPlayer.fullSkinPlayer != null)
                player.characterPlayer.fullSkinPlayer.status = _status;
        }
    }
    public void SetClotherStatus(string _status)
    {
        if (num_page != 3)
        {
            if (player.characterPlayer.skinClother[clothShop[current_index].clothType] != null)
                player.characterPlayer.skinClother[clothShop[current_index].clothType].status = _status;
        }
        else
        {
            if (player.characterPlayer.fullSkinPlayer != null)
                player.characterPlayer.fullSkinPlayer.status = _status;
        }
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
        if (num_page != 3)
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
        else
        {
            CheckButtonStatusFullSkin(index);
        }
    }
    private void CheckButtonStatusFullSkin(int index)
    {
        string status = fullSkinShop[index].status;
        if (status == "NotPurchase")
        {
            SetButtonStatus(true, fullSkinShop[index].price.ToString(), true);
        }
        else if (status == "Purchase")
        {
            SetButtonStatus(true, "SELECT", false);
        }
        else if (status == "Selected")
        {
            SetButtonStatus(true, "UNSELECT", false);
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
        if (num_page != 3)
        {
            ClotherShop skin = clothShop[index];
            player.SettingSkin(skin, skin.clothType);
        }
        else
        {
            SetTempPlayerFullSkin(index);
        }
    }
    public void SetParamSkin(int index)
    {
        if (num_page != 3)
        {
            ClotherShop skin = clothShop[index];
            paramText.text = skin.paramCloth;
        }
        else
        {
            SetParamFullSkin(index);
        }
    }
    public void SetTempPlayerFullSkin(int index)
    {
        FullSkinObject skin = fullSkinShop[index];
        player.SettingFullSkin(skin);
    }
    public void SetParamFullSkin(int index)
    {
        FullSkinObject skin = fullSkinShop[index];
        paramText.text = skin.param;
    }
    private void OnDisable()
    {
        ChooseType.OnChangeTypeClothes -= ChangeTyeClother;
    }
}
