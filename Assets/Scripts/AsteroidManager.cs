using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private int asteroidCount;
    [SerializeField] private float asteroidSpawnRadius;
    
    private List<GameObject> asteroids = new List<GameObject>();
    public List<GameObject> Asteroids => asteroids;
    
    private void Start()
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
