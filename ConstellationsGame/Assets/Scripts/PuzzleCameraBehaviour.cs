using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCameraBehaviour : MonoBehaviour
{
    public Transform mazePuzzleTransform;
    private Transform originalTransform;

    public ScaleBehaviour scaleBehaviour;
    public MazeBehaviour mazeBehaviour;

    private bool cameraInPuzzlePosition = false;
    private bool cameraInPlayerPosition = true;

    private bool lerpPositionActive = false;
    private bool lerpRotationActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If player has not interacted with a puzzle yet, the camera should just follow the player position
        if (Camera.main == null && cameraInPlayerPosition && !lerpPositionActive && !lerpRotationActive)
        {
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;
            originalTransform = transform;
        }
        
        // When player interacts with maze puzzle, move the camera from player's position to maze puzzle camera position
        if (Camera.main == null && cameraInPlayerPosition && !mazeBehaviour.mazeCompleted)
        {
            cameraInPlayerPosition = false;
            
            StartCoroutine(LerpPosition(mazePuzzleTransform.position, 5));
            StartCoroutine(LerpRotation(mazePuzzleTransform.rotation, 5));

            cameraInPuzzlePosition = true;
        }
        else if (Camera.main == null && cameraInPuzzlePosition && mazeBehaviour.mazeCompleted)
        {
            cameraInPuzzlePosition = false;

            StartCoroutine(LerpPosition(originalTransform.position, 5));
            StartCoroutine(LerpRotation(originalTransform.rotation, 5));

            cameraInPlayerPosition = true;
        }

        // Change priority of main camera until the lerp is complete
        if (!lerpPositionActive && !lerpRotationActive && mazeBehaviour.mazeCompleted)
        {
            mazeBehaviour.ChangeToMainCamera(true);
        }
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        lerpPositionActive = true;

        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        lerpPositionActive = false;
    }

    IEnumerator LerpRotation(Quaternion endValue, float duration)
    {
        lerpRotationActive = true;
        
        float time = 0;
        Quaternion startValue = transform.rotation;

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endValue;

        lerpRotationActive = false;
    }
}
