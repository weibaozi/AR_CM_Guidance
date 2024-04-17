using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void EasyLevel()
    {
        print("Easy");
        SceneManager.LoadScene("Easy");
    }

    public void MediumLevel()
    {
        print("Button Clicked");
        SceneManager.LoadScene("Medium");
    }

    public void HardLevel()
    {
        print("Button Clicked");
    }
}
