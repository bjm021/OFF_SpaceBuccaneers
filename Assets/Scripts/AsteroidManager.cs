using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    #region Singleton

    public static AsteroidManager Instance { get; private set; }
    
    private void Awake()
    {
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
    
    [SerializeField] private GameObject asteroidPrefab;
    [Space]
    [SerializeField] private int asteroidCount;
    [SerializeField] private Transform[] asteroidSpawnPoints;
    [SerializeField] private float asteroidSpawnRadius;
    [Space]
    [SerializeField] private int specialAsteroidCount;
    [SerializeField] private Transform[] specialAsteroidSpawnPoints;
    [SerializeField] private int specialAsteroidSpawnRadius;
    

    private List<GameObject> asteroids = new List<GameObject>();
    public List<GameObject> Asteroids => asteroids;
    
    private void Initialize()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            var asteroid = Instantiate(asteroidPrefab, asteroidSpawnPoints[Random.Range(0, asteroidSpawnPoints.Length)].position, Quaternion.identity);

            asteroid.transform.position = new Vector3(asteroid.transform.position.x, 0, asteroid.transform.position.z);
            asteroids.Add(asteroid);
        }
        
        for (int i = 0; i < specialAsteroidCount; i++)
        {
            var asteroid = Instantiate(asteroidPrefab, specialAsteroidSpawnPoints[Random.Range(0, specialAsteroidSpawnPoints.Length)].position, Quaternion.identity);

            asteroid.transform.position = new Vector3(asteroid.transform.position.x, 0, asteroid.transform.position.z);
            asteroids.Add(asteroid);
        }
    }
}
