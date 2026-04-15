using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCDialogUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    
    private NPC currentNPC;

    private void Awake()
    {
        if (dialogPanel != null)
            dialogPanel.SetActive(false);
    }

    public void ShowDialog(string npcName, string question, string[] answers, NPC npc)
    {
        currentNPC = npc;
        
        questionText.text = question;

        // Setup 2 nút Yes/No
        if (yesButton != null && answers.Length > 0)
        {
            yesButton.GetComponentInChildren<TextMeshProUGUI>().text = answers[0];
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(() => OnAnswerSelected(0, answers[0]));
        }

        if (noButton != null && answers.Length > 1)
        {
            noButton.GetComponentInChildren<TextMeshProUGUI>().text = answers[1];
            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(() => OnAnswerSelected(1, answers[1]));
        }

        dialogPanel.SetActive(true);
        Time.timeScale = 0f; // Pause game
    }

    private void OnAnswerSelected(int answerIndex, string answerText)
    {
        Debug.Log($"Bạn chọn câu trả lời {answerIndex}: {answerText}");
        CloseDialog();
    }

    public void CloseDialog()
    {
        dialogPanel.SetActive(false);
        Time.timeScale = 1f; // Resume game
    }
}
