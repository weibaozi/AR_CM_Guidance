using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

//control manipulator bends
public class ArduinoCMControl : MonoBehaviour
{

    //joy stick
    // public InputActionReference joystickInputActionReference;

    public float rotationSpeed = 50f;

    public float maxAngel = 3f;
    public float trigger = 0f;
    public GameObject toolTipObject;

    public UDPListener uDPListener;

    public float CMSignal = 0;
    // private Rigidbody rb;
    private float movementX;
    private float movementY;

    private float rotationX;
    [SerializeField] private GameObject[] LinkObjects;

    private GameObject baseObject;

    
    
    // Start is called before the first frame update
    void Start()
    {
        // rb = GetComponent<Rigidbody>();
        //enable input action

        
        baseObject = gameObject;

        var children = GetComponentsInChildren<Transform>();


        LinkObjects = System.Array.FindAll(children, p =>
        {
            return p.gameObject.name.Contains("Link");
        }).Select(p => p.gameObject).ToArray();

        toolTipObject = System.Array.Find(children, p =>
        {
            return p.gameObject.name.Contains("ToolTip");
        }).gameObject;
    }

    void FixedUpdate()
    {
        
        //set bend status depends on CMSignal
        CMSignal = uDPListener.CMSignal;
        if (CMSignal > maxAngel)
        {
            CMSignal = maxAngel;
        }

        foreach (var link in LinkObjects) {
            //  Debug.Log($"Rotating {link.name} to Z rotation of {CMSignal}");
            link.transform.localRotation = Quaternion.Euler(0, 0, CMSignal);
        }
        //read button value
        // {
        //     //rotate tooltip
        //     toolTipObject.transform.Rotate(new Vector3(0.0f, trigger * rotationSpeed, 0.0f));
        // }
    }

    void Update()
    {
        // set tooltip script trigger value
        if (toolTipObject != null)
        {
            TooltipController tooltipController = toolTipObject.GetComponent<TooltipController>();
            tooltipController.trigger = trigger;
        }
    }
}
