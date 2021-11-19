/*--------------------------------------------------------------
    Name: LibraBehaviour
    Purpose: Resets the rock position if it falls on the ground.
    Authour: Logan Ryan
    Modified: 18 November 2021
----------------------------------------------------------------
    Copyright 2021 Bookshelf Studios
--------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraBehaviour : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private ScaleBehaviour scaleBehaviour; // The main script for the scale
    #endregion

    #region Functions
    // OnTriggerEnter is called when the Collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        scaleBehaviour.ResetRockPosition(other.gameObject);
    }
    #endregion
}
