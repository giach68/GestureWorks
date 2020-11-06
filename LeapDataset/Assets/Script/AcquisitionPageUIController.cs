using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;

public class AcquisitionPageUIController : MonoBehaviour
{
    public int acquisitionSecondsTimer;
    public GameObject acquisitionPagePanel;
    public TextMeshProUGUI gestureNameText;
    public TextMeshProUGUI gestureAcquisitionTimer;
    //public List<string> gestureNamesList = new List<string>();
    public List<AcquisitionDisplayInfo> gestureNamesList;// = new List<AcquisitionDisplayInfo>();

    private int secondsLeft;
    private Stopwatch stopWatch;
    private GameObject recorderGameObject;
    private Recorder recorder;
    private int gestureNamesIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        stopWatch = new Stopwatch();
        stopWatch.Start();
        secondsLeft = acquisitionSecondsTimer;
        SetTimerText("Starting gesture acquisition in: ", secondsLeft, gestureAcquisitionTimer);

        // Get recorder object
        recorderGameObject = GameObject.Find("Recorder");
        recorder = recorderGameObject.GetComponent<Recorder>();

        // Set first gesture name
        gestureNameText.text = "Gesture: " + gestureNamesList[0].GestureName;
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

        secondsLeft = UpdateTextOneSecondElapsed("Starting gesture acquisition in: ", secondsLeft, gestureAcquisitionTimer);

        if (secondsLeft == 0)
        {
            stopWatch.Stop();
            secondsLeft = acquisitionSecondsTimer;
            //recorder.Start();

            // If the index is not already out of the list (-1 on the count because the index starts from 0)
            if (gestureNamesIndex != gestureNamesList.Count - 1)
            {
                // Update gesture name list index
                gestureNameText.text = "Gesture: " + gestureNamesList[++gestureNamesIndex].GestureName;

                stopWatch.Start();
            }
        }
    }

    private int UpdateTextOneSecondElapsed(string text, int secondsLeft, TextMeshProUGUI textMeshToUpdate)
    {
        if (stopWatch.ElapsedMilliseconds >= 1000L)
        {
            secondsLeft--;
            SetTimerText(text, secondsLeft, textMeshToUpdate);
            stopWatch.Restart();
        }

        return secondsLeft;
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
}