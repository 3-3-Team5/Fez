using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    public int itemId; // ������ ���̵�
    public string itemName; // ������ �̸�
    public string itemDescription; // ������ ����
    //public Sprite itemSprite; // ������ �̹���
    public int itemIndex; // �κ��丮�� ���° ��ġ�� �ִ���

    //public GameObject itemPrefab;
    // ���� ���� �������� �ִٸ� ������ Ÿ�� �߰� & ���� Ÿ���� ��ġ ���� �ʿ�)
    // ���� �����Ѵٸ� ������ �ùٸ� ��ġ ���� �߰� (�ش� ��ġ�� �´� �ڸ����� �Ǵ� or itemId�� �Ǵ�)
    /*
    public Item(int id, string name, string description)
    {
        itemId = id;
        itemName = name;
        itemDescription = description;
        itemSprite = Resources.Load<Sprite>(itemName); // or id�� �ε�
    }
    */
}
