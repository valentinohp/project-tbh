using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
    public static EnemyBulletPool Instance;
    [SerializeField] private int _poolSize;
    [SerializeField] private GameObject _bulletPrefab;
    public List<Bullet> Bullets = new List<Bullet>();
    private int _bulletPointer = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
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

    public void Clear()
    {
        for (int i = 0; i < Bullets.Count; i++)
        {
            Bullets[i].gameObject.SetActive(false);
        }
    }
}