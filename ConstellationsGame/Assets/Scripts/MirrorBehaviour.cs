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
        GameObject mirror = this.gameObject.transform.Find("Mirror").gameObject;
        Quaternion currentRot = mirror.transform.rotation;
        Quaternion newRot = Quaternion.Euler(0, currentRot.eulerAngles.y + rotateAmount, 0);

        mirror.transform.rotation = Quaternion.RotateTowards(currentRot, newRot, Time.time * rotateSpeed);
    }
}