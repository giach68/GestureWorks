using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System.Globalization;

public class posTracker : MonoBehaviour
{
    public logger log= new logger();
    public Leap.Unity.LeapServiceProvider LP;
    public  GameObject left;
    public GameObject right;
    private long frame_offset;
    private float timer;
    public bool pos_left;
    public bool pos_right;
    public float sampling_rate;
    private string output = "";

    private string fileName;
    private string folder;

    // Start is called before the first frame update
    void Start()
    {
        //right = GameObject.Find("LoPoly Rigged Hand Right");
        //left = GameObject.Find("LoPoly Rigged Hand Left");
        frame_offset = LP.CurrentFrame.Id;
        timer = 0;
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        //output += " parse_left = " + (pos_left ? "true" : "false") + "\n";
        //output += " parse_right = " + (pos_right ? "true" : "false") + "\n";
        //output += " sampling_rate = " + sampling_rate.ToString() + "\n";
        //output += " data format = 0(sx)/1(dx):palmpos(x;y;z)|palmquat(x,y,z,w)|thumbApos(x;y;z)|thumbAquat(x;y;z;w)|" +
        //    "thumbBpos(x; y; z)|thumbBquat(x; y; z; w)|thumbEndpos(x;y;z)|thumbEndquat(x;y;z;w)|" +
        //    "indexApos(x;y;z)|indexAquat(x;y;z;w)|indexBpos(x;y;z)|indexBquat(x;y;z;w)|indexCpos(x;y;z)|indexCquat(x;y;z;w)|" +
        //    "indexEndpos(x;y;z)|indexEndquat(x;y;z;w)|middleApos(x;y;z)|middleAquat(x;y;z;w)|middleBpos(x;y;z)|middleBquat(x;y;z;w)|" +
        //    "middleCpos(x;y;z)|middleCquat(x;y;z;w)|middleEndpos(x;y;z)|middleEndquat(x;y;z;w)|ringApos(x;y;z)|ringAquat(x;y;z;w)|" +
        //    "ringBpos(x;y;z)|ringBquat(x;y;z;w)|ringCpos(x;y;z)|ringCquat(x;y;z;w)|ringEndpos(x;y;z)|ringEndquat(x;y;z;w)|" +
        //    "pinkyApos(x;y;z)|pinkyAquat(x;y;z;w)|pinkyBpos(x;y;z)|pinkyBquat(x;y;z;w)|pinkyCpos(x;y;z)|pinkyCquat(x;y;z;w)|" +
        //    "pinkyEndpos(x;y;z)|pinkyEndquat(x;y;z;w)|\n";
        //output += "\n";
        //output += "\n";
        //output += "\n";
    }

    public void setFilePath(string folder, string fileName)
    {
        this.folder = folder;
        this.fileName = fileName;
    }

    public void OnEnable()
    {
        //log.enabled = true;
        log.Enable(folder, fileName);
    }

    public void OnDisable()
    {
        log.writeData(output);
        output = "";
        log.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (frame_offset == 0)
            frame_offset = LP.CurrentFrame.Id;
        //float check_time = Mathf.Round((Time.unscaledTime * (1.0f / sampling_rate))) / (1.0f / sampling_rate);
        //if (check_time > timer)
        //{
        //    UpdateRecording();
        //}

    }

    public void UpdateRecording()
    {
        right = GameObject.Find("LoPoly Rigged Hand Right");
        left = GameObject.Find("LoPoly Rigged Hand Left");
        //timer = Mathf.Round((Time.unscaledTime * (1.0f / sampling_rate))) / (1.0f / sampling_rate);
        //Leap.Frame f = LP.CurrentFrame;
        //output += " frame number #" + (LP.CurrentFrame.Id - frame_offset).ToString() + "\n";
        if (pos_left) output += getHandInfo(left, 0) + "\n";
        if (pos_right) output += getHandInfo(right, 1) + "\n";
        log.writeData(output);
        output = "";
    }

    public void StartGesture(string gesture)
    {
        string input = "##;" + gesture + ";##\n";
        log.addToLine(input);
    }

    public void EndGesture()
    {
        string input = "##;##;##\n";
        log.addToLine(input);
    }

