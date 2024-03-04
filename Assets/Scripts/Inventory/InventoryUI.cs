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
            AddEventTrigger(slot, i); // 이벤트 트리거 추가
        }
    }
    private void AddEventTrigger(GameObject slot, int index)
    {
        EventTrigger trigger = slot.GetComponent<EventTrigger>() ?? slot.AddComponent<EventTrigger>();

        // 드래그 시작 이벤트
        EventTrigger.Entry beginDragEntry = new EventTrigger.Entry();
        beginDragEntry.eventID = EventTriggerType.BeginDrag;
        beginDragEntry.callback.AddListener((data) => { OnBeginDrag(index); });
        trigger.triggers.Add(beginDragEntry);

        // 드래그 이벤트
        EventTrigger.Entry dragEntry = new EventTrigger.Entry();
        dragEntry.eventID = EventTriggerType.Drag;
        dragEntry.callback.AddListener((data) => { OnDrag(data); });
        trigger.triggers.Add(dragEntry);

        // 드랍 이벤트
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
            // 이미지 복제
            if (draggedItemIcon == null)
            {
                draggedItemIcon = new GameObject("DraggedItemIcon");
                var image = draggedItemIcon.AddComponent<Image>();
                var rectTransform = draggedItemIcon.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(100, 100);
                image.sprite = itemSprite;
                image.raycastTarget = false; // 드래그 중인 아이콘은 히트 제외
                draggedItemIcon.transform.SetParent(transform.GetChild(0));
                draggedItemIcon.transform.localScale = Vector3.one;
            }

            draggedItemIcon.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f); // 반투명하게 설정
        }

        // 드래그 시작 로직
        Debug.Log("Begin Dragging item from slot: " + index);
        oldIndex = index;
    }
    private void OnDrag(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        if (draggedItemIcon != null)
        {
            draggedItemIcon.transform.position = pointerData.position; // 마우스 또는 터치 위치로 이동
        }
    }

    private void OnEndDrag(BaseEventData data, int index)
    {
        if (draggedItemIcon != null)
        {
            Destroy(draggedItemIcon); // 드래그 아이콘 제거
            draggedItemIcon = null;
        }

        PointerEventData pointerData = (PointerEventData)data;
        GameObject dropTarget = pointerData.pointerEnter; // 드랍 타겟

        if (dropTarget != null && dropTarget.CompareTag("ItemSlot"))
        {
            int newIndex = Convert.ToInt32(dropTarget.name);

            if (newIndex != oldIndex)
            {
                inventory.MoveItem(oldIndex, newIndex);
                UpdateUI(); // 인벤토리 UI 업데이트
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
        // 퍼즐 맞췄는지 체크
        for (int i = 0; i < inventory._items.Length; i++)
        {
            // 빈 자리 있으면 리턴
            if (inventory._items[i] == null) yield break;
            // 자기 자리 아닌 퍼즐이 있으면 리턴
            if (inventory._items[i].itemId.ToString() != itemSlots.transform.GetChild(i).name) yield break;
        }

        // 퍼즐 클리어 애니메이션
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
