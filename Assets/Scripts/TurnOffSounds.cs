using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;	
using UnityEngine;

public class TurnOffSounds : MonoBehaviour
{
	public Sprite turnOn;
	public Sprite turnOff;

    public void click()
    {
    	GetComponent<Image>().sprite = GetComponent<Image>().sprite == turnOn ? turnOff : turnOn;
    	TurnSounds();
    }

    private void TurnSounds() {
    	//turnoff/on sounds
    }
}
