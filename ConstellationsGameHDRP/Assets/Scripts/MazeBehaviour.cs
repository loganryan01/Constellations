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

    private Vector3 rotationDirection; // Direction of rotation
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
            gameObject.transform.Rotate(rotationDirection * rotateSpeed * Time.deltaTime);

            // If the maze does not exceed the maximum rotation
            if (transform.rotation.eulerAngles.x > maxRotation && transform.rotation.eulerAngles.x < 180)
            {
                transform.rotation = Quaternion.Euler(maxRotation, 0, transform.rotation.eulerAngles.z);
            }
            else if (transform.rotation.eulerAngles.x < 360 - maxRotation && transform.rotation.eulerAngles.x > 180)
            {
                transform.rotation = Quaternion.Euler(-maxRotation, 0, transform.rotation.eulerAngles.z);
            }

            if (transform.rotation.eulerAngles.z > maxRotation && transform.rotation.eulerAngles.z < 180)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, maxRotation);
            }
            else if (transform.rotation.eulerAngles.z < 360 - maxRotation && transform.rotation.eulerAngles.z > 180)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, -maxRotation);
            }

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

        rotationDirection = gameObject.transform.forward * inputVector.y  + gameObject.transform.right * inputVector.x;
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
