using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static SettingData settingData;
    private static List<ScoreboardData> scoreboardDatas;
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
    public static void SaveScoreBoard(ScoreboardData newScoreBoard)
    {
        string path = Application.persistentDataPath + "/scoreboard.data";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream;
        scoreboardDatas= new List<ScoreboardData>();
        if (File.Exists(path))
        {

            stream = new FileStream(path, FileMode.Open);
            scoreboardDatas = formatter.Deserialize(stream) as List<ScoreboardData>;
            stream.Close();
        }
        scoreboardDatas.Add(newScoreBoard);
        formatter = new BinaryFormatter();
        stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, scoreboardDatas);
        stream.Close();
    }
    public  static List<ScoreboardData> LoadScoreBoard()
    {
        string path = Application.persistentDataPath + "/scoreboard.data";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream;
        scoreboardDatas= new List<ScoreboardData>();
        if (File.Exists(path))
        {

            stream = new FileStream(path, FileMode.Open);
            scoreboardDatas = formatter.Deserialize(stream) as List<ScoreboardData>;
            stream.Close();
        }
        return scoreboardDatas;
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
            settingData = new SettingData();
            SaveSetting(settingData);
        }

    }
    //Load Sound setting
    public static void LoadSoundSetting(SoundManager soundManager)
    {
        if (settingData == null)
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
    public int musicVolume = 100;
    public int sfxVolume = 100;
    public bool isMuted = false;


}
[System.Serializable]
public class ScoreboardData
{
    //Scoreboard info
    public DateTime dateTime = DateTime.Now;
    public List<string> playerNameList = new List<string>();
    public List<int> playerPlace = new List<int>();
    public List<int> playerTurns = new List<int>();
    public List<int> playerBuffer = new List<int>();
    public List<int> playerFailed = new List<int>();
    public ScoreboardData()
    {

    }
    public void AddPlayer(string name, int turn, int buffer, int failed)
    {
        playerNameList.Add(name);
        playerPlace.Add(playerNameList.Count);
        playerTurns.Add(turn);
        playerBuffer.Add(buffer);
        playerFailed.Add(failed);
    }

}