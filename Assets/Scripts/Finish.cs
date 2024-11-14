using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SaveGame(); // Сохранение прогресса при завершении уровня
            FindObjectOfType<CoinManager>().SaveCoins();
            LoadNextLevel(); // Переход к следующему уровню
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
            Debug.Log("Поздравляем, игра пройдена!"); // Вы можете сделать здесь экран завершения игры
            // Вернуться в главное меню или завершить игру
            SceneManager.LoadScene("MainMenu"); // Подставьте название вашей сцены с главным меню
        }
    }
}
