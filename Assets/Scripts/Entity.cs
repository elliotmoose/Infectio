using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour 
{
    protected float _maxHealth = 400;
	protected float _curHealth = 400;
    protected float _movementSpeed = 3;

    protected GameObject _equippedWeapon;
    protected List<EntityEffect> _entityEffect = new List<EntityEffect>();
    public GameObject EquipWeapon(WeaponData weaponData) {
        //0. Clean up previously equipped weapon
        if(this._equippedWeapon != null) 
        {
            GameObject.Destroy(_equippedWeapon);
        }

        //1. Instantiate weapon prefab (based on weaponId) and attach as child
        GameObject weaponSlot = GetWeaponSlot();

        if(weaponSlot == null) 
        {
            Debug.LogWarning("EquipWeapon: This entity has no weapon slot");
            return null;
        }
        
        Object weaponPrefab = Resources.Load($"Prefabs/Weapons/{weaponData.type.ToString()}");
        GameObject newWeaponObject = (GameObject)GameObject.Instantiate(weaponPrefab, weaponSlot.transform.position, weaponSlot.transform.rotation, weaponSlot.transform);
        //2. Update weapon's weaponData
        Weapon weaponComponent = newWeaponObject.GetComponent<Weapon>();
        weaponComponent.Activate(weaponData, this.gameObject);
        //3. Assign game object to equipped weapon
        Debug.Log($"{weaponData.type} equipped");
        this._equippedWeapon = newWeaponObject;
        return newWeaponObject;
        
    }

	// public List<Effect> effects; //immunity, or slow effects
	
    #region Health
    //getter methods 
    public float GetMaxHealth()
    {
        return _maxHealth;
    }
    public float GetCurHealth()
    {
        return _curHealth;
    }

    //setter methods 
	public void SetMaxHealth(float health){
        //curhealth % will remain
        _curHealth = (_curHealth/_maxHealth)*health;
        _maxHealth = health;
    }

    public void Heal(float heal)
    {
        _curHealth += heal;
    }

    public void TakeDamage(float damage)
    {
        _curHealth -= damage; //input how much to decrease by-- POSITIVE VALUE        
        OnTakeDamage(damage);
        if(_curHealth <= 0) {
            this.Die();
        }
    }	

    protected virtual void OnTakeDamage(float damage){}

    public abstract void Die();

    #endregion

    public void SetMovementSpeed(float speed) 
    {
        this._movementSpeed = speed;
    }

    public float GetMovementSpeed() 
    {
        return this._movementSpeed;
    }

    #region Weapon
    
    public GameObject GetWeaponSlot() 
    {
        return this.transform.Find("WeaponSlot").gameObject;
    }

    public GameObject GetEquippedWeaponGameObject() 
    {
        for (int i = 0; i < this.transform.Find("WeaponSlot").transform.childCount; i++)
        {
            if(this.transform.Find("WeaponSlot").transform.GetChild(i).gameObject.activeSelf == true)
            {
                return this.transform.Find("WeaponSlot").transform.GetChild(i).gameObject;
            }
        }
        return null;
    }


    public Weapon GetEquippedWeaponComponent() {
        GameObject weaponGameObject = GetEquippedWeaponGameObject();
        if(weaponGameObject == null)
        {
            Debug.LogWarning("GetWeaponComponent: No weapon gameobject on this entity");
            return null;
        }

        return weaponGameObject.GetComponent<Weapon>();        
    }

    public WeaponData GetEquippedWeaponData() 
    {
        Weapon weaponComponent = GetEquippedWeaponComponent();
        if(weaponComponent == null)
        {
            Debug.LogWarning("GetWeaponData: No weapon component on this entity");
            return null;
        }

        return weaponComponent.GetWeaponData();
    }
    
    #endregion
    

    public void ApplyEffect(EntityEffect entityEffect){
        // entityEffect.SetEntity(this.gameObject);
        entityEffect.Update();
        if(entityEffect.active == false){
            _entityEffect.Remove(entityEffect);
            Debug.Log("entityEffect deleted");
        }
    }
	
    // public event Action <float> onHealthChanged = delegate{};
}