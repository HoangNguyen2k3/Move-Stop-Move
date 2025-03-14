using UnityEngine;
using UnityEngine.UI;

public class TableColor : MonoBehaviour
{
    public GameObject[] currentPart;
    private int current_num_choose = 0;
    public string saveName;

    private void Awake()
    {
        ComponentOptColor.OnChangePart += ChangCurrentNum;
    }

    private void TakeColor(object sender, WeaponShop e)
    {
        saveName = e.nameWeapon;
        Debug.Log(saveName + "normal");
    }

    /*    public void TakeColor(WeaponShop e)
        {
            saveName = e.nameWeapon;
            Debug.Log(saveName + "normal");
        }*/

    private void ChangCurrentNum(object sender, int e)
    {
        current_num_choose = e;
    }
    public void TakeColorForPart()
    {
        Color selectedColor = gameObject.GetComponent<Image>().color;
        currentPart[current_num_choose].GetComponent<ColorComponent>().ChangeColor(selectedColor);
        string hexColor = "#" + ColorUtility.ToHtmlStringRGB(selectedColor);
        PlayerPrefs.SetString("Color_" + PurchaseCustomWeapon.lastWeaponShop.nameWeapon + "_custom_" + current_num_choose.ToString(), hexColor);
        PlayerPrefs.Save();
    }
}

