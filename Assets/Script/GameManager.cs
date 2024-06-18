using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Player> players = new List<Player>();
    public List<bool> winners = new List<bool>();
    [SerializeField] List<GameObject> modelFigures = new List<GameObject>();
    //Determine the turn order of the players to play the game
    [SerializeField] List<int> TurnOrder = new List<int>(); // Element index is the player id, value are order
    public GameObject gameObjectPlayerScoreBoardPanel; //Scoreboard Panel
        public GameObject gameObjectScoreBoardContent; // Content of the scoreboard
    public GameObject gameObjectScoreBoardPlayerPanel; //Player panel

    public bool isGamePause = true;
    public bool nextTurnAwait = false; //True if the game is waiting for the next turn to start
    public DiceManager diceManager;
    public float force;

    public int playerIndex = 0; //Player turn
    public int nexTurnDelaySecond = 1;
    float nextTurnDelayTimer = 0;
    UIGameScript uiGameScript;
    SoundManager soundManager;
    bool isIntroduced = false;
    float playerSpeed = 5f;
    [Header("Sound clips")]
    public AudioClip BuffClip;
    public AudioClip FailClip;
    public AudioClip rollDiceSuccess;
    public List<AudioClip> jumpLevelClip;
    public AudioClip winnerClip;
    public AudioClip transistionClip;

    MapInitializeScript mapInitializeScript;
    void Start()
    {
        mapInitializeScript = FindObjectOfType<MapInitializeScript>();
        diceManager = FindObjectOfType<DiceManager>();
        uiGameScript = FindObjectOfType<UIGameScript>();
        soundManager = FindObjectOfType<SoundManager>();
        //Load player data
        this.players = mapInitializeScript.players;
        foreach (var player in players)
        {
            player.modelFigure = Instantiate(modelFigures[player.modelFigureId]);

        }
        mapInitializeScript.UnParentChildSectorTile();
        ReallocatePlayerModelFigure(0, players);

        StartCoroutine(GameStart());

    }
    IEnumerator GameStart()
    {
        yield return StartCoroutine(uiGameScript.ShowBriefMessage("Game Start!", 80));
        yield return StartCoroutine(TurnOrder_Initialize());
        yield return StartCoroutine(GamePlay());
    }
    IEnumerator TurnOrder_Initialize()
    {
        int turn = 0;
        while (TurnOrder.Count < players.Count)
        {
            TurnOrder.Add(turn);
            turn++;
            yield return null;
        }
    }
    IEnumerator GamePlay()
    {
        int winnerCount = 0;
        while (winnerCount != players.Count)
        {
            if (playerIndex >= players.Count)
            {
                playerIndex = 0;
            }
            while (players[playerIndex].isWon)
            {
                playerIndex++;
                if (playerIndex >= players.Count)
                {
                    playerIndex = 0;
                }
            }

            soundManager.PlaySFX(transistionClip);
            yield return StartCoroutine(uiGameScript.ClickToContinue("Player " + players[playerIndex].name + " turn!", 70));
            players[playerIndex].turns++;

            yield return new WaitForSeconds(1);



            while (true)
            {

                //Wait for Input force from player
                yield return StartCoroutine(HandleInputForce());
                //Wait for the dice to roll
                yield return StartCoroutine(diceManager.RollDiceOnForce(force, uiGameScript.mouseDown, uiGameScript.mouseUp));
                if (diceManager.GetDiceValue() == -1)
                {
                    yield return StartCoroutine(uiGameScript.ShowBriefMessage("Roll again!", 70));
                }
                else
                {
                    yield return StartCoroutine(uiGameScript.ShowBriefMessage(diceManager.GetDiceValue().ToString() + "!", 70));
                    yield return StartCoroutine(uiGameScript.CloseRollDicePanel());
                    break;
                }

            }
            //Get the dice result
            int diceValue = 5;
            int indexDestination = diceValue + players[playerIndex].tileIndex;
            if (indexDestination > SectorTileManager.listTileType.Count - 1)
            {
                yield return StartCoroutine(uiGameScript.ShowBriefMessage("Number steps is too big!", 70));
                continue;
            }
            int jumpedStep = 0;
            diceManager.ResetPosition();
            //Assign from-to destination
            while (players[playerIndex].tileIndex != indexDestination )
            {
                yield return StartCoroutine(PlayerJumpTileToTile(players[playerIndex], players[playerIndex].tileIndex + 1, jumpedStep++));
                if(players[playerIndex].tileIndex == SectorTileManager.listTileType.Count - 1)
                {
                    break;
                }
            }
            //Check if the player is standing special tile
            //Is at final
            if (SectorTileManager.listTileType[players[playerIndex].tileIndex] == SectorTileManager.listTileType.Count - 1)
            {
                soundManager.PlaySFX(winnerClip);
                yield return StartCoroutine(uiGameScript.ShowBriefMessage("Player " + players[playerIndex].name, 80));
                players[playerIndex].isWon = true;
                players[playerIndex].rank = winnerCount + 1;
                winnerCount++;
                continue;
            }
            //Buff tile

            if (SectorTileManager.listTileType[players[playerIndex].tileIndex] == 1)
            {
                soundManager.PlaySFX(BuffClip);
                yield return StartCoroutine(uiGameScript.ShowBriefMessage("Buff!", 70));
                players[playerIndex].buffTimes++;
                continue;
            }
            while (SectorTileManager.listTileType[players[playerIndex].tileIndex] == 2)
            {
                soundManager.PlaySFX(FailClip);
                jumpedStep = 0;

                yield return StartCoroutine(uiGameScript.ShowBriefMessage("Fail!", 70));
                players[playerIndex].failTimes++;
                for (int i = 0; i < 3; i++)
                {
                    if (players[playerIndex].tileIndex - 1 < 0) break;
                    yield return StartCoroutine(PlayerJumpTileToTile(players[playerIndex], players[playerIndex].tileIndex - 1, jumpedStep++));
                }
            }

            playerIndex++;

            //Jump player to tile
        }

        yield return StartCoroutine(uiGameScript.ShowBriefMessage("Game Complete!", 70));

        yield return StartCoroutine(FinalizeScore());
    }
    IEnumerator PlayerJumpTileToTile(Player player, int tileto, int jumpedStep)
    {
        mapInitializeScript.startPos = players[playerIndex].modelFigure.transform.position;
        Vector3 desJumping = mapInitializeScript.gameObjectTiles[tileto].transform.position;
        desJumping = new Vector3(desJumping.x, desJumping.y + 5f, desJumping.z);
        mapInitializeScript.endPos = desJumping;
        mapInitializeScript.startRotation = players[playerIndex].modelFigure.transform.rotation;
        soundManager.PlaySFX(jumpLevelClip[jumpedStep++]);
        mapInitializeScript.endRotation = mapInitializeScript.gameObjectTiles[tileto + 1].transform.rotation;
        yield return StartCoroutine(
        mapInitializeScript.Slerp(players[playerIndex].modelFigure.transform));
        players[playerIndex].tileIndex = tileto;
    }

    IEnumerator HandleInputForce()
    {
        uiGameScript.ShowForceInputUI();
        yield return StartCoroutine(uiGameScript.ShowRollDicePanel());
        //Pending for input force
        yield return StartCoroutine(uiGameScript.ForceInput());
        force = uiGameScript.forceDrag;
        uiGameScript.HideForceInputUI();


    }
    IEnumerator FinalizeScore()
    {
        gameObjectPlayerScoreBoardPanel.SetActive(true);
        //Sort the player from high to low base on ranking score
        players = players.OrderBy(x => x.rank).ToList();
        for (int i = 0; i < players.Count; i++)
        {
            var playerPanel = Instantiate(gameObjectScoreBoardPlayerPanel, gameObjectScoreBoardContent.transform);
            //Assign info
            playerPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = players[i].rank.ToString();
            playerPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = players[i].name;
            playerPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = players[i].turns.ToString();
            playerPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = players[i].buffTimes.ToString();
            playerPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = players[i].failTimes.ToString();
            playerPanel.GetComponent<Image>().color = new Color(255,255,255,0);
            yield return null;
        }
    }
    public int NextTurn()
    {
        //If the order haven't determine, go to the next player element
        if (TurnOrder.Count <= players.Count)
        {
            playerIndex++;

        }
        //If the order have determined, go to the first player follow from other
        for (int i = 0; i < TurnOrder.Count; i++)
        {
            if (TurnOrder[i] - playerIndex == 1)
            {
                return i;
            }
        }
        return -1;
    }
    /// <summary>
    /// Start to reallocate the player model figure given when the game start
    /// </summary>
    /// <param name="tileIndex"></param>
    /// <param name="listPlayer"></param>
    void ReallocatePlayerModelFigure(int tileIndex, List<Player> listPlayer)
    {
        Vector3 initialVector = mapInitializeScript.gameObjectTiles[tileIndex].transform.position;
        foreach (var player in listPlayer)
        {
            player.modelFigure.transform.localScale = new Vector3(1f, 1f, 1f);
            player.modelFigure.transform.position = new Vector3(initialVector.x, initialVector.y + 2f, initialVector.z);
        }
    }
}
[Serializable]
public class Player
{
    public string name;
    public int tileIndex;
    public GameObject modelFigure;
    public int modelFigureId = -1;
    public int rank;
    public int turns;
    public int failTimes;
    public int buffTimes;
    public bool isWon = false;
    public Player()
    {

    }
    public Player(string name, int tileIndex, int score, int turns, int failTimes, int buffTimes)
    {
        this.name = name;
        this.tileIndex = tileIndex;
        this.rank = score;
        this.turns = turns;
        this.failTimes = failTimes;
        this.buffTimes = buffTimes;
    }
}
