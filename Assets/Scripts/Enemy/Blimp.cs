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
        public int RandomSpawn = -1;
        private int _currentState = 1;
        private Queue _positionQueue;
        private bool _targetSet = false;
        private bool _canMove = true;
        private bool _exit = false;
        [SerializeField] private Transform _bulletSpawnPoint;
        private Transform _targetShoot;
        private float _rotateZ;
        [SerializeField] private float _shootInterval = 3f;
        private int _healthLeft;
        [SerializeField] private float _score = 10000f;
        [SerializeField] private ScoreManager _scoreManager;

        public void Init()
        {
            _healthLeft = _health;
            _positionQueue = new Queue();

            if (RandomSpawn == -1)
            {
                RandomSpawn = Random.Range(0, _corners.Length);
            }

            _enterPoint = _corners[RandomSpawn];

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
                if (RandomSpawn + 1 == _corners.Length)
                {
                    _positionQueue.Enqueue(_corners[0]);
                    RandomSpawn = 0;
                }
                else
                {
                    _positionQueue.Enqueue(_corners[++RandomSpawn]);
                }
            }

            _targetShoot = TrainController.Position;
            StartCoroutine(Shoot());
            _currentState = 1;
            _targetSet = false;
            _canMove = true;
            _exit = false;
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

        private IEnumerator Shoot()
        {
            if (_targetShoot == null)
            {
                _targetShoot = TrainController.Position;
            }

            SoundManager.Instance.PlaySFX("EnemyAttackDrone");
            EnemyBulletPool.Instance.Shoot(_bulletSpawnPoint.position, Quaternion.Euler(0f, 0f, _rotateZ));
            yield return new WaitForSeconds(_shootInterval);
            StartCoroutine(Shoot());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Bullet")
            {
                SoundManager.Instance.PlaySFX("PlayerAttackHits");
                _healthLeft--;
                other.gameObject.SetActive(false);
            }

            if (other.tag == "Player")
            {
                _healthLeft = 0;
            }

            if (_healthLeft <= 0)
            {
                SoundManager.Instance.PlaySFX("EnemyDefeated");
                gameObject.SetActive(false);
                _scoreManager.AddScore(_score);
            }
        }
    }
}