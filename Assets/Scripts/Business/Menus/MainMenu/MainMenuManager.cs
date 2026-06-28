using Assets.Scripts.Business.Menus.Options;
using Assets.Scripts.Business.SaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Business.Menus.MainMenu
{
    public class MainMenuManager: MenuManager
    {
        //[SerializeField] private GameObject _optionsMenu;
        public override void SetUp(Action<float>[] buttonActionsMap)
        {
            this.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
            {
                buttonActionsMap[0](0f);
            });
            this.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
            {
                buttonActionsMap[1](0f);
            });

            if (SaveManager.CheckForSave()) this.transform.GetChild(0).gameObject.SetActive(true);
            else this.transform.GetChild(0).gameObject.SetActive(false);

            //_optionsMenu.GetComponent<OptionsMenuManager>().SetUp(new Action<float>[] {
            //    (float value) => { BackToMainMenu(); },
            //    (float value) => { buttonActionsMap[1](value); }
            //});
            //_optionsMenu.SetActive(false);

            //transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => {
            //this.gameObject.SetActive(false);
            //_optionsMenu.SetActive(true);
            //});
        }

        private void BackToMainMenu()
        {
            this.gameObject.SetActive(true);
            //_optionsMenu.SetActive(false);
        }
    }
}
