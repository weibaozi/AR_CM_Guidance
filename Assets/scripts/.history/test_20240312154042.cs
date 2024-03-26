using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class test : MonoBehaviour
{
    // Start is called before the first frame update
    private KeyboardControll keyboardControll;

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

    private void update()
    {
        Vector2 test =keyboardControll.CM.KeyBend.ReadValue<Vector2>();
        print("test is "+test);   
    }
}
