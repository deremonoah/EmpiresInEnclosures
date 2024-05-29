using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private Animator animor;
    private void Awake()
    {
        animor = GetComponent<Animator>();
    }
    public void SetAnimationState(int state)
    {
        animor.SetFloat("State", state);
    }

    //add took damage and then maybe a coroutine for just chaning the material, so its independant of animator
    public void TookDamage()
    {
        //in future linking this to the amount of damage taken so you sell the impact more
        StartCoroutine(DisplayDamageRoutine());
    }

    public IEnumerator DisplayDamageRoutine()
    {
        //change material
        SpriteRenderer sr= this.gameObject.GetComponent<SpriteRenderer>();
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
}
