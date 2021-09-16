using System.Collections;
using System.Collections.Generic;
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
