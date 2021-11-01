//saves so much space with global using, no need to put separate usings in each file, just put them all in the main one
global using System;
global using Assets.Scripts.Models;
global using Assets.Scripts.Models.Towers;
global using Assets.Scripts.Models.TowerSets;
global using Assets.Scripts.Unity.UI_New.InGame.StoreMenu;
global using SC2Expansion.Utils;
global using SC2Expansion.Towers;
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
global using BTD_Mod_Helper;
global using BTD_Mod_Helper.Extensions;
global using BTD_Mod_Helper.Api.Towers;
global using BTD_Mod_Helper.Api.ModOptions;
[assembly:MelonOptionalDependencies("SC2ExpansionModOptions")]
[assembly:MelonGame("Ninja Kiwi","BloonsTD6")]
[assembly:MelonInfo(typeof(SC2Expansion.SC2Expansion),"SC2Expansion","1.5","Silentstorm#5336")]
namespace SC2Expansion{
    public class SC2Expansion:BloonsTD6Mod{
        public static ModSettingBool ProtossEnabled=Ext.ProtossEnabled;
        public static ModSettingBool TerranEnabled=Ext.TerranEnabled;
        public static ModSettingBool ZergEnabled=Ext.ZergEnabled;
        public static ModSettingBool RemoveBaseTowers=Ext.RemoveBaseTowers;
        public static ModSettingBool HeroesEnabled=Ext.HeroesEnabled;
        public override void OnApplicationStart(){
            Ext.ModVolume=1;
        }
        /*
        [HarmonyPatch(typeof(TitleScreen), nameof(TitleScreen.Start))]
    internal class TitleScreen_Start
    {
        [HarmonyPostfix]
        [HarmonyPriority(Priority.High)]
        internal static void Postfix()
        {
            foreach (var mod in MelonHandler.Mods.OfType<BloonsMod>())
            {
                ModContent.LoadAllModContent(mod);
            }

            MelonMain.DoPatchMethods(mod => mod.OnTitleScreen());
        }
    }
    */
        public override void OnUpdate(){
            if(uObject.FindObjectOfType<FXVolumeControl>()!=null){
                Ext.ModVolume=uObject.FindObjectOfType<FXVolumeControl>().volume;
            }
        }
        [HarmonyPatch(typeof(GameModelLoader),"Load")]
        public static class GameModelLoaderLoad_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref GameModel __result){
                if(RemoveBaseTowers==true){
                    __result.towerSet=__result.towerSet.Remove(a=>a.name.Contains("ShopTowerDetail"));
                }
            }
        }
        //would like this to be in gamemodelloader 
        /*public override void OnTitleScreen(){
            if(HeroesEnabled==true){
                Artanis.ArtanisSetup();
            }
        }*/
    }
}