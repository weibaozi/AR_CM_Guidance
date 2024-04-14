using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
using System;


public class Accuracy : MonoBehaviour
{ 
    // Start is called before the first frame update
    public MyUtils myUtils;

    public int intermediatePointCount = 10;

    private GameObject nextPoint;

    private GameObject prevPoint;

    //for getting tip location
    private GameObject CM;

    private Vector3 tipLocation;

    public List<float> accuracies = new List<float>();

    public float avgAccuracy;
    void Start()
    {
        myUtils=GameObject.Find("MyUtils").GetComponent<MyUtils>();
        CM = GameObject.Find("Continuum_Manipulator");
        nextPoint = myUtils.nextPoint;
        prevPoint = myUtils.prevPoint;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
            
            
            Debug.Log("distance is "+distance);
            }
            //if distance is too far from previous distance setting, consider it noise
            print("shortest distance is "+shortestDistance);
            print("last accuracy is "+accuracies.Count);
            if (accuracies.Count == 0 || Math.Abs(shortestDistance - accuracies[accuracies.Count - 1]) < 1f)
            {
                //apend to accuracies
                print("test");
                accuracies.Add(shortestDistance);

            }
        }
        

        avgAccuracy = accuracies.Average();

    }
}
