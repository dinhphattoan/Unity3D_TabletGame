using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    public List<Player> players;
    [SerializeField] SplineContainer splineContainer;
    [Space]
    [Header("Map attributes justifiable")]
    public float number_of_sector;
    public readonly List<GameObject> gameObjectTiles = new List<GameObject>();
    //Buff for additional turn


    //Attributes for road initialization
    public SplineInstantiate splineInstantiate;
    // Start is called before the first frame update
    void Start()
    {
        SectorTileManager.tile1 = _tile1;
        SectorTileManager.tile2 = _tile2;
        SectorTileManager.tile3 = _tile3;
        listTileType.Clear();
        for (int i = 0; i < this.number_of_sector; i++)
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
        //Iterate over the listTileType and create new tiles
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
    //Save all the change
    public void SaveMapSetting()
    {
        //Save the map setting object to next scene

        DontDestroyOnLoad(transform.parent.gameObject);

    }
    public Vector3 startPos;
    public Vector3 endPos;
    public Quaternion startRotation;
    public Quaternion endRotation;
    public float journeyTime = 1.0f;
    public float speed;
    public bool repeatable;

    float startTime;
    Vector3 centerPoint;
    Vector3 startRelCenter;
    Vector3 endRelCenter;
    public void GetCenter(Vector3 direction)
    {
        centerPoint = (startPos + endPos) * .5f;
        centerPoint -= direction;
        startRelCenter = startPos - centerPoint;
        endRelCenter = endPos - centerPoint;
    }
    public IEnumerator Slerp(Transform figure)
    {
        Debug.Log("Start: " + startPos + " end: " + endPos);
        startTime = Time.time;
        float fracComplete = 0f;
        while (fracComplete <= 1)
        {
            GetCenter(Vector3.up);
            fracComplete = (Time.time - startTime) / journeyTime;
            figure.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete*2);
            figure.position += centerPoint;
            figure.rotation = Quaternion.Slerp(startRotation, endRotation, fracComplete*2);
            yield return null;
        }

    }
    /// <summary>
    /// Bring each sector tile out to world space scale
    /// </summary>
    public void UnParentChildSectorTile()
    {
        this.transform.parent.transform.position = new Vector3(0, 0, 0);
        foreach (var ob in gameObjectTiles)
        {

            ob.transform.parent = null;
        }
    }
}
