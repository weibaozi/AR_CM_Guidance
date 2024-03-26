using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class test : MonoBehaviour
{
    // Start is called before the first frame update
    public InputActionReference testref;
    private KeyboardControll keyboardControll;
    

    private void Awake()
    {
        keyboardControll = new KeyboardControll();
    }

    private void OnEnable()
    {
        keyboardControll.Enable();
        testref.action.Enable();
    }

    private void OnDisable()
    {
        keyboardControll.Disable();
        testref.action.Disable();
    }

    private void Update()
    {
        Vector2 test = keyboardControll.CM.KeyBend.ReadValue<Vector2>();
        print(testref.action.ReadValue<float>());
        // print("test is "+test);  
        // print(keyboardControll.CM.KeyDrill.ReadValue<float>()); 
    }
}
