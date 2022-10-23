using UnityEngine;
using UnityEngine.UI;
using Wanpis.TBH.Enemy;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuNagivation : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private TrainController _player;
    [SerializeField] private Turret _turret;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private GameObject _gameOver;
    [SerializeField] private Button _backButton;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private GameObject _title;

    private void Start()
    {
        _playButton.onClick.RemoveAllListeners();
        _playButton.onClick.AddListener(StartPlay);

        _quitButton.onClick.RemoveAllListeners();
        _quitButton.onClick.AddListener(QuitGame);

        _backButton.onClick.RemoveAllListeners();
        _backButton.onClick.AddListener(BackToMainMenu);
    }

    private void StartPlay()
    {
        _playButton.gameObject.SetActive(false);
        _quitButton.gameObject.SetActive(false);
        TrainController.IsAlive = true;
        _turret.StartShooting();
        _enemySpawner.gameObject.SetActive(true);
        SoundManager.Instance.PlaySFX("StartGame");
        _title.SetActive(false);
    }

    private void QuitGame()
    {
        SoundManager.Instance.PlaySFX("Proceed");
        Application.Quit();
    }

    public void GameOver()
    {
        SoundManager.Instance.PlaySFX("PlayerDefeated");
        _gameOver.SetActive(true);
        _score.text = ScoreManager.Score.ToString();
    }

    private void BackToMainMenu()
    {
        SoundManager.Instance.PlaySFX("Back");
        SceneManager.LoadScene("Gameplay");
        ScoreManager.Score = 0;
        BulletPool.Instance.Clear();
        EnemyBulletPool.Instance.Clear();
    }
}
