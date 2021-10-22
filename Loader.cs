//saves so much space with global using, no need to put separate usings in each file, just put them all in the main one
global using System;
global using Assets.Scripts.Models;
global using Assets.Scripts.Models.Towers;
global using Assets.Scripts.Models.TowerSets;
global using Assets.Scripts.Unity.UI_New.InGame.StoreMenu;
global using SC2Expansion.Utils;
global using HarmonyLib;
global using MelonLoader;
global using System.Collections.Generic;
global using UnityEngine;
global using Image=UnityEngine.UI.Image;
global using Assets.Scripts.Models.GenericBehaviors;
global using Assets.Scripts.Models.Map;
global using Assets.Scripts.Unity.Display;
global using Assets.Scripts.Utils;
global using System.Linq;
global using uObject=UnityEngine.Object;
global using Assets.Scripts.Models.Towers.Behaviors.Attack;
global using Assets.Scripts.Models.Towers.Behaviors;
global using Assets.Scripts.Models.Towers.Behaviors.Emissions;
global using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
global using Assets.Scripts.Unity;
global using Assets.Scripts.Models.Towers.Behaviors.Abilities;
global using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
global using Assets.Scripts.Simulation.Towers;
global using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
global using UnhollowerBaseLib;
global using Assets.Scripts.Models.Towers.TowerFilters;
global using Assets.Scripts.Simulation.Towers.Weapons;
global using Assets.Scripts.Models.Towers.Filters;
global using Assets.Scripts.Simulation.Towers.Behaviors.Abilities;
global using Assets.Scripts.Simulation.Objects;
global using UnityEngine.UI;
global using UnityEngine.Events;
global using System.IO;
global using System.IO.Pipes;
global using System.Threading.Tasks;
global using System.Reflection;
global using Assets.Scripts.Models.Towers.Upgrades;
global using NinjaKiwi.Common;
global using Assets.Scripts.Unity.Player;
global using Newtonsoft.Json;
[assembly:MelonOptionalDependencies("SC2ExpansionModOptions")]
[assembly:MelonGame("Ninja Kiwi","BloonsTD6")]
[assembly:MelonInfo(typeof(SC2Expansion.SC2Expansion),"SC2Expansion","1.5","Silentstorm#5336")]
namespace SC2Expansion{
    public class SC2Expansion:MelonMod{
        public override void OnApplicationStart(){
            Ext.ModVolume=1;
            Ext.ModHelperLoaded=false;
            if(MelonHandler.IsModAlreadyLoaded("BloonsTD6 Mod Helper")){
                if(File.Exists(MelonHandler.ModsDirectory+"\\SC2ExpansionModOptions.dll")&&MelonHandler.Mods.First(a=>a.Info.Name.Contains("SC2ExpansionModOptions")).Info.Version=="1.0.0"){
                        Ext.ModHelperLoaded=true;
                        Ext.LoadSettings();
                }else{
                    MelonLogger.Msg("Mod Options library not extracted or out of date, installing");
                    File.WriteAllBytes(MelonHandler.ModsDirectory+"\\SC2ExpansionModOptions.dll",Models.Models.SC2ExpansionModOptions);
                    MelonLogger.Msg("Mod Options library installed, please restart your game to take advantage of this");
                }
            }
        }
        [HarmonyPatch(typeof(Btd6Player),"CheckForNewParagonPipEvent")]
        public class Btd6PlayerIsBad{
            [HarmonyPrefix]
            public static bool Prefix(string checkSpecificTowerId,string checkSpecificTowerSet,ref bool __result)=>__result=false;
        }
        public static void SettingsClientStart(){
            var client=new NamedPipeClientStream("SC2ExpansionModOptions");
            client.Connect();
            StreamWriter writer=new StreamWriter(client);
            StreamReader reader=new StreamReader(client);
            while(true){
                writer.WriteLine("ProtossEnabled");
                writer.Flush();
                Ext.ProtossEnabled=reader.ReadLine();
                writer.WriteLine("TerranEnabled");
                writer.Flush();
                Ext.TerranEnabled=reader.ReadLine();
                writer.WriteLine("ZergEnabled");
                writer.Flush();
                Ext.ZergEnabled=reader.ReadLine();
                writer.WriteLine("RemoveBaseTowers");
                writer.Flush();
                Ext.RemoveBaseTowers=reader.ReadLine();
                writer.WriteLine("HeroesEnabled");
                writer.Flush();
                Ext.HeroesEnabled=reader.ReadLine();
            }
        }
        public override void OnUpdate(){
            if(uObject.FindObjectOfType<FXVolumeControl>()!=null){
                Ext.ModVolume=uObject.FindObjectOfType<FXVolumeControl>().volume;
            }
        }
        [HarmonyPatch(typeof(GameModelLoader),"Load")]
        public static class Load_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref GameModel __result){
                if(Ext.ModHelperLoaded){
                    Task.Run(()=>SettingsClientStart());
                    MelonLogger.Msg("Client started");
                }
                /*if(RemoveBaseTowers==true){
                    __result.towerSet=__result.towerSet.Remove(a=>a.name.Contains("ShopTowerDetail"));
                }*/
            }
        }
        /*public override void OnTowerUpgraded(Tower tower,string upgradeName,TowerModel newBaseTowerModel){
            base.OnTowerUpgraded(tower,upgradeName,newBaseTowerModel);
            if(upgradeName.Contains("Primal")){
                int RandNum=new System.Random().Next(1,3);
                if(tower.namedMonkeyKey.Contains("Hydralisk")){
                    if(RandNum==1){
                        newBaseTowerModel.GetAttackModel().range+=5;
                        newBaseTowerModel.range=newBaseTowerModel.GetAttackModel().range;
                    }
                    if(RandNum==2){
                        newBaseTowerModel.GetAttackModel().weapons[0].rate-=0.2f;
                    }
                    if(RandNum==3){
                        newBaseTowerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage+=2;
                    }
                }
                if(tower.namedMonkeyKey.Contains("Mutalisk")){
                    if(RandNum==1){
                        newBaseTowerModel.GetAttackModel().range+=8;
                        newBaseTowerModel.range=newBaseTowerModel.GetAttackModel().range;
                    }
                    if(RandNum==2){
                        newBaseTowerModel.GetAttackModel().weapons[0].rate-=0.25f;
                    }
                    if(RandNum==3){
                        newBaseTowerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage+=3;
                    }
                }
                if(tower.namedMonkeyKey.Contains("Ultralisk")){
                    if(RandNum==1){
                        newBaseTowerModel.GetAttackModel().weapons[1].projectile.pierce+=5;
                    }
                    if(RandNum==2){
                        newBaseTowerModel.GetAttackModel().weapons[1].projectile.GetBehavior<TravelAlongPathModel>().speedFrames+=0.2f;
                    }
                    if(RandNum==3){
                        newBaseTowerModel.GetAttackModel().weapons[1].projectile.GetDamageModel().damage+=3;
                    }
                }
                if(tower.namedMonkeyKey.Contains("SpawningPool")){
                    if(RandNum==1){
                        newBaseTowerModel.GetAttackModel().weapons[1].projectile.pierce+=4;
                    }
                    if(RandNum==2){
                        newBaseTowerModel.GetAttackModel().weapons[1].projectile.GetBehavior<TravelAlongPathModel>().speedFrames+=0.15f;
                    }
                    if(RandNum==3){
                        newBaseTowerModel.GetAttackModel().weapons[1].projectile.GetDamageModel().damage+=2;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(StandardTowerPurchaseButton),nameof(StandardTowerPurchaseButton.UpdateTowerDisplay))]
        public class SetBG{
            [HarmonyPostfix]
            public static void Postfix(ref StandardTowerPurchaseButton __instance){
                __instance.bg=__instance.gameObject.GetComponent<Image>();
                if(__instance.baseTowerModel.emoteSpriteLarge!=null){
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
                }
            }
            private static Texture2D LoadTextureFromBytes(byte[] FileData){
                Texture2D Tex2D=new(2,2);
                if(ImageConversion.LoadImage(Tex2D,FileData))return Tex2D;
                return null;
            }
            private static Sprite LoadSprite(Texture2D text){
                return Sprite.Create(text,new(0,0,text.width,text.height),new());
            }
        }*/
    }
}