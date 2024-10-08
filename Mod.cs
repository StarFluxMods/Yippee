using KitchenLib;
using KitchenLib.Logging;
using KitchenLib.Logging.Exceptions;
using KitchenMods;
using System.Linq;
using System.Reflection;
using Kitchen;
using KitchenData;
using KitchenLib.Event;
using KitchenLib.Interfaces;
using KitchenLib.Preferences;
using KitchenLib.Utils;
using UnityEngine;
using Yipee.Customs;
using Yipee.Menus;
using KitchenLogger = KitchenLib.Logging.KitchenLogger;

namespace Yipee
{
    public class Mod : BaseMod, IAutoRegisterAll
    {
        public const string MOD_GUID = "com.starfluxgames.yippee";
        public const string MOD_NAME = "Yippee";
        public const string MOD_VERSION = "0.1.4";
        public const string MOD_AUTHOR = "StarFluxGames";
        public const string MOD_GAMEVERSION = ">=1.2.0";

        public static AssetBundle Bundle;
        public static KitchenLogger Logger;
        public static PreferenceManager manager;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }
        
        protected override void OnInitialise()
        {
            Logger.LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
            ((Unlock)GDOUtils.GetCustomGameDataObject<YipeeCard>().GameDataObject).IsUnlockable = manager.GetPreference<PreferenceBool>("shouldOnlyUseCard").Value;
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).FirstOrDefault() ?? throw new MissingAssetBundleException(MOD_GUID);
            Logger = InitLogger();
            
            manager = new PreferenceManager(MOD_GUID);
            manager.RegisterPreference(new PreferenceBool("shouldOnlyUseCard", true));
            manager.Load();
            manager.Save();
            
            ModsPreferencesMenu<MenuAction>.RegisterMenu(MOD_NAME, typeof(PreferenceMenu<MenuAction>), typeof(MenuAction));
            ModsPreferencesMenu<MenuAction>.RegisterMenu(MOD_NAME, typeof(PreferenceMenu<MenuAction>), typeof(MenuAction));
            
            Events.MainMenuView_SetupMenusEvent += (s, args) =>
            {
                args.addMenu.Invoke(args.instance, new object[] { typeof(PreferenceMenu<MenuAction>), new PreferenceMenu<MenuAction>(args.instance.ButtonContainer, args.module_list) });
            };
            
            Events.PlayerPauseView_SetupMenusEvent += (s, args) =>
            {
                args.addMenu.Invoke(args.instance, new object[] { typeof(PreferenceMenu<MenuAction>), new PreferenceMenu<MenuAction>(args.instance.ButtonContainer, args.module_list) });
            };
        }
    }
}

