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
        mapPanel.SetActive(true);     // Hiện panel chọn map
        guidePanel.SetActive(false);  // Tắt panel hướng dẫn nếu đang mở
    }

    // Gọi khi bấm nút "Guide"
    public void OnGuideButton()
    {
        guidePanel.SetActive(true);   // Hiện hướng dẫn
        mapPanel.SetActive(false);    // Tắt map nếu đang mở
    }

    // Gọi khi bấm nút "Exit" trong panel hướng dẫn
    public void OnExitGuide()
    {
        guidePanel.SetActive(false);  // Tắt hướng dẫn
    }
    public void OnExitMapPanel()
    {
        mapPanel.SetActive(false);  // Tắt hướng dẫn
    }
    public void OnBackGroundSOund()
    {
        AudioManager.Instance.ToggleMusic(); // Bật/Tắt nhạc nền
    }
    public void OnShowSettings()
    {
        

        // Toggle: nếu đang bật thì tắt, đang tắt thì bật
        bool isActive = settingsPanel.activeSelf;
        settingsPanel.SetActive(!isActive);
    }

    public void OnExitGame()
    {
        Application.Quit(); // Thoát game
    }

    // Gọi khi bấm nút từng map (ví dụ map1, map2,...)
    public void LoadSceneByName(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
        
    }
}
