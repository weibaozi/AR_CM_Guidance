using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TooltipSelectController : MonoBehaviour
{
    // Start is called before the first frame update


    public InputActionReference triggerInputActionReference;
    public AudioSource audioSource;
    //scenemanager
    [SerializeField] public string Easy;
    [SerializeField] public string Hard;
    [SerializeField] public string VeryHard;

    [SerializeField] public string Menu;

    private float trigger = 0f;
    void Start()
    {

        
    }

    void Update()
    {
        trigger = triggerInputActionReference.action.ReadValue<float>();
    }

    // Update is called once per frame
   void OnTriggerEnter(Collider other){
        
        if (other.gameObject.CompareTag("Level Selector"))
        {
            if (trigger > 0.5f)
            {
                audioSource.Play();
                // print("point is triggered "+other.gameObject.name);
                if (other.gameObject.name == Easy)
                {
                    // print("easy");
                    SceneManager.LoadScene(Easy);
                }
                if (other.gameObject.name == Hard)
                {
                    // print("medium");
                    SceneManager.LoadScene(Hard);
                }
                if (other.gameObject.name == VeryHard)
                {
                    // print("hard");
                    SceneManager.LoadScene(VeryHard);
                }
                if (other.gameObject.name == Menu)
                {
                    SceneManager.LoadScene(Menu);
                }
            }
        }
    }
}
