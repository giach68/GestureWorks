using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class logger
{
    private string path;
    private string buffer;
    private StreamWriter sw;

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

    public void Disable()
    {
        //writeData("");
        //sw.Flush();
        sw.Close();
        UnityEngine.Debug.Log("Text salvato in: " + path);
    }

    public void writeData(string text)
    {
        buffer += text;
        sw.WriteAsync( buffer);
        buffer = "";
    }

    public void newLine(string text)
    {
        buffer += "\n" + text;
        return;
    }
    public void addToLine(string text)
    {
        buffer += text;
        return;
    }
    public void newLineMarked(string text)
    {
        buffer += "\n%" + text;
        return;
    }
    public void closeLine()
    {
        buffer += "\n";
        return;
    }

}
