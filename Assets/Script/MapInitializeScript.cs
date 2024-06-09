using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInitializeScript : MonoBehaviour
{
    [Header("Preload Resources")]
    //0 for tree light, 1 for tree dark
    [SerializeField] List<GameObject> listPreloadResources;
    //Storing all sectors, 0 is normal, 1 is bonus, 2 is fail
    [SerializeField] List<GameObject> listSectorTile;
    [Space]
    [Header("Map attributes justifiable")]
    public Transform startingPoint; //First initial point of the map race
    public Transform endingPoint;   // Ending point of the map race
    //Numbers of steps in one map to finish, not including first and final steps
    public float number_of_sector;
    //Buff for additional turn
    public float number_of_bonus;
    public float number_of_fail;
    [Space]
    [Header("Road System Attributes")]
    //Attributes for road initialization
    public bool isProtrude;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitializeSteps()
    {

    }
    /// <summary>
    /// Generate the map from the given parameters set from player
    /// </summary>
    void GenerateMap()
    {
        
    }
}
[Serializable]
public struct Sector
{
    public Vector3 location;
    public int index;
    public bool isBonus;
    public bool isFail;
}
