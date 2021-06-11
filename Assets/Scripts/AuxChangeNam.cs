using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeNamePlayer : MonoBehaviour
{
    public GameObject player;
    public bool exist0 = false;
    public bool exist1 = false;
    public bool exist2 = false;
 
    void Update()
    {
        player = GameObject.Find("PlayerController(Clone)");
        //player3 = GameObject.Find("PlayerController(Clone)");

        //player0
        if(player.name == "PlayerController(Clone)" && !exist0)
        {
            player.name = "player0";
            exist0 = true;
        }

        if(player.name == "player1" && exist0)
        {
            player.name = "player0";
            exist0 = true;
        }

        if(player.name == "player2" && exist0)
        {
            player.name = "player0";
            exist0 = true;
        }

        //player1
        if(player.name == "PlayerController(Clone)" && !exist1)
        {
            player.name = "player1";
            exist1 = true;
        }

        if(player.name == "player2" && exist1)
        {
            player.name = "player1";
            exist1 = true;
        }

        //player2
        if(player.name == "PlayerController(Clone)" && !exist2)
        {
            player.name = "player2";
            exist2 = true;
        }
//
        //if(player2.name == "PlayerController(Clone)")
        //{
        //    player2.name = "player2";
        //}
//
        //if(player3.name == "PlayerController(Clone)")
        //{
        //    player3.name = "player3";
        //}

    }
}
