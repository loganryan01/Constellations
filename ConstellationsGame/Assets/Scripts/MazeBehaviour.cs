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

        //ballRigidbody.AddForce(ballRigidbody.velocity.normalized * 3.0f);
        if (ballRigidbody.IsSleeping())
        {
            ballRigidbody.WakeUp();
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
        Vector3 rotate = transform.forward * movementX + transform.right * movementZ;
        transform.Rotate(rotate * rotateSpeed * Time.deltaTime);
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
