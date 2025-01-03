using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonReset : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ResetState);
    }

    private void ResetState()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
