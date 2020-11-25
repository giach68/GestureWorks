using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System;
using UnityEngine.UI;
using UnityEngine.Video;

//  UnityEngine.Debug.Log();

public class AcquisitionPageUIController : MonoBehaviour
{
    private const string ConfigFilePath = @".\Assets\Script\gestureConfiguration.yaml";
    private const string VideoFolderPath = @".\Assets\VideoPlayer\";

    public GameObject acquisitionPagePanel;
    public TextMeshProUGUI gestureNameText;
    public TextAsset sequenceFile;
    public TextMeshProUGUI gestureDescription;
    public VideoPlayer videoPlayer;
    public GameObject finalPagePanel;
    public bool changeColorOnNewGesture;
    //public List<AcquisitionDisplayInfo> gestureNamesList;// = new List<AcquisitionDisplayInfo>();

    private int secondsLeft;
    private Stopwatch stopWatch;
    private Recorder recorder;
    private int gestureSequenceIndex = 0;
    private List<Gesture> gestureDatasetList;
    private string[] gestureSequenceStringArray;
    private Image topColorTitle;
    private Color originalTopColorTitle;
    private Color finishingGestureTopColorTitle = new Color(0.8784314f, 0.7411765f, 0.2431373f); // orange color

    // Start is called before the first frame update
    void Start()
    {
        // Set gesture dataset list using the configuration file
        YAMLParser yamlParser = new YAMLParser();
        gestureDatasetList = yamlParser.DeserializeGestureDataset(@ConfigFilePath);

        // Set gesture sequence array using a sequence file
        gestureSequenceStringArray = sequenceFile.text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        // Print in Unity Console
        UnityEngine.Debug.Log("Gestures in sequence file: " + string.Join(", ", gestureSequenceStringArray));
        

        // Get color title image
        topColorTitle = GameObject.Find("TopColorTitle").GetComponent<Image>();
        // Get original color bar title
        originalTopColorTitle = topColorTitle.color;

        // Get recorder object
        recorder = GameObject.Find("Recorder").GetComponent<Recorder>();

        // Start recorder
        recorder.enabled = true;
        recorder.StartGesture(gestureSequenceStringArray[0]);

        // Set first gesture information
        DisplayGestureInformation(gestureSequenceStringArray[0]);
        stopWatch = new Stopwatch();
        stopWatch.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (secondsLeft == 0)
        {
            stopWatch.Stop();

            // If the index is not already out of the list (-1 on the count because the index starts from 0)
            if (gestureSequenceIndex != gestureSequenceStringArray.Length - 1)
            {
                gestureSequenceIndex++;

                // Update gesture name list index
                DisplayGestureInformation(gestureSequenceStringArray[gestureSequenceIndex]);
                stopWatch.Start();

                // New gesture name to the recorder
                recorder.EndGesture();
                recorder.StartGesture(gestureSequenceStringArray[gestureSequenceIndex]);
            }
            else
            {
                //Stop the recorder
                recorder.EndGesture();
                recorder.Stop();

                // Hide the panel by inactivating it
                acquisitionPagePanel.SetActive(false);

                // Activate acquisition panel and enable acquisitionPanel script
                finalPagePanel.SetActive(true);
            }
        }
        else if (secondsLeft == 1 && changeColorOnNewGesture)
        {
            // If "changeColorOnNewGesture" is true in the inspector, then change bar color to inform 
            // the user that the gesture acquisition is finishing
            topColorTitle.color = finishingGestureTopColorTitle;
        }

        if (stopWatch.ElapsedMilliseconds >= 1000L)
        {
            secondsLeft--;
            stopWatch.Restart();
        }
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

            if (currentGesture.videoFileName.Equals(""))
                videoPlayer.url = VideoFolderPath + "blank video image.mp4";
            else
                videoPlayer.url = VideoFolderPath + currentGesture.videoFileName;

            // Change bar color to the original one
            if (changeColorOnNewGesture)
                topColorTitle.color = originalTopColorTitle;

            return true; //found
        }

        return false; //not found
    }
}