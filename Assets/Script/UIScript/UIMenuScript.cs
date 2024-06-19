using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMenuScript : IUIScript
{
    [SerializeField] Button buttonStartClient;
    [SerializeField] Button buttonStartHost;
    //Interaction home menuu
    [Header("Sound attributes")]
    [SerializeField] GameObject settingPanelObject; // Menu setting
    [SerializeField] GameObject hostPanelObject;// Menu host
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

    public void OnClick_NavigateToMenu()
    {

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
