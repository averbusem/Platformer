using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SaveGame(); // ���������� ��������� ��� ���������� ������
            FindObjectOfType<CoinManager>().SaveCoins();
            LoadNextLevel(); // ������� � ���������� ������
        }
    }

    private void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("�����������, ���� ��������!"); // �� ������ ������� ����� ����� ���������� ����
            // ��������� � ������� ���� ��� ��������� ����
            SceneManager.LoadScene("MainMenu"); // ���������� �������� ����� ����� � ������� ����
        }
    }
}
