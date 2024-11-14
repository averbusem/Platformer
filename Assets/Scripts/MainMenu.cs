using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        GameManager.instance.ResetProgress(); // ����� ���������
        SceneManager.LoadScene(0); // ��������� ������ ������� �����
    }

    public void ContinueGame()
    {
        GameManager.instance.LoadGame(); // ��������� ���������� ��������
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
