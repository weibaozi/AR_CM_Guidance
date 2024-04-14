using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
public class MyUtils : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] guidingPoints;

    public GameObject[] activePoints;

    public GameObject nextPoint;

    public GameObject prevPoint;
    void Start()
    {
        // initial for guiding points
        var Points=GameObject.Find("Guiding Points").GetComponentsInChildren<Transform>();
        guidingPoints = Points.Skip(1).Select(p => p.gameObject).ToArray();
        nextPoint = guidingPoints[0];
        prevPoint = null;
        if (guidingPoints.Length == 0)
        {
            Debug.Log("No guiding points found");
        }
        else
        {
            Debug.Log("guiding points found");
            nextPoint = guidingPoints[0];
        }
        
    }

    // Update is called once per frame
    void PointUpdate()
    {
        prevPoint = nextPoint; 
        activePoints = guidingPoints.Where(p => p.activeSelf).ToArray();
        nextPoint = activePoints[0];

    }
    void Update()
    {
        PointUpdate();
        
    }
}
