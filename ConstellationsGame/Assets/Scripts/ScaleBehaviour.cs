using System.Collections;
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

    private bool leftMoving = false;
    private bool rightMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (leftWeight > rightWeight && !leftMoving)
        {
            Debug.Log("Left is moving");
            StartCoroutine(LerpPosition(leftScalePositions[2], 5, leftScale));
            StartCoroutine(LerpPosition(rightScalePositions[0], 5, rightScale));
            StartCoroutine(LerpRotation(Quaternion.Euler(armRotations[0]), 5, arm));

            leftMoving = true;
            rightMoving = false;
        } 
        else if (leftWeight == rightWeight && leftMoving ||
            leftWeight == rightWeight && rightMoving)
        {
            Debug.Log("Scale is balanced");

            if (leftMoving)
            {
                leftScalePositions[1] -= leftScalePositions[2];
                rightScalePositions[1] -= rightScalePositions[0];
            }
            else if (rightMoving)
            {
                leftScalePositions[1] -= leftScalePositions[0];
                rightScalePositions[1] -= rightScalePositions[2];
            }

            StartCoroutine(LerpPosition(leftScalePositions[1], 5, leftScale));
            StartCoroutine(LerpPosition(rightScalePositions[1], 5, rightScale));
            StartCoroutine(LerpRotation(Quaternion.Euler(armRotations[1]), 5, arm));

            leftMoving = false;
            rightMoving = false;
        }
        else if (leftWeight < rightWeight && !rightMoving)
        {
            Debug.Log("Right is moving");
            StartCoroutine(LerpPosition(leftScalePositions[0], 5, leftScale));
            StartCoroutine(LerpPosition(rightScalePositions[2], 5, rightScale));
            StartCoroutine(LerpRotation(Quaternion.Euler(armRotations[2]), 5, arm));

            leftMoving = false;
            rightMoving = true;
        }
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration, GameObject hand)
    {
        float time = 0;
        Vector3 startPosition = hand.transform.position;
        Vector3 endPosition = hand.transform.position + targetPosition;

        while (time < duration)
        {
            hand.transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        hand.transform.position = endPosition;
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
