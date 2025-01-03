using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;

    void Update()
    {
        if (transform.position != cameraPosition.position)
        {

            transform.position = cameraPosition.position;
        }
    }
}
