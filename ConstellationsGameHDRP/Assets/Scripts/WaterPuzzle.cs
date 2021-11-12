/*----------------------------------------------------
    Name: WaterPuzzle
    Purpose: Checks if the pisces puzzle is completed.
    Author: Mara Dusevic
    Modified: 11 November 2021
------------------------------------------------------
    Copyright 2021 Bookshelf Studios
----------------------------------------------------*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaterPuzzle : MonoBehaviour
{
    #region Fields
    
    [Header("Dialogue")]
    [SerializeField] private DialogueTrigger dialogueTrigger; // Trigger for dialogue events
    public UnityEvent onComplete; // On completion event

    private List<ChannelHolder> _channelSections = new List<ChannelHolder>(); // All channel sections in the puzzle
    private bool _hasWon = false; // Boolean on the status of the puzzle

    #endregion
    
    #region Functions
    
    // Start function - runs at the beginning
    private void Start()
    {
        _channelSections.AddRange(transform.gameObject.GetComponentsInChildren<ChannelHolder>());
    }

    // Update function - run every frame
    private void Update()
    {
        // If the player hasn't won, check status of puzzle.
        if (!_hasWon)
        {
            WinCheck();
        }
    }
    
    // Checks whether player has won the puzzle
    private void WinCheck()
    {
        // Loops through all the channels holders
        int sectionsFinished = 0;
        foreach (ChannelHolder channelSection in _channelSections)
        {
            // If channel section has been completed, increase counter
            if (channelSection.IsCompleted() == true)
            {
                sectionsFinished++;
            }
        }
        
        // If all channels are in finished, call dialogue and set puzzle to won
        if (sectionsFinished == _channelSections.Count)
        {
            _hasWon = true;
            onComplete.Invoke();
        }
    }

    #endregion
}
