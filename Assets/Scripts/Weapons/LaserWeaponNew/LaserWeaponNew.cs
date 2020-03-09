using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeaponNew : Weapon
{
    public GameObject laser;
    private GameObject laserObj;
    private LaserNew _laserScript;
    public Transform laserSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        laserObj = GameObject.Instantiate(laser, laserSpawnPoint.transform.position, laserSpawnPoint.transform.rotation);
        _laserScript = laser.GetComponent<LaserNew>();
        _laserScript.Activate(this._weaponData, this._owner);
    }

    // Update is called once per frame
    void Update()
    {
        laserObj.transform.position = this.transform.position;
        laserObj.transform.rotation = this.transform.rotation;
    }

    protected override void Fire()
    {
        //throw new System.NotImplementedException();
    }
}
