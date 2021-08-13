using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    [Header("Channel Rotations")]
    [SerializeField]
    private float rotateAmount = 45.0f;
    [SerializeField]
    private float rotateSpeed = 5.0f;
    [SerializeField]
    public float correctRotation = 0.0f;

    [Header("Interact Text")]
    [SerializeField]
    public GameObject buttonText;

    private bool playingRotation = false;
    private bool isDone = false;

    private void Start()
    {
        buttonText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        buttonText.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        buttonText.SetActive(false);
    }

    private void Update()
    {
        if (gameObject.transform.rotation.eulerAngles.y == correctRotation)
        {
            isDone = true;
            Debug.Log(gameObject.name + " is done");
        }
    }

    public void RotateWaterChannel()
    {
        if (isDone == true)
        {
            return;
        }

        Quaternion newRot = Quaternion.Euler(0, gameObject.transform.rotation.eulerAngles.y + rotateAmount, 0);

        if (playingRotation == false)
        {
            StartCoroutine(LerpRotation(newRot, rotateSpeed, gameObject));
        }
    }

    IEnumerator LerpRotation(Quaternion endValue, float duration, GameObject channel)
    {
        playingRotation = true;

        float time = 0;
        Quaternion startValue = channel.transform.rotation;

        while (time < duration)
        {
            channel.transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        channel.transform.rotation = endValue;

        playingRotation = false;
    }
}
