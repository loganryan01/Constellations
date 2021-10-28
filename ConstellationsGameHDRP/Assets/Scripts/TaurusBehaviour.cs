/*----------------------------------
    Name: TaurusBehaviour
    Purpose: Controls the player.
    Authour: Mara Dusevic
    Modified: 28 October 2021
------------------------------------
    Copyright 2021 Bookshelf Studios
----------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaurusBehaviour : MonoBehaviour
{
    MazeBehaviour mazeBehaviour;
    
    // Start is called before the first frame update
    void Start()
    {
        mazeBehaviour = FindObjectOfType<MazeBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteraction()
    {
        mazeBehaviour.onInteraction.Invoke();
    }

    public bool CheckPuzzleCompletion()
    {
        return mazeBehaviour.mazeCompleted;
    }

    public GameObject GetMazeObject()
    {
        return mazeBehaviour.gameObject;
    }
}
