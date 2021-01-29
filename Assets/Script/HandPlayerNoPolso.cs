using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using System.Xml;

public class HandPlayer: MonoBehaviour
{
    //public GameObject left;
    //public GameObject right;
    private string[] lines;
    private FingerModel[] fingers;
    private Transform palm;
    private Transform wrist;
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
    private String label = ""; //contiene l'etichetta del gesto in corso
    private int frame = 0;
    private int frameCounter = 0;
    private bool backMode = false; //decide se devo andare all'indietro

    void Start()
    {
        //right = GameObject.Find("LoPoly Rigged Hand Right");
        //left = GameObject.Find("LoPoly Rigged Hand Left");
        //frame_offset = LP.CurrentFrame.Id;

        // Recupero componenti della mano
        RiggedHand rHand = handRight.GetComponent(typeof(RiggedHand)) as RiggedHand; //cast + rinomina
        fingers = rHand.fingers; // 0: Thumb; 1:Index; 2:Middle; 3:Ring; 4:Pinky
        wrist = rHand.wristJoint;
        palm = rHand.palm;

        stopWatch = new Stopwatch();
        player_on = false;
        texture = new Texture2D(1, 1);
        textureSEg = new Texture2D(1, 1);
        textureSEg.SetPixel(0, 0, Color.red);
        textureSEg.Apply();

        style.fontSize = 15;

    }

    //funzione di unity per aggiungere elementi grafici sulla scena
    //chiamato per ogni frame di unity come update
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
            //index: riga file che leggo
            index = -1;
            frameCounter = -1;
            PlayRecords();
        }
        if (player_on)
        {
            if (Input.GetKeyDown("n")) //pausa con n
            {                
                if (stopWatch.IsRunning)
                 stopWatch.Stop();
                else
                    stopWatch.Start();
            }
            if (index >= lines.Length || index < 0) //sono alla fine del file
            {
                // end player
                backMode = false;
                player_on = false;
                text = "";
                stopWatch.Reset();
                UnityEngine.Debug.Log("Stop player"+" "+index.ToString());
                return;
            }
            if (Input.GetKeyDown("1"))
            {
                frameInterval = 1000;
            }
            if (Input.GetKeyDown("5"))
            {
                frameInterval = 200;
            }
            if (Input.GetKeyDown("0"))
            {
                frameInterval = 70;
            }
            if (Input.GetKeyDown("b"))
            {
                if (backMode)
                    backMode = false;
                else
                    backMode = true;
            }
            // update the joints position
            if (stopWatch.ElapsedMilliseconds > frameInterval) //frameinterval: ogni quanti ms aggiorno la visualizzazione mano
            {//passo alla prossima riga 'scaduto' il tempo
                if (backMode)
                    UpdateJoints(index--);
                else
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
        // carica testo dal file per riga
        // fa uno split per ogni riga, mette ogni riga in un array lines di stringhe
        lines = file.text.Split(new string[] {"\n", "\r"}, StringSplitOptions.RemoveEmptyEntries);

        // Start reading and move hand model
        if (!player_on)
        {
            player_on = true;
            index = 0;
            frameCounter = 0;
            stopWatch.Restart();
            UnityEngine.Debug.Log("Start player");
        }
        else //se il player è già acceso, vado qua. fermo la registrazione
        {
            //+2 per superare limite file EOF
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
       
        //split separata con ;, ogni cella è una valore splittato
        string[] valString = lines[index].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
       
        //verifica se ha ## (finito un gesto)
        if (valString[1].Contains("##"))
        {
            label = ""; //etichetta
            //textureSEg.SetPixel(0, 0, Color.red);
            //textureSEg.Apply();
            //Thread.Sleep(700); cambio etichetta 
            return;
        }
        else if (valString[0].Contains("##"))
        {
            label = valString[1];

            //Thread.Sleep(700); cambio etichetta
            //textureSEg.SetPixel(0, 0, Color.green);
            //textureSEg.Apply();
            return;
        }
       
        text = (label + "\n" + frameCounter.ToString());

        if (backMode)
            frameCounter--;
        else
            frameCounter++;

        //conversione tutta in un'istruzione forse da fare
        float[] values = new float[valString.Length];
        for(int ind = 0;  ind< valString.Length; ind++)
        {
            values[ind] = float.Parse(valString[ind], CultureInfo.InvariantCulture.NumberFormat);
        }

        //--- PALM
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
            foreach (Transform joint in finger.joints)
            {
                joint.position = new Vector3(values[++i], values[++i], values[++i]);
                joint.rotation = new Quaternion(values[++i], values[++i], values[++i], values[++i]);
            }
        }

    }

}