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

    public void RotateMirror()
    {
        //GameObject mirror = this.gameObject.transform.Find("Mirror").gameObject;
        GameObject mirror = gameObject;

        Quaternion newRot = Quaternion.Euler(0, mirror.transform.rotation.eulerAngles.y + rotateAmount, 0);

        if (playingRotation == false)
        {
            StartCoroutine(LerpRotation(newRot, rotateSpeed, mirror));
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