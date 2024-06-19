using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static SettingData settingData;
    //Save game setting locally
    public static void SaveSetting(SettingData settingData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/setting.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, settingData);
        stream.Close();
        LoadSetting();
    }
    //Load the saved setting, if save wasn't exist, create and override new default values
    public static void LoadSetting()
    {

        string path = Application.persistentDataPath + "/setting.data";
        Debug.Log(path);
        SettingData tempSettingData = new SettingData();
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

    }
    //Load Sound setting
    public static void LoadSoundSetting(SoundManager soundManager)
    {
        if(settingData==null)
        {
            LoadSetting();
        }
        soundManager.soundAudios[0].volume = SaveSystem.settingData.musicVolume;
        soundManager.soundAudios[1].volume = SaveSystem.settingData.sfxVolume;
    }

}
[System.Serializable]
public class SettingData
{
    public int musicVolume=100;
    public int sfxVolume = 100;
    public bool isMuted= false;


}
