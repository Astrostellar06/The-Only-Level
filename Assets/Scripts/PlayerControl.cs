using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR;
using Unity.VisualScripting;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private IntVar level;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float groundDrag;

    public float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    public bool canJump = true;

    [Header("Ground Check")]
    public bool isGrounded;
    private Dictionary<GameObject, bool> listContacts = new Dictionary<GameObject, bool>();
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private string groundMaskName;

    [Header("Keybind")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sneakKey = KeyCode.LeftControl;

    public int konamiProgress = 0;

    [SerializeField] private Transform orientation;
    [SerializeField] private GameObject rotationCamera;

    float horizontalInput;
    float verticalInput;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        MyInput();
        if (level.value != 22 && level.value != 38)
            MovePlayer();
        else if (level.value == 38)
            MovePlayer2();
        else if (Input.GetMouseButtonDown(0))
            Tp();
    }

    private void FixedUpdate()
    {
        CheckAllContacts();
    }

    private void CheckAllContacts()
    {
        bool newIsGrounded = false; 
        foreach (KeyValuePair<GameObject, bool> entry in listContacts)
        {
            newIsGrounded |= entry.Value;
            if (newIsGrounded)
                break;
        }
        isGrounded = newIsGrounded;
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
            rb.drag = 0;
        if ((level.value == 17 && !isGrounded && listContacts.Count > 0) || level.value == 27)
        {
            rb.useGravity = false;
            rb.drag = groundDrag;
        }
        else
            rb.useGravity = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer(groundMaskName))
            return;
        rb.useGravity = level.value != 17;
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(groundMaskName) && (Vector3.Angle(collision.GetContact(i).normal, (level.value != 18 && level.value != 38 ? Vector3.up : level.value == 38 ? Vector3.back : Vector3.down)) < 45 || Physics.Raycast(transform.position, (level.value != 18 && level.value != 38 ? Vector3.up : level.value == 38 ? Vector3.back : Vector3.down), .8f)))
            {
                listContacts[collision.gameObject] = true;
                return;
            }
        }
        listContacts[collision.gameObject] = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer(groundMaskName))
            return;

        listContacts.Remove(collision.gameObject);
        if (listContacts.Count == 0)
        {
            rb.drag = 0;
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundMaskName) && !listContacts.ContainsKey(collision.gameObject))
            listContacts.Add(collision.gameObject, false);
        if (level.value == 19 && collision.gameObject.name == "Floor")
            Manager.instance.Death();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (level.value == 31)
        {
            horizontalInput = (Input.GetKey(KeyCode.V) ? -1 : 0) + (Input.GetKey(KeyCode.N) ? 1 : 0);
            verticalInput = (Input.GetKey(KeyCode.G) ? 1 : 0) + (Input.GetKey(KeyCode.B) ? -1 : 0);
        }

        if (Input.GetKey(jumpKey) && canJump && isGrounded && level.value != 11 && level.value != 14 && level.value != 17 && level.value != 27)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(KeyCode.O))    // For debug to go faster
            Manager.instance.currentExit.Open();

        if ((level.value == 20 && Input.GetKeyDown(KeyCode.LeftShift)) || (level.value == 24 && Input.GetKeyDown(KeyCode.Alpha6)))
            Manager.instance.currentExit.Open();
        if (level.value == 24 && Input.GetKeyDown(KeyCode.Alpha8))
            Manager.instance.Death();
        if (level.value == 36)
        {
            switch (konamiProgress)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                        konamiProgress++;
                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                        konamiProgress++;
                    else if (Input.anyKeyDown)
                        konamiProgress = 0;
                    break;
                case 2:
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                        konamiProgress++;
                    else if (Input.anyKeyDown)
                        konamiProgress = 0;
                    break;
                case 3:
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                        konamiProgress++;
                    else if (Input.anyKeyDown)
                        konamiProgress = 0;
                    break;
                case 4:
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                        konamiProgress++;
                    else if (Input.anyKeyDown)
                        konamiProgress = 0;
                    break;
                case 5:
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                        konamiProgress++;
                    else if (Input.anyKeyDown)
                        konamiProgress = 0;
                    break;
                case 6:
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                        konamiProgress++;
                    else if (Input.anyKeyDown)
                        konamiProgress = 0;
                    break;
                case 7:
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                        konamiProgress++;
                    else if (Input.anyKeyDown)
                        konamiProgress = 0;
                    break;
                case 8:
                    if (Input.GetKeyDown(KeyCode.B))
                        konamiProgress++;
                    else if (Input.anyKeyDown)
                        konamiProgress = 0;
                    break;
                case 9:
                    if (Input.GetKeyDown(KeyCode.Q))
                        Manager.instance.currentExit.Open();
                    else if (Input.anyKeyDown)
                        konamiProgress = 0;
                    break;
            }
        }
    }

    private void MovePlayer()
    {
        Vector3 move = orientation.right * horizontalInput + orientation.forward * verticalInput;
        
        move = (level.value == 3 ? move * -1 : move);

        Vector3 velocity = move * moveSpeed * (isGrounded || level.value == 27 ? 1 : airMultiplier);

        if (level.value != 17 && level.value != 27)
        {
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
            return;
        }
        if (level.value == 27)
        {
            Vector3 rise = orientation.up * ((Input.GetKey(jumpKey) ? 1 : 0) + (Input.GetKey(sneakKey) ? -1 : 0)) * moveSpeed;
            rb.velocity = new Vector3(velocity.x, rise.y, velocity.z);
            return;
        }


        if (verticalInput > 0 && listContacts.Count > 0 && Physics.Raycast(new Vector3(transform.position.x, transform.position.y - .7f, transform.position.z), orientation.forward, .6f, groundMask))
            rb.velocity = new Vector3((orientation.right * horizontalInput).x, moveSpeed * .6f, (orientation.right * horizontalInput).y);
        else if (verticalInput < 0 && listContacts.Count > 0 && Physics.Raycast(new Vector3(transform.position.x, transform.position.y - .7f, transform.position.z), orientation.forward, .6f, groundMask) && !isGrounded)
            rb.velocity = new Vector3((orientation.right * horizontalInput).x, moveSpeed * -.6f, (orientation.right * horizontalInput).y);
        else
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    private void MovePlayer2()
    {
        Vector3 move = rotationCamera.transform.forward * verticalInput + rotationCamera.transform.right * horizontalInput;

        Vector3 velocity = move * moveSpeed * (isGrounded || level.value == 27 ? 1 : airMultiplier);
        rb.velocity = new Vector3(velocity.x, velocity.y, rb.velocity.z);
    }

    public void Jump()
    {
        rb.drag = 0;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce((level.value == 18 ? Vector3.down : level.value == 38 ? Vector3.forward : Vector3.up) * jumpForce * (level.value == 14 ? 2 : (level.value == 18 ? 1.2f : 1)), ForceMode.Impulse);
    }

    public void Jump2()
    {
        rb.drag = 0;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce * 1.5f, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void Tp()
    {
        var ray = Manager.instance.mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            rb.transform.position = hit.point;
            Physics.SyncTransforms();
        }
    }
}
