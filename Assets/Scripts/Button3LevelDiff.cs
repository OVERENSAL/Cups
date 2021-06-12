using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button3LevelDiff : MonoBehaviour
{
    public void getLevel3(string scene)
    {
        DiffHandler.levelDifficult = "3";
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayingPlace");
    }
}
