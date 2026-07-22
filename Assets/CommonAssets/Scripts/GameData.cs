using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class GameData : MonoBehaviour
{
    public static GameData ins;
    public void Awake() 
    {
        if (ins != null) { return; }
        ins = this;

        if (PlayerPrefs.HasKey("gameData"))  { JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("gameData"), ins); }
        
    }

    public int coins = 100;

    public Action onCoinsUpdate;


    public void UpdateCoins(int amount)
    {
        coins += amount;
        SaveData();
        onCoinsUpdate?.Invoke();
    }



    public void SaveData()
    {
        PlayerPrefs.SetString("gameData", JsonUtility.ToJson(ins));
    }

    
}
