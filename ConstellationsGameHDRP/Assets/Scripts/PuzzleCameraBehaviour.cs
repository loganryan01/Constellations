using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCameraBehaviour : MonoBehaviour
{
    #region Fields
    public Transform mazePuzzleTransform; // Transform for the camera to be at when the player is solving the maze puzzle
    public Transform scalePuzzleTransform; // Transform for the camera to be at when the player is solving the scale puzzle
    private Transform originalTransform; // Transform for the camera where the player interacted with the puzzle

    public PlayerController playerController; // The controller for the player
    public DialogueManager dialogueManager; // Manager script for the dialogue in the camera

    private bool cameraInPuzzlePosition = false; // Is the camera in the puzzle position
    private bool cameraInPlayerPosition = true; // Is the camera in the player position
    #endregion

    #region Functions
    // Update function - run every frame
    void Update()
    {
        // If player has not interacted with a puzzle yet, the camera should just follow the player position
        if (Camera.main != null && cameraInPlayerPosition && !cameraInPuzzlePosition)
        {
            originalTransform = Camera.main.transform;
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;
        }

        // If the player is solving the scale puzzle
        if (playerController.scaleBehaviour != null)
        {
            // When player interacts with scale puzzle, move the camera from player's position to maze puzzle camera position
            if (Camera.main == null && cameraInPlayerPosition && !playerController.scaleBehaviour.scalePuzzleCompleted)
            {
                StartCoroutine(LerpPosition(scalePuzzleTransform.position, 5, 0));
                StartCoroutine(LerpRotation(scalePuzzleTransform.rotation, 5, 0));
            }
            else if (Camera.main == null && cameraInPuzzlePosition && playerController.scaleBehaviour.scalePuzzleCompleted && dialogueManager.dialogueEnded)
            {
                StartCoroutine(LerpPosition(originalTransform.position, 5, 1));
                StartCoroutine(LerpRotation(originalTransform.rotation, 5, 1));
            }

            // If the player has solved the scale puzzle, and the dialogue has finished
            if (cameraInPlayerPosition && playerController.scaleBehaviour.scalePuzzleCompleted && dialogueManager.dialogueEnded)
            {
                playerController.scaleBehaviour.ChangeToMainCamera(true);
            }
        }

        // If the player is solving the maze puzzle
        if (playerController.mazeBehaviour != null)
        {
            // When player interacts with maze puzzle, move the camera from player's position to maze puzzle camera position
            if (Camera.main == null && cameraInPlayerPosition && !playerController.mazeBehaviour.mazeCompleted)
            {
                StartCoroutine(LerpPosition(mazePuzzleTransform.position, 5, 0));
                StartCoroutine(LerpRotation(mazePuzzleTransform.rotation, 5, 0));
            }
            else if (Camera.main == null && cameraInPuzzlePosition && playerController.mazeBehaviour.mazeCompleted && dialogueManager.dialogueEnded)
            {
                StartCoroutine(LerpPosition(originalTransform.position, 5, 1));
                StartCoroutine(LerpRotation(originalTransform.rotation, 5, 1));
            }

            // If the player has solved the maze puzzle, and the dialogue has finished
            if (cameraInPlayerPosition && playerController.mazeBehaviour.mazeCompleted && dialogueManager.dialogueEnded)
            {
                playerController.mazeBehaviour.ChangeToMainCamera(true);
            }
        }
    }

    // Move to target position over a time period
    IEnumerator LerpPosition(Vector3 targetPosition, float duration, int puzzleCase)
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

        // While timer is not at duration
        while (time < duration)
        {
            // Move towards target position
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);

            // Increase timer by delta time
            time += Time.deltaTime;
            yield return null;
        }

        // When the timer is up, set position to target position
        transform.position = targetPosition;

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
        }
    }

    // Rotate to target object over period of time 
    IEnumerator LerpRotation(Quaternion endValue, float duration, int puzzleCase)
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

        // Get the starting rotation
        Quaternion startValue = transform.rotation;

        // While timer is not at duration
        while (time < duration)
        {
            // Rotate towards target rotation
            transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);

            // Increase timer by delta time
            time += Time.deltaTime;
            yield return null;
        }

        // When the timer is up, set rotation to target rotation
        transform.rotation = endValue;

        // If camera is rotating from player to puzzle,
        if (puzzleCase == 0)
        {
            // Then the camera is in puzzle position
            cameraInPuzzlePosition = true;
        }
        // If camera is rotating from puzzle to player,
        else if (puzzleCase == 1)
        {
            // Then the camera is in player position
            cameraInPlayerPosition = true;
        }
    }
    #endregion
}
