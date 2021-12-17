using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    int id;
    string title;
    string description;
    Sprite icon;

    public Item(int id, string title, string description, Sprite icon)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.icon = icon;
    }

    public Item(Item item)
    {
        this.id = item.id;
        this.title = item.title;
        this.description = item.description;
        this.icon = item.icon;
    }
}
