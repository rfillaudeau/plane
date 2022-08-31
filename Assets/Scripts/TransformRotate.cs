using UnityEngine;

public class TransformRotate : MonoBehaviour
{
    [SerializeField] private Vector3 _speed;

    private void Update()
    {
        transform.Rotate(_speed * Time.deltaTime);
    }
}
