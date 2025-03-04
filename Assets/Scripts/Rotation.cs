using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private Joystick my_joyStick;
    [SerializeField] private float angle = 5f;
    private Vector3 direct;
    private void Update()
    {
        direct.x = my_joyStick.Horizontal;
        direct.z = my_joyStick.Vertical;
        RotateCharacter();
    }
    private void RotateCharacter()
    {
        if (direct == Vector3.zero)
            return;
        Quaternion rot = Quaternion.LookRotation(direct.normalized);
        transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rot, angle * Time.deltaTime);
    }
}
