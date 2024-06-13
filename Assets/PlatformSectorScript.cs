using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformSectorScript : MonoBehaviour
{
    //0 is normal tile, 1 is buffed tile, 2 is tile fail
    public int tileIndex = 0;
    void Awake()
    {
        SectorTileManager.listPlatform.Add(this.gameObject);
    }
    public int NextTileType()
    {
        tileIndex++;
        if (tileIndex > 2)
        {
            tileIndex = 0;
        }
        
        return tileIndex;
    }
    void OnDestroy()
    {
        int prevCount = SectorTileManager.listPlatform.Count;
        SectorTileManager.listPlatform.Remove(this.gameObject);
    }
}
