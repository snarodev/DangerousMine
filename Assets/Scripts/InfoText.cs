using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour
{
    public static InfoText infoText;

    public Text messageText;

    private void Start()
    {
        infoText = this;
    }

    public void DisplayMessage(string message)
    {
        messageText.color = Color.black;
        messageText.text = message;

        StopAllCoroutines();

        StartCoroutine(TextDisplay());
    }

    IEnumerator TextDisplay()
    {
        yield return new WaitForSeconds(2);

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            messageText.color = Color.Lerp(Color.black, Color.clear, i);

            yield return 0;
        }
        messageText.color = Color.clear;

    }
}
