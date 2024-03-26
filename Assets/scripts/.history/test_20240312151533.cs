using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    public InputActionReference bendInputActionReference;
    public InputActionReference triggerInputActionReference;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float trigger = triggerInputActionReference.action.ReadValue<float>();
        Vector2 bend = bendInputActionReference.action.ReadValue<Vector2>();

        Debug.Log("trigger: " + trigger);
        Debug.Log("bend: " + bend);
        
    }
}
