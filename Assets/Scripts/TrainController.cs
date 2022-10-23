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
    public static bool IsAlive = false;
    [SerializeField] private Timer _chargeCooldownTimer;
    [SerializeField] private TMP_Text _chargesLeftUI;
    [SerializeField] private Slider _cooldownSlider;
    public static Transform Position;
    [SerializeField] private TMP_Text _healthUI;
    [SerializeField] private MenuNagivation _menuNavigation;
    [SerializeField] private Image _lever;
    [SerializeField] private Sprite _leverUp;
    [SerializeField] private Sprite _leverDown;

    private void Start()
    {
        Position = transform;
        _chargesLeft = _maxCharges;
        _currentTrack = 3;
        _healthLeft = _maxHealth;
        _chargeCooldownTimer.OnTimerEnd += AddCharge;
        _healthUI.text = "Health: " + _healthLeft;
    }

    private void Update()
    {
        if (IsAlive)
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
            SoundManager.Instance.PlaySFX("PlayerChangeRail");
        }
        else if (move < 0 && _currentTrack > 1)
        {
            _currentTrack--;
            transform.Translate(new Vector3(-0.75f, 0, 0));
            _chargesLeft--;
            SoundManager.Instance.PlaySFX("PlayerChangeRail");
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
            _cooldownSlider.value = (_chargeCooldownTimer.GetDuration() - _chargeCooldownTimer.GetRemainingTime()) / _chargeCooldownTimer.GetDuration();
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
        if (_chargesLeft == 0)
        {
            _lever.sprite = _leverDown;
        }
        else
        {
            _lever.sprite = _leverUp;
        }
    }

    private void TakeDamage()
    {
        _healthLeft--;
        _healthUI.text = "Health " + _healthLeft;
        if (_healthLeft <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        IsAlive = false;
        _healthUI.text = "Dead";
        gameObject.SetActive(false);
        _menuNavigation.GameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" || other.tag == "EnemyBullet")
        {
            if (other.tag == "Enemy")
            {
                SoundManager.Instance.PlaySFX("TrainCrash");
            }

            if (other.tag == "EnemyBullet")
            {
                SoundManager.Instance.PlaySFX("EnemyAttackHits");
            }

            TakeDamage();
            other.gameObject.SetActive(false);
        }
    }
}
