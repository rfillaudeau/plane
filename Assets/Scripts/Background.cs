using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private float _parallax = 1f;

    private Vector3 _startPosition;
    private Material _material;

    private void Start()
    {
        _startPosition = transform.position;
        _material = GetComponent<MeshRenderer>().material;
    }

    private void FixedUpdate()
    {
        Vector2 offset = _material.mainTextureOffset;

        offset.x = transform.position.x / transform.localScale.x;
        offset.y = transform.position.y / transform.localScale.y;

        _material.mainTextureOffset = offset / _parallax;
    }
}
