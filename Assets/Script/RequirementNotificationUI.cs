using UnityEngine;
using TMPro;
using System.Collections;
            
public class RequirementNotificationUI : MonoBehaviour
{
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TextMeshProUGUI requirementText;
    [SerializeField] private float displayDuration = 3f;
    
    private GameManager gameManager;
    private Coroutine hideCoroutine;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        
        if (notificationPanel != null)
            notificationPanel.SetActive(false);
    }

    public void ShowRequirement()
    {
        if (notificationPanel == null || requirementText == null || gameManager == null)
            return;

        // Xóa coroutine cũ nếu có
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        // Cập nhật text dựa vào game mode
        string message = GetRequirementMessage();
        requirementText.text = message;

        // Hiển thị panel
        notificationPanel.SetActive(true);

        // Tự động ẩn sau duration
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private string GetRequirementMessage()
    {
        // Lấy thông tin từ GameManager
        int currentFood = gameManager.GetCurrentFoodCount();
        int requiredFood = gameManager.GetRequiredFoodCount();
        int currentCloth = gameManager.GetCurrentClothCount();
        int requiredCloth = gameManager.GetRequiredClothCount();
        int currentCoin = gameManager.GetCurrentCoinCount();
        int requiredCoin = gameManager.GetRequiredCoinCount();

        string message = "⚠️ Chưa đủ điều kiện!\n\n";

        // Hiển thị dựa vào game mode
        if (gameManager.gameMode == GameManager.GameMode.Level1)
        {
            message += $"🍎 Food: {currentFood}/{requiredFood}\n";
            message += $"👕 Cloth: {currentCloth}/{requiredCloth}";
        }
        else if (gameManager.gameMode == GameManager.GameMode.Level2)
        {
            message += $"💰 Coin: {currentCoin}/{requiredCoin}\n";
            message += $"👕 Cloth: {currentCloth}/{requiredCloth}";
        }

        return message;
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        notificationPanel.SetActive(false);
    }

    public void HideNotification()
    {
        notificationPanel.SetActive(false);
    }
}
