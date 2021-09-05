//saves so much space with global, no need to put separate usings in each file, just put them all in the main one
global using Assets.Scripts.Models;
global using Assets.Scripts.Models.Profile;
global using Assets.Scripts.Models.Towers;
global using Assets.Scripts.Models.Towers.Upgrades;
global using Assets.Scripts.Models.TowerSets;
global using Assets.Scripts.Unity.UI_New.InGame.StoreMenu;
global using SC2Expansion.Towers;
global using SC2Expansion.Utils;
global using HarmonyLib;
global using MelonLoader;
global using System.Collections.Generic;
global using UnityEngine;
global using Image=UnityEngine.UI.Image;
global using BTD_Mod_Helper;
global using BTD_Mod_Helper.Api.ModOptions;
global using Assets.Scripts.Models.GenericBehaviors;
global using Assets.Scripts.Models.Map;
global using Assets.Scripts.Unity.Display;
global using Assets.Scripts.Utils;
global using System.Linq;
global using Object=UnityEngine.Object;
global using Assets.Scripts.Models.Towers.Behaviors.Attack;
global using Assets.Scripts.Models.Towers.Behaviors;
global using Assets.Scripts.Models.Towers.Behaviors.Emissions;
global using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
global using BTD_Mod_Helper.Api.Towers;
global using BTD_Mod_Helper.Extensions;
global using Assets.Scripts.Unity;
global using NinjaKiwi.Common;
global using Assets.Scripts.Models.Towers.Behaviors.Abilities;
global using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
[assembly: MelonGame("Ninja Kiwi","BloonsTD6")]
[assembly: MelonInfo(typeof(SC2Expansion.SC2Expansion),"SC2Expansion","1.1","Silentstorm#5336")]
namespace SC2Expansion{
    public class SC2Expansion:BloonsTD6Mod{
        //public override string GithubReleaseURL=>"https://api.github.com/repos/Onixiya/SC2Expansion/releases";
        public static readonly ModSettingBool ProtossEnabled=true;
        public static readonly ModSettingBool TerranEnabled=true;
        public static readonly ModSettingBool ZergEnabled=true;
        //private static readonly ModSettingBool HeroesEnabled=true;
        public override void OnApplicationStart(){
            if(ProtossEnabled==true){
                HighTemplar.Assets=AssetBundle.LoadFromMemory(Models.Models.hightemplar);
            }
            if(TerranEnabled==true){
                SC2Marine.Assets=AssetBundle.LoadFromMemory(Models.Models.sc2marine);
            }
            if(ZergEnabled==true){
                Hydralisk.Assets=AssetBundle.LoadFromMemory(Models.Models.hydralisk);
                BanelingNest.Assets=AssetBundle.LoadFromMemory(Models.Models.banelingnest);
            }
        }
        [HarmonyPatch(typeof(StandardTowerPurchaseButton),nameof(StandardTowerPurchaseButton.UpdateTowerDisplay))]
        public class SetBG{
            [HarmonyPostfix]
            public static void Postfix(ref StandardTowerPurchaseButton __instance){
                __instance.bg=__instance.gameObject.GetComponent<Image>();
                if(__instance.baseTowerModel.emoteSpriteLarge!=null)
                    switch(__instance.baseTowerModel.emoteSpriteLarge.guidRef){
                        case "Protoss":
                            __instance.bg.overrideSprite=LoadSprite(LoadTextureFromBytes(Properties.Resources.ProtossContainer));
                            break;
                        case "Terran":
                            __instance.bg.overrideSprite=LoadSprite(LoadTextureFromBytes(Properties.Resources.TerranContainer));
                            break;
                        case "Zerg":
                            __instance.bg.overrideSprite=LoadSprite(LoadTextureFromBytes(Properties.Resources.ZergContainer));
                            break;
                    }
                if(__instance.baseTowerModel.emoteSpriteSmall!=null)
                    switch(__instance.baseTowerModel.emoteSpriteSmall.guidRef) {
                        case "Protoss":
                            __instance.bg.overrideSprite=LoadSprite(LoadTextureFromBytes(Properties.Resources.ProtossContainer));
                            break;
                        case "Terran":
                            __instance.bg.overrideSprite=LoadSprite(LoadTextureFromBytes(Properties.Resources.TerranContainer));
                            break;
                        case "Zerg":
                            __instance.bg.overrideSprite=LoadSprite(LoadTextureFromBytes(Properties.Resources.ZergContainer));
                            break;
                    }
                return;
            }
            private static Texture2D LoadTextureFromBytes(byte[] FileData){
                Texture2D Tex2D=new(2,2);
                if(ImageConversion.LoadImage(Tex2D,FileData)) return Tex2D;
                return null;
            }
            private static Sprite LoadSprite(Texture2D text){
                return Sprite.Create(text,new(0,0,text.width,text.height),new());
            }
        }
        internal static List<(TowerModel,TowerDetailsModel,TowerModel[],UpgradeModel[])>towers=new();
        [HarmonyPatch(typeof(ProfileModel),"Validate")]
        public class ProfileModel_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref ProfileModel __instance){
                var unlockedTowers=__instance.unlockedTowers;
                var unlockedUpgrades=__instance.acquiredUpgrades;
                foreach(var tower in towers){
                    if(!unlockedTowers.Contains(tower.Item1.baseId))unlockedTowers.Add(tower.Item1.baseId);
                    foreach(var upgrade in tower.Item4)
                        if(!unlockedUpgrades.Contains(upgrade.name))unlockedUpgrades.Add(upgrade.name);
                }
            }
        }
        [HarmonyPatch(typeof(GameModelLoader),nameof(GameModelLoader.Load))]
        public static class GameStart{
            [HarmonyPostfix]
            public static void Postfix(ref GameModel __result){
                int towersloaded=0;
                if(ProtossEnabled==true){
                    towers.Add(HighTemplar.GetTower(__result));
                    towersloaded++;
                }
                if(TerranEnabled==true){
                    towers.Add(SC2Marine.GetTower(__result));
                    towersloaded++;
                }
                if(ZergEnabled==true){
                    towers.Add(Hydralisk.GetTower(__result));
                    towersloaded++;
                }
                foreach(var tower in towers){
                    __result.towers=__result.towers.Add(tower.Item3);
                    __result.towerSet=__result.towerSet.Add(tower.Item2);
                    __result.upgrades=__result.upgrades.Add(tower.Item4);
                }
                if(towersloaded==0){
                    MelonLogger.Msg("No towers loaded");
                }else if(towersloaded==1){
                    MelonLogger.Msg("1 tower loaded");
                }else{
                    MelonLogger.Msg(towersloaded+" towers loaded");
                }
            }
        }
    }
}