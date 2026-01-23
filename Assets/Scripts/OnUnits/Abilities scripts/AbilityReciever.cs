using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityReciever : MonoBehaviour
{
    private Collider2D _collider;
    //this doesn't actually receive anything it just takes care of the case of
    //non aura units instantiating in the trigger
    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        _collider.enabled = false;
        yield return new WaitForFixedUpdate();
        _collider.enabled = true;
    }
}
