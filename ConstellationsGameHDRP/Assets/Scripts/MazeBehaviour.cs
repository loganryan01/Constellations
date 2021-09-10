using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MazeBehaviour : MonoBehaviour
{
    #region Fields
    [Header("Rotation Controls")]
    public float rotateSpeed; // Speed of the rotation of the maze
    public float maxRotation; // Max rotation of the maze

    [Header("Cameras")]
    public Camera mainCam; // The main camera of the player
    public Camera mazeCamera; // The camera for the puzzles

    [Header("Ball controls")]
    public Rigidbody ballRigidbody; // The rigidbody of the ball used for the maze
    public MazeBallBehaviour mazeBallBehaviour; // The script to control the maze ball
    
    [Header("Button Text")]
    public GameObject buttonText; // Text that displays what button to push to interact with the object

    [HideInInspector]
    public bool mazeCompleted; // Is the maze puzzle completed

    private float movementX; // The input from the 'a' and 'd' keys
    private float movementZ; // The input from the 'w' and 's' keys
    private float angleX; // Current x angle of the maze
    private float angleZ; // Current z angle of the maze
    private DialogueTrigger dialogueTrigger; // Dialogue for Taurus
    #endregion

    #region Functions
    // Start function
    void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    // Update function - run every frame
    void Update()
    {
        // If the player hasn't reached the end of the maze,
        if (!mazeBallBehaviour.touchedEnd)
        {
            // Get the player input and rotate the maze
            Rotate();

            // If the maze does not exceed 
            if (transform.rotation.eulerAngles.x < maxRotation || transform.rotation.eulerAngles.x > 360 - maxRotation)
            {
                angleX = transform.rotation.eulerAngles.x;
            }

            if (transform.rotation.eulerAngles.z < maxRotation || transform.rotation.eulerAngles.z > 360 - maxRotation)
            {
                angleZ = transform.rotation.eulerAngles.z;
            }

            gameObject.transform.eulerAngles = new Vector3(angleX, 0, angleZ);

            if (ballRigidbody.IsSleeping())
            {
                ballRigidbody.WakeUp();
            }
        }

        if (mazeBallBehaviour.touchedEnd && !mazeCompleted)
        {
            mazeCompleted = true;
            dialogueTrigger.TriggerDialogue();
        }

        
    }

    public void RotateMaze(InputAction.CallbackContext value)
    {
        Vector2 inputVector = value.ReadValue<Vector2>();

        movementX = -inputVector.x;
        movementZ = inputVector.y;
    }

    private void Rotate()
    {
        Vector3 rotate = gameObject.transform.forward * movementX + gameObject.transform.right * movementZ;

        gameObject.transform.Rotate(rotate * rotateSpeed * Time.deltaTime);
    }

    public void ChangeToMainCamera(bool enableMainCam)
    {
        if (!enableMainCam)
        {
            mainCam.enabled = false;
            mazeCamera.enabled = true;
        }
        else
        {
            mainCam.enabled = true;
            mazeCamera.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buttonText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buttonText.SetActive(false);
        }
    }
    #endregion
}
