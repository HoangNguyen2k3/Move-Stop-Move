using UnityEngine;
using UnityEngine.UI;

public class TableColor : MonoBehaviour
{
    public GameObject[] currentPart;
    private int current_num_choose = 0;
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
        currentPart[current_num_choose].GetComponent<ColorComponent>().ChangeColor(gameObject.GetComponent<Image>().color);

    }
    private void OnDisable()
    {
        ComponentOptColor.OnChangePart -= ChangCurrentNum;
    }
}
