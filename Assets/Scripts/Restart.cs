using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Restart : MonoBehaviour
{
    [SerializeField]
    private GameObject deathPanel;
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        deathPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
