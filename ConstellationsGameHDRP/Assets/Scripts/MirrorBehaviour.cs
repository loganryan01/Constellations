using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBehaviour : MonoBehaviour
{
    #region Fields
    [Header("Mirror Rotations")]
    [SerializeField] public float rotateAmount = 45.0f; // Amount to rotate object
    [SerializeField] public float rotateDuration = 2.0f; // How long the object should rotate

    [Header("Interact Text")]
    [SerializeField] public GameObject buttonText; // Text to display what button for player to push

    private bool _playingRotation = false; // Is the object rotating
    #endregion

    #region Functions
    // When the Collider other enters the trigger,
    private void OnTriggerEnter(Collider other)
    {
        // Display text when player is in range
        if (other.CompareTag("Player"))
        {
            buttonText.SetActive(true);
        }
    }

    // When the Collider other has stopped touching the trigger
    private void OnTriggerExit(Collider other)
    {
        // Hide text when player is out of range
        if (other.CompareTag("Player"))
        {
            buttonText.SetActive(false);
        }
    }

    // Rotate the mirror when the player interacts with the mirror
    public void RotateMirror()
    {
        Quaternion newRot = Quaternion.Euler(0, transform.rotation.eulerAngles.y + rotateAmount, 0);

        if (_playingRotation == false)
        {
            StartCoroutine(LerpRotation(newRot, rotateDuration, gameObject));
        }
    }

    // Rotate object over a duration of time
    private IEnumerator LerpRotation(Quaternion endValue, float duration, GameObject mirror)
    {
        // The coroutine is now playing 
        _playingRotation = true;
        
        // Start the timer
        float time = 0;

        // Get the starting rotation
        Quaternion startValue = mirror.transform.rotation;

        // While time is less than rotation time
        while (time < duration)
        {
            // Rotate the object a little abit
            mirror.transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);

            // Increase time by delta time
            time += Time.deltaTime;
            yield return null;
        }

        // When time is up, set the rotation to the end value
        mirror.transform.rotation = endValue;

        // The coroutine is completed
        _playingRotation = false;
    }
    #endregion
}