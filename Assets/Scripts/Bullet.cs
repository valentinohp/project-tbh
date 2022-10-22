using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void Update()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
    }
}
