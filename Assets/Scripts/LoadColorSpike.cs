using UnityEngine;

public class LoadColorSpike : MonoBehaviour
{
    int level;
    int currentColor;
    void Start()
    {
        level = int.Parse(gameObject.transform.parent.transform.parent.name.Replace("Level", ""));
        Color color = Color.HSVToRGB(1f / 40 * ((level - 1) % 40), 1f, 1f);
        gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", color);
        currentColor = Random.Range(0, 360);
    }

    private void Update()
    {
        if (level != 34)
            return;
        Color color = Color.HSVToRGB(currentColor / 360f, 1f, 1f);
        currentColor++;
        if (currentColor >= 360)
            currentColor = 0;
        gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", color);
    }
}
