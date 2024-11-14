using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        GameManager.instance.ResetProgress(); // Сброс прогресса
        SceneManager.LoadScene(0); // Загружаем первую игровую сцену
    }

    public void ContinueGame()
    {
        GameManager.instance.LoadGame(); // Загружаем сохранённый прогресс
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowMenu()
    {
        gameObject.SetActive(true);
    }

    public void HideMenu()
    {
        gameObject.SetActive(false);
    }
}
