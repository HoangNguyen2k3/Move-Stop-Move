using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRandomObj", menuName = "ScriptableObject/EnemyRandomObj")]
public class EnemyRandomObj : ScriptableObject
{
    public Material[] materials_body;
    public Material[] materials_pants;
    public GameObject[] weapon_hold;
    public GameObject[] weapon_throw;
    public GameObject[] hairs;
    public string[] nameEnemy;
}
