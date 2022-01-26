using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directional_Graph : MonoBehaviour
{
    public GameObject MoveTarget;
    public GameObject CloseTarget;
    public GameObject OpenTarget;
    List<GameObject> TargetList;
    List<Target> FinalTragetList;
    public GameObject obj;
    LineRenderer lineRenderer;
    private bool link = false;
    private bool first = true;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public Color c3 = Color.cyan;
    public Color c4 = Color.magenta;
    
    public void clear(){
        if(TargetList.Count > 0){
            for(int x = 0; x<=TargetList.Count-1; x++){
                Destroy(TargetList[x]);
            }
            link=false;
            TargetList.Clear();
            lineRenderer.positionCount = 0;
            FinalTragetList.Clear();
            Debug.Log(FinalTragetList.Count);
        }
    }

    //Removes Last Node
    public void RemoveLast(){
        if(TargetList.Count > 0){
            Destroy(TargetList[TargetList.Count-1]);
            TargetList.RemoveAt(TargetList.Count-1);
            lineRenderer.positionCount -= 1;
        }
    }
    public void AddGrabTarget(){
        if(TargetList.Count==0){
            if(first ==true){
                configure();
                first = false;
            }
            link=true;
            TargetList.Add((GameObject)Instantiate(CloseTarget, Camera.main.transform.position
            + Camera.main.transform.forward /2, Quaternion.LookRotation(Camera.main.transform.forward)));
        } else {
            TargetList.Add((GameObject)Instantiate(CloseTarget, Camera.main.transform.position
            + Camera.main.transform.forward/2, Quaternion.LookRotation(Camera.main.transform.forward)));
        }
    }
    public void AddOpenTarget(){
        if(TargetList.Count==0){
            if(first ==true){
                configure();
                first = false;
            }
            link=true;
            TargetList.Add((GameObject)Instantiate(OpenTarget, Camera.main.transform.position
            + Camera.main.transform.forward /2, Quaternion.LookRotation(Camera.main.transform.forward)));
        } else {
            TargetList.Add((GameObject)Instantiate(OpenTarget, Camera.main.transform.position 
            + Camera.main.transform.forward/2, Quaternion.LookRotation(Camera.main.transform.forward)));
        }
    }
    //Adds Target Nodes
    public void AddObject()
    {
        if(TargetList.Count==0){
            if(first ==true){
                configure();
                first = false;
            }
            link=true;
            TargetList.Add((GameObject)Instantiate(MoveTarget, Camera.main.transform.position 
            + Camera.main.transform.forward /2, Quaternion.LookRotation(Camera.main.transform.forward)));
        } else {
            TargetList.Add((GameObject)Instantiate(MoveTarget, Camera.main.transform.position 
            + Camera.main.transform.forward/2, Quaternion.LookRotation(Camera.main.transform.forward)));
        }
    }

    //Links new Target node with prior node && keeps paths updated
    void Link(){
        if(link == true){
            for(int x = 0; x<=TargetList.Count-1; x++){
                lineRenderer.positionCount = x+1;
                lineRenderer.SetPosition(x, TargetList[x].transform.position);
            }
        }
        
    }


    //Configures the path to parse to robot
    public void CreatePath(){
        for(int x =0; x<TargetList.Count; x++){
            if(TargetList[x].gameObject.CompareTag("Close")){
                FinalTragetList.Add(new Target(TargetList[x].transform.position.x, TargetList[x].transform.position.y,
                TargetList[x].transform.position.z, TargetList[x].transform.localRotation.eulerAngles.x,
                TargetList[x].transform.localRotation.eulerAngles.y, TargetList[x].transform.localRotation.eulerAngles.z, 2));
                Debug.Log(FinalTragetList[x].toString());

            } else if(TargetList[x].gameObject.CompareTag("Open")) {
                FinalTragetList.Add(new Target(TargetList[x].transform.position.x, TargetList[x].transform.position.y, 
                TargetList[x].transform.position.z, TargetList[x].transform.localRotation.eulerAngles.x,
                TargetList[x].transform.localRotation.eulerAngles.y, TargetList[x].transform.localRotation.eulerAngles.z, 1));
                Debug.Log(FinalTragetList[x].toString());

            }else {
                FinalTragetList.Add(new Target(TargetList[x].transform.position.x, TargetList[x].transform.position.y, 
                TargetList[x].transform.position.z, TargetList[x].transform.localRotation.eulerAngles.x,
                TargetList[x].transform.localRotation.eulerAngles.y, TargetList[x].transform.localRotation.eulerAngles.z, 0));
                Debug.Log(FinalTragetList[x].toString());
                
            }
        }
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c3, 0.0f), new GradientColorKey(c4, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;   
    }

    void configure(){
        
        lineRenderer = obj.AddComponent<LineRenderer>();
        lineRenderer.SetWidth(.01f,.01f); 
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
    }

    // Start is called before the first frame update
    void Start()
    {
        TargetList = new List<GameObject>();
        FinalTragetList = new List<Target>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Link();
    }
    
}

public class Target{
    double X;
    double Y;
    double Z;
    double rX;
    double rY;
    double rZ;
    int Grip;

    public Target(double x, double y, double z, double rx, double ry, double rz, int grip){
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.rX = rx;
        this.rY = ry;
        this.rZ = rz;
        this.Grip = grip;
    }
    
    public string toString(){
        string g = "";
        if(Grip == 1){
            g = "Open";
        } else if(Grip == 2){
            g = "Close";
        } else {
            g = "Move";
        }
        string re = "X: " + X + " Y: " + Y + " Z: " + Z + " rX: " + rX + " rY: " + rY + " rZ: " + rZ +" " + "Function: " +g;
        return re;
    }
}