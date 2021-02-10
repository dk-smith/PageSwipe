using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PageSlide : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{

    [SerializeField] private GameObject pagePanel;
    [SerializeField] private GameObject sellItemPanel;
    [SerializeField] private Item[] items;
    private RectTransform pageHolderRect;
    private RectTransform currentPageRect;
    private RectTransform newPageRect;
    private Coroutine swipeLock;
    private sbyte direction;
    private LinkedListNode<SellItemObject[]> currentPageNode;
    private LinkedList<SellItemObject[]> pagesList;

    void Start()
    {
        pageHolderRect = GetComponent<RectTransform>();
        pagesList = GetComponent<DataLoader>().LoadPages();
        currentPageNode = pagesList.First;
        currentPageRect = FillPage(Instantiate(pagePanel, pageHolderRect).transform as RectTransform, currentPageNode.Value);
    }

    public void OnBeginDrag(PointerEventData data)
    {
        if (swipeLock != null) return;
        direction = (sbyte)(data.delta.x > 0 ? 1 : -1);
        if (EndOfList) return;
        if (newPageRect == null) newPageRect = NewPage();
    }

    public void OnDrag(PointerEventData data)
    {
        if (swipeLock != null || EndOfList) return;
        float delta = data.pressPosition.x - data.position.x;
        float newPageX = newPageRect.anchoredPosition.x + data.delta.x;
        float currentPageX = currentPageRect.anchoredPosition.x + data.delta.x;
        if ((Mathf.Abs(newPageX) >= pageHolderRect.rect.width) || (direction == 1 ? (newPageX > 0) : (newPageX < 0))) return;
        newPageRect.anchoredPosition = new Vector2(newPageX, 0);
        currentPageRect.anchoredPosition = new Vector2(currentPageX, 0);
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (swipeLock != null || EndOfList) return;
        if (Mathf.Abs(data.pressPosition.x - data.position.x) < 100)
            swipeLock = StartCoroutine(CancelSwipe());
        else Swipe(direction);
    }

    void Update()
    {
        if (swipeLock == null && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Swipe(1);
        } else if (swipeLock == null && Input.GetKeyDown(KeyCode.RightArrow))
        {
            Swipe(-1);
        }
    }

    public void Swipe(int newDirection)
    {
        if (swipeLock == null)
        {
            direction = (sbyte)newDirection;
            if (EndOfList) return;
            if (newPageRect == null) newPageRect = NewPage();
            swipeLock = StartCoroutine(SwipeMoving());
        }
    }

    private IEnumerator SwipeMoving()
    {
        float delta = 0;
        do
        {
            delta += 5f * Time.deltaTime;
            currentPageRect.anchoredPosition = Vector2.Lerp(currentPageRect.anchoredPosition, new Vector2(direction * pageHolderRect.rect.width, 0), delta);
            newPageRect.anchoredPosition = Vector2.Lerp(newPageRect.anchoredPosition, new Vector2(0, 0), delta);
            yield return null;
        } while (delta < 1);
        Destroy(currentPageRect.gameObject);
        currentPageRect = newPageRect;
        newPageRect = null;
        currentPageNode = direction == -1 ? currentPageNode.Next : currentPageNode.Previous;
        swipeLock = null;
    }

    IEnumerator CancelSwipe()
    {
        float delta = 0;
        do
        {
            delta += 5f * Time.deltaTime;
            currentPageRect.anchoredPosition = Vector2.Lerp(currentPageRect.anchoredPosition, new Vector2(0, 0), delta); //Mathf.SmoothStep(0f, 1f,
            newPageRect.anchoredPosition = Vector2.Lerp(newPageRect.anchoredPosition, new Vector2(-direction * pageHolderRect.rect.width, 0), delta);
            yield return null;
        } while (delta < 1);
        Destroy(newPageRect.gameObject);
        newPageRect = null;
        swipeLock = null;
    }

    RectTransform NewPage()
    {
        RectTransform newPageRect = Instantiate(pagePanel, pageHolderRect).transform as RectTransform;
        newPageRect.anchoredPosition = new Vector2(-1 * direction * pageHolderRect.rect.width, 0);
        return FillPage(newPageRect, direction == 1 ? currentPageNode.Previous.Value : currentPageNode.Next.Value);
    }

    RectTransform FillPage(RectTransform page, SellItemObject[] sellItems)
    {
        for (int i = 0; i < sellItems.Length; i++)
            if (sellItems[i] != null) Instantiate(sellItemPanel, page).GetComponent<SellItemController>().Fill(sellItems[i], items);
        return page;
    }

    bool EndOfList { get => (direction == 1 && currentPageNode.Previous == null || direction == -1 && currentPageNode.Next == null); }

}
