using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelChoosing : MonoBehaviour
{
    public string scene;
    // Start is called before the first frame update
    public void change()
    {
        UnityEngine.Debug.Log("clicklevel");
        SceneManager.LoadScene(sceneName: scene);
    }
}
