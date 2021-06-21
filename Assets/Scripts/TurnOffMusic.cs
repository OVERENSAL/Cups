using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;	

public class TurnOffMusic : MonoBehaviour
{
    public Sprite turnOn;
	public Sprite turnOff;
    public GameObject button;
    public static bool music = true;

    void Start()
    {
        if (music){
            button.GetComponent<Image>().sprite = turnOn;
        } else {
            button.GetComponent<Image>().sprite = turnOff;
        }
    }

    public void click()
    {
    	this.gameObject.GetComponent<Image>().sprite = this.gameObject.GetComponent<Image>().sprite == turnOn ? turnOff : turnOn;
        music = !music;
        
    }
}
