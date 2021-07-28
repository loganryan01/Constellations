﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBehaviour : MonoBehaviour
{
    public GameObject leftScale;
    public GameObject rightScale;
    public GameObject arm;

    public Vector3[] leftScalePositions;
    public Vector3[] rightScalePositions;
    public Vector3[] armRotations;

    [HideInInspector]
    public float leftWeight;
    [HideInInspector]
    public float rightWeight;

    public bool startOnLeft = false;
    public bool startOnRight = false;

    private bool leftMoving = false;
    private bool rightMoving = false;

    private Vector3 heavyLeftPosition;
    private Vector3 heavyRightPosition;

    private Vector3 middleLeftPosition;
    private Vector3 middleRightPosition;

    private Vector3 lightLeftPosition;
    private Vector3 lightRightPosition;

    // Start is called before the first frame update
    void Start()
    {
        heavyLeftPosition = leftScale.transform.position + leftScalePositions[2];
        heavyRightPosition = rightScale.transform.position + rightScalePositions[2];

        lightLeftPosition = leftScale.transform.position + leftScalePositions[0];
        lightRightPosition = rightScale.transform.position + rightScalePositions[0];

        if (startOnLeft)
        {
            leftScale.transform.position = heavyLeftPosition;
            rightScale.transform.position = lightRightPosition;
            arm.transform.rotation = Quaternion.Euler(armRotations[0]);
        }
        else if (startOnRight)
        {
            leftScale.transform.position = lightLeftPosition;
            rightScale.transform.position = heavyRightPosition;
            arm.transform.rotation = Quaternion.Euler(armRotations[2]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (leftWeight > rightWeight && !leftMoving)
        {
            StartCoroutine(LerpPosition(heavyLeftPosition, 5, leftScale));
            StartCoroutine(LerpPosition(lightRightPosition, 5, rightScale));
            StartCoroutine(LerpRotation(Quaternion.Euler(armRotations[0]), 5, arm));

            leftMoving = true;
            rightMoving = false;
        } 
        else if (leftWeight == rightWeight && leftMoving ||
            leftWeight == rightWeight && rightMoving)
        {
            if (leftMoving)
            {
                middleLeftPosition = leftScale.transform.position - leftScalePositions[2];
                middleRightPosition = rightScale.transform.position - rightScalePositions[0];
            }
            else if (rightMoving)
            {
                middleLeftPosition = leftScale.transform.position - leftScalePositions[0];
                middleRightPosition = rightScale.transform.position - rightScalePositions[2];
            }

            StartCoroutine(LerpPosition(middleLeftPosition, 5, leftScale));
            StartCoroutine(LerpPosition(middleRightPosition, 5, rightScale));
            StartCoroutine(LerpRotation(Quaternion.Euler(armRotations[1]), 5, arm));

            leftMoving = false;
            rightMoving = false;
        }
        else if (leftWeight < rightWeight && !rightMoving)
        {
            StartCoroutine(LerpPosition(lightLeftPosition, 5, leftScale));
            StartCoroutine(LerpPosition(heavyRightPosition, 5, rightScale));
            StartCoroutine(LerpRotation(Quaternion.Euler(armRotations[2]), 5, arm));

            leftMoving = false;
            rightMoving = true;
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
}