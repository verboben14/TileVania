using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] float levelExitSlowMotionFactor = 0.4f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            StartCoroutine(LoadNextLevelWithDelay());
        }
    }

    private IEnumerator LoadNextLevelWithDelay()
    {
        Time.timeScale = levelExitSlowMotionFactor;
        yield return new WaitForSeconds(levelLoadDelay);

        Time.timeScale = 1f;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
