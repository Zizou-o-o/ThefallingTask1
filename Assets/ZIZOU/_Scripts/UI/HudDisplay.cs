using TMPro;
using UnityEngine;

public class HudDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text waveText;

    private void OnEnable()
    {
        ScoreManager.ScoreChanged += UpdateScore;
        WaveManager.WaveStarted += UpdateWave;
    }

    private void OnDisable()
    {
        ScoreManager.ScoreChanged -= UpdateScore;
        WaveManager.WaveStarted -= UpdateWave;
    }

    private void UpdateScore(int score)
    {
        scoreText.text = " " + score;
    }

    private void UpdateWave(int wave)
    {
        waveText.text = " " + wave;
    }
}