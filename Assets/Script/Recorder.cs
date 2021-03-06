using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using System.Diagnostics;

public class Recorder : MonoBehaviour
{
    [Tooltip("frame per second")]
    public float freq = 40;
    public string fileName;
    public string folder;

    private PosTracker pt;
    private Stopwatch stopWatch;

    public void Awake()
    {
        GameObject leapRig = GameObject.Find("Leap Rig"); //prendo l'oggetto nella scena che si chiama Leap Rig
        pt = leapRig.GetComponent<PosTracker>(); //prendo il componente di tipo posTracker e lo metto in pt
        stopWatch = new Stopwatch();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTimer();
    }

    public void EndGesture()
    {
        pt.EndGesture();
    }

    public void StartGesture(string gestureName)
    {
        pt.StartGesture(gestureName);
    }

    void CheckTimer()
    {
        if (freq > 0 && stopWatch.ElapsedMilliseconds >= (1000.0f / freq)) //ogni 33ms (se freq = 30) registro nuova pos della mano
        {
            UpdateRecording();
            stopWatch.Restart();
        }
    }

    public void Record()
    {
        pt.SetFilePath(folder, fileName);
        UnityEngine.Debug.Log("Recording");

        // Enable pt component invoking onEnable() method
        pt.enabled = true;

        if (freq > 0) //può essere <0 se l'utente non vuole registrare e mette 0 su unity
            stopWatch.Restart();
    }

    public void Stop()
    {
        string msg = "Stop Recording";
        UnityEngine.Debug.Log(msg);
        pt.enabled = false;

        if (freq > 0)
            stopWatch.Stop();
    }

    public void UpdateRecording()
    {
        pt.UpdateRecording();
    }

    //Unity method
    public void Close()
    {
        Stop();
    }
}