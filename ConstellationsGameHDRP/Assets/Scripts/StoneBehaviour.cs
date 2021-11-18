/*----------------------------------------------------
    Name: StoneBehaviour
    Purpose: Controls the stones for the scale puzzle.
    Authour: Logan Ryan
    Modified: 18 November 2021
------------------------------------------------------
    Copyright 2021 Bookshelf Studios
----------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoneBehaviour : MonoBehaviour
{
    #region Fields
    [Header("Interaction Settings")]
    public Transform rockPositionOnScale; // The position of the rock on the scale
    public UnityEvent onInteraction; // Events to be triggered when the player interacts with the stone
    public UnityEvent onReturn; // Events to be triggered when the player returns the stone to the scale

    private bool inCorrectPosition = false; // Is the stone in the correct position
    private bool isPlayerHoldingRock = false; // Is the player currently holding the stone
    private Rigidbody stoneRigidbody; // Rigidbody of the stone
    private Transform holdingPosition; // The position where the player holds the stone
    #endregion

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        stoneRigidbody = GetComponent<Rigidbody>();
    }

    // When the player interacts with the scale while holding the stone
    public void InteractWithScale()
    {
        // Move the stone from the player's hand to the position on the scale
        isPlayerHoldingRock = false;

        holdingPosition = null;

        StartCoroutine(LerpPosition(rockPositionOnScale.position, 5, gameObject));
    }

    // Check if the stone is in the correct position
    public bool IsStoneInCorrectPosition()
    {
        return inCorrectPosition;
    }

    // Move stone to the player's hand
    public void MoveStoneToCorrectPosition(Transform newTransform)
    {
        transform.position = newTransform.position;
        transform.parent = newTransform;
    }

    // Move to target position over a time period
    IEnumerator LerpPosition(Vector3 targetPosition, float duration, GameObject stone)
    {
        // Set timer to 0 and get starting position
        float time = 0;
        Vector3 startPosition = stone.transform.position;

        // While timer is less than duration of movement
        while (time < duration)
        {
            // Move hand a small distance to target location
            stone.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);

            // Increase time by delta time
            time += Time.deltaTime;
            yield return null;
        }

        // When time is up, move hand to target position
        stone.transform.position = targetPosition;

        onReturn.Invoke();
    }
    #endregion
}
