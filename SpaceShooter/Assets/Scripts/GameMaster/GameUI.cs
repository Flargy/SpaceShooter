using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel = null;
    [SerializeField] private GameObject backgroundPanel = null;
    [SerializeField] private Slider bossSlider;

    [SerializeField] private Text playerHealth = null;
    private int health = 3;
    [SerializeField] private Text currentScore = null;
    private int score = 0;
    [SerializeField] private Text Highscore = null;
    [SerializeField] private Text currentWave = null;
    private int wave = 1;

    [SerializeField] private List<SpriteRenderer> background = new List<SpriteRenderer>();
    [SerializeField] private Image newBackgroundVisual;
    private int currentSpriteIndex = 0;
    [SerializeField] private List<Sprite> backgrounds = new List<Sprite>();

    public void UpdatePlayerHealth()
    {
        health -= 1;
        playerHealth.text = "" + health;
    }

    public void AssignBossHealth(float health)
    {
        bossSlider.gameObject.SetActive(true);
        bossSlider.maxValue = health;
        bossSlider.value = bossSlider.maxValue;
    }

    public void UpdateBossSlider(float damage)
    {
        bossSlider.value -= damage;
        if(bossSlider.value <= bossSlider.minValue)
        {
            Debug.Log("Boss is dead");
            bossSlider.gameObject.SetActive(false);
        }
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

    public void ShowBackgrounds(bool status)
    {
        backgroundPanel.SetActive(status);
        menuPanel.SetActive(!status);
    }

    public void NextBackground(int i)
    {
        currentSpriteIndex += i;
        if(currentSpriteIndex < 0)
        {
            currentSpriteIndex = backgrounds.Count - 1;
        }else if(currentSpriteIndex > backgrounds.Count - 1)
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
        ShowBackgrounds(false);
    }
}
