using UnityEngine;

public class QuanticDoor : MonoBehaviour
{
    bool open = false;
    void Update()
    {
        if (IsObjectVisible(gameObject))
        {
            if (open)
                Manager.instance.currentExit.Close();
            open = false;
        }
        else
        {
            if (!open)
                Manager.instance.currentExit.Open();
            open = true;
        }
    }

    bool IsObjectVisible(GameObject obj)
    {
        Collider collider = obj.GetComponent<Collider>();
        if (collider == null) return false;

        Vector3[] objectCorners = new Vector3[8];
        objectCorners[0] = collider.bounds.min;
        objectCorners[1] = new Vector3(collider.bounds.min.x, collider.bounds.min.y, collider.bounds.max.z);
        objectCorners[2] = new Vector3(collider.bounds.min.x, collider.bounds.max.y, collider.bounds.min.z);
        objectCorners[3] = new Vector3(collider.bounds.max.x, collider.bounds.min.y, collider.bounds.min.z);
        objectCorners[4] = collider.bounds.max;
        objectCorners[5] = new Vector3(collider.bounds.max.x, collider.bounds.max.y, collider.bounds.min.z);
        objectCorners[6] = new Vector3(collider.bounds.min.x, collider.bounds.max.y, collider.bounds.max.z);
        objectCorners[7] = new Vector3(collider.bounds.max.x, collider.bounds.min.y, collider.bounds.max.z);

        foreach (Vector3 corner in objectCorners)
        {
            Vector3 screenPos = Manager.instance.mainCamera.WorldToViewportPoint(corner);
            if (screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1 && screenPos.z > 0)
                return true;
        }

        return false;
    }
}
