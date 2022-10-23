using System.Collections;
using UnityEngine;

namespace Wanpis.TBH.Enemy
{
    public class EnemyTrain : MonoBehaviour
    {
        [SerializeField] private int _health = 5;
        [SerializeField] private Vector3[] _spawnPoints;
        [SerializeField] private float _moveSpeed;
        private Vector3 _enterPoint;
        private Vector3 _target;
        private Vector3 _exitPoint;
        public int RandomSpawn = -1;
        private int _currentState = 1;
        private bool _targetSet = false;
        private bool _canMove = true;
        private bool _exit = false;
        private int _healthLeft;
        [SerializeField] private float _score = 10000f;
        [SerializeField] private ScoreManager _scoreManager;

        public void Init()
        {
            _healthLeft = _health;

            if (RandomSpawn == -1)
            {
                RandomSpawn = Random.Range(0, _spawnPoints.Length);
            }

            _enterPoint = _spawnPoints[RandomSpawn];

            if (_enterPoint.y < 0)
            {
                transform.position = _enterPoint + new Vector3(0, -1.5f, 0);
                _exitPoint = _enterPoint + new Vector3(0, 15f, 0);
            }
            else
            {
                transform.position = _enterPoint + new Vector3(0, 1.5f, 0);
                _exitPoint = _enterPoint + new Vector3(0, -15f, 0);
            }

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
            else if (_currentState == 2 && !_targetSet)
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
        }

        private void Spawn()
        {
            _target = _enterPoint;
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