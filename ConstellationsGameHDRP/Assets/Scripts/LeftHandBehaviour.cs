using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandBehaviour : MonoBehaviour
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
        // If the rock has been placed on the left hand of the scale, add the weight to the left hand
        if (other.gameObject.CompareTag("Rock") && other.gameObject.transform.parent != transform)
        {
            //other.gameObject.transform.position = gameObject.transform.position;
            scaleBehaviour.leftWeight += other.gameObject.GetComponent<Rigidbody>().mass;
            scaleBehaviour.UpdateScale();

            other.gameObject.transform.parent = transform;
            other.gameObject.transform.localPosition = new Vector3(-6, 137.5f, -0.1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the rock has been remove from the left hand of the scale, remove the weight from the left hand
        if (other.gameObject.CompareTag("Rock"))
        {
            scaleBehaviour.leftWeight -= other.gameObject.GetComponent<Rigidbody>().mass;
            scaleBehaviour.UpdateScale();

            other.gameObject.transform.parent = null;
        }
    }
    #endregion
}
