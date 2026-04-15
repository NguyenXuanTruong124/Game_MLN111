using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    private int clothScore = 0;
    private int coinScore = 0;
    
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI clothText;
    [SerializeField] private TextMeshProUGUI bookText;
    
    // Win conditions
    [SerializeField] private int requiredFoodCount = 5;
    [SerializeField] private int requiredClothCount = 3;
    [SerializeField] private int requiredCoinCount = 10;
    [SerializeField] private int requiredKeyCount = 4;
    
    public GameMode gameMode = GameMode.Level1; // Thay đổi từ [SerializeField] private sang public

    public enum GameMode
    {
        Level1,  // Food + Cloth
        Level2   // Coin + Cloth + Key
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

    public void AddBookScore(int points)
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
        // Màn 1: Hiển thị Food/Cloth
        if (gameMode == GameMode.Level1)
        {
            scoreText.text = $"{score}/{requiredFoodCount}";
            clothText.text = $"{clothScore}/{requiredClothCount}";
        }
        // Màn 2: Hiển thị Coin/Cloth
        else if (gameMode == GameMode.Level2)
        {
            scoreText.text = $"{coinScore}/{requiredCoinCount}";
            bookText.text = $"{score}/{requiredKeyCount}";
        }
    }

    public bool CanWin()
    {
        if (gameMode == GameMode.Level1)
        {
            // Màn 1: Cần Food + Cloth
            return score >= requiredFoodCount && clothScore >= requiredClothCount;
        }
        else if (gameMode == GameMode.Level2)
        {
            // Màn 2: Cần Coin + Cloth + Key (4 cái)
            return coinScore >= requiredCoinCount && clothScore >= requiredClothCount && score >= requiredKeyCount;
        }

        return false;
    }

    public int GetRequiredFoodCount() => requiredFoodCount;
    public int GetRequiredClothCount() => requiredClothCount;
    public int GetRequiredCoinCount() => requiredCoinCount;
    public int GetRequiredKeyCount() => requiredKeyCount;
    public int GetCurrentFoodCount() => score;
    public int GetCurrentClothCount() => clothScore;
    public int GetCurrentCoinCount() => coinScore;
    public int GetCurrentKeyCount() => score; // Dùng score để track key (tùy cách quản lý)
}
