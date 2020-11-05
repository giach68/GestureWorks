using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI initialTimerTextMesh;
    public int initialSecondsTimer;
    public GameObject initialPagePanel;

    private int secondsLeft;
    private Stopwatch stopWatch;
    private GameObject recorderGameObject;
    private Recorder recorder;

    // Start is called before the first frame update
    public void Start()
    {
        // ------ INITIAL PAGE
        // Set initial countdown
        stopWatch = new Stopwatch();
        stopWatch.Start();
        secondsLeft = initialSecondsTimer;
        SetTimerText("Remaining seconds before starting acquisition: ", secondsLeft);

        // Get recorder object
        recorderGameObject = GameObject.Find("Recorder");
        recorder = recorderGameObject.GetComponent<Recorder>();
    }

    // Update is called once per frame
    public void Update()
    {
        // ------ MANAGE INITIAL PAGE
        FirstInitialPage();
    }

    private void FirstInitialPage()
    {
        // Check and update initial countdown every second
        if (stopWatch.ElapsedMilliseconds >= 1000L)
        {
            secondsLeft--;
            SetTimerText("Remaining seconds before starting acquisition: ", secondsLeft);
            stopWatch.Restart();
        }

        // When the timer is elapsed, stop it and change page to display
        if (secondsLeft == 0)
        {
            stopWatch.Stop();
            //Hide the panel by inactivating it
            initialPagePanel.SetActive(false);
            //Enable recorder
            recorder.enabled = true;
            
        }
    }

    /// <summary>
    /// Update a TextMesh text timer writing the message contained in 'text' and remaining seconds cointained in 'seconds'
    /// </summary>
    /// <param name="text">Message to display</param>
    /// <param name="secondsLeft">Remaining seconds</param>
    private void SetTimerText(string text, int secondsLeft)
    {
        initialTimerTextMesh.text = text + secondsLeft.ToString() + " seconds";
    }
}
