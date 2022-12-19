using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class UI : MonoBehaviour
{
    [Header("HUD")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI amoText;
    public Image healthBarFill;
    public player player;

    [Header("Paused Menu")]
    public GameObject pauseMenu;

    [Header("End Game Screen")]
    public GameObject endGameScreen;
    public TextMeshProUGUI endGameHeaderText;
    public TextMeshProUGUI endGameScoreText;

    public static UI instance;

    private void Awake()
    {
        instance = this;
    }
    public void updatehealthBar(int currentHP, int maxHP)
    {
        healthBarFill.fillAmount = (float)currentHP / (float)maxHP;
        
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = "Score:" + score;
        
    }
    

    public void UpdateAmmoText(int currentAmmo, int maxAmmo)
    {
        amoText.text = "Ammo:" + currentAmmo + "/" + maxAmmo;
       
    }
    public void TogglePauseMenu(bool paused)
    {
        pauseMenu.SetActive(paused);
    }
    public void setEndGameScreen(bool won, int score)
    {
        endGameScreen.SetActive(true);
        endGameHeaderText.text = won == true ? "You won" : "You Lose";
        endGameHeaderText.color = won == true ? Color.green : Color.red;
        endGameScoreText.text = "<b>Score</b>\n" + score;
    }

    public void onResumeButton()
    {
        GameManager.instance.TogglePausedGame();
    }

    public void onRestartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void onMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
