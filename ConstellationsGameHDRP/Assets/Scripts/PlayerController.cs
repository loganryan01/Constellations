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

    private void Start()
    {
        mainCam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
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

        if (enableOutline)
        {
            CanThePlayerInteract();
        }
    }

    public void ChangeLookSensitivity(float newLookSensitivity)
    {
        lookSensitivity = newLookSensitivity;
    }

    public void DisableOutlines(int layer)
    {
        if (lastSeenObject.layer != layer)
        {
            for (int i = 0; i < lastSeenObject.transform.childCount; i++)
            {
                lastSeenObject.transform.GetChild(i).gameObject.layer = layer;
            }

            lastSeenObject.transform.gameObject.layer = layer;
        }
    }

    public void DisableOutlines(int layer, Transform gameObjectTransform)
    {
        if (gameObjectTransform.gameObject.layer != layer)
        {
            for (int i = 0; i < gameObjectTransform.childCount; i++)
            {
                gameObjectTransform.GetChild(i).gameObject.layer = layer;
            }

            gameObjectTransform.gameObject.layer = layer;
        }
    }

    public void EnableOutlines()
    {
        enableOutline = !enableOutline;
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
                hitObject.GetComponent<MirrorBehaviour>().RotateMirror();
            }
            else if (hitObject.GetComponent<ScaleBehaviour>())
            {
                ScaleBehaviour scaleBehaviour = hitObject.GetComponent<ScaleBehaviour>();

                if (!scaleBehaviour.scalePuzzleCompleted)
                {
                    scaleBehaviour.onInteraction.Invoke();
                }
            }
            else if (hitObject.GetComponent<MazeBehaviour>())
            {
                MazeBehaviour mazeBehaviour = hitObject.GetComponent<MazeBehaviour>();

                if (!mazeBehaviour.mazeCompleted)
                {
                    mazeBehaviour.onInteraction.Invoke();
                }
            }
            else if (hitObject.GetComponent<ChannelBehaviour>())
            {
                hitObject.GetComponent<ChannelBehaviour>().RotateWaterChannel();
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

            if (hitObject.GetComponent<ScaleBehaviour>() && !hitObject.GetComponent<ScaleBehaviour>().scalePuzzleCompleted || 
                hitObject.GetComponent<ChannelBehaviour>() && !hitObject.GetComponent<ChannelBehaviour>().CheckCorrectRotation())
            {
                lastSeenObject = hitObject;

                buttonText.SetActive(true);

                if (hitObject.layer != 1)
                {
                    for (int i = 0; i < hitObject.transform.childCount; i++)
                    {
                        hitObject.transform.GetChild(i).gameObject.layer = 1;
                    }

                    hitObject.layer = 1;
                }
            }
            else if (hitObject.GetComponent<MazeBehaviour>() && !hitObject.GetComponent<MazeBehaviour>().mazeCompleted)
            {
                buttonText.SetActive(true);

                Transform mazeChild = hitObject.transform.GetChild(0);

                lastSeenObject = mazeChild.gameObject;

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
                buttonText.SetActive(true);

                if (lastSeenObject != hitObject)
                {
                    lastSeenObject = hitObject;

                    // Change layer of children to 1
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
            buttonText.SetActive(false);

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
}
