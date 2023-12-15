using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class RecordCSVWriter
{
    static string filePath;
    public static void CSV_SetFileName(string fileName)
    {
        if(fileName == null)
        {
            fileName = "test";
        }
        string fileFullName = fileName + ".csv";
        filePath = Path.Combine(Application.persistentDataPath, fileFullName);
    }

    public static void CSV_Write(string buttonName, string extraData = null)
    {
        TextWriter tw = new StreamWriter(filePath, true);
        string content = buttonName + "," + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "," + extraData;
        tw.WriteLine(content);
        tw.Close();

    }
}
