using System.Collections;
using UnityEngine;

namespace Wanpis.TBH.Enemy
{
    public class Blimp : MonoBehaviour
    {
        [SerializeField] private int _health = 3;
        [SerializeField] private Vector3[] _corners;
        [SerializeField] private float _moveSpeed;
        private Vector3 _enterPoint;
        private Vector3 _target;
        private Vector3 _exitPoint;
        [SerializeField] private int _randomSpawn = -1;
        private int _currentState = 1;
        private Queue _positionQueue = new Queue();
        private bool _targetSet = false;
        private bool _canMove = true;
        private bool _exit = false;
        [SerializeField] private Transform _bulletSpawnPoint;
        private Transform _targetShoot;
        private float _rotateZ;
        [SerializeField] private float _shootInterval = 3f;

        private void Start()
        {
            if (_randomSpawn == -1)
            {
                _randomSpawn = Random.Range(0, _corners.Length);
            }

            _enterPoint = _corners[_randomSpawn];

            if (_enterPoint.y < 0)
            {
                transform.position = _exitPoint = _enterPoint + new Vector3(0, -1.5f, 0);
            }
            else
            {
                transform.position = _exitPoint = _enterPoint + new Vector3(0, 1.5f, 0);
            }

            for (int i = 0; i < _corners.Length; i++)
            {
                if (_randomSpawn + 1 == _corners.Length)
                {
                    _positionQueue.Enqueue(_corners[0]);
                    _randomSpawn = 0;
                }
                else
                {
                    _positionQueue.Enqueue(_corners[++_randomSpawn]);
                }
            }

            _targetShoot = TrainController.Position;
            StartCoroutine(Shoot());
        }

        private void Update()
        {
            if (_currentState == 1 && !_targetSet)
            {
                Spawn();
            }
            else if (_currentState >= 2 && _currentState <= 5 && !_targetSet)
            {
                Cycle();
            }
            else if (_currentState == 6 && !_targetSet)
            {
                Exit();
            }

            if (_canMove)
            {
                float move = _moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _target, move);
            }

            if (transform.position == _target && _targetSet)
            {
                _currentState++;
                _targetSet = false;
                _canMove = false;
                StartCoroutine(Wait());
                if (_exit)
                {
                    gameObject.SetActive(false);
                }
            }

            if (_targetShoot == null)
            {
                _targetShoot = TrainController.Position;
            }

            Vector3 difference = _targetShoot.position - transform.position;
            difference.Normalize();
            _rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }

        private void Spawn()
        {
            _target = _enterPoint;
            _targetSet = true;
        }

        private void Cycle()
        {
            _target = (Vector3)_positionQueue.Dequeue();
            _targetSet = true;
        }

        private void Exit()
        {
            _target = _exitPoint;
            _targetSet = true;
            _exit = true;
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(2);
            _canMove = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Bullet" || other.tag == "Player")
            {
                _health--;
                if (other.tag == "Bullet")
                {
                    other.gameObject.SetActive(false);
                }

                if (_health <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private IEnumerator Shoot()
        {
            if (_targetShoot == null)
            {
                _targetShoot = TrainController.Position;
            }

            EnemyBulletPool.Instance.Shoot(_bulletSpawnPoint.position, Quaternion.Euler(0f, 0f, _rotateZ));
            yield return new WaitForSeconds(_shootInterval);
            StartCoroutine(Shoot());
        }
    }
}