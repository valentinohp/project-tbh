using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField] private GameObject[] _railPrefabs;
    [SerializeField] private int _trackSize = 20;
    [SerializeField] private float _speed = 1f;
    private GameObject[] _rails;
    private Vector3 _defaultSpawnPosition;
    private float _boundary;
    private Vector3[] _railPositions;
    private int _pointer;
    public bool _isMoving = false;

    private void Start()
    {
        _rails = new GameObject[_trackSize];
        _railPositions = new Vector3[_trackSize];
        _boundary = _trackSize * -0.32f;
        _defaultSpawnPosition = new Vector3(transform.position.x, -_boundary, 0);

        for (int i = 0; i < _trackSize; i++)
        {
            GameObject rail = _railPrefabs[Random.Range(0, _railPrefabs.Length)];
            float y = -0.32f * _trackSize + i * 0.64f;
            GameObject railObject = Instantiate(rail, transform);
            railObject.transform.position += new Vector3(0, y, 0);
            _railPositions[i] = railObject.transform.position;
            _rails[i] = railObject;
        }

        _boundary += 0.64f;
        _pointer = _trackSize - 1;
    }

    private void Update()
    {
        if (_isMoving)
        {
            MoveTrack();
        }
    }

    private void MoveTrack()
    {
        Vector3 move = new Vector3(0, -_speed * Time.deltaTime, 0);

        for (int i = 0; i < _railPositions.Length; i++)
        {
            _railPositions[i] += move;
            _rails[i].transform.position = _railPositions[i];
        }

        if (_railPositions[_pointer].y < -_boundary)
        {
            if (_pointer + 1 == _trackSize)
            {
                _railPositions[0] = _railPositions[_pointer] + new Vector3(0, 0.64f, 0);
                _pointer = 0;
            }
            else
            {
                _railPositions[_pointer+1] = _railPositions[_pointer] + new Vector3(0, 0.64f, 0);
                _pointer++;
            }
        }
    }
}
