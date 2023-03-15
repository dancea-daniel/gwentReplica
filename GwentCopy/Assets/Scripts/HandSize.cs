using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandSize : MonoBehaviour
{
    

    public void ResizeHand()
    {
        //Debug.Log("Resizing your hand V2...");
        // Initial Spacing
        gameObject.GetComponent<GridLayoutGroup>().spacing = new Vector2(95, 0);

        float maxWidth = gameObject.GetComponent<RectTransform>().rect.width;
        float spacingX = gameObject.GetComponent<GridLayoutGroup>().spacing.x;
        int numberOfCards = 0;
        float cardWidth = 0;

        //Debug.Log("Current number of children: " + transform.childCount);
        foreach (Transform child in transform)
        {
            numberOfCards += 1;
            RectTransform rt = (RectTransform)child.transform;
            cardWidth = rt.rect.width;
        }

        float handWidth = numberOfCards * cardWidth + (numberOfCards - 1) * spacingX;
        float offset = 50;
        if (handWidth > maxWidth - offset)
        {
            spacingX = (maxWidth - offset - numberOfCards * cardWidth) / numberOfCards - 1;
            gameObject.GetComponent<GridLayoutGroup>().spacing = new Vector2(spacingX, 0);
        }

    }
}
