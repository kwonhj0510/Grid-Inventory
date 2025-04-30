using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    //타일 사이즈
    private const float tileSizeWidth = 100;
    private const float tileSizeHeight = 100;

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
    /// 아이템 선택
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];
        inventoryItemSlot[x, y] = null;

        return toReturn;
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
    /// 아이템 배치
    /// </summary>
    /// <param name="inventoryItem">배치할 아이템</param>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        inventoryItemSlot[posX, posY] = inventoryItem;

        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight / 2);

        rectTransform.localPosition = position;
    }
}
