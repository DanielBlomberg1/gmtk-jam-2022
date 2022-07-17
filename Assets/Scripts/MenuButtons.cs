using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void Quit(){
        Application.Quit();
    }

    public void Play(){
        SceneManager.LoadScene("scene_main");
    }
}
