using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMenuScript : IUIScript
{
    //Interaction home menuu
    [Header("Sound attributes")]
    public GameObject settingPanelObject; // Menu setting

    [Header("Text objects")]
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI sfxText;
    public Slider musicSlider;
    public Slider sfcSlider;
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
    public void OnClick_NavigateToMenu()
    {

    }
    private void LoadSetting()
    {
        soundManager.soundAudios[0].volume = SaveSystem.settingData.musicVolume;
        soundManager.soundAudios[1].volume = SaveSystem.settingData.sfxVolume;
        if (settingPanelObject != null || settingPanelObject.activeSelf == true)
        {
            musicSlider.value = SaveSystem.settingData.musicVolume;
            sfcSlider.value = SaveSystem.settingData.sfxVolume;
            musicText.text = SaveSystem.settingData.musicVolume.ToString() + "%";
            sfxText.text = SaveSystem.settingData.sfxVolume.ToString() + "%";
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
        SaveSystem.settingData = SaveSystem.LoadSetting();
    }
}
