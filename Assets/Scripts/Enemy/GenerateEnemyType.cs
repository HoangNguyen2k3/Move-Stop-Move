using UnityEngine;

public class GenerateEnemyType : MonoBehaviour
{
    [SerializeField] private Material[] materials_body;
    [SerializeField] private Material[] materials_pants;
    [SerializeField] private GameObject[] weapon_hold;
    [SerializeField] private GameObject[] weapon_throw;
    [SerializeField] private GameObject[] hairs;

    [SerializeField] private SkinnedMeshRenderer skin;
    [SerializeField] private SkinnedMeshRenderer pant;
    [SerializeField] private GameObject weapon_start_hold;
    //    [SerializeField] private GameObject weapon_start_throw;
    [SerializeField] private GameObject hair;

    private EnemyAI enemyAI;
    public int random_level;
    private LevelManager levelManager;

    private void Awake()
    {
        levelManager = GetComponent<LevelManager>();
        enemyAI = GetComponent<EnemyAI>();
        random_level = Random.Range(0, 10);
        levelManager.startLevel = random_level;
        skin.material = materials_body[Random.Range(0, materials_body.Length)];
        pant.material = materials_pants[Random.Range(0, materials_pants.Length)];

        int index = Random.Range(0, weapon_throw.Length);
        enemyAI.weaponThrow = weapon_throw[index];
        //       weapon_start_throw = enemyAI.weaponThrow;

        GameObject weapon_h = weapon_hold[index];
        if (weapon_start_hold != null)
        {
            MeshFilter weaponMeshFilter = weapon_start_hold.GetComponent<MeshFilter>();
            MeshRenderer weaponMeshRenderer = weapon_start_hold.GetComponent<MeshRenderer>();

            if (weaponMeshFilter != null && weaponMeshRenderer != null)
            {
                weaponMeshFilter.mesh = weapon_h.GetComponent<MeshFilter>().sharedMesh;
                weaponMeshRenderer.materials = weapon_h.GetComponent<MeshRenderer>().sharedMaterials;
            }
        }

        GameObject hair_h = hairs[Random.Range(0, hairs.Length)];
        if (hair != null && hair_h != null)
        {
            MeshFilter hairFilter = hair.GetComponent<MeshFilter>();
            MeshRenderer hairRenderer = hair.GetComponent<MeshRenderer>();

            if (hairFilter != null && hairRenderer != null)
            {
                hairFilter.mesh = hair_h.GetComponent<MeshFilter>().sharedMesh;
                hairRenderer.materials = hair_h.GetComponent<MeshRenderer>().sharedMaterials;
            }
        }
    }
    private void Start()
    {
    }


}
