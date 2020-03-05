﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    // Start is called before the first frame update
    public bool isAlive = true;
    public float dnaWorth = 20f; //worth in dna
    public float scoreWorth = 20f; //worth in score

    public GameObject dnaPrefab;

    private NavMeshAgent _navMeshAgent;
    private NavMeshObstacle _navMeshObstacle;

    private GameObject _target;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
    }
        
    public void LoadFromEnemyData(EnemyGroupData enemyGroupData) 
    {
        this.SetMovementSpeed(enemyGroupData.movementSpeed);
        this.SetMaxHealth(enemyGroupData.health);

        WeaponData weaponData = WeaponData.NewWeaponDataForType(enemyGroupData.weaponType);
        //TODO: apply damage increment here                        
        this.EquipWeapon(weaponData); //attach weapon
    }

    // Update is called once per frame
    void Update()
    {
        if(this._target == null)
        {
            this._target = GameObject.Find("Player");
        }
        
        UpdateEffects();
        
        Weapon weaponComponent = GetEquippedWeaponComponent();
        float weaponRange = weaponComponent.GetWeaponRange();

        RotateToTarget();
        if (Vector3.Distance(_target.transform.position, this.transform.position) < weaponRange)
        {
            Attack();
        }
        else 
        {
            Chase();
        }
    }

    void SetTarget(GameObject target)
    {
        this._target = target;
    }

    void Chase() 
    {                        
        if(_navMeshAgent.enabled) {
            _navMeshAgent.speed = this._movementSpeed;        
            _navMeshAgent.destination = _target.transform.position;
        }
        else {
            StartCoroutine(SetNavMeshAgentEnabled(true));        
        }
    }

    void Attack(){
        StartCoroutine(SetNavMeshAgentEnabled(false));                
        GetEquippedWeaponComponent().AttemptFire();
    }

    void RotateToTarget() 
    {        
        this.transform.rotation = Quaternion.LookRotation(_target.transform.position - this.transform.position);
    }

    protected override void OnTakeDamage(float damage)
    {
        //if it overshot, compensate        
        WaveManager.GetInstance().OnEnemyTakeDamage(damage + Mathf.Min(_curHealth, 0));
    }

    public override void Die() 
    {
        isAlive = false;
        DropDna();
        ScoreManager.GetInstance().OnEnemyDied(this);
        WaveManager.GetInstance().OnEnemyDied(this);
        GameObject.Destroy(gameObject);
    }

    void DropDna() 
    {
        GameObject.Instantiate(dnaPrefab, this.transform.position, Quaternion.identity);
    }

    //this has to be staggered so that the enemy won't teleport when the agent is reactivated
    IEnumerator SetNavMeshAgentEnabled(bool enabled) {
        if(_navMeshAgent.enabled != enabled) 
        {
            _navMeshObstacle.enabled = false;
            _navMeshAgent.enabled = false;            
            _navMeshObstacle.enabled = !enabled;
            yield return new WaitForSeconds(0.01f);
            _navMeshAgent.enabled = enabled;
        }
    }
}
