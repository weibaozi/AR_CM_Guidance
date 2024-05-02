using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class guiding : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject guidingManipulator;

    // public GameObject GuidingPointsObject;
    public GameObject[] guidingPoints;

    public MyUtils myUtils;

    void Start()
    {
        // myUtils=GameObject.Find("MyUtils").GetComponent<MyUtils>();
        guidingPoints = myUtils.guidingPoints;
        if (guidingPoints.Length == 0)
        {
            Debug.Log("No guiding points found");
        }
        else
        {
            Debug.Log("guiding points found");
            var firstPointLocation = guidingPoints[0].transform.position;
            var startlocation = GameObject.Find("Drill Point").transform.position;
            //move the manipulator to the start location
            guidingManipulator.transform.position = startlocation;
        }
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (myUtils.isRestart)
        {
            print("Restarting guiding");
            Start();
        }
        if (guidingPoints.Length == 0)
        {
            // Debug.Log("No guiding points found");
        }
        else
        {
            // Debug.Log("guiding points found");
            var firstPointLocation = guidingPoints[0].transform.position;
            var startlocation = GameObject.Find("Drill Point").transform.position;
            // print("1st point location is " + firstPointLocation + "start location is " + startlocation);
            Vector3 directionToTarget =    firstPointLocation - startlocation;
            //if z is positive
            // Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget,Vector3.down);
                // targetRotation = Quaternion.Euler(270, 0, 0) * targetRotation;
            guidingManipulator.transform.rotation = targetRotation;

            Plane pathPlane = myUtils.pathPlane;
            // Plane tip_plane = new Plane(guidingManipulator.transform.up, guidingManipulator.transform.position);

            //find Dihedral Angle in degree
            float angle = myUtils.CalculateDihedralAngle(pathPlane.normal,guidingManipulator.transform.up);
            //rotate the manipulator base on current angle
            // print("angle: "+angle);
            //calculate to rotate +angle or -angle
            guidingManipulator.transform.Rotate(0,0,angle,Space.Self);
            float angle2=myUtils.CalculateDihedralAngle(pathPlane.normal,guidingManipulator.transform.up);
            // print("angle2: "+angle2);
            if (angle2>5)
            {
                guidingManipulator.transform.Rotate(0,0,-angle*2,Space.Self);
            }
            // if (myUtils.CalculateDihedralAngle(pathPlane.normal,tip_plane.normal) > 5)
            // {
            //     guidingManipulator.transform.Rotate(0,0,-angle*2,Space.Self);
            // }
            // guidingManipulator.transform.Rotate(0,0,-180+angle,Space.Self);

            //if first guding point is deactivated, then deactivate the guiding manipulator
            if (guidingPoints[0].activeSelf == false)
            {
                guidingManipulator.SetActive(false);
            }else{
                guidingManipulator.SetActive(true);
            }
        }
    }
}
