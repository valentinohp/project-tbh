using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainController : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _topBoundary;
    [SerializeField] private float _bottomBoundary;
    [SerializeField] private int _maxCharges = 3;
    private int _healthLeft;
    private int _chargesLeft;
    private int _currentTrack;
    private bool _axisInUse = false;
    private bool _isAlive = true;
    [SerializeField] private Timer _chargeCooldownTimer;
    [SerializeField] private TMP_Text _chargesLeftUI;
    [SerializeField] private Slider _cooldownSlider;
    public static Transform Position;

    private void Start()
    {
        Position = transform;
        _chargesLeft = _maxCharges;
        _currentTrack = 3;
        _healthLeft = _maxHealth;
        _chargeCooldownTimer.OnTimerEnd += AddCharge;
    }

    private void Update()
    {
        if (_isAlive)
        {
            MovementVertical();
            MovementHorizontal();
            RechargeHorizontal();
        }
    }

    private void MovementVertical()
    {
        float yMovementClamp = Mathf.Clamp(transform.position.y, _bottomBoundary, _topBoundary);
        transform.position = new Vector3(transform.position.x, yMovementClamp, 0);
        float move = Input.GetAxis("Vertical") * _moveSpeed * Time.deltaTime;
        transform.Translate(new Vector3(0, move, 0));
    }

    private void MovementHorizontal()
    {
        float move = Input.GetAxisRaw("Horizontal");

        if (move != 0)
        {
            if (_axisInUse == false)
            {
                if (_chargesLeft > 0)
                {
                    ChangeTrack(move);
                }
                _axisInUse = true;
            }
        }

        if (move == 0)
        {
            _axisInUse = false;
        }
    }

    private void ChangeTrack(float move)
    {
        if (move > 0 && _currentTrack < 5)
        {
            _currentTrack++;
            transform.Translate(new Vector3(0.75f, 0, 0));
            _chargesLeft--;
        }
        else if (move < 0 && _currentTrack > 1)
        {
            _currentTrack--;
            transform.Translate(new Vector3(-0.75f, 0, 0));
            _chargesLeft--;
        }

        UpdateChargesUI();
    }

    private void RechargeHorizontal()
    {
        if (_chargesLeft < _maxCharges && !_chargeCooldownTimer.GetIsRunning())
        {
            _chargeCooldownTimer.StartTimer();
            _cooldownSlider.gameObject.SetActive(true);
        }

        if (_chargesLeft == _maxCharges && _chargeCooldownTimer.GetIsRunning())
        {
            _chargeCooldownTimer.EndTimer();
            _cooldownSlider.gameObject.SetActive(false);
        }

        if (_chargeCooldownTimer.GetIsRunning())
        {
            _cooldownSlider.value = _chargeCooldownTimer.GetRemainingTime() / _chargeCooldownTimer.GetDuration();
        }
    }

    private void AddCharge()
    {
        _chargesLeft++;
        UpdateChargesUI();
    }

    private void UpdateChargesUI()
    {
        _chargesLeftUI.text = _chargesLeft.ToString();
    }

    private void TakeDamage()
    {
        _healthLeft--;
        Debug.Log("Remaining health: " + _healthLeft);
        if (_healthLeft == 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("player dead");
        _isAlive = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" || other.tag == "EnemyBullet")
        {
            TakeDamage();
        }
    }
}
