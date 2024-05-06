using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
using System.Text;
using System.IO;
using TMPro;
using UnityEngine.UI;
public class MyUtils : MonoBehaviour
{
    // Start is called before the first frame update
    public enum State
    {
        PreDrilling,
        Drilling,
        Finished
    }

    public enum TutorialState
    {
        HitPoint,
        Radar,
        RotationArrow,
        VerticalBar,
        HorizontalBar,

        Finished
    }


    public GameObject[] guidingPoints;

    public GameObject[] activePoints;

    public GameObject nextPoint;

    public GameObject prevPoint;

    public State state=State.PreDrilling;

    public TutorialState tutorialState=TutorialState.HitPoint;

    public Plane pathPlane;

    public bool isRadar=true;

    public bool isVisual=true;

    public bool isRestart=false;

    public bool restartFlag=false;

    public bool isBending=false;

    public string currentLevel = "Easy1";
    public Accuracy accuracy;

    public Timer timer;

    public GameObject RadarUI;

    public GameObject bendPoint;

    public Vector3 startLocation;

    public UIInteract uIInteract;

    public TextMeshPro tutorialText;

    public GameObject tutorialTextPlate;

    public GameObject guidingManipulator;

    public GameObject RadarPoint;

    public GameObject RotationArrow;

    public GameObject VerticalArrow;

    public GameObject HorizontalArrow;

    public GameObject CM;

    public void ReStart()
    {
        //restart the game
        isRestart=true;
        isBending=false;
        //reactive guidingpoints
        // for (int i = 0; i < guidingPoints.Length; i++)
        // {
        //     guidingPoints[i].SetActive(true);
        // }
        state = State.PreDrilling;
        myStart();
    }

    public static void WriteToCSV(List<string> targetNames, List<float> accuracies,string filePath)
    {
        var file = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        var writer = new StreamWriter(file); 
        writer.Write("CurrentPoint,Accuracy\n");

        Debug.Log("dataArray count: " + targetNames.Count);
        
        // Create CSV format from array
        for (int i = 0; i < targetNames.ToArray().Length; i++)
        {
            // print("!!dataArray[i].Item1 is " + dataArray[i].Item1);
            writer.Write(targetNames[i] + "," + accuracies[i].ToString()+"\n");
        }

        writer.Close();
    }

    public void SaveResult()
    {
        //save as csv
        //get current object component
        string currentWorldTime = System.DateTime.Now.ToString("yyyy-MM-dd HH_mm");
        string SisRadar = isRadar ? "Radar" : "NoRadar";
        string SisVisual = isVisual ? "Visual" : "NoVisual";
        string filename = currentLevel + "_" + SisRadar + "_" + SisVisual + "_" + currentWorldTime + ".csv";
        var path = Path.Combine(Application.persistentDataPath, filename);
        var targetNames=accuracy.targetNames;
        var accuracies=accuracy.accuracies;
        WriteToCSV(targetNames, accuracies, path);
        
        

        Debug.Log("Save Result to :" + path);
    }

    public void setRadarUI(bool isRadar)
    {
        RadarUI.SetActive(isRadar);
    }

