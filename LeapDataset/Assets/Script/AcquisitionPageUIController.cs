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

    private int secondsLeft;
    private Stopwatch stopWatch;

    // Start is called before the first frame update
    void Start()
    {
        stopWatch = new Stopwatch();
        stopWatch.Start();
        secondsLeft = acquisitionSecondsTimer;
        SetTimerText("Starting gesture acquisition in:  ", secondsLeft, gestureAcquisitionTimer);
    }

    // Update is called once per frame
    void Update()
    {
        /*// If the stopwatch is not started
        if (!stopWatch.IsRunning)
        {
            stopWatch.Start();
            secondsLeft = acquisitionSecondsTimer;
        }*/

        secondsLeft = UpdateTextOneSecondElapsed("Starting gesture acquisition in: ", secondsLeft, gestureAcquisitionTimer);

        if (secondsLeft == 0)
        {
            stopWatch.Stop();
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
