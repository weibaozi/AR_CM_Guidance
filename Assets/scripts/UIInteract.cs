using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIInteract : MonoBehaviour
{

    public MyUtils myUtils;
    public GameObject GuidingPointsSet;

    public GameObject[] Levels;

    void activeLevel(GameObject level)
    {
        for (int i = 0; i < level.transform.childCount; i++)
        {
            level.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    void deactivateLevel(GameObject level)
    {
        for (int i = 0; i < level.transform.childCount; i++)
        {
            level.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void EasyLevel()
    {
        print("Easy");
        myUtils.currentLevel = "Easy";
        // SceneManager.LoadScene("Easy");
        for (int i = 0; i < Levels.Length; i++)
        {
            if (Levels[i].name == "Easy")
            {
                activeLevel(Levels[i]);
            }
            else
            {
                deactivateLevel(Levels[i]);
            }
        }
        myUtils.restartFlag = true;

    }

    public void MediumLevel()
    {
        print("Medium");
        myUtils.currentLevel = "Medium";
        // SceneManager.LoadScene("Medium");
        for (int i = 0; i < Levels.Length; i++)
        {
            if (Levels[i].name == "Medium")
            {
                activeLevel(Levels[i]);
            }
            else
            {
                deactivateLevel(Levels[i]);
            }
        }
        myUtils.restartFlag = true;
    }

    public void LoadLevel(string level)
    {
        print("load level" + level);
        myUtils.currentLevel = level;
        for (int i = 0; i < Levels.Length; i++)
        {
            if (Levels[i].name == level)
            {
                activeLevel(Levels[i]);
            }
            else
            {
                deactivateLevel(Levels[i]);
            }
        }
        ReLoad();

    }    // Start is called before the first frame update

    public void RadarSwitch()
    {
        myUtils.isRadar = !myUtils.isRadar;
        myUtils.setRadarUI(myUtils.isRadar);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
        //reactive guidingpoints

        
    }

    public void ReLoad()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // for (int i = 0; i < myUtils.guidingPoints.Length; i++)
        // {
        //     myUtils.guidingPoints[i].SetActive(true);
        // }
        // myUtils.state = MyUtils.State.PreDrilling;
        myUtils.restartFlag = true;
    }


    public void VisualSwitch()
    {
        myUtils.isVisual = !myUtils.isVisual;
        myUtils.setVisual(myUtils.isVisual);
    }


    void Start()
    {

        var childcount = GuidingPointsSet.transform.childCount;
        Levels = new GameObject[childcount];
        for (int i = 0; i < childcount; i++)
        {
            Levels[i] = GuidingPointsSet.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
