using UnityEngine;
using System;

//Attached to the Card prefab

public class Card : MonoBehaviour
{
    public int index;
    public Action<int> onCardClicked;

    //This is the cards index/position on the game board
    public void Initialize(int arrayIndex)
    {
        index = arrayIndex;
    }

    void OnMouseDown()
    {
        onCardClicked?.Invoke(index);
    }
}