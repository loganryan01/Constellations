using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class ChannelBehaviour : MonoBehaviour
{
    #region Fields
    
    [Header("Channel Rotations")]
    [SerializeField] private float rotateAmount = 45.0f; // Channel's rotation amount
    [SerializeField] private float rotateSpeed = 5.0f; // Speed of the channel's rotation
    [SerializeField] public int[] correctRotations = { 0 }; // Array of all the correct rotations

    [Header("Interact Text")]
    [SerializeField] public GameObject buttonText; // UI element to indicate how to interact

    private bool _playingRotation = false; // Whether rotation is playing currently
    private bool _isDone = false; // Whether the channel is in correct position

    #endregion
    
    #region Functions

    // Update function - runs every frame
    private void Update()
    {
        // If the channel is in the correct position, return
        if (_isDone)
        {
            return;
        }
        
        // Gets the local y rotation value of the channel
        float yRot = this.transform.localRotation.eulerAngles.y;
        
        // Loops through given correct rotations
        foreach (float rot in correctRotations)
        {
            // If the channel's current y rot is the same as one of the given correct angle, set channel to completed
            if (Mathf.RoundToInt(yRot) == rot)
            {
                _isDone = true;
            }
        }
    }

    // Returns a bool on the channel's completion status (Does it have the correct rotation)
    public bool CheckCorrectRotation()
    {
        // If the channel is set as completed return true
        if (this._isDone)
        {
            return true;
        }

        // Otherwise return false
        return false;
    }

    // Rotates the water channel when called
    public void RotateWaterChannel()
    {
        // If the channel is in correct rotation, return.
        if (this._isDone == true)
        {
            return;
        }

        // Creates a target rotation based off current rotation
        Quaternion newRot = Quaternion.Euler(0, gameObject.transform.localRotation.eulerAngles.y + rotateAmount, 0);

        // If the rotation animation is not playing
        if (this._playingRotation == false)
        {
            // Starts coroutine to rotate channel to target rotation at a given speed
            StartCoroutine(LerpRotation(newRot, rotateSpeed, gameObject));
        }
    }

    // Rotates the channel over a set duration to a given rotation
    private IEnumerator LerpRotation(Quaternion endValue, float duration, GameObject channel)
    {
        // Sets the channel as playing rotation animation
        _playingRotation = true;

        float time = 0;
        Quaternion startValue = channel.transform.localRotation;

        // While current time is under the set duration
        while (time < duration)
        {
            // Rotates the channel from current rotation to end rotation over a given time
            channel.transform.localRotation = Quaternion.Lerp(startValue, endValue, time / duration);
            
            // Updates the elapsed time
            time += Time.deltaTime;
            
            yield return null;
        }

        // Sets the current rotation to end rotation 
        channel.transform.localRotation = endValue;

        // Sets the channel as not playing rotation animation
        _playingRotation = false;
    }
    
    #endregion
}
