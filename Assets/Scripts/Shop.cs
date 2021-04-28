using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    Transform player;

    public GameObject clickPrompt;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < 4)
        {
            clickPrompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Q))
            {
                ShopPanel shopPanel = UIController.ui.GetTopPanel<ShopPanel>();

                if (shopPanel == null)
                {
                    if (UIController.ui.GetOpenPanelAmount() == 0)
                    {
                        UIController.ui.OpenPanel<ShopPanel>(new ShopPanel.ShopPanelPanelOpenInfo(new int[3] { 1, 2, 3 }));
                    }
                }
                else
                {
                    UIController.ui.CloseCurrentPanel();
                }
            }


            PlayerInventory.playerInventory.AddCheckpoint(transform.position);
        }
        else
        {
            if (Vector2.Distance(transform.position, player.position) < 10)
            {
                ShopPanel shopPanel = UIController.ui.GetTopPanel<ShopPanel>();
                if (shopPanel != null)
                {
                    UIController.ui.CloseCurrentPanel();
                }
            }
                
            
            clickPrompt.SetActive(false);
        }
    }
}
