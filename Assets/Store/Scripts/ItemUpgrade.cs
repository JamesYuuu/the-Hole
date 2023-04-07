using UnityEngine;

public class ItemUpgrade: MonoBehaviour
{

    public static ItemUpgrade _instance;
    public string ItemName;
    public bool IsSelected;

    private void Awake()
    {
        _instance = this;
    }

    public void Update()
    {
        if (IsSelected == true && InputManager.GetInstance().PlayerPressedPrimaryL())
        {
            if (ItemName == "Energy Drink")
            {
                //upgrade;
                //delete;
            }
            else if (ItemName == "O2 Tank")
            {
                //upgrade;
                //delete;
            }
        }
    }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;
    }
    public void SetItemName(string name)
    {
        ItemName = name;
    }
}