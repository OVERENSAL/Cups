using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPlaying : MonoBehaviour
{
    public Transform trayPos;
    public GameObject cup;
    public GameObject fullCup;
    public GameObject emptyCup;
    public GameObject smallCup;
    public GameObject smallFullCup;
    public GameObject smallEmptyCup;
    public GameObject fireWork;
    public GameObject fireWork2;
    public GameObject panel;
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
    public static bool isWin;
    public static CupCoords[,] cupCoords;
    public static int[,] map;
    public static int[,] target;

    void Start()
    {
        GameObject.FindWithTag("Panel").SetActive(false);
        string[] args = new string[] { };
        creator.Main(args);
        cupCoords = new CupCoords[CreateCups.width, CreateCups.width];
        map = new int[CreateCups.width, CreateCups.width];
        target = new int[CreateCups.width, CreateCups.width];
        map = creator.createMap(map, CreateCups.allCupsNumber);
        creator.getAvaivableCups(map);
        target = creator.shuffle(DiffHandler.levelDifficult, target, map);

        createCups(map, cupCoords, true);
        initTargetGrid();
        createCups(target, targetGrid1, targetGrid2, targetGrid3, targetGrid4, false);
    }

    void createCups(int[,] map, GridLayoutGroup grid1, GridLayoutGroup grid2, GridLayoutGroup grid3, GridLayoutGroup grid4, bool size)//������ �������
    {
        int indexX = (int)Mathf.Sqrt(map.Length) - 1;
        int indexY = 0;
        while (indexX != 0 || indexY != 0)
        {
            //������� ������ � ������� �����
            GameObject gameObject = Instantiate(emptyOrFull(map[indexX, indexY], size), getGrid(indexX, indexY, grid1, grid2, grid3, grid4).transform.position, Quaternion.identity);
            gameObject.transform.SetParent(getGrid(indexX, indexY, grid1, grid2, grid3, grid4).transform, false);
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

    } //������ ����, ���� ���� �������� ������

    GameObject emptyOrFull(int number, bool size)
    {
        if(number == 0)
        {
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
    }//������ ������, ������ ���� ���������� ������

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
    }//������������� ���������� ������� � ������� �����

    void createCups(int[,] map, CupCoords[,] cupCoords, bool size)
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

            if (indexY == 0)//������������� ������, ����� �������� ������ 7
            {
                for (int i = 0; i < (7 - indexX); i++)
                {
                    addCoords.x += 29;
                }
            }

            Vector3 mainCoords = trayPos.position + coords + addCoords;
            if (map[indexX, indexY] != 0) {
                GameObject gameObject = Instantiate(emptyOrFull(map[indexX, indexY], size), mainCoords, Quaternion.identity);
            }
            addCoords.x += 58;
            cupCoords[indexX, indexY] = new CupCoords(mainCoords.x, mainCoords.y);
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
    }//�������� ������ �� ������� �����

    public static CupCoords[,] getCupCoords()
    {
        return cupCoords;
    }

    public static int[,] getMap()
    {
        return map;
    }

    public static int[,] getTargetMap()
    {
        return target;
    }

    public void showPanel()
    {
        panel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isWin)
        {
            GameObject firework = Instantiate(fireWork);
            firework.transform.position = new Vector3(firework.transform.position.x, firework.transform.position.y, 0);
            GameObject firework2 = Instantiate(fireWork2);
            firework2.transform.position = new Vector3(firework2.transform.position.x, firework2.transform.position.y, 0);
            Invoke("showPanel", 1.5f);
            isWin = false;
        }
    }
}
