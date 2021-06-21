using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;	
using UnityEngine;

public class TurnOffSounds : MonoBehaviour
{
	public Sprite turnOn;
	public Sprite turnOff;
    public GameObject button;
    public static bool sound = true;

    void Start()
    {
        if (sound) {
            button.GetComponent<Image>().sprite = turnOn;
        } else {
            button.GetComponent<Image>().sprite = turnOff;
        }
    }

    public void click()
    {
    	this.gameObject.GetComponent<Image>().sprite = this.gameObject.GetComponent<Image>().sprite == turnOn ? turnOff : turnOn;
        sound = !sound;
        
    }

}
