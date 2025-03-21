using UnityEngine;

public class RandomZombieColor : MonoBehaviour
{
    [SerializeField] private Material[] materials;

    private void Awake()
    {
        int rand = Random.Range(0, materials.Length);
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = materials[rand];
        int rand_level = Random.Range(1, 6);
        float temp = 0.65f;
        switch (rand_level)
        {
            case 1: temp = 0.55f; break;
            case 2: temp = 0.60f; break;
            case 3: temp = 0.70f; break;
            case 4: temp = 0.75f; break;
        }

        gameObject.transform.localScale = new Vector3(temp, temp, temp);
    }
}
