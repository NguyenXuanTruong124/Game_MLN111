using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    private int score = 0;
    private int clothScore = 0;
    private int coinScore = 0;
    
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI clothText;
    
    // Win conditions
    [SerializeField] private int requiredFoodCount = 5;      // Màn 1: cần 5 food
    [SerializeField] private int requiredClothCount = 5;     // Cần 3 cloth
    [SerializeField] private int requiredCoinCount = 5;      // Màn 2: cần coin
    
    [SerializeField] private GameMode gameMode = GameMode.Level1;

    public enum GameMode
    {
        Level1,  // Food + Cloth
        Level2   // Coin + Cloth
    }

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

    public void AddCoinScore(int points)
    {
        coinScore += points;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = score.ToString();
        clothText.text = clothScore.ToString(); 
    }

    // Kiểm tra đã đủ điều kiện win chưa
    public bool CanWin()
    {
        if (gameMode == GameMode.Level1)
        {
            // Màn 1: Cần đủ Food và Cloth
            return score >= requiredFoodCount && clothScore >= requiredClothCount;
        }
        else if (gameMode == GameMode.Level2)
        {
            // Màn 2: Cần đủ Coin và Cloth
            return coinScore >= requiredCoinCount && clothScore >= requiredClothCount;
        }

        return false;
    }

    public int GetRequiredFoodCount() => requiredFoodCount;
    public int GetRequiredClothCount() => requiredClothCount;
    public int GetRequiredCoinCount() => requiredCoinCount;
    public int GetCurrentFoodCount() => score;
    public int GetCurrentClothCount() => clothScore;
    public int GetCurrentCoinCount() => coinScore;
}
