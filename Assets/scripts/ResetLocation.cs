using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ResetLocation : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 originalPosition;

    public InputActionReference resetInputActionReference;
    void Start()
    {
        originalPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (resetInputActionReference != null)
        {
           float reset = resetInputActionReference.action.ReadValue<float>();
            if (reset > 0.5f)
            {
                transform.position = originalPosition;
            }
        }
        
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reset Plane"))
        {
            transform.position = originalPosition;
        }
    }
}
