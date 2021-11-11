/*----------------------------------------------------
    Name: StoneBehaviour
    Purpose: Controls the stones for the scale puzzle.
    Authour: Logan Ryan
    Modified: 11 November 2021
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
    public Transform rockPositionOnScale;
    public UnityEvent onInteraction;
    public UnityEvent onReturn;

    private bool inCorrectPosition = false;
    private bool isPlayerHoldingRock = false;
    private Rigidbody stoneRigidbody;
    private Transform holdingPosition;
    #endregion

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        stoneRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerHoldingRock)
        {
            gameObject.transform.position = holdingPosition.position;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    //Debug.Log("Collided with: " + other.name);

    //    if (other.name == "Libra_Scales_Puzzle")
    //    {
    //        Debug.Log(name + "collided with: " + other.name);

            
    //    }
    //}

    public void InteractWithScale()
    {
        isPlayerHoldingRock = false;

        holdingPosition = null;

        StartCoroutine(LerpPosition(rockPositionOnScale.position, 5, gameObject));
    }

    public bool IsStoneInCorrectPosition()
    {
        return inCorrectPosition;
    }

    public void MoveStoneToCorrectPosition(Transform newTransform)
    {
        stoneRigidbody.isKinematic = true;

        isPlayerHoldingRock = true;

        holdingPosition = newTransform;
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
