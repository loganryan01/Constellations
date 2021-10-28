/*------------------------------------
    Name: SagittariusBehaviour
    Purpose: Resets Sagittarius puzzle
    Author: Logan Ryan
    Modified: 28 October 2021
--------------------------------------
    Copyright 2021 Bookshelf Studios
------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SagittariusBehaviour : MonoBehaviour
{
    MirrorBehaviour[] mirrorBehaviours = new MirrorBehaviour[5];
    
    // Start is called before the first frame update
    void Start()
    {
        mirrorBehaviours = FindObjectsOfType<MirrorBehaviour>();
    }

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

    public bool CheckPuzzleCompletion()
    {
        return mirrorBehaviours[0].laserBehaviour.laserPuzzleCompleted;
    }
}
