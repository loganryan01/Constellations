using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private Transform mainCam;
    public PlayerInputActions playerInput;
    private CharacterController controller;

    // Variables needed for dialogue to work
    public DialogueManager dialogueManager;

    // Variables needed for the scale puzzle to work
    [HideInInspector]
    public PlayerInput playerInputComponent;
    [HideInInspector]
    public ScaleBehaviour scaleBehaviour;

    // Variables needed for the maze puzzle to work
    [HideInInspector]
    public MazeBehaviour mazeBehaviour;

    // Variables need for the laser puzzle to work
    [HideInInspector]
    public LaserBehaviour laserBehaviour;

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

    [Header("Interact Text")]
    public GameObject buttonText; // Text that displays button to press to interact with puzzle

    private bool interactTriggered = false;

    private void Start()
    {
        mainCam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        playerInputComponent = GetComponent<PlayerInput>();
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInput.PlayerController.Enable();
        playerInput.ScalePuzzle.Enable();
    }

    private void OnDisable()
    {
        playerInput.PlayerController.Disable();
        playerInput.ScalePuzzle.Disable();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }

        PlayerMovement();
        PlayerLook();
        PlayerInteract();
        CanThePlayerInteract();

        // When the puzzles are completed
        if (Camera.main != null && laserBehaviour != null && laserBehaviour.laserPuzzleCompleted && dialogueManager.dialogueEnded)
        {
            laserBehaviour = null;
        }
        else if (Camera.main != null && mazeBehaviour != null && mazeBehaviour.mazeCompleted && playerInputComponent.currentActionMap != playerInputComponent.actions.FindActionMap("PlayerController") &&
            dialogueManager.dialogueEnded)
        {
            mazeBehaviour = null;
        }
        else if (Camera.main != null && scaleBehaviour != null && scaleBehaviour.scalePuzzleCompleted && playerInputComponent.currentActionMap != playerInputComponent.actions.FindActionMap("PlayerController") &&
            dialogueManager.dialogueEnded)
        {
            scaleBehaviour = null;
        }
    }

    public void ChangeLookSensitivity(float newLookSensitivity)
    {
        lookSensitivity = newLookSensitivity;
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputVector = value.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputVector.x, 0, inputVector.y);
    }

    public void OnLook(InputAction.CallbackContext value)
    {
        Vector2 mouseVector = value.ReadValue<Vector2>();
        rawInputLook = mouseVector;
    }

    public void OnInteract(InputAction.CallbackContext value)
    {
        if (value.canceled)
        {
            interactTriggered = true;
            Debug.Log("Interact");
        }
    }

    private void PlayerMovement()
    {
        float movementX = rawInputMovement.x;
        float movementZ = rawInputMovement.z;

        Vector3 move = transform.right * movementX + transform.forward * movementZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void PlayerLook()
    {
        float mouseX = rawInputLook.x * lookSensitivity * Time.deltaTime;
        float mouseY = rawInputLook.y * lookSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minViewAngle, maxViewAngle);

        mainCam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void PlayerInteract()
    {
        if (!interactTriggered)
        {
            return;
        }

        RaycastHit hit;
        Physics.Raycast(mainCam.position, mainCam.TransformDirection(Vector3.forward), out hit, interactDist);

        if (hit.collider != null)
        {
            GameObject hitObject = hit.transform.gameObject;

            if (hitObject.GetComponent<MirrorBehaviour>())
            {
                laserBehaviour = FindObjectOfType<LaserBehaviour>();
                
                hitObject.GetComponent<MirrorBehaviour>().RotateMirror();
                interactTriggered = false;
            }
            else if (hitObject.GetComponent<ScaleBehaviour>())
            {
                //===== On Interaction =====
                scaleBehaviour = hitObject.GetComponent<ScaleBehaviour>();

                scaleBehaviour.ChangeToMainCamera(false);
                interactTriggered = false;

                playerInputComponent.SwitchCurrentActionMap("ScalePuzzle");

                hitObject.layer = 2;

                Cursor.lockState = CursorLockMode.None;
            }
            else if (hitObject.GetComponent<MazeBehaviour>())
            {
                mazeBehaviour = hitObject.GetComponent<MazeBehaviour>();
                
                // Interact with maze
                playerInputComponent.SwitchCurrentActionMap("MazePuzzle");
                mazeBehaviour.ChangeToMainCamera(false);
            }
            else if (hitObject.GetComponent<ChannelBehaviour>())
            {
                hitObject.GetComponent<ChannelBehaviour>().RotateWaterChannel();
                interactTriggered = false;
            }
        }

        interactTriggered = false;
    }

    private void CanThePlayerInteract()
    {
        RaycastHit hit;
        Physics.Raycast(mainCam.position, mainCam.TransformDirection(Vector3.forward), out hit, interactDist);

        if (hit.collider != null)
        {
            GameObject hitObject = hit.transform.gameObject;

            if (hitObject.GetComponent<ScaleBehaviour>() || 
                hitObject.GetComponent<MirrorBehaviour>() ||
                hitObject.GetComponent<MazeBehaviour>() ||
                hitObject.GetComponent<ChannelBehaviour>())
            {
                buttonText.SetActive(true);
            }
        }
        else
        {
            buttonText.SetActive(false);
        }
    }
}
