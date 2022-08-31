using System.Collections;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    [SerializeField] private Damageable _target;
    [SerializeField] private Missile[] _missilePrefabs;
    [SerializeField] private int _poolSize = 1;
    [SerializeField] private float _spawnRate = 3f;
    private float _spawnRange;

    private Missile[] _pool;

    private bool _canSpawn = true;

    private void OnEnable()
    {
        _target.onDie += ExplodeMissiles;
    }

    private void OnDisable()
    {
        _target.onDie -= ExplodeMissiles;
    }

    private void Start()
    {
        _spawnRange = Vector3.Distance(Camera.main.ScreenToWorldPoint(Vector3.zero), _target.transform.position) + 1f;
        _canSpawn = true;

        _pool = new Missile[_poolSize];

        for (int i = 0; i < _poolSize; i++)
        {
            Missile missile = Instantiate(
                _missilePrefabs[Random.Range(0, _missilePrefabs.Length)],
                Vector3.zero,
                Quaternion.identity
            );

            missile.gameObject.SetActive(false);

            _pool[i] = missile;
        }

        StartCoroutine(SpawnMissileLoop());
    }

    private Missile GetAvailableObjectFromPool()
    {
        foreach (Missile missile in _pool)
        {
            if (missile.gameObject.activeInHierarchy)
            {
                continue;
            }

            return missile;
        }

        return null;
    }

    private void SpawnMissile()
    {
        Missile missile = GetAvailableObjectFromPool();
        if (missile == null)
        {
            return;
        }

        missile.transform.position = _target.transform.position + _target.transform.up * -_spawnRange;
        missile.target = _target.transform;
        missile.gameObject.SetActive(true);
    }

    private IEnumerator SpawnMissileLoop()
    {
        while (_canSpawn)
        {
            SpawnMissile();

            yield return new WaitForSeconds(_spawnRate);
        }
    }

    private void ExplodeMissiles()
    {
        _canSpawn = false;

        foreach (Missile missile in _pool)
        {
            if (!missile.gameObject.activeInHierarchy)
            {
                continue;
            }

            missile.Explode();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(_target.transform.position, _target.transform.position + _target.transform.up * -_spawnRange);
    }
}
