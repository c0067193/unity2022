using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int scoreToWin;
    public int currentScore;
    public Enemy2 enemy2;

    public bool gamePaused;

    public static GameManager instance;
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            TogglePausedGame();
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Time.timeScale = 1.0f;
    }
    public void TogglePausedGame()
    {
        //if the game is not pasused then gamepaused set to be true
        // if the game is paused then gamepasused set to be false
        gamePaused = !gamePaused;
        Time.timeScale = gamePaused == true ? 0.0f : 1.0f;

        Cursor.lockState = gamePaused == true ? CursorLockMode.None : CursorLockMode.Locked;

        //toggle the pasuse menu
        UI.instance.TogglePauseMenu(gamePaused);
    }
    public void addScore(int score)
    {
        //if (enemy2.health < 0)
       // {
        //    score += 1;
       // }
        currentScore = score;
       

        //update the score text
        UI.instance.UpdateScoreText(currentScore);

        //have we reached the score to win?
        if (currentScore >= scoreToWin)
            winGame();
    }
    void winGame()
    {
        //set the end game screen
        UI.instance.setEndGameScreen(true, currentScore);
        Invoke("Swichscene", 0);
    }
    public void LoseGame()
    {
        //set the end game screen
        UI.instance.setEndGameScreen(false, currentScore);
        Time.timeScale = 0.0f;
        gamePaused = true;
        Cursor.lockState = CursorLockMode.None;
        Invoke("Swichscene", 0);
    }
  public void Swichscene()
    {
        SceneManager.LoadScene(3);
    }
}
