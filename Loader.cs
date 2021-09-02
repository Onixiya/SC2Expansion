using Assets.Scripts.Models;
using Assets.Scripts.Models.Profile;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Upgrades;
using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Unity.UI_New.InGame.StoreMenu;
using SC2Expansion.Towers;
using SC2Expansion.Utils;
using HarmonyLib;
using MelonLoader;
using System.Collections.Generic;
using UnityEngine;
using Color=UnityEngine.Color;
using Image=UnityEngine.UI.Image;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.ModOptions;
using Assets.Scripts.Simulation.Towers.Weapons;
using System.Threading.Tasks;

[assembly: MelonGame("Ninja Kiwi","BloonsTD6")]
[assembly: MelonInfo(typeof(SC2Expansion.SC2Expansion),"SC2Expansion","1.0","Silentstorm#5336")]

namespace SC2Expansion{
    public class SC2Expansion:BloonsTD6Mod {
        private static readonly ModSettingBool ProtossEnabled = true;
        private static readonly ModSettingBool TerranEnabled = true;
        private static readonly ModSettingBool ZergEnabled = true;
        //private static readonly ModSettingBool HeroesEnabled=true;
        public override void OnApplicationStart() {
            if(ProtossEnabled==true) {
                HighTemplar.Assets=AssetBundle.LoadFromMemory(Models.Models.hightemplar);
            }
            if(TerranEnabled==true) {
                SC2Marine.Assets=AssetBundle.LoadFromMemory(Models.Models.sc2marine);
            }
            if(ZergEnabled==true) {
                Hydralisk.Assets=AssetBundle.LoadFromMemory(Models.Models.hydralisk);
            }
        }
        [HarmonyPatch(typeof(StandardTowerPurchaseButton),nameof(StandardTowerPurchaseButton.UpdateTowerDisplay))]
        public class SetBG {
            [HarmonyPostfix]
            public static void Postfix(ref StandardTowerPurchaseButton __instance) {
                __instance.bg=__instance.gameObject.GetComponent<Image>();
                if(__instance.baseTowerModel.emoteSpriteLarge!=null)
                    switch(__instance.baseTowerModel.emoteSpriteLarge.guidRef) {
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
            private static Texture2D LoadTextureFromBytes(byte[] FileData) {
                Texture2D Tex2D = new(2,2);
                if(ImageConversion.LoadImage(Tex2D,FileData)) return Tex2D;
                return null;
            }
            private static Sprite LoadSprite(Texture2D text) {
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
            [HarmonyPatch(typeof(Tower),nameof(Tower.Hilight))]
            public static class TH{
                [HarmonyPostfix]
                public static void Postfix(ref Tower __instance){
                    if(__instance?.Node?.graphic?.genericRenderers==null)return;
                    foreach(var graphicGenericRenderer in __instance.Node.graphic.genericRenderers)
                        graphicGenericRenderer.material.SetColor("_OutlineColor",Color.white);
                }
            }
            [HarmonyPatch(typeof(Tower),nameof(Tower.UnHighlight))]
            public static class TU{
                [HarmonyPostfix]
                public static void Postfix(ref Tower __instance){
                    if(__instance?.Node?.graphic?.genericRenderers==null)return;
                    foreach(var graphicGenericRenderer in __instance.Node.graphic.genericRenderers)
                        graphicGenericRenderer.material.SetColor("_OutlineColor",Color.black);
                }
            }
        }
    }
}