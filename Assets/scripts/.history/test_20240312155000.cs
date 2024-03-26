using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class test : MonoBehaviour
{
    // Start is called before the first frame update
    private KeyboardControll keyboardControll;
    private inputActionReference testref;

    private void Awake()
    {
        keyboardControll = new KeyboardControll();
    }

    private void OnEnable()
    {
        keyboardControll.Enable();
    }

    private void OnDisable()
    {
        keyboardControll.Disable();
    }

    private void Update()
    {
        Vector2 test = keyboardControll.CM.KeyBend.ReadValue<Vector2>();
        print("test is "+test);   
    }
}
