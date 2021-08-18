using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChannelHolder : MonoBehaviour
{
    [SerializeField]
    private GameObject startPool;
    [SerializeField]
    private GameObject endPool;
    [SerializeField]
    private bool allowWater = false;
    [SerializeField]
    private float waterDuration = 2.0f;

    private List<WaterBehaviour> _channels = new List<WaterBehaviour>();
    private Vector3 _scaleWaterTo;
    private bool _hasWaterPlayed = false;

    private void Start()
    {
        _scaleWaterTo = CalculateWaterScale(startPool, endPool);
        
        foreach (Transform child in transform)
        {
            _channels.Add(child.gameObject.GetComponent<WaterBehaviour>());
        }
    }

    private void Update()
    {
        ChannelsCheck();
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
        if (allowWater)
        {
            return;
        }

        int channelsFinished = 0;
        foreach (WaterBehaviour channel in _channels)
        {
            if (channel.CheckCorrectRotation() == true)
            {
                channelsFinished++;
            }
        }

        if (channelsFinished == _channels.Count)
        {
            allowWater = true;
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
}
