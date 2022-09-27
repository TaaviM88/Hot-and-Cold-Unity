using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public long lastUpdated;
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

    public int GetPercentageComplete()
    {
        int totalCollected = 0;
        foreach (bool collected in coinsCollected.Values)
        {
            if(collected)
            {
                totalCollected++;
            }
        }

        //ensure we don't divide by 0 when calculating the percentage
        int percentageCompleted = -1;
        if(coinsCollected.Count != 0)
        {
            percentageCompleted = (totalCollected * 100 / coinsCollected.Count);
        }
        return percentageCompleted;
    }
}

