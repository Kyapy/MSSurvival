using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Slider explevelSlider;
    public TMP_Text explevelText;

    public LevelUPSelectionButton[] LevelUpButtons;

    public GameObject levelUpPanel;

    public TMP_Text coinText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateExperience(int currentExp, int levelExp, int currentLv)
    {
        explevelSlider.maxValue = levelExp;
        explevelSlider.value = currentExp;

        explevelText.text = "Lv " + currentLv;
    }

    public void SkipLevelp()
    {
        levelUpPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void UpdateCoins()
    {
        coinText.text = "Coins: " + CoinController.instance.currentCoins;
    }
}
