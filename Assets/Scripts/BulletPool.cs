using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;
    [SerializeField] private int _poolSize;
    [SerializeField] private GameObject _bulletPrefab;
    public List<Bullet> Bullets = new List<Bullet>();
    private int _bulletPointer = 0;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject bulletObject = Instantiate(_bulletPrefab, transform);
            bulletObject.SetActive(false);
            Bullets.Add(bulletObject.GetComponent<Bullet>());
        }
    }

    public void Shoot(Vector3 position, Quaternion rotation)
    {
        if (_bulletPointer == _poolSize)
        {
            _bulletPointer = 0;
        }

        Bullet bullet = Bullets[_bulletPointer];
        bullet.InUse = true;
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.gameObject.SetActive(true);
        _bulletPointer++;
    }
}