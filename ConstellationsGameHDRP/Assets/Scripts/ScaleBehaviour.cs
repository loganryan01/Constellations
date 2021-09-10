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
    
    [Header("Interact Text")]
    [SerializeField]
    public GameObject buttonText;

    [Header("Scale Settings")]
    public GameObject leftScale;
    public GameObject rightScale;
    public GameObject arm;
    public GameObject leftDoor;
    public GameObject rightDoor;

    public Vector3[] leftScalePositions;
    public Vector3[] rightScalePositions;
    public Vector3[] armRotations;
    public Vector3[] doorRotations;

    [Header("Dialogue Settings")]
    public DialogueManager dialogueManager;

    //[HideInInspector]
    public float leftWeight;
    //[HideInInspector]
    public float rightWeight;

    [Header("Start settings")]
    public Side startingSide;
    [HideInInspector]
    public bool scalePuzzleCompleted = false;

    private GameObject rockGameObject;

    private bool leftIsHeavy = false;
    private bool rightIsHeavy = false;

    private Vector3 heavyLeftPosition;
    private Vector3 heavyRightPosition;

    private Vector3 middleLeftPosition;
    private Vector3 middleRightPosition;

    private Vector3 lightLeftPosition;
    private Vector3 lightRightPosition;

    private Camera mainCam;
    private Camera scaleCamera;

    private DialogueTrigger dialogueTrigger;

    // Start is called before the first frame update
    void Start()
    {
        buttonText.SetActive(false);

        scaleCamera = GameObject.Find("PuzzleCamera").GetComponent<Camera>();
        scaleCamera.enabled = false;

        mainCam = Camera.main;

        heavyLeftPosition = leftScale.transform.position + leftScalePositions[2];
        heavyRightPosition = rightScale.transform.position + rightScalePositions[2];

        lightLeftPosition = leftScale.transform.position + leftScalePositions[0];
        lightRightPosition = rightScale.transform.position + rightScalePositions[0];

        if (startingSide == Side.Left)
        {
            leftScale.transform.position = heavyLeftPosition;
            rightScale.transform.position = lightRightPosition;
            arm.transform.rotation = Quaternion.Euler(armRotations[0]);
            leftIsHeavy = true;
        }
        else if (startingSide == Side.Right)
        {
            leftScale.transform.position = lightLeftPosition;
            rightScale.transform.position = heavyRightPosition;
            arm.transform.rotation = Quaternion.Euler(armRotations[2]);
            rightIsHeavy = true;
        }

        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    // Update is called once per frame
    void Update()
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

        // When the player clicks the rock, make the rock follow the mouse
        if (rockGameObject != null)
        {
            Cursor.lockState = CursorLockMode.None;

            Camera scaleCamera = GameObject.Find("PuzzleCamera").GetComponent<Camera>();

            // Screen Position
            Vector3 mousePosition = Mouse.current.position.ReadValue();

            // World Position
            Vector3 rockPosition = scaleCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 5));

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

    IEnumerator LerpPosition(Vector3 targetPosition, float duration, GameObject hand)
    {
        float time = 0;
        Vector3 startPosition = hand.transform.position;

        while (time < duration)
        {
            hand.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        hand.transform.position = targetPosition;
    }

    IEnumerator LerpRotation(Quaternion endValue, float duration, GameObject arm)
    {
        float time = 0;
        Quaternion startValue = arm.transform.rotation;

        while (time < duration)
        {
            arm.transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        arm.transform.rotation = endValue;
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

    public void OnClick(InputAction.CallbackContext value)
    {
        Camera scaleCamera = GameObject.Find("PuzzleCamera").GetComponent<Camera>();

        Vector3 pos = Mouse.current.position.ReadValue();

        Ray ray = scaleCamera.ScreenPointToRay(pos);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 25))
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Rock"))
                {
                    rockGameObject = hit.collider.gameObject;
                }
            }
        }
    }

    public void OnDeselect(InputAction.CallbackContext value)
    {
        if (rockGameObject != null)
        {
            rockGameObject.GetComponent<Rigidbody>().isKinematic = false;
            rockGameObject = null;
        }
    }
}
