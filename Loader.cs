﻿//saves so much space with global using, no need to put separate usings in each file, just put them all in the main one
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
global using Assets.Scripts.Models.Towers.Behaviors.Abilities;
global using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
global using Assets.Scripts.Simulation.Towers;
global using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
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
                //Adept.Assets=AssetBundle.LoadFromMemory(Models.Models.adept);
                //Archon.Assets=AssetBundle.LoadFromMemory(Models.Models.archon);
                //Carrier.Assets=AssetBundle.LoadFromMemory(Models.Models.carrier);
                //Colossus.Assets=AssetBundle.LoadFromMemory(Models.Models.colossus);
                //DarkShrine.Assets=AssetBundle.LoadFromMemory(Models.Models.darkshrine);
                //FleetBeacon.Assets=AssetBundle.LoadFromMemory(Models.Models.fleetbeacon);
                Gateway.Assets=AssetBundle.LoadFromMemory(Models.Models.gateway);
                //Immortal.Assets=AssetBundle.LoadFromMemory(Models.Models.immortal);
                HighTemplar.Assets=AssetBundle.LoadFromMemory(Models.Models.hightemplar);
                //Nexus.Assets=AssetBundle.LoadFromMemory(Models.Models.nexus);
                //Observer.Assets=AssetBundle.LoadFromMemory(Models.Models.observer);
                //PhotonCannon.Assets=AssetBundle.LoadFromMemory(Models.Models.photoncannon);
                //Pylon.Assets=AssetBundle.LoadFromMemory(Models.Models.pylon);
                //Sentry.Assets=AssetBundle.LoadFromMemory(Models.Models.sentry);
                //Stalker.Assets=AssetBundle.LoadFromMemory(Models.Models.stalker);
                //Tempest.Assets=AssetBundle.LoadFromMemory(Models.Models.tempest);
                //VoidRay.Assets=AssetBundle.LoadFromMemory(Models.Models.voidray);
            }
            if(TerranEnabled==true){
                //Banshee.Assets=AssetBundle.LoadFromMemory(Models.Models.banshee);
                //CommandCenter.Assets=AssetBundle.LoadFromMemory(Models.Models.commandcenter);
                //Cyclone.Assets=AssetBundle.LoadFromMemory(Models.Models.cyclone);
                //Firebat.Assets=AssetBundle.LoadFromMemory(Models.Models.firebat);
                //GhostAcademy.Assets=AssetBundle.LoadFromMemory(Models.Models.ghostacademy);
                //Ghost.Assets=AssetBundle.LoadFromMemory(Models.Models.ghost);
                //Liberator.Assets=AssetBundle.LoadFromMemory(Models.Models.liberator);
                Marine.Assets=AssetBundle.LoadFromMemory(Models.Models.marine);
                //Maurader.Assets=AssetBundle.LoadFromMemory(Models.Models.maurader);
                //MissileTurret.Assets=AssetBundle.LoadFromMemory(Models.Models.missileturret);
                //raven.Assets=AssetBundle.LoadFromMemory(Models.Models.raven);
                //Reaper.Assets=AssetBundle.LoadFromMemory(Models.Models.reaper);
                //SeigeTank.Assets=AssetBundle.LoadFromMemory(Models.Models.seigetank);
                //Thor.Assets=AssetBundle.LoadFromMemory(Models.Models.thor);
                //Viking.Assets=AssetBundle.LoadFromMemory(Models.Models.viking);
            }
            if(ZergEnabled==true){
                BanelingNest.Assets=AssetBundle.LoadFromMemory(Models.Models.banelingnest);
                //BroodLord.Assets=AssetBundle.LoadFromMemory(Models.Models.broodlord);
                //CreepTumor.Assets=AssetBundle.LoadFromMemory(Models.Models.creeptumor);
                //Defiler.Assets=AssetBundle.LoadFromMemory(Models.Models.defiler);
                //Hatchery.Assets=AssetBundle.LoadFromMemory(Models.Models.hatchery);
                Hydralisk.Assets=AssetBundle.LoadFromMemory(Models.Models.hydralisk);
                //Infestor.Assets=AssetBundle.LoadFromMemory(Models.Models.infestor);
                //Mutalisk.Assets=AssetBundle.LoadFromMemory(Models.Models.mutalisk);
                //Overlord.Assets=AssetBundle.LoadFromMemory(Models.Models.overlord);
                //Queen.Assets=AssetBundle.LoadFromMemory(Models.Models.queen);
                //Roach.Assets=AssetBundle.LoadFromMemory(Models.Models.roach);
                //SpawningPool.Assets=AssetBundle.LoadFromMemory(Models.Models.spawningpool);
                //SpineCrawler.Assets=AssetBundle.LoadFromMemory(Models.Models.spinecrawler);
                //SporeCrawler.Assets=AssetBundle.LoadFromMemory(Models.Models.sporecrawler);
                //SwarmHost.Assets=AssetBundle.LoadFromMemory(Models.Models.swarmhost);
                //UltraliskCavern.Assets=AssetBundle.LoadFromMemory(Models.Models.ultraliskcavern);
            }
        }
        public override void OnTowerUpgraded(Tower tower,string upgradeName,TowerModel newBaseTowerModel) {
            base.OnTowerUpgraded(tower,upgradeName,newBaseTowerModel);
            if(upgradeName.Contains("Primal")){
                var Rand=new System.Random();
                int RandNum=Rand.Next(1,3);
                if(tower.namedMonkeyKey.Contains("Hydralisk")){
                    if(RandNum==1){
                        MelonLogger.Msg(1);
                        newBaseTowerModel.GetAttackModel().range+=5;
                        newBaseTowerModel.range=newBaseTowerModel.GetAttackModel().range;
                    }
                    if(RandNum==2){
                        MelonLogger.Msg(2);
                        newBaseTowerModel.GetAttackModel().weapons[0].rate-=0.2f;
                    }
                    if(RandNum==3){
                        MelonLogger.Msg(3);
                        newBaseTowerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage+=2;
                    }
                }
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
                foreach(var tower in towers){
                    __result.towers=__result.towers.Add(tower.Item3);
                    __result.towerSet=__result.towerSet.Add(tower.Item2);
                    __result.upgrades=__result.upgrades.Add(tower.Item4);
                }
            }
        }
    }
}