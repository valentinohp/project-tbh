using System.Collections;
using UnityEngine;

namespace Wanpis.TBH.Enemy
{
    public class Car : MonoBehaviour
    {
        [SerializeField] private int _health = 5;
        [SerializeField] private Vector3[] _spawnPoints;
        [SerializeField] private float _moveSpeed;
        private Vector3 _target;
        private Vector3 _exitPoint;
        public int RandomSpawn = -1;
        private int _currentState = 1;
        private bool _targetSet = false;
        private bool _exit = false;
        [SerializeField] private float _shootInterval = 3f;
        private int _healthLeft;
        [SerializeField] private Timer _carDuration;
        [SerializeField] private float _score = 10000f;
        [SerializeField] private ScoreManager _scoreManager;

        public void Init()
        {
            _healthLeft = _health;
            _carDuration.OnTimerEnd += ChangeExitState;

            if (RandomSpawn == -1)
            {
                RandomSpawn = Random.Range(0, _spawnPoints.Length);
            }

            transform.position = _spawnPoints[RandomSpawn];

            if (_spawnPoints[RandomSpawn].y < 0)
            {
                _exitPoint = _spawnPoints[RandomSpawn] + new Vector3(0, 15f, 0);
            }
            else
            {
                _exitPoint = _spawnPoints[RandomSpawn] + new Vector3(0, -15f, 0);
            }

            _carDuration.StartTimer();
            StartCoroutine(StartShoot());
            _currentState = 1;
            _targetSet = false;
            _exit = false;
        }

        private void Update()
        {
            if (_currentState == 1)
            {
                Chase();
            }
            else if (_currentState == 2 && !_targetSet)
            {
                Exit();
            }

            float move = _moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, _target.y, 0), move);

            if (_exit && transform.position == _target)
            {
                gameObject.SetActive(false);
            }
        }

        private void Chase()
        {
            _target = TrainController.Position.position;
        }

        private void Exit()
        {
            _target = _exitPoint;
            _targetSet = true;
            _exit = true;
        }

        private void ChangeExitState()
        {
            _currentState++;
        }

        private IEnumerator StartShoot()
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(Shoot());
        }

        private IEnumerator Shoot()
        {
            if (gameObject.name == "MiniCar")
            {
                SoundManager.Instance.PlaySFX("EnemyAttackBike");
            }
            else
            {
                SoundManager.Instance.PlaySFX("EnemyAttackCar");
            }

            if (transform.position.x < 0)
            {
                EnemyBulletPool.Instance.Shoot(transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
            else
            {
                EnemyBulletPool.Instance.Shoot(transform.position, Quaternion.Euler(0f, 0f, -180f));
            }
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
