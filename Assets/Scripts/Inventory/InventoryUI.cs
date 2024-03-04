using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Player player;
    private Inventory inventory;
    [SerializeField]
    private GameObject itemSlots;
    private int oldIndex;
    private GameObject draggedItemIcon;
    private GridLayoutGroup gridLayoutGroup;
    private Vector2 puzzleClearSpacing = new Vector2(0, 0);
    private Vector2 puzzleStartSpacing = new Vector2(20, 20);
    private float puzzleClearDuration = 2.0f;

    private void Awake()
    {
        inventory = player.inventory;
        gridLayoutGroup = itemSlots.GetComponent<GridLayoutGroup>();
    }
    private void Start()
    {
        InitializeInventorySlots();
    }

    private void OnEnable()
    {
        UpdateUI();
        StartCoroutine(PuzzleClearCheck());
    }
    private void InitializeInventorySlots()
    {
        for (int i = 0; i < itemSlots.transform.childCount; i++)
        {
            GameObject slot = itemSlots.transform.GetChild(i).gameObject;
            AddEventTrigger(slot, i); // �̺�Ʈ Ʈ���� �߰�
        }
    }
    private void AddEventTrigger(GameObject slot, int index)
    {
        EventTrigger trigger = slot.GetComponent<EventTrigger>() ?? slot.AddComponent<EventTrigger>();

        // �巡�� ���� �̺�Ʈ
        EventTrigger.Entry beginDragEntry = new EventTrigger.Entry();
        beginDragEntry.eventID = EventTriggerType.BeginDrag;
        beginDragEntry.callback.AddListener((data) => { OnBeginDrag(index); });
        trigger.triggers.Add(beginDragEntry);

        // �巡�� �̺�Ʈ
        EventTrigger.Entry dragEntry = new EventTrigger.Entry();
        dragEntry.eventID = EventTriggerType.Drag;
        dragEntry.callback.AddListener((data) => { OnDrag(data); });
        trigger.triggers.Add(dragEntry);

        // ��� �̺�Ʈ
        EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
        endDragEntry.eventID = EventTriggerType.EndDrag;
        endDragEntry.callback.AddListener((data) => { OnEndDrag(data, index); });
        trigger.triggers.Add(endDragEntry);
    }

    private void OnBeginDrag(int index)
    {
        Sprite itemSprite = Resources.Load<Sprite>(inventory._items[index].itemId.ToString());

        if (itemSprite != null)
        {
            // �̹��� ����
            if (draggedItemIcon == null)
            {
                draggedItemIcon = new GameObject("DraggedItemIcon");
                var image = draggedItemIcon.AddComponent<Image>();
                var rectTransform = draggedItemIcon.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(100, 100);
                image.sprite = itemSprite;
                image.raycastTarget = false; // �巡�� ���� �������� ��Ʈ ����
                draggedItemIcon.transform.SetParent(transform.GetChild(0));
                draggedItemIcon.transform.localScale = Vector3.one;
            }

            draggedItemIcon.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f); // �������ϰ� ����
        }

        // �巡�� ���� ����
        Debug.Log("Begin Dragging item from slot: " + index);
        oldIndex = index;
    }
    private void OnDrag(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        if (draggedItemIcon != null)
        {
            draggedItemIcon.transform.position = pointerData.position; // ���콺 �Ǵ� ��ġ ��ġ�� �̵�
        }
    }

    private void OnEndDrag(BaseEventData data, int index)
    {
        if (draggedItemIcon != null)
        {
            Destroy(draggedItemIcon); // �巡�� ������ ����
            draggedItemIcon = null;
        }

        PointerEventData pointerData = (PointerEventData)data;
        GameObject dropTarget = pointerData.pointerEnter; // ��� Ÿ��

        if (dropTarget != null && dropTarget.CompareTag("ItemSlot"))
        {
            int newIndex = Convert.ToInt32(dropTarget.name);

            if (newIndex != oldIndex)
            {
                inventory.MoveItem(oldIndex, newIndex);
                UpdateUI(); // �κ��丮 UI ������Ʈ
            }
        }

        Debug.Log("Dropped on slot: " + dropTarget.name);
    }

    private void UpdateUI()
    {
        for (int i = 0; i < inventory._items.Length; i++)
        {
            if (inventory._items[i] == null)
                itemSlots.transform.GetChild(i).GetComponent<Image>().sprite = null;
            else
                itemSlots.transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + inventory._items[i].itemId.ToString());
        }

        StartCoroutine(PuzzleClearCheck());
    }

    private IEnumerator PuzzleClearCheck()
    {
        // ���� ������� üũ
        for (int i = 0; i < inventory._items.Length; i++)
        {
            // �� �ڸ� ������ ����
            if (inventory._items[i] == null) yield break;
            // �ڱ� �ڸ� �ƴ� ������ ������ ����
            if (inventory._items[i].itemId.ToString() != itemSlots.transform.GetChild(i).name) yield break;
        }

        // ���� Ŭ���� �ִϸ��̼�
        float time = 0f;

        while (time < puzzleClearDuration)
        {
            time += Time.deltaTime;
            float t = time / puzzleClearDuration;
            gridLayoutGroup.spacing = Vector2.Lerp(puzzleStartSpacing, puzzleClearSpacing, t);

            yield return null;
        }

        gridLayoutGroup.spacing = puzzleClearSpacing;

        Instantiate(Resources.Load<GameObject>("Prefabs/Fade"));

        gameObject.SetActive(false);
    }
}
