using System.Linq;
using UnityEngine;

public class DeactivateLevel : MonoBehaviour
{
    [SerializeField] GameObject[] keepObjects;
    [SerializeField] GameObject[] removeObjects;
    void Start()
    {
        if (gameObject.transform.parent.gameObject.name != "Level 40")
            return;
        MeshRenderer[] mr = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr2 in mr)
            if (!keepObjects.Contains(mr2.gameObject))
                mr2.enabled = false;
        foreach (GameObject go in removeObjects)
            go.GetComponent<MeshRenderer>().enabled = false;
    }
}
