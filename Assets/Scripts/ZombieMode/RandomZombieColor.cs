using UnityEngine;

public class RandomZombieColor : MonoBehaviour
{
    [SerializeField] private Material[] materials;

    private void Awake()
    {
        int rand = Random.Range(0, materials.Length);
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = materials[rand];
        int rand_level = Random.Range(1, 6);
        float temp = 1f; ;
        switch (rand_level)
        {
            case 1: temp = 0.75f; break;
            case 2: temp = 0.85f; break;
            case 3: temp = 1.15f; break;
            case 4: temp = 1.25f; break;
        }

        gameObject.transform.localScale = new Vector3(temp, temp, temp);
    }
}
