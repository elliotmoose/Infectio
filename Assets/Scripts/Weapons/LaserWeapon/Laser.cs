using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    protected WeaponData _weaponData;
    protected GameObject _owner;

    public Transform laserSpawnPoint; // get reference by dragging
    public Transform laserEndPoint; // get reference by dragging

    public void Start()
    {
    }

    public void Activate(WeaponData weaponData, GameObject owner)
    {
        this._weaponData = weaponData;
        this._owner = owner;
    }

    void OnTriggerStay(Collider col)
    {
        Attack(col);
    }
    private void OnTriggerExit(Collider other)
    {
        // return lightsaber to original position
        this.transform.localScale = new Vector3(0.1f, 3f, 0.1f);
        Vector3 oldPos = this.transform.localPosition;
        Vector3 newPos = this.transform.localPosition;
        newPos.z = (laserEndPoint.localPosition.z - laserSpawnPoint.localPosition.z) / 2;
        this.transform.localPosition = newPos;
    }

    public void Attack(Collider col)
    {
        /*
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
                Vector3 newPos = this.transform.localPosition;
                Vector3 oldLength = this.transform.localScale;
                Vector3 newLength = entity.transform.position - laserSpawnPoint.transform.position;

                this.transform.localScale = new Vector3(0.1f, Mathf.Sqrt(Mathf.Pow(newLength.x, 2) + Mathf.Pow(newLength.z, 2)), 0.1f);


                newPos.z = Mathf.Sqrt(Mathf.Pow(newLength.x, 2) + Mathf.Pow(newLength.z, 2)) / oldLength.y;
                this.transform.localPosition = newPos;
                entity.TakeDamage(this._weaponData.damage);
            }
            if (entity == null)
            {
                // return lightsaber to original position
                this.transform.localScale = new Vector3(0.1f, 3f, 0.1f);
                Vector3 oldPos = this.transform.localPosition;
                Vector3 newPos = this.transform.localPosition;
                newPos.z = (laserEndPoint.localPosition.z - laserSpawnPoint.localPosition.z) / 2;
                this.transform.localPosition = newPos;
            }
            
        }
        //this.transform.localScale = new Vector3(0.1f, 3f, 0.1f);
        //this.transform.position = (laserEndPoint.transform.position - laserSpawnPoint.transform.position) / 2;*/
    }

    // laser check if it hits enemies
    // if yes then decrease laser length
    // decrease enemy health
    // if no then keep original length
}