    public void setVisual(bool isVisual)
    {
        //set visual
        for (int i = 0; i < guidingPoints.Length; i++)
        {
            if (guidingPoints[i].activeSelf == false)
            {
                continue;
            }
            var pointMesh = guidingPoints[i].GetComponent<MeshRenderer>();
            pointMesh.enabled = isVisual;
        }
    }
    public float CalculateDihedralAngle(Vector3 n1, Vector3 n2)
    {
        n1.Normalize(); // Normalize the first normal vector
        n2.Normalize(); // Normalize the second normal vector

        float dotProduct = Vector3.Dot(n1, n2);
        float angleRadians = Mathf.Acos(dotProduct); // Get the angle in radians
        float angleDegrees = angleRadians * Mathf.Rad2Deg; // Convert radians to degrees

        return angleDegrees;
    }
    void find_path_plane(){
    // print("test");
    if (guidingPoints.Length < 3){
        Debug.Log("Not enough guiding points");
        return;
    }
    var pointA = guidingPoints[0].transform;
    var pointC = guidingPoints[guidingPoints.Length -1 ].transform;
    // var pointC = guidingPoints[4].transform;

    //random assign pointB
    // var random_index = Random.Range(1, guidingPoints.Length - 1);
    // var pointB = guidingPoints[random_index].transform;
    //assign pointB as the middle point
    var Length = guidingPoints.Length;
    var pointB = guidingPoints[(Length-1)/2].transform;
    // var pointB = guidingPoints[4].transform;

    //find the plane
    Vector3 center = (pointA.position + pointB.position + pointC.position) / 3;

    Vector3 normal = Vector3.Cross(pointC.position - pointA.position,pointB.position - pointA.position).normalized;
    // Vector3 normal = Vector3.Cross(pointB.position - pointA.position,pointC.position - pointA.position).normalized;
    Plane plane = new Plane(normal, center);
    print("!!C-A is " + (pointC.position - pointA.position) + "!!B-A is " + (pointB.position - pointA.position) + "!!normal is " + normal + "!!center is " + center);
    pathPlane = plane;
    // Debug.DrawLine(center, center + normal, Color.green, 1000f);
    // Debug.DrawLine(pointA.position, pointB.position, Color.white, 1000f);
    // Debug.DrawLine(pointA.position, pointC.position, Color.green, 1000f);
    // print("drawed line");
    print("!!Plane is " + plane);

    }

    void placeStraightPoints(GameObject[] Points){
        //find direction from drill point to Points and place the point in between
        var direction = Points[Points.Length - 1].transform.position - startLocation; 

        // Calculate the number of points needed to fill the gap
        var numPoints = Points.Length;

        // Place the points in between the drill point and the last point
        for (int i = 0; i <= numPoints - 2; i++)
        {
            var newPosition= startLocation + direction/(numPoints) * (i+1)    ;
            Points[i].transform.position = newPosition;
        }
    }
    void setChildMesh(GameObject parent, bool isVisual){
        //search for mesh renderer in children and children's children
        var mesh = parent.GetComponent<MeshRenderer>();
        if (mesh != null)
        {
            mesh.enabled = isVisual;
        }
        foreach (Transform child in parent.transform)
        {
            setChildMesh(child.gameObject,isVisual);
        }
    }

