using System;
using System.Collections;
using System.Collections.Generic;
using SensorToolkit;
using UnityEngine;
public class MetalDetector : MonoBehaviour
{
    public RangeSensor sensor;

    public float minBeepPerSec = 0.2f;

    public float maxBeepPerSec = 30f;
    
    public AudioClip beep;

    public float beepHz = 0;
    
    public float msToNextBeep;

    public float timeSinceLastBeep = 0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    void CalculateBeepHz()
    {
        var myPosition = transform.position;
        var closestDistance = sensor.SensorRange;
        sensor.DetectedObjects.Sort((a, b) =>
            Mathf.RoundToInt(Vector3.Distance(a.transform.position, myPosition) - Vector3.Distance(b.transform.position, myPosition)));

        foreach (var obj in sensor.DetectedObjects)
        {
            var dist = Vector3.Distance(obj.transform.position, myPosition);
            if (dist < closestDistance)
            {
                closestDistance = dist;
            }
        }
        
        var value = 0f;
        if (sensor.DetectedObjects.Count > 0)
        {
            value = 1 - (closestDistance / sensor.SensorRange);
        }

        beepHz = value != 0 ? Mathf.Lerp(minBeepPerSec, maxBeepPerSec, value) : 0;
        msToNextBeep = 1000 / beepHz;
    }

    void Beep()
    {
        timeSinceLastBeep += Time.deltaTime * 1000;
        if (timeSinceLastBeep >= msToNextBeep)
        {
            timeSinceLastBeep = 0;
            AudioSource.PlayClipAtPoint(beep,transform.position);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        CalculateBeepHz();
        Beep();
    }
}
