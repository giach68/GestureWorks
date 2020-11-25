using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System.Globalization;

//raccoglie dati dalla mano
public class PosTracker : MonoBehaviour
{
    public FileLogger log= new FileLogger();
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
    }

    // Update is called once per frame
    void Update()
    {
        if (frame_offset == 0)
            frame_offset = LP.CurrentFrame.Id;
    }

    public void SetFilePath(string folder, string fileName)
    {
        this.folder = folder;
        this.fileName = fileName;
    }

    //chiamata da unity quando il componente è attivato (spunta)
    //in questo caso, viene attivato da recorder (non è chiamato in automatico, ma è invocato da recorder quando si mette a true con pt.enabled = true)
    public void OnEnable()
    {
        //log.enabled = true;
        log.Enable(folder, fileName);
    }

    //chiamata da unity quando il componente è disattivato (tolgo spunta)
    //in questo caso, viene disattivato da recorder (non è chiamato in automatico, ma è invocato da recorder quando si mette a false con pt.enabled = false)
    public void OnDisable()
    {
        log.WriteData(output);
        output = "";
        log.Disable();
    }

    public void UpdateRecording()
    {
        right = GameObject.Find("LoPoly Rigged Hand Right");
        left = GameObject.Find("LoPoly Rigged Hand Left");

        if (pos_left) 
            output += GetHandInfo(left, HandType.Left);
        if (pos_right) 
            output += GetHandInfo(right, HandType.Right);

        log.WriteData(output);
        output = "";
    }

    public void StartGesture(string gesture)
    {
        //scrivo inizio delimitazione gesto
        string input = "##;" + gesture + ";##";
        log.AddToLine(input);
    }

    public void EndGesture()
    {
        StartGesture("##");
    }

    //id: che mano è (0 sx, 1 dx), vedi invocazione
    string GetHandInfo(GameObject hand, HandType id)
    { 
        string info = "";

        //dx
        if (id.Equals(HandType.Right) && hand != null)
        {
            //info = id.ToString() + ":"; 

            //Wrist
            info += GameObject.FindWithTag("RW").transform.position.x + ";" +
                    GameObject.FindWithTag("RW").transform.position.y + ";" +
                    GameObject.FindWithTag("RW").transform.position.z + ";";

            //Wrist quaternions
            info += GameObject.FindWithTag("RW").transform.rotation.x + ";" +
                    GameObject.FindWithTag("RW").transform.rotation.y + ";" +
                    GameObject.FindWithTag("RW").transform.rotation.z + ";" +
                    GameObject.FindWithTag("RW").transform.rotation.w + ";";

            //Palm
            info += GameObject.FindWithTag("RP").transform.position.x + ";" + //vedi tag su oggetto R_Palm su Unity - 1 tag 1 oggetto solo
                    GameObject.FindWithTag("RP").transform.position.y + ";" +
                    GameObject.FindWithTag("RP").transform.position.z + ";";

            //Palm quaternions
            info += GameObject.FindWithTag("RP").transform.rotation.x + ";" +
                    GameObject.FindWithTag("RP").transform.rotation.y + ";" +
                    GameObject.FindWithTag("RP").transform.rotation.z + ";" +
                    GameObject.FindWithTag("RP").transform.rotation.w + ";";

            info += GetFingerInfo(hand, "thumb", id);
            info += GetFingerInfo(hand, "index", id);
            info += GetFingerInfo(hand, "middle", id);
            info += GetFingerInfo(hand, "ring", id);
            info += GetFingerInfo(hand, "pinky", id);
            //add any more needed info here

            return info;
        }
        //sx
        else if (id.Equals(HandType.Left) && hand != null)
        {
            //Wrist
            info += GameObject.FindWithTag("LW").transform.position.x + ";" +
                    GameObject.FindWithTag("LW").transform.position.y + ";" +
                    GameObject.FindWithTag("LW").transform.position.z + ";";

            //Wrist quaternions
            info += GameObject.FindWithTag("LW").transform.rotation.x + ";" +
                    GameObject.FindWithTag("LW").transform.rotation.y + ";" +
                    GameObject.FindWithTag("LW").transform.rotation.z + ";" +
                    GameObject.FindWithTag("LW").transform.rotation.w + ";";

            //Palm
            info += GameObject.FindWithTag("LP").transform.position.x + ";" + //vedi tag su oggetto R_Palm su Unity - 1 tag 1 oggetto solo
                    GameObject.FindWithTag("LP").transform.position.y + ";" +
                    GameObject.FindWithTag("LP").transform.position.z + ";";

            //Palm quaternions
            info += GameObject.FindWithTag("LP").transform.rotation.x + ";" +
                    GameObject.FindWithTag("LP").transform.rotation.y + ";" +
                    GameObject.FindWithTag("LP").transform.rotation.z + ";" +
                    GameObject.FindWithTag("LP").transform.rotation.w + ";";

            info += GetFingerInfo(hand, "thumb", id);
            info += GetFingerInfo(hand, "index", id);
            info += GetFingerInfo(hand, "middle", id);
            info += GetFingerInfo(hand, "ring", id);
            info += GetFingerInfo(hand, "pinky", id);// + "|" + timer;
            return info;
        }
        //errore
        else
        {
            info = "???";
            info += id + ";";
            info += "invalid_hand;";
            return info;

        }
    }

    string GetFingerInfo(GameObject hand, string finger, HandType id)
    {
        string info = "";

        //left
        if(id.Equals(HandType.Left) && hand != null)
        {
            if (finger.Equals("thumb")){
                //thumb has one less node than the other fingers
                for(int i=1; i<4; i++) {
                    info += GameObject.FindWithTag("Lt"+ i.ToString()).transform.position.x + ";" +
                            GameObject.FindWithTag("Lt" + i.ToString()).transform.position.y + ";" +
                            GameObject.FindWithTag("Lt"+ i.ToString()).transform.position.z +";";

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
                    info += GameObject.FindWithTag("Li" + i.ToString()).transform.position.x + ";" +
                            GameObject.FindWithTag("Li" + i.ToString()).transform.position.y + ";" +
                            GameObject.FindWithTag("Li" + i.ToString()).transform.position.z + ";";
                    
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
                    info += GameObject.FindWithTag("Lm" + i.ToString()).transform.position.x + ";" +
                            GameObject.FindWithTag("Lm" + i.ToString()).transform.position.y + ";" +
                            GameObject.FindWithTag("Lm" + i.ToString()).transform.position.z + ";";

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
                    info += GameObject.FindWithTag("Lr" + i.ToString()).transform.position.x + ";" +
                            GameObject.FindWithTag("Lr" + i.ToString()).transform.position.y + ";" +
                            GameObject.FindWithTag("Lr" + i.ToString()).transform.position.z + ";";

                    info += GameObject.FindWithTag("Lr" + i.ToString()).transform.rotation.x.ToString() + ";";
                    info += GameObject.FindWithTag("Lr" + i.ToString()).transform.rotation.y.ToString() + ";";
                    info += GameObject.FindWithTag("Lr" + i.ToString()).transform.rotation.z.ToString() + ";";
                    info += GameObject.FindWithTag("Lr" + i.ToString()).transform.rotation.w.ToString() + ";";
                }
            }
            else if (finger.Equals("pinky"))
            {
                for (int i = 1; i < 5; i++)
                {
                    info += GameObject.FindWithTag("Lp" + i.ToString()).transform.position.x + ";" +
                            GameObject.FindWithTag("Lp" + i.ToString()).transform.position.y + ";" +
                            GameObject.FindWithTag("Lp" + i.ToString()).transform.position.z + ";";

                    info += GameObject.FindWithTag("Lp" + i.ToString()).transform.rotation.x.ToString() + ";";
                    info += GameObject.FindWithTag("Lp" + i.ToString()).transform.rotation.y.ToString() + ";";
                    info += GameObject.FindWithTag("Lp" + i.ToString()).transform.rotation.z.ToString() + ";";
                    info += GameObject.FindWithTag("Lp" + i.ToString()).transform.rotation.w.ToString() + ";";
                }
            }
        }
        //right
        else if(id.Equals(HandType.Right) && hand != null)
        {
            if (finger.Equals("thumb"))
            {
                for (int i = 1; i < 4; i++)
                {
                    info += GameObject.FindWithTag("Rt" + i.ToString()).transform.position.x + ";" +
                            GameObject.FindWithTag("Rt" + i.ToString()).transform.position.y + ";" +
                            GameObject.FindWithTag("Rt" + i.ToString()).transform.position.z + ";";

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
                    info += GameObject.FindWithTag("Ri" + i.ToString()).transform.position.x + ";" +
                            GameObject.FindWithTag("Ri" + i.ToString()).transform.position.y + ";" +
                            GameObject.FindWithTag("Ri" + i.ToString()).transform.position.z + ";";

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
                    info += GameObject.FindWithTag("Rm" + i.ToString()).transform.position.x + ";" +
                            GameObject.FindWithTag("Rm" + i.ToString()).transform.position.y + ";" +
                            GameObject.FindWithTag("Rm" + i.ToString()).transform.position.z + ";";

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
                    info += GameObject.FindWithTag("Rr" + i.ToString()).transform.position.x + ";" +
                            GameObject.FindWithTag("Rr" + i.ToString()).transform.position.y + ";" +
                            GameObject.FindWithTag("Rr" + i.ToString()).transform.position.z + ";";

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
                    info += GameObject.FindWithTag("Rp" + i.ToString()).transform.position.x + ";" +
                            GameObject.FindWithTag("Rp" + i.ToString()).transform.position.y + ";" +
                            GameObject.FindWithTag("Rp" + i.ToString()).transform.position.z + ";";

                    info += GameObject.FindWithTag("Rp" + i.ToString()).transform.rotation.x.ToString() + ";";
                    info += GameObject.FindWithTag("Rp" + i.ToString()).transform.rotation.y.ToString() + ";";
                    info += GameObject.FindWithTag("Rp" + i.ToString()).transform.rotation.z.ToString() + ";";
                    info += GameObject.FindWithTag("Rp" + i.ToString()).transform.rotation.w.ToString() + ";";
                }
            }
        }
        //errore
        else
        {
            info = "???";
            info += id + ";";
            info += "invalid_hand;";
            return info;
        }

        return info;
    }
}
