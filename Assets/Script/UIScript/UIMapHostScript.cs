using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIMapHostScript : IUIScript
{
    [Space]
    [Header("UI Attributes")]
    public Color buffColor;
    public Color failedColor;

    public float keyboardSeperateSpace = 1f;

    [Header("UI Panel GameObjects")]
    [Space]
    [SerializeField] GameObject gameObjectMapSetting; //Map Setting Panel
    [SerializeField] GameObject gameObjectPlayerSetting; //Player Setting Panel
    [SerializeField] GameObject gameObjectPlayerTab; //Player Tab Panel for editing
    [SerializeField] GameObject gameObjectModelPortfolio; // Model Portfolio showcase
    [Header("UI Components")]
    [SerializeField] TextMeshProUGUI textMeshNumberPlayer;
    [SerializeField] Transform panelTablet;
    //A keyboard holding button for tiles
    [SerializeField] List<GameObject> listButtons = new List<GameObject>();
    [Header("UI Interactions")]
    [SerializeField] GameObject textMeshPlayerNameInput; 
    [SerializeField] List<Button> playerTabButtons = new List<Button>(); 
    [SerializeField] List<GameObject> modelFigurePanels = new List<GameObject>(); 

    int numberplayer = 1;
    int playerIdEditing = -1;
    public List<Player> players = new List<Player>();

    [Header("Model Figure")]
    [Space]
    [SerializeField] GridLayoutGroup modelContentGrid;
    [SerializeField] GameObject modelFigurePanelPlaceHolder;
    [SerializeField] List<GameObject> listOfFigurePanel = new List<GameObject>();

    [Header("Reference scripts")]
    [Space]
    [SerializeField] MapInitializeScript mapInitializeScript;
    [Header("Audio clips")]
    [Space]
    public AudioClip errorClip; //Error sound


    void Start()
    {
        base.Initialize();
        IniMap();
        BackgroundTransistionOpen();
        mapInitializeScript = FindObjectOfType<MapInitializeScript>();
        mapInitializeScript.InitTiles();
    }

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
                horiPosition += rectTransform.sizeDelta.x + keyboardSeperateSpace;

                if (horiPosition >= (panel.sizeDelta.x - rectTransform.sizeDelta.x))
                {
                    horiPosition = rectTransform.sizeDelta.x;
                    vertPosition -= rectTransform.sizeDelta.y + keyboardSeperateSpace;
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
        var platformscript = SectorTileManager.GetTileGameObjectAtIndexType(index).GetComponent<PlatformSectorScript>();
        int indexTile = platformscript.NextTileType();
        SectorTileManager.SetTileType(index, indexTile);
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
        if(playerIdEditing!=-1)
        {
            OnClick_PlayerTab(playerIdEditing);
        }
        foreach (var player in players)
        {
            if (player.modelFigureId == -1 || player.name == "")
            {
                soundManager.PlaySFX(errorClip);
                return;
            }
        }
        //Save the last editing
        if (playerIdEditing != -1)
        {
            //Save the name
            players[playerIdEditing].name = textMeshPlayerNameInput.GetComponent<TMP_InputField>().text;
        }
        mapInitializeScript.SaveMapSetting();
        mapInitializeScript.SetPlayerList(this);
        this.BackgroundTransistionClose(2);
    }
}
