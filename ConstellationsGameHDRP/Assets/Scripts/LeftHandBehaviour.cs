/*-------------------------------------------------------------------
    Name: LeftHandBehaviour
    Purpose: Checks how much weight is on the left hand of the scale.
    Authour: Logan Ryan
    Modified: 7 October 2021
---------------------------------------------------------------------
    Copyright 2021 Bookshelf Studios
-------------------------------------------------------------------*/
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LeftHandBehaviour : MonoBehaviour
{
    #region Fields
    // The main scale script
    ScaleBehaviour scaleBehaviour;
    //bool coroutinePlaying = false;

    public Transform stoneEntryPoint;
    public UnityEvent onArrivalToEntryPoint;
    #endregion

    #region Functions
    // Start function
    void Start()
    {
        scaleBehaviour = GetComponentInParent<ScaleBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the rock has been placed on the left hand of the scale, add the weight to the left hand
        if (other.gameObject.CompareTag("Rock") && other.gameObject.transform.parent != transform)
        {
            scaleBehaviour.leftWeight += other.gameObject.GetComponent<Rigidbody>().mass;

            other.gameObject.transform.parent = transform;
            other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            StartCoroutine(LerpPosition(stoneEntryPoint.position, 5, other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the rock has been remove from the left hand of the scale, remove the weight from the left hand
        if (other.gameObject.CompareTag("Rock") && !scaleBehaviour.scalePuzzleCompleted/*!coroutinePlaying*/)
        {
            Debug.Log(other.name + " has been removed from the left hand");
            scaleBehaviour.leftWeight -= other.gameObject.GetComponent<Rigidbody>().mass;
            scaleBehaviour.UpdateScale();

            other.gameObject.transform.parent = null;
        }
    }

    // Move to target position over a time period
    IEnumerator LerpPosition(Vector3 targetPosition, float duration, GameObject hand)
    {
        //coroutinePlaying = true;

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

        onArrivalToEntryPoint.Invoke();

        //coroutinePlaying = false;
    }
    #endregion
}
