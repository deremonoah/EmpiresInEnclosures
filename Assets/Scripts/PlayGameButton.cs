using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGameButton : MonoBehaviour
{
    public void PlayGamePress()
    {
        SceneManager.LoadScene(1);//can use name as a string
    }
}
