using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeName : MonoBehaviour
{
    public GameObject actual;
    public GameObject before1;
    public GameObject before2;
    public GameObject before3;
    public int a = 1;
    
    void Update()
    {   

        if(a == 1){
            actual = GameObject.Find("PlayerController(Clone)");
            before1 = GameObject.Find("Player00");
            before2 = GameObject.Find("Player01");
            before3 = GameObject.Find("Player02");
                if(actual.name == "PlayerController(Clone)")
                {
                    if(before1.name == "Player00"){
                        actual.name = "Player00";
                        before1.name = "Player01";
                        before2.name = "Player02";
                        before3.name = "Player03";
                        actual = GameObject.Find("PlayerController(Clone)");
                        return;
                    }
                    if(before1.name == "Player01"){
                        actual.name = "Player00";
                        before1 = GameObject.Find("Player00");
                        before1.name = "Player01";
                        before2.name = "Player02";
                        before3.name = "Player03";
                        return;
                    }
                    a = a + 1;
                }
        }
        
    }
}