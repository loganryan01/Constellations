using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCameraBehaviour : MonoBehaviour
{
    public Transform mazePuzzleTransform;
    public Transform scalePuzzleTransform;
    private Transform originalTransform;

    public ScaleBehaviour scaleBehaviour;
    public MazeBehaviour mazeBehaviour;

    private bool cameraInPuzzlePosition = false;
    private bool cameraInPlayerPosition = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If player has not interacted with a puzzle yet, the camera should just follow the player position
        if (Camera.main != null && cameraInPlayerPosition && !cameraInPuzzlePosition)
        {
            originalTransform = Camera.main.transform;
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;
        }
        
        // If the maze does exist
        if (mazeBehaviour != null)
        {
            // When player interacts with maze puzzle, move the camera from player's position to maze puzzle camera position
            if (Camera.main == null && cameraInPlayerPosition && !mazeBehaviour.mazeCompleted)
            {
                StartCoroutine(LerpPosition(mazePuzzleTransform.position, 5, 0));
                StartCoroutine(LerpRotation(mazePuzzleTransform.rotation, 5, 0));
            }
            else if (Camera.main == null && cameraInPuzzlePosition && mazeBehaviour.mazeCompleted)
            {
                StartCoroutine(LerpPosition(originalTransform.position, 5, 1));
                StartCoroutine(LerpRotation(originalTransform.rotation, 5, 1));
            }

            // Change to main camera when the lerp has finished
            if (cameraInPlayerPosition && mazeBehaviour.mazeCompleted)
            {
                mazeBehaviour.ChangeToMainCamera(true);
            }
        }

        if (scaleBehaviour != null)
        {
            // When player interacts with scale puzzle, move the camera from player's position to maze puzzle camera position
            if (Camera.main == null && cameraInPlayerPosition && !scaleBehaviour.lockScale)
            {
                StartCoroutine(LerpPosition(scalePuzzleTransform.position, 5, 0));
                StartCoroutine(LerpRotation(scalePuzzleTransform.rotation, 5, 0));
            }
            else if (Camera.main == null && cameraInPuzzlePosition && scaleBehaviour.lockScale)
            {
                StartCoroutine(LerpPosition(originalTransform.position, 5, 1));
                StartCoroutine(LerpRotation(originalTransform.rotation, 5, 1));
            }


            if (cameraInPlayerPosition && scaleBehaviour.lockScale)
            {
                scaleBehaviour.ChangeToMainCamera(true);
            }
        }
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration, int puzzleCase)
    {
        // Case 0 = Move to puzzle position
        if (puzzleCase == 0)
        {
            cameraInPlayerPosition = false;
        }
        else if (puzzleCase == 1)
        {
            cameraInPuzzlePosition = false;
        }

        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        if (puzzleCase == 0)
        {
            cameraInPuzzlePosition = true;
        }
        else if (puzzleCase == 1)
        {
            cameraInPlayerPosition = true;
        }
    }

    IEnumerator LerpRotation(Quaternion endValue, float duration, int puzzleCase)
    {
        // Case 0 = Move to puzzle position
        if (puzzleCase == 0)
        {
            cameraInPlayerPosition = false;
        }
        else if (puzzleCase == 1)
        {
            cameraInPuzzlePosition = false;
        }
        
        float time = 0;
        Quaternion startValue = transform.rotation;

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endValue;

        if (puzzleCase == 0)
        {
            cameraInPuzzlePosition = true;
        }
        else if (puzzleCase == 1)
        {
            cameraInPlayerPosition = true;
        }
    }
}
