using System;
using UnityEngine.UI;

namespace Assets.Scripts.Business.Menus.Options
{
    public class OptionsMenuManager: MenuManager
    {
        public override void SetUp(Action<float>[] buttonActionsMap)
        {
            this.transform.GetChild(transform.childCount-1).GetComponent<Button>().onClick.AddListener(() => { buttonActionsMap[0](0); });

            //_slider = GetComponentInChildren<Slider>();
            //_slider.onValueChanged.AddListener((float value) => { buttonActionsMap[1](value); });
        }
    }
}