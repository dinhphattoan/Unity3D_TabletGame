using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class LobbyUIManager : IUIScript
{
    //Holding player ui title
    [SerializeField] GameObject playerUIPlaceHolder;
    [SerializeField] GameObject playerBarLayoutPane; // Panel holds player's bars
    [SerializeField] GameObject loadingBarPanel; // Panel holds loading bar
    PlayerHostManager playerHostManager;
    // Start is called before the first frame update
    void Start()
    {
        base.Initialize();
        playerHostManager = FindObjectOfType<PlayerHostManager>();
        //Implement connecting to the host and get the list of other client script on the host
        ReUpdateList(playerHostManager.GetPlayerList());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ReUpdateList(List<PlayerHostManager.Player> listPlayer)
    {
        //Detach the loading bar
        loadingBarPanel.SetActive(false);
        loadingBarPanel.transform.parent = null;
        //Clear old player bar
        foreach (Transform child in playerBarLayoutPane.transform)
        {
            Destroy(child.gameObject);
        }
        int i = 0;
        foreach (var player in listPlayer)
        {
            AddNewPlayerBar(i, player.name); i++;
        }
        loadingBarPanel.SetActive(true);
        loadingBarPanel.transform.SetParent(playerBarLayoutPane.transform);
    }
    void AddNewPlayerBar(int index, string name)
    {

        //Add new placeholder 
        GameObject newPlayerBar = Instantiate(playerUIPlaceHolder, playerBarLayoutPane.transform);
        newPlayerBar.SetActive(true);
        newPlayerBar.name = name;
        FillPlayerUIForm(index, name, newPlayerBar);
        newPlayerBar.transform.SetParent(playerBarLayoutPane.transform);
    }
    //Filling the infomation 
    void FillPlayerUIForm(int playerIndex, string playerName, GameObject placeHolder)
    {
        TextMeshProUGUI playertitle = placeHolder.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI info = placeHolder.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        playertitle.text = "Player " + playerIndex.ToString();
        info.text = "Name: " + playerName;
    }
    public void OnClick_RefreshPlayer()
    {
        ReUpdateList(playerHostManager.GetPlayerList());
    }
}
