using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuCamControl : MonoBehaviour
{
    public float mouseSensitivity;
    public float horizontalRotation;
    public float verticalRotation;
    public float horizontalRotationMax;
    public float verticalRotationMax;

    private void Start()
    {
        horizontalRotation = gameObject.transform.localEulerAngles.y;
        verticalRotation = gameObject.transform.localEulerAngles.x;
        horizontalRotationMax = gameObject.transform.localEulerAngles.y;
        verticalRotationMax = gameObject.transform.localEulerAngles.x;
    }

    private void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        horizontalRotation += mouseX;
        verticalRotation -= mouseY;

        horizontalRotation = Mathf.Clamp(horizontalRotation, horizontalRotationMax - 10f, horizontalRotationMax + 10f);
        verticalRotation = Mathf.Clamp(verticalRotation, verticalRotationMax - 10f, verticalRotationMax + 10f);

        gameObject.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }
}
