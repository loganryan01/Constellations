using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class ChannelBehaviour : MonoBehaviour
{
    [Header("Channel Rotations")]
    [SerializeField]
    private float rotateAmount = 45.0f;
    [SerializeField]
    private float rotateSpeed = 5.0f;
    [SerializeField]
    public int[] correctRotations = { 0 };

    [Header("Interact Text")]
    [SerializeField]
    public GameObject buttonText;

    private bool _playingRotation = false;
    private bool _isDone = false;

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
        if (_isDone)
        {
            return;
        }

        float yRot = this.transform.localRotation.eulerAngles.y;
        foreach (float rot in correctRotations)
        {
            if (Mathf.RoundToInt(yRot) == rot)
            {
                _isDone = true;
            }
        }
    }

    public bool CheckCorrectRotation()
    {
        if (this._isDone)
        {
            return true;
        }

        return false;
    }

    public void RotateWaterChannel()
    {
        if (this._isDone == true)
        {
            return;
        }

        Quaternion newRot = Quaternion.Euler(0, gameObject.transform.localRotation.eulerAngles.y + rotateAmount, 0);

        if (this._playingRotation == false)
        {
            StartCoroutine(LerpRotation(newRot, rotateSpeed, gameObject));
        }
    }

    private IEnumerator LerpRotation(Quaternion endValue, float duration, GameObject channel)
    {
        _playingRotation = true;

        float time = 0;
        Quaternion startValue = channel.transform.localRotation;

        while (time < duration)
        {
            channel.transform.localRotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        channel.transform.localRotation = endValue;

        _playingRotation = false;
    }
}
