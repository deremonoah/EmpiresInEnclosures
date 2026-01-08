using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarFinder : MonoBehaviour
{
    private Image hpBar;
    private void OnEnable()
    {
        hpBar = GetComponent<Image>();
    }

    public Image getHPBar()
    {
        return hpBar;
    }
}
