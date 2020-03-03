using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : Weapon
{

    public GameObject laser;
    public Transform laserSpawnPoint;

    protected override void Fire()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject laserObj = GameObject.Instantiate(laser, laserSpawnPoint.transform.position, laserSpawnPoint.transform.rotation);
    }


    // create cylinder as laser with fixed length
    // at every update check if cylinder hits enemies
    // if yes cylinder length becomes spawnpoint to enemy location
    // if no then cylinder length  is still the same

    // if cylinder hits enemy, enemy takes damage
    // if the same enemy is hit over and over again the damage is multiplied
    // else the multiplier resets to 0
}
