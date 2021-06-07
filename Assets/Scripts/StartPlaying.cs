using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct CupCoords
{
    public int x;
    public int y;

    public CupCoords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class StartPlaying : MonoBehaviour
{
    public Transform trayPos;
    public GameObject cup;
    public GameObject fullCup;
    public GameObject emptyCup;
    public GameObject smallCup;
    public GameObject smallFullCup;
    public GameObject smallEmptyCup;
    public GridLayoutGroup grid1;
    public GridLayoutGroup grid2;
    public GridLayoutGroup grid3;
    public GridLayoutGroup grid4;
    public GridLayoutGroup targetGrid1;
    public GridLayoutGroup targetGrid2;
    public GridLayoutGroup targetGrid3;
    public GridLayoutGroup targetGrid4;
    public List<GameObject> cups = new List<GameObject>();
    public List<GameObject> targetCups = new List<GameObject>();
    public List<List<int>> cupsIndexes = new List<List<int>>();
    public List<GridLayoutGroup> grids = new List<GridLayoutGroup>();
    public Vector3 coords = new Vector3(-205, 75, 0);

    CreateCups creator = new CreateCups();
    public CupCoords[,] cupsCoords;

    void Start()
    {
        string[] args = new string[] { };
        creator.Main(args);
        cupsCoords = new CupCoords[CreateCups.width, CreateCups.width];
        int[,] map = new int[CreateCups.width, CreateCups.width];
        int[,] target = new int[CreateCups.width, CreateCups.width];
        map = creator.createMap(map, CreateCups.allCupsNumber);
        creator.getAvaivableCups(map);
        target = creator.shuffle(1, target, map);

        initGrid();
        //createCups(map, grid1, grid2, grid3, grid4, true);
        createCups(map, true);
        fillArrayOfCoords();
        initTargetGrid();
        createCups(target, targetGrid1, targetGrid2, targetGrid3, targetGrid4, false);

    }

    void fillArrayOfCoords()
    {
        GameObject[] tempCups;
        tempCups = GameObject.FindGameObjectsWithTag("Cup");
        for(int i = 0; i < tempCups.Length; i++)
        {
            Debug.Log(tempCups[0].transform.position);
        }
        int indexX = CreateCups.width - 1;
        int indexY = 0;
        for(int i = 0; i < 4; i++)
        {
            GridLayoutGroup tmpGrid = getGrid(indexX, indexY, grid1, grid2, grid3, grid4);
            int gridObjectIterator = 0;
            /*Transform[] cups = tmpGrid.GetComponents<Transform>();//возврат грида
            Debug.Log(cups.Length + " length");
            for(int j = 0; j < cups[0].childCount; j++)
            {
                Debug.Log(cups[0].GetChild(j).localPosition);
            }*/
            //Debug.Log(cups[0].childCount + "childcount");
            while (indexX != -1)
            {
                //cupsCoords[indexX, indexY] = getCupCoords(tmpGrid.transform.GetChild(gridObjectIterator));
                //Debug.Log(tmpGrid.transform.GetChild(gridObjectIterator).position + " " + indexX + " " + indexY);
                //Debug.Log(tmpGrid.GetComponent<Transform>().position);
                indexX--;
                indexY++;
                gridObjectIterator++;
            }
            //Debug.Log("next");
            indexX = CreateCups.width - i - 2;
            indexY = 0;
        }
    }

    CupCoords getCupCoords(Transform transform)
    {
        return new CupCoords((int)transform.position.x, (int)transform.position.x);
    }

    void createCups(int[,] map, GridLayoutGroup grid1, GridLayoutGroup grid2, GridLayoutGroup grid3, GridLayoutGroup grid4, bool size)
    {
        int indexX = (int)Mathf.Sqrt(map.Length) - 1;
        int indexY = 0;
        while (indexX != 0 || indexY != 0)
        {
            //создаем прикол в позиции грида
            GameObject gameObject = Instantiate(emptyOrFull(map[indexX, indexY], size), getGrid(indexX, indexY, grid1, grid2, grid3, grid4).transform.position, Quaternion.identity);
            gameObject.transform.SetParent(getGrid(indexX, indexY, grid1, grid2, grid3, grid4).transform, false);
            //Debug.Log(gameObject.transform.position);
            cups.Add(gameObject);
            if (map[indexX, indexY] > 0)
            {
                List<int> temp = new List<int> { indexX, indexY };
                cupsIndexes.Add(temp);
            }
            indexX--;
            indexY++;
            if (indexX < 0)
            {
                indexX = indexY - 2;
                indexY = 0;
            }
        }
    }

    GridLayoutGroup getGrid(int indexX, int indexY, GridLayoutGroup grid1, GridLayoutGroup grid2, GridLayoutGroup grid3, GridLayoutGroup grid4)
    {
        if (indexX + indexY == CreateCups.width - 1)
            return grid1;
        else if (indexX + indexY == CreateCups.width - 2)
            return grid2;
        else if (indexX + indexY == CreateCups.width - 3)
            return grid3;
        else 
            return grid4;

    } //вернет грид, куда нуно вставить прикол

    GameObject emptyOrFull(int number, bool size)
    {
        if(number == 0)
        {
            if (size)
                return emptyCup;
            return smallEmptyCup;
        }
        else if (number == 1)
        {
            if (size)
                return cup;
            return smallCup;
        }

        if (size)
            return fullCup;
        return smallFullCup;
    }//вернет пустой, полный либо прозрачный прикол

    void initGrid()
    {
        grids.Add(grid1);
        grids.Add(grid2);
        grids.Add(grid3);
        grids.Add(grid4);
        for(int i = 0; i < 4; i++)
        {
            grids[i].constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grids[i].constraintCount = CreateCups.width - i;
        }
    }//мем

    void initTargetGrid()
    {
        targetGrid1.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        targetGrid1.constraintCount = CreateCups.width;
        targetGrid2.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        targetGrid2.constraintCount = CreateCups.width - 1;
        targetGrid3.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        targetGrid3.constraintCount = CreateCups.width - 2;
        targetGrid4.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        targetGrid4.constraintCount = CreateCups.width - 3;
    }//инициализация количества колонок в целевом гриде

    void createCups(int[,] map, bool size)
    {
        Vector3 addCoords = new Vector3(0, 0, 0);
        int indexX = CreateCups.width - 1;
        int indexY = 0;
        while (indexX != 0 || indexY != 0)
        {
            if (indexY == 0 && indexX != CreateCups.width - 1)
            {
                addCoords.x += 29;
                addCoords.y += 92;
            }

            if (indexY == 0)//инициализация сдвига, когда стаканов меньше 7
            {
                for (int i = 0; i < (7 - indexX); i++)
                {
                    addCoords.x += 29;
                }
            }
            GameObject gameObject = Instantiate(emptyOrFull(map[indexX, indexY], size), trayPos.position + coords + addCoords, Quaternion.identity);
            addCoords.x += 58;
            //Debug.Log(gameObject.transform.position);
            cups.Add(gameObject);
            if (map[indexX, indexY] > 0)
            {
                List<int> temp = new List<int> { indexX, indexY };
                cupsIndexes.Add(temp);
            }
            indexX--;
            indexY++;
            if (indexX < 0)
            {
                addCoords.x = -29;              
                indexX = indexY - 2;
                indexY = 0;
            }

        }
    }//помещаем прикол на главный экран

    // Update is called once per frame
    void Update()
    {
        
    }
}
