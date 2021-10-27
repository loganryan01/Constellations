/*----------------------------------
    Name: PlayerController
    Purpose: Controls the player.
    Authour: Mara Dusevic
    Modified: 27 October 2021
------------------------------------
    Copyright 2021 Bookshelf Studios
----------------------------------*/
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class PuzzleOutlineEvent : UnityEvent<int, Transform>
{

}

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

    [Header("Ending settings")]
    public int numberOfPuzzles = 4;
    public Color targetColour = new Color(0,0,0,1);
    public Image elementToFade;
    private int puzzlesCompleted = 0;

    PuzzleOutlineEvent puzzleOutline;
    #endregion

    #region Functions
    // Start function
    private void Start()
    {
        lookSensitivity = PlayerPrefs.GetFloat("Look Sensitivity");
        
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
    // Change the layer of the last seen object
    public void DisableOutlines(int layer)
    {
        // Check if the last seen object still has outlines being displayed
        if (lastSeenObject.layer != layer)
        {
            // If it does, change the layer to disable the outlines
            for (int i = 0; i < lastSeenObject.transform.childCount; i++)
            {
                if (lastSeenObject.transform.childCount > 0)
                {
                    DisableOutlines(layer, lastSeenObject.transform.GetChild(0));
                }
                lastSeenObject.transform.GetChild(i).gameObject.layer = layer;
            }

            lastSeenObject.transform.gameObject.layer = layer;
        }
    }

    // Disable the outlines of an object
    // Change the layer of chosen game object children
    public void DisableOutlines(int layer, Transform gameObjectTransform)
    {
        // Check if the last seen object children still has outlines being displayed
        if (gameObjectTransform.gameObject.layer != layer)
        {
            // Change the layer of the gameobject
            gameObjectTransform.gameObject.layer = layer;
            // Check if the current gameobject has children
            // If it does go into that child
            for (int i = 0; i < gameObjectTransform.childCount; i++)
            {
                if (gameObjectTransform.childCount > 0)
                {
                    DisableOutlines(layer, gameObjectTransform.GetChild(i));
                }
            }
        }
    }

    // Should the outlines be displayed
    public void EnableOutlines()
    {
        enableOutline = !enableOutline;
    }

    // Checks if player has completed all the puzzles
    public void EndGameCheck()
    {
        // Gets called when a puzzle is completed
        // Increase number of puzzles solved by 1
        puzzlesCompleted++;

        // Check if the number of puzzles completed is equal to the number of puzzles in the game
        // If they are, then screen fades to black and load the ending scene
        if(puzzlesCompleted == numberOfPuzzles)
        {
            StartCoroutine(FadeToBlack(targetColour, 5));
        }
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
                    if (puzzleOutline == null)
                    {
                        puzzleOutline = new PuzzleOutlineEvent();
                    }

                    puzzleOutline.AddListener(DisableOutlines);
                    puzzleOutline.Invoke(2, hitObject.transform);

                    scaleBehaviour.onInteraction.Invoke();

                    puzzleOutline.RemoveAllListeners();
                }
            }
            // If it hits the maze, move the camera
            else if (hitObject.GetComponent<MazeBehaviour>())
            {
                MazeBehaviour mazeBehaviour = hitObject.GetComponent<MazeBehaviour>();

                if (!mazeBehaviour.mazeCompleted)
                {
                    if (puzzleOutline == null)
                    {
                        puzzleOutline = new PuzzleOutlineEvent();
                    }

                    puzzleOutline.AddListener(DisableOutlines);
                    puzzleOutline.Invoke(2, hitObject.transform);

                    mazeBehaviour.onInteraction.Invoke();

                    puzzleOutline.RemoveAllListeners();
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
            if (hitObject.GetComponent<ChannelBehaviour>() && !hitObject.GetComponent<ChannelBehaviour>().CheckCorrectRotation())
            {
                lastSeenObject = hitObject;

                // Display button text
                buttonText.SetActive(true);

                // Draw outline for the object's children
                DisableOutlines(1, hitObject.transform);
            }
            // If the object is a mirror
            else if (hitObject.GetComponent<MazeBehaviour>() && !hitObject.GetComponent<MazeBehaviour>().mazeCompleted)
            {
                lastSeenObject = hitObject;

                // Display button text
                buttonText.SetActive(true);

                // Draw outline for the object's grandchildren
                DisableOutlines(1, hitObject.transform);
            }
            else if (hitObject.GetComponent<MirrorBehaviour>() && !hitObject.GetComponent<MirrorBehaviour>().laserBehaviour.laserPuzzleCompleted)
            {
                lastSeenObject = hitObject;

                // Display button text
                buttonText.SetActive(true);

                // Draw outline for the object's children
                DisableOutlines(1, hitObject.transform.GetChild(0));
                DisableOutlines(1, hitObject.transform.GetChild(1));
                //DisableOutlines(1, hitObject.transform.GetChild(2));
            }
            else if (hitObject.GetComponent<ScaleBehaviour>() && !hitObject.GetComponent<ScaleBehaviour>().scalePuzzleCompleted)
            {
                lastSeenObject = hitObject;

                // Display button text
                buttonText.SetActive(true);

                // Draw outline for the object's children
                DisableOutlines(1, hitObject.transform);
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
                        DisableOutlines(9, lastSeenObject.transform);
                        DisableOutlines(0, lastSeenObject.transform.GetChild(0));
                        DisableOutlines(9, lastSeenObject.transform.GetChild(1));
                    }
                    else if (lastSeenObject.GetComponent<ScaleBehaviour>())
                    {
                        DisableOutlines(0, lastSeenObject.transform.GetChild(6));
                        DisableOutlines(0, lastSeenObject.transform);
                    }
                    else if (lastSeenObject.GetComponent<ChannelBehaviour>())
                    {
                        Debug.Log("Disabling outlines for channel");
                        DisableOutlines(0, lastSeenObject.transform);
                    }
                    else
                    {
                        DisableOutlines(0, lastSeenObject.transform);
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
                    DisableOutlines(9, lastSeenObject.transform);
                    DisableOutlines(0, lastSeenObject.transform.GetChild(0));
                    DisableOutlines(9, lastSeenObject.transform.GetChild(1));
                }
                else if (lastSeenObject.GetComponent<ScaleBehaviour>())
                {
                    DisableOutlines(0, lastSeenObject.transform.GetChild(6));
                    DisableOutlines(0, lastSeenObject.transform);
                }
                else if (lastSeenObject.GetComponent<ChannelBehaviour>())
                {
                    Debug.Log("Disabling outlines for channel");
                    DisableOutlines(0, lastSeenObject.transform);
                }
                else
                {
                    DisableOutlines(0, lastSeenObject.transform);
                }
            }
        }
    }

    IEnumerator FadeToBlack(Color endValue, float duration)
    {
        float time = 0;
        Color startValue = elementToFade.color;

        while (time < duration)
        {
            elementToFade.color = Color.Lerp(startValue, endValue, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        elementToFade.color = endValue;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    #endregion
}
