/*----------------------------------
    Name: PiscesBehaviour
    Purpose: Resets Pisces puzzle
    Author: Logan Ryan
    Modified: 18 November 2021
------------------------------------
    Copyright 2021 Bookshelf Studios
----------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiscesBehaviour : MonoBehaviour
{
    #region Fields
    ChannelBehaviour[] channelBehaviours = new ChannelBehaviour[8]; // Channels for the pisces puzzle
    #endregion

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        channelBehaviours = FindObjectsOfType<ChannelBehaviour>();
    }

    // Check if the puzzle is completed
    public bool CheckPuzzleCompletion()
    {
        foreach (var channelBehaviour in channelBehaviours)
        {
            if (!channelBehaviour.CheckCorrectRotation())
            {
                return false;
            }
        }

        return true;
    }

    // Reset the channels
    public void ResetChannels()
    {
        foreach (var channelBehaviour in channelBehaviours)
        {
            if (!channelBehaviour.CheckCorrectRotation())
            {
                channelBehaviour.RotateToDefaultPosition();
            }
        }
    }
    #endregion
}
