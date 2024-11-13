using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngineInternal;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
       if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

}
