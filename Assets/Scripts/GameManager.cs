using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int totalCoins;
    private int lastSceneIndex;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Метод для сохранения прогресса
    public void SaveGame()
    {
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            PlayerPrefs.SetInt("LastSceneIndex", SceneManager.GetActiveScene().buildIndex + 1);
        }
        PlayerPrefs.Save();
    }

    // Метод для загрузки прогресса
    public void LoadGame()
    {
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0); 
        lastSceneIndex = PlayerPrefs.GetInt("LastSceneIndex", 0);
        SceneManager.LoadScene(lastSceneIndex);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("TotalCoins");
        PlayerPrefs.DeleteKey("LastSceneIndex");
        totalCoins = 0;
    }

    public int GetTotalCoins()
    {
        return totalCoins;
    }
}
