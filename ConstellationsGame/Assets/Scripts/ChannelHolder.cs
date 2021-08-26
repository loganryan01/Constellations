using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChannelHolder : MonoBehaviour
{
    [Header("Water")]
    [SerializeField]
    private GameObject startPool;
    [SerializeField]
    private GameObject endPool;
    [SerializeField]
    private float waterDuration = 2.0f;
    
    [Header("Fish")]
    [SerializeField]
    private GameObject fish;
    [SerializeField]
    private GameObject fishWaypoint = null;
    [SerializeField]
    private float fishSpeed = 1.1f;
    [SerializeField]
    private float fishDelayTime = 2.2f;
    
    // All channels within the channel section
    private List<ChannelBehaviour> _channels = new List<ChannelBehaviour>();
    
    // Scale calculated for water to expand to
    private Vector3 _scaleWaterTo;
    
    private bool _allowWater = false; // Whether water is allowed to be scaled
    private bool _hasWaterPlayed = false; // Whether water scaling has finished
    private bool _hasFishMoved = false; // Whether fish have moved 

    private void Start()
    {
        _scaleWaterTo = CalculateWaterScale(startPool, endPool);
        
        foreach (Transform child in transform)
        {
            _channels.Add(child.gameObject.GetComponent<ChannelBehaviour>());
        }
    }

    private void Update()
    {
        ChannelsCheck();
        if (_allowWater && !_hasFishMoved)
        {
            MoveFish();   
        }
    }

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

    private void ChannelsCheck()
    {
        if (_allowWater)
        {
            return;
        }

        int channelsFinished = 0;
        foreach (ChannelBehaviour channel in _channels)
        {
            if (channel.CheckCorrectRotation() == true)
            {
                channelsFinished++;
            }
        }

        if (channelsFinished == _channels.Count)
        {
            _allowWater = true;
            DrawWater();
        }
    }

    private void DrawWater()
    {
        if (_hasWaterPlayed)
        {
            return;
        }
        
        GameObject startPoint = startPool.transform.Find("WaterPivotPoint").gameObject;
        StartCoroutine(LerpWater(startPoint, _scaleWaterTo, waterDuration));
    }
    
    private void MoveFish()
    {
        StartCoroutine(FishMovement(fishDelayTime));
    }

    private IEnumerator LerpWater(GameObject waterToScale, Vector3 scaleTo, float duration)
    {
        float elapsedTime = 0;
        Vector3 startWaterScale = waterToScale.transform.localScale;

        while (elapsedTime < waterDuration)
        {
            waterToScale.transform.localScale = Vector3.MoveTowards(startWaterScale, scaleTo, elapsedTime / waterDuration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        } 
        
        waterToScale.transform.localScale = scaleTo;
        _hasWaterPlayed = true;
    }
    
    private IEnumerator FishMovement(float delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        
        Vector3 waypoint = fishWaypoint.transform.position;
        Vector3 fishPosition = fish.transform.position;
        Vector3 endPos = new Vector3(waypoint.x, fishPosition.y, waypoint.z);
        
        this.fish.transform.position = Vector3.MoveTowards(fishPosition, endPos, fishSpeed * Time.deltaTime);

        if (fish.transform.position == endPos)
        {
            _hasFishMoved = true;
        }
    }
}
