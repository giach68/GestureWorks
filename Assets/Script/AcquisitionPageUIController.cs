﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Video;
using Random = System.Random;
using System.Text.RegularExpressions;

//  UnityEngine.Debug.Log();

public class AcquisitionPageUIController : MonoBehaviour
{
    private const string ConfigFilePath = @".\Assets\Script\gestureConfiguration.yaml";
    private const string VideoFolderPath = @".\Assets\VideoPlayer\";

    public GameObject acquisitionPagePanel;
    public TextMeshProUGUI gestureNameText;
    public TextMeshProUGUI gestureDescription;
    public VideoPlayer videoPlayer;
    public GameObject finalPagePanel;
    public bool changeColorOnNewGesture;
    public string sequenceFilesPath;
    public int secondsToWaitAfterAcquisition;
    public string sequenceBaseFileName = "sequence_";

    // Using centiseconds beacuse milliseconds are not counted accurately
    // 1 second = 100 centiseconds
    private int centisecondsLeft;
    private Stopwatch stopWatch;
    private Recorder recorder;
    private int gestureSequenceIndex = 0;
    private List<Gesture> gestureDatasetList;
    private string[] gestureSequenceStringArray;
    private Image topColorTitle;
    private Color originalTopColorTitle;
    private Color finishingGestureTopColorTitle = new Color(0.8784314f, 0.7411765f, 0.2431373f); // orange color
    private List<string> sequenceFilesNames = new List<string>();
    private int sequenceFileIndex;
    private FinalPageUIController finalPageController;
    private bool acquisitionCompleted = true;

