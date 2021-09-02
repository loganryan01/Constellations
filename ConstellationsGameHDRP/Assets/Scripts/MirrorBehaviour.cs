using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBehaviour : MonoBehaviour
{
    [Header("Mirror Rotations")]
    [SerializeField] public float rotateAmount = 45.0f;
    [SerializeField] public float rotateDuration = 2.0f;

    [Header("Interact Text")]
    [SerializeField] public GameObject buttonText;

    private bool _playingRotation = false;

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
        Quaternion newRot = Quaternion.Euler(0, transform.rotation.eulerAngles.y + rotateAmount, 0);

        if (_playingRotation == false)
        {
            StartCoroutine(LerpRotation(newRot, rotateDuration, gameObject));
        }
    }

    private IEnumerator LerpRotation(Quaternion endValue, float duration, GameObject mirror)
    {
        _playingRotation = true;
        
        float time = 0;
        Quaternion startValue = mirror.transform.rotation;

        while (time < duration)
        {
            mirror.transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        mirror.transform.rotation = endValue;

        _playingRotation = false;
    }
}