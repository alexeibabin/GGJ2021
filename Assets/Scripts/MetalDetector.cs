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

    public float fovDetectionActivationDistance = 0;
    
    [SerializeField]
    private float msToNextBeep;
    [SerializeField]
    private float timeSinceLastBeep = 0f;

    private Camera mainCamera = null;

    public bool lookingAtObject = false;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    void CalculateBeepHz()
    {
        var myPosition = transform.position;
        var closestDistance = sensor.SensorRange;
        GameObject closestObject = null;
        sensor.DetectedObjects.Sort((a, b) =>
            Mathf.RoundToInt(Vector3.Distance(a.transform.position, myPosition) - Vector3.Distance(b.transform.position, myPosition)));

        foreach (var obj in sensor.DetectedObjects)
        {
            var dist = Vector3.Distance(obj.transform.position, myPosition);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestObject = obj;
            }
        }
        
        //distance to object form 0 to 1. when 1 is closest
        var normalizedDistance = 0f;

        if (sensor.DetectedObjects.Count > 0)
        {

            //if were closer to an object that fovDetectionActivationDistance
            //we calculate if the player is looking towards the object, if he is, we increase the beeps frequency
            //if not, we keep it at the frequency as though hes at the threshold.
            if (closestDistance < fovDetectionActivationDistance)
            {
                var mainCameraTransform = Camera.main.transform;
                var dot = Vector3.Dot(mainCameraTransform.forward,
                    (closestObject.transform.position - mainCameraTransform.position)
                    .normalized); // == 1 when player looks directly at it
                var closestNormalized = (1 -(closestDistance / sensor.SensorRange));
                var thresholdNormalized = (1 -(fovDetectionActivationDistance / sensor.SensorRange));
                lookingAtObject = dot > 0;
                normalizedDistance = Mathf.Lerp(thresholdNormalized, closestNormalized, Mathf.Clamp01(dot));
            }
            else
            {
                normalizedDistance = 1 - (closestDistance / sensor.SensorRange);
            }
        }

        beepHz = normalizedDistance != 0 ? Mathf.Lerp(minBeepPerSec, maxBeepPerSec, normalizedDistance) : 0;
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
