using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBehaviour : MonoBehaviour
{
    [Header("Mirror Rotations")]
    [SerializeField]
    public float rotateAmount = 45.0f;
    [SerializeField]
    public float rotateSpeed = 5.0f;

    [Header("Interact Text")]
    [SerializeField]
    public GameObject buttonText;

    private bool playingRotation = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buttonText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buttonText.SetActive(false);
        }
    }

    public void RotateMirror()
    {
        Quaternion newRot = Quaternion.Euler(0, gameObject.transform.rotation.eulerAngles.y + rotateAmount, 0);

        if (playingRotation == false)
        {
            StartCoroutine(LerpRotation(newRot, rotateSpeed, gameObject));
        }
    }

    IEnumerator LerpRotation(Quaternion endValue, float duration, GameObject mirror)
    {
        playingRotation = true;
        
        float time = 0;
        Quaternion startValue = mirror.transform.rotation;

        while (time < duration)
        {
            mirror.transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        mirror.transform.rotation = endValue;

        playingRotation = false;
    }
}