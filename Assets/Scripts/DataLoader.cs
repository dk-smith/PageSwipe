using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DataLoader : MonoBehaviour
{
    [SerializeField] private GameObject sellItemPanel;
    private SellItemObject[] list;

    public LinkedList<SellItemObject[]> LoadPages()
    {
        LinkedList<SellItemObject[]> result = new LinkedList<SellItemObject[]>();
        TextAsset json = Resources.Load<TextAsset>("sellData");
        if (json != null)
        { 
            list = JsonUtility.FromJson<SellItemListObject>(json.text).sellItems;
            SellItemObject[] array = new SellItemObject[6];
            int i = 0;
            foreach (var obj in list)
            {
                array[i++] = obj;
                if (i > 5)
                {
                    result.AddLast(array);
                    i = 0;
                    array = new SellItemObject[6];
                }
            }
            result.AddLast(array);
            GetComponent<PageSlide>();
        }
        else Debug.Log("Data not found!");
        return result;
    }

}
