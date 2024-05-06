using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class NewRadar : MonoBehaviour
{
    public MyUtils myUtils;
    public GameObject centerObject;

    public GameObject radarPointObject;

    public float radarRadius = 6f;

    public float scale = 0.5f;

    public GameObject rotationArrow;

    private GameObject nextPoint;

    public GameObject VerticalBarGameObject;

    public Plane pathPlane;

    private VerticalBarController verticalBarController;
    // Start is called before the first frame update
    private GameObject[] guidingPoints;

    private GameObject[] activePoints;

    private Transform pointA;
    private Transform pointB;
    private Transform pointC;



    void Restart()
    {
        pathPlane = myUtils.pathPlane;
    }
    void align_rotation_arrow()
    {

        Plane tip_plane = new Plane(centerObject.transform.forward, centerObject.transform.position);
        //find Dihedral Angle in degree
        float angle = myUtils.CalculateDihedralAngle(tip_plane.normal, pathPlane.normal);
        rotationArrow.transform.localEulerAngles = new Vector3(0, 0, angle+180);
        print("angle is "+angle);

    }
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

        verticalBarController=VerticalBarGameObject.GetComponent<VerticalBarController>();

        pathPlane = myUtils.pathPlane;
    }



    void FixedUpdate(){
        guidingPoints = myUtils.guidingPoints;
        activePoints = myUtils.activePoints;
        nextPoint = myUtils.nextPoint;
        //number
        //find global l2 distance
        if (nextPoint!=null){
            // verticalBarController.distance= (float)(nextPoint.transform.position.y-centerObject.transform.position.y);
            verticalBarController.distance=  Vector3.Distance(nextPoint.transform.position,centerObject.transform.position);
            align_rotation_arrow();
        }
        if (myUtils.isRestart){
            Restart();
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
