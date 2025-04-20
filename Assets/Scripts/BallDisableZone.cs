using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallDisableZone : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void OnTriggerExit(Collider other)
    {
        GameOver();
    }

    private void OnTriggerEnter(Collider other)
    {
        BallController ball = other.GetComponent<BallController>();
        if (ball != null)
        {
            ball.enabled = false;
        }
    }

    public void GameOver()
    {
        if (Time.timeScale == 0f) return;

        Debug.Log("GameOver çağrıldı.");
        Time.timeScale = 0f;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
    public void RestartGame()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Time.timeScale = 1f;

        if (ScoreUI.Instance != null)
        {
            ScoreUI.Instance.ResetScore();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}