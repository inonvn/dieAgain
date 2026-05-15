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
    public bool playerDie;

    private void Awake()
    {
        instance = this;
        CheckTB();
    }
    public void LoadLV(int LV)
    {
        var e = SaveLV.Find(o=>o.LV == LV);
        if (e != null)
        {
            var e1 = Instantiate(e.GameObject);
            var e2 = Instantiate(movePlayer.gameObject,e.PlayerSpawn,Quaternion.identity);
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