    void TutorialUpdate(){
        state = State.Drilling;
        if (currentLevel != "Tutorial"){
            return;
        }
        if (tutorialState == TutorialState.HitPoint){
            tutorialText.text = "Hit the point with the drill";
            if (activePoints.Length == 0){
                tutorialState = TutorialState.Radar;
                guidingPoints[1].SetActive(true);
                RadarPoint.GetComponent<MeshRenderer>().enabled = true;
            }
        }else if (tutorialState == TutorialState.Radar){
            tutorialText.text = "Watch the radar to drill the point";
            if (activePoints.Length == 0){
                tutorialState = TutorialState.RotationArrow;
                guidingPoints[2].SetActive(true);
                RotationArrow.GetComponent<RawImage>().enabled = true;
            }
        }else if (tutorialState == TutorialState.RotationArrow){
            tutorialText.text = "Align the red arrow the yellow line in the radar while keeping the red point in the center.";
            if (activePoints.Length == 0){
                tutorialState = TutorialState.VerticalBar;
                guidingPoints[3].SetActive(true);
                VerticalArrow.GetComponent<RawImage>().enabled = true;
            }
        }else if (tutorialState == TutorialState.VerticalBar){
            tutorialText.text = "The vertical arrow indicate the distance to the next point.";
            if (activePoints.Length == 0){
                tutorialState = TutorialState.HorizontalBar;
                guidingPoints[4].SetActive(true);
                HorizontalArrow.GetComponent<RawImage>().enabled = true;
                guidingPoints[4].transform.parent=CM.transform;
                guidingPoints[4].transform.localPosition=new Vector3((float)-14.8,-9,(float)-0.2);
                bendPoint.SetActive(false);
                isBending=true;
                
            }
        }else if (tutorialState == TutorialState.HorizontalBar){
            tutorialText.text = "Use the horizontal arrow indicate time to bend the drill(press trigger).";
            if (activePoints.Length == 0){
                tutorialState = TutorialState.Finished;
                guidingPoints[4].transform.parent=guidingPoints[3].transform.parent.transform;
                // guidingPoints[0].SetActive(true);
            }
        }else if (tutorialState == TutorialState.Finished){
            tutorialText.text = "Tutorial Finished";
            state = State.Finished;

        }

    }
    void myStart(){
        // initial for guiding points
        // var Points=GameObject.Find("Guiding Points").GetComponentsInChildren<Transform>();
        startLocation = GameObject.Find("Drill Point").transform.position;
        GameObject[] Points=GameObject.FindGameObjectsWithTag("Guiding Points");
        GameObject[] bendPoints = GameObject.FindGameObjectsWithTag("BendPoint");

        if (bendPoints.Length > 0)
        {
            bendPoint = bendPoints[0];
        }
        //sort by name
        Points = Points.OrderBy(go => go.name).ToArray();
        // if (currentLevel == "Medium1"){
        //     placeStraightPoints(Points);
        // }
        guidingPoints = Points.Select(p => p.gameObject).ToArray();

        nextPoint = guidingPoints[0];
        prevPoint = null;
        if (guidingPoints.Length == 0)
        {
            Debug.Log("No guiding points found");
        }
        else
        {
            Debug.Log("guiding points found");
            nextPoint = guidingPoints[0];
        }
        //print points length
        print("!!Points length is " + Points.Length);
        find_path_plane();

        accuracy=GetComponent<Accuracy>();
        timer=GetComponent<Timer>();

        if (currentLevel == "Tutorial")
        {
            tutorialTextPlate.SetActive(true);
            setChildMesh(guidingManipulator,false);
            tutorialState = TutorialState.HitPoint;
            //set the four guiding object mesh to false
            RadarPoint.GetComponent<MeshRenderer>().enabled = false;
            RotationArrow.GetComponent<RawImage>().enabled = false;
            VerticalArrow.GetComponent<RawImage>().enabled = false;
            HorizontalArrow.GetComponent<RawImage>().enabled = false;
            for (int i = 1; i < guidingPoints.Length; i++)
            {
                guidingPoints[i].SetActive(false);
            }
        }else{
            tutorialState = TutorialState.Finished;
            tutorialTextPlate.SetActive(false);
            setChildMesh(guidingManipulator,true);
            RadarPoint.GetComponent<MeshRenderer>().enabled = true;
            RotationArrow.GetComponent<RawImage>().enabled = true;
            VerticalArrow.GetComponent<RawImage>().enabled = true;
            HorizontalArrow.GetComponent<RawImage>().enabled = true;
        }
        
    }
    void Start()
    {
        // uIInteract.LoadLevel("Easy1");  
        
        myStart();
        uIInteract.LoadLevel("Tutorial");  
    }

    // Update is called once per frame
    void PointUpdate()
    {
        prevPoint = nextPoint; 
        activePoints = guidingPoints.Where(p => p.activeSelf).ToArray();
        if (activePoints.Length == 0)
        {
            print("No guiding points left");
            state = State.Finished;
            nextPoint = null;
        }
        else
        {
            nextPoint = activePoints[0];
        }

        if (bendPoint.activeSelf)
        {
            isBending=false;
        }
        else
        {
            isBending=true;
        }
    }

    void FixedUpdate()
    {
        if (isRestart)
        {
            isRestart=false;
        }
        PointUpdate();
        TutorialUpdate();
        //if first guiding point is deactivated, change state to drilling
        if (state == State.PreDrilling && guidingPoints[0].activeSelf == false)
        {
            state = State.Drilling;
        }
        //if no guiding points left, change state to finished
        
        
        if (restartFlag){

            ReStart();
            restartFlag=false;
        }
    }
}
