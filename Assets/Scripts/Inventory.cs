[System.Serializable]
public class Inventory
{
    public ItemData[] _items;

    public Inventory(int size)
    {
        _items = new ItemData[size];
    }

    // 아이템 추가
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
        ItemData temp = _items[oldIndex];
        _items[oldIndex] = _items[newIndex];
        _items[newIndex] = temp;

        // 필요한 경우 itemIndex 업데이트
        //if (_items[oldIndex] != null) _items[oldIndex].itemIndex = oldIndex;
        //if (_items[newIndex] != null) _items[newIndex].itemIndex = newIndex;

        /*
        _items[oldIndex].itemIndex = newIndex;
        if (_items[newIndex] != null)
            _items[newIndex].itemIndex = oldIndex;
        /*
        // 아이템 이동
        ItemData item1 = _items[oldIndex];
        ItemData item2 = _items[newIndex];
        _items[oldIndex] = item2;
        _items[newIndex] = item1;
        */
    }
}
