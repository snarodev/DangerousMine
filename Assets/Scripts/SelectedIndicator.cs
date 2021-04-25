using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedIndicator : MonoBehaviour
{
    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (UIController.ui != null && UIController.ui.GetTopPanel() != null && UIController.ui.GetTopPanel().GetType() == typeof (InventoryPanel))
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                image.enabled = false;
            }
            else
            {
                image.enabled = true;
                transform.position = Vector3.Lerp(transform.position, EventSystem.current.currentSelectedGameObject.transform.position, 0.2f);
            }
        }
        else
        {
            image.enabled = false;
        }
    }
}
