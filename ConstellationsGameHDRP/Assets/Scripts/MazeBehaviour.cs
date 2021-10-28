/*----------------------------------
    Name: MazeBehaviour
    Purpose: Rotates the maze.
    Authour: Logan Ryan
    Modified: 28 October 2021
------------------------------------
    Copyright 2021 Bookshelf Studios
----------------------------------*/
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class MazeBehaviour : MonoBehaviour
{
    #region Fields
    [Header("Rotation Controls")]
    public float rotateSpeed; // Speed of the rotation of the maze
    public float maxRotation; // Max rotation of the maze

    [Header("Cameras")]
    public Camera mainCam; // The main camera of the player
    public Camera puzzleCamera; // The camera for the puzzles

    [Header("Ball controls")]
    public Rigidbody ballRigidbody; // The rigidbody of the ball used for the maze
    public MazeBallBehaviour mazeBallBehaviour; // The script to control the maze ball
    
    [Header("Button Text")]
    public GameObject buttonText; // Text that displays what button to push to interact with the object

    [Header("Interaction Settings")]
    public UnityEvent onInteraction;

    [HideInInspector]
    public bool mazeCompleted; // Is the maze puzzle completed

    private Vector3 rotationDirection; // Direction of rotation
    private Vector3 startingRotation; // The starting rotation of the maze
    private Vector3 ballStartingPosition; // The starting position of the ball
    #endregion

    #region Functions
    private void Start()
    {
        startingRotation = transform.rotation.eulerAngles;
        ballStartingPosition = mazeBallBehaviour.gameObject.transform.position;
    }

    // Update function - run every frame
    void Update()
    {
        // If the player hasn't reached the end of the maze,
        if (!mazeBallBehaviour.touchedEnd)
        {
            gameObject.transform.Rotate(rotationDirection * rotateSpeed * Time.deltaTime);

            // If the maze does exceedes the maximum rotation on the x axis,
            if (transform.rotation.eulerAngles.x > maxRotation && transform.rotation.eulerAngles.x < 180)
            {
                // Set the x axis to the maximum rotation
                transform.rotation = Quaternion.Euler(maxRotation, 0, transform.rotation.eulerAngles.z);
            }
            else if (transform.rotation.eulerAngles.x < 360 - maxRotation && transform.rotation.eulerAngles.x > 180)
            {
                transform.rotation = Quaternion.Euler(-maxRotation, 0, transform.rotation.eulerAngles.z);
            }

            // If the maze does exceedes the maximum rotation on the z axis,
            if (transform.rotation.eulerAngles.z > maxRotation && transform.rotation.eulerAngles.z < 180)
            {
                // Set the z axis to the maximum rotation
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, maxRotation);
            }
            else if (transform.rotation.eulerAngles.z < 360 - maxRotation && transform.rotation.eulerAngles.z > 180)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, -maxRotation);
            }

            // If the ball is sleeping, wake it up
            if (ballRigidbody.IsSleeping())
            {
                ballRigidbody.WakeUp();
            }
        }

        // If the maze ball reaches the end, play dialogue
        if (mazeBallBehaviour.touchedEnd && !mazeCompleted)
        {
            mazeCompleted = true;
        }
    }

    // Get the input for the maze rotation
    public void RotateMaze(InputAction.CallbackContext value)
    {
        Vector2 inputVector = value.ReadValue<Vector2>();

        rotationDirection = gameObject.transform.forward * (-inputVector.y)  + gameObject.transform.right * (-inputVector.x);
    }

    public void ResetPuzzle(InputAction.CallbackContext value)
    {
        transform.rotation = Quaternion.Euler(startingRotation);
        mazeBallBehaviour.gameObject.transform.position = ballStartingPosition;
    }
    #endregion
}
