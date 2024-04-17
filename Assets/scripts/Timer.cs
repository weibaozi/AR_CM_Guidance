using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    public float currentTime = 0f;

    public bool isTimeRunning = false;

    public MyUtils myUtils;

    // public GameObject timerText;

    public TextMeshPro timerText;

    // public void StartTimer()
    // {
    //     currentTime = 0f;
    //     isTimeRunning = true;
    // }

    // public void StopTimer()
    // {
    //     isTimeRunning = false;
    // }
    void Start()
    {
        // myUtils=GameObject.Find("MyUtils").GetComponent<MyUtils>();
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (myUtils.isRestart)
        {
            currentTime = 0f;
            isTimeRunning = false;
            timerText.text = "Time: 0  ";
        }
        if (myUtils.state == MyUtils.State.Drilling)
        {
            isTimeRunning = true;
        }
        if (myUtils.state == MyUtils.State.Finished)
        {
            isTimeRunning = false;
        }
        if (isTimeRunning)
        {
            currentTime += 1 * Time.deltaTime;
            //set timer text to "time: " + currentTime
            // timerText.GetComponent<TMPro.TextMeshProUGUI>().text = "Time: " + currentTime.ToString("0.00");
            List<float> dataArray = myUtils.accuracy.accuracies;
            int count=dataArray.ToArray().Length;
            timerText.text = "Time: " + currentTime.ToString("0.00") + " dc:  " + count.ToString();
        }
    }
}
