using UnityEngine;

[CreateAssetMenu(fileName = "SaveDayZombieMode", menuName = "ScriptableObject/ZombieMode/SaveDayZombieMode")]
public class SaveDayZombieMode : ScriptableObject
{
    public int current_day;
    public int[] num_enemy_day = { 40, 50, 60, 70, 80 };
}
