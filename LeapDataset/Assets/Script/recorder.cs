using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using System.Diagnostics;

//è la prima classe chiamata

public class Recorder : MonoBehaviour
{
    public float timer;
    [Tooltip("frame per second")]
    public float freq=30;
    bool recording;
    public string fileName;
    public string folder;
    private PosTracker pt;
    public string[] seqElements= new string[5];
    private int seqIndx;
    private Stopwatch stopWatch;

    // Start is called before the first frame update
    public void Start()
    {
        recording = false;
        GameObject leapRig = GameObject.Find("Leap Rig"); //prendo l'oggetto nella scena che si chiama Leap Rig
        pt = leapRig.GetComponent<PosTracker>(); //prendo il componente di tipo posTracker e lo metto in pt
        seqIndx = 0;
        stopWatch = new Stopwatch();
    }

    // Update is called once per frame
    void Update()
    {
        // press Space for start recording
        if (Input.GetKeyDown(KeyCode.Space) && !recording)
        {
            Record();
        }

        // press Space for stop recording
        if (Input.GetKeyDown(KeyCode.Space) && recording)
        {
            Stop();
        }

        //mentre sto registrando, delimito l'inizio di un gesto
        else if(Input.GetKeyDown(KeyCode.N) && recording) // start gesture label at N press
        {
            //check che non si esca dall'array (ovvero quanti gesti sto registrando)
            if(seqIndx < seqElements.Length)
                pt.StartGesture(seqElements[seqIndx++]);
            else
                pt.StartGesture("end");
        }
        //al rilascio del bottone, scrivo end e finisco il gesto
        else if(Input.GetKeyUp(KeyCode.N) && recording) // end gesture label at N relaese
        {
            pt.EndGesture();
        }

        if(freq > 0 && recording && stopWatch.ElapsedMilliseconds >= (1000.0f/freq)) //ogni 33ms (se freq = 30) registro nuova pos della mano
        {
            UpdateRecording();
            stopWatch.Restart(); //riparte conteggio cronometro
        }

    }

    public void Record()
    {
        if (!recording)
            StartCoroutine(Wait()); //inizia un processo (wait) in parallelo (asincrono)
        else
            Stop();
    }

    public void Stop()
    {
        string msg = "Stop Recording";
        recording = false;
        UnityEngine.Debug.Log(msg);
        pt.enabled = recording;
        if(freq > 0)
            stopWatch.Stop();
    }

    IEnumerator Wait()
    {
        string msg = "The recording will start in " + timer + " seconds";
        
        UnityEngine.Debug.Log(msg);
        yield return new WaitForSeconds(timer); //tipo crea figlio e aspetta che finisca

        pt.SetFilePath(folder, fileName);
        recording = true;
        UnityEngine.Debug.Log("Recording");

        pt.enabled = recording; //visto che recording è true, abilito il componente pt (posTracker), invocando il metodo onEnable()
        if(freq>0) //può essere <0 se l'utente non vuole registrare e mette 0 su unity
            stopWatch.Restart();
    }

    public void UpdateRecording()
    {
        pt.UpdateRecording();
    }

    /*
    public void StartRecording(string fileName, string folder)
    {
        recording = true;

        UnityEngine.Debug.Log("Recording");
        seqIndx = 0;
        pt.setFilePath(folder, fileName);
        pt.enabled = recording;
    }   
    */

    //di unity, chiude tutto 
    public void Close()
    {
        Stop();
    }
}
