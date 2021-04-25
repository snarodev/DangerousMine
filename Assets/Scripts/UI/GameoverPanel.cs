using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverPanel : Panel
{
    public Image backgroundImage;
    public Text deathText;

    public GameObject respawnListPrefab;
    public Transform respawnListParent;

    public override void Open(PanelOpenInfo panelOpenInfo)
    {
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        deathText.color = Color.clear;
        backgroundImage.color = Color.clear;

        yield return new WaitForSeconds(0.5f);

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            backgroundImage.color = Color.Lerp(Color.clear, Color.white, i);

            yield return 0;
        }

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            deathText.color = Color.Lerp(Color.clear, Color.black, i);

            yield return 0;
        }

        for (int i = 0; i < PlayerInventory.playerInventory.checkPoints.Count; i++)
        {
            GameObject go = Instantiate(respawnListPrefab, respawnListParent);
            go.GetComponent<RespawnLocationButton>().Init(this, PlayerInventory.playerInventory.checkPoints[i], Mathf.Abs(Mathf.FloorToInt(PlayerInventory.playerInventory.checkPoints[i].y * 0.1f)) - 1);
        }
    }

    public void RespawnButtonClicked(Vector2 location, int price)
    {
        PlayerInventory.playerInventory.transform.position = location;
        PlayerInventory.playerInventory.goldAmount -= price;
        PlayerHealth.playerHealth.Revive();
        PlayerInventory.playerInventory.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        UIController.ui.CloseCurrentPanel();
    }
}
