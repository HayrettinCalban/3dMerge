using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI Instance { get; private set; }
    public int score = 0;
    public int highScore = 0;
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            highScore = PlayerPrefs.GetInt("HighScore", 0);

            // SceneManager olaylarına abone ol
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        FindScoreTextReference();

        UpdateScoreText();
    }


    void FindScoreTextReference()
    {

        var go = GameObject.Find("ScoreText");
        if (go != null)
        {
            scoreText = go.GetComponent<TextMeshProUGUI>();
            if (scoreText == null)
            {
                Debug.LogError("ScoreText objesinde TextMeshProUGUI componenti bulunamadı!");
            }
        }
        else
        {
            scoreText = null;
            Debug.LogWarning("ScoreText isimli GameObject sahnede bulunamadı. Skor gösterilemeyebilir.");
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"Skor eklendi: {amount}. Yeni skor: {score}");
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            Debug.Log($"Yeni Yüksek Skor: {highScore}");
        }
        UpdateScoreText();
    }

    public void ResetScore()
    {
        score = 0;
        Debug.Log("Skor sıfırlandı.");
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}\nHigh Score: {highScore}";
            Debug.Log("Skor Text'i güncellendi.");
        }
        else
        {
            Debug.LogWarning("ScoreText referansı null, UI güncellenemiyor.");
        }
    }
}