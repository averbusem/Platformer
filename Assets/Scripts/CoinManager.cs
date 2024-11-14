using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    private Text coinText;
    private int totalCoins = 0;
    private int currentCoin = 0;

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

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateUI();
        UpdateCoinText();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateUI();
        UpdateCoinText();
    }

    private void UpdateUI()
    {
        GameObject coinTextObject = GameObject.Find("PlayerCoinsCount");
        if (coinTextObject != null)
        {
            coinText = coinTextObject.GetComponent<Text>();
        }
    }

    public void AddCoin()
    {
        currentCoin++;
        totalCoins++;
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = totalCoins.ToString();
        }
    }

    public void ResetLevelCoins()
    {
        totalCoins -= currentCoin;
        currentCoin = 0;
        UpdateCoinText();
    }

    public void SaveCoins()
    {
        currentCoin = 0;
    }

    public void ResetAllCoins()
    {
        totalCoins = 0;
        currentCoin = 0;
        UpdateCoinText();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
