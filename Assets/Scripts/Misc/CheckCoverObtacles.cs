using System.Collections.Generic;
using UnityEngine;

public class CheckCoverObtacles : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask layerObticles;

    private HashSet<TouchToObjectEnv> currentObstacles = new HashSet<TouchToObjectEnv>();

    private void Update()
    {
        if (!player) { return; }

        Vector3 direction = player.position - transform.position;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, Mathf.Infinity, layerObticles);

        HashSet<TouchToObjectEnv> newObstacles = new HashSet<TouchToObjectEnv>();

        foreach (RaycastHit hit in hits)
        {
            TouchToObjectEnv touchObject = hit.transform.GetComponent<TouchToObjectEnv>();
            if (touchObject != null)
            {
                touchObject.TransparentObject();
                newObstacles.Add(touchObject);
            }
        }

        foreach (TouchToObjectEnv obj in currentObstacles)
        {
            if (!newObstacles.Contains(obj))
            {
                obj.ReturnColorObject();
            }
        }

        currentObstacles = newObstacles;
    }
}
