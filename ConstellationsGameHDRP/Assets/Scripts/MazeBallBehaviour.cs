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
    private Rigidbody rb;

    [Header("Puzzle Completion Settings")]
    public UnityEvent onComplete;

    [Header("Ball Settings")]
    public float thrust;
    #endregion

    #region Functions
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.velocity.magnitude >= 1)
        {
            Vector3 direction = new Vector3();

            if (rb.velocity.x < 0)
            {
                direction.x = -1;
            }
            else if (rb.velocity.x > -1 || rb.velocity.x < 1)
            {
                direction.x = 0;
            }
            else if (rb.velocity.x > 0)
            {
                direction.x = 1;
            }

            if (rb.velocity.z < 0)
            {
                direction.z = -1;
            }
            else if (rb.velocity.z > -1 || rb.velocity.z < 1)
            {
                direction.z = 0;
            }
            else if (rb.velocity.z > 0)
            {
                direction.z = 1;
            }

            rb.AddForce(direction * thrust);
        }
    }

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
