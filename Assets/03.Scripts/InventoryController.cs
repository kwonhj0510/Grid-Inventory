using UnityEngine;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    public ItemGrid selectItemGrid;

    private InventoryItem selectItem;
    RectTransform rectTransform;

    [SerializeField] private List<ItemData> items;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform canvasTransform;
    
    private void Update()
    {
        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if (selectItemGrid == null) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab, canvasTransform).GetComponent<InventoryItem>();
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        selectItem = inventoryItem;

        int selectedItemId = Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemId]);
    }

    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition = (selectItemGrid.GetTileGridPosition(Input.mousePosition));    //�׸��� �� ��ġ
        Debug.Log($"������ Cell ��ġ: {selectItemGrid.GetTileGridPosition(Input.mousePosition)}");
        if (selectItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        selectItemGrid.PlaceItem(selectItem, tileGridPosition.x, tileGridPosition.y);   //������ ��ġ
        selectItem = null;
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectItem = selectItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y); //������ ����

        if (selectItem != null)
        {
            rectTransform = selectItem.GetComponent<RectTransform>();
        }
    }

    private void ItemIconDrag()
    {
        if (selectItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }
}
