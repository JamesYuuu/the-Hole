using UnityEngine;

public class ItemUpgrade: MonoBehaviour
{

    public static ItemUpgrade _instance;
    public GameObject item;
    public bool IsSelected;

    private void Awake()
    {
        _instance = this;
    }

    public void Start()
    {
        IsSelected = false;
        item = null;
    }

    public void Update()
    {
        bool IsUpgrade = false;
        if (item!=null && IsSelected == true && InputManager.GetInstance().PlayerPressedPrimaryL())
        {
            print(item.GetComponent<Item>().GetName() + " has been upgraded!");
            if (item.GetComponent<Item>().GetName() == "Energy Drink")
            {
                GrappleController._instance.SetGrappleValues(Grappleable.Hand.Left,PlayerData.GrapplingRange*1.3f,PlayerData.GrapplingShootSpeed*1.3f,PlayerData.GrapplingRange*1.3f);
                GrappleController._instance.SetGrappleValues(Grappleable.Hand.Right,PlayerData.GrapplingRange*1.3f,PlayerData.GrapplingShootSpeed*1.3f,PlayerData.GrapplingRange*1.3f);
                IsUpgrade = true;
                IsSelected = false;
            }
            else if (item.GetComponent<Item>().GetName() == "O2 Tank")
            {
                PlayerHealth._Instance.ChangeOxygen(50.0f);
                IsUpgrade = true;
                IsSelected = false;
            }
        }

        if (IsUpgrade == true)
        {
            Destroy(item);
        }
    }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;
    }
    public void SetItem(GameObject gameObject)
    {
        item = gameObject;
    }
}