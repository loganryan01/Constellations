using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleCameraBehaviour : MonoBehaviour
{
    #region Fields
    public Camera mainCamera; // Main camera for the scene

    public Transform mazePuzzleTransform; // Transform for the camera to be at when the player is solving the maze puzzle
    public Transform scalePuzzleTransform; // Transform for the camera to be at when the player is solving the scale puzzle
    private Transform originalTransform; // Transform for the camera where the player interacted with the puzzle

    public PlayerController playerController; // The controller for the player
    public DialogueManager dialogueManager; // Manager script for the dialogue in the camera

    public UnityEvent onReturnToPlayer;

    private bool cameraInPuzzlePosition = false; // Is the camera in the puzzle position
    private bool cameraInPlayerPosition = true; // Is the camera in the player position
    #endregion

    #region Functions
    // Start function
    private void Start()
    {
        gameObject.GetComponent<Camera>().enabled = false;
    }

    // Update function - run every frame
    void Update()
    {
        if (cameraInPlayerPosition)
        {
            originalTransform = mainCamera.transform;
            transform.position = mainCamera.transform.position;
            transform.rotation = mainCamera.transform.rotation;
        }
        
        // If the player is solving the scale puzzle
        //if (playerController.scaleBehaviour != null)
        //{
        //    // When player interacts with scale puzzle, move the camera from player's position to maze puzzle camera position
        //    if (cameraInPlayerPosition && !playerController.scaleBehaviour.scalePuzzleCompleted)
        //    {
        //        MoveToPuzzlePosition(scalePuzzleTransform.position, scalePuzzleTransform.rotation);
        //    }
        //}

        // If the player is solving the maze puzzle
        if (playerController.mazeBehaviour != null)
        {
            // When player interacts with maze puzzle, move the camera from player's position to maze puzzle camera position
            if (cameraInPlayerPosition && !playerController.mazeBehaviour.mazeCompleted)
            {
                MoveToPuzzlePosition(mazePuzzleTransform.position, mazePuzzleTransform.rotation);
            }
        }
    }

    public void MoveToPuzzlePosition(Vector3 position, Quaternion rotation)
    {
        StartCoroutine(LerpPositionAndRotation(position, rotation, 5, 0));
    }

    public void MoveToPlayerPosition()
    {
        StartCoroutine(LerpPositionAndRotation(originalTransform.position, originalTransform.rotation, 5, 1));
    }

    IEnumerator LerpPositionAndRotation(Vector3 targetPosition, Quaternion targetRotation, float duration, int puzzleCase)
    {
        // Case 0 = Move to puzzle position
        // Case 1 = Move to player position
        if (puzzleCase == 0)
        {
            cameraInPlayerPosition = false;
        }
        else if (puzzleCase == 1)
        {
            cameraInPuzzlePosition = false;
        }

        // Start timer
        float time = 0;

        // Get starting position
        Vector3 startPosition = transform.position;

        // Get the starting rotation
        Quaternion startValue = transform.rotation;

        // While timer is not at duration
        while (time < duration)
        {
            // Move towards target position
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);

            // Rotate towards target rotation
            transform.rotation = Quaternion.Lerp(startValue, targetRotation, time / duration);

            // Increase timer by delta time
            time += Time.deltaTime;
            yield return null;
        }

        // When the timer is up, set position to target position
        transform.position = targetPosition;

        // When the timer is up, set rotation to target rotation
        transform.rotation = targetRotation;

        // If camera is moving from player to puzzle,
        if (puzzleCase == 0)
        {
            // Then the camera is in puzzle position
            cameraInPuzzlePosition = true;
        }
        // If camera is moving from puzzle to player,
        else if (puzzleCase == 1)
        {
            // Then the camera is in player position
            cameraInPlayerPosition = true;

            onReturnToPlayer.Invoke();
        }
    }
    #endregion
}
