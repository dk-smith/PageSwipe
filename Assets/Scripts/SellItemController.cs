using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class SellItemController : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI starText;
    [SerializeField] private TextMeshProUGUI amountText;
    private Item item;

    public Item Item { get => item; set => item = value; }

    public void Fill(SellItemObject sellItem, Item[] items)
    {
        item = items[sellItem.item];
        itemImage.sprite = item.Image;
        itemName.text = item.Title;
        playerName.text = sellItem.playerName;
        priceText.text = (item.Price * sellItem.amount).ToString();
        starText.text = sellItem.playerLevel.ToString();
        amountText.text = "x" + sellItem.amount;
        StartCoroutine(GetImage(sellItem.playerImage));
    }


    IEnumerator GetImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D image = ((DownloadHandlerTexture)request.downloadHandler).texture;
            playerImage.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), Vector2.zero);
            playerImage.GetComponent<Animation>().Play();
            //playerImage.color.a = 1;
        }
    }
}