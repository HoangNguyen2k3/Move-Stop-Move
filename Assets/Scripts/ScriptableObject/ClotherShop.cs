using UnityEngine;

[CreateAssetMenu(fileName = "ClotherShop", menuName = "ScriptableObject/ClotherShop")]
public class ClotherShop : ScriptableObject
{
    public string nameClothShop;
    public GameObject skin;
    public string paramCloth;
    public string status;
    public float price;
    public int clothType;
}
