using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidManager : MonoBehaviour
{
    #region Singleton

    public static AsteroidManager Instance { get; private set; }
    
    private void Start()
    {
        if (!GameManager.Instance.IsHost)
        {
            Debug.Log("AsteroidManager is not host, destroying...");
            Destroy(gameObject);
            return;
        }
        if (Instance == null) 
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Debug.LogWarning("More than one instance of AsteroidManager found!");
            Destroy(gameObject);
        }
    }
    
    #endregion
    
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private GameObject[] specialAsteroidPrefabs;
    [Space]
    [SerializeField] private int asteroidCount;
    [SerializeField] private Transform[] asteroidSpawnPoints;
    [SerializeField] private float asteroidSpawnRadius;
    [Space]
    [SerializeField] private int specialAsteroidCount;
    [SerializeField] private Transform[] specialAsteroidSpawnPoints;
    [SerializeField] private int specialAsteroidSpawnRadius;

    [Space]
    [SerializeField] private int continuousAsteroidRate = 2;
    [SerializeField] private float continuousAsteroidTimeUnit = 30f;
    [SerializeField] private int maxAsteroidOnScreen = 10;
    
    [Space]
    [SerializeField] private int continuousSpecialAsteroidRate = 2;
    [SerializeField] private float continuousSpecialAsteroidTimeUnit = 30f;
    [SerializeField] private int maxSpecialAsteroidOnScreen = 10;


    private List<GameObject> _asteroids = new List<GameObject>();
    private List<GameObject> _specialAsteroids = new List<GameObject>();
    public List<GameObject> Asteroids => _asteroids;
    public List<GameObject> SpecialAsteroids => _specialAsteroids;
    
    private void Initialize()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            var position = asteroidSpawnPoints.Length > 0
                ? asteroidSpawnPoints[Random.Range(0, asteroidSpawnPoints.Length)].position
                : transform.position;
            position += Random.insideUnitSphere * asteroidSpawnRadius;
            position = new Vector3(position.x, 0, position.z);
            var asteroid = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)], position, 
                Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            _asteroids.Add(asteroid);
        }
        
        for (int i = 0; i < specialAsteroidCount; i++)
        {
            var position = specialAsteroidSpawnPoints.Length > 0
                ? specialAsteroidSpawnPoints[Random.Range(0, specialAsteroidSpawnPoints.Length)].position
                : transform.position;
            position += Random.insideUnitSphere * specialAsteroidSpawnRadius;
            position = new Vector3(position.x, 0, position.z);
            var asteroid = Instantiate(specialAsteroidPrefabs[Random.Range(0, specialAsteroidPrefabs.Length)], position, 
                Quaternion.Euler(0, Random.Range(0, 360), 0));
            asteroid.GetComponent<NetworkObject>().Spawn(); 

            _specialAsteroids.Add(asteroid);
        }
        
        StartCoroutine(ContinuousSpawningRoutine());
        StartCoroutine(ContinuousSpecialSpawningRoutine());
    }

    private void OnDrawGizmos()
    {
        if (asteroidSpawnPoints.Length > 0)
        {
            Gizmos.color = Color.yellow;
            foreach (var spawnPoints in asteroidSpawnPoints)
            {
                Gizmos.DrawWireSphere(spawnPoints.position, asteroidSpawnRadius + 0.1f);
            }
        }

        if (specialAsteroidSpawnPoints.Length > 0)
        {
            Gizmos.color = Color.magenta;
            foreach (var spawnPoints in specialAsteroidSpawnPoints)
            {
                Gizmos.DrawWireSphere(spawnPoints.position, specialAsteroidSpawnRadius + 0.1f);
            }
        }
    }

    private IEnumerator ContinuousSpawningRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(continuousAsteroidTimeUnit);
            for (int i = 0; i < continuousAsteroidRate; i++)
            {
                
                if (_asteroids.Count >= maxAsteroidOnScreen)
                {
                    continue;
                }
                
                var finalPosition = asteroidSpawnPoints.Length > 0
                    ? asteroidSpawnPoints[Random.Range(0, asteroidSpawnPoints.Length)].position
                    : transform.position;
                finalPosition += Random.insideUnitSphere * asteroidSpawnRadius;
                finalPosition = new Vector3(finalPosition.x, 0, finalPosition.z);

                // random if up or down
                var initialPositionY = Random.Range(1,100) % 2 == 0 ? 50 : -50;
                
                
                var initialPosition = new Vector3(finalPosition.x, finalPosition.y, initialPositionY);
                var asteroid = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)], initialPosition, 
                    Quaternion.LookRotation(finalPosition-initialPosition, Vector3.up));

                asteroid.GetComponent<Asteroid>().MoveTo(finalPosition);
                _asteroids.Add(asteroid);
            }
        }
    }
    
    
    
    
    private IEnumerator ContinuousSpecialSpawningRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(continuousSpecialAsteroidTimeUnit);
            for (int i = 0; i < continuousSpecialAsteroidRate; i++)
            {
                
                if (_specialAsteroids.Count >= maxSpecialAsteroidOnScreen)
                {
                    continue;
                }
                
                var finalPosition = specialAsteroidSpawnPoints.Length > 0
                    ? specialAsteroidSpawnPoints[Random.Range(0, specialAsteroidSpawnPoints.Length)].position
                    : transform.position;
                finalPosition += Random.insideUnitSphere * specialAsteroidSpawnRadius;
                finalPosition = new Vector3(finalPosition.x, 0, finalPosition.z);

                // random if up or down
                var initialPositionY = Random.Range(1,100) % 2 == 0 ? 50 : -50;
                
                
                var initialPosition = new Vector3(finalPosition.x, finalPosition.y, initialPositionY);
                var specialAsteroid = Instantiate(specialAsteroidPrefabs[Random.Range(0, specialAsteroidPrefabs.Length)], initialPosition, 
                    Quaternion.LookRotation(finalPosition-initialPosition, Vector3.up));
                specialAsteroid.GetComponent<NetworkObject>().Spawn(); 

                specialAsteroid.GetComponent<Asteroid>().MoveTo(finalPosition);
                _specialAsteroids.Add(specialAsteroid);
            }
        }
    }

    
}
