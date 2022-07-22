using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : SingletonMonoBehaviour<BattleManager>
{
    [SerializeField] private GameObject[] playersObject;
    private Player[] players;

    private void Start()
    {
        players = new Player[2]{
            playersObject[0].GetComponent<Player>(),
            playersObject[1].GetComponent<Player>()
        };
    }

    void Update()
    {
        SetPlayerSide();
    }

    //make plyers look at each others (only for 2 players match)
    private void SetPlayerSide()
    {
        if(playersObject[0].transform.position.x < playersObject[1].transform.position.x)
        {
            players[0].shouldLookRight = true;
            players[1].shouldLookRight = false;
        }
        else
        {
            players[0].shouldLookRight = false;
            players[1].shouldLookRight = true;
        }
    }
}
