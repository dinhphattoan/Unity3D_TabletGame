using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IUIScript : MonoBehaviour
{
    [SerializeField] Image backgroundTransistionImage;
    [SerializeField] protected SoundManager soundManager;
    public float maxDistancedelta = 0.05f;
    protected bool isTrasistioning = false;
    [Space]
    [Header("Preload Sounds")]
    public AudioClip clipOnClick;
    public AudioClip clipTransistion;
    // Start is called before the first frame update
    protected virtual void Initialize()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
        SaveSystem.settingData = SaveSystem.LoadSetting();
        soundManager.soundAudios[0].volume = SaveSystem.settingData.musicVolume;
        soundManager.soundAudios[1].volume = SaveSystem.settingData.sfxVolume;
        this.BackgroundTransistionOpen();
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Background Transistion Open
    /// </summary>
    /// <param name="nextScene">Load next scene if there's index, -1 will not</param>
    protected void BackgroundTransistionClose(int nextScene)
    {
        StartCoroutine(BackgroundTransistionCloseCoroutine(nextScene));

    }
    /// <summary>
    /// Background Transistion Open
    /// </summary>
    /// <param name="nextScene">Load next scene if there's index, -1 will not</param>
    protected void BackgroundTransistionOpen()
    {
        StartCoroutine(BackgroundTransistionOpenCoroutine());
    }

    private IEnumerator BackgroundTransistionOpenCoroutine()
    {
        this.backgroundTransistionImage.transform.parent.gameObject.SetActive(true);
        if (backgroundTransistionImage != null)
        {
            //resize image to fit the size of the screen
            backgroundTransistionImage.rectTransform.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
            backgroundTransistionImage.rectTransform.position = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
            //Moving from center to left
            Vector3 vectorDestination = new Vector3(-backgroundTransistionImage.rectTransform.position.x, backgroundTransistionImage.rectTransform.position.y, 0);
            while (vectorDestination.x < backgroundTransistionImage.rectTransform.position.x)
            {
                backgroundTransistionImage.rectTransform.position = Vector3.MoveTowards(backgroundTransistionImage.transform.position, vectorDestination,
                maxDistancedelta);
                yield return null;
            }
            this.backgroundTransistionImage.transform.parent.gameObject.SetActive(false);
        }
    }
    private IEnumerator BackgroundTransistionCloseCoroutine(int nextScene)
    {
        this.backgroundTransistionImage.transform.parent.gameObject.SetActive(true);
        if (backgroundTransistionImage != null)
        {
            yield return new WaitForSeconds(0.5f);
            //resize image and reposition to fit the size of the screen
            backgroundTransistionImage.rectTransform.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
            backgroundTransistionImage.rectTransform.position = new Vector2(Camera.main.pixelWidth + Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
            //Moving from right to center
            while (backgroundTransistionImage.rectTransform.position.x > Camera.main.pixelWidth / 2)
            {
                backgroundTransistionImage.transform.position = Vector3.MoveTowards(backgroundTransistionImage.rectTransform.transform.position,
                new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0), maxDistancedelta);
                yield return null;
            }

            if (nextScene >= 0 && nextScene < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextScene);
            }
            else this.backgroundTransistionImage.transform.parent.gameObject.SetActive(false);
        }
    }
    //Event handler
    public void OnClick_ButtonClick()
    {
        soundManager.PlaySFX(clipOnClick);
    }
}
