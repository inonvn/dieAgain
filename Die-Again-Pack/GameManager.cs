using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<SaveObj> SaveLV;
    public int LvNow;
    public CheckTypeDriver CheckType;
    public MovePlayer movePlayer;
    public Action<int> OnLevelLoaded;
    public Action OnDieLoaded;
    public bool playerDie;
    public int deathCount;

    public void PlayerDied()
    {
        playerDie = true;
        deathCount++;
        OnDieLoaded?.Invoke();
    }

    private void Awake()
    {
        instance = this;
        CheckTB();
    }
    private GameObject currentLevelObj;
    public GameObject currentPlayerObj;

    public void LoadLV(int LV)
    {
        var e = SaveLV.Find(o=>o.LV == LV);
        LvNow = LV;

        if (e != null)
        {
            if (currentLevelObj != null) Destroy(currentLevelObj);
            if (currentPlayerObj != null) Destroy(currentPlayerObj);

            currentLevelObj = Instantiate(e.GameObject);
           
            currentPlayerObj = Instantiate(movePlayer.gameObject,e.spawnPos,Quaternion.identity);
            
            // Đặt lại trạng thái sống của người chơi
            playerDie = false;
            
            OnLevelLoaded?.Invoke(LV);
        }
    }
    public void CheckTB ()
    {
        if (Application.isMobilePlatform)
        {
            CheckType = CheckTypeDriver.moblie;
        }
        else
        {
            CheckType = CheckTypeDriver.Pc;
        }
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
public enum CheckTypeDriver
{
    Pc,
    moblie,
}
