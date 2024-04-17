using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
using System;
using TMPro;

public class Accuracy : MonoBehaviour
{ 
    // Start is called before the first frame update
    public MyUtils myUtils;

    public int intermediatePointCount = 10;

    private GameObject nextPoint;

    private GameObject prevPoint;

    //for getting tip location
    public GameObject CM;

    private Vector3 tipLocation;

    public List<float> accuracies = new List<float>();

    public List<string> targetNames = new List<string>();
    public List<(string, float)> accuraciesWithTarget = new List<(string, float)>();

    public float avgAccuracy;

    public TextMeshPro TimerText;


    void ReStart()
    {
        accuracies.Clear();
        targetNames.Clear();
        accuraciesWithTarget.Clear();
    } 
    void Start()
    {
        myUtils=GetComponent<MyUtils>();
        // CM = GameObject.Find("Continuum_Manipulator");
        nextPoint = myUtils.nextPoint;
        prevPoint = myUtils.prevPoint;
        // TimerText.text = "test init";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (myUtils.isRestart)
        {
            ReStart();
        }
        nextPoint = myUtils.nextPoint;
        prevPoint = myUtils.prevPoint;
        tipLocation = CM.transform.position;
        //find shortest distance between tip location and path between next and prev point
        var shortestDistance = 9999f;
        if (myUtils.state == MyUtils.State.Drilling){ 
            for (int i = 0; i < intermediatePointCount; i++)
            {
            
                var interLocation = prevPoint.transform.position + i/intermediatePointCount * (nextPoint.transform.position - prevPoint.transform.position);
                var distance = Vector3.Distance(tipLocation, interLocation);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                }
            
            }
            if (accuracies.Count == 0 || Math.Abs(shortestDistance - accuracies[accuracies.Count - 1]) < 1f)
            // if (accuracies.Count == 0 || true)
            {
                //apend to accuracies
                accuracies.Add(shortestDistance);
                targetNames.Add(nextPoint.name);
                accuraciesWithTarget.Add((nextPoint.name, shortestDistance));
            }
        }
        
        // avgAccuracy = accuracies.Average();

    }
}
