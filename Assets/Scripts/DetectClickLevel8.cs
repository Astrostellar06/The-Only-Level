using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectClickLevel8 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (Manager.instance.level.value == 8)
        {
            Manager.instance.currentExit.Open();
        }
    }
}
