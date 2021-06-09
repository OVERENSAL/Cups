using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCups : MonoBehaviour
{
    CupsHandler handler = new CupsHandler();
    private bool isDragging;

    public void OnMouseDown()
    {
        handler.init();
        isDragging = handler.isMovingCup(transform);
        //handler.setAvailablePlaces(transform);
    }

    public void OnMouseUp()
    {
        print(transform.position);
        //handler.changeCups();
        isDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {   
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }
    }
}
