using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WaterPuzzle : MonoBehaviour
{
    #region Fields
    
    [Header("Dialogue")]
    [SerializeField] private DialogueTrigger dialogueTrigger;

    private List<ChannelHolder> _channelSections = new List<ChannelHolder>();
    private bool _hasWon = false;

    #endregion
    
    #region Functions
    
    // Start function 
    private void Start()
    {
        foreach (Transform child in transform)
        {
            _channelSections.Add(child.gameObject.GetComponent<ChannelHolder>());
        }
    }

    // Update function - run every frame
    private void Update()
    {
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
            dialogueTrigger.TriggerDialogue();
        }
    }
    
    #endregion
}
