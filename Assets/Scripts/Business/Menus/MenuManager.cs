using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Business.Menus
{
    public class MenuManager: MonoBehaviour
    {
        protected List<Button> internalFunctionButtons = new();
        protected List<Button> externalFunctionButtons = new();

        public virtual void SetUp(Action<float>[] buttonActionsMap)
        {
            for (int i = 0; i < externalFunctionButtons.Count; i++)
            {
                if (buttonActionsMap.Length > i)
                {
                    externalFunctionButtons[i].onClick.AddListener(() => { buttonActionsMap[i](0f); });
                }
                else break;
            }
            buttonActionsMap[1](0f);
        }
    }
}
