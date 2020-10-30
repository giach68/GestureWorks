using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using System.Diagnostics;

public class recorder : MonoBehaviour
{
    public float timer;
    [Tooltip("frame per second")]
    public float freq=30;
    bool recording;
    public string fileName;
    public string folder;
    private posTracker pt;
    public string[] seqElements= new string[5];
    private int seqIndx;
    private Stopwatch stopWatch;

    // Start is called before the first frame update
    public void Start()
    {
        recording = false;
        GameObject leapRig = GameObject.Find("Leap Rig");
        pt = leapRig.GetComponent<posTracker>();
        seqIndx = 0;
        stopWatch = new Stopwatch();
    }

    // Update is called once per frame
    void Update()
    {
        // press Space for start recording
        if (Input.GetKey(KeyCode.Space) && !recording)
        {
            Record();
        }

        // press Space for stop recording
        if (Input.GetKey(KeyCode.Space) && recording)
        {
            Stop();

        }
        else if(Input.GetKeyDown(KeyCode.N) && recording) // start gesture label at N press
        {
            if(seqIndx < seqElements.Length)
                pt.StartGesture(seqElements[seqIndx++]);
            else
                pt.StartGesture("end");
        }
        else if(Input.GetKeyUp(KeyCode.N) && recording) // end gesture label at N relaese
        {
            pt.EndGesture();
        }

        if(freq > 0 && recording && stopWatch.ElapsedMilliseconds >= (1000.0f/freq))
        {
            UpdateRecording();
            stopWatch.Restart();
        }

    }

    public void Record()
    {
        if (!recording)
            StartCoroutine(Wait());
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
        recording = true;
        UnityEngine.Debug.Log(msg);
        yield return new WaitForSeconds(timer);
        GameObject leapRig = GameObject.Find("Leap Rig");
        posTracker pt = leapRig.GetComponent<posTracker>();
        UnityEngine.Debug.Log("Recording");
        pt.enabled = recording;
        if(freq>0)
            stopWatch.Restart();
    }

    public void UpdateRecording()
    {
        pt.UpdateRecording();
    }

    public void StartRecording(string fileName, string folder)
    {
        recording = true;

        UnityEngine.Debug.Log("Recording");
        seqIndx = 0;
        pt.setFilePath(folder, fileName);
        pt.enabled = recording;
    }

    public void Close()
    {
        Stop();
    }
}
