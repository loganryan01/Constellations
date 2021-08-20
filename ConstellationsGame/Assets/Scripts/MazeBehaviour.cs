using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MazeBehaviour : MonoBehaviour
{
    public float rotateSpeed;
    public Camera mainCam;
    public Camera mazeCamera;
    public Rigidbody ballRigidbody;
    public MazeBallBehaviour mazeBallBehaviour;
    public float maxRotation;
    public GameObject buttonText;

    [HideInInspector]
    public bool mazeCompleted;
    [HideInInspector]
    public bool dialogueEnded;

    private float movementX;
    private float movementZ;
    private float angleX;
    private float angleZ;
    private DialogueTrigger dialogueTrigger;

    // Start is called before the first frame update
    void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mazeBallBehaviour.touchedEnd)
        {
            Rotate();

            if (transform.rotation.eulerAngles.x < maxRotation || transform.rotation.eulerAngles.x > 360 - maxRotation)
            {
                angleX = transform.rotation.eulerAngles.x;
            }

            if (transform.rotation.eulerAngles.z < maxRotation || transform.rotation.eulerAngles.z > 360 - maxRotation)
            {
                angleZ = transform.rotation.eulerAngles.z;
            }

            gameObject.transform.eulerAngles = new Vector3(angleX, 0, angleZ);

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

        movementX = -inputVector.x;
        movementZ = inputVector.y;
    }

    private void Rotate()
    {
        Vector3 rotate = gameObject.transform.forward * movementX + gameObject.transform.right * movementZ;
        gameObject.transform.Rotate(rotate * rotateSpeed * Time.deltaTime);
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
        buttonText.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        buttonText.SetActive(false);
    }
}
