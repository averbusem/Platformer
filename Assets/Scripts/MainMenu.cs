using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("SampleScene"); 
    }

    public void ContinueGame()
    {
        // Логика продолжения игры (например, загрузка сохраненного прогресса)
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
