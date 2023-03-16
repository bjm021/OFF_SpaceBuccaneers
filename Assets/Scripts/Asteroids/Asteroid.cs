using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    
    [SerializeField] private int minResources;
    [SerializeField] private int maxResources;
    
    [SerializeField] private UnityEvent onHalfResources;
    public bool Dead { get; private set; }
    
    private int _startResources;
    private int _currentResources;
    
    private void Start()
    {
        Dead = false;
        _currentResources = Random.Range(minResources, maxResources);
        _startResources = _currentResources;
    }
    
    public int Mine(int damage)
    {
        int previousResources = _currentResources;
        _currentResources -= damage;

        if (previousResources > _startResources / 2 && _currentResources <= _startResources / 2)
        {
            onHalfResources.Invoke();
        }
        
        if (_currentResources <= 0)
        {
            Dead = true;
            Die();
        }

        return _currentResources;
    }

    private Coroutine _moveRoutine;
    
    private void Die()
    { 
        if (this == null) return;
        if (_moveRoutine != null) StopCoroutine(_moveRoutine);
        AsteroidManager.Instance.Asteroids.Remove(gameObject);
        Destroy(gameObject);
    }
    
    private IEnumerator AsteroidMoveCoroutine(Vector3 finalPosition)
    {
        var initialPosition = gameObject.transform.position;

        while (Vector3.Distance(gameObject.transform.position, finalPosition) > 0.1f)
        {
            gameObject.transform.position += (finalPosition - transform.position)/10*Time.deltaTime;
            yield return null;
        }
        
   
    }
    
    public void MoveTo(Vector3 finalPosition)
    {
        _moveRoutine = StartCoroutine(AsteroidMoveCoroutine(finalPosition));
    }
    
}
