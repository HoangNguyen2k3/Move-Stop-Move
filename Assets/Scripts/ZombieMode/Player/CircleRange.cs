using System.Collections.Generic;
using UnityEngine;

public class CircleRange : MonoBehaviour
{
    [HideInInspector] public Transform firstEnemy = null;
    [SerializeField] private GameObject circleTarget;
    private bool isEnemyInRange = false;
    public PlayerZombie player;
    public List<Transform> enemiesInRange = new List<Transform>();
    private void Update()
    {
        if (isEnemyInRange)
        {
            if (firstEnemy)
            {
                circleTarget.transform.position = firstEnemy.position;
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ApplicationVariable.ENEMY_TAG))
        {
            Transform enemy = other.transform;
            if (!enemiesInRange.Contains(enemy))
                enemiesInRange.Add(enemy);

            if (firstEnemy == null)
            {
                firstEnemy = enemy;
                UpdateCircleTarget();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ApplicationVariable.ENEMY_TAG))
        {
            Transform enemy = other.transform;
            enemiesInRange.Remove(enemy);

            if (enemy == firstEnemy)
            {
                firstEnemy = enemiesInRange.Count > 0 ? enemiesInRange[0] : null;
                UpdateCircleTarget();
            }
        }
    }

    public void UpdateCircleTarget()
    {
        if (firstEnemy != null)
        {
            if (firstEnemy.GetComponent<EnemiesHealth>().isAlive == false)
            {
                isEnemyInRange = false;
                circleTarget.SetActive(false);
                return;
            }
            circleTarget.SetActive(true);
            isEnemyInRange = true;
            circleTarget.transform.position = firstEnemy.position;
        }
        else
        {
            isEnemyInRange = false;
            circleTarget.SetActive(false);
        }
    }
    public void RemoveEnemyFromList(Transform enemy)
    {
        if (enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);

            if (enemy == firstEnemy)
            {
                firstEnemy = enemiesInRange.Count > 0 ? enemiesInRange[0] : null;
                UpdateCircleTarget();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(ApplicationVariable.ENEMY_TAG) && firstEnemy == other.transform)
        {
            player.AttackEnemy(other.gameObject);


        }
    }
    private void OnDestroy()
    {
        Destroy(circleTarget);
    }
}
