//saves so much space with global using, no need to put separate usings in each file, just put them all in the main one
global using System;
global using Assets.Scripts.Models;
global using Assets.Scripts.Models.Towers;
global using HarmonyLib;
global using MelonLoader;
global using System.Collections.Generic;
global using UnityEngine;
global using uImage=UnityEngine.UI.Image;
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
global using static SC2Expansion.SC2Expansion;
global using Assets.Scripts.Models.Towers.TowerFilters;
global using Assets.Scripts.Simulation.Towers.Weapons;
global using Assets.Scripts.Models.Towers.Filters;
global using Assets.Scripts.Simulation.Towers.Behaviors.Abilities;
global using Assets.Scripts.Unity.Audio;
global using BTD_Mod_Helper;
global using BTD_Mod_Helper.Extensions;
global using BTD_Mod_Helper.Api.Towers;
global using BTD_Mod_Helper.Api.ModOptions;
global using UnhollowerBaseLib;
global using Assets.Scripts.Simulation.Objects;
global using System.Threading.Tasks;
global using System.Threading;
[assembly:MelonGame("Ninja Kiwi","BloonsTD6")]
[assembly:MelonInfo(typeof(SC2Expansion.SC2Expansion),"SC2Expansion","1.5","Silentstorm#5336")]
namespace SC2Expansion{
    public class SC2Expansion:BloonsTD6Mod{
        [HarmonyPatch(typeof(GameModelLoader),"Load")]
        public static class GameModelLoaderLoad_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref GameModel __result){
                if(RemoveBaseTowers==true){
                    __result.towerSet=__result.towerSet.Empty();
                }
            }
        }
        public static void SetUpgradeSounds(TowerModel towerModel,string soundToUse){
            towerModel.GetBehavior<CreateSoundOnUpgradeModel>().sound.assetId=soundToUse;
            towerModel.GetBehavior<CreateSoundOnUpgradeModel>().sound1.assetId=soundToUse;
            towerModel.GetBehavior<CreateSoundOnUpgradeModel>().sound2.assetId=soundToUse;
            towerModel.GetBehavior<CreateSoundOnUpgradeModel>().sound3.assetId=soundToUse;
            towerModel.GetBehavior<CreateSoundOnUpgradeModel>().sound4.assetId=soundToUse;
            towerModel.GetBehavior<CreateSoundOnUpgradeModel>().sound5.assetId=soundToUse;
            towerModel.GetBehavior<CreateSoundOnUpgradeModel>().sound6.assetId=soundToUse;
            towerModel.GetBehavior<CreateSoundOnUpgradeModel>().sound7.assetId=soundToUse;
            towerModel.GetBehavior<CreateSoundOnUpgradeModel>().sound8.assetId=soundToUse;
        }
        public static void LoadImage(AssetBundle assetBundle,string asset,uImage image){
            var text=assetBundle.LoadAsset(asset).Cast<Texture2D>();
            image.canvasRenderer.SetTexture(text);
            image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
        }
        public static void LoadModel(AssetBundle assetBundle,string asset,Factory factory,Il2CppSystem.Action<UnityDisplayNode>action){
            var udn=uObject.Instantiate(assetBundle.LoadAsset(asset).Cast<GameObject>(),factory.PrototypeRoot).AddComponent<UnityDisplayNode>();
            udn.transform.position=new(-30000,-30000);
            udn.name=asset;
            udn.isSprite=false;
            action.Invoke(udn);
            DisplayDict.Add(asset,udn);
        }
        public override void OnUpdate() {
            if(DisplayDict.Count!=0){
                foreach(var proto in DisplayDict.Values)uObject.Destroy(proto.gameObject);
                DisplayDict.Clear();
            }
        }
        public static readonly ModSettingBool ProtossEnabled=true;
        public static readonly ModSettingBool TerranEnabled=true;
        public static readonly ModSettingBool ZergEnabled=true;
        public static readonly ModSettingBool RemoveBaseTowers=false;
        public static readonly ModSettingBool HeroesEnabled=true;
        public static Dictionary<string,UnityDisplayNode>DisplayDict=new();
        public static AudioFactory AudioFactoryInstance;
        public class ProtossSet:ModTowerSet{
            public override string DisplayName=>"Protoss";
            public override string Container=>"ProtossContainer";
            public override string Button=>"ProtossButton";
            public override string ContainerLarge=>Container;
            public override string Portrait=>"ProtossHex";
        }
        public class TerranSet:ModTowerSet{
            public override string DisplayName=>"Terran";
            public override string Container=>"TerranContainer";
            public override string Button=>"TerranButton";
            public override string ContainerLarge=>Container;
            public override string Portrait=>"TerranPortrait";
        }
        public class ZergSet:ModTowerSet{
            public override string DisplayName=>"Zerg";
            public override string Container=>"ZergContainer";
            public override string Button=>"ZergButton";
            public override string ContainerLarge=>Container;
            public override string Portrait=>"ZergCreep";
        }
    }
}