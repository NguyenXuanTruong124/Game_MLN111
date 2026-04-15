using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCData npcData;
    [SerializeField] private float interactionDistance = 2f;
    
    private bool playerInRange = false;
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
            Debug.LogError("NPCDialogUI không tìm thấy!");

        // Set sprite từ NPCData
        if (npcData != null && npcData.npcSprite != null)
        {
            spriteRenderer.sprite = npcData.npcSprite;
        }
        else
        {
            Debug.LogWarning("NPCData hoặc npcSprite chưa được gán!");
        }

        interactionDistance = npcData != null ? npcData.interactionDistance : 2f;
    }

    private void Update()
    {
        if (playerTransform == null || npcData == null) return;

        // Kiểm tra khoảng cách với player
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        playerInRange = distance <= interactionDistance;

        // Debug info
        if (playerInRange)
        {
            Debug.Log($"NPC: {npcData.npcName} - Khoảng cách: {distance:F2}, Sẵn sàng tương tác (ấn F)");
        }

        // Nếu gần và ấn F
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F được ấn! Hiển thị dialog...");
            ShowDialog();
        }
    }

    private void ShowDialog()
    {
        if (dialogUI != null)
        {
            Debug.Log($"Hiển thị dialog cho: {npcData.npcName}");
            dialogUI.ShowDialog(npcData.npcName, npcData.question, npcData.answers, this);
        }
        else
        {
            Debug.LogError("dialogUI vẫn null!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (npcData == null) return;
        
        // Vẽ vòng tròn interaction range trong editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, npcData.interactionDistance);
    }
}
