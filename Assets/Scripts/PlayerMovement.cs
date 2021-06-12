using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviourPunCallbacks, IDamageable
{
    CharacterController characterController;
    [Header("Opciones de Personaje")]
    [SerializeField]
    float walkSpeed = 4.0f;
    float runSpeed = 12.0f;
    float jumpSpeed = 8.0f;
    float gravity = 20.0f;
    //float timeRunning = 0.0f;
    private float Loop ;
    private bool isRunning = false;
    private bool isWalking = false;
    private float sprintDuration = 2.0f;
    private float sprintTimer = 0.0f;

    [Header("Opciones de Camara")]
    [SerializeField] Camera cam;
    [SerializeField] GameObject cameraHolder;
    private float mouseHorizontal = 3.0f;
    private float mouseVertical = 2.0f;
    //private float minRotation = -65.0f;
    //private float maxRotation = 20.0f;
    private float h_mouse , v_mouse;

    [SerializeField] Item[] items;

    [SerializeField] Image healthbarImage; //Healthbar
    [SerializeField] GameObject ui;

    int itemIndex;
    int previousItemIndex = -1;

    PhotonView PV;

    const float maxHealth = 100f;
    float currentHealth = maxHealth;

    PlayerManager playerManager;

    private Vector3 move = Vector3.zero;
    
    void Awake()
    {
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if(PV.IsMine)
        {
            EquipItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(ui);
        }
    }

    void FixedUpdate()
    {
        if(!PV.IsMine)
            return;
        h_mouse = mouseHorizontal * Input.GetAxis("Mouse X");
        //v_mouse += mouseVertical * Input.GetAxis("Mouse Y");

        //v_mouse = Mathf.Clamp(v_mouse, minRotation, maxRotation);
        //cam.transform.localEulerAngles = new Vector3(-v_mouse, 0, 0);
        transform.Rotate(0, h_mouse, 0);

        v_mouse += Input.GetAxis("Mouse Y") * mouseVertical;
        v_mouse = Mathf.Clamp(v_mouse, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * v_mouse;

        
        if(characterController.isGrounded)
        {

            move = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            if ( Input.GetAxis( "Horizontal" ) !=0f || Input.GetAxis( "Vertical" ) != 0f)
                {
                    if ( Input.GetKey( "left shift" ) || Input.GetKey( "right shift" ) && sprintTimer < 2 )
                    {
                        // Running
                        isWalking = false;
                        isRunning = true;
                    }
                    else
                    {
                        // Walking
                        isWalking = true;
                        isRunning = false;
                    }
                }
            else
            {
                // Stopped
                isWalking = false;
                isRunning = false;
            }
            
            // check the sprint timer
            if ( isRunning )
            {
                sprintTimer += Time.deltaTime;

                if ( sprintTimer > sprintDuration )
                {
                    isRunning = false;
                    isWalking = true;
                    if(isWalking){
                        move = transform.TransformDirection(move) * walkSpeed;
                    }
                }
                else if ( characterController.isGrounded )
                {
                    move = transform.TransformDirection(move) * runSpeed;
                }
            }
            else
            {
                sprintTimer -= Time.deltaTime * 0.4f;
                move = transform.TransformDirection(move) * walkSpeed;
            }

            sprintTimer = Mathf.Clamp( sprintTimer, 0, sprintDuration );
            
            if (Input.GetKey(KeyCode.Space))
                move.y = jumpSpeed;
        }

        move.y -= gravity * Time.deltaTime;

        characterController.Move(move * Time.deltaTime);
        for(int i = 0; i < items.Length; i++)
        {
            if(Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f) //Change gun
        {
            if(itemIndex >= items.Length - 1)
            {
                EquipItem(0);
            }
            else
            {
                EquipItem(itemIndex + 1);
            }
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f) //Change gun
        {
            if(itemIndex <= 0)
            {
                EquipItem(items.Length - 1);
            }
            else
            {
                EquipItem(itemIndex - 1);
            }
        }

        if(Input.GetMouseButtonDown(0)) //Use gun/item
        {
            items[itemIndex].Use();
        }

        if(transform.position.y < -10f) //Die if you fall
        {
            Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
            this.transform.position = spawnpoint.position;
            this.transform.rotation = spawnpoint.rotation;
        }
    }

    void EquipItem(int _index){
        if(_index == previousItemIndex)
            return;
        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);

        if(previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }
        previousItemIndex = itemIndex;

        if(PV.IsMine){
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if(!PV.IsMine)
            return;

        currentHealth -= damage;

        healthbarImage.fillAmount = currentHealth / maxHealth;

        if(currentHealth <= 0)
        {
            healthbarImage.fillAmount = 1;
            currentHealth = maxHealth;
            Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
            this.transform.position = spawnpoint.position;
            this.transform.rotation = spawnpoint.rotation;
        }
    }

    void Die()
    {
        playerManager.Die();
    }
}