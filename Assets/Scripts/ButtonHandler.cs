using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public void click() {
    	UnityEngine.Debug.Log("click");
        SceneManager.LoadScene(sceneName: "LevelChoosing");
    }
}
