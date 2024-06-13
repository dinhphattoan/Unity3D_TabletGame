using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class MapInitializeScript : SectorTileManager
{
    //Normal tile
    public GameObject _tile1;
    //Buffed tile
    public GameObject _tile2;
    //Failed tile
    public GameObject _tile3;

    [SerializeField] SplineContainer splineContainer;
    [Space]
    [Header("Map attributes justifiable")]
    public float number_of_sector;
    public List<GameObject> gameObjectTiles = new List<GameObject>();
    //Buff for additional turn


    //Attributes for road initialization
    public SplineInstantiate splineInstantiate;

    // Start is called before the first frame update
    void Start()
    {
        SectorTileManager.tile1 = _tile1;
        SectorTileManager.tile2 = _tile2;
        SectorTileManager.tile3 = _tile3;
<<<<<<< HEAD
        listTileType.Clear();
=======
>>>>>>> 635c24750cba69e253067105a180fe52f510d8c3
        for (int i = 0; i < 50; i++)
        { listTileType.Add(0); }
    }
    /// <summary>
    /// Need to call when there's a change of numbers of tile type to reupdate all tile, clear all previous tiles and replace the new one
    /// </summary>
    /// <param name="prevCount">The previous count from last of update</param>
    public void InitTiles()
    {
        // Create a temporary list to store the new tiles
        List<GameObject> newTiles = new List<GameObject>();

        foreach (var ob in gameObjectTiles)
        {
            Destroy(ob);
        }
        gameObjectTiles.Clear();
        // Iterate over the listTileType and create new tiles
        for (int i = 0; i < listTileType.Count; i++)
        {
            GameObject newTile = Instantiate(listTileType[i] == 0 ? tile1 : listTileType[i] == 1 ? tile2 : tile3, splineContainer.transform);
            newTile.transform.position = listPlatform[i].transform.position;
            newTile.SetActive(true);
            newTiles.Add(newTile);
        }

        // Clear the existing game object tiles
        gameObjectTiles.Clear();

        // Add the new tiles to the game object tiles
        gameObjectTiles.AddRange(newTiles);
    }
    // Update is called once per frame
    void Update()
    {
    }
}