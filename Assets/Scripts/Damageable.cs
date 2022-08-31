using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public event Action onTakeDamage;
    public event Action onHealthUpdated;
    public event Action onDie;

    public int health { get { return _health; } }
    public int maxHealth { get { return _maxHealth; } }
    public bool isDead { get { return _health == 0; } }

    [SerializeField] private int _maxHealth = 1;
    [SerializeField] private int _health;

    private void Start()
    {
        _health = _maxHealth;

        onHealthUpdated?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health < 0)
        {
            _health = 0;
        }

        onTakeDamage?.Invoke();
        onHealthUpdated?.Invoke();

        if (_health == 0)
        {
            onDie?.Invoke();
        }
    }
}
