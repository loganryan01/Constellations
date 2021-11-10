﻿/*--------------------------------------------------------------
    Name: RightHandBehaviour
    Purpose: Measures the weight in the right hand of the scale.
    Authour: Logan Ryan
    Modified: 7 October 2021
----------------------------------------------------------------
    Copyright 2021 Bookshelf Studios
--------------------------------------------------------------*/
using System.Collections;
using UnityEngine;

public class RightHandBehaviour : MonoBehaviour
{
    #region Fields
    // The main scale script
    ScaleBehaviour scaleBehaviour;
    public Transform stoneEntryPoint;
    #endregion

    #region Functions
    // Start function
    void Start()
    {
        scaleBehaviour = GetComponentInParent<ScaleBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the rock has been placed on the right hand of the scale, add the weight to the right hand
        if (other.gameObject.CompareTag("Rock") && other.gameObject.transform.parent != transform)
        {
            //other.gameObject.transform.position = gameObject.transform.position;

            scaleBehaviour.rightWeight += other.gameObject.GetComponent<Rigidbody>().mass;

            other.gameObject.transform.parent = transform;
            scaleBehaviour.ReleaseRock(true);

            StartCoroutine(LerpPosition(stoneEntryPoint.position, 5, other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the rock has been remove from the right hand of the scale, remove the weight from the right hand
        if (other.gameObject.CompareTag("Rock"))
        {
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
            hand.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);

            // Increase time by delta time
            time += Time.deltaTime;
            yield return null;
        }

        // When time is up, move hand to target position
        hand.transform.position = targetPosition;

        scaleBehaviour.UpdateScale();
    }
    #endregion
}
