using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private Transform[] _backgrounds;
    [SerializeField] private float _speed = 1f;
    public bool _isMoving = false;

    private void Update()
    {
        if (_isMoving)
        {
            MoveBackground();
        }
    }

    private void MoveBackground()
    {
        Vector3 move = new Vector3(0, -_speed * Time.deltaTime, 0);
        for (int i = 0; i < _backgrounds.Length; i++)
        {
            _backgrounds[i].transform.position += move;
            if (_backgrounds[i].transform.position.y <= -13.5f)
            {
                _backgrounds[i].transform.position = new Vector3(0, 27f, 0);
            }
        }
    }
}
