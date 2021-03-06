using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCups : MonoBehaviour
{
    private static Dictionary<int, int> rules = new Dictionary<int, int>();
    public static List<List<int>> freeIndexes = new List<List<int>>();
    public static List<List<int>> availableIndexes = new List<List<int>>();
    public static int width = 0;
    public static int allCupsNumber = 0;
    public void Main(string[] args)
    {
        if (rules.Count == 0)
        {
            initDictionary();
        }
        
        const int minCupsNumber = 5;
        const int maxCupsNumber = 8;
        if (LevelCounter.levelCount >= 10) {
            width = 8;
        }
        else if (LevelCounter.levelCount > 5) {
            width = Random.Range(7, maxCupsNumber + 1);
        } else {
            width = Random.Range(minCupsNumber, maxCupsNumber + 1);
        }
        
        allCupsNumber = Random.Range(getMinCupsNumber(), getMaxCupsNumber());

        int[,] map = new int[width, width];
        int[,] target = new int[width, width];

        map = createMap(map, allCupsNumber);
        getAvaivableCups(map);
        target = shuffle(1, target, map);
    }

    int getMinCupsNumber() {
        if (LevelCounter.levelCount < 5) {
            return 5;
        } else if (LevelCounter.levelCount < 10) {
            return 8;
        } else {
            return 12;
        }
    }

    int getMaxCupsNumber() {
        if (LevelCounter.levelCount < 5) {
            return 9;
        } else if (LevelCounter.levelCount < 10) {
            return 12;
        } else {
            return 17;
        }
    }

    public int[,] shuffle(int level, int[,] target, int[,] map)
    {
        int iteration = getIteration(level);
        for (int i = 0; i < Mathf.Sqrt(map.Length); i++)
        {
            for (int j = 0; j < Mathf.Sqrt(map.Length); j++)
            {
                target[i, j] = map[i, j];
            }
        }
        for (int i = 0; i < iteration; i++)
        {
            //shuf();
            getAvaivableCups(target);
            getFreeIndexes(target);
            int cupIndex = Random.Range(0, availableIndexes.Count);
            int freeIndex = Random.Range(0, freeIndexes.Count);
            while (isUnderTheAvailableIndex(cupIndex, freeIndex))
            {
                freeIndex = Random.Range(0, freeIndexes.Count);
            }
            int transfer = target[availableIndexes[cupIndex][0], availableIndexes[cupIndex][1]];
            target[availableIndexes[cupIndex][0], availableIndexes[cupIndex][1]] = 0;
            target[freeIndexes[freeIndex][0], freeIndexes[freeIndex][1]] = transfer;

            while (isEqual()) {
                shuf();
            }
        }

        return target;

        bool isEqual() {
            for(int i = 0; i < CreateCups.width; i++)
            {
                for (int j = 0; j < CreateCups.width; j++)
                {
                    if (map[i, j] != target[i, j])
                    {   
                        return false;
                    }
                }
            }

            return true;
        }

        void shuf() {
            /*int memoryX = -1;
            int memoryY = -1;*/
            for (int i = 0; i < iteration; i++)
            {
                getAvaivableCups(target);
                getFreeIndexes(target);
                int cupIndex = Random.Range(0, availableIndexes.Count);
                int freeIndex = Random.Range(0, freeIndexes.Count);
                /*if (memoryX != -1) {
                    while (availableIndexes[freeIndex][0] != memoryX || availableIndexes[freeIndex][1] != memoryY) {
                        cupIndex = Random.Range(0, availableIndexes.Count);
                    }
                }*/
                while (isUnderTheAvailableIndex(cupIndex, freeIndex))
                {
                    freeIndex = Random.Range(0, freeIndexes.Count);
                }
                /*memoryX = freeIndexes[freeIndex][0];
                memoryY = freeIndexes[freeIndex][1];*/
                int transfer = target[availableIndexes[cupIndex][0], availableIndexes[cupIndex][1]];
                target[availableIndexes[cupIndex][0], availableIndexes[cupIndex][1]] = 0;
                target[freeIndexes[freeIndex][0], freeIndexes[freeIndex][1]] = transfer;
            }
        }
    }

    bool isUnderTheAvailableIndex(int cupIndex, int freeIndex)
    {
        int x = availableIndexes[cupIndex][0];
        int y = availableIndexes[cupIndex][1];
        if (x == 0)//top line
        {
            if (freeIndexes[freeIndex][0] == x && freeIndexes[freeIndex][1] == y - 1)
            {
                return true;
            }
        }
        else if (y == 0)//left line 
        {
            if (freeIndexes[freeIndex][0] == x - 1 && freeIndexes[freeIndex][1] == y)
            {
                return true;
            }
        }
        else//other
        {
            if ((freeIndexes[freeIndex][0] == x - 1 && freeIndexes[freeIndex][1] == y) || (freeIndexes[freeIndex][0] == x && freeIndexes[freeIndex][1] == y - 1))
            {
                return true;
            }
        }

        return false;
    }

    public int getIteration(int level)
    {
        if (level == 1)
        {
            return Random.Range(2, 4);
        }
        else if (level == 2)
        {
            return Random.Range(4, 7);
        }
        else
        {
            return Random.Range(10, 15);
        }
    }

    public void getAvaivableCups(int[,] map)
    {
        availableIndexes.Clear();
        int indexX = (int)Mathf.Sqrt(map.Length) - 1;
        int indexY = 0;
        while (indexX != 0 || indexY != 0) //?????????? ?? ??????????? ????
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
            List<int> availableTemp = new List<int>();
            availableTemp.Add(indexX);
            availableTemp.Add(indexY);
            availableIndexes.Add(availableTemp);
        }
    }

    public int[,] createMap(int[,] map, int all)//??????? ?????? ??? ????? ????????????? ?????????? (???????)
    {
        for (int i = 0; i < all; i++)
        {
            getFreeIndexes(map);
            int number = Random.Range(0, freeIndexes.Count);
            map[freeIndexes[number][0], freeIndexes[number][1]] = Random.Range(1, 3);
            freeIndexes.RemoveAt(number);
        }

        return map;
    }

    public void getFreeIndexes(int[,] map)
    {
        freeIndexes.Clear();
        int indexX = (int)Mathf.Sqrt(map.Length) - 1;
        int indexY = 0;
        while (indexX != 0 || indexY != 0) //???? ?????????? ?? ??????????? ????
        {
            if (isFloor())
            {
                addFreeIndexes();
            }
            if (indexX < Mathf.Sqrt(map.Length) - 1 && indexY < Mathf.Sqrt(map.Length) - 1)
            {
                if (map[indexX + 1, indexY] > 0 && map[indexX, indexY + 1] > 0 && map[indexX, indexY] == 0)
                {
                    addFreeIndexes();
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

        bool isFloor()
        {
            if (indexX + indexY == (int)Mathf.Sqrt(map.Length) - 1 && map[indexX, indexY] == 0)
            {
                return true;
            }
            return false;
        }

        void addFreeIndexes()
        {
            List<int> freeTemp = new List<int>();
            freeTemp.Add(indexX);
            freeTemp.Add(indexY);
            freeIndexes.Add(freeTemp);
        }
    }

    public void initDictionary()
    {
        rules.Add(5, 9);
        rules.Add(8, 13);
        rules.Add(12, 18);
    }
}
