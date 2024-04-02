using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

//control manipulator bends
public class ARManipulator_Controller : MonoBehaviour
{

    //joy stick
    // public InputActionReference joystickInputActionReference;
    public InputActionReference bendInputActionReference;
    public InputActionReference triggerInputActionReference;
    public float rotationSpeed = 50f;
    public float bentSpeed = 0.1f;

    public float maxAngel = 3f;
    public float trigger = 1f;
    public GameObject toolTipObject;
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
        bendInputActionReference.action.Enable();
        triggerInputActionReference.action.Enable();

        
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

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = -movementVector.x;
        movementY = movementVector.y;

    }

    void FixedUpdate()
    {
        //print primarybuttong value
        // rotationX = joystickInputActionReference.action.ReadValue<Vector2>().x;
        rotationX = bendInputActionReference.action.ReadValue<Vector2>().x;
        


        foreach (var link in LinkObjects) {
            if(Mathf.Abs(180-link.transform.localRotation.eulerAngles.z) > (180-maxAngel))
            {
                link.transform.Rotate(new Vector3(0.0f, 0.0f, -rotationX * bentSpeed));
            }
            //if angel is less than 10 degree and rotation is positive
            
            if ( (link.transform.localRotation.eulerAngles.z >maxAngel) && (link.transform.localRotation.eulerAngles.z < 180)) 
            {
                if ( -rotationX < 0)
                {
                    link.transform.Rotate(new Vector3(0.0f, 0.0f, -rotationX * bentSpeed));
                }
                
            }else if( (link.transform.localRotation.eulerAngles.z < (360-maxAngel)) && (link.transform.localRotation.eulerAngles.z > 180 ))
            {
                if (-rotationX > 0)
                {
                    link.transform.Rotate(new Vector3(0.0f, 0.0f, -rotationX * bentSpeed));
                }
            }else{
                link.transform.Rotate(new Vector3(0.0f, 0.0f, -rotationX * bentSpeed));
            }
        }
        //read button value
        trigger= triggerInputActionReference.action.ReadValue<float>();
        {
            //rotate tooltip
            toolTipObject.transform.Rotate(new Vector3(0.0f, trigger * rotationSpeed, 0.0f));
        }
        print(rotationX);
        print("trigger " + trigger);
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
