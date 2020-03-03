using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    protected WeaponData _weaponData;
    protected GameObject _owner;

    protected Vector3 _origin;

    // Update is called once per frame
    void Update()
    {
        CheckActivated();
    }

    public void Activate(WeaponData weaponData, GameObject owner)
    {
        this._weaponData = weaponData;
        this._owner = owner;
    }

    public void SetOrigin(Vector3 origin)
    {
        this._origin = origin;
    }

    protected void CheckActivated()
    {
        if (this._weaponData == null || this._owner == null)
        {
            Debug.LogWarning("Please call Activate() on instantiation of this projectile");
        }
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
                entity.TakeDamage(this._weaponData.damage);
            }
        }
    }

    // laser check if it hits enemies
    // if yes then decrease laser length
    // decrease enemy health
    // if no then keep original length
}
