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
    [SerializeField] private int asteroidCount;
    [SerializeField] private float asteroidSpawnRadius;
    
    private List<GameObject> asteroids = new List<GameObject>();
    public List<GameObject> Asteroids => asteroids;
    
    private void Initialize()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            var asteroid = Instantiate(asteroidPrefab, transform);
            asteroid.transform.position = Random.insideUnitSphere * asteroidSpawnRadius;
            asteroid.transform.position = new Vector3(asteroid.transform.position.x, 0, asteroid.transform.position.z);
            asteroids.Add(asteroid);
        }
    }
}
