using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel = null;
    [SerializeField] private GameObject backgroundPanel = null;
    [SerializeField] private GameObject gameOverPanel = null;
    [SerializeField] private GameObject howToPlayPanel = null;
    [SerializeField] private Slider bossSlider = null;
    [SerializeField] private Text gameOverScore = null;
    [SerializeField] private Text playerHealth = null;
    [SerializeField] private Text currentScore = null;
    [SerializeField] private Text currentWave = null;
    [SerializeField] private List<SpriteRenderer> background = new List<SpriteRenderer>();
    [SerializeField] private Image newBackgroundVisual = null;
    [SerializeField] private List<Sprite> backgrounds = new List<Sprite>();

    [field: SerializeField] private List<Text> upgradeTexts = new List<Text>();

    private int health = 3;
    private int score = 0;
    private int wave = 1;
    private int currentSpriteIndex = 0;

    public void UpdatePlayerHealth()
    {
        health -= 1;
        playerHealth.text = "" + health;
    }

    public void AssignBossHealth(float health)
    {
        Debug.Log("health: " + health);
        bossSlider.gameObject.SetActive(true);
        bossSlider.maxValue = health;
        bossSlider.value = bossSlider.maxValue;
    }

    public void UpdateBossSlider(float damage)
    {
        bossSlider.value -= damage;

        if(bossSlider.value <= 0)
        {

            HideBossHealth();
        }
    }

    public void HideBossHealth()
    {
        bossSlider.gameObject.SetActive(false);
    }


    public void UpdateUpgrades(PowerUpEnums.PowerEnum targetEnum, float upgrade)
    {
        upgradeTexts[(int)targetEnum].text = upgrade.ToString("F1");
    }

    public void UpdatePlayerScore(int point)
    {
        score += point;
        currentScore.text = "" + score;
    }

    public void UpdateWave()
    {
        wave += 1;
        currentWave.text = "" + wave;
    }

    public void StartTheGame()
    {
        menuPanel.SetActive(false);
        GameVariables.gameRunning = true;
    }

    public void PauseGame()
    {
        menuPanel.SetActive(true);
        GameVariables.gameRunning = false;
    }

    public void ToggleBackground()
    {
        backgroundPanel.SetActive(!backgroundPanel.activeSelf);
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    public void ToggleHowToPlay()
    {
        howToPlayPanel.SetActive(!howToPlayPanel.activeSelf);
        menuPanel.SetActive(!menuPanel.activeSelf);
    }
    public void ToggleGameOver()
    {
        gameOverPanel.SetActive(!gameOverPanel.activeSelf);
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    public void NextBackground(int i)
    {
        currentSpriteIndex += i;
        if(currentSpriteIndex < 0)
        {
            currentSpriteIndex = backgrounds.Count - 1;
        }
        else if(currentSpriteIndex > backgrounds.Count - 1)
        {
            currentSpriteIndex = 0;
        }

        newBackgroundVisual.sprite = backgrounds[currentSpriteIndex];
    }

    public void Changebackground()
    {
        foreach(SpriteRenderer renderer in background)
        {
            renderer.sprite = backgrounds[currentSpriteIndex];
        }

        ToggleBackground();
    }


    public void GameOver()
    {
        gameOverScore.text = "" + score;
        score = 0;
        health = 3;
        wave = 1;
        playerHealth.text = "" + health;
        currentScore.text = "" + score;
        currentWave.text = "" + wave;
        menuPanel.SetActive(true);
        ToggleGameOver();
    }

}
