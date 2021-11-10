/*-------------------------------------
    Name: ScaleBehaviour
    Purpose: Controls the scale puzzle.
    Authour: Logan Ryan
    Modified: 27 October 2021
---------------------------------------
    Copyright 2021 Bookshelf Studios
-------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class PuzzleInteractionEvent : UnityEvent<int, Transform>
{

}

public class ScaleBehaviour : MonoBehaviour
{
    public enum Side
    {
        Left,
        Right,
        None
    }

    #region Fields
    [Header("Scale Settings")]
    public Camera mainCamera; // The main camera of the scene
    public Camera puzzleCamera; // The camera for the puzzles

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

    [Header("Interaction Settings")]
    public UnityEvent onInteraction;

    [Header("Puzzle Completed Settings")]
    public UnityEvent onComplete;

    [Header("Quit Settings")]
    public UnityEvent onQuit;

    [HideInInspector]
    public bool scalePuzzleCompleted = false; // Is the scale puzzle completed

    private GameObject rockGameObject; // The rock that the player is holding
    private Rigidbody rockGameObjectRigidbody; // The rigidbody of the rock game object the player is holding
    private GameObject lastTouchedRock; // The rock that was last touched

    private Vector3 heavyLeftPosition; // The position where the left hand is the heaviest
    private Vector3 heavyRightPosition; // The position where the right hand is the heaviest

    private Vector3 middleLeftPosition; // The position where the left hand is in the middle
    private Vector3 middleRightPosition; // The position where the right hand is in the middle

    private Vector3 lightLeftPosition; // The position where the left hand is the lightest
    private Vector3 lightRightPosition; // The position where the right hand is the lightest

    private Vector3[] rockStartingPositions = new Vector3[3];
    List<GameObject> rockGameObjects = new List<GameObject>();
    GameObject[] rockArray = new GameObject[4];
    #endregion

    #region Functions
    // Start function
    void Start()
    {
        // Calculate the position when the hand is the heaviest
        heavyLeftPosition = leftScale.transform.position + leftScalePositions[2];
        heavyRightPosition = rightScale.transform.position + rightScalePositions[2];

        // Get the middle position
        middleLeftPosition = leftScale.transform.position;
        middleRightPosition = rightScale.transform.position;

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
        }

        // Setup rock default positions
        rockArray = GameObject.FindGameObjectsWithTag("Rock");

        for (int i = 0; i < rockArray.Length; i++)
        {
            if (rockArray[i].layer == 0)
            {
                rockGameObjects.Add(rockArray[i]);
            }
        }

        UpdateRockPositions();
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
            Vector3 rockPosition = puzzleCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 4.25f));

            // Set the position of the rock to be the mouse position
            rockGameObject.transform.position = rockPosition;

            rockGameObject.layer = 0;
        }
        else if (rockGameObject == null)
        {
            OutlineRock();
        }
    }

    // Update the scale positions and rotations
    public void UpdateScale()
    {
        ReleaseRock(false);
        
        // If the puzzle is not completed,
        if (!scalePuzzleCompleted)
        {
            // If the weight on the left side of the scale is heavier than the right side,            
            if (leftWeight > rightWeight)
            {
                ChangeScale(heavyLeftPosition, lightRightPosition, armRotations[0]);
            }
            // If the weight is equal,
            else if (leftWeight == rightWeight)
            {
                ChangeScale(middleLeftPosition, middleRightPosition, armRotations[1]);

                // ===== Puzzle Complete Function =====
                if (leftWeight + rightWeight > 0)
                {
                    // Remove rock object from player's mouse
                    if (rockGameObject != null)
                    {
                        Debug.Log("Removing Rock");
                        rockGameObject.GetComponent<Rigidbody>().isKinematic = false;
                        rockGameObject = null;
                    }

                    onComplete.Invoke();

                    // Lock the scale so the player can't interact with scale
                    scalePuzzleCompleted = true;
                }
            }
            // If the right side is the heaviest
            else if (leftWeight < rightWeight)
            {
                ChangeScale(lightLeftPosition, heavyRightPosition, armRotations[2]);
            }

            
        }
    }

    // Open the doors when the puzzle is completed
    public void OpenDoors()
    {
        StartCoroutine(LerpRotation(Quaternion.Euler(doorRotations[0]), 5, leftDoor));
        StartCoroutine(LerpRotation(Quaternion.Euler(doorRotations[1]), 5, rightDoor));
    }

    // Change the material from a statue to galaxy
    public void IncreaseGalaxyIntensity(Transform gameObjectTransform)
    {
        // Get all the mesh renderers in the scale
        for (int i = 0; i < gameObjectTransform.childCount; i++)
        {
            MeshRenderer meshRenderer = gameObjectTransform.GetChild(i).GetComponent<MeshRenderer>();
            
            if (meshRenderer != null && meshRenderer.material.name == "M_Scales (Instance)")
            {
                StartCoroutine(LerpFloat(2, 5, meshRenderer));
            }

            // Check if the game object has a child object
            if (gameObjectTransform.childCount > 0)
            {
                IncreaseGalaxyIntensity(gameObjectTransform.GetChild(i));
            }
        }
    }

    public void DisplayControls(GameObject controls)
    {
        controls.SetActive(!controls.activeInHierarchy);
    }

    public void ReleaseRock(bool enableKinematic)
    {
        // Remove rock object from player's mouse
        if (rockGameObject != null)
        {
            Debug.Log("Removing Rock");
            rockGameObject.GetComponent<Rigidbody>().isKinematic = enableKinematic;
            rockGameObject = null;
        }
    }

    public void UpdateRockPositions()
    {
        for (int i = 0; i < rockGameObjects.Count; i++)
        {
            rockStartingPositions[i] = rockGameObjects[i].transform.position;
        }
    }

    // Outline the rock when the player hover the mouse over it
    private void OutlineRock()
    {
        // Get the mouse current position
        Vector3 pos = Mouse.current.position.ReadValue();

        // Shoot a ray
        Ray ray = puzzleCamera.ScreenPointToRay(pos);

        RaycastHit hit;

        // If it hits a rock,
        if (Physics.Raycast(ray, out hit, 27.5f))
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Rock") && rockGameObject == null)
                {
                    lastTouchedRock = hit.collider.gameObject;
                    lastTouchedRock.layer = 1;
                }
                else if (lastTouchedRock != null)
                {
                    lastTouchedRock.layer = 0;
                    lastTouchedRock = null;
                }
            }
        }
    }

    // Rotate the scale position
    private void ChangeScale(Vector3 leftHandPosition, Vector3 rightHandPosition, Vector3 fulcrumRotation)
    {
        // Move the left hand to the desired position
        StartCoroutine(LerpPosition(leftHandPosition, 5, leftScale));

        // Move the right hand to the desired position
        StartCoroutine(LerpPosition(rightHandPosition, 5, rightScale));

        // Rotate the arm to the desired rotation
        StartCoroutine(LerpRotation(Quaternion.Euler(fulcrumRotation), 5, arm));
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

    IEnumerator LerpFloat(float endValue, float duration, MeshRenderer meshRenderer)
    {
        float time = 0;
        float startValue = meshRenderer.material.GetFloat("_GalaxyIntensity");

        while (time < duration)
        {
            meshRenderer.material.SetFloat("_GalaxyIntensity", Mathf.Lerp(startValue, endValue, time / duration));

            time += Time.deltaTime;
            yield return null;
        }

        meshRenderer.material.SetFloat("_GalaxyIntensity", endValue);
    }

    // Action for when the player left clicks
    public void OnClick(InputAction.CallbackContext value)
    {
        // Get the mouse current position
        Vector3 pos = Mouse.current.position.ReadValue();

        // Shoot a ray
        Ray ray = puzzleCamera.ScreenPointToRay(pos);

        RaycastHit hit;

        // If it hits a rock,
        if (Physics.Raycast(ray, out hit, 27.5f))
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Rock") && rockGameObject == null)
                {
                    // The rock then follows the player's cursor 
                    rockGameObject = hit.collider.gameObject;
                    rockGameObjectRigidbody = rockGameObject.GetComponent<Rigidbody>();
                    rockGameObjectRigidbody.isKinematic = true;
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

    // Action for when the player wants to reset the puzzle
    public void ResetPuzzle(InputAction.CallbackContext value)
    {
        rockGameObject = null;
        
        for (int i = 0; i < rockGameObjects.Count; i++)
        {
            rockGameObjects[i].transform.position = rockStartingPositions[i];
        }
    }

    public void QuitPuzzle(InputAction.CallbackContext value)
    {
        onQuit.Invoke();
    }
    #endregion
}
