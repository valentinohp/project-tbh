using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wanpis.TBH.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public List<GameObject> Enemies = new List<GameObject>();
        [SerializeField] private List<GameObject> _enemyPool;
        private List<GameObject> _batchEnemySpawn = new List<GameObject>();
        private List<int> _enemyTypeCount = new List<int>();
        private int _waveNumber = 1;
        private bool _spawning = false;

        private void Start()
        {
            for (int i = 0; i < 3; i++)
            {
                _enemyTypeCount.Add(0);
            }
        }

        private void Update()
        {
            if (Enemies.Count == 0 && !_spawning)
            {
                StartCoroutine(Spawn());
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                GameObject enemy = Enemies[i].gameObject;
                if (!enemy.activeInHierarchy)
                {
                    _enemyPool.Add(enemy);
                    Enemies.RemoveAt(i);
                }
            }
        }

        private IEnumerator Spawn()
        {
            _spawning = true;
            yield return new WaitForSeconds(5f);
            int enemyCount = CountNumberOfEnemies(_waveNumber);
            for (int i = 0; i < enemyCount; i++)
            {
                int index = Random.Range(0, _enemyPool.Count);
                GameObject selectedEnemy = _enemyPool[index];
                if (selectedEnemy.name == "Blimp")
                {
                    if (_enemyTypeCount[0] >= 4)
                    {
                        continue;
                    }
                    _enemyTypeCount[0]++;
                }
                else if (selectedEnemy.name == "EnemyTrain")
                {
                    if (_enemyTypeCount[1] >= 4)
                    {
                        continue;
                    }
                    _enemyTypeCount[1]++;
                }
                else if (selectedEnemy.name == "Car" || selectedEnemy.name == "MiniCar")
                {
                    if (_enemyTypeCount[2] >= 2)
                    {
                        continue;
                    }
                    _enemyTypeCount[2]++;
                }
            }

            if (_enemyTypeCount[0] > 0)
            {
                int randomSpawn = Random.Range(0, 4);
                for (int i = 0; i < _enemyTypeCount[0]; i++)
                {
                    Blimp blimp = null;
                    for (int j = 0; j < _enemyPool.Count; j++)
                    {
                        if (_enemyPool[j].name == "Blimp")
                        {
                            blimp = _enemyPool[j].GetComponent<Blimp>();
                            _enemyPool.RemoveAt(j);
                            break;
                        }
                    }

                    blimp.RandomSpawn = randomSpawn++;
                    if (randomSpawn == 4)
                    {
                        randomSpawn = 0;
                    }

                    blimp.gameObject.SetActive(true);
                    blimp.gameObject.transform.position = transform.position;
                    blimp.Init();
                    _batchEnemySpawn.Add(blimp.gameObject);
                }

                _enemyTypeCount[0] = 0;
            }

            if (_enemyTypeCount[1] > 0)
            {
                int randomSpawn = Random.Range(0, 10);
                for (int i = 0; i < _enemyTypeCount[1]; i++)
                {
                    EnemyTrain enemyTrain = null;
                    for (int j = 0; j < _enemyPool.Count; j++)
                    {
                        if (_enemyPool[j].name == "EnemyTrain")
                        {
                            enemyTrain = _enemyPool[j].GetComponent<EnemyTrain>();
                            _enemyPool.RemoveAt(j);
                            break;
                        }
                    }

                    enemyTrain.RandomSpawn = randomSpawn++;
                    if (randomSpawn == 10)
                    {
                        randomSpawn = 0;
                    }

                    enemyTrain.gameObject.SetActive(true);
                    enemyTrain.gameObject.transform.position = transform.position;
                    enemyTrain.Init();
                    _batchEnemySpawn.Add(enemyTrain.gameObject);
                }

                _enemyTypeCount[1] = 0;
            }

            if (_enemyTypeCount[2] > 0)
            {
                int randomSpawn = Random.Range(0, 4);
                int prev = randomSpawn;
                Car car = null;
                for (int j = 0; j < _enemyPool.Count; j++)
                {
                    if (_enemyPool[j].name == "Car" || _enemyPool[j].name == "MiniCar")
                    {
                        car = _enemyPool[j].GetComponent<Car>();
                        _enemyPool.RemoveAt(j);
                        break;
                    }
                }

                car.RandomSpawn = randomSpawn;

                car.gameObject.SetActive(true);
                car.gameObject.transform.position = transform.position;
                car.Init();
                _batchEnemySpawn.Add(car.gameObject);

                if (_enemyTypeCount[2] > 1)
                {
                    car = null;
                    for (int j = 0; j < _enemyPool.Count; j++)
                    {
                        if (_enemyPool[j].name == "Car" || _enemyPool[j].name == "MiniCar")
                        {
                            car = _enemyPool[j].GetComponent<Car>();
                            _enemyPool.RemoveAt(j);
                            break;
                        }
                    }

                    if (prev < 2)
                    {
                        car.RandomSpawn = Random.Range(2, 4);
                    }
                    else
                    {
                        car.RandomSpawn = Random.Range(0, 2);
                    }

                    car.gameObject.SetActive(true);
                    car.gameObject.transform.position = transform.position;
                    car.Init();
                    _batchEnemySpawn.Add(car.gameObject);
                }

                _enemyTypeCount[2] = 0;
            }

            for (int i = 0; i < _batchEnemySpawn.Count; i++)
            {
                Enemies.Add(_batchEnemySpawn[i]);
            }

            _batchEnemySpawn = new List<GameObject>();
            _spawning = false;
            _waveNumber++;
        }

        private int CountNumberOfEnemies(int waveNumber)
        {
            if (waveNumber >= 1 && waveNumber <= 3)
                return 1;
            else if (waveNumber == 4)
                return 2;
            else if (waveNumber == 5)
                return 3;
            else if (waveNumber >= 6 && waveNumber <= 8)
                return 4;
            else if (waveNumber >= 9 && waveNumber <= 10)
                return 5;
            else
                return Random.Range(6, 11);
        }
    }
}
