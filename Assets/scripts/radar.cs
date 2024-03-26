using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class radar : MonoBehaviour
{
    public GameObject centerObject;

    public GameObject radarPointObject;
    public GameObject[] radarObjects;

    public float radarRadius = 6f;

    public float scale = 0.5f;

    private GameObject CurrentPoint;

    public GameObject tooltipGameObject;

    public GameObject VerticalBarGameObject;

    private TooltipController tooltipController;

    private VerticalBarController verticalBarController;
    // Start is called before the first frame update
    void Start()
    {
        if (centerObject == null)
        {
            centerObject = GameObject.Find("Continuum_Manipulator");
        }
        if (radarPointObject == null)
        {
            radarPointObject = GameObject.Find("radar point");
        }
        var Points=GameObject.Find("Guiding Points").GetComponentsInChildren<Transform>();
        radarObjects = Points.Skip(1).Select(p => p.gameObject).ToArray();
        var firstPointLocation = radarObjects[0].transform.position;
        Vector3 relativePosition = centerObject.transform.InverseTransformPoint(radarObjects[0].transform.position);
        print("relative position is " + relativePosition);
        //constrain the relative position to a circle(less than radarRadius)
        if (relativePosition.magnitude > radarRadius)
        {
            relativePosition = relativePosition.normalized * radarRadius;
        }

        // Vector3 radarPointLocation = new Vector3(-relativePosition.x, relativePosition.z,0 );
        Vector3 radarPointLocation = new Vector3(-relativePosition.x, relativePosition.z,0 );
        radarPointObject.transform.localPosition = radarPointLocation;

        tooltipController=tooltipGameObject.GetComponent<TooltipController>();
        verticalBarController=VerticalBarGameObject.GetComponent<VerticalBarController>();

        
    }
    void FixedUpdate(){
        var activePoints = radarObjects.Where(p => p.activeSelf);
        //number
        tooltipController.pointCount=activePoints.Count();
        if (activePoints.Count() == 0)
        {
            // Debug.Log("No guiding points found");
            CurrentPoint=null;
        }
        else
        {
            CurrentPoint=activePoints.FirstOrDefault();
            tooltipController.distance=(float)Vector3.Distance(CurrentPoint.transform.position, centerObject.transform.position);
            
            //find global y axis distance 
            verticalBarController.distance= (float)(CurrentPoint.transform.position.y-centerObject.transform.position.y);
            // verticalBarController.distance=(float)Vector3.Distance(CurrentPoint.transform.position, centerObject.transform.position);
            // print("current point is "+CurrentPoint);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        //find first active point
        var activePoints = radarObjects.Where(p => p.activeSelf);
        if (CurrentPoint == null)
        {
            // Debug.Log("No guiding points found");
        }
        else
        {
            Vector3 relativePosition = centerObject.transform.InverseTransformPoint(CurrentPoint.transform.position);
            //times scale
            relativePosition = relativePosition * scale;
            //constrain the relative position to a circle(less than radarRadius)
            if (relativePosition.magnitude > radarRadius)
            {
                relativePosition = relativePosition.normalized * radarRadius;
            }
            Vector3 radarPointLocation = new Vector3(-relativePosition.x, relativePosition.z,0 );
            radarPointObject.transform.localPosition = radarPointLocation;
        }
    }
}
