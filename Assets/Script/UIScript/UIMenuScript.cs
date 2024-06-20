using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMenuScript : IUIScript
{
    [Header("UI Menu Attributes")]
    [SerializeField] GameObject settingPanelObject; // Menu setting
    [SerializeField] GameObject hostPanelObject;// Menu host
    [SerializeField] GameObject scoreboardPanelObject;// Menu scoreboard
    [SerializeField] GameObject gameObjectScoreBoardPanelContent;
    [SerializeField] GameObject gameObjectPlayerPanelContent;
    [SerializeField] GameObject gameObjectScoreBoardLabel;

    [Header("Text objects")]
    [SerializeField] TextMeshProUGUI musicText;
    [SerializeField] TextMeshProUGUI sfxText;
    [Header("Text Input Object")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfcSlider;

    void Start()
    {
        base.Initialize();
    }

    void Update()
    {

    }


    public void OnClick_HostGame()
    {
        this.BackgroundTransistionClose(1);
    }
    public void OnClick_ScoreboardOpen()
    {
        settingPanelObject.SetActive(false);
        scoreboardPanelObject.SetActive(true);
        LoadScoreboard();
    }
    public void OnClick_ScoreboardClose()
    {
        scoreboardPanelObject.SetActive(false);

    }
    private void LoadScoreboard()
    {
        //Clear prev
        gameObjectScoreBoardLabel.transform.SetParent(null);
        gameObjectPlayerPanelContent.transform.SetParent(null);
        for (int i = 0; i < gameObjectScoreBoardPanelContent.transform.childCount; i++)
        {
            Destroy(gameObjectScoreBoardPanelContent.transform.GetChild(i).gameObject);
        }

        List<ScoreboardData> scoreboardDatas = SaveSystem.LoadScoreBoard();
        var gameObjectPanelHeader = Instantiate(gameObjectPlayerPanelContent, gameObjectScoreBoardPanelContent.transform);
        gameObjectPanelHeader.SetActive(true);
        for (int i = 0; i < scoreboardDatas.Count; i++)
        {
            var scoreboardLabel = Instantiate(gameObjectScoreBoardLabel, gameObjectScoreBoardPanelContent.transform);
            scoreboardLabel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Date played: " + scoreboardDatas[i].dateTime.ToString();
            scoreboardLabel.GetComponent<Image>().color = new Color(255, 255, 255, 0);
            scoreboardLabel.SetActive(true);
            //Assign info
            for (int j = 0; j < scoreboardDatas[i].playerNameList.Count; j++)
            {
                var playerPanel = Instantiate(gameObjectPlayerPanelContent, gameObjectScoreBoardPanelContent.transform);
                playerPanel.SetActive(true);
                playerPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = scoreboardDatas[i].playerPlace[j].ToString();
                playerPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = scoreboardDatas[i].playerNameList[j];
                playerPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = scoreboardDatas[i].playerTurns[j].ToString();
                playerPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = scoreboardDatas[i].playerBuffer[j].ToString();
                playerPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = scoreboardDatas[i].playerFailed[j].ToString();
                playerPanel.GetComponent<Image>().color = new Color(255, 255, 255, 0);
            }

        }
    }
    private void LoadSetting()
    {
        SaveSystem.LoadSoundSetting(soundManager);
        if (settingPanelObject != null || settingPanelObject.activeSelf == true)
        {
            musicSlider.value = soundManager.soundAudios[0].volume; // Load music sound
            sfcSlider.value = soundManager.soundAudios[1].volume; //Load sfx sound
            musicText.text = musicSlider.value.ToString() + "%";
            sfxText.text = sfcSlider.value.ToString() + "%";
        }

    }

    public void sliderSound_OnValueChange(int soundIndex)
    {
        var target = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();
        if (target != null)
        {
            int volume = Int32.Parse(target.value.ToString());
            soundManager.soundAudios[soundIndex].volume = volume;
            if (soundIndex == 0)
            {
                musicText.text = volume.ToString() + "%";
            }
            else
            {
                sfxText.text = volume.ToString() + "%";
            }
        }

    }
    public void OnClick_SettingClose()
    {
        LoadSetting();
        this.settingPanelObject.SetActive(false);
    }

    public void OnClick_SettingOpen()
    {
        this.settingPanelObject.SetActive(true);
        LoadSetting();
    }

    public void OnClick_SaveSetting()
    {
        //Save setting file
        SettingData settingData = new SettingData()
        {
            musicVolume = soundManager.soundAudios[0].volume,
            sfxVolume = soundManager.soundAudios[1].volume
        };
        SaveSystem.SaveSetting(settingData);
    }
}
