using Unity.VisualScripting;
using UnityEngine;

public class DetectExit : MonoBehaviour
{
    private bool hasExited = false;
    [SerializeField] private IntVar level;

    [SerializeField] private Exit door;
    [SerializeField] private PhysicMaterial normalPhysics;
    [SerializeField] private PhysicMaterial bouncyPhysics;
    [SerializeField] private Camera camera21;
    [SerializeField] private MouseScrollDetector mouseScrollDetector;
    [SerializeField] private GameObject lava;
    [SerializeField] private QuanticDoor quanticScript;
    public GameObject nextNextLevel;
    public DetectExit nextDetectExit;
    private GameObject player;
    private GameObject respawn;
    void Start()
    {
        player = Manager.instance.player;
        respawn = Manager.instance.respawn;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!(other.CompareTag("Player") && !hasExited))
            return;

        hasExited = true;   // Prevents the player from entering the exit multiple times
        level.value++;  // Next level
        respawn.transform.position = new Vector3(respawn.transform.position.x + 14, respawn.transform.position.y, respawn.transform.position.z);    // Move respawn point to the next level
        if (level.value != 16 && level.value != 24)
            door.Close();     // Close the door of the previous level
        if (level.value > 2 && level.value < 40)
            nextNextLevel.SetActive(true);
        if (level.value != 23)
        {
            Exit exit = GameObject.Find($"Level {level.value}/Exit").GetComponent<Exit>();
            Manager.instance.currentExit = exit;  // Set the current exit
        }
        if (level.value > 2 && level.value != 16 && level.value != 24)
            Destroy(GameObject.Find("Level " + (level.value - 2)));

        if (level.value == 14)
        {
            player.GetComponentInChildren<MeshCollider>().material = bouncyPhysics;
            player.GetComponent<PlayerControl>().Jump();
        }

        if (level.value == 15)
            player.GetComponentInChildren<MeshCollider>().material = normalPhysics;

        if (level.value == 12)
            player.GetComponent<PlayerControl>().canJump = true;

        if (level.value == 18)
        {
            player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, 180f);
            Physics.gravity = new Vector3(0, 9.81f, 0);
            Manager.instance.mainCamera.gameObject.GetComponent<PlayerCam>().Rotate();
        }

        if (level.value == 19)
        {
            player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, 0);
            Physics.gravity = new Vector3(0, -9.81f, 0);
            Manager.instance.mainCamera.gameObject.GetComponent<PlayerCam>().Rotate();
        }

        if (level.value == 21)
        {
            Manager.instance.mainCamera.enabled = false;
            camera21.gameObject.SetActive(true);
            Manager.instance.robot.SetActive(true);
        }
        if (level.value == 22)
        {
            camera21.gameObject.SetActive(false);
            Manager.instance.mainCamera.enabled = true;
            Manager.instance.robot.SetActive(false);
        }
        if (level.value == 26)
            nextDetectExit.mouseScrollDetector.enabled = true;
        if (level.value == 30)
            nextDetectExit.lava.SetActive(true);
        if (level.value == 31)
            lava.SetActive(false);
        if (level.value == 32)
            nextDetectExit.quanticScript.enabled = true;
        if (level.value == 33)
            quanticScript.enabled = false;
        if (level.value == 37)
            Manager.instance.spinCamera.enabled = true;

        if (level.value == 38)
        {
            Manager.instance.spinCamera.enabled = false;
            Manager.instance.rotationCamera.transform.rotation = Quaternion.Euler(Manager.instance.rotationCamera.transform.localEulerAngles.x, Manager.instance.rotationCamera.transform.localEulerAngles.y, 0);
            player.transform.rotation = Quaternion.Euler(90, player.transform.rotation.y, player.transform.rotation.z);
            Physics.gravity = new Vector3(0, 0, -9.81f);
            Manager.instance.mainCamera.gameObject.GetComponent<PlayerCam>().Rotate();
        }
        if (level.value == 39)
        {
            player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, 0);
            Physics.gravity = new Vector3(0, -9.81f, 0);
            Manager.instance.mainCamera.gameObject.GetComponent<PlayerCam>().Rotate();
        }

        OverlayManager.instance.NewLevel();
    }
}
