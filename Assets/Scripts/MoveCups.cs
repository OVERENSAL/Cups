using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCups : MonoBehaviour
{
    CupsHandler handler = new CupsHandler();
    public AudioClip took;
    public AudioClip take;
    private bool isDragging;
    Vector3 cupPosition = new Vector3(0, 0, 0);

    public void OnMouseDown()
    {
        handler.init();
        isDragging = handler.isMovingCup(transform);
        handler.setAvailablePlaces(transform);
        if (isDragging)
        {
            playAudio(take);
            cupPosition.x = transform.position.x;
            cupPosition.y = transform.position.y;
        }
    }

    public void OnMouseUp()
    {
        if (handler.isValidCoords(transform))
        {
            handler.changeMap();
        } else
        {
            transform.position = cupPosition;
        }
        playAudio(took);
        isDragging = false;
        handler.isWin();


        /*for (int i = 0; i < CreateCups.width; i++)
        {
            for (int j = 0; j < CreateCups.width - i; j++)
            {
                print(i + " " +  j + " " + CupsHandler.map[i, j]);
            }
        }*/

    }

    public void playAudio(AudioClip clip) {
        if (TurnOffSounds.sound) {
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            bool touch = false;
            
            foreach(CupCoords cupCoords in CupsHandler.availablePlaces)
            {
                if (Mathf.Sqrt(Mathf.Pow(Mathf.Abs(mousePosition.x - cupCoords.x), 2) + Mathf.Pow(Mathf.Abs(mousePosition.y - cupCoords.y), 2)) < 40)
                {
                    transform.position = new Vector3(cupCoords.x, cupCoords.y, 0);
                    touch = true;
                } 
            }

            if (!touch)
            {
                transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            }
            


        }
    }
}
