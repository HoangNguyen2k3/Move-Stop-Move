using UnityEngine;

[CreateAssetMenu(fileName = "FullSkinObject", menuName = "ScriptableObject/FullSkinObject")]
public class FullSkinObject : ScriptableObject
{
    public Material skin;
    public GameObject accessories;
    public GameObject head;
    public GameObject weaponSkin;
    public GameObject tail;
    public float price;
    public string status;
    public string param;
}
