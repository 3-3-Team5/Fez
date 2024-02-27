[System.Serializable]
public class Inventory
{
    public ItemData[] _items;

    public Inventory(int size)
    {
        _items = new ItemData[size];
    }

    // ������ �߰�
    public bool AddItem(ItemData item)
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
        ItemData temp = _items[oldIndex];
        _items[oldIndex] = _items[newIndex];
        _items[newIndex] = temp;

        // �ʿ��� ��� itemIndex ������Ʈ
        //if (_items[oldIndex] != null) _items[oldIndex].itemIndex = oldIndex;
        //if (_items[newIndex] != null) _items[newIndex].itemIndex = newIndex;

        /*
        _items[oldIndex].itemIndex = newIndex;
        if (_items[newIndex] != null)
            _items[newIndex].itemIndex = oldIndex;
        /*
        // ������ �̵�
        ItemData item1 = _items[oldIndex];
        ItemData item2 = _items[newIndex];
        _items[oldIndex] = item2;
        _items[newIndex] = item1;
        */
    }
}
