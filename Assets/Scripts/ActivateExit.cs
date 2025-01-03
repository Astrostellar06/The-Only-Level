using UnityEngine;

public class ActivateExit : MonoBehaviour
{
    [SerializeField] private Exit exit;
    [SerializeField] private Animator button;
    [SerializeField] private IntVar level;
    private bool isOn = false;
    private float timeEnter;
    [SerializeField] private int numberOfSpikes;

    private void Start()
    {
        button.Play("StopPush");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOn = true;
            timeEnter = 0;
            if (level.value != 2 && level.value != 4 && level.value != 6 && level.value != 7 && level.value != 8 && level.value != 10 && level.value != 12 && level.value != 15 && level.value != 16 && level.value != 20 && level.value != 24 && level.value != 26 && level.value != 29 && level.value != 32 && level.value != 35 && level.value != 36 && level.value != 39)
                exit.Open();
            if (level.value != 6)
                button.Play("Push");

            if (level.value == 2)
                exit.Close();
            if (level.value == 4)
                Manager.instance.clickCount++;
            if (level.value == 4 && Manager.instance.clickCount == 5)
                exit.Open();
            if (gameObject.transform.parent.name == "Level 15" && level.value == 16)
                GameObject.Find("Level 16/Exit").GetComponent<Exit>().Open();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && level.value != 6)
        {
            button.Play("StopPush");
            isOn = false;
        }
    }

    private void OnMouseDown()
    {
        if (level.value == 6)
        {
            exit.Open();
            button.Play("Push");
        }
    }

    private void Update()
    {
        if (isOn && level.value == 12)
        {
            timeEnter += Time.deltaTime;
            if (timeEnter > 10)
                exit.Open();
        }
        if (level.value == 39 && Manager.instance.spikes == numberOfSpikes)
            exit.Open();
    }

    public void StopPush()
    {
        button.Play("StopPush");
    }
}
