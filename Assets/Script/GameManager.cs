using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    private int score = 0;
    private int clothScore = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField]private TextMeshProUGUI clothText;
    void Start()
    {
        UpdateScore();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScore();
    }

    public void AddClothScore(int points)
    {
        clothScore += points;
        UpdateScore();
    }
    public void UpdateScore()
    {
        scoreText.text = score.ToString();
        clothText.text = clothScore.ToString();
    }
}
