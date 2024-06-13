using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class UIMapHostScript : IUIScript
{

    [Space]
    [Header("UI Attributes")]
    public Color buffColor;
    public Color failedColor;
    //A keyboard holding button for tiles
    public Transform panelTablet;
    public MapInitializeScript mapInitializeScript;
    public List<GameObject> listButtons = new List<GameObject>();
    //The space between each tile on keyboard

    public float seperateSpace = 1f;

    void Start()
    {

        base.Initialize();
        IniMap();
        BackgroundTransistionOpen();
        mapInitializeScript = FindObjectOfType<MapInitializeScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick_NavigateToMenu()
    {
        BackgroundTransistionClose(0);

    }
    public void IniMap()
    {
        if (listButtons.Count == 1)
        {
            //Place all the buttons
            RectTransform panel = panelTablet.GetComponent<RectTransform>();
            listButtons[0].SetActive(true);
            float vertPosition = -listButtons[0].GetComponent<RectTransform>().sizeDelta.y;
            float horiPosition = listButtons[0].GetComponent<RectTransform>().sizeDelta.x; ;
            for (int i = 1; i < mapInitializeScript.number_of_sector - 1; i++)
            {
                GameObject newButton = Instantiate(listButtons[i - 1], panelTablet);
                newButton.name = (i + 1).ToString();
                TextMeshProUGUI text = newButton.GetComponentInChildren<TextMeshProUGUI>();
                text.text = (i + 1).ToString();
                //Add onclick event 

                RectTransform rectTransform = newButton.GetComponent<RectTransform>();
                horiPosition += rectTransform.sizeDelta.x + seperateSpace;

                if (horiPosition >= (panel.sizeDelta.x - rectTransform.sizeDelta.x))
                {
                    horiPosition = rectTransform.sizeDelta.x;
                    vertPosition -= rectTransform.sizeDelta.y + seperateSpace;
                }

                rectTransform.anchoredPosition = new Vector2(horiPosition, vertPosition);
                listButtons.Add(newButton);
                var button = newButton.GetComponent<Button>();
                button.onClick.AddListener(delegate { OnClick_ButtonTile(); });

            }
        }

    }
    public void OnClick_ButtonOk()
    {
<<<<<<< HEAD
        
=======

>>>>>>> 635c24750cba69e253067105a180fe52f510d8c3

    }

    void OnClick_ButtonTile()
    {
        GameObject button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        int index = Int32.Parse(button.name) - 1;
        var platformscript = SectorTileManager.listPlatform[index].GetComponent<PlatformSectorScript>();
        int indexTile = platformscript.NextTileType();
        SectorTileManager.listTileType[index] = indexTile;
        if (platformscript.tileIndex == 0)
        {
            listButtons[index].GetComponent<Image>().color = Color.white;

        }
        else if (platformscript.tileIndex == 1)
        {
            listButtons[index].GetComponent<Image>().color = buffColor;
        }
        else
        {
            listButtons[index].GetComponent<Image>().color = failedColor;
        }
        mapInitializeScript.InitTiles();
        this.OnClick_ButtonClick();
    }
}
