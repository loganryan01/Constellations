/*-------------------------------------------------------
    Name: Dialogue
    Purpose: Holds information about characters dialogue.
    Authour: Logan Ryan
    Modified: 7 October 2021
---------------------------------------------------------
    Copyright 2021 Bookshelf Studios
-------------------------------------------------------*/
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;

    public UnityEvent OnDialogueEnd;
}
