using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Player> players = new List<Player>();

    [SerializeField] List<GameObject> modelFigures = new List<GameObject>();
    //Determine the turn order of the players to play the game
    [Header("UI Panels")]
    [Space]
    [SerializeField] GameObject gameObjectPlayerScoreBoardPanel; //Scoreboard Panel
    [SerializeField] GameObject gameObjectScoreBoardContent; // Content of the scoreboard
    [SerializeField] GameObject gameObjectScoreBoardPlayerPanel; //Player panel

    [Header("In Game System Attributes")]

    float force;
    int playerIndex = 0; //Player turn
    List<bool> winners = new List<bool>();
    [Header("Sound clips")]
    [Space]
    [SerializeField] AudioClip BuffClip;
    [SerializeField] AudioClip FailClip;
    [SerializeField] AudioClip rollDiceSuccess;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] List<AudioClip> jumpLevelClip;
    [SerializeField] AudioClip winnerClip;
    [SerializeField] AudioClip finalWinnerClip;
    [SerializeField] AudioClip transistionClip;

    [Header("Reference scritps")]
    [Space]
    [SerializeField] UIGameScript uiGameScript;
    [SerializeField] SoundManager soundManager;
    MapInitializeScript mapInitializeScript;
    [SerializeField] DiceManager diceManager;
    void Awake()
    {
        //Fetch prev scene script
        mapInitializeScript = FindObjectOfType<MapInitializeScript>();
        //Load player data
        this.players = mapInitializeScript.Players;
        foreach (var player in players)
        {
            player.modelFigure = Instantiate(modelFigures[player.modelFigureId]);

        }
        ReallocatePlayerModelFigure(0, players);
    }
    void Start()
    {




        StartCoroutine(GameStart());

    }



    IEnumerator GameStart()
    {
        yield return StartCoroutine(uiGameScript.ShowBriefMessage("Game Start!", 80));
        yield return StartCoroutine(GamePlay());
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
            yield return StartCoroutine(uiGameScript.ClickToContinue("Player " + players[playerIndex].name + " turn!\nPress Space to continue", 40));
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
                    yield return StartCoroutine(uiGameScript.ShowBriefMessage(diceManager.GetDiceValue().ToString(), 100));
                    yield return StartCoroutine(uiGameScript.CloseRollDicePanel());
                    break;
                }

            }
            //Get the dice result
            int diceValue = diceManager.GetDiceValue();
            int indexDestination = diceValue + players[playerIndex].tileIndex;
            if (indexDestination > SectorTileManager.GetTileCount() - 1)
            {
                soundManager.PlaySFX(FailClip);
                yield return StartCoroutine(uiGameScript.ShowBriefMessage("Number steps is too big!", 70));
                diceManager.ResetPosition();
                playerIndex++;
                continue;
            }
            int jumpedStep = 0;
            diceManager.ResetPosition();
            //Assign from-to destination
            while (players[playerIndex].tileIndex != indexDestination)
            {
                soundManager.PlaySFX(jumpClip);
                players[playerIndex].modelFigure.GetComponent<Rigidbody>().isKinematic = true;
                yield return StartCoroutine(PlayerJumpTileToTile(players[playerIndex], players[playerIndex].tileIndex + 1, jumpedStep++));
                players[playerIndex].modelFigure.GetComponent<Rigidbody>().isKinematic = false;
            }
            //Check if the player is standing special tile
            //Is at final
            if (players[playerIndex].tileIndex == SectorTileManager.GetTileCount() - 1)
            {
                soundManager.PlaySFX(winnerClip);
                yield return StartCoroutine(uiGameScript.ShowBriefMessage("Player " + players[playerIndex].name + " Win!", 80));
                players[playerIndex].isWon = true;
                players[playerIndex].rank = winnerCount + 1;
                winnerCount++;
                continue;
            }
            //Buff tile

            if (SectorTileManager.GetTileTypeAtIndex(players[playerIndex].tileIndex) == 1)
            {
                soundManager.PlaySFX(BuffClip);
                yield return StartCoroutine(uiGameScript.ShowBriefMessage("Buff!", 70));
                players[playerIndex].buffTimes++;
                continue;
            }
            while (SectorTileManager.GetTileTypeAtIndex(players[playerIndex].tileIndex) == 2)
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
        }
        soundManager.PlaySFX(finalWinnerClip);
        yield return StartCoroutine(uiGameScript.ShowBriefMessage("Game Complete!", 70));
        yield return StartCoroutine(FinalizeScore());
    }

    IEnumerator PlayerJumpTileToTile(Player player, int tileto, int jumpedStep)
    {
        Vector3 desJumping = mapInitializeScript.gameObjectTiles[tileto].transform.position;
        desJumping = new Vector3(desJumping.x, desJumping.y + 5f, desJumping.z);
        //Get a direction rotation from player to the next tile
        Vector3 direction = mapInitializeScript.gameObjectTiles[tileto].transform.position - players[playerIndex].modelFigure.transform.position;
        soundManager.PlaySFX(jumpLevelClip[jumpedStep++]);

        yield return StartCoroutine(
        MapInitializeScript.Slerp(players[playerIndex].modelFigure.transform, players[playerIndex].modelFigure.transform.position,
        new Vector3(desJumping.x, desJumping.y, desJumping.z), players[playerIndex].modelFigure.transform.rotation, Quaternion.LookRotation(direction)));
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
        ScoreboardData scoreboardData = new ScoreboardData();
        for (int i = 0; i < players.Count; i++)
        {
            var playerPanel = Instantiate(gameObjectScoreBoardPlayerPanel, gameObjectScoreBoardContent.transform);
            //Assign info
            playerPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = players[i].rank.ToString();
            playerPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = players[i].name;
            playerPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = players[i].turns.ToString();
            playerPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = players[i].buffTimes.ToString();
            playerPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = players[i].failTimes.ToString();
            playerPanel.GetComponent<Image>().color = new Color(255, 255, 255, 0);
            scoreboardData.AddPlayer(players[i].name, players[i].turns, players[i].buffTimes, players[i].failTimes);
            yield return null;
        }
        SaveSystem.SaveScoreBoard(scoreboardData);
    }

    void ReallocatePlayerModelFigure(int tileIndex, List<Player> listPlayer)
    {
        Vector3 initialVector = mapInitializeScript.gameObjectTiles[tileIndex].transform.position;
        foreach (var player in listPlayer)
        {
            player.modelFigure.transform.localScale = new Vector3(1f, 1f, 1f);
            player.modelFigure.transform.position = new Vector3(initialVector.x, initialVector.y + 2f, initialVector.z);
        }
    }
    public Player GetPlayerAtIndex(int index)
    {
        return players[index];
    }
    public List<Player> GetAllPlayers()
    {
        return players;
    }
}
[Serializable]
public class Player
{
    public string name = "";
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
