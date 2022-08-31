using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action onGameOver;
    public static event Action<int> onScoreUpdated;

    [SerializeField] private Player _player;

    private int _score;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        Time.timeScale = 1;
        _score = 0;

        onScoreUpdated?.Invoke(_score);
    }

    private void OnEnable()
    {
        _player.onExploded += GameOver;
        Missile.onExplodeWithPoints += AddPoints;
    }

    private void OnDisable()
    {
        _player.onExploded -= GameOver;
        Missile.onExplodeWithPoints -= AddPoints;
    }

    private void GameOver()
    {
        // Time.timeScale = 0;

        onGameOver?.Invoke();
    }

    private void AddPoints(int points)
    {
        _score += points;

        onScoreUpdated?.Invoke(_score);
    }
}
