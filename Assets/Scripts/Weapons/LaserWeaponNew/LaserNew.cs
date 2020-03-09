using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserNew : MonoBehaviour
{
    protected WeaponData _weaponData;
    protected GameObject _owner;

    //public Transform laserSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(WeaponData weaponData, GameObject owner)
    {
        this._weaponData = weaponData;
        this._owner = owner;
    }
}
