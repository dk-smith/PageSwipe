using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class SellItemListObject
{
    public SellItemObject[] sellItems;

}

[Serializable]
public class SellItemObject
{
    public int id;
    public string playerImage;
    public string playerName;
    public int playerLevel;
    public int item;
    public int amount;
    
}
