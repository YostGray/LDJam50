using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//新版自动地砖
public class NewAutoTile : MonoBehaviour
{
    [SerializeField]
    private float sideLength;
    [SerializeField]
    //最左下角的那一块为起点
    private Transform basePoint;
    //是否需要合并碰撞体
    [SerializeField]
    private bool isCoillder;
    [SerializeField]
    private Sprite SpriteMid, SpriteLeft, SpriteRight, SpriteUp, SpriteDown,
                            SpriteUpLeft, SpriteUpRight, SpriteDownLeft, SpriteDownRight, SpriteUpDown, SpriteLeftRight,
                            SpriteOnlyOne, SpriteOnlyLeft, SpriteOnlyRight, SpriteOnlyUp, SpriteOnlyDown;
    [SerializeField]
    private GameObject conerLeftUp, conerRightUp, conerLeftDown, conerRightDown;


    private BaseTile[] allBaseTiles;
    private List<List<bool>> mapContainer = new List<List<bool>>();

    void Awake()
    {
        updateAllTile();
        //test();
    }

    //获取孩子中所有的tiles
    private void getAllTiles()
    {
        allBaseTiles = this.GetComponentsInChildren<BaseTile>(false);
        return;
    }

    //填入容器
    private void initinalizeContainer()
    {
        getAllTiles();
        foreach (BaseTile tile in allBaseTiles)
        {
            Vector2 pos = tile.transform.position;
            int x, y;
            x = (int)((pos.x - basePoint.position.x) / sideLength + 0.05f);
            y = (int)((pos.y - basePoint.position.y) / sideLength + 0.05f);

            while (mapContainer.Count <= x)
            {
                mapContainer.Add(new  List<bool>());
            }
            while (mapContainer[x].Count <= y)
            {
                mapContainer[x].Add(false);
            }
            mapContainer[x][y] = true;
        }
    }

    void updateAllTile()
    {
        initinalizeContainer();
        foreach (BaseTile tile in allBaseTiles)
        {
            Vector2 pos = tile.transform.position;
            int x, y, code = 0;
            x = (int)((pos.x - basePoint.position.x) / sideLength + 0.05f);
            y = (int)((pos.y - basePoint.position.y) / sideLength + 0.05f);

            if (isCoillder)
                changeCoillder2D(tile.transform, x, y);

            if (hasUp(x, y))
                code += 1;
            if (hasDown(x, y))
                code += 4;
            if (hasLeft(x, y))
                code += 8;
            if (hasRight(x, y))
                code += 2;
            tile.gameObject.GetComponent<SpriteRenderer>().sprite = findSpriteWithCode(code, tile.transform, x, y);
 
        }
    }

    protected bool hasUp(int x, int y)
    {
        if (x < 0 || y < 0)
            return false;
        if (!(mapContainer.Count > x) || mapContainer[x] == null)
            return false;
        if (mapContainer[x].Count == y + 1)
            return false;
        else
            return mapContainer[x][y + 1];
    }

    protected bool hasDown(int x, int y)
    {
        if (x < 0 || y < 1)
            return false;
        if (!(mapContainer.Count > x) || mapContainer[x] == null)
            return false;
        return mapContainer[x][y - 1];
    }

    protected bool hasLeft(int x, int y)
    {
        if (x < 1 || y < 0)
            return false;
        if (!(mapContainer.Count > x))
            return false;
        if (mapContainer[x - 1] == null || mapContainer[x - 1].Count < y + 1)
            return false;
        return mapContainer[x - 1][y];
    }

    protected bool hasRight(int x, int y)
    {
        if (x < 0 || y < 0)
            return false;
        if (!(mapContainer.Count > x + 1))
            return false;
        if (mapContainer[x + 1] == null || mapContainer[x + 1].Count < y + 1)
            return false;
        return mapContainer[x + 1][y];
    }

    protected bool hasLeftUp(int x, int y)
    {
        return hasLeft(x, y + 1);
    }

    protected bool hasRightUp(int x, int y)
    {
        return hasRight(x, y + 1);
    }

    protected bool hasLeftDown(int x, int y)
    {
        return hasLeft(x, y - 1);
    }

    protected bool hasRightDown(int x, int y)
    {
        return hasRight(x, y - 1);
    }

    void changeCoillder2D(Transform tra, int x, int y)
    {
        if (hasUp(x, y) && hasDown(x, y) && hasLeft(x, y) && hasRight(x, y))
        {
            tra.GetComponent<BoxCollider2D>().enabled = false;
            return;
        }
        else if (hasLeft(x, y))
        {
            tra.GetComponent<BoxCollider2D>().enabled = false;
            return;
        }
        else if (!hasLeft(x, y))
        {
            BoxCollider2D theBox = tra.GetComponent<BoxCollider2D>();
            int count = 1;
            while (hasRight(x + count - 1, y))
            {
                count++;
            }
            theBox.size = new Vector2(sideLength * count, sideLength);
            theBox.offset = new Vector2(sideLength / 2.0f * (count - 1), 0);
        }
        return;
    }

    void changeConer(Transform tra, int x, int y)
    {
        if (!hasLeftUp(x, y) && hasLeft(x, y) && hasUp(x, y))
            Instantiate(conerLeftUp, tra);
        if (!hasRightUp(x, y) && hasRight(x, y) && hasUp(x, y))
            Instantiate(conerRightUp, tra);
        if (!hasRightDown(x, y) && hasRight(x, y) && hasDown(x, y))
            Instantiate(conerRightDown, tra);
        if (!hasLeftDown(x, y) && hasLeft(x, y) && hasDown(x, y))
            Instantiate(conerLeftDown, tra);
    }

    Sprite findSpriteWithCode(int code, Transform tra, int x, int y)
    {
        switch (code)
        {
            case 0:
                return SpriteOnlyOne;
            case 1:
                return SpriteOnlyUp;
            case 2:
                return SpriteOnlyRight;
            case 3:
                changeConer(tra, x, y);
                return SpriteDownLeft;
            case 4:
                return SpriteOnlyDown;
            case 5:
                return SpriteUpDown;
            case 6:
                changeConer(tra, x, y);
                return SpriteUpLeft;
            case 7:
                changeConer(tra, x, y);
                return SpriteLeft;
            case 8:
                return SpriteOnlyLeft;
            case 9:
                changeConer(tra, x, y);
                return SpriteDownRight;
            case 10:
                return SpriteLeftRight;
            case 11:
                changeConer(tra, x, y);
                return SpriteDown;
            case 12:
                changeConer(tra, x, y);
                return SpriteUpRight;
            case 13:
                changeConer(tra, x, y);
                return SpriteRight;
            case 14:
                changeConer(tra, x, y);
                return SpriteUp;
            case 15:
                changeConer(tra, x, y);
                return SpriteMid;
            default:
                changeConer(tra, x, y);
                return SpriteMid;
        }
    }

    protected void test()
    {
        if (mapContainer.Count == 0)
        {
            Debug.Log("TilesEmpty!");
        }

        foreach (List<bool> arraylist in mapContainer)
        {
            string output = null;
            foreach (var e in arraylist)
            {
                if (e)
                    output += "■";
                else
                    output += "□";
            }
            Debug.Log(output);
        }
    }
}
