using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolCell : MonoBehaviour
{
    public Image image;

    public int inventoryId;

    public InventoryTool tool;
    InventoryPanel inventoryPanel;

    public void Init(InventoryTool tool, int inventoryId, InventoryPanel playerInventory)
    {
        this.tool = tool;
        this.inventoryId = inventoryId;
        this.inventoryPanel = playerInventory;

        image.sprite = ToolController.toolController.GetTool(tool.id).sprite;
    }

    public void ToolCellClicked()
    {
        inventoryPanel.ToolCellClicked(this);
    }
}
