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

    void OnTriggerStay(Collider col)
    {
        Attack(col);
    }

    public void Attack(Collider col)
    {
        //means parent died already
        if (_owner == null)
        {
            return; // game over screen
        }

        //attack ENTITIES of different TAG 
        if (col.gameObject.tag != _owner.tag)
        {
            Entity entity = col.gameObject.GetComponent<Entity>();
            // check if entity is enemy or not
            if (entity != null && entity.tag == "enemy")
            {
                // change cylider length
                Vector3 oldPos = this.transform.position;
                Vector3 newPos;
                Vector3 newLength = entity.transform.position - laserSpawnPoint.transform.position;

                this.transform.localScale = new Vector3(0.1f, Mathf.Sqrt(Mathf.Pow(newLength.x, 2) + Mathf.Pow(newLength.z, 2)), 0.1f);


                newPos.y = oldPos.y * (Mathf.Sqrt(Mathf.Pow(newLength.x, 2) + Mathf.Pow(newLength.z, 2)) / 3f);
                entity.TakeDamage(this._weaponData.damage);
            }
        }
        //this.transform.localScale = new Vector3(0.1f, 3f, 0.1f);
        //this.transform.position = (laserEndPoint.transform.position - laserSpawnPoint.transform.position) / 2;
    }

    // laser check if it hits enemies
    // if yes then decrease laser length
    // decrease enemy health
    // if no then keep original length
}