    string getFingerInfo(GameObject hand, string finger, int id)
    {
        float xPos = 0;
        float yPos = 0;
        float zPos = 0;
        string info = "";

        if(id == 0 && hand != null)
        {
            if (finger.Equals("thumb")){
                for(int i=1; i<4; i++) {
                    xPos = GameObject.FindWithTag("Lt"+ i.ToString()).transform.position.x;
                    yPos = GameObject.FindWithTag("Lt" + i.ToString()).transform.position.y;
                    zPos = GameObject.FindWithTag("Lt"+i.ToString()).transform.position.z;
                    info += xPos + ";";
                    info += yPos + ";";
                    info += zPos + ";";
                    info += GameObject.FindWithTag("Lt" + i.ToString()).transform.rotation.x.ToString() + ";";
                    info += GameObject.FindWithTag("Lt" + i.ToString()).transform.rotation.y.ToString() + ";";
                    info += GameObject.FindWithTag("Lt" + i.ToString()).transform.rotation.z.ToString() + ";";
                    info += GameObject.FindWithTag("Lt" + i.ToString()).transform.rotation.w.ToString() + ";";
                }
            }
            else if (finger.Equals("index"))
            {
                for (int i = 1; i < 5; i++)
                {
                    xPos = GameObject.FindWithTag("Li" + i.ToString()).transform.position.x;
                    yPos = GameObject.FindWithTag("Li" + i.ToString()).transform.position.y;
                    zPos = GameObject.FindWithTag("Li" + i.ToString()).transform.position.z;
                    info += xPos + ";";
                    info += yPos + ";";
                    info += zPos + ";";
                    info += GameObject.FindWithTag("Li" + i.ToString()).transform.rotation.x.ToString() + ";";
                    info += GameObject.FindWithTag("Li" + i.ToString()).transform.rotation.y.ToString() + ";";
                    info += GameObject.FindWithTag("Li" + i.ToString()).transform.rotation.z.ToString() + ";";
                    info += GameObject.FindWithTag("Li" + i.ToString()).transform.rotation.w.ToString() + ";";
                }
            }
            else if (finger.Equals("middle"))
            {
                for (int i = 1; i < 5; i++)
                {
                    xPos = GameObject.FindWithTag("Lm" + i.ToString()).transform.position.x;
                    yPos = GameObject.FindWithTag("Lm" + i.ToString()).transform.position.y;
                    zPos = GameObject.FindWithTag("Lm" + i.ToString()).transform.position.z;
                    info += xPos + ";";
                    info += yPos + ";";
                    info += zPos + ";";
                    info += GameObject.FindWithTag("Lm" + i.ToString()).transform.rotation.x.ToString() + ";";
                    info += GameObject.FindWithTag("Lm" + i.ToString()).transform.rotation.y.ToString() + ";";
                    info += GameObject.FindWithTag("Lm" + i.ToString()).transform.rotation.z.ToString() + ";";
                    info += GameObject.FindWithTag("Lm" + i.ToString()).transform.rotation.w.ToString() + ";";
                }
            }
            else if (finger.Equals("ring"))
            {
                for (int i = 1; i < 5; i++)
                {
                    xPos = GameObject.FindWithTag("Lr" + i.ToString()).transform.position.x;
                    yPos = GameObject.FindWithTag("Lr" + i.ToString()).transform.position.y;
                    zPos = GameObject.FindWithTag("Lr" + i.ToString()).transform.position.z;
                    info += xPos + ";";
                    info += yPos + ";";
                    info += zPos + ";";
                    info += "(" + GameObject.FindWithTag("Lr" + i.ToString()).transform.rotation.x.ToString() + ";";
                    info += GameObject.FindWithTag("Lr" + i.ToString()).transform.rotation.y.ToString() + ";";
                    info += GameObject.FindWithTag("Lr" + i.ToString()).transform.rotation.z.ToString() + ";";
                    info += GameObject.FindWithTag("Lr" + i.ToString()).transform.rotation.w.ToString() + ";";
                }
            }
            else if (finger.Equals("pinky"))
            {
                for (int i = 1; i < 5; i++)
                {
                    xPos = GameObject.FindWithTag("Lp" + i.ToString()).transform.position.x;
                    yPos = GameObject.FindWithTag("Lp" + i.ToString()).transform.position.y;
                    zPos = GameObject.FindWithTag("Lp" + i.ToString()).transform.position.z;
                    info += xPos + ";";
                    info += yPos + ";";
                    info += zPos + ";";
                    info += GameObject.FindWithTag("Lp" + i.ToString()).transform.rotation.x.ToString() + ";";
                    info += GameObject.FindWithTag("Lp" + i.ToString()).transform.rotation.y.ToString() + ";";
                    info += GameObject.FindWithTag("Lp" + i.ToString()).transform.rotation.z.ToString() + ";";
                    info += GameObject.FindWithTag("Lp" + i.ToString()).transform.rotation.w.ToString() + ";";
                }
            }
        }
        else if(id == 1 && hand != null)
        {
            if (finger.Equals("thumb"))
            {
                for (int i = 1; i < 4; i++)
                {
                    xPos = GameObject.FindWithTag("Rt" + i.ToString()).transform.position.x;
                    yPos = GameObject.FindWithTag("Rt" + i.ToString()).transform.position.y;
                    zPos = GameObject.FindWithTag("Rt" + i.ToString()).transform.position.z;
                    info += xPos + ";";
                    info += yPos + ";";
                    info += zPos + ";";
                    info += GameObject.FindWithTag("Rt" + i.ToString()).transform.rotation.x.ToString() + ";";
                    info += GameObject.FindWithTag("Rt" + i.ToString()).transform.rotation.y.ToString() + ";";
                    info += GameObject.FindWithTag("Rt" + i.ToString()).transform.rotation.z.ToString() + ";";
                    info += GameObject.FindWithTag("Rt" + i.ToString()).transform.rotation.w.ToString() + ";";
                }
            }
            else if (finger.Equals("index"))
            {
                for (int i = 1; i < 5; i++)
                {
                    xPos = GameObject.FindWithTag("Ri" + i.ToString()).transform.position.x;
                    yPos = GameObject.FindWithTag("Ri" + i.ToString()).transform.position.y;
                    zPos = GameObject.FindWithTag("Ri" + i.ToString()).transform.position.z;
                    info += xPos + ";";
                    info += yPos + ";";
                    info += zPos + ";";
                    info += GameObject.FindWithTag("Ri" + i.ToString()).transform.rotation.x.ToString() + ";";
                    info += GameObject.FindWithTag("Ri" + i.ToString()).transform.rotation.y.ToString() + ";";
                    info += GameObject.FindWithTag("Ri" + i.ToString()).transform.rotation.z.ToString() + ";";
                    info += GameObject.FindWithTag("Ri" + i.ToString()).transform.rotation.w.ToString() + ";";
                }
            }
            else if (finger.Equals("middle"))
            {
                for (int i = 1; i < 5; i++)
                {
                    xPos = GameObject.FindWithTag("Rm" + i.ToString()).transform.position.x;
                    yPos = GameObject.FindWithTag("Rm" + i.ToString()).transform.position.y;
                    zPos = GameObject.FindWithTag("Rm" + i.ToString()).transform.position.z;
                    info += xPos + ";";
                    info += yPos + ";";
                    info += zPos + ";";
                    info += GameObject.FindWithTag("Rm" + i.ToString()).transform.rotation.x.ToString() + ";";
                    info += GameObject.FindWithTag("Rm" + i.ToString()).transform.rotation.y.ToString() + ";";
                    info += GameObject.FindWithTag("Rm" + i.ToString()).transform.rotation.z.ToString() + ";";
                    info += GameObject.FindWithTag("Rm" + i.ToString()).transform.rotation.w.ToString() + ";";
                }
            }
            else if (finger.Equals("ring"))
            {
                for (int i = 1; i < 5; i++)
                {
                    xPos = GameObject.FindWithTag("Rr" + i.ToString()).transform.position.x;
                    yPos = GameObject.FindWithTag("Rr" + i.ToString()).transform.position.y;
                    zPos = GameObject.FindWithTag("Rr" + i.ToString()).transform.position.z;
                    info += xPos + ";";
                    info += yPos + ";";
                    info += zPos + ";";
                    info += GameObject.FindWithTag("Rr" + i.ToString()).transform.rotation.x.ToString() + ";";
                    info += GameObject.FindWithTag("Rr" + i.ToString()).transform.rotation.y.ToString() + ";";
                    info += GameObject.FindWithTag("Rr" + i.ToString()).transform.rotation.z.ToString() + ";";
                    info += GameObject.FindWithTag("Rr" + i.ToString()).transform.rotation.w.ToString() + ";";
                }
            }
            else if (finger.Equals("pinky"))
            {
                for (int i = 1; i < 5; i++)
                {
                    xPos = GameObject.FindWithTag("Rp" + i.ToString()).transform.position.x;
                    yPos = GameObject.FindWithTag("Rp" + i.ToString()).transform.position.y;
                    zPos = GameObject.FindWithTag("Rp" + i.ToString()).transform.position.z;
                    info += xPos + ";";
                    info += yPos + ";";
                    info += zPos + ";";
                    info += GameObject.FindWithTag("Rp" + i.ToString()).transform.rotation.x.ToString() + ";";
                    info += GameObject.FindWithTag("Rp" + i.ToString()).transform.rotation.y.ToString() + ";";
                    info += GameObject.FindWithTag("Rp" + i.ToString()).transform.rotation.z.ToString() + ";";
                    info += GameObject.FindWithTag("Rp" + i.ToString()).transform.rotation.w.ToString() + ";";
                }
            }
        }
        else
        {

            info = "???";
            info += id.ToString() + ";"; //0 for left hand Id
            info += "invalid_hand;";
            return info;
        }

        return info;
    }

