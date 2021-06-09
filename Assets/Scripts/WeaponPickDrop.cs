using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickDrop : MonoBehaviour
{
    //[SerializeField] AQUIVALASCRIPTDELARMA armaScript;
    [SerializeField] Rigidbody rb;
    [SerializeField] BoxCollider coll;
<<<<<<< HEAD
    [SerializeField] Transform player;
    [SerializeField] Transform weaponContainer, cam;
=======
    [SerializeField] Transform player, weaponContainer, cam;
>>>>>>> e2a481b922b7c619a93788a676c5460aef63dd0d

    [SerializeField] float pickUpRange;
    [SerializeField] float dropForwardForce, dropUpwardForce;

    [SerializeField] bool equipped;
    [SerializeField] static bool slotFull;

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
            slotFull = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //Check if player is in the range and if E is pressed
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull)
        {
            PickUp();
            return;
        }
<<<<<<< HEAD
        //Drop if equipped and E is pressed
=======
        //Drop if equipped and Q is predded
>>>>>>> e2a481b922b7c619a93788a676c5460aef63dd0d
        if (equipped && Input.GetKeyDown(KeyCode.E))
        {
            Drop();
            return;
        }
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

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
<<<<<<< HEAD

=======
>>>>>>> e2a481b922b7c619a93788a676c5460aef63dd0d
        equipped = false;
        slotFull = false;

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