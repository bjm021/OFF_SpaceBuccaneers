using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    
    [SerializeField] private int minResources;
    [SerializeField] private int maxResources;
    public bool Dead { get; private set; }
    
    private int _currentResources;
    private void Start()
    {
        Dead = false;
        _currentResources = Random.Range(minResources, maxResources);
    }
    
    public int Mine(int damage)
    {
        _currentResources -= damage;
        if (_currentResources <= 0)
        {
            Dead = true;
            Die();
        }
        return _currentResources;
    }
    
    private void Die()
    {
        AsteroidManager.Instance.Asteroids.Remove(gameObject);
        Destroy(gameObject);
    }
    
}
