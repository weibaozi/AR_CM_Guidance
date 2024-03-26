using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class Manipulator_Controller : MonoBehaviour
{
    public float speed = 0;
    public float bentSpeed = 0;

    private Rigidbody rb;
    //private float rotationZ;
    private float movementX;
    private float movementY;
    [SerializeField] private GameObject[] LinkObjects;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        var children = GetComponentsInChildren<Transform>();


        LinkObjects = System.Array.FindAll(children, p =>
        {
            return p.gameObject.name.Contains("Link");
        }).Select(p => p.gameObject).ToArray();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = -movementVector.x;
        movementY = movementVector.y;

    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(0.0f, movementX, 0.0f);
        rb.AddTorque(movement * speed);

        foreach (var link in LinkObjects) {
            link.transform.Rotate(new Vector3(0.0f, 0.0f, movementY * bentSpeed));
        }

        
    }
}
