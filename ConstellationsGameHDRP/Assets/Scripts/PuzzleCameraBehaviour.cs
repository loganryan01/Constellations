/*---------------------------------------------------------------
    Name: PuzzleCameraBehaviour
    Purpose: Controls the transform of the camera for the puzzle.
    Authour: Logan Ryan
    Modified: 7 October 2021
-----------------------------------------------------------------
    Copyright 2021 Bookshelf Studios
---------------------------------------------------------------*/
using System.Collections;
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
    #endregion

    #region Functions
    // Start function
    private void Start()
    {
        gameObject.GetComponent<Camera>().enabled = false;
    }

    public void MoveToPuzzlePosition(Transform puzzleTransform)
    {
        originalTransform = mainCamera.transform;
        transform.position = mainCamera.transform.position;
        transform.rotation = mainCamera.transform.rotation;

        StartCoroutine(LerpPositionAndRotation(puzzleTransform.position, puzzleTransform.rotation, 5, 0));
    }

    public void MoveToPlayerPosition()
    {
        StartCoroutine(LerpPositionAndRotation(originalTransform.position, originalTransform.rotation, 5, 1));
    }

    // Change Cameras
    public void ChangeToMainCamera(bool enableMainCam)
    {
        if (!enableMainCam)
        {
            mainCamera.enabled = false;
            GetComponent<Camera>().enabled = true;
        }
        else
        {
            mainCamera.enabled = true;
            GetComponent<Camera>().enabled = false;
        }
    }

    IEnumerator LerpPositionAndRotation(Vector3 targetPosition, Quaternion targetRotation, float duration, int puzzleCase)
    {
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

        if (puzzleCase == 1)
        {
            onReturnToPlayer.Invoke();
        }
    }
    #endregion
}
