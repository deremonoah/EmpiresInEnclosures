using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBarFinder : MonoBehaviour
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