    // Start is called before the first frame update
    void Start()
    {
        // Set gesture dataset list using the configuration file
        YAMLParser yamlParser = new YAMLParser();
        gestureDatasetList = yamlParser.DeserializeGestureDataset(@ConfigFilePath);

        // Get files from path
        ReadAndSortFilesFromFolder(sequenceFilesPath);

        // Read first sequence file 
        ReadSequenceFile(0);

        // Print in Unity Console
        UnityEngine.Debug.Log("Gestures in sequence file: " + string.Join(", ", gestureSequenceStringArray));

        // Get color title image
        topColorTitle = GameObject.Find("TopColorTitle").GetComponent<Image>();
        // Get original color bar title
        originalTopColorTitle = topColorTitle.color;

        // Get mainAcquisitionPage object
        finalPageController = finalPagePanel.GetComponent<FinalPageUIController>();

        // Get recorder object
        recorder = GameObject.Find("Recorder").GetComponent<Recorder>();

        // Set first gesture information
        DisplayGestureInformation(gestureSequenceStringArray[0]);
        stopWatch = new Stopwatch();
        stopWatch.Start();

        // Start recorder
        recorder.enabled = true;
        recorder.Record();
        recorder.StartGesture(gestureSequenceStringArray[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (centisecondsLeft <= 0)
        {
            stopWatch.Stop();

            // If there are still gestures to read from the current sequence
            if (AreThereGesturesToReadInSequence(gestureSequenceIndex) && acquisitionCompleted)
            {
                gestureSequenceIndex++;

                // Update gesture name list index
                DisplayGestureInformation(gestureSequenceStringArray[gestureSequenceIndex]);
                stopWatch.Start();

                // New gesture name to the recorder
                recorder.EndGesture();
                recorder.StartGesture(gestureSequenceStringArray[gestureSequenceIndex]);
            }
            // If there are no more gestures in the current sequence and there is a new sequence to read
            else if (!AreThereGesturesToReadInSequence(gestureSequenceIndex) && AreThereNewSequencesToRead(sequenceFileIndex) && acquisitionCompleted)
            {
                // Stop the recorder
                recorder.EndGesture();
                recorder.Stop();
                recorder.enabled = false;

                // Activate final panel (final panel is above the acquisition panel, this one)
                finalPageController.enabled = true;
                finalPageController.ChangeMessage("Acquisition number " + (sequenceFileIndex + 1) + " completed");
                finalPagePanel.SetActive(true);

                // Reset gesture sequence index
                gestureSequenceIndex = 0;
                acquisitionCompleted = false;

                // Set a sleep of tot sec
                StartCoroutine(WaitAndReset(secondsToWaitAfterAcquisition));
            }
            // If there are no more gestures in the current sequence and there are no more sequences to read
            else if (!AreThereGesturesToReadInSequence(gestureSequenceIndex) && !AreThereNewSequencesToRead(sequenceFileIndex))
            {
                // Stop the recorder
                recorder.EndGesture();
                recorder.Stop();
                recorder.enabled = false;

                // Hide the panel by deactivating it
                acquisitionPagePanel.SetActive(false);

                // Activate final panel
                finalPageController.enabled = true;
                finalPageController.ChangeMessage("Acquisitions completed");
                finalPagePanel.SetActive(true);
            }
        }
        // 1 second = 100 centiseconds
        else if (centisecondsLeft == 100 && changeColorOnNewGesture)
        {
            // If "changeColorOnNewGesture" is true in the inspector, then change bar color to inform 
            // the user that the gesture acquisition is finishing
            topColorTitle.color = finishingGestureTopColorTitle;
        }

        // 10 ms = 1 centisecond
        if (stopWatch.ElapsedMilliseconds >= 10L)
        {
            centisecondsLeft--;
            stopWatch.Restart();
        }
    }

    bool AreThereGesturesToReadInSequence(int index)
    {
        // True if the index is NOT out of the list, so there are gestures left to read
        return index != gestureSequenceStringArray.Length - 1; //(-1 on the count because the index starts from 0)
    }

    bool AreThereNewSequencesToRead(int index)
    {
        // True if the index is NOT out of the list, so there is at least a new sequence to read
        return index != sequenceFilesNames.Count - 1; //(-1 on the count because the index starts from 0)
    }

    void ReadAndSortFilesFromFolder(string sequenceFilePath)
    {
        Regex rgx = new Regex(@"(\w+_)(\d+)(\.txt)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //in alternativa per tutto il percorso ([\w|\\|_|\-|.|\d]*_)(\d+)(\.txt)

        string[] inputSequencesFiles = Directory.GetFiles(sequenceFilePath, "*.txt");

        Console.WriteLine("Original sequence:");
        foreach (string s in inputSequencesFiles)
        {
            UnityEngine.Debug.Log("\t- " +  s);
        }

        // Build the sorted list, the structure orders by the key
        SortedList sequenceFilesSortedList = new SortedList();
        foreach (string s in inputSequencesFiles)
        {
            MatchCollection matches = rgx.Matches(Path.GetFileName(s));
            GroupCollection groups = matches[0].Groups;

            // Put the number as key to have the SortedList putting it in the correct place
            // Put the string (sequence filename) as value to be used later
            sequenceFilesSortedList.Add(Int32.Parse(groups[2].Value), s);
        }

        Console.WriteLine("Correct sequence:");
        foreach (DictionaryEntry e in sequenceFilesSortedList)
        {
            UnityEngine.Debug.Log("\t- " +  e.Value);
        }

        foreach (string bau in sequenceFilesSortedList.GetValueList())
        {
            sequenceFilesNames.Add(bau);
        }

        foreach (string s in sequenceFilesNames)
            UnityEngine.Debug.Log(s);
    }

    void ReadSequenceFile(int index)
    {
        gestureSequenceStringArray = File.ReadAllLines(sequenceFilesNames[index]);        
    }

    bool DisplayGestureInformation(string currentGestureNameInSequence)
    {
        // Search for the gesture where the sequence name is the same as the gesture read in the file
        Gesture currentGesture = gestureDatasetList.Find(gesture => gesture.gestureNameInSequence == currentGestureNameInSequence);

        if (!(currentGesture is null))
        {
            if (currentGesture.gestureNameInSequence.Equals("NONGESTURE"))
            {
                centisecondsLeft = new Random().Next((currentGesture.timerDuration - 2) * 100, (currentGesture.timerDuration +1) * 100);
            }
            else
                centisecondsLeft = currentGesture.timerDuration * 100;

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

    IEnumerator WaitAndReset(int seconds)
    {
        yield return new WaitForSeconds(seconds);

        // Read new sequence file
        sequenceFileIndex++;
        ReadSequenceFile(sequenceFileIndex);

        // Print in Unity Console
        UnityEngine.Debug.Log("Gestures in sequence file: " + string.Join(", ", gestureSequenceStringArray));

        // Set first gesture information
        DisplayGestureInformation(gestureSequenceStringArray[0]);
        stopWatch.Reset();
        stopWatch.Start();

        // Start recorder
        recorder.enabled = true;
        recorder.Record();
        recorder.StartGesture(gestureSequenceStringArray[0]);

        // Hide the panel by deactivating it
        finalPagePanel.SetActive(false);

        acquisitionCompleted = true;
    }
}