using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button2LevelDiff : MonoBehaviour
{
    public void getLevel2(string scene)
    {
        DiffHandler.levelDifficult = "2";
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayingPlace");
    }
}
