using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public GameObject GameOver;
    public GameObject GameWin;
    public GameObject GamePause;
    public GameObject GameSettings;
    
    private bool isPaused = false;


    public void ShowGameOver()
    {
        GameOver.SetActive(true);
        Time.timeScale = 0f;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLose();
        }
    }


    public void ShowGameWin()
    
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayWin();
        }
        GameWin.SetActive(true);
        Time.timeScale = 0f;
        
    }

   
    public void PauseGame()
    {
        isPaused = true;
        GamePause.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        GamePause.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu"); 
    }

    public void OpenSettings()
    {

        bool isActive = GameSettings.activeSelf;
        GameSettings.SetActive(!isActive);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameOver.activeSelf && !GameWin.activeSelf)
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
