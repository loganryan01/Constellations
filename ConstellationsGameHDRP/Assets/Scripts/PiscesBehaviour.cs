/*----------------------------------
    Name: PiscesBehaviour
    Purpose: Resets Pisces puzzle
    Author: Logan Ryan
    Modified: 28 October 2021
------------------------------------
    Copyright 2021 Bookshelf Studios
----------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiscesBehaviour : MonoBehaviour
{
    ChannelBehaviour[] channelBehaviours = new ChannelBehaviour[8];
    
    // Start is called before the first frame update
    void Start()
    {
        channelBehaviours = FindObjectsOfType<ChannelBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
