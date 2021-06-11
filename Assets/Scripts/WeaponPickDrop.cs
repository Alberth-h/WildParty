using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickDrop : MonoBehaviour
{
    [Header("Opciones del contedor del arma")]
    //[SerializeField] AQUIVALASCRIPTDELARMA armaScript;
     
    [SerializeField]Rigidbody rb;
    [SerializeField]BoxCollider coll;
    //[SerializeField]Transform player0;
    //[SerializeField]Transform player1;
    [SerializeField]Transform player;
    [SerializeField]Transform weaponContainer, cam;

    [Header("Opciones de las fisicas")]
    [SerializeField]float pickUpRange;
    [SerializeField]float dropForwardForce, dropUpwardForce;
    [SerializeField]bool equipped;

    // Start is called before the first frame update
    private void Start()
    {
        //Setup
        if (!equipped)
        {
            //armaScript.disnabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            //armaScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //Check if player is in the range and if E is pressed
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
            return;
        }

        //Vector3 distanceToPlayer0 = player0.position - transform.position;
        //Vector3 distanceToPlayer1 = player1.position - transform.position;
        //if (!equipped && distanceToPlayer0.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E))
        //{
        //    PickUp();
        //    return;
        //}
        //if (!equipped && distanceToPlayer1.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E))
        //{
        //    PickUp();
        //    return;
        //}

        //Drop if equipped and E is pressed
        if (equipped && Input.GetKeyDown(KeyCode.E))
        {
            Drop();
            return;
        }
    }

    private void PickUp()
    {
        equipped = true;

        //Make weapon a child of the camera and move it to default position
        transform.SetParent(weaponContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;


        //Make Rigidbody not kinematic and BoxCollider normal
        rb.isKinematic = true;
        coll.isTrigger = true;

        //Eneable script
        //armaScript.enable = true;
    }

    private void Drop()
    {
        equipped = false;

        //Set parent to null
        transform.SetParent(null);

        //Make Rigidbody not kinematic and BoxCollider normal
        rb.isKinematic = false;
        coll.isTrigger = false;

        //Add force
        rb.AddForce(cam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(cam.up * dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random)*10);
        //Diseable script
        //armaScript.enable = true;
    }
}