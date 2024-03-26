using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalBarController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject BarArrowObject;
    public float BarRadius = 9.8f;

    public float scale = 4f;

    public float distance=0;



    // Start is called before the first frame update
    void Start()
    {
        if (BarArrowObject == null)
        {
            BarArrowObject = GameObject.Find("Vertical Bar Arrow");
        }
    }

    void Update()
    {
        float relativeDistance = distance *100f * scale;
        if (relativeDistance > BarRadius)
        {
            relativeDistance = BarRadius;
        }
        if (relativeDistance < -BarRadius)
        {
            relativeDistance = -BarRadius;
        }
        Vector3 relativePosition = new Vector3(relativeDistance,1.22f,0);
        BarArrowObject.transform.localPosition = relativePosition;

    }
}
