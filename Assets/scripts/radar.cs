using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class radar : MonoBehaviour
{
    public MyUtils myUtils;
    public GameObject centerObject;

    public GameObject radarPointObject;

    public float radarRadius = 6f;

    public float scale = 0.5f;

    private GameObject nextPoint;

    public GameObject tooltipGameObject;

    public GameObject VerticalBarGameObject;

    private TooltipController tooltipController;

    private VerticalBarController verticalBarController;
    // Start is called before the first frame update
    private GameObject[] guidingPoints;

    private GameObject[] activePoints;
    void Start()
    {
        myUtils=GameObject.Find("MyUtils").GetComponent<MyUtils>();
        if (centerObject == null)
        {
            centerObject = GameObject.Find("Continuum_Manipulator");
        }
        if (radarPointObject == null)
        {
            radarPointObject = GameObject.Find("radar point");
        }

        guidingPoints = myUtils.guidingPoints;
        activePoints = myUtils.activePoints;
        var firstPointLocation = guidingPoints[0].transform.position;
        Vector3 relativePosition = centerObject.transform.InverseTransformPoint(guidingPoints[0].transform.position);
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
        guidingPoints = myUtils.guidingPoints;
        activePoints = myUtils.activePoints;
        //number
        tooltipController.pointCount=activePoints.Count();
        if (activePoints.Count() == 0)
        {
            // Debug.Log("No guiding points found");
            nextPoint=null;
        }
        else
        {
            nextPoint=activePoints.FirstOrDefault();
            tooltipController.distance=(float)Vector3.Distance(nextPoint.transform.position, centerObject.transform.position);
            
            //find global y axis distance 
            verticalBarController.distance= (float)(nextPoint.transform.position.y-centerObject.transform.position.y);
            // verticalBarController.distance=(float)Vector3.Distance(nextPoint.transform.position, centerObject.transform.position);
            // print("current point is "+nextPoint);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        //find first active point
        if (nextPoint == null)
        {
            // Debug.Log("No guiding points found");
        }
        else
        {
            Vector3 relativePosition = centerObject.transform.InverseTransformPoint(nextPoint.transform.position);
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
