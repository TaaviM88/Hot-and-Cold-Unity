using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int scoreCount;
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> coinsCollected;
    //the values degfined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        this.scoreCount = 0;
        playerPosition = Vector3.zero;
        coinsCollected = new SerializableDictionary<string, bool>();
    }
}

