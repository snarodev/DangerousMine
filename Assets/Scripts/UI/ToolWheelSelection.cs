using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolWheelSelection : Panel
{
    public Image[] wheelToolSlots;

    public int[] wheelToolIds;

    public Image handTool;

    int inventoryId;

    public class ToolWheelSelectionPanelOpenInfo : PanelOpenInfo
    {
        public int inventoryId;

        public ToolWheelSelectionPanelOpenInfo(int inventoryId)
        {
            this.inventoryId = inventoryId;
        }
    }

    public override void Open(PanelOpenInfo panelOpenInfo)
    {
        ToolWheelSelectionPanelOpenInfo info = (ToolWheelSelectionPanelOpenInfo)panelOpenInfo;
        inventoryId = info.inventoryId;
        
        wheelToolIds = PlayerInventory.playerInventory.toolWheelIds;
        for (int i = 0; i < wheelToolIds.Length; i++)
        {
            int wheelId = wheelToolIds[i];

            if (wheelId == -1)
            {
                // Slot empty
                wheelToolSlots[i].color = Color.white;
            }
            else
            {
                int id = PlayerInventory.playerInventory.inventoryTools[wheelId].id;
                wheelToolSlots[i].sprite = ToolController.toolController.GetTool(id).sprite;
            }
        }

        handTool.sprite = ToolController.toolController.GetTool(PlayerInventory.playerInventory.inventoryTools[info.inventoryId].id).sprite;
    }

    private void Update()
    {
        handTool.transform.position = Input.mousePosition;
    }

    public void ToolSelectionWheelButton(int slotId)
    {
        //PlayerInventory.playerInventory.currentToolId = 0;

        PlayerInventory.playerInventory.toolWheelIds[slotId] = inventoryId;

        UIController.ui.CloseCurrentPanel();
    }

    public void ToolSelectionWheelButtonCancel()
    {
        UIController.ui.CloseCurrentPanel();
    }
}


public abstract class PanelOpenInfo
{
    
}