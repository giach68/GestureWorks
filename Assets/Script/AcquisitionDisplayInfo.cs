using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System.Runtime.Remoting.Metadata.W3cXsd2001;


[System.Serializable]
public class AcquisitionDisplayInfo
{
    public string gestureName;
    public string gestureVideoPath;

    public string GestureName
    {
        get { return gestureName; }
        set { gestureName = value; }
    }

    public string GestureVideoPath
    {
        get { return gestureVideoPath; }
        set { gestureVideoPath = value; }
    }

    public AcquisitionDisplayInfo(string gestureName, string gestureVideoPath)
    {
        this.gestureName = gestureName;
        this.gestureVideoPath = gestureVideoPath;
    }
}

