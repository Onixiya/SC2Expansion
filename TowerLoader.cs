using Assets.Scripts.Models;
using Assets.Scripts.Models.Profile;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Upgrades;
using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Unity.UI_New.InGame.StoreMenu;
using Assets.Scripts.Unity.UI_New.Upgrade;
using SC2Towers.Towers;
using HarmonyLib;
using MelonLoader;
using System.Collections.Generic;
using UnityEngine;
using Color=UnityEngine.Color;
using Image=UnityEngine.UI.Image;
using SC2Towers.Utils;

[assembly: MelonGame("Ninja Kiwi","BloonsTD6")]
[assembly: MelonInfo(typeof(SC2Towers.SC2Towers),"SC2Towers","1.0","Silentstorm#5336")]

namespace SC2Towers{
    public class SC2Towers:MelonMod{
        public override void OnApplicationStart(){
            //should figure out how to make it do a foreach thing instead of like this
            //Hydralisk.Assets=AssetBundle.LoadFromMemory(Models.Models.model);
            //HighTemplar.Assets=AssetBundle.LoadFromMemory(Models.Models.model);
            SC2Marine.Assets=AssetBundle.LoadFromMemory(Models.Models.model);
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
        internal static List<(TowerModel, TowerDetailsModel, TowerModel[], UpgradeModel[])> towers = new();
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
                //towers.Add(Hydralisk.GetTower(__result));
                //towers.Add(HighTemplar.GetTower(__result));
                towers.Add(SC2Marine.GetTower(__result));
                MelonLogger.Msg("SC2Marine loaded");
                foreach(var tower in towers){
                    __result.towers=__result.towers.Add(tower.Item3);
                    __result.towerSet=__result.towerSet.Add(tower.Item2);
                    __result.upgrades=__result.upgrades.Add(tower.Item4);
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