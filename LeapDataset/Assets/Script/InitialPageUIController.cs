using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

public class InitialPageUIController : MonoBehaviour
{    
    public int initialSecondsTimer;
    public GameObject initialPagePanel;
    public TextMeshProUGUI initialTimerTextMesh;
    public GameObject acquisitionPagePanel;

    private int secondsLeft;
    private Stopwatch stopWatch;
    private GameObject recorderGameObject;
    private Recorder recorder;
    private AcquisitionPageUIController acquisitionPageController;

    // Start is called before the first frame update
    public void Start()
    {
        // Set initial countdown
        stopWatch = new Stopwatch();
        stopWatch.Start();
        secondsLeft = initialSecondsTimer;
        SetTimerText("Remaining seconds before starting acquisition: ", secondsLeft, initialTimerTextMesh);

        // Get recorder object
        recorderGameObject = GameObject.Find("Recorder");
        recorder = recorderGameObject.GetComponent<Recorder>();

        // Get mainAcquisitionPage object
        acquisitionPageController = acquisitionPagePanel.GetComponent<AcquisitionPageUIController>();
    }

    // Update is called once per frame
    public void Update()
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
            // Activate acquisition panel and enable acquisitionPanel script
            acquisitionPagePanel.SetActive(true);
            acquisitionPageController.enabled = true;
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
