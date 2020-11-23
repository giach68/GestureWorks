using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System;

//  UnityEngine.Debug.Log();

public class AcquisitionPageUIController : MonoBehaviour
{
    public const string ConfigFilePath = @".\Assets\Script\gestureConfiguration.yaml";

    public GameObject acquisitionPagePanel;
    public TextMeshProUGUI gestureNameText;
    public TextAsset sequenceFile;
    public TextMeshProUGUI gestureDescription;
    //public List<AcquisitionDisplayInfo> gestureNamesList;// = new List<AcquisitionDisplayInfo>();

    private int secondsLeft;
    private Stopwatch stopWatch;
    private GameObject recorderGameObject;
    private Recorder recorder;
    private int gestureSequenceIndex = 0;
    private List<Gesture> gestureDatasetList;
    private string[] gestureSequenceStringArray;

    // Start is called before the first frame update
    void Start()
    {
        //Set gesture dataset list using the configuration file
        YAMLParser yamlParser = new YAMLParser();
        gestureDatasetList = yamlParser.DeserializeGestureDataset(@ConfigFilePath);

        //Set gesture sequence array using a sequence file
        gestureSequenceStringArray = sequenceFile.text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        //Set first gesture information
        DisplayGestureInformation(gestureSequenceStringArray[0]);
        stopWatch = new Stopwatch();
        stopWatch.Start();

        // Get recorder object
        recorderGameObject = GameObject.Find("Recorder");
        recorder = recorderGameObject.GetComponent<Recorder>();      
    }

    // Update is called once per frame
    void Update()
    {
        /*// If the stopwatch is not running
        if (!stopWatch.IsRunning)
        {
            stopWatch.Start();
            secondsLeft = acquisitionSecondsTimer;
        }*/

        if (secondsLeft == 0)
        {
            stopWatch.Stop();
            //recorder.Start();

            // If the index is not already out of the list (-1 on the count because the index starts from 0)
            if (gestureSequenceIndex != gestureSequenceStringArray.Length - 1)
            {
                // Update gesture name list index
                DisplayGestureInformation(gestureSequenceStringArray[++gestureSequenceIndex]);
                stopWatch.Start();
            }
        }

        if (stopWatch.ElapsedMilliseconds >= 1000L)
        {
            secondsLeft--;
            stopWatch.Restart();
        }

        //UnityEngine.Debug.Log(secondsLeft);
    }

    /// <summary>
    /// Update a TextMesh named "textMeshToUpdate" text timer writing the message contained in 'text' and remaining seconds cointained in 'seconds'
    /// </summary>
    /// <param name="text">Message to display</param>
    /// <param name="secondsLeft">Remaining seconds</param>
    /// <param name="textMeshToUpdate">Text Mesh to update</param>
    private void SetTimerText(string text, int secondsLeft, TextMeshProUGUI textMeshToUpdate)
    {
        textMeshToUpdate.text = text + secondsLeft.ToString() + " seconds";
    }

    //TODO: manage errors
    bool DisplayGestureInformation(string currentGestureNameInSequence)
    {
        // Search for the gesture where the sequence name is the same as the gesture read in the file
        Gesture currentGesture = gestureDatasetList.Find(gesture => gesture.gestureNameInSequence == currentGestureNameInSequence);

        if (!(currentGesture is null))
        {
            secondsLeft = currentGesture.timerDuration;
            gestureNameText.text = "Gesture " + currentGesture.gestureDisplayName;
            gestureDescription.text = currentGesture.gestureDescription;

            return true; //found
        }

        return false; //not found
    }

}