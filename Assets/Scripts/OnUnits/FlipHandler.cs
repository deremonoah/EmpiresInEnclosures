using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipHandler : MonoBehaviour
{
    // flip if you are on the player layer
    void Start()
    {
        if (this.gameObject.layer == 7)//so on the player layer, flip the transform 180 so it faces the right way to do animations
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }
}
