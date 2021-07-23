using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Transform mainCam;
    private PlayerInputActions playerInput;
    private CharacterController controller;

    private Vector3 rawInputMovement;
    private Vector2 rawInputLook;
    private float xRotation = 0f;

    private void Start()
    {
        mainCam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInput.PlayerController.Enable();
    }

    private void OnDisable()
    {
        playerInput.PlayerController.Disable();
    }

    private void Update()
    {
        PlayerMovement();
        PlayerLook();
        PlayerInteract();
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputVector = value.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputVector.x, 0, inputVector.y);
    }

    public void OnLook(InputAction.CallbackContext value)
    {
        Vector2 mouseVector = value.ReadValue<Vector2>();
        rawInputLook = mouseVector;
    }

    public void OnInteract(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            // Do Interactions
        }
    }

    private void PlayerMovement()
    {
        float movementX = rawInputMovement.x;
        float movementZ = rawInputMovement.z;

        Vector3 move = transform.right * movementX + transform.forward * movementZ;
        controller.Move(move * 5.0f * Time.deltaTime);

        Debug.Log("Move");
    }

    private void PlayerLook()
    {
        float mouseX = rawInputLook.x * 60f * Time.deltaTime;
        float mouseY = rawInputLook.y * 60f * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -50f, 50f);

        mainCam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        Debug.Log("Look Around");
    }

    private void PlayerInteract()
    {
        Debug.Log("Interact");
    }
}
