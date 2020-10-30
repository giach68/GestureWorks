using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//scrive su file, chamato da posTracker
public class FileLogger
{
    private string path;
    private string buffer;
    private StreamWriter sw;

    //simula onEnable(), inizializza logger (tipo costruttore, ma riuso l'oggetto)
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

    //chiude lo stream write
    public void Disable()
    {
        //writeData("");
        //sw.Flush();
        sw.Close();
        UnityEngine.Debug.Log("Text salvato in: " + path);
    }

    public void WriteData(string text)
    {
        buffer += text;
        CloseLine();
        sw.WriteAsync(buffer); //funzione di c#, asincorna altrimenti sarebbe bloccante
        buffer = "";
    }

    //aggiongo al buffer e lo scrivo dopo
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

    /*public void newLine(string text)
    {
        buffer += "\n" + text;
        return;
    }*/

    /*
    public void newLineMarked(string text)
    {
        buffer += "\n%" + text;
        return;
    }*/


}
