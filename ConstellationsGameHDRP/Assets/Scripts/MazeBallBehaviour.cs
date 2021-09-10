using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBallBehaviour : MonoBehaviour
{
    #region Fields
    [HideInInspector]
    public bool touchedEnd; // Has the ball reached the end
    #endregion

    #region Functions
    private void OnTriggerEnter(Collider other)
    {
        // If the ball touches the end,
        if (other.name == "EndGoal")
        {
            // Set bool to true
            touchedEnd = true;
        }
    }
    #endregion
}
