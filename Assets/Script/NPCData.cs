using UnityEngine;

[CreateAssetMenu(fileName = "NPC Data", menuName = "NPC/New NPC")]
public class NPCData : ScriptableObject
{
    [SerializeField] public string npcName = "NPC";
    [SerializeField] public string question = "Bạn là ai?";
    [SerializeField] public string[] answers = { "Câu trả lời 1", "Câu trả lời 2" };
    [SerializeField] public int correctAnswerIndex = 0;
    [SerializeField] public GameObject rewardPrefab;
    [SerializeField] public Sprite npcSprite;
    [SerializeField] public float interactionDistance = 2f;
    [SerializeField] public Vector3 rewardSpawnOffset = Vector3.zero; 
}
