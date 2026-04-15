using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class NPCDialogUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private float feedbackDuration = 2f;
    
    private NPC currentNPC;
    private NPCData currentNPCData;
    private bool isAnswered = false;

    private void Awake()    
    {
        if (dialogPanel != null)
            dialogPanel.SetActive(false);
    }

    public void ShowDialog(string npcName, string question, string[] answers, NPC npc, NPCData npcData)
    {
        currentNPC = npc;
        currentNPCData = npcData;
        isAnswered = false;
        
        questionText.text = question;

        // Setup 2 nút Yes/No
        if (yesButton != null && answers.Length > 0)
        {
            yesButton.GetComponentInChildren<TextMeshProUGUI>().text = answers[0];
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(() => OnAnswerSelected(0, answers[0]));
            yesButton.interactable = true;
        }

        if (noButton != null && answers.Length > 1)
        {
            noButton.GetComponentInChildren<TextMeshProUGUI>().text = answers[1];
            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(() => OnAnswerSelected(1, answers[1]));
            noButton.interactable = true;
        }

        dialogPanel.SetActive(true);
        Time.timeScale = 0f; // Pause game
    }

    private void OnAnswerSelected(int answerIndex, string answerText)
    {
        if (isAnswered) return;
        
        isAnswered = true;
        bool isCorrect = (answerIndex == currentNPCData.correctAnswerIndex);
        
        // Vô hiệu hóa nút
        if (yesButton != null) yesButton.interactable = false;
        if (noButton != null) noButton.interactable = false;
        
        // Đổi màu nút được chọn
        HighlightSelectedButton(answerIndex, isCorrect);
        
        // Hiển thị feedback
        ShowFeedback(isCorrect);
        
        if (isCorrect)
        {
            SpawnReward();
            // Khóa NPC sau khi trả lời đúng
            if (currentNPC != null)
                currentNPC.LockInteraction();
            
            StartCoroutine(CloseDialogAfterDelay());
        }
        else
        {
            // Nếu sai, cho phép chọn lại sau 1 giây
            StartCoroutine(AllowRetryAfterDelay());
        }
    }

    private void HighlightSelectedButton(int answerIndex, bool isCorrect)
    {
        Color highlightColor = isCorrect ? Color.green : Color.red;
        
        if (answerIndex == 0 && yesButton != null)
        {
            yesButton.GetComponent<Image>().color = highlightColor;
            yesButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        else if (answerIndex == 1 && noButton != null)
        {
            noButton.GetComponent<Image>().color = highlightColor;
            noButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
    }

    private void ShowFeedback(bool isCorrect)
    {
        if (isCorrect)
        {
            questionText.text = "Bạn đã trả lời đúng!";
            questionText.color = Color.green;
            Debug.Log("✓ Trả lời đúng!");
        }
        else
        {
            questionText.text = "Bạn đã trả lời sai!";
            questionText.color = Color.red;
            Debug.Log("✗ Trả lời sai!");
        }
    }

    private void SpawnReward()
    {
        if (currentNPCData.rewardPrefab != null && currentNPC != null)
        {
            Vector3 spawnPosition = currentNPC.transform.position + currentNPCData.rewardSpawnOffset;
            Instantiate(currentNPCData.rewardPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Reward spawned!");
        }
    }

    private IEnumerator AllowRetryAfterDelay()
    {
        yield return new WaitForSecondsRealtime(1f);
        
        // Reset để cho phép chọn lại
        isAnswered = false;
        questionText.text = currentNPCData.question;
        questionText.color = Color.white;
        
        // Bật lại nút
        if (yesButton != null)
        {
            yesButton.interactable = true;
            yesButton.GetComponent<Image>().color = Color.white;
            yesButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
        
        if (noButton != null)
        {
            noButton.interactable = true;
            noButton.GetComponent<Image>().color = Color.white;
            noButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
    }

    private IEnumerator CloseDialogAfterDelay()
    {
        yield return new WaitForSecondsRealtime(feedbackDuration);
        CloseDialog();
    }

    public void CloseDialog()
    {
        questionText.color = Color.white;
        
        // Reset nút về màu trắng
        if (yesButton != null)
        {
            yesButton.GetComponent<Image>().color = Color.white;
            yesButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
        
        if (noButton != null)
        {
            noButton.GetComponent<Image>().color = Color.white;
            noButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
        
        dialogPanel.SetActive(false);
        Time.timeScale = 1f; // Resume game
    }
}
