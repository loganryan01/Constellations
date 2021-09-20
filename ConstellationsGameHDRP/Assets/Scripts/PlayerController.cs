using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    #region Fields
    private Transform mainCam;
    public PlayerInputActions playerInput;
    private CharacterController controller;

    [Header("Movement")]
    [SerializeField]
    public float moveSpeed = 5.0f;

    private Vector3 rawInputMovement;

    [Header("Gravity/Ground Settings")]
    [SerializeField]
    public float gravity = -9.81f;
    [SerializeField]
    public Transform groundCheck;
    [SerializeField]
    public LayerMask groundMask;
    [SerializeField]
    public float groundDistance = 0.4f;

    private Vector3 velocity;
    private bool isGrounded = true;

    [Header("Looking Around")]
    [SerializeField]
    public float lookSensitivity = 60.0f;
    [SerializeField]
    public float minViewAngle = -40.0f;
    [SerializeField]
    public float maxViewAngle = 50.0f;

    private Vector2 rawInputLook;
    private float xRotation = 0.0f;

    [Header("Interactions")]
    [SerializeField]
    private float interactDist = 4.0f;

    private bool interactTriggered = false;
    private bool enableOutline = true;

    private GameObject lastSeenObject;

    [Header("Interact Text")]
    public GameObject buttonText; // Text that displays button to press to interact with puzzle
    #endregion

    #region Functions
    // Start function
    private void Start()
    {
        mainCam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Awake function
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = new PlayerInputActions();
    }

    // On Enable function
    private void OnEnable()
    {
        playerInput.PlayerController.Enable();
        playerInput.ScalePuzzle.Enable();
    }

    // On Disable
    private void OnDisable()
    {
        playerInput.PlayerController.Disable();
        playerInput.ScalePuzzle.Disable();
    }

    // Update function - run every frame
    private void Update()
    {
        // Check if player is touching the round
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // If the player is on the ground, enable gravity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }

        PlayerMovement();
        PlayerLook();
        PlayerInteract();

        if (enableOutline)
        {
            CanThePlayerInteract();
        }
    }

    // Change the player's look sensitivity
    public void ChangeLookSensitivity(float newLookSensitivity)
    {
        lookSensitivity = newLookSensitivity;
    }

    // Disable the outlines of an object
    public void DisableOutlines(int layer)
    {
        // Check if the last seen object still has outlines being displayed
        if (lastSeenObject.layer != layer)
        {
            // If it does, change the layer to disable the outlines
            for (int i = 0; i < lastSeenObject.transform.childCount; i++)
            {
                lastSeenObject.transform.GetChild(i).gameObject.layer = layer;
            }

            lastSeenObject.transform.gameObject.layer = layer;
        }
    }

    // Disable the outlines of an object
    public void DisableOutlines(int layer, Transform gameObjectTransform)
    {
        // Check if the last seen object children still has outlines being displayed
        if (gameObjectTransform.gameObject.layer != layer)
        {
            // If it does, change the layer to disable the outlines
            for (int i = 0; i < gameObjectTransform.childCount; i++)
            {
                gameObjectTransform.GetChild(i).gameObject.layer = layer;
            }

            gameObjectTransform.gameObject.layer = layer;
        }
    }

    // Should the outlines be displayed
    public void EnableOutlines()
    {
        enableOutline = !enableOutline;
    }

    // Get the movement from the player keyboard input
    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputVector = value.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputVector.x, 0, inputVector.y);
    }

    // Get the rotation from the player mouse input
    public void OnLook(InputAction.CallbackContext value)
    {
        Vector2 mouseVector = value.ReadValue<Vector2>();
        rawInputLook = mouseVector;
    }

    // Get input to interact with object
    public void OnInteract(InputAction.CallbackContext value)
    {
        if (value.canceled)
        {
            interactTriggered = true;
            Debug.Log("Interact");
        }
    }

    // Lock the cursor in the center of the screen or not
    public void LockCursor(bool lockCursor)
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Move the player
    private void PlayerMovement()
    {
        float movementX = rawInputMovement.x;
        float movementZ = rawInputMovement.z;

        Vector3 move = transform.right * movementX + transform.forward * movementZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    // Rotate the player head
    private void PlayerLook()
    {
        float mouseX = rawInputLook.x * lookSensitivity * Time.deltaTime;
        float mouseY = rawInputLook.y * lookSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minViewAngle, maxViewAngle);

        mainCam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    // Interact with an object
    private void PlayerInteract()
    {
        if (!interactTriggered)
        {
            return;
        }

        // Draw a raycast from the camera
        RaycastHit hit;
        Physics.Raycast(mainCam.position, mainCam.TransformDirection(Vector3.forward), out hit, interactDist);

        // If it hits an object with a collider
        if (hit.collider != null)
        {
            GameObject hitObject = hit.transform.gameObject;

            // If it hits a mirror, rotate it
            if (hitObject.GetComponent<MirrorBehaviour>())
            {
                hitObject.GetComponent<MirrorBehaviour>().RotateMirror();
            }
            // If it hits the scale, move the camera
            else if (hitObject.GetComponent<ScaleBehaviour>())
            {
                ScaleBehaviour scaleBehaviour = hitObject.GetComponent<ScaleBehaviour>();

                if (!scaleBehaviour.scalePuzzleCompleted)
                {
                    scaleBehaviour.onInteraction.Invoke();
                }
            }
            // If it hits the maze, move the camera
            else if (hitObject.GetComponent<MazeBehaviour>())
            {
                MazeBehaviour mazeBehaviour = hitObject.GetComponent<MazeBehaviour>();

                if (!mazeBehaviour.mazeCompleted)
                {
                    mazeBehaviour.onInteraction.Invoke();
                }
            }
            // If it hits a channel, rotate the water channel
            else if (hitObject.GetComponent<ChannelBehaviour>())
            {
                hitObject.GetComponent<ChannelBehaviour>().RotateWaterChannel();
            }
        }

        interactTriggered = false;
    }

    // Check if the player can interact with the object
    private void CanThePlayerInteract()
    {
        // Draw raycast from main camera
        RaycastHit hit;
        Physics.Raycast(mainCam.position, mainCam.TransformDirection(Vector3.forward), out hit, interactDist);

        // If the raycast hits an object with a collider,
        if (hit.collider != null)
        {
            GameObject hitObject = hit.transform.gameObject;

            // If the object is the scale or a channel, 
            if (hitObject.GetComponent<ScaleBehaviour>() && !hitObject.GetComponent<ScaleBehaviour>().scalePuzzleCompleted || 
                hitObject.GetComponent<ChannelBehaviour>() && !hitObject.GetComponent<ChannelBehaviour>().CheckCorrectRotation())
            {
                lastSeenObject = hitObject;

                // Display button text
                buttonText.SetActive(true);

                // Draw outline for the object's children
                if (hitObject.layer != 1)
                {
                    for (int i = 0; i < hitObject.transform.childCount; i++)
                    {
                        hitObject.transform.GetChild(i).gameObject.layer = 1;
                    }

                    hitObject.layer = 1;
                }
            }
            // If the object is a mirror
            else if (hitObject.GetComponent<MazeBehaviour>() && !hitObject.GetComponent<MazeBehaviour>().mazeCompleted)
            {
                // Display button text
                buttonText.SetActive(true);

                Transform mazeChild = hitObject.transform.GetChild(0);

                lastSeenObject = mazeChild.gameObject;

                // Draw outline for the object's grandchildren
                if (hitObject.layer != 1)
                {
                    for (int i = 0; i < mazeChild.childCount; i++)
                    {
                        mazeChild.GetChild(i).gameObject.layer = 1;
                    }

                    mazeChild.gameObject.layer = 1;
                }
            }
            else if (hitObject.GetComponent<MirrorBehaviour>() && !hitObject.GetComponent<MirrorBehaviour>().laserBehaviour.laserPuzzleCompleted)
            {
                // Display button text
                buttonText.SetActive(true);

                if (lastSeenObject != hitObject)
                {
                    lastSeenObject = hitObject;

                    // Draw outline for the object's children
                    for (int i = 0; i < hitObject.transform.childCount; i++)
                    {
                        if (hitObject.transform.GetChild(i).gameObject.layer != 1)
                        {
                            hitObject.transform.GetChild(i).gameObject.layer = 1;
                        }
                    }
                }
            }
        }
        else
        {
            // Hide Button's text
            buttonText.SetActive(false);

            // Hide outline for object
            if (lastSeenObject != null)
            {
                // Change layer of mirror children to 0
                if (lastSeenObject.GetComponent<MirrorBehaviour>())
                {
                    DisableOutlines(0, lastSeenObject.transform.GetChild(0));
                    DisableOutlines(0, lastSeenObject.transform.GetChild(1));
                }
                else
                {
                    DisableOutlines(0);
                }
            }
        }
    }
    #endregion
}
