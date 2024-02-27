using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class Item
{
    public int itemId; // 아이템 아이디
    public string itemName; // 아이템 이름
    public string itemDescription; // 아이템 설명
    public Sprite itemSprite; // 아이템 이미지
    public int itemIndex; // 인벤토리의 몇번째 위치에 있는지
    // 퍼즐 외의 아이템이 있다면 아이템 타입 추가 & 퍼즐 타입은 위치 정보 필요)
    // 퍼즐만 존재한다면 퍼즐의 올바른 위치 정보 추가 (해당 위치로 맞는 자리인지 판단 or itemId로 판단)

    public Item(int id, string name, string description)
    {
        itemId = id;
        itemName = name;
        itemDescription = description;
        itemSprite = Resources.Load<Sprite>(itemName); // or id로 로드
    }
}

public class Inventory
{
    private Item[] _items;

    public Inventory(int size)
    {
        _items = new Item[size];
    }

    // 아이템 추가
    public bool AddItem(Item item)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] == null)
            {
                _items[i] = item;
                return true;
            }
        }
        return false; // 인벤토리가 꽉 찼음
    }

    // 아이템 삭제
    public void RemoveItem(int index)
    {
        if (index >= 0 && index < _items.Length)
        {
            _items[index] = null; // 해당 슬롯을 비움
        }
    }

    // 아이템 옮기기
    public void MoveItem(int oldIndex, int newIndex)
    {
        if (oldIndex < 0 || oldIndex >= _items.Length || newIndex < 0 || newIndex >= _items.Length || oldIndex == newIndex)
        {
            return; // 불가능한 인덱스
        }

        // 아이템 이동
        Item item1 = _items[oldIndex];
        Item item2 = _items[newIndex];
        _items[oldIndex] = item2;
        _items[newIndex] = item1;
    }

    // 인벤토리 변경 후 UI 업데이트
    public void InventoryUpdate()
    {

    }
}
