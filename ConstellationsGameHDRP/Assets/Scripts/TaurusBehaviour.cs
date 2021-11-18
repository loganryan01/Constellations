/*-------------------------------------------------------
    Name: TaurusBehaviour
    Purpose: Controls the interaction trigger for taurus.
    Authour: Logan Ryan
    Modified: 18 November 2021
---------------------------------------------------------
    Copyright 2021 Bookshelf Studios
-------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaurusBehaviour : MonoBehaviour
{
    #region Fields
    MazeBehaviour mazeBehaviour; // The main maze script
    public GameObject outlineObject; // The object to be outlined
    #endregion

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        mazeBehaviour = FindObjectOfType<MazeBehaviour>();
    }

    // Invoke the interaction events for the Taurus Puzzle
    public void OnInteraction()
    {
        mazeBehaviour.onInteraction.Invoke();
    }

    // Check if the taurus puzzle is completed
    public bool CheckPuzzleCompletion()
    {
        return mazeBehaviour.mazeCompleted;
    }

    // Get the maze game object
    public GameObject GetMazeObject()
    {
        return mazeBehaviour.gameObject;
    }

    // Get the object that needs to be outlined
    public GameObject GetOutlineObject()
    {
        return outlineObject;
    }
    #endregion
}
