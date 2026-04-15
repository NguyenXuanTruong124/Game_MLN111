using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuUIManager : MonoBehaviour
{
    public GameObject mapPanel;
    public GameObject guidePanel;
    public GameObject settingsPanel;
    public GameObject exitConfirmPanel; // Panel xác nhận thoát
    

    // Gọi khi bấm nút "Play"
    public void OnPlayButton()
    {
        mapPanel.SetActive(true);
        guidePanel.SetActive(false);
    }

    // Gọi khi bấm nút "Guide"
    public void OnGuideButton()
    {
        guidePanel.SetActive(true);
        mapPanel.SetActive(false);
    }

    // Gọi khi bấm nút "Exit" trong panel hướng dẫn
    public void OnExitGuide()
    {
        guidePanel.SetActive(false);
    }

    public void OnExitMapPanel()
    {
        mapPanel.SetActive(false);
    }

    public void OnBackGroundSOund()
    {
        AudioManager.Instance.ToggleMusic();
    }

    public void OnShowSettings()
    {
        bool isActive = settingsPanel.activeSelf;
        settingsPanel.SetActive(!isActive);
    }

    // Gọi khi bấm nút "Exit" - hiện panel xác nhận
    public void OnExitGame()
    {
        if (exitConfirmPanel != null)
        {
            exitConfirmPanel.SetActive(true);
        }
    }

    // Xác nhận thoát game
    public void OnConfirmExit()
    {
        Application.Quit();
    }

    // Hủy bỏ thoát game
    public void OnCancelExit()
    {
        if (exitConfirmPanel != null)
        {
            exitConfirmPanel.SetActive(false);
        }
    }

    // Gọi khi bấm nút từng map
    public void LoadSceneByName(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
