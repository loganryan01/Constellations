/*------------------------------------
    Name: MirrorBehaviour
    Purpose: Rotates the mirror.
    Authour: Mara Dusevic / Logan Ryan
    Modified: 7 October 2021
--------------------------------------
    Copyright 2021 Bookshelf Studios
------------------------------------*/
using System.Collections;
using UnityEngine;

public class MirrorBehaviour : MonoBehaviour
{
    #region Fields
    [Header("Mirror Rotations")]
    [SerializeField] public float rotateAmount = 45.0f; // Amount to rotate object
    [SerializeField] public float rotateDuration = 2.0f; // How long the object should rotate
    public LaserBehaviour laserBehaviour;

    private bool _playingRotation = false; // Is the object rotating
    #endregion

    #region Functions

    // Rotate the mirror when the player interacts with the mirror
    public void RotateMirror()
    {
        if (laserBehaviour.laserPuzzleCompleted)
        {
            return;
        }
        
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