﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : Weapon
{

    public GameObject laser; // get reference by dragging
    public Transform laserSpawnPoint; // get reference by dragging

    protected override void Fire()
    {
        // do nothing tbh :/
    }

    // Start is called before the first frame update
    void Start()
    {
        //GameObject laserObj = GameObject.Instantiate(laser, laserSpawnPoint.transform.position, laserSpawnPoint.transform.rotation);
        Laser laserScript = laser.GetComponent<Laser>();
        laserScript.Activate(this._weaponData, this._owner);
        laserScript.SetOrigin(laserSpawnPoint.transform.position);
    }


    // create cylinder as laser with fixed length
    // at every update check if cylinder hits enemies - Laser
    // if yes cylinder length becomes spawnpoint to enemy location
    // if no then cylinder length  is still the same

    // if cylinder hits enemy, enemy takes damage
    // if the same enemy is hit over and over again the damage is multiplied
    // else the multiplier resets to 0
}