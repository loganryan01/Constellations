using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Dialogue manager for the game
    public DialogueManager dialogueManager;
    
    // The character's dialogue information
    public Dialogue dialogue;

    // Start the dialogue
    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(dialogue);
    }
}
