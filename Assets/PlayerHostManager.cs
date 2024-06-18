using System;
using System.Collections;
using System.Collections.Generic;
using CI.PowerConsole;
using Unity.Netcode;
using UnityEngine;
public class PlayerHostManager : NetworkBehaviour
{
    [Serializable]
    public struct Player : INetworkSerializable
    {
        public string name;
        public int modelFigureId;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref name);
            serializer.SerializeValue(ref modelFigureId);
        }
    }
    [SerializeField]
    public Player player;
    //List of player info that connected, 0 is the host 
    NetworkVariable<List<Player>> networkPlayerHostManager = new NetworkVariable<List<Player>>
   (new List<Player>(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    NetworkVariable<int> random = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public override void OnNetworkSpawn()
    {
        networkPlayerHostManager.Value.Add(this.player);
        base.OnNetworkSpawn();
    }
    public override void OnNetworkDespawn()
    {
        networkPlayerHostManager.Value.Remove(this.player);
        base.OnNetworkDespawn();
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.T))
            random.Value++;
        PowerConsole.Log(0, random.Value.ToString());
    }
    //Hosting the local server, save all the current map setting and begin to come to awaiting for players state
    public void SetPlayerName(ulong index, string name)
    {
        this.player.name = name;
        networkPlayerHostManager.Value[(int)index] = this.player;


    }

    public override void OnDestroy()
    {
        networkPlayerHostManager.Value.Remove(player);
    }
    [ServerRpc]
    public void SendPlayerHostManagerToAllClientsServerRpc()
    {
        //Validate new client data
        //Check if name is duplicate
        foreach (var players in networkPlayerHostManager.Value)
        {
            if (player.name.Equals(players.name))
            {
                return;
            }
            if (players.modelFigureId == player.modelFigureId)
            {
                return;
            }
        }

        networkPlayerHostManager.Value.Add(player);
        ReUpdateListClientRpc();
    }
    [ClientRpc]
    public void ReUpdateListClientRpc()
    {

        var lobbyUIManager = FindObjectOfType<LobbyUIManager>();
        if (lobbyUIManager)
        {
            lobbyUIManager.ReUpdateList(networkPlayerHostManager.Value);
        }
    }
    public List<PlayerHostManager.Player> GetPlayerList()
    {
        return networkPlayerHostManager.Value;
    }
}
