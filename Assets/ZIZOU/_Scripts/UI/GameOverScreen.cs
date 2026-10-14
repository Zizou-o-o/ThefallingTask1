using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    

    private int lastScore;

    private void Awake()
    {
        panel.SetActive(false);
    }

    private void OnEnable()
    {
        ScoreManager.ScoreChanged += OnScoreChanged;
        GameManager.GameOver += Show;
    }

    private void OnDisable()
    {
        ScoreManager.ScoreChanged -= OnScoreChanged;
        GameManager.GameOver -= Show;
    }

    private void OnScoreChanged(int score)
    {
        lastScore = score;
    }

    private void Show()
    {
        
        panel.SetActive(true);
    }
}
