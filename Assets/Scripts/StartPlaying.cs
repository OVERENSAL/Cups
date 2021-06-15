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
    public Text diffText;
    public Text levelText;
    public Text time;
    public SpriteRenderer cupRenderer;
    public SpriteRenderer fullCupRenderer;
    public Sprite cupSprite;
    public Sprite fullCupSprite;
    public Sprite vineSprite;
    public Sprite vineEmptySprite;
    public Sprite glassSprite;
    public Sprite waterGlassSprite;
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
    private float second = 1;
    private bool timeIsStop = false;

    void Start()
    {
        cupRenderer.sprite = cupSprite;
        fullCupRenderer.sprite = fullCupSprite;
        getSprite();
        
        second = getLevelDiff() * 10;
        time.text = "0:" + second;
        diffText.text = getText();
        levelText.text = "Task " + (LevelCounter.levelCount + 1);
        panel.SetActive(false);
        string[] args = new string[] { };
        creator.Main(args);
        cupCoords = new CupCoords[CreateCups.width, CreateCups.width];
        map = new int[CreateCups.width, CreateCups.width];
        map = creator.createMap(map, CreateCups.allCupsNumber);
        creator.getAvaivableCups(map);
        target = new int[CreateCups.width, CreateCups.width];
        target = creator.shuffle(getLevelDiff(), target, map);

        createCups(map, cupCoords, true);
        initTargetGrid();
        createCups(target, targetGrid1, targetGrid2, targetGrid3, targetGrid4, false);
    }

    void getSprite() {
        if (getLevelDiff() == 2) {
            cupRenderer.sprite = vineEmptySprite;
            fullCupRenderer.sprite = vineSprite;
        } else if (getLevelDiff() == 2) {
            cupRenderer.sprite = glassSprite;
            fullCupRenderer.sprite = waterGlassSprite;
        }
    }

    bool targetIsEqualStart() {
        if (target == null) {
            print("sd");
            return true;
        }
        for(int i = 0; i < CreateCups.width; i++)
        {
            for (int j = 0; j < CreateCups.width; j++)
            {
                if (map[i, j] != target[i, j])
                {   
                    print("false");
                    return false;
                }
            }
        }
        print("hi i am oldman");
        return false;
    }

    int getLevelDiff() {
        if (LevelCounter.levelCount < 5) {
            return 1;
        } else if (LevelCounter.levelCount < 10) {
            return 2;
        } else {
            return 3;
        }
    }

    string getText() {
        if (getLevelDiff() == 1) {
            return "Easy";
        } else if (getLevelDiff() == 2) {
            return "Medium";
        } else {
            return "Hard";
        }
    }

    void createCups(int[,] map, GridLayoutGroup grid1, GridLayoutGroup grid2, GridLayoutGroup grid3, GridLayoutGroup grid4, bool size)//мелкие приколы
    {
        int indexX = (int)Mathf.Sqrt(map.Length) - 1;
        int indexY = 0;
        while (indexX != 0 || indexY != 0)
        {
            //создаем прикол в позиции грида
            GameObject gameObject = Instantiate(emptyOrFull(map[indexX, indexY], size), getGrid(indexX, indexY, grid1, grid2, grid3, grid4).transform.position, Quaternion.identity);
            gameObject.transform.SetParent(getGrid(indexX, indexY, grid1, grid2, grid3, grid4).transform, false);
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

            if (indexY == 0)//инициализация сдвига, когда стаканов меньше 7
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
    }//помещаем прикол на главный экран

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

    string getZero(float second) {
        if (second < 10) {
            return "0";
        }
        return "";
    }

    // Update is called once per frame
    void Update()
    {
        if (!timeIsStop) {
            second -= Time.deltaTime;
            time.text = "0:" + getZero(Mathf.Round(second)) + Mathf.Round(second);
            if (Mathf.Round(second) < 5) {
                time.color = new Color(255, 0, 0);
            }
        }
        
        if (isWin)
        {
            GameObject firework = Instantiate(fireWork);
            firework.transform.position = new Vector3(firework.transform.position.x, firework.transform.position.y, 0);
            GameObject firework2 = Instantiate(fireWork2);
            firework2.transform.position = new Vector3(firework2.transform.position.x, firework2.transform.position.y, 0);
            Invoke("showPanel", 1.5f);
            LevelCounter.levelCount++;
            timeIsStop = true;
            isWin = false;
        }
    }
}
