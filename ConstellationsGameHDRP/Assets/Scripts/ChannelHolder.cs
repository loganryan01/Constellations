/*-----------------------------------------------------
    Name: ChannelHolder
    Purpose: Control the channels of the pisces puzzle.
    Authour: Mara Dusevic
    Modified: 7 October 2021
-------------------------------------------------------
    Copyright 2021 Bookshelf Studios
-----------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelHolder : MonoBehaviour
{
    #region Fields
    
    [Header("Water")]
    [SerializeField] private GameObject startPool; // The starting pool for the channel section
    [SerializeField] private GameObject endPool; // The ending pool for the channel section
    [SerializeField] private float waterDuration = 2.0f; // Duration for water to fill up channel
    
    [Header("Fish")]
    [SerializeField] private GameObject fish; // Channel section's assigned fish 
    [SerializeField] private GameObject fishWaypoint = null; // Attached fish's destination
    [SerializeField] private float fishSpeed = 1.1f; // Movement speed of fish
    [SerializeField] private float fishDelayTime = 2.2f; // Delay time of fish's start

    // All channels within the channel section
    private List<ChannelBehaviour> _channels = new List<ChannelBehaviour>();

    private Vector3 _scaleWaterTo;  // Scale calculated for water to expand to
    private bool _allowWater = false; // Whether water is allowed to be scaled
    private bool _hasWaterPlayed = false; // Whether water scaling has finished
    private bool _hasFishMoved = false; // Whether fish have moved 

    #endregion
    
    #region Functions
    
    // Start function 
    private void Start()
    {
        _scaleWaterTo = CalculateWaterScale(startPool, endPool);
        
        foreach (Transform child in transform)
        {
            _channels.Add(child.gameObject.GetComponent<ChannelBehaviour>());
        }
    }

    // Update function - run every frame
    private void Update()
    {
        ChannelsCheck();
        if (_allowWater && !_hasFishMoved)
        {
            MoveFish();   
        }
    }

    // Calculates the water's scale length between two given objects
    private Vector3 CalculateWaterScale(GameObject start, GameObject end)
    {
        Vector3 startPnt = start.transform.Find("WaterPivotPoint").position;
        Vector3 endPnt = end.transform.Find("WaterPivotPoint").position;

        float distBtnPoints = Vector3.Distance(startPnt, endPnt);
        float xScale = distBtnPoints / 10.0f;
        
        Vector3 scale = start.transform.Find("WaterPivotPoint").localScale;
        Vector3 waterScale = new Vector3(xScale, scale.y, scale.z);
        return waterScale;
    }
    
    // Returns boolean on the status of the channel (Checks whether fish has moved yet)
    public bool IsCompleted()
    {
        return _hasFishMoved;
    }

    // Checks each channel in section to determine whether to draw water 
    private void ChannelsCheck()
    {
        // If water has already been allowed return
        if (_allowWater)
        {
            return;
        }

        // Loops through all the channels
        int channelsFinished = 0;
        foreach (ChannelBehaviour channel in _channels)
        {
            // If channel is in correct position, increase counter
            if (channel.CheckCorrectRotation() == true)
            {
                channelsFinished++;
            }
        }

        // If all channels are in the correct position, allow water, and draw water
        if (channelsFinished == _channels.Count)
        {
            _allowWater = true;
            DrawWater();
        }
    }

    // Draws the water from starting pool to end pool
    private void DrawWater()
    {
        // If water animation has already played, return
        if (_hasWaterPlayed)
        {
            return;
        }
        
        // Gets the pivot point of the start pool to scale water 
        GameObject startPoint = startPool.transform.Find("WaterPivotPoint").gameObject;
        
        // Starts coroutine to scale water from given points over a set duration
        StartCoroutine(LerpWater(startPoint, _scaleWaterTo, waterDuration));
    }
    
    // Moves fish from starting pool to end pool
    private void MoveFish()
    {
        // Starts coroutine to move fish from current position to set end point with a delay time
        StartCoroutine(FishMovement(fishDelayTime));
    }

    // Scales water each frame until its reached the end point
    private IEnumerator LerpWater(GameObject waterToScale, Vector3 scaleTo, float duration)
    {
        float elapsedTime = 0;
        Vector3 startWaterScale = waterToScale.transform.localScale;

        // While elapsed time is under the set duration
        while (elapsedTime < waterDuration)
        {
            // Scales the given object to move towards given end point 
            waterToScale.transform.localScale = Vector3.MoveTowards(startWaterScale, scaleTo, elapsedTime / waterDuration);
            
            // Updates the elapsed time
            elapsedTime += Time.deltaTime;
            
            // Waits until the end of the frame before continuing
            yield return new WaitForEndOfFrame();
        } 
        
        // Sets the scale to the end scale 
        waterToScale.transform.localScale = scaleTo;
        
        // Sets the water animation as finished
        _hasWaterPlayed = true;
    }
    
    // Moves fish after given delay to waypoint
    private IEnumerator FishMovement(float delayTime)
    {
        // Waits for a given time before continuing 
        yield return new WaitForSecondsRealtime(delayTime);
        
        Vector3 waypoint = fishWaypoint.transform.position; // Waypoint position
        Vector3 fishPosition = fish.transform.position; // Fish's current position
        Vector3 endPos = new Vector3(waypoint.x, fishPosition.y, waypoint.z); // End point for fish to travel to
        
        // Moves the fish to the end point overtime at a given speed
        this.fish.transform.position = Vector3.MoveTowards(fishPosition, endPos, fishSpeed * Time.deltaTime);

        // If the fish has reached the end position, set fish as being finished
        if (fish.transform.position == endPos)
        {
            _hasFishMoved = true;
        }
    }
    
    #endregion
}
