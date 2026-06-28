using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Business.Menus.Options
{
    public class OptionsMenuManager: MenuManager
    {
        public override void SetUp(Action<float>[] buttonActionsMap)
        {
            this.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { buttonActionsMap[0](0); });
        }
    }
}