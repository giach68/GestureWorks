using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

public class UIController : MonoBehaviour
{
    
    public int initialSecondsTimer;
    public int acquisitionSecondsTimer;
    public GameObject initialPagePanel;
    public GameObject explainationPanel;
    public TextMeshProUGUI initialTimerTextMesh;
    public TextMeshProUGUI gestureNameText;
    public TextMeshProUGUI gestureAcquisitionTimer;

    private int secondsLeft;
    private Stopwatch stopWatch;
    private GameObject recorderGameObject;
    private Recorder recorder;
    private bool isStopwatchStarted;

    // Start is called before the first frame update
    public void Start()
    {
        // ------ INITIAL PAGE
        // Set initial countdown
        stopWatch = new Stopwatch();
        stopWatch.Start();
        secondsLeft = initialSecondsTimer;
        SetTimerText("Remaining seconds before starting acquisition: ", secondsLeft, initialTimerTextMesh);

        // Get recorder object
        recorderGameObject = GameObject.Find("Recorder");
        recorder = recorderGameObject.GetComponent<Recorder>();
    }

    // Update is called once per frame
    public void Update()
    {
        // ------ MANAGE INITIAL PAGE
        FirstInitialPage();

        // ------ MANAGE MAIN ACQUISITION
        MainPageAcquisition();
    }

    private void FirstInitialPage()
    {
        // If the main panel is not currently displayed
        if (!explainationPanel.activeSelf)
        {
            // Check and update initial countdown every second
            secondsLeft = UpdateTextOneSecondElapsed("Remaining seconds before starting acquisition: ", secondsLeft, initialTimerTextMesh);

            // When the timer is elapsed, stop it and change page to display
            if (secondsLeft == 0)
            {
                stopWatch.Stop();
                // Hide the panel by inactivating it
                initialPagePanel.SetActive(false);
                // Enable recorder
                recorder.enabled = true;
                // Enable main panel
                explainationPanel.SetActive(true);
            }
        }
    }

    private void MainPageAcquisition()
    {
        // If the main panel is currently displayed
        if (explainationPanel.activeSelf)
        { 
            // If the stopwatch is not started
            if (!stopWatch.IsRunning)
            {                
                stopWatch.Start();
                secondsLeft = acquisitionSecondsTimer;
            }

            secondsLeft = UpdateTextOneSecondElapsed("Starting gesture culo acquisition in: ", secondsLeft, gestureAcquisitionTimer);

            if (secondsLeft == 0)
            {
                stopWatch.Stop();
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
