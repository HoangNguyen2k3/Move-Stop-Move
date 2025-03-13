using UnityEngine;
using UnityEngine.UI;

public class ChooseClother : MonoBehaviour
{
    [SerializeField] private Button[] button;
    [SerializeField] private GameObject[] activeButton;
    [SerializeField] private ClotherShop[] clothShop;
    [SerializeField] private PlayerController player;

    private void Start()
    {
        SetActiveChooseClothType(0);
        for (int i = 0; i < button.Length; i++)
        {
            int index = i;
            button[i].onClick.AddListener(() =>
            {
                SetActiveChooseClothType(index);
                // SetPlayerSkin(index);
                SetTempPlayerSkin(index);
            });
        }
    }
    public void SetActiveChooseClothType(int index)
    {
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
    public void SetPlayerSkin(int index)
    {
        ClotherShop skin = clothShop[index];
    }
    public void SetTempPlayerSkin(int index)
    {
        ClotherShop skin = clothShop[index];
        player.SettingSkin(skin, skin.clothType);
    }
}
