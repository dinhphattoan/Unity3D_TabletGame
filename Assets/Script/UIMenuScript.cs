using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuScript : IUIScript
{
    //The background image for menu
    public Image backgroundImage;
    public RectTransform interactionUI;
    public float backgroundTransparency = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        InitalizeUI();
        this.BackgroundTransistionOpen();
    }
    void InitalizeUI()
    {
    }
    // Update is called once per frame
    void Update()
    {
        //Set the background transprency
        backgroundImage.rectTransform.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);

        backgroundImage.color = new Color(255, 255, 255, backgroundTransparency);
    }

}
