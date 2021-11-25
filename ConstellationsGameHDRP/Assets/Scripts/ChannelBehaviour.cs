/*------------------------------------------------
    Name: ChannelBehaviour
    Purpose: Control the rotation of the channels.
    Author: Mara Dusevic
    Modified: 11 November 2021
--------------------------------------------------
    Copyright 2021 Bookshelf Studios
------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConnectedChannel
{
    public GameObject channelObj; // Connected channels object
    public float connectedRotAmount; // Amount to rotate the connected channel with the given channel

    public ConnectedChannel(GameObject a_channelObj, float a_connectedRotAmount)
    {
        channelObj = a_channelObj;
        connectedRotAmount = a_connectedRotAmount;
    }

    // Returns the channels behaviour script
    public ChannelBehaviour GetBehaviour()
    {
        return channelObj.GetComponent<ChannelBehaviour>();
    }
}

public class ChannelBehaviour : MonoBehaviour
{
    #region Fields
    
    [Header("Channel Rotations")]
    [SerializeField] private float rotateAmount = 45.0f; // Channel's rotation amount
    [SerializeField] private float rotateSpeed = 5.0f; // Speed of the channel's rotation
    [SerializeField] public int[] correctRotations = { 0 }; // Array of all the correct rotations

    [Header("Connected Channels")]
    [SerializeField] private List<ConnectedChannel> connectedChannels; // All channels connected to this channel

    [Header("Interact Text")]
    [SerializeField] public GameObject buttonText; // UI element to indicate how to interact

    [Header("Audio Settings")]
    public AudioClip[] grindingSoundEffects; // Grinding sound effects for interaction
    public AudioSource audioSource; // Audio source for channel

    private ChannelHolder _channelHolder; // Object holding the channel
    private bool _playingRotation = false; // Whether rotation is playing currently
    private bool _isDone = false; // Whether the channel is in correct position
    private float _defaultRotation; // Default rotation for channels

    #endregion

    #region Functions

    // Start function - runs at the beginning
    private void Start()
    {
        _channelHolder = this.transform.parent.gameObject.GetComponent<ChannelHolder>();
        _defaultRotation = this.transform.localRotation.eulerAngles.y;
    }

    // Update function - runs every frame
    private void Update()
    {
        // Gets the local y rotation value of the channel
        float yRot = this.transform.localRotation.eulerAngles.y;
        
        // Loops through given correct rotations
        foreach (float rot in correctRotations)
        {
            // If the channel's current y rot is the same as one of the given correct angle, set channel to completed
            if (Mathf.RoundToInt(yRot) == rot)
            {
                // If finished, set to true and exit
                _isDone = true;
                return;
            }

            // Otherwise set as false;
            _isDone = false;
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

    // Returns a bool on channel sections' completion
    private bool IsChannelSectionDone()
    {
        return _channelHolder.GetStatus();
    }

    // Rotates the water channel when called
    public void RotateWaterChannel()
    {
        // If the channel section has been completed, return.
        if (this.IsChannelSectionDone() == true)
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

            // Loops through all the connected channels to rotate simultaneously.
            foreach (ConnectedChannel channel in connectedChannels)
            {
                // If the channel's holder is set to completed, ignore channel from rotation.
                if (channel.GetBehaviour().IsChannelSectionDone())
                {
                    return;
                }
                
                // Creates a target rotation based off current rotation
                Quaternion rot = Quaternion.Euler(0, channel.channelObj.transform.localRotation.eulerAngles.y + channel.connectedRotAmount, 0);

                // If the rotation animation is not playing
                if (channel.GetBehaviour()._playingRotation == false)
                {
                    // Starts coroutine to rotate connected channel to target rotation at a given speed
                    channel.GetBehaviour().StartCoroutine(LerpRotation(rot, channel.GetBehaviour().rotateSpeed, channel.channelObj));
                }
            }
        }
    }

    // Rotates the water channel to default position
    public void RotateToDefaultPosition()
    {
        // Creates a target rotation based off default rotation
        Quaternion newRot = Quaternion.Euler(0, _defaultRotation, 0);
            
        // If the rotation animation is not playing 
        if (_playingRotation == false)
        {
            // Starts coroutine to rotate channel to default rotation at a given speed
            StartCoroutine(LerpRotation(newRot, rotateSpeed, gameObject));
        }
    }

    // Rotates the channel over a set duration to a given rotation
    private IEnumerator LerpRotation(Quaternion endValue, float duration, GameObject channel)
    {
        int sfxIndex = Random.Range(0, 2);

        audioSource.clip = grindingSoundEffects[sfxIndex];
        audioSource.Play();

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

        audioSource.Stop();
    }
    
    #endregion
}
