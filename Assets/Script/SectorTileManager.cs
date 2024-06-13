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
    //List of choosen tiles from player
    public static List<int> listTileType = new List<int>();

    public static List<GameObject> listPlatform = new List<GameObject>();
    /// <summary>
    /// Set the tile type, return -1 when change successful, others mean fail at index
    /// </summary>
    /// <param name="elementIndex">The index of the tile in the list</param>
    /// <param name="tileIndex">The tile index that need to be transformed to</param>
    /// <returns></returns>
    public static int SetTileType(int elementIndex, int tileIndex)
    {
        if (elementIndex >= listPlatform.Count)
        {
            return elementIndex;
        }
        else if (tileIndex == 1)
        {
            listPlatform[elementIndex] = tile2;
        }
        else if (tileIndex == 2)
        {
            listPlatform[elementIndex] = tile3;
        }
        return -1;
    }
    public int CheckIndexOnTile(int elementAt)
    {
        return listPlatform[elementAt].GetComponent<PlatformSectorScript>().tileIndex;
    }

    public static void EvaluateCount()
    {
        //Fill all the tile that set to special
        int trace = 0;
        foreach (var i in listTileType)
        {
            SetTileType(trace++, i);

        }
    }
}
