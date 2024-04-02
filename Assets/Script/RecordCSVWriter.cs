using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class RecordCSVWriter
{
    static string filePath;
    static string usageFilePath;
    public static void CSV_SetFileName(string fileName)
    {
        if(fileName == null)
        {
            fileName = "test";
        }
        string fileFullName = fileName + ".csv";
        filePath = Path.Combine(Application.persistentDataPath, fileFullName);

        string usageFileFullName = fileName + "_usageLog.csv";
        usageFilePath = Path.Combine(Application.persistentDataPath, usageFileFullName);
    }

    public static void CSV_Write(string buttonName, string extraData = null)
    {
        try
        {
             TextWriter tw = new StreamWriter(filePath, true);
            string content = buttonName + "," + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "," + extraData;
            tw.WriteLine(content);
            tw.Close();
        }
        catch
        {}
    }

    public static void CSV_ApplicationUsageLog(string status, string extraData = null)
    {
        try
        {
            TextWriter tw = new StreamWriter(usageFilePath, true);
            string content = status + "," + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "," + extraData;
            tw.WriteLine(content);
            tw.Close();
        }
        catch
        {}
    }
}
