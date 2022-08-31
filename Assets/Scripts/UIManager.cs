using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _gameOverScreen;

    [SerializeField] private Damageable _player;

    private void OnEnable()
    {
        _player.onHealthUpdated += UpdateHealthText;
        GameManager.onGameOver += ShowGameOverScreen;
        GameManager.onScoreUpdated += UpdateScoreText;
    }

    private void OnDisable()
    {
        _player.onHealthUpdated -= UpdateHealthText;
        GameManager.onGameOver -= ShowGameOverScreen;
        GameManager.onScoreUpdated -= UpdateScoreText;
    }

    private void UpdateHealthText()
    {
        _healthText.SetText($"Health: {_player.health}");
    }

    private void UpdateScoreText(int score)
    {
        _scoreText.SetText($"Score: {score}");
    }

    private void ShowGameOverScreen()
    {
        _gameOverScreen.SetActive(true);
    }
}
