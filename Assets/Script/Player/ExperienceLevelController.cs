using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceLevelController : MonoBehaviour
{
    public static ExperienceLevelController Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    public int currentExperience;

    public ExpPickup pickup;

    public List<int> expLevels;
    public int currentLevel = 1, levelCount = 100;

    private void Start()
    {
        while(expLevels.Count <levelCount)
        {
            expLevels.Add(Mathf.CeilToInt(expLevels[expLevels.Count - 1] * 1.1f));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetExp(int amountToGet)
    {
        currentExperience += amountToGet;

        if (currentExperience >= expLevels[currentLevel])
        {
            LevelUp();
        }

        UIController.Instance.UpdateExperience(currentExperience, expLevels[currentLevel], currentLevel);
        Debug.Log("Current Exp: " + currentExperience);
    }  
    
    public void SpawnExp(Vector3 position, int expValue)
    {
        Instantiate(pickup, position, Quaternion.identity).expValue = expValue;
    }
    void LevelUp()
    {
        currentExperience -= expLevels[currentLevel];

        currentLevel++;

        if (currentLevel >= expLevels.Count)
        {
            currentLevel = expLevels.Count - 1;
        }

        //PlayerController.instance.activeWeapon.LevelUp();

        UIController.Instance.levelUpPanel.SetActive(true);

        Time.timeScale = 0f;

        UIController.Instance.LevelUpButtons[0].UpdateButtonDisplay(PlayerController.instance.activeWeapon);
    }
}
