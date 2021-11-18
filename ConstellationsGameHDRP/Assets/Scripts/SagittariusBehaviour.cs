/*------------------------------------
    Name: SagittariusBehaviour
    Purpose: Resets Sagittarius puzzle
    Author: Logan Ryan
    Modified: 18 November 2021
--------------------------------------
    Copyright 2021 Bookshelf Studios
------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SagittariusBehaviour : MonoBehaviour
{
    #region Fields
    MirrorBehaviour[] mirrorBehaviours = new MirrorBehaviour[5]; // Number of mirrors in the sagittarius puzzle
    #endregion

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        mirrorBehaviours = FindObjectsOfType<MirrorBehaviour>();
    }

    // Reset all the mirrors to their default positions
    public void ResetMirrors()
    {
        if (!CheckPuzzleCompletion())
        {
            foreach (var mirror in mirrorBehaviours)
            {
                mirror.RotateMirrorToDefaultPosition();
            }
        }
    }

    // Check if the puzzle is completed
    public bool CheckPuzzleCompletion()
    {
        return mirrorBehaviours[0].laserBehaviour.laserPuzzleCompleted;
    }
    #endregion
}
