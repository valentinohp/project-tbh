using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreUI;
    public static float Score;
    [SerializeField] private float _scoreTime = 0.5f;

    private void Update()
    {
        if (TrainController.IsAlive)
        {
            Score += _scoreTime;
            _scoreUI.text = Score.ToString("0000000000000");
        }
    }

    public void AddScore(float score)
    {
        Score += score;
    }
}
