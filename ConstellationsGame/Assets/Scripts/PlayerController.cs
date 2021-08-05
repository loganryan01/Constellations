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

    // Variables needed for the scale puzzle to work
    private PlayerInput playerInputComponenet;
    private GameObject rockGameObject;
    private ScaleBehaviour scaleBehaviour;
    private BoxCollider[] scaleBoxColliders;
    private Vector3 scaleOriginalPosition;
    private bool scalePuzzleCompleted;

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

    private void Start()
    {
        mainCam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        playerInputComponenet = GetComponent<PlayerInput>();
        scaleBoxColliders = GameObject.Find("SM_Scales").GetComponents<BoxCollider>();
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

        if (scaleBehaviour != null)
        {
            ScaleGame();
        }
        else if (Camera.main != null && scalePuzzleCompleted && playerInputComponenet.currentActionMap != playerInputComponenet.actions.FindActionMap("PlayerController"))
        {
            playerInputComponenet.SwitchCurrentActionMap("PlayerController");
        }
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

    public void OnClick(InputAction.CallbackContext value)
    {
        Camera scaleCamera = GameObject.Find("ScaleCamera").GetComponent<Camera>();

        Vector3 pos = Mouse.current.position.ReadValue();

        Ray ray = scaleCamera.ScreenPointToRay(pos);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 25))
        {
            if (hit.collider != null)
            {
                //Debug.Log(hit.collider.gameObject.transform.name);
                
                if (hit.collider.CompareTag("Rock"))
                {
                    rockGameObject = hit.collider.gameObject;
                }
            }
        }
    }

    public void OnDeselect(InputAction.CallbackContext value)
    {
        if (rockGameObject != null)
        {
            rockGameObject.GetComponent<Rigidbody>().isKinematic = false;
            rockGameObject = null;
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
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, interactDist);

        if (hit.collider != null)
        {
            GameObject hitObject = hit.transform.gameObject;

            if (hitObject.GetComponent<MirrorBehaviour>())
            {
                hitObject.GetComponent<MirrorBehaviour>().RotateMirror();
                interactTriggered = false;
            }
            else if (hitObject.GetComponent<ScaleBehaviour>())
            {
                scaleBehaviour = hitObject.GetComponent<ScaleBehaviour>();
                
                hitObject.GetComponent<ScaleBehaviour>().ChangeToMainCamera(false);
                interactTriggered = false;

                playerInputComponenet.SwitchCurrentActionMap("ScalePuzzle");

                scaleBoxColliders[0].enabled = false;
                scaleBoxColliders[1].enabled = false;
                scaleOriginalPosition = transform.position;
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 20);

                Cursor.lockState = CursorLockMode.None;
            }
            //else if (hitObject.GetComponent<MazeBehaviour>())
            //{
            //    // Interact with maze
            //}
        }

        interactTriggered = false;
    }

    public void ScaleGame()
    {
        // When the scale puzzle is completed, switch back to player controller
        if (scaleBehaviour.lockScale)
        {
            scaleBehaviour = null;

            Cursor.lockState = CursorLockMode.Locked;

            scaleBoxColliders[0].enabled = true;
            scaleBoxColliders[1].enabled = true;

            transform.position = scaleOriginalPosition;

            scalePuzzleCompleted = true;
        }
        
        // When the player clicks the rock, make the rock follow the mouse
        if (rockGameObject != null)
        {
            Cursor.lockState = CursorLockMode.None;

            Camera scaleCamera = GameObject.Find("ScaleCamera").GetComponent<Camera>();

            // Screen Position
            Vector3 mousePosition = Mouse.current.position.ReadValue();

            // World Position
            Vector3 rockPosition = scaleCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 8));

            rockGameObject.transform.position = rockPosition;
            rockGameObject.GetComponent<Rigidbody>().isKinematic = true; 
        }
    }
}
