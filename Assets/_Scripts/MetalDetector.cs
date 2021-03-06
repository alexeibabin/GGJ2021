using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SensorToolkit;
using UnityEngine;
using UnityEngine.UIElements;

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
    
    public AnimationCurve beepCurve;

    public float autoPickupInSeconds = 10f;

    [SerializeField]
    private float timeToAutoPickUp;
    
    [Header("Wifi indicator stuff")] 
    
    public GameObject[] wifiSprites;

    public float[] wifiThresholds;

    private Camera mainCamera = null;

    [SerializeField]
    private float normalizedDistance = 0f;
    
    private GameObject closestObject = null;

    [Header("---debug----")]
    public bool lookingAtObject = false;

    public bool pickUpItem = false;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
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
                closestObject = obj;
            }
        }
        
        //distance to object form 0 to 1. when 1 is closest
        normalizedDistance = 0f;

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

        
        beepHz = normalizedDistance != 0 ? minBeepPerSec + (maxBeepPerSec * beepCurve.Evaluate(normalizedDistance)) : 0;
        msToNextBeep = 1000 / beepHz;
    }

    void Beep()
    {
        if (beepHz > 0)
        {
            timeSinceLastBeep += Time.deltaTime * 1000;
            if (timeSinceLastBeep >= msToNextBeep)
            {
                timeSinceLastBeep = 0;
                AudioSource.PlayClipAtPoint(beep, transform.position);
            }
        }
        else
        {
            timeSinceLastBeep = Int16.MaxValue;
        }
    }

    void ShowWifiSymbols()
    {
        for (int i = 0; i < wifiThresholds.Length; i++)
        {
            wifiSprites[i].SetActive(normalizedDistance > wifiThresholds[i]);
        }
    }

    void PickUpItems()
    {
        if (wifiSprites[wifiSprites.Length - 1].activeSelf)
        {
            timeToAutoPickUp -= Time.deltaTime;
            if (Input.GetMouseButtonDown(0) || timeToAutoPickUp < 0)
            {
                Debug.Log("Picking up " + closestObject.name);
                timeToAutoPickUp = autoPickupInSeconds;
                var c = closestObject.GetComponent<NarrativeItem>();
                if (c != null)
                {
                    c.OnPickUp();
                }
            }
        }
        else
        {
            timeToAutoPickUp = autoPickupInSeconds;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        CalculateBeepHz();
        Beep();

        ShowWifiSymbols();
        PickUpItems();
        
        if (pickUpItem || Input.GetKeyUp(KeyCode.P))
        {
            pickUpItem = false;

            var c = FindObjectsOfType<NarrativeItem>().First();
            if (c != null)
            {
                c.OnPickUp();
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(this.transform.position + Vector3.up, new Vector3(1,2,1));
    }
}
