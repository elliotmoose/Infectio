using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : Weapon
{

    public GameObject laser; // get reference by dragging
    //public Transform laserSpawnPoint; // get reference by dragging
    //public Transform endpoint;
    private Laser _laserScript;

    protected override void Fire()
    {

        // if it collides with something
       // _laserScript.Attack();
    }

    // Start is called before the first frame update
    void Start()
    {
        //GameObject laserObj = GameObject.Instantiate(laser, laserSpawnPoint.transform.position, laserSpawnPoint.transform.rotation);
        _laserScript = laser.GetComponent<Laser>();
        _laserScript.Activate(this._weaponData, this._owner);
    }


    // create cylinder as laser with fixed length
    // at every update check if cylinder hits enemies - Laser
    // if yes cylinder length becomes spawnpoint to enemy location
    // if no then cylinder length  is still the same

    // if cylinder hits enemy, enemy takes damage
    // if the same enemy is hit over and over again the damage is multiplied
    // else the multiplier resets to 0
}
