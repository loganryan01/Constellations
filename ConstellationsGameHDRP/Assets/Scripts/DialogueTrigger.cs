/*-------------------------------------------------
    Name: DialogueTrigger
    Purpose: Starts the dialogue for the character.
    Authour: Logan Ryan
    Modified: 7 October 2021
---------------------------------------------------
    Copyright 2021 Bookshelf Studios
-------------------------------------------------*/
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
