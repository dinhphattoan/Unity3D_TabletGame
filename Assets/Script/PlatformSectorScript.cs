using UnityEngine;

public class PlatformSectorScript : MonoBehaviour
{
    //0 is normal tile, 1 is buffed tile, 2 is tile fail
    public int tileIndex = 0;
    void Awake()
    {
        SectorTileManager.AddNewTileObject(this.gameObject);
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
        SectorTileManager.RemoveTileObject(this.gameObject);
    }
}
