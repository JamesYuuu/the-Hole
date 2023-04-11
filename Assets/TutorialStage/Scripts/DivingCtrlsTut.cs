using System.Collections.Generic;
using UnityEngine;

public class DivingCtrlsTut : MonoBehaviour
{
    [SerializeField] private string animTriggerName = "OnToggle";
    [SerializeField] private List<Animator> iconAnimators;
    private static bool _seen = false;
    
    /// <summary>
    /// Auto-toggles icons the first time the player enters the water
    /// </summary>
    void Start()
    {
        if (_seen) return;
        ToggleAllIcons();
    }

    public void ToggleAllIcons()
    {
        iconAnimators.ForEach(animator => animator.SetTrigger(animTriggerName));
        _seen = true;
    }
    public void ToggleIcon(Animator antr)
    {
        antr.SetTrigger(animTriggerName);
    }
}
