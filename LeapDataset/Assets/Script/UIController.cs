using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI initialTimerTextMesh;
    public int initialSecondsTimer;
    private int secondsLeft;
    private Stopwatch stopWatch;

    // Start is called before the first frame update
    public void Start()
    {
        // Set initial countdown
        stopWatch = new Stopwatch();
        stopWatch.Start();
        secondsLeft = initialSecondsTimer;
        SetTimerText("Remaining seconds before starting acquisition: ", secondsLeft);
    }

    // Update is called once per frame
    public void Update()
    {
        if (stopWatch.ElapsedMilliseconds >= 1000L)
        {
            secondsLeft--;
            SetTimerText("Remaining seconds before starting acquisition: ", secondsLeft);
            stopWatch.Restart();
        }
        else if(secondsLeft == 0)
        {
            stopWatch.Stop();
        }
    }

    private void SetTimerText(string text, int secondsLeft)
    {
        initialTimerTextMesh.text = text + secondsLeft.ToString() + " seconds";
    }
}
