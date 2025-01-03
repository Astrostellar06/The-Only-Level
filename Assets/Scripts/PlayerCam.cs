using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float mouseSensitivity;

    public Transform orientation;
    [SerializeField] IntVar level;
    [SerializeField] Transform spin;
    [SerializeField] Transform normalRotation;

    private float verticalRotation = 0f;
    private float horizontalRotation = 90f;
    private float horizontalRotation2 = 90f;

    private void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        if (level.value == 33)
        {
            mouseX = (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0) + (Input.GetKey(KeyCode.RightArrow) ? 1 : 0);
            mouseY = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) + (Input.GetKey(KeyCode.DownArrow) ? -1 : 0);
        }
        horizontalRotation += (level.value == 5 || level.value == 9 || level.value == 18 ? -mouseX : mouseX);
        horizontalRotation2 += (level.value == 5 || level.value == 9 ? -mouseX : mouseX);
        verticalRotation -= (level.value == 5 || level.value == 9 || level.value == 18 ? -mouseY : mouseY);

        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        normalRotation.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, spin.transform.eulerAngles.z);

        if (level.value == 5)
            normalRotation.Rotate(Vector3.right, 180f, Space.World);


        if (level.value == 38)
        {
            normalRotation.Rotate(Vector3.right, 90f, Space.World);
            transform.localRotation = normalRotation.localRotation;
            orientation.localRotation = normalRotation.localRotation;
            return;
        }

        if (level.value == 35 && mouseSensitivity == 200)
            Manager.instance.currentExit.Open();
        else if (level.value == 35 && mouseSensitivity != 200)
            Manager.instance.currentExit.Close();

        transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, normalRotation.eulerAngles.z);
        orientation.localRotation = Quaternion.Euler(0f, horizontalRotation2, 0f);
    }

    public void Rotate()
    {
        horizontalRotation2 = 0f;
        horizontalRotation = 0f;
    }
}
