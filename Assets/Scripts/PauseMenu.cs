using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    public static PauseMenu Instance;
    [SerializeField]
    private GameObject gameUI;
    private bool isPaused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadLevel()
    {
        //Logic for loading level
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
