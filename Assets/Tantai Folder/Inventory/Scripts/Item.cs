using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public string description;
    public ItemType type;
    public Sprite icon;

    public int recover;

    public enum ItemType
    {
        Energy,
        Duck
    }
}
