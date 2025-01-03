using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private GameObject exitSolid;
    public void Open()
    {
        gameObject.GetComponent<Animator>().Play("OpenDoor");
        exitSolid.SetActive(false);
    }

    public void Close()
    {
        gameObject.GetComponent<Animator>().Play("CloseDoor");
        exitSolid.SetActive(true);
    }
}
