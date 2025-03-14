using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenerateEnemyType : MonoBehaviour
{
    [SerializeField] private EnemyRandomObj enemyRandomObj;

    [SerializeField] private SkinnedMeshRenderer skin;
    [SerializeField] private SkinnedMeshRenderer pant;
    [SerializeField] private GameObject weapon_start_hold;
    [SerializeField] private GameObject hair;
    [SerializeField] public TextMeshProUGUI nameEnemy;
    [SerializeField] public Image image;
    private EnemyAI enemyAI;
    public int random_level;
    private LevelManager levelManager;

    private void Awake()
    {
        levelManager = GetComponent<LevelManager>();
        enemyAI = GetComponent<EnemyAI>();
        random_level = Random.Range(0, 10);
        levelManager.startLevel = random_level;
        skin.material = enemyRandomObj.materials_body[Random.Range(0, enemyRandomObj.materials_body.Length)];
        pant.material = enemyRandomObj.materials_pants[Random.Range(0, enemyRandomObj.materials_pants.Length)];

        int index = Random.Range(0, enemyRandomObj.weapon_throw.Length);
        enemyAI.weaponThrow = enemyRandomObj.weapon_throw[index];
        GameObject weapon_h = enemyRandomObj.weapon_hold[index];
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

        GameObject hair_h = enemyRandomObj.hairs[Random.Range(0, enemyRandomObj.hairs.Length)];
        /*        if (hair != null && hair_h != null)
                {
                    MeshFilter hairFilter = hair.GetComponent<MeshFilter>();
                    MeshRenderer hairRenderer = hair.GetComponent<MeshRenderer>();

                    if (hairFilter != null && hairRenderer != null)
                    {
                        hairFilter.mesh = hair_h.GetComponent<MeshFilter>().sharedMesh;
                        hairRenderer.materials = hair_h.GetComponent<MeshRenderer>().sharedMaterials;
                    }
                }*/
        SetSkinEnemy(hair_h);
        nameEnemy.text = enemyRandomObj.nameEnemy[Random.Range(0, enemyRandomObj.nameEnemy.Length)];
        nameEnemy.GetComponent<TextMeshProUGUI>().color = skin.material.color;
        image.GetComponent<Image>().color = skin.material.color;
    }
    public void SetSkinEnemy(GameObject hair_new)
    {
        if (hair != null && hair_new != null)
        {
            foreach (Transform item in hair.transform)
            {
                Destroy(item.gameObject);
            }
            Instantiate(hair_new, hair.transform);
        }
    }
}
