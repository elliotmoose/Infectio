using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    protected WeaponData _weaponData;
    protected GameObject _owner;

    public Transform laserSpawnPoint; // get reference by dragging
    public Transform laserEndPoint; // get reference by dragging

    public void Activate(WeaponData weaponData, GameObject owner)
    {
        this._weaponData = weaponData;
        this._owner = owner;
    }

    void OnTriggerEnter(Collider col)
    {
        //means parent died already
        if (_owner == null)
        {
            return;
        }

        //attack ENTITIES of different TAG 
        if (col.gameObject.tag != _owner.tag)
        {
            Entity entity = col.gameObject.GetComponent<Entity>();
            if (entity != null)
            {
                
                // change cylider length
                Vector3 cylinder = this.transform.position;
                Vector3 newLength = entity.transform.position - laserSpawnPoint.transform.position;
                //float multiply = newLength.y / oldLength.y;

                this.transform.localScale = new Vector3(0.1f, newLength.y, 0.1f);

                //this.transform.position = (laserEndPoint.transform.position - laserSpawnPoint.transform.position) / 2;
                entity.TakeDamage(this._weaponData.damage);
            }
        }
        this.transform.localScale = new Vector3(0.1f, 3f, 0.1f);
        //this.transform.position = (laserEndPoint.transform.position - laserSpawnPoint.transform.position) / 2;
        // FIX ISSUE REGARDING LASER SPAWN POINT
    }

    // laser check if it hits enemies
    // if yes then decrease laser length
    // decrease enemy health
    // if no then keep original length
}
