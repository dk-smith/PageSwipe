using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SellItem", menuName = "ScriptableObjects/SellItem", order = 1)]
public class Item : ScriptableObject
{
    static private int count = 0;
    [SerializeField] private string title;
    [SerializeField] private Sprite image;
    [SerializeField] private int price;

    public int Id { get; } = count++;

    public string Title => title;

    public Sprite Image => image;

    public int Price { get => price; set => price = value; }
}
