using System.Collections.Generic;
using UnityEngine;

public class CheckCoverObtacles : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask layerObticles;
    [SerializeField] private bool isZombieMap = false;
    private HashSet<TouchToObjectEnv> currentObstacles = new HashSet<TouchToObjectEnv>();

    private void Update()
    {
        if (!player) { return; }
        if (isZombieMap)
        {
            BaseMap();
        }
        else
        {
            BaseMap();
        }
    }

    private void BaseMap()
    {
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
    private void ZombieMap()
    {

        Vector3 direction = player.position - transform.position;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, Mathf.Infinity, layerObticles);

        HashSet<TouchToObjectEnv> newObstacles = new HashSet<TouchToObjectEnv>();

        float playerZ = player.position.z;
        float cameraZ = transform.position.z;

        // ?? V? ???ng ray t? camera ??n player
        Debug.DrawRay(transform.position, direction * 50f, Color.red);

        foreach (RaycastHit hit in hits)
        {
            TouchToObjectEnv touchObject = hit.transform.GetComponent<TouchToObjectEnv>();
            if (touchObject != null)
            {
                float objectZ = hit.transform.position.z;

                // ?? In thông tin ra Console ?? debug
                Debug.Log($"Camera Z: {cameraZ}, Player Z: {playerZ}, Object Z: {objectZ}, Object: {touchObject.name}");

                // N?u playerZ > cameraZ (h??ng camera nhìn v? Z d??ng)
                if (playerZ > cameraZ)
                {
                    if (objectZ > cameraZ && objectZ < playerZ)
                    {
                        Debug.DrawLine(transform.position, hit.point, Color.green); // V? ???ng t? camera ??n v?t th? b? làm m?
                        touchObject.TransparentObject();
                        newObstacles.Add(touchObject);
                    }
                }
                // N?u playerZ < cameraZ (h??ng camera nhìn v? Z âm)
                else
                {
                    if (objectZ < cameraZ && objectZ > playerZ)
                    {
                        Debug.DrawLine(transform.position, hit.point, Color.green);
                        touchObject.TransparentObject();
                        newObstacles.Add(touchObject);
                    }
                }
            }
        }

        // Khôi ph?c màu cho nh?ng v?t th? không còn b? che khu?t
        foreach (TouchToObjectEnv obj in currentObstacles)
        {
            if (!newObstacles.Contains(obj))
            {
                obj.ReturnColorObject();
            }
        }

        // C?p nh?t danh sách v?t th? b? làm m?
        currentObstacles = newObstacles;
    }
}
