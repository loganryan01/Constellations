using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class LaserBehaviour : MonoBehaviour
{
    #region Fields
    [Header("Line Controls")]
    public LineRenderer lr;

    [Tooltip("Max distance for beam to travel")]
    public int distance;

    [Tooltip("Max reflections")]
    public int limit = 100;

    // The power of the laser
    public float laserPower = 1.0f;

    [Header("Tag controls")]
    [Tooltip("If laser touches this tag, something will happen")]
    public string winTag;

    [Tooltip("If laser touches this tag, reflect")]
    public string refTag;

    // Layer to reflect off
    public LayerMask layerMask;

    [Header("Saggitarius Dialogue")]
    [SerializeField] private DialogueTrigger dialogueTrigger;

    // Dialogue Controls
    [HideInInspector]
    public bool laserPuzzleCompleted = false;

    [Header("Puzzle Completion Function")]
    public UnityEvent onComplete;

    private int verti = 1; //segment handler don't touch.
    private bool iactive; // Is the laser active
    private Vector3 currot; // Current rotation of the laser
    private Vector3 curpos; // Current position of the laser
    #endregion

    #region Functions
    // Update function - run every frame
    void Update()
    {
        DrawLaser();
    }

    // Draw the laser
    void DrawLaser()
    {
        verti = 1;

        // Activate the laser
        iactive = true;

        // Set rotation and position
        currot = transform.forward;
        curpos = transform.position;

        // Set the number of vertices to 1
        lr.positionCount = 1;

        // Set the postion of the vertex to the start of the laser
        lr.SetPosition(0, transform.position);

        // While the laser is active
        while (iactive)
        {
            // Increase the number of vertices by 1
            verti++;
            lr.positionCount = verti;

            RaycastHit hit;

            // If the laser hits an object,
            if (Physics.Raycast(curpos, currot, out hit, distance, layerMask))
            {
                // Get the position of where the laser hit the object
                curpos = hit.point;

                // Reflect the laser off the object
                currot = Vector3.Reflect(currot, hit.normal * laserPower);

                // Set the position of the latest vertex to the laser reflection point
                lr.SetPosition(verti - 1, hit.point);

                // If the laser did not hit an object with the reflection tag, turn off the laser
                if (hit.transform.gameObject.tag != refTag)
                {
                    iactive = false;
                }

                // If the laser hits the object with the win tag, trigger event
                if (hit.transform.gameObject.tag == winTag && !laserPuzzleCompleted)
                {
                    laserPuzzleCompleted = true;
                    onComplete.Invoke();
                }
            }
            else
            {
                // Turn off the laser
                iactive = false;

                // Set the position of the latest vertex to the end of the laser
                lr.SetPosition(verti - 1, curpos + 100 * currot);
            }

            // If the laser has reflected off the max amount of reflections, turn off the laser
            if (verti > limit)
            {
                iactive = false;
            }
        }
    } 

    public void LockCursor(bool lockCursor)
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    #endregion
}
