using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCData npcData;
    [SerializeField] private float interactionDistance = 2f;
    
    private bool playerInRange = false;
    private bool hasInteracted = false; // Khóa NPC khi trả lời đúng
    private Transform playerTransform;
    private NPCDialogUI dialogUI;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerTransform = FindAnyObjectByType<PlayerController>().transform;
        dialogUI = FindAnyObjectByType<NPCDialogUI>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playerTransform == null)
            Debug.LogError("PlayerController không tìm thấy!");
        
        if (dialogUI == null)
            Debug.LogError("NPCDialogUI không tìm thấy! Hãy tạo GameObject có script NPCDialogUI");

        // Set sprite từ NPCData
        if (npcData != null && npcData.npcSprite != null)
        {
            spriteRenderer.sprite = npcData.npcSprite;
        }
        else
        {
            Debug.LogWarning("NPCData hoặc npcSprite chưa được gán!");
        }

        if (npcData != null)
            interactionDistance = npcData.interactionDistance;
    }

    private void Update()
    {
        // Nếu đã trả lời đúng rồi thì ko cho tương tác
        if (hasInteracted) return;
        
        if (playerTransform == null || npcData == null || dialogUI == null) return;

        // Kiểm tra khoảng cách với player
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        playerInRange = distance <= interactionDistance;

        // Nếu gần và ấn F
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            ShowDialog();
        }
    }

    private void ShowDialog()
    {
        if (dialogUI != null && npcData != null)
        {
            dialogUI.ShowDialog(npcData.npcName, npcData.question, npcData.answers, this, npcData);
        }
        else
        {
            Debug.LogError("dialogUI hoặc npcData vẫn null!");
        }
    }

    // Method được gọi từ NPCDialogUI khi trả lời đúng
    public void LockInteraction()
    {
        hasInteracted = true;
        Debug.Log($"NPC {npcData.npcName} đã bị khóa!");
    }

    private void OnDrawGizmosSelected()
    {
        if (npcData == null) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, npcData.interactionDistance);
    }
}
