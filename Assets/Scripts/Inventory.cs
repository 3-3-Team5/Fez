using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class Item
{
    public int itemId; // ������ ���̵�
    public string itemName; // ������ �̸�
    public string itemDescription; // ������ ����
    public Sprite itemSprite; // ������ �̹���
    public int itemIndex; // �κ��丮�� ���° ��ġ�� �ִ���
    // ���� ���� �������� �ִٸ� ������ Ÿ�� �߰� & ���� Ÿ���� ��ġ ���� �ʿ�)
    // ���� �����Ѵٸ� ������ �ùٸ� ��ġ ���� �߰� (�ش� ��ġ�� �´� �ڸ����� �Ǵ� or itemId�� �Ǵ�)

    public Item(int id, string name, string description)
    {
        itemId = id;
        itemName = name;
        itemDescription = description;
        itemSprite = Resources.Load<Sprite>(itemName); // or id�� �ε�
    }
}

public class Inventory
{
    private Item[] _items;

    public Inventory(int size)
    {
        _items = new Item[size];
    }

    // ������ �߰�
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
        return false; // �κ��丮�� �� á��
    }

    // ������ ����
    public void RemoveItem(int index)
    {
        if (index >= 0 && index < _items.Length)
        {
            _items[index] = null; // �ش� ������ ���
        }
    }

    // ������ �ű��
    public void MoveItem(int oldIndex, int newIndex)
    {
        if (oldIndex < 0 || oldIndex >= _items.Length || newIndex < 0 || newIndex >= _items.Length || oldIndex == newIndex)
        {
            return; // �Ұ����� �ε���
        }

        // ������ �̵�
        Item item1 = _items[oldIndex];
        Item item2 = _items[newIndex];
        _items[oldIndex] = item2;
        _items[newIndex] = item1;
    }

    // �κ��丮 ���� �� UI ������Ʈ
    public void InventoryUpdate()
    {

    }
}
