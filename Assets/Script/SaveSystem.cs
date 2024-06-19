using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem 
{
    public static SettingData settingData;
    //Save game setting locally
    public static void SaveSetting(SettingData settingData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/setting.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream,settingData);
        stream.Close();


    }
    //Load the saved setting
    public static SettingData LoadSetting()
    {

        string path = Application.persistentDataPath + "/setting.data";
        Debug.Log(path);
        SettingData  settingData = new SettingData();
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SettingData data = formatter.Deserialize(stream) as SettingData;
            stream.Close();
            settingData = data;
            
        }
        else
        {
            SaveSetting(settingData);
        }
        return settingData;
    }
}
