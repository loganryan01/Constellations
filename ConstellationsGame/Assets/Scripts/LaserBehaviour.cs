using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserBehaviour : MonoBehaviour
{
    [Tooltip("Max distance for beam to travel")]
    public int distance;

    public LineRenderer lr;

    [Tooltip("If laser touches this tag, something will happen")]
    public string winTag;

    [Tooltip("If laser touches this tag, reflect")]
    public string refTag;

    [Tooltip("Max reflections")]
    public int limit = 100;

    // Dialogue Controls
    [HideInInspector]
    public bool dialogueStarted = false;

    private int verti = 1; //segment handler don't touch.
    private bool iactive;
    private Vector3 currot;
    private Vector3 curpos;
    private DialogueTrigger dialogueTrigger;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        DrawLaser();
    }

    void DrawLaser()
    {
        verti = 1;
        iactive = true;
        currot = transform.forward;
        curpos = transform.position;
        lr.positionCount = 1;
        lr.SetPosition(0, transform.position);

        while (iactive)
        {
            verti++;
            RaycastHit hit;
            lr.positionCount = verti;

            if (Physics.Raycast(curpos, currot, out hit, distance, 7))
            {
                //verti++;
                curpos = hit.point;
                currot = Vector3.Reflect(currot, hit.normal);
                lr.SetPosition(verti - 1, hit.point);
                if (hit.transform.gameObject.tag != refTag)
                {
                    iactive = false;
                }

                if (hit.transform.gameObject.tag == winTag && !dialogueStarted)
                {
                    WinFunction();
                }
            }
            else
            {
                //verti++;
                iactive = false;
                lr.SetPosition(verti - 1, curpos + 100 * currot);

            }

            if (verti > limit)
            {
                iactive = false;
            }
        }

        //Debug.Log(dialogueStarted);
        if (Cursor.lockState == CursorLockMode.None)
        {
            playerController.lookSensitivity = 0;
        }
        else
        {
            playerController.lookSensitivity = 60;
        }

    } 

    void WinFunction()
    {
        Debug.Log("Hit End point");
        dialogueStarted = true;
        dialogueTrigger.TriggerDialogue();
    }
}
