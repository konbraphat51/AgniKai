using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : SingletonMonoBehaviour<BattleManager>
{
    [SerializeField] private GameObject[] players;

    void Update()
    {
        SetPlayerSide();
    }

    //make plyers look at each others (only for 2 players match)
    private void SetPlayerSide()
    {
        
    }
}
