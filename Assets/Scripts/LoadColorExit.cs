using UnityEngine;

public class LoadColorExit : MonoBehaviour
{
    void Start()
    {
        int level = int.Parse(gameObject.transform.parent.name.Replace("Level", ""));
        Color color = Color.HSVToRGB(1f / 40 * ((level - 1) % 40), 1f, 1f);
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);
    }

}
