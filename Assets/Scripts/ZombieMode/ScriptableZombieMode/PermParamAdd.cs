using UnityEngine;

[CreateAssetMenu(fileName = "PermParamAdd", menuName = "ScriptableObject/ZombieMode/PermParamAdd")]
public class PermParamAdd : ScriptableObject
{
    public int num_add_shield;
    public float num_add_speed;
    public float num_add_range;
    public float num_max_throw;

    public int max_shield = 4;
    public float max_speed = 100f;
    public float max_range = 100f;
    public float max_throw = 10f;

    public float price_current_shield = 1000f;
    public float price_current_speed = 250f;
    public float price_current_range = 250f;
    public float price_current_throw = 500f;
}
