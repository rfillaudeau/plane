using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Missile : MonoBehaviour
{
    public static event Action<int> onExplodeWithPoints;

    public Transform target;

    [SerializeField] private ParticleSystem _explosionParticle;
    [SerializeField] private GameObject _model;
    [SerializeField] private GameObject _trail;

    [SerializeField] private int _strength = 1;
    [SerializeField] private float _explodeAfter = 1f;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _turnSpeed = 10f;
    [SerializeField] private int _points = 1;

    private Rigidbody _rigidbody;
    private Collider _collider;

    private bool _isExploding = false;

    public void Explode()
    {
        if (!gameObject.activeInHierarchy || _isExploding)
        {
            return;
        }

        onExplodeWithPoints?.Invoke(_points);

        _isExploding = true;
        _collider.enabled = false;
        _model.SetActive(false);
        _trail.SetActive(false);

        _explosionParticle.Play();

        StartCoroutine(DisableCountdown());
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        _isExploding = false;
        _collider.enabled = true;
        _model.SetActive(true);
        _trail.SetActive(true);

        StartCoroutine(ExplodeCountdown());
    }

    private void Update()
    {
        if (target == null || _isExploding)
        {
            return;
        }

        Vector3 targetDirection = target.position - transform.position;
        transform.up = Vector3.Lerp(transform.up, targetDirection, _turnSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (target == null || _isExploding)
        {
            return;
        }

        _rigidbody.AddForce(transform.up * _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_strength);
        }

        Explode();
    }

    private IEnumerator ExplodeCountdown()
    {
        yield return new WaitForSeconds(_explodeAfter);

        Explode();
    }

    private IEnumerator DisableCountdown()
    {
        yield return new WaitForSeconds(_explosionParticle.main.duration);

        gameObject.SetActive(false);
    }
}
