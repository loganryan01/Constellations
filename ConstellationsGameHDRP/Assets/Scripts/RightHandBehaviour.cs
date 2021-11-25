/*--------------------------------------------------------------
    Name: RightHandBehaviour
    Purpose: Measures the weight in the right hand of the scale.
    Authour: Logan Ryan
    Modified: 18 November 2021
----------------------------------------------------------------
    Copyright 2021 Bookshelf Studios
--------------------------------------------------------------*/
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RightHandBehaviour : MonoBehaviour
{
    #region Fields
    ScaleBehaviour scaleBehaviour; // The main scale script
    private int numberOfRocks; // Number of rocks moving to this hand

    public Transform stoneEntryPoint; // The resting place for the stones
    public UnityEvent onArrivalToEntryPoint; // Events to trigger when the stone has arrived at the designated position
    #endregion

    #region Functions
    // Start function
    void Start()
    {
        scaleBehaviour = GetComponentInParent<ScaleBehaviour>();
    }

    // OnTriggerEnter is called when the collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        // If the rock has been placed on the right hand of the scale, add the weight to the right hand
        if (other.gameObject.CompareTag("Rock") && other.gameObject.transform.parent != transform && !scaleBehaviour.mainCamera.enabled)
        {
            numberOfRocks++;

            scaleBehaviour.StopAllCoroutines();
            
            scaleBehaviour.rightWeight += other.gameObject.GetComponent<Rigidbody>().mass;

            other.gameObject.transform.parent = transform;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            // Move the rock to the correct position
            StartCoroutine(LerpPosition(stoneEntryPoint.position, 5, other.gameObject));
        }
    }

    // OnTriggerExit is called when the collider other has stopped touching the trigger
    private void OnTriggerExit(Collider other)
    {
        // If the rock has been remove from the right hand of the scale, remove the weight from the right hand
        if (other.gameObject.CompareTag("Rock") && !scaleBehaviour.scalePuzzleCompleted && !scaleBehaviour.mainCamera.enabled)
        {
            scaleBehaviour.StopAllCoroutines();

            scaleBehaviour.rightWeight -= other.gameObject.GetComponent<Rigidbody>().mass;
            scaleBehaviour.UpdateScale();

            other.gameObject.transform.parent = null;
        }
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
            hand.transform.position = Vector3.LerpUnclamped(startPosition, targetPosition, time / duration);

            // Increase time by delta time
            time += Time.deltaTime;
            yield return null;
        }

        // When time is up, move hand to target position
        hand.transform.position = targetPosition;

        numberOfRocks--;

        if (numberOfRocks == 0)
        {
            onArrivalToEntryPoint.Invoke();
        }
    }
    #endregion
}
