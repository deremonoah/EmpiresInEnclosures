using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarManager : MonoBehaviour
{
    public Image healthBar;
    public float health;

    public void UpdateHP(float HPpercent)
    {
        healthBar.fillAmount = HPpercent;
    }
}
