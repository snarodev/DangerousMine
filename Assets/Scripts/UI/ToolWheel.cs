using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolWheel : Panel
{
    Vector2 firstPos;

    public Image[] wheelToolSlots;

    public Image innerCircle;

    public int[] wheelToolIds;

    int newToolId = 0;

    public override void Open(PanelOpenInfo panelOpenInfo)
    {
        firstPos = Input.mousePosition;

        wheelToolIds = PlayerInventory.playerInventory.toolWheelIds;

        newToolId = PlayerInventory.playerInventory.currentWheelSlot;

        for (int i = 0; i < wheelToolIds.Length; i++)
        {
            int wheelId = wheelToolIds[i];
           
            if (wheelId == -1)
            {
                // Slot empty
                wheelToolSlots[i].sprite = null;
                wheelToolSlots[i].color = Color.clear;

                wheelToolSlots[i].transform.Find("ToolDurabilityBackground").gameObject.SetActive(false);
                wheelToolSlots[i].transform.Find("ToolDurability").gameObject.SetActive(false);
            }
            else
            {
                if (PlayerInventory.playerInventory.currentWheelSlot == i)
                    wheelToolSlots[i].color = Color.green;
              
                int id = PlayerInventory.playerInventory.inventoryTools[wheelId].id;
                wheelToolSlots[i].sprite = ToolController.toolController.GetTool(id).sprite;

                if (ToolController.toolController.GetTool(id).maxDurability == 0)
                {
                    wheelToolSlots[i].transform.Find("ToolDurabilityBackground").gameObject.SetActive(false);
                    wheelToolSlots[i].transform.Find("ToolDurability").gameObject.SetActive(false);
                }
                else
                {
                    wheelToolSlots[i].transform.Find("ToolDurability").GetComponent<Image>().fillAmount = PlayerInventory.playerInventory.inventoryTools[wheelId].durability / (float)ToolController.toolController.GetTool(id).maxDurability;
                }

            }
        }

        transform.position = Input.mousePosition;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (newToolId != -1)
            {
                PlayerInventory.playerInventory.currentWheelSlot = newToolId;
            }


            UIController.ui.CloseCurrentPanel();
        }

        float distance = Vector2.Distance(firstPos, Input.mousePosition);

        if (distance > innerCircle.rectTransform.rect.size.x * 0.5f)
        {
            Vector2 delta = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - firstPos;

            delta.Normalize();

            innerCircle.color = new Color(1, 1, 1, 0.8f);

            
            wheelToolSlots[0].color = wheelToolIds[0] == -1 ? Color.clear : Color.white;
            wheelToolSlots[1].color = wheelToolIds[1] == -1 ? Color.clear : Color.white;
            wheelToolSlots[2].color = wheelToolIds[2] == -1 ? Color.clear : Color.white;
            wheelToolSlots[3].color = wheelToolIds[3] == -1 ? Color.clear : Color.white;


            if (delta.y > 0 && delta.y > Mathf.Abs(delta.x))    // Top
            {
                if (wheelToolIds[0] == -1)
                {
                    wheelToolSlots[0].color = Color.red;
                }
                else
                {
                    wheelToolSlots[0].color = new Color(0.5f, 0.5f, 0.5f, 1);
                    newToolId = 0;
                }
                
            }
            else if (delta.x > 0 && delta.x > Mathf.Abs(delta.y))  // Right
            {
                if (wheelToolIds[1] == -1)
                {
                    wheelToolSlots[1].color = Color.red;
                }
                else
                {
                    wheelToolSlots[1].color = new Color(0.5f, 0.5f, 0.5f, 1);
                    newToolId = 1;
                }
                
            }
            else if (delta.y < 0 && Mathf.Abs(delta.y) > Mathf.Abs(delta.x))   // Bottom
            {
                if (wheelToolIds[2] == -1)
                {
                    wheelToolSlots[2].color = Color.red;
                }
                else
                {
                    wheelToolSlots[2].color = new Color(0.5f, 0.5f, 0.5f, 1);
                    newToolId = 2;
                }
                
            }
            else if (delta.x < 0 && Mathf.Abs(delta.x) > Mathf.Abs(delta.y))  //Left
            {
                if (wheelToolIds[3] == -1)
                {
                    wheelToolSlots[3].color = Color.red;
                }
                else
                {
                    wheelToolSlots[3].color = new Color(0.5f, 0.5f, 0.5f, 1);
                    newToolId = 3;
                }
                
            }
        }
        else
        {
            innerCircle.color = new Color(1, 1, 1, 1);
        }
    }
}
