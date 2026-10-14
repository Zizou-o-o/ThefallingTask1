using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
    [SerializeField] private Image[] lifeIcons;

    private void OnEnable() { GameManager.LivesChanged += UpdateLives; }
    private void OnDisable() { GameManager.LivesChanged -= UpdateLives; }

    private void UpdateLives(int lives)
    {
        for (int i = 0; i < lifeIcons.Length; i++)
            lifeIcons[i].enabled = i < lives;
    }
}