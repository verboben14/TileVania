using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] float delayBeforeReload = 1f;
    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;

    private void Awake()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numberOfGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateLivesText();
        UpdateScoreText();
    }

    public IEnumerator ProcessPlayerDeath()
    {
        yield return new WaitForSeconds(delayBeforeReload);

        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddCoinToScore(int coinValue)
    {
        score += coinValue;
        UpdateScoreText();
    }

    private void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        UpdateLivesText();
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    private void UpdateLivesText()
    {
        livesText.text = playerLives.ToString();
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
