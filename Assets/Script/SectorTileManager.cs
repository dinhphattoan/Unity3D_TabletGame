using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class SectorTileManager : MonoBehaviour
{
    protected static GameObject tile1;
    protected static GameObject tile2;
    protected static GameObject tile3;
    protected static List<int> listTileType = new List<int>();

    [SerializeField] protected static List<GameObject> listPlatform = new List<GameObject>();



    public static void SetTileType(int elementIndex, int tileIndex)
    {
        
        if (elementIndex >= listPlatform.Count)
        {
           return;
        }
        listTileType[elementIndex] = tileIndex;
    }


    public int CheckIndexOnTile(int elementAt)
    {
        return listPlatform[elementAt].GetComponent<PlatformSectorScript>().tileIndex;
    }
    public static int GetTileCount()
    {
        return listTileType.Count;
    }
    public static int GetTileTypeAtIndex(int index)
    {
        return listTileType[index];
    }
    public static GameObject GetTileGameObjectAtIndexType(int index)
    {
        return listPlatform[index];
    }

    public static void AddNewTileObject(GameObject tileObject)
    {
        if (tileObject.GetComponent<PlatformSectorScript>() != null)
        {
            listPlatform.Add(tileObject);
        }
    }
    public static void RemoveTileObject(GameObject tileObject)
    {
        if (tileObject.GetComponent<PlatformSectorScript>() != null)
        {
            int index = listPlatform.IndexOf(tileObject);
            listPlatform.RemoveAt(index);
        }
    }
}
