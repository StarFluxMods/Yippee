using System.Collections.Generic;
using Kitchen;
using Kitchen.Modules;
using KitchenData;
using KitchenLib;
using KitchenLib.Preferences;
using KitchenLib.Utils;
using UnityEngine;
using Yipee.Customs;

namespace Yipee.Menus
{
    public class PreferenceMenu<T>: KLMenu<T>
    {
        public PreferenceMenu(Transform container, ModuleList module_list) : base(container, module_list)
        {
        }
        
        private Option<bool> shouldOnlyUseCard = new Option<bool>(new List<bool> { true, false }, Mod.manager.GetPreference<PreferenceBool>("shouldOnlyUseCard").Value, new List<string> { "With Card", "Always Active" });
        
        public override void Setup(int player_id)
        {
            AddLabel("Yippee Trigger");
            New<SpacerElement>(true);
            AddSelect(shouldOnlyUseCard);
            shouldOnlyUseCard.OnChanged += delegate (object _, bool result)
            {
                Mod.manager.GetPreference<PreferenceBool>("shouldOnlyUseCard").Set(result);
                ((Unlock)GDOUtils.GetCustomGameDataObject<YipeeCard>().GameDataObject).IsUnlockable = result;
                Mod.manager.Save();
            };
            New<SpacerElement>(true);
            New<SpacerElement>(true);
            AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate(int i)
            {
                Mod.manager.Save();
                RequestPreviousMenu();
            });
        }
    }
}