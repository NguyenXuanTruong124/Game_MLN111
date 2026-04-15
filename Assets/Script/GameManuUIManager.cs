using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuUIManager : MonoBehaviour
{
    public GameObject mapPanel;
    public GameObject guidePanel;
    public GameObject settingsPanel;

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

    // Bấm Exit thì thoát game ngay
    public void OnExitGame()
    {
        Debug.Log("Đang thoát game...");
        Application.Quit();
    }

    // Gọi khi bấm nút từng map
    public void LoadSceneByName(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
