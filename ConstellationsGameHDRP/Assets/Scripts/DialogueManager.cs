/*----------------------------------------------
    Name: DialogueManager
    Purpose: Controls the dialogue for the game.
    Authour: Logan Ryan
    Modified: 7 October 2021
------------------------------------------------
    Copyright 2021 Bookshelf Studios
----------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Fields
    [Header("Character texts")]
    public Text nameText; // Name of the character talking
    public Text dialogueText; // What the character is saying

    [Header("Dialogue Box Animator")]
    public Animator animator; // Animator to control animation for dialogue box

    [HideInInspector]
    public bool dialogueEnded = true; // Has the character finished talking

    private Queue<string> sentences; // The sentences that the person is saying
    private UnityEvent onDialogueEnd;
    #endregion

    #region Functions

    // Start function
    void Start()
    {
        sentences = new Queue<string>();
    }

    // Start the character's dialogue
    public void StartDialogue(Dialogue dialogue)
    {
        // Since the character has started talking, the dialogue has not ended
        dialogueEnded = false;

        // Move the dialogue box to position
        animator.SetBool("IsOpen", true);
        
        // Set the name text box to the name of the character who is speaking
        nameText.text = dialogue.name;

        // Empty the queue
        sentences.Clear();

        // Add the sentences from the dialogue component to the end of the queue
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        onDialogueEnd = dialogue.OnDialogueEnd;

        // Display the first sentence
        DisplayNextSentence();
    }

    // Display the next sentence that the character is saying
    public void DisplayNextSentence()
    {
        // If the character has run out of sentences to say then end the dialogue
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Get the next sentence from the front of the queue
        string sentence = sentences.Dequeue();

        // Stop the previous sentence from typing
        StopAllCoroutines();

        // Start typing the next sentence
        StartCoroutine(TypeSenetence(sentence));
    }

    // Type a sentence
    IEnumerator TypeSenetence(string sentence)
    {
        // Empty the dialogue text box
        dialogueText.text = "";

        // Every 1 second, add a letter to the dialogue text box
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    // End the character's dialogue
    void EndDialogue()
    {
        // Close the dialogue box
        animator.SetBool("IsOpen", false);

        // Since the character is no longer talking, the dialogue has ended
        dialogueEnded = true;
        onDialogueEnd.Invoke();
    }

    #endregion
}
