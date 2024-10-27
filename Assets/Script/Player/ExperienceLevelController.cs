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

    public List<Weapon> weaponsToUpgrade;

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

        //UIController.Instance.LevelUpButtons[0].UpdateButtonDisplay(PlayerController.instance.activeWeapon);
        //UIController.Instance.LevelUpButtons[0].UpdateButtonDisplay(PlayerController.instance.assignedWeapons[0]);

        //UIController.Instance.LevelUpButtons[1].UpdateButtonDisplay(PlayerController.instance.unassignedWeapons[0]);
        //UIController.Instance.LevelUpButtons[2].UpdateButtonDisplay(PlayerController.instance.unassignedWeapons[1]);

        weaponsToUpgrade.Clear();

        List<Weapon> availableWeapons = new List<Weapon>();
        availableWeapons.AddRange(PlayerController.instance.assignedWeapons);

        if (availableWeapons.Count > 0)
        {
            int selected = Random.Range(0, availableWeapons.Count);
            weaponsToUpgrade.Add(availableWeapons[selected]);
            availableWeapons.RemoveAt(selected); 
        }

        if (PlayerController.instance.assignedWeapons.Count + PlayerController.instance.fullyLevelledWeapons.Count < PlayerController.instance.maxWeapons)
        {
            availableWeapons.AddRange(PlayerController.instance.unassignedWeapons);
        }

        for (int i = weaponsToUpgrade.Count; i < PlayerController.instance.maxWeapons; i++)
        {
            if (availableWeapons.Count > 0)
            {
                int selected = Random.Range(0, availableWeapons.Count);
                weaponsToUpgrade.Add(availableWeapons[selected]);
                availableWeapons.RemoveAt(selected);
            }
        }

        for (int i = 0; i < weaponsToUpgrade.Count; i++)
        {
            UIController.Instance.LevelUpButtons[i].UpdateButtonDisplay(weaponsToUpgrade[i]);
        }

        for (int i = 0; i < UIController.Instance.LevelUpButtons.Length; i++)
        {
            if (i <weaponsToUpgrade.Count)
            {
                UIController.Instance.LevelUpButtons[i].gameObject.SetActive(true);
            }
            else
            {
                UIController.Instance.LevelUpButtons[i].gameObject.SetActive(false);
            }
        }
    }
}
