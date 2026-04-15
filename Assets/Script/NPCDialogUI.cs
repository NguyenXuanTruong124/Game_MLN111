using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCDialogUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Transform answersContainer;
    [SerializeField] private Button answerButtonPrefab;
    
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

        // Xóa các câu trả lời cũ
        foreach (Transform child in answersContainer)
        {
            Destroy(child.gameObject);
        }

        // Tạo nút cho mỗi câu trả lời
        for (int i = 0; i < answers.Length; i++)
        {
            Button answerButton = Instantiate(answerButtonPrefab, answersContainer);
            answerButton.GetComponentInChildren<TextMeshProUGUI>().text = answers[i];
            
            int index = i;
            answerButton.onClick.AddListener(() => OnAnswerSelected(index, answers[index]));
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
