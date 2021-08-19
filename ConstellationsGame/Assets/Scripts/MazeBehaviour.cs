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

    [HideInInspector]
    public bool mazeCompleted;

    private float movementX;
    private float movementZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();

        if (transform.rotation.eulerAngles.x > maxRotation)
        {
            Debug.Log("X is greater than max rotation");
            //transform.rotation = Quaternion.Euler(maxRotation, transform.rotation.y, transform.rotation.z);
        }

        if (transform.rotation.eulerAngles.x < -maxRotation)
        {
            Debug.Log("X is less than max rotation");
            //transform.rotation = Quaternion.Euler(-maxRotation, transform.rotation.y, transform.rotation.z);
        }

        //if (transform.rotation.eulerAngles.z > maxRotation)
        //{
        //    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, maxRotation);
        //}

        //if (transform.rotation.eulerAngles.z < -maxRotation)
        //{
        //    //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -maxRotation);
        //}

        if (ballRigidbody.IsSleeping())
        {
            ballRigidbody.WakeUp();
        }

        if (mazeBallBehaviour.touchedEnd)
        {
            mazeCompleted = true;
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
}
