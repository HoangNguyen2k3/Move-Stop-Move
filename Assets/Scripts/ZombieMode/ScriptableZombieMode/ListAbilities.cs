using UnityEngine;

[CreateAssetMenu(fileName = "ListAbilities", menuName = "ScriptableObject/ZombieMode/ListAbilities")]
public class ListAbilities : ScriptableObject
{
    public Sprite[] spriteAbilities;
    public string[] list_name;
    public int current_rand_1;
    public int current_rand_2;

    public void SetupAbilitiesInStartGame()
    {
        current_rand_1 = Random.Range(0, spriteAbilities.Length);
        while (true)
        {
            current_rand_2 = Random.Range(0, spriteAbilities.Length);
            if (current_rand_1 != current_rand_2)
            {
                break;
            }
        }
        /*        ability1.sprite = spriteAbilities[current_rand_1];
                ability2.sprite = spriteAbilities[current_rand_2];*/
    }

}
