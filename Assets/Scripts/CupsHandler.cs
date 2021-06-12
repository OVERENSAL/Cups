using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject panel;
    public static List<CupIndexes> availableIndexes = new List<CupIndexes>();
    public static List<CupCoords> availablePlaces = new List<CupCoords>();
    CreateCups creator = new CreateCups();
    StartPlaying start = new StartPlaying();
    public static CupCoords[,] cupCoords = new CupCoords[CreateCups.width, CreateCups.width];
    public static int[,] map = new int[CreateCups.width, CreateCups.width];
    public static int[,] targetMap = new int[CreateCups.width, CreateCups.width];
    public static int[,] currentMapChange = new int[CreateCups.width, CreateCups.width];
    public int takeIndexX = 0, takeIndexY = 0, placingIndexX = 0, placingIndexY = 0;

    public void Main(string[] args)
	{

    }

    public void init()
    {
        if (takeIndexX == 0 && takeIndexY == 0)
        {
            map = StartPlaying.getMap();
            targetMap = StartPlaying.getTargetMap();
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
        availablePlaces.Clear(); // [(x,y), ... ]
        int X = 0;
        int Y = 0;

        print(CreateCups.width + " width");

        for (int indexX = 0; indexX < CreateCups.width; indexX++)
        {
            for (int indexY = 0; indexY < CreateCups.width - indexX; indexY++)
            {
                setIndexes(transform);
                if (map[indexX, indexY] == 0 && ((indexX + indexY == CreateCups.width - 1) 
                    || map[indexX + 1, indexY] != 0 && map[indexX, indexY + 1] != 0) /*&& !(X == indexX && Y == indexY)*/ && !(X == indexX+1 && Y == indexY) && !(X == indexX && Y == indexY+1))
                {
                    availablePlaces.Add(new CupCoords(cupCoords[indexX, indexY].x, cupCoords[indexX, indexY].y));
                }                
            }
        }

        print(availablePlaces.Count+" Count");

        void setIndexes(Transform transform)
        {
            for (int indexX = 0; indexX < CreateCups.width; indexX++)
            {
                for (int indexY = 0; indexY < CreateCups.width - indexX; indexY++)
                {
                    if (cupCoords[indexX, indexY].x == transform.position.x && cupCoords[indexX, indexY].y == transform.position.y)
                    {
                        X = indexX;
                        Y = indexY;
                    }
                }
            }
        }

    }

    public bool isValidCoords(Transform transform)
    {
        for(int i = 0; i < CreateCups.width; i++)
        {
            for(int j = 0; j < CreateCups.width; j++)
            {
                if (transform.position.x == cupCoords[i, j].x && transform.position.y == cupCoords[i, j].y)
                {
                    placingIndexX = i;
                    placingIndexY = j;
                    return true;
                }
            }
        }

        return false;
    }

    public void changeMap()
    {
        map[placingIndexX, placingIndexY] = map[takeIndexX, takeIndexY];
        map[takeIndexX, takeIndexY] = 0;
    }

    public bool isWin()
    {
        for(int i = 0; i < CreateCups.width; i++)
        {
            for (int j = 0; j < CreateCups.width; j++)
            {
                if (map[i, j] != targetMap[i, j])
                {   
                    return false;
                }
            }
        }

        StartPlaying.isWin = true;
        return true;
    }
}
