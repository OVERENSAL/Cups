using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;	
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void getScene(string scene) {
    	UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
