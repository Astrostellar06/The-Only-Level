using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensibilityChange : MonoBehaviour
{
    [SerializeField] PlayerCam cam;
    public void ChangeSensibility(float value)
    {
        cam.mouseSensitivity = value;
    }
}
