using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button1LevelDiff : MonoBehaviour
{
    public void getLevel1(string scene)
    {
        DiffHandler.levelDifficult = "1";
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayingPlace");
    }
}
