using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandBehaviour : MonoBehaviour
{
    ScaleBehaviour scaleBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        scaleBehaviour = GetComponentInParent<ScaleBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rock"))
        {
            scaleBehaviour.leftWeight += other.gameObject.GetComponent<Rigidbody>().mass;

            other.gameObject.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Rock"))
        {
            scaleBehaviour.leftWeight -= other.gameObject.GetComponent<Rigidbody>().mass;

            other.gameObject.transform.parent = null;
        }
    }
}