    string getHandInfo(GameObject hand, int id)
    {
        float xPos = 0;
        float yPos = 0;
        float zPos = 0;
        string info = "";
        if (id == 1 && hand != null)
        {
            //info = id.ToString() + ":"; 
            xPos = GameObject.FindWithTag("RP").transform.position.x;
            yPos = GameObject.FindWithTag("RP").transform.position.y;
            zPos = GameObject.FindWithTag("RP").transform.position.z;

            info += xPos + ";";
            info += yPos + ";";
            info += zPos + ";";
            info += GameObject.FindWithTag("RP").transform.rotation.x + ";";
            info += GameObject.FindWithTag("RP").transform.rotation.y + ";";
            info += GameObject.FindWithTag("RP").transform.rotation.z + ";";
            info += GameObject.FindWithTag("RP").transform.rotation.w + ";";
           
            info += getFingerInfo(hand, "thumb", id);
            info += getFingerInfo(hand, "index", id);
            info += getFingerInfo(hand, "middle", id);
            info += getFingerInfo(hand, "ring", id);
            info += getFingerInfo(hand, "pinky", id);
            //add any more needed info here
           
            return info;
        }
        else if(id == 0 && hand != null)
        {
            info = id.ToString() + ":";
            xPos = GameObject.FindWithTag("LP").transform.position.x;
            yPos = GameObject.FindWithTag("LP").transform.position.y;
            zPos = GameObject.FindWithTag("LP").transform.position.z;

            info += "palmpos(" + xPos + ";";
            info += yPos + ";";
            info += zPos + ")";
            info += " | ";
            info += "parmrot(" + GameObject.FindWithTag("LP").transform.rotation.x + ";";
            info += GameObject.FindWithTag("LP").transform.rotation.y + ";";
            info += GameObject.FindWithTag("LP").transform.rotation.z + ";";
            info += GameObject.FindWithTag("LP").transform.rotation.w + ")";

            info += getFingerInfo(hand, "thumb", id);
            info += getFingerInfo(hand, "index", id);
            info += getFingerInfo(hand, "middle", id);
            info += getFingerInfo(hand, "ring", id);
            info += getFingerInfo(hand, "pinky", id) + "|" + timer;
            return info;
        }
        else
        {

            info = "???";
            info += id.ToString() + ";"; //0 for left hand Id
            info += "invalid_hand;";
            return info;

        }
    }
}
