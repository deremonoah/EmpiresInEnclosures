using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasEnabler : MonoBehaviour
{
    [SerializeField] List<GameObject> cans;
    void Start()
    {
        StartCoroutine(EnableCanvasesRoutine());
    }

public IEnumerator EnableCanvasesRoutine()
    {
        //this is to see if this fixes the null issue with out panel scripts
        yield return new WaitForSeconds(0.003f);
        foreach(GameObject can in cans)
        {
            can.SetActive(true);
        }
    }
}
