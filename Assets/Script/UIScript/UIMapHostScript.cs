using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using CI.PowerConsole;
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
    public TextMeshProUGUI textMeshNumberPlayer;
    public GameObject gameObjectMapSetting; //Map Setting Panel
    public GameObject gameObjectPlayerSetting; //Player Setting Panel
    public GameObject gameObjectPlayerTab; //Player Tab Panel for editing
    public GameObject gameObjectModelPortfolio; // Model Portfolio showcase
    public GameObject textMeshPlayerNameInput; //Input field for player name
    public List<Button> playerTabButtons = new List<Button>(); // Player tab button
    public List<GameObject> modelFigurePanels = new List<GameObject>(); //List of panel for model figure
    int numberplayer = 1;
    int playerIdEditing = -1;
    public List<Player> players = new List<Player>();

    [Header("Model Figure")]
    [Space]
    [SerializeField] GridLayoutGroup modelContentGrid;
    [SerializeField] GameObject modelFigurePanelPlaceHolder; //A placeholder panel for figure
    [SerializeField] List<GameObject> listOfFigurePanel = new List<GameObject>(); //List of figure panel when initialize
    //List of available model figure for choosing
    public List<GameObject> modelFigures = new List<GameObject>();
    //List of camera that splot the model figure
    public List<GameObject> cameraSplotOn = new List<GameObject>();
    public List<RenderTexture> renderTextures = new List<RenderTexture>();
    public List<RawImage> rawImages = new List<RawImage>();
    public float spotDistance = 5f;
    public float rotateSpeed = 5f;
    //The space between each tile on keyboard

    public float seperateSpace = 1f;

    void Start()
    {
        base.Initialize();
        IniMap();
        BackgroundTransistionOpen();
        mapInitializeScript = FindObjectOfType<MapInitializeScript>();
        mapInitializeScript.InitTiles();
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
    /// <summary>
    /// Button onclick event on hosting the server on local ip address and port
    /// </summary>
    public void OnClick_ButtonNext()
    {
        this.gameObjectMapSetting.SetActive(false);
        this.gameObjectPlayerSetting.SetActive(true);
        this.gameObjectPlayerTab.SetActive(false);
        players.Clear();
        for (int i = 0; i < 4; i++)
        {
            if (i < numberplayer)
            {
                players.Add(new Player());
                playerTabButtons[i].gameObject.SetActive(true);
            }
            else
            {
                playerTabButtons[i].gameObject.SetActive(false);
            }
        }
        foreach (var model in modelFigurePanels)
        {
            model.SetActive(true);
        }

    }
    public void OnClick_ButtonReturnMapSetting()
    {
        this.gameObjectMapSetting.SetActive(true);
        this.gameObjectPlayerSetting.SetActive(false);
        this.players.Clear();
        foreach (var model in modelFigurePanels)
        {
            model.GetComponent<Image>().color = Color.white;
        }
        playerIdEditing = -1;

    }
    public void OnClick_ButtonPlayerUp()
    {
        if (numberplayer < 4)
        {
            numberplayer++;
            textMeshNumberPlayer.text = numberplayer.ToString();
        }
    }
    public void OnClick_PlayerTab(int playerId)
    {
        gameObjectPlayerTab.SetActive(false);
        //Save prev editing
        if (playerIdEditing != -1)
        {
            //Save the name
            players[playerIdEditing].name = textMeshPlayerNameInput.GetComponent<TMP_InputField>().text;

        }
        playerIdEditing = playerId;
        gameObjectPlayerTab.SetActive(true);
        textMeshPlayerNameInput.GetComponent<TMP_InputField>().text = players[playerIdEditing].name;
        //Refresh the portfolio 
        foreach (var modelFigurePanel in modelFigurePanels)
        {
            modelFigurePanel.SetActive(true);
        }
        foreach (var player in players)
        {
            if (player.modelFigureId != -1 && players.IndexOf(player) != playerIdEditing)
            {
                modelFigurePanels[player.modelFigureId].SetActive(false);
            }
        }
        if (players[playerIdEditing].modelFigureId != -1)
        {
            Image image = modelFigurePanels[players[playerIdEditing].modelFigureId].GetComponent<Image>();
            image.color = Color.red;
        }

    }
    public void OnClick_ModelFigure(int figureId)
    {
        //Refresh current appear portfolios 
        for (int child = 0; child < modelFigurePanels.Count; child++)
        {
            Image image = modelFigurePanels[child].transform.GetComponent<Image>();
            image.color = Color.white;
        }
        //Refresh the model portfolio
        if (playerIdEditing != -1)
        {
            //Unselect
            if (players[playerIdEditing].modelFigureId == figureId)
            {
                Image image = gameObjectModelPortfolio.transform.GetChild(figureId).GetComponent<Image>();
                image.color = Color.white;
                players[playerIdEditing].modelFigureId = -1;
            }
            //Select
            else
            {
                players[playerIdEditing].modelFigureId = figureId;
                Image image = gameObjectModelPortfolio.transform.GetChild(figureId).GetComponent<Image>();
                image.color = Color.red;
            }

        }
    }

    public void OnClick_ButtonPlayerDown()
    {
        if (numberplayer > 1)
        {
            numberplayer--;
            textMeshNumberPlayer.text = numberplayer.ToString();
        }
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
    public void OnClick_ButtonPlay()
    {
        //Save the last editing
        if (playerIdEditing != -1)
        {
            //Save the name
            players[playerIdEditing].name = textMeshPlayerNameInput.GetComponent<TMP_InputField>().text;
        }
        mapInitializeScript.SaveMapSetting();
        mapInitializeScript.players= this.players;
        this.BackgroundTransistionClose(2);
    }
}
