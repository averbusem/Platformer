using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("SampleScene");
        PlayerPrefs.DeleteKey("SavedScene");
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("SavedScene"))
        {
            string savedScene = PlayerPrefs.GetString("SavedScene");
            SceneManager.LoadScene(savedScene);
        }
        else
        {
            Debug.Log("You haven't any saved games");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
