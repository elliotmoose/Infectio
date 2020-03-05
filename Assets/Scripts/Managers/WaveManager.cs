﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager GetInstance() 
    {
        GameObject gameManager = GameObject.Find("GameManager");
        if(gameManager == null) 
        {
            Debug.LogError("GameManager has not been instantiated yet");
            return null;
        }

        WaveManager waveManager = gameManager.GetComponent<WaveManager>();

        if(waveManager == null) 
        {
            Debug.LogError("GameManager has no component WaveManager");
            return null;
        }

        return waveManager;
    }

    // public GameObject enemyPrefab;
    // public GameObject rapidEnemyPrefab;

    [SerializeField]
    private int _waveLevel = 0;
    [SerializeField]
    bool _isDowntime = true;

    [SerializeField]
    private float _maxDowntime = 3; //time in between waves
    private float _curDowntime = 0;
    
    [SerializeField]
    private float _spawnDistance = 10; //distance from specified centre (player position)
    private float _spawnRate = 10f;//spawn 2 per second
    
    [SerializeField]
    private float _waveMaxHealth = 0; //total enemy max health
    [SerializeField]
    private float _waveCurHealth = 0; //total enemy current health
    
    private WaveData _currentWave;
    private float _timeSinceLastSpawn = 0; //staggering of spawn within wave


    // Start is called before the first frame update
    void Start()
    {
        StartSpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        if(_isDowntime) {
            UpdateDowntime();
        }
        else {
            SpawnIfNeeded();
        }
    }
    
    #region Wave Spawning
    private void StartSpawnWave()
    {
        Debug.Log("WaveManager: Wave Started");
        _waveLevel += 1;
        _isDowntime = false;
        _currentWave = new WaveData(this._waveLevel);   //new wave for current level     
        _waveMaxHealth = _currentWave.GetMaxHealth();
        _waveCurHealth = _waveMaxHealth;        
    }

    //checks if there are enemy groups in the queue left to spawn
    private void SpawnIfNeeded()
    {                
        _timeSinceLastSpawn += Time.deltaTime;

        if(_currentWave == null) 
        {
            return;
        }

        if(_currentWave.enemyGroups.Count == 0) 
        {
            
            _currentWave = null;
            return;
        }

        if(!_currentWave.IsEmpty()) 
        {
            if (_timeSinceLastSpawn >= 1 / _spawnRate)
            {
                _timeSinceLastSpawn = 0;
                EnemyGroupData nextSpawnGroupData = _currentWave.PopEnemyFromGroup();
                if(nextSpawnGroupData != null)
                {
                    SpawnEnemy(nextSpawnGroupData);
                }
            }
        }
    }

    //Enemy group data to specify the enemy stats
    void SpawnEnemy(EnemyGroupData enemyGroupData) 
    {
        float spawnHeight = 0;
        float spawnAngle = Random.Range(0, 359); //randomize the angle in which enemy is spawned
        Vector3 spawnReferenceCenter = GameObject.Find("Player").transform.position; 
        Quaternion spawnDirection = Quaternion.AngleAxis(spawnAngle, Vector3.up); 
        Vector3 spawnPosition = spawnReferenceCenter + spawnDirection * Vector3.forward.normalized * _spawnDistance;

        //Get a valid spawn position based on center + direction
        bool clockwise = true;
        int attempts = 0;
        while(!MapManager.IsInMap(spawnPosition))
        {
            spawnAngle += clockwise ? 90 : -90;
            spawnDirection = Quaternion.AngleAxis(spawnAngle, Vector3.up); 
            spawnPosition = spawnReferenceCenter + spawnDirection * Vector3.forward.normalized * _spawnDistance;
            attempts++;

            //after trying half a circle, go other direction instead so that enemies don't keep spawning on one side of the map
            if(attempts == 2) 
            {
                spawnAngle -= 180; 
                clockwise = false;
            }
        }        

        GameObject enemyPrefab = enemyGroupData.GetPrefab();
        GameObject enemyObj = GameObject.Instantiate(enemyPrefab, new Vector3(spawnPosition.x,spawnHeight,spawnPosition.z), Quaternion.identity);    
        enemyObj.GetComponent<Enemy>().LoadFromEnemyData(enemyGroupData); //load movement speed, max health etc        
    }
    #endregion
    
    public void OnEnemyTakeDamage(float damage) 
    {
        if(this._waveCurHealth > 0)
        {
            this._waveCurHealth -= damage;
        }
    }

    public void OnEnemyDied(Enemy enemy) 
    {
        CheckShouldStartDowntime();
    }

    public float GetWavePercentageHealth()
    {
        if(_waveCurHealth == 0)
        {
            return 0;
        }

        return _waveCurHealth/_waveMaxHealth;
    }

    #region Downtime
    //If all enemies are dead, we can start next round
    void CheckShouldStartDowntime() 
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        //all enemies dead, no more to spawn, begin downtime
        bool allEnemiesDead = true;
        foreach (GameObject enemyObj in enemies)
        {
            Enemy enemy = enemyObj.GetComponent<Enemy>();

            //as long as 1 enemy is alive, should not start downtime
            if(enemy.isAlive) 
            {
                allEnemiesDead = false;
                break;
            }
        }

        if(allEnemiesDead && !_isDowntime && _currentWave == null) 
        {
            StartDowntime();
        }
    }

    void StartDowntime() 
    {
        _isDowntime = true;
        _curDowntime = 0;        
        Debug.Log($"WaveManager: Downtime Started: {_maxDowntime}s");
    }
    
    void UpdateDowntime() 
    {
        if(_curDowntime < _maxDowntime) 
        {
            _curDowntime += Time.deltaTime;
        }
        else 
        {
            //begin next wave
            StartSpawnWave();
        }
    }
    #endregion
    #region Tests
    IEnumerator TestKillAllEnemies() 
    {
        yield return new WaitForSeconds(3);
        Debug.Log("WaveManager: Clearing enemies...");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        //all enemies dead, begin downtime
        foreach(GameObject enemy in enemies) 
        {
            GameObject.Destroy(enemy);
        }        
    }
    #endregion
}
