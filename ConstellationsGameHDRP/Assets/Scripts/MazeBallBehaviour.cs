/*-------------------------------------------------------------------
    Name: MazeBallBehaviour
    Purpose: Checks if the maze ball has reached the end of the maze.
    Authour: Logan Ryan
    Modified: 7 October 2021
---------------------------------------------------------------------
    Copyright 2021 Bookshelf Studios
-------------------------------------------------------------------*/
using UnityEngine;
using UnityEngine.Events;

public class MazeBallBehaviour : MonoBehaviour
{
    #region Fields
    [HideInInspector]
    public bool touchedEnd; // Has the ball reached the end

    public UnityEvent onComplete;
    #endregion

    #region Functions
    private void OnTriggerEnter(Collider other)
    {
        // If the ball touches the end,
        if (other.name == "EndGoal")
        {
            // Set bool to true
            touchedEnd = true;
            onComplete.Invoke();
        }
    }
    #endregion
}
