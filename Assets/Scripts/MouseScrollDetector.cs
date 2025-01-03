using Unity.VisualScripting;
using UnityEngine;

public class MouseScrollDetector : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    public Vector2 scaleRange = new Vector2(0.1f, 2f);
    public Vector2 positionRange = new Vector2(2.5f, 3.7f);

    private void Start()
    {
        gameObject.GetComponent<Animator>().enabled = false;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            float newScale = Mathf.Clamp(gameObject.transform.localScale.y - scroll * scrollSpeed, scaleRange.x, scaleRange.y);
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, newScale, gameObject.transform.localScale.z);

            float scaleProgress = Mathf.InverseLerp(scaleRange.x, scaleRange.y, newScale);
            float newPosition = Mathf.Lerp(positionRange.x, positionRange.y, scaleProgress);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, newPosition, gameObject.transform.position.z);
        }
    }
}