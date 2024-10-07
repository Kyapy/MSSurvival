using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUPSelectionButton : MonoBehaviour
{
    public TMP_Text upgradeDescText, nameLevelText;
    public Image weaponIcon;

    private Weapon assignedWeapon;

    public void UpdateButtonDisplay(Weapon theWeapon)
    {
        upgradeDescText.text = theWeapon.stats[theWeapon.weaponLevel].upgradeText;
        weaponIcon.sprite = theWeapon.icon;

        nameLevelText.text = theWeapon.name + " - Lv " + theWeapon.weaponLevel;

        assignedWeapon = theWeapon;
    }

    public void SelectUpgrade()
    {
        if (assignedWeapon != null)
        {
            assignedWeapon.LevelUp();

            UIController.Instance.levelUpPanel.SetActive(false);

            Time.timeScale = 1f;
        }
    }
}
