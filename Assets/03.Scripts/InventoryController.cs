using UnityEngine;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    public ItemGrid selectItemGrid;

    private InventoryItem selectedItem;
    private InventoryItem overlapItem;
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
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        int selectedItemId = Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemId]);
    }

    private void LeftMouseButtonPress()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.itemData.width - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.itemData.height - 1) * ItemGrid.tileSizeHeight / 2;
        }

        Vector2Int tileGridPosition = selectItemGrid.GetTileGridPosition(position);    //그리드 셀 위치
        Debug.Log($"선택한 Cell 위치: {selectItemGrid.GetTileGridPosition(position)}");

        if (selectedItem == null) { PickUpItem(tileGridPosition); }
        else { PlaceItem(tileGridPosition); }
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = selectItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);   //아이템 배치
        
        if (complete)
        {
            selectedItem = null;
            if(overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
            }
        }
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y); //아이템 선택

        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }
}
