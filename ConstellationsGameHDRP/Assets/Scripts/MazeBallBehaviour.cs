/*-------------------------------------------------------------------
    Name: MazeBallBehaviour
    Purpose: Checks if the maze ball has reached the end of the maze.
    Authour: Logan Ryan
    Modified: 18 November 2021
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
    private Rigidbody rb; // The rigidbody of the ball

    [Header("Puzzle Completion Settings")]
    public UnityEvent onComplete; // Events to play when the puzzle is completed

    [Header("Ball Settings")]
    public float thrust; // How much thrust to apply to the ball
    #endregion

    #region Functions
    // Start function
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update function
    private void Update()
    {
        // If the ball is moving.
        if (rb.velocity.magnitude >= 1)
        {
            Vector3 direction = new Vector3();

            // Get the x-direction of the ball
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

            // Get the z-direction of the ball
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

            // Apply additional force to the ball
            rb.AddForce(direction * thrust);
        }
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        // If the ball touches the end,
        if (other.name == "EndGoal")
        {
            // Set bool to true and trigger events when the puzzle is completed
            touchedEnd = true;
            onComplete.Invoke();
        }
    }
    #endregion
}
