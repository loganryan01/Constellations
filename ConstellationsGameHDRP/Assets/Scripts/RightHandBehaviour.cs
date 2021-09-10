using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandBehaviour : MonoBehaviour
{
    #region Fields
    // The main scale script
    ScaleBehaviour scaleBehaviour;
    #endregion

    #region Functions
    // Start function
    void Start()
    {
        scaleBehaviour = GetComponentInParent<ScaleBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the rock has been placed on the right hand of the scale, add the weight to the right hand
        if (other.gameObject.CompareTag("Rock") && other.gameObject.transform.parent != transform)
        {
            scaleBehaviour.rightWeight += other.gameObject.GetComponent<Rigidbody>().mass;

            other.gameObject.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the rock has been remove from the right hand of the scale, remove the weight from the right hand
        if (other.gameObject.CompareTag("Rock"))
        {
            scaleBehaviour.rightWeight -= other.gameObject.GetComponent<Rigidbody>().mass;

            other.gameObject.transform.parent = null;
        }
    }
    #endregion
}
