using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment,//장비
    Consumption,//소모
    Misc//기타
}
public enum EquipType
{
    temp,
    Weapon,//무기
    Armor,
    Helmet,
    Shoulder,
    Pants,
    Shoes
}

[System.Serializable]
public class Item
{

    public string itemName;
    public int itemValue;
    public int itemPrice;
    public string itemDesc;
    public ItemType itemType;
    public Sprite itemImage;


    //장비 
    public EquipType itemEquipType;
    public int itemStrength;
    public int itemDefense;
    public float itemCritical;
    public int upgrade;


    //소모 아이템 세팅
    public Item(int _itemType, string _itemName, int _itemValue, int _itemPrice, string _itemDesc, Sprite _itemImage)
    {
        itemType = ChangeItemType(_itemType);
        itemName = _itemName;
        itemValue = _itemValue;
        itemPrice = _itemPrice;
        itemDesc = _itemDesc;
        itemImage = _itemImage;
    }
    //장비아이템 세팅
    public Item(int _itemType,string _itemName, int _itemValue, int _itemPrice, string _itemDesc, Sprite _itemImage,int _equipType,int _itemStrength,int _itemDefense,float _itemCritical,int _upgrade)
    {
        itemType = ChangeItemType(_itemType);
        itemName = _itemName;
        itemValue = _itemValue;
        itemPrice = _itemPrice;
        itemDesc = _itemDesc;
        itemImage = _itemImage;
        itemEquipType = ChangeEquipType(_equipType);
        itemStrength = _itemStrength;
        itemDefense = _itemDefense;
        itemCritical = _itemCritical;
        upgrade = _upgrade;
    }

    public ItemType ChangeItemType(int type)
    {
        switch (type)
        {
            case 1:
                return ItemType.Equipment;
            case 2:
                return ItemType.Consumption;
            case 3:
                return ItemType.Misc;
        }
        return ItemType.Misc;
    }
    public EquipType ChangeEquipType(int type)
    {
        switch (type)
        {
            case 1:
                return EquipType.Weapon;
            case 2:
                return EquipType.Armor;
            case 3:
                return EquipType.Helmet;
            case 4:
                return EquipType.Shoulder;
            case 5:
                return EquipType.Pants;
            case 6:
                return EquipType.Shoes;
        }
        return EquipType.temp;
    }
}
