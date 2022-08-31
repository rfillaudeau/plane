using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Damageable))]
public class Player : MonoBehaviour
{
    public event Action onExploded;

    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _turnSpeed = 10f;

    private Vector2 _touchStartPosition;
    private Vector2 _touchCurrentPosition;
    [SerializeField] private float _touchThreshold = 1f;

    [SerializeField] private Transform _directionIndicator;
    [SerializeField] private float _directionIndicatorRadius = 1f;

    [SerializeField] private Transform _plane;
    [SerializeField] private float _rotationSpeed = 10f;

    [SerializeField] private ParticleSystem _explosionParticle;

    private Rigidbody _rigidbody;
    private Damageable _damageable;

    private Vector3 _targetDirection = Vector3.up;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _damageable = GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        _damageable.onDie += Explode;
    }

    private void OnDisable()
    {
        _damageable.onDie -= Explode;
    }

    private void Update()
    {
        HandleDirection();
        HandleDirectionIndicator();
        HandlePlaneRotation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_damageable.isDead)
        {
            return;
        }

        _rigidbody.AddForce(transform.up * _speed);
    }

    private void HandleDirection()
    {
        if (_damageable.isDead)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _touchStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            _touchCurrentPosition = Input.mousePosition;
        }

        if (Vector2.Distance(_touchStartPosition, _touchCurrentPosition) >= _touchThreshold)
        {
            _targetDirection = (_touchCurrentPosition - _touchStartPosition).normalized;

            transform.up = Vector3.Lerp(transform.up, _targetDirection, _turnSpeed * Time.deltaTime);
        }
    }

    private void HandleDirectionIndicator()
    {
        _directionIndicator.position = transform.position + _targetDirection * _directionIndicatorRadius;
        _directionIndicator.up = _targetDirection;
    }

    private void HandlePlaneRotation()
    {
        float rotationAngle = Vector3.SignedAngle(transform.up, _targetDirection, Vector3.forward);

        _plane.localRotation = Quaternion.RotateTowards(
            _plane.localRotation,
            Quaternion.Euler(0f, Mathf.Round(rotationAngle), 0f),
            _rotationSpeed * Time.deltaTime
        );
    }

    private void Explode()
    {
        _plane.gameObject.SetActive(false);
        _directionIndicator.gameObject.SetActive(false);

        _explosionParticle.Play();

        StartCoroutine(DisableCountdown());
    }

    private IEnumerator DisableCountdown()
    {
        yield return new WaitForSeconds(_explosionParticle.main.duration);

        onExploded?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _targetDirection * _directionIndicatorRadius);
    }
}
