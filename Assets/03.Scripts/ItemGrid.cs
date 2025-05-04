using System;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    //타일 사이즈
    public const float tileSizeWidth = 100;
    public const float tileSizeHeight = 100;

    private InventoryItem[,] inventoryItemSlot;

    private RectTransform rectTransform;

    //그리드 사이즈
    [SerializeField] private int gridSizeWidth;
    [SerializeField] private int gridSizeHeight;

    private Vector2 positionOnTheGrid = new Vector2(); //월드 기준 인벤토리 그리드상의 마우스 위치
    private Vector2Int tileGridPosition = new Vector2Int(); //타일 기준 인벤토리 그리드상의 마우스 위치

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
    }


    /// <summary>
    /// 그리드 크기 초기화
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    /// <summary>
    /// 그리드 내의 타일 위치 검사
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <returns></returns>
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    /// <summary>
    /// 아이템 선택
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null) { return null; }

        CleanGridReference(toReturn);

        return toReturn;
    }

    private void CleanGridReference(InventoryItem item)
    {
        for (int ix = 0; ix < item.itemData.width; ix++)
        {
            for (int iy = 0; iy < item.itemData.height; iy++)
            {
                inventoryItemSlot[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
    }

    /// <summary>
    /// 아이템 배치
    /// </summary>
    /// <param name="inventoryItem">배치할 아이템</param>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
    {
        if(BoundryCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.height) == false) { return false; }
        if (OverlapCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.height, ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }
        if(overlapItem != null) { CleanGridReference(overlapItem); }


        RectTransform rect = inventoryItem.GetComponent<RectTransform>();
        rect.SetParent(this.transform);

        for (int x = 0; x < inventoryItem.itemData.width; x++)
        {
            for (int y = 0; y < inventoryItem.itemData.height; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
                Debug.Log($"배치된 좌표: {posX + x},{posY + y}");
            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.itemData.width / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.itemData.height / 2);

        rect.localPosition = position;

        return true;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(inventoryItemSlot[posX + x, posY + y] != null)
                {
                    if(overlapItem == null)
                    {
                        overlapItem = inventoryItemSlot[posX + x, posY + y];
                    }
                    else
                    {
                        if(overlapItem != inventoryItemSlot[posX + x, posY + y])
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    private bool PositionCheck(int posX, int posY)
    {
        if(posX < 0 || posY < 0)
        {
            return false;
        }

        if(posX >= gridSizeWidth ||  posY >= gridSizeHeight)
        {
            return false;
        }

        return true;
    }

    private bool BoundryCheck(int posX, int posY, int width, int height)
    {
        if(PositionCheck(posX, posY) == false) { return false; }

        posX += width - 1;
        posY += height - 1;

        if(PositionCheck(posX, posY) == false ) { return false; }

        return true;
    }
}
