using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct CupIndexes
{
    public int x;
    public int y;

    public CupIndexes(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public struct CupCoords
{
    public float x;
    public float y;

    public CupCoords(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}

public class CupsHandler : MonoBehaviour
{
    public static List<CupIndexes> availableIndexes = new List<CupIndexes>();
    public static List<CupCoords> availablePlaces = new List<CupCoords>();
    CreateCups creator = new CreateCups();
    StartPlaying start = new StartPlaying();
    public static CupCoords[,] cupCoords = new CupCoords[CreateCups.width, CreateCups.width];
    public static int[,] map = new int[CreateCups.width, CreateCups.width];
    public static int[,] mapCoords = new int[CreateCups.width, CreateCups.width];
    public int takeIndexX = 0, takeIndexY = 0;

    public void Main(string[] args)
	{
        
    }

    public void init()
    {
        if (takeIndexX == 0 && takeIndexY == 0)
        {
            map = StartPlaying.getMap();
            cupCoords = StartPlaying.getCupCoords();
        }
        getAvaivableCups(map);
    }

    public void getAvaivableCups(int[,] map)
    {
        availableIndexes.Clear();
        int indexX = (int)Mathf.Sqrt(map.Length) - 1;
        int indexY = 0;
        while (indexX != 0 || indexY != 0)
        {
            if (indexX > 0 && indexY == 0)
            {
                if (map[indexX - 1, indexY] == 0 && map[indexX, indexY] > 0)
                {
                    addAvailableIndexes();
                }
            }
            else if (indexX == 0 && indexY > 0)
            {
                if (map[indexX, indexY - 1] == 0 && map[indexX, indexY] > 0)
                {
                    addAvailableIndexes();
                }
            }
            else
            {
                if (map[indexX - 1, indexY] == 0 && map[indexX, indexY - 1] == 0 && map[indexX, indexY] > 0)
                {
                    addAvailableIndexes();
                }
            }

            indexX--;
            indexY++;
            if (indexX < 0)
            {
                indexX = indexY - 2;
                indexY = 0;
            }
        }

        void addAvailableIndexes()
        {
            availableIndexes.Add(new CupIndexes(indexX, indexY));
        }
    }

    public bool isMovingCup(Transform transform)
    {
        for(int i = 0; i < availableIndexes.Count; i++)
        {
            if (transform.position.x == cupCoords[availableIndexes[i].x, availableIndexes[i].y].x && transform.position.y == cupCoords[availableIndexes[i].x, availableIndexes[i].y].y)
            {
                takeIndexX = availableIndexes[i].x;
                takeIndexY = availableIndexes[i].y;
                return true;
            }
        }

        return false;
    }

    public void setAvailablePlaces(Transform transform)
    {
        availablePlaces.Clear();
        int indexX = 0;
        int indexY = 0;
        while (indexX != CreateCups.width)
        {
            if (indexX + indexY == CreateCups.width - 1)
            {
                if (map[indexX, indexY] == 0)
                {
                    print(indexX + " " + indexY);
                    availablePlaces.Add(new CupCoords(transform.position.x, transform.position.y));
                    continue;
                }
                continue;
            }

            if (map[indexX + 1, indexY] != 0 && map[indexX, indexY + 1] != 0
                && (((indexX + 1) != takeIndexX || indexY != takeIndexY)
                && (indexX != takeIndexX || (indexY + 1) != takeIndexY)))
            {
                print(indexX + " " + indexY);
                availablePlaces.Add(new CupCoords(transform.position.x, transform.position.y));
                continue;
            }
            indexY++;
            if (indexY == CreateCups.width)
            {
                indexX++;
                indexY = 0;
            }
        }


    }

    public void changeCups()
    {

    }
}
