using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBallBehaviour : MonoBehaviour
{
    [HideInInspector]
    public bool touchedEnd;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "EndGoal")
        {
            touchedEnd = true;
        }
    }
}
