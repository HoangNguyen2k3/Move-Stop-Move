using UnityEngine;
using UnityEngine.UI;

public class TableColor : MonoBehaviour
{
    public GameObject[] currentPart;
    private int current_num_choose = 0;
    [SerializeField] private bool isHammer = true;

    private string hammer_save = "Color_Hammer_";
    private string candy_save = "Color_Candy_";
    private void OnEnable()
    {
        ComponentOptColor.OnChangePart += ChangCurrentNum;
    }

    private void ChangCurrentNum(object sender, int e)
    {
        current_num_choose = e;
    }
    public void TakeColorForPart()
    {

        Color selectedColor = gameObject.GetComponent<Image>().color;
        currentPart[current_num_choose].GetComponent<ColorComponent>().ChangeColor(selectedColor);
        string hexColor = "#" + ColorUtility.ToHtmlStringRGB(selectedColor);
        Debug.Log(hexColor);
        if (isHammer)
        {
            PlayerPrefs.SetString(hammer_save + current_num_choose.ToString(), hexColor);
        }
        else
        {
            PlayerPrefs.SetString(candy_save + current_num_choose.ToString(), hexColor);
        }
        PlayerPrefs.Save();
    }
    private void OnDisable()
    {
        ComponentOptColor.OnChangePart -= ChangCurrentNum;
    }
}

