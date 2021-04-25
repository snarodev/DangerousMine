using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnLocationButton : MonoBehaviour
{
    public Text buttonText;

    GameoverPanel gameoverPanel;
    Vector2 respawnLocation;
    int price;

    public void Init(GameoverPanel gameoverPanel, Vector2 respawnLocation, int price)
    {
        if (PlayerInventory.playerInventory.goldAmount < price)
        {
            buttonText.color = Color.red;
        }

        this.gameoverPanel = gameoverPanel;
        this.respawnLocation = respawnLocation;
        this.price = price;
        buttonText.text = "Respawn at " + respawnLocation.y + " for " + price + "$";
    }

    public void ButtonClicked()
    {
        if (PlayerInventory.playerInventory.goldAmount >= price)
        {
            gameoverPanel.RespawnButtonClicked(respawnLocation, price);
        }
    }
}
