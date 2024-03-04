using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    public int itemId; // 아이템 아이디
    public string itemName; // 아이템 이름
    public string itemDescription; // 아이템 설명
    //public Sprite itemSprite; // 아이템 이미지
    public int itemIndex; // 인벤토리의 몇번째 위치에 있는지

    //public GameObject itemPrefab;
    // 퍼즐 외의 아이템이 있다면 아이템 타입 추가 & 퍼즐 타입은 위치 정보 필요)
    // 퍼즐만 존재한다면 퍼즐의 올바른 위치 정보 추가 (해당 위치로 맞는 자리인지 판단 or itemId로 판단)
    /*
    public Item(int id, string name, string description)
    {
        itemId = id;
        itemName = name;
        itemDescription = description;
        itemSprite = Resources.Load<Sprite>(itemName); // or id로 로드
    }
    */
}
