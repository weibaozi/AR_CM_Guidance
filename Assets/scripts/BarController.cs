using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BarController : MonoBehaviour
{
    public GameObject centerObject;

    public GameObject BarArrowObject;
    public GameObject[] radarObjects;

    public GameObject ManipulatorObject;

    public MyUtils myUtils;

    private bool isBendingPoint = false;

    private GameObject[] activePoints;

    private GameObject nextPoint;

    public float BarRadius = 9.8f;

    public float scale = 0.5f;

    private GameObject CurrentPoint;

    private ARManipulator_Controller manipulatorController;


    // Start is called before the first frame update
    void Start()
    {
        if (centerObject == null)
        {
            centerObject = GameObject.Find("Tool");
        }
        if (BarArrowObject == null)
        {
            BarArrowObject = GameObject.Find("Bar Arrow");
        }
        var Points=GameObject.Find("Guiding Points").GetComponentsInChildren<Transform>();
        radarObjects = Points.Skip(1).Select(p => p.gameObject).ToArray();
        var firstPointLocation = radarObjects[0].transform.position;
        // BarArrowObject.transform.localPosition = radarPointLocation;

        manipulatorController=ManipulatorObject.GetComponent<ARManipulator_Controller>();
        //set self deactive
        // gameObject.SetActive(false);


        
    }
    void FixedUpdate(){
        nextPoint = myUtils.nextPoint;
        isBendingPoint = myUtils.isBending;
        if (nextPoint == null)
        {
            // Debug.Log("No guiding points found");
        }
        else
        {
            float relativePositionX = centerObject.transform.InverseTransformPoint(nextPoint.transform.position).x;
            //times scale
            relativePositionX = relativePositionX * scale;
            //if x abs is larger than BarRadius, then set it to BarRadius
            if (Mathf.Abs(relativePositionX) > BarRadius)
            {
                relativePositionX = relativePositionX > 0 ? BarRadius : -BarRadius;
            }
            Vector3 radarPointLocation = new Vector3(relativePositionX, 0,0 );
            if (isBendingPoint)
            {
                print("bending");
                BarArrowObject.transform.localPosition = radarPointLocation;
            }
            
        }
    }
    // Update is called once per fram
}
