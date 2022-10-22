using System.Collections;
using UnityEngine;
using Wanpis.TBH.Enemy;

public class Turret : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _defaultBulletPoint;
    private float _distanceToTarget = 1000f;
    private float _angleToTarget;
    private Transform _target;
    private float _rotateZ;

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private void Update()
    {
        for (int i = 0; i < _enemySpawner.Enemies.Count; i++)
        {
            Transform target = _enemySpawner.Enemies[i].transform;

            if (!target.gameObject.activeInHierarchy)
            {
                _enemySpawner.Enemies.RemoveAt(i);
                continue;
            }

            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= _distanceToTarget || !_target.gameObject.activeInHierarchy)
            {
                _target = target;
            }
        }

        if (_enemySpawner.Enemies.Count == 0)
        {
            _target = _defaultBulletPoint;
        }

        _distanceToTarget = Vector3.Distance(transform.position, _target.position);

        Vector3 difference = _target.position - transform.position;
        difference.Normalize();
        _rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, _rotateZ);
    }

    private IEnumerator Shoot()
    {
        Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.Euler(0f, 0f, _rotateZ + 90f));
        yield return new WaitForSeconds(1);
        StartCoroutine(Shoot());
    }
}
