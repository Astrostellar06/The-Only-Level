using UnityEngine;

public class SpikeDetect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Manager.instance.level.value != 11 && Manager.instance.level.value != 39)
            Manager.instance.Death();
        else if (Manager.instance.level.value == 11)
            Manager.instance.player.GetComponent<PlayerControl>().Jump();
        else if (Manager.instance.level.value == 39)
        {
            Manager.instance.spikes++;
            gameObject.SetActive(false);
        }
    }
}
