using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Called by posTracker
public class FileLogger
{
    private string path;
    private string buffer;
    private StreamWriter sw;

    // Similar to onEnable(), initialize logger (like a constructor)
    public void Enable(string folder, string fileName)
    {
        Directory.CreateDirectory(string.Format("Assets/LeapLogs/{0}", folder));
        path = string.Format("Assets/LeapLogs/{0}/{1}_{2}.txt", folder, fileName, System.DateTime.Now.ToString("MM_dd_h_mmss"));
        //if (!File.Exists(path)) {
        //    //sw.WriteAsync("% Hand Tracking log file " + System.DateTime.Now.ToString() + "\n");
        //}
        sw = File.CreateText(path);
        buffer = "";
        //else
        //{
        //    sw = File.AppendText(path);
        //    File.WriteAsync("% Hand Tracking log file " + System.DateTime.Now.ToString() + "\n%");

        //}
    }

    // Close write stream
    public void Disable()
    {
        sw.Close();
        UnityEngine.Debug.Log("Acquisition saved in: " + path);
    }

    // Write to file
    public void WriteData(string text)
    {
        buffer += text;
        CloseLine();
        sw.Write(buffer);
        buffer = "";
    }

    // Add to buffer and write later
    public void AddToLine(string text)
    {
        buffer += text;
        CloseLine();
        return;
    }

    public void CloseLine()
    {
        buffer += "\n";
        return;
    }
}