using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    GameObject controller;

    void Awake(){
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if(PV.IsMine){
            CreateController();
        }
    }

    void CreateController()
    {
        //Debug.Log("Insantiated Player Controller");
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[]{PV.ViewID});
    }

    public void Die()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller.transform.position = spawnpoint.position;
        controller.transform.rotation = spawnpoint.rotation;
    }
}
