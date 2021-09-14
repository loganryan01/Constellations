using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScaleBehaviour : MonoBehaviour
{
    public enum Side
    {
        Left,
        Right
    }

    #region Fields
    [Header("Interact Text")]
    public GameObject buttonText; // Text that displays button to press to interact with puzzle

    [Header("Scale Settings")]
    public GameObject leftScale; // Left hand of the scale
    public GameObject rightScale; // Right hand of the scale
    public GameObject arm; // Fulcrum of the scale
    public GameObject leftDoor; // Left door of the temple
    public GameObject rightDoor; // Right door of the temple

    public Vector3[] leftScalePositions; // Positions of the left hand
    public Vector3[] rightScalePositions; // Positions of the right hand
    public Vector3[] armRotations; // Rotations of the arm
    public Vector3[] doorRotations; // Rotations of the doors

    [Header("Dialogue Settings")]
    public DialogueManager dialogueManager; // Manager script of the dialogue

    [HideInInspector]
    public float leftWeight; // Weight of the left hand
    [HideInInspector]
    public float rightWeight; // Weight of the right hand

    [Header("Start settings")]
    public Side startingSide; // What side the scale is starting on
    [HideInInspector]
    public bool scalePuzzleCompleted = false; // Is the scale puzzle completed

    private GameObject rockGameObject; // The rock that the player is holding

    private bool leftIsHeavy = false; // Is the left hand heavier than the right hand
    private bool rightIsHeavy = false; // Is the right hand heavier than the left hand

    private Vector3 heavyLeftPosition; // The position where the left hand is the heaviest
    private Vector3 heavyRightPosition; // The position where the right hand is the heaviest

    private Vector3 middleLeftPosition; // The position where the left hand is in the middle
    private Vector3 middleRightPosition; // The position where the right hand is in the middle

    private Vector3 lightLeftPosition; // The position where the left hand is the lightest
    private Vector3 lightRightPosition; // The position where the right hand is the lightest

    private Camera mainCam; // The main camera of the scene
    private Camera scaleCamera; // The camera for the puzzles

    private DialogueTrigger dialogueTrigger; // Dialogue for the puzzle
    #endregion

    #region Functions
    // Start function
    void Start()
    {
        // Hide text that displays what button to push to interact with scale
        buttonText.SetActive(false);

        // Get the camera for the puzzle and hide it
        scaleCamera = GameObject.Find("PuzzleCamera").GetComponent<Camera>();
        scaleCamera.enabled = false;

        // Get the main camera of the scene
        mainCam = Camera.main;

        // Calculate the position when the hand is the heaviest
        heavyLeftPosition = leftScale.transform.position + leftScalePositions[2];
        heavyRightPosition = rightScale.transform.position + rightScalePositions[2];

        // Calculate the position when the hand is the lightest
        lightLeftPosition = leftScale.transform.position + leftScalePositions[0];
        lightRightPosition = rightScale.transform.position + rightScalePositions[0];

        // If the scale is starting on the left side
        if (startingSide == Side.Left)
        {
            // Move the left hand to the heavy position
            leftScale.transform.position = heavyLeftPosition;

            // Move the right hand to the light position
            rightScale.transform.position = lightRightPosition;

            // Rotate fulcrum to the left
            arm.transform.rotation = Quaternion.Euler(armRotations[0]);
            leftIsHeavy = true;
        }
        // If the scale is starting on the right side
        else if (startingSide == Side.Right)
        {
            // Move the left hand to the light position
            leftScale.transform.position = lightLeftPosition;

            // Move the right hand to the heavy position
            rightScale.transform.position = heavyRightPosition;

            // Rotate fulcrum to the right
            arm.transform.rotation = Quaternion.Euler(armRotations[2]);
            rightIsHeavy = true;
        }

        // Get dialogue for the scale
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    // Update function - run every frame
    void Update()
    {
        // When the player clicks the rock, make the rock follow the mouse
        if (rockGameObject != null)
        {
            // Unlock the mouse
            Cursor.lockState = CursorLockMode.None;

            // Get the Screen Position
            Vector3 mousePosition = Mouse.current.position.ReadValue();

            // Get the World Position
            Vector3 rockPosition = scaleCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 5));

            // Set the position of the rock to be the mouse position
            rockGameObject.transform.position = rockPosition;
            rockGameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        // When the dialogue has finished,
        if (Camera.main == null && scalePuzzleCompleted && dialogueManager.dialogueEnded)
        {
            // Lock the cursor in the center
            Cursor.lockState = CursorLockMode.Locked;

            // Open the doors
            StartCoroutine(LerpRotation(Quaternion.Euler(doorRotations[0]), 5, leftDoor));
            StartCoroutine(LerpRotation(Quaternion.Euler(doorRotations[1]), 5, rightDoor));
        }
    }

    public void UpdateScale()
    {
        // If the puzzle is not completed,
        if (!scalePuzzleCompleted)
        {
            // If the weight on the left side of the scale is heavier than the right side,            
            if (leftWeight > rightWeight && !leftIsHeavy)
            {
                // Move the left hand to the down position
                StartCoroutine(LerpPosition(heavyLeftPosition, 5, leftScale));

                // Move the right hand to the up position
                StartCoroutine(LerpPosition(lightRightPosition, 5, rightScale));

                // Rotate the arm to the left side
                StartCoroutine(LerpRotation(Quaternion.Euler(armRotations[0]), 5, arm));

                leftIsHeavy = true;
                rightIsHeavy = false;
            }
            // If the weight is equal,
            else if (leftWeight == rightWeight && leftIsHeavy ||
                     leftWeight == rightWeight && rightIsHeavy)
            {
                // If the left side was the heaviest,
                if (leftIsHeavy)
                {
                    // Calculate the middle position for the left hand by subtracting the left hand current position with the down position for the left hand
                    middleLeftPosition = leftScale.transform.position - leftScalePositions[2];

                    // Calculate the middle position for the right hand by subtracting the right hand current position with the up position for the right hand
                    middleRightPosition = rightScale.transform.position - rightScalePositions[0];
                }
                // If the right side was the heaviest,
                else if (rightIsHeavy)
                {
                    // Calculate the middle position for the left hand by subtracting the left hand current position with the up position for the left hand
                    middleLeftPosition = leftScale.transform.position - leftScalePositions[0];

                    // Calculate the middle position for the right hand by subtracting the right hand current position with the down position for the right hand
                    middleRightPosition = rightScale.transform.position - rightScalePositions[2];
                }

                // Move the left hand of the scale to the middle position
                StartCoroutine(LerpPosition(middleLeftPosition, 5, leftScale));

                // Move the right hand of the scale to the middle position
                StartCoroutine(LerpPosition(middleRightPosition, 5, rightScale));

                // Rotate the arm to the middle of the scale
                StartCoroutine(LerpRotation(Quaternion.Euler(armRotations[1]), 5, arm));

                leftIsHeavy = false;
                rightIsHeavy = false;

                // Lock the scale so the player can't interact with scale
                scalePuzzleCompleted = true;

                // Play dialogue
                dialogueTrigger.TriggerDialogue();

                // Remove rock object from player's mouse
                if (rockGameObject != null)
                {
                    rockGameObject.GetComponent<Rigidbody>().isKinematic = false;
                    rockGameObject = null;
                }
            }
            // If the right side is the heaviest
            else if (leftWeight < rightWeight && !rightIsHeavy)
            {
                // Move the left hand to the top of the scale
                StartCoroutine(LerpPosition(lightLeftPosition, 5, leftScale));

                // Move the right hand to the bottom of the scale
                StartCoroutine(LerpPosition(heavyRightPosition, 5, rightScale));

                // Rotate the arm to the right of the scale
                StartCoroutine(LerpRotation(Quaternion.Euler(armRotations[2]), 5, arm));

                leftIsHeavy = false;
                rightIsHeavy = true;
            }
        }
    }

    // Move to target position over a time period
    IEnumerator LerpPosition(Vector3 targetPosition, float duration, GameObject hand)
    {
        // Set timer to 0 and get starting position
        float time = 0;
        Vector3 startPosition = hand.transform.position;

        // While timer is less than duration of movement
        while (time < duration)
        {
            // Move hand a small distance to target location
            hand.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);

            // Increase time by delta time
            time += Time.deltaTime;
            yield return null;
        }

        // When time is up, move hand to target position
        hand.transform.position = targetPosition;
    }

    // Rotate to target rotation over a certain time period
    IEnumerator LerpRotation(Quaternion endValue, float duration, GameObject arm)
    {
        // Set timer to 0 and get starting rotation
        float time = 0;
        Quaternion startValue = arm.transform.rotation;

        // While timer is less than duration of rotation
        while (time < duration)
        {
            // Rotate arm a small distance to target location
            arm.transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);

            // Increase time by delta time
            time += Time.deltaTime;
            yield return null;
        }

        // When time is up, rotate arm to target rotation
        arm.transform.rotation = endValue;
    }

    // When the collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buttonText.SetActive(true);
        }
    }

    // When the collider other has stopped touching the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buttonText.SetActive(false);
        }
    }

    // Change Cameras
    public void ChangeToMainCamera(bool enableMainCam)
    {
        if (!enableMainCam)
        {
            mainCam.enabled = false;
            scaleCamera.enabled = true;
        }
        else
        {
            mainCam.enabled = true;
            scaleCamera.enabled = false;
        }
    }

    // Action for when the player left clicks
    public void OnClick(InputAction.CallbackContext value)
    {
        // Get the mouse current position
        Vector3 pos = Mouse.current.position.ReadValue();

        // Shoot a ray
        Ray ray = scaleCamera.ScreenPointToRay(pos);

        RaycastHit hit;

        // If it hits a rock,
        if (Physics.Raycast(ray, out hit, 25))
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Rock"))
                {
                    // The rock then follows the player's cursor 
                    rockGameObject = hit.collider.gameObject;
                }
            }
        }
    }

    // Action for when the player right-clicks
    public void OnDeselect(InputAction.CallbackContext value)
    {
        // If the player is holding a rock,
        if (rockGameObject != null)
        {
            // Let go of the rock
            rockGameObject.GetComponent<Rigidbody>().isKinematic = false;
            rockGameObject = null;
        }
    }
    #endregion
}
