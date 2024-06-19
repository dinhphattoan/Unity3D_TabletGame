using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;


public class MapInitializeScript : SectorTileManager
{
    //Normal tile
    GameObject _tile1;
    //Buffed tile
    GameObject _tile2;
    //Failed tile
    GameObject _tile3;
    List<Player> players;
    [SerializeField] SplineContainer splineContainer;
    [Space]
    [Header("Map attributes justifiable")]
    public float number_of_sector;
    public readonly List<GameObject> gameObjectTiles = new List<GameObject>();


    public SplineInstantiate splineInstantiate;
    void Start()
    {
        SectorTileManager.tile1 = _tile1;
        SectorTileManager.tile2 = _tile2;
        SectorTileManager.tile3 = _tile3;
        listTileType.Clear();
        for (int i = 0; i < this.number_of_sector; i++)
        { listTileType.Add(0); }
    }
    public void InitTiles()
    {
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


    public void SaveMapSetting()
    {
        //Save the map setting object to next scene
        DontDestroyOnLoad(transform.parent.gameObject);

    }

    public IEnumerator Slerp(Transform figure, Vector3 startPos, Vector3 endPos, Quaternion startRotation, Quaternion endRotation)
    {
        Debug.Log("Start: " + startPos + " end: " + endPos);
        float journeyTime = 1.0f;

        float startTime;
        Vector3 centerPoint;
        Vector3 startRelCenter;
        Vector3 endRelCenter;
        startTime = Time.time;
        float fracComplete = 0f;
        while (fracComplete <= 1)
        {
            centerPoint = (startPos + endPos) * .5f;
            centerPoint -= Vector3.up;
            startRelCenter = startPos - centerPoint;
            endRelCenter = endPos - centerPoint;
            fracComplete = (Time.time - startTime) / journeyTime;
            figure.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete * 2.5f);
            figure.position += centerPoint;
            figure.rotation = Quaternion.Slerp(startRotation, endRotation, fracComplete * 2.5f);
            yield return null;
        }

    }


    public void UnParentChildSectorTile()
    {
        this.transform.parent.transform.position = new Vector3(0, 0, 0);
        foreach (var ob in gameObjectTiles)
        {

            ob.transform.parent = null;
        }
    }
    public void SetPlayerList(UIMapHostScript hostScript)
    {
        players = hostScript.players;
    }
    public List<Player> Players
    {
        get
        {
            return players;
        }
    }

}
