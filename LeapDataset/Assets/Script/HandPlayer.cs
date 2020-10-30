using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System;
using System.Diagnostics;
using System.Threading;
using System.Globalization;

public class HandPlayer: MonoBehaviour
{
    //public GameObject left;
    //public GameObject right;
    private string[] lines;
    private FingerModel[] fingers;
    private Transform palm;
    private int index;
    private bool player_on;
    private Stopwatch stopWatch;
    private Texture2D texture, textureSEg;


    [Header("Hand Player")]
    [Tooltip("file contenente le coordinate")]
    public TextAsset file;
    public GameObject handRight;
    [Tooltip("in millisecondi")]
    public int frameInterval = 400;
    [Header("Camera offsets")]
    public GameObject camera;
    public float xOffset = -1.5f;
    public float yOffset = 0;
    public float zOffset = 0; //-0.5f;
    private GUIStyle style = new GUIStyle();
    private String text = "";
    private int frame = 0;



    void Start()
    {
        //right = GameObject.Find("LoPoly Rigged Hand Right");
        //left = GameObject.Find("LoPoly Rigged Hand Left");
        //frame_offset = LP.CurrentFrame.Id;

        // Recupero componenti della mano
        RiggedHand rHand = handRight.GetComponent(typeof(RiggedHand)) as RiggedHand;
        fingers = rHand.fingers; // 0: Thumb; 1:Index; 2:Middle; 3:Ring; 4:Pinky
        palm = rHand.palm;
        stopWatch = new Stopwatch();
        player_on = false;
        texture = new Texture2D(1, 1);
        textureSEg = new Texture2D(1, 1);
        textureSEg.SetPixel(0, 0, Color.red);
        textureSEg.Apply();



    }
    void OnGUI()
    {
        GUI.Box(new Rect(700, 150, 100, 100), text, style);
        ////GUI.Box(new Rect(600, 150, 100, 100), index.ToString() , style);
        //GUI.DrawTexture(new Rect(500, 100, 200, 100), textureSEg, ScaleMode.ScaleToFit, true, 10.0F);
        if (player_on)
        {
            texture.SetPixel(0, 0, Color.green);
            texture.Apply();
            GUI.DrawTexture(new Rect(10, 10, 20, 10), texture, ScaleMode.ScaleToFit, true, 10.0F);
        }
        else
        {
            texture.SetPixel(0, 0, Color.red);
            texture.Apply();
            GUI.DrawTexture(new Rect(10, 10, 20, 10), texture, ScaleMode.ScaleToFit, true, 10.0F);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            index = -1;
            PlayRecords();
        }
        if (player_on)
        {
            if (Input.GetKeyDown("n"))
            {
                
                if (stopWatch.IsRunning)
                 stopWatch.Stop();
                else
                    stopWatch.Start();
            }
            if (index >= lines.Length)
            {
                // end player
                player_on = false;
                text = "";
                stopWatch.Reset();
                UnityEngine.Debug.Log("Stop player");
                return;
            }
            // update the joints position
            if(stopWatch.ElapsedMilliseconds > frameInterval)
            {
                UpdateJoints(index++);
                stopWatch.Restart();
            }
        }
    }
    /// <summary>
    /// Reproduce the gesture with the datas in the file
    /// </summary>
    public void PlayRecords()
    {
        // carica testo dal file 
        lines = file.text.Split(new string[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);

        // Start reading and move hand model
        if (!player_on)
        {
            player_on = true;
            index = 0;
            stopWatch.Restart();
            UnityEngine.Debug.Log("Start player");
        }
        else
        {
            index = lines.Length + 2;
        }
    }

    /// <summary>
    /// Change transform's values according to the new line to parse.
    /// </summary>
    /// <param name="index"></param>
    public void UpdateJoints(int index)
    {
        UnityEngine.Debug.Log("Update");

        string[] valString = lines[index].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        if(valString[1].Contains("##"))
        {
            text = "";
            //textureSEg.SetPixel(0, 0, Color.red);
            //textureSEg.Apply();
            Thread.Sleep(700);
            return;
        }
        else if (valString[0].Contains("##"))
        {
            text = valString[1];

            Thread.Sleep(700);
            //textureSEg.SetPixel(0, 0, Color.green);
            //textureSEg.Apply();
            return;
        }

        float[] values = new float[valString.Length];
        for(int ind = 0;  ind< valString.Length; ind++)
        {
            values[ind] = float.Parse(valString[ind], CultureInfo.InvariantCulture.NumberFormat);
        }

        // palmpos(x;y;z);palmquat(x,y,z,w) 0->6
        palm.position = new Vector3(values[0], values[1], values[2]);
        if (index < 10)
        {
            camera.transform.position = new Vector3(values[0] + xOffset, values[1] + yOffset, values[2] + zOffset);
            camera.transform.rotation = Quaternion.Euler(40, 80, 0);
        }
        palm.rotation = new Quaternion(values[3], values[4], values[5], values[6]);
        int i = 6;

        // thumbApos(x;y;z)|thumbAquat(x;y;z;w);thumbBpos(x; y; z);thumbBquat(x; y; z; w);thumbEndpos(x;y;z);thumbEndquat(x;y;z;w);
        //    "indexApos(x;y;z)|indexAquat(x;y;z;w)|indexBpos(x;y;z)|indexBquat(x;y;z;w)|indexCpos(x;y;z)|indexCquat(x;y;z;w)|" +
        //    "indexEndpos(x;y;z)|indexEndquat(x;y;z;w)|middleApos(x;y;z)|middleAquat(x;y;z;w)|middleBpos(x;y;z)|middleBquat(x;y;z;w)|" +
        //    "middleCpos(x;y;z)|middleCquat(x;y;z;w)|middleEndpos(x;y;z)|middleEndquat(x;y;z;w)|ringApos(x;y;z)|ringAquat(x;y;z;w)|" +
        //    "ringBpos(x;y;z)|ringBquat(x;y;z;w)|ringCpos(x;y;z)|ringCquat(x;y;z;w)|ringEndpos(x;y;z)|ringEndquat(x;y;z;w)|" +
        //    "pinkyApos(x;y;z)|pinkyAquat(x;y;z;w)|pinkyBpos(x;y;z)|pinkyBquat(x;y;z;w)|pinkyCpos(x;y;z)|pinkyCquat(x;y;z;w)|" +
        //    "pinkyEndpos(x;y;z)|pinkyEndquat(x;y;z;w)
        /// Attenzione: joints delle dita devono essere impostati manualmente e correttamente in modo che rispettino l'ordine 
        foreach (FingerModel finger in fingers)
        {
            foreach(Transform joint in finger.joints)
            {
                joint.position= new Vector3(values[++i], values[++i], values[++i]);
                joint.rotation = new Quaternion(values[++i], values[++i], values[++i], values[++i]);
            }
        }
        
    }

}
