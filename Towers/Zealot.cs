using Assets.Scripts.Models;
using Assets.Scripts.Models.Bloons.Behaviors;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Map;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Upgrades;
using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Simulation.Towers.Weapons;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Utils;
using SC2Expansion.Utils;
using HarmonyLib;
using NinjaKiwi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using Object=UnityEngine.Object;
using MelonLoader;
namespace SC2Expansion.Towers{
    public class Zealot{
        public static string name="Zealot";
        public static UpgradeModel[]GetUpgrades(){
            return new UpgradeModel[]{
                new("Charge",250,0,new("ZealotChargeIcon"),0,1,0,"","Grooved Spines"),
                new("Frenzy",725,0,new("ZealotFrenzyIcon"),0,2,0,"","Frenzy"),
                new("Morph into Lurker",975,0,new("ZealotLurkerIcon"),0,3,0,"","Morph into Lurker"),
                new("Seismic Spines",1250,0,new("ZealotSeismicSpinesIcon"),0,4,0,"","Seismic Spines")
                //new("Morph into Impaler",1750,0,new("ZealotImpalerIcon"),0,5,0,"","Morph into Impaler")
            };
        }
        public static(TowerModel,TowerDetailsModel,TowerModel[],UpgradeModel[])GetTower(GameModel gameModel){
            var ZealotDetails=gameModel.towerSet[0].Clone().Cast<TowerDetailsModel>();
            ZealotDetails.towerId=name;
            ZealotDetails.towerIndex=33;
            if(!LocalizationManager.Instance.textTable.ContainsKey("Grooved Spines Description"))LocalizationManager.Instance.textTable.
                    Add("Grooved Spines Description","Increases attack range");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Frenzy Description"))LocalizationManager.Instance.textTable.
                    Add("Frenzy Description","Increases attack speed by 50% for 15 seconds");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Morph into Lurker Description"))LocalizationManager.Instance.textTable.
                    Add("Morph into Lurker Description","Morphs into a lurker changing the attack into a straight line that damages everything in its path");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Seismec Spines Description"))LocalizationManager.Instance.textTable.
                    Add("Seismic Spines Description","Increases attack range and damage");
            /*if(!LocalizationManager.Instance.textTable.ContainsKey("Morph into Impaler Description"))LocalizationManager.Instance.textTable.
                    Add("Morph into Impaler Description","Deals massive damage to a single target with long range");*/
            return (GetT0(gameModel),ZealotDetails,new[]{GetT0(gameModel),GetT1(gameModel),GetT2(gameModel)},GetUpgrades());
        }
        public static TowerModel GetT0(GameModel gameModel){
            var Zealot=gameModel.towers[0].Clone().Cast<TowerModel>();
            Zealot.name=name;
            Zealot.baseId=name;
            Zealot.display="ZealotModel";
            Zealot.portrait=new("ZealotIcon");
            Zealot.icon=new("ZealotIcon");
            Zealot.towerSet="Primary";
            Zealot.emoteSpriteLarge=new("Protoss");
            Zealot.radius=7;
            Zealot.cost=500;
            Zealot.range=40;
            Zealot.towerSize=TowerModel.TowerSize.XL;
            Zealot.footprint.ignoresPlacementCheck=true;
            Zealot.cachedThrowMarkerHeight=10;
            Zealot.areaTypes=new(1);
            Zealot.areaTypes[0]=AreaType.land;
            Zealot.upgrades=new UpgradePathModel[]{new("Grooved Spines",name+"-100")};
            for(var i=0;i<Zealot.behaviors.Count;i++){
                var b=Zealot.behaviors[i];
                if(b.GetIl2CppType()==Il2CppType.Of<AttackModel>()){
                    var att=gameModel.towers.First(a=>a.name.Contains("DartMonkey")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();
                    att.weapons[0].name="ZealotSpine";
                    att.weapons[0].rate=0.7f;
                    att.weapons[0].rateFrames=200;
                    att.range=40;
                    for(var j=0;j<att.weapons[0].projectile.behaviors.Length;j++){
                        var pb=att.weapons[0].projectile.behaviors[j];
                        if(pb.GetIl2CppType()==Il2CppType.Of<DamageModel>()){
                            var d=pb.Cast<DamageModel>();
                            d.damage=2;
                            pb=d;
                        }
                    }
                    Zealot.behaviors[i]=att;
                }
                if(b.GetIl2CppType()==Il2CppType.Of<DisplayModel>()){
                    var display=b.Cast<DisplayModel>();
                    display.display="ZealotModel";
                    b=display;
                }
            }
            return Zealot;
        }
        public static TowerModel GetT1(GameModel gameModel){
            var Zealot=gameModel.towers[0].Clone().Cast<TowerModel>();
            Zealot.name=name+"-100";
            Zealot.baseId=name;
            Zealot.tier=100;
            Zealot.tiers=new int[]{1,0,0};
            Zealot.display="ZealotModel";
            Zealot.portrait=new("ZealotIcon");
            Zealot.icon=new("ZealotIcon");
            Zealot.towerSet="Primary";
            Zealot.emoteSpriteLarge=new("Zerg");
            Zealot.radius=15;
            Zealot.range=50;
            Zealot.towerSize=TowerModel.TowerSize.XL;
            Zealot.footprint.ignoresPlacementCheck=true;
            Zealot.cachedThrowMarkerHeight=10;
            Zealot.areaTypes=new(1);
            Zealot.areaTypes[0]=AreaType.land;
            Zealot.appliedUpgrades=new(new[]{"Grooved Spines"});
            Zealot.upgrades=new[]{new UpgradePathModel("Frenzy",name+"-200")};
            for(var i=0;i<Zealot.behaviors.Count;i++){
                var b=Zealot.behaviors[i];
                if(b.GetIl2CppType()==Il2CppType.Of<AttackModel>()){
                    var att=gameModel.towers.First(a=>a.name.Contains("DartMonkey-001")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();
                    att.weapons[0].rate=0.82f;
                    att.weapons[0].rateFrames=56*5;
                    att.range=50;
                    att.weapons[0].projectile.display="ZealotSpine";
                    for(var j=0;j<att.weapons[0].projectile.behaviors.Length;j++){
                        var pb=att.weapons[0].projectile.behaviors[j];
                        if(pb.GetIl2CppType()==Il2CppType.Of<DamageModel>()){
                            var d=pb.Cast<DamageModel>();
                            d.damage=2;
                            pb=d;
                        }
                    }
                }
            }
            return Zealot;
        }
        public static TowerModel GetT2(GameModel gameModel){
            var Zealot=gameModel.towers[0].Clone().Cast<TowerModel>();
            Zealot.name=name+"-200";
            Zealot.baseId=name;
            Zealot.tier=2;
            Zealot.tiers=new int[]{2,0,0};
            Zealot.display="ZealotModel";
            Zealot.portrait=new("ZealotIcon");
            Zealot.icon=new("ZealotIcon");
            Zealot.towerSet="Primary";
            Zealot.emoteSpriteLarge=new("Zerg");
            Zealot.radius=15;
            Zealot.range=50;
            Zealot.towerSize=TowerModel.TowerSize.XL;
            Zealot.footprint.ignoresPlacementCheck=true;
            Zealot.cachedThrowMarkerHeight=10;
            Zealot.areaTypes=new(1);
            Zealot.areaTypes[0]=AreaType.land;
            Zealot.appliedUpgrades=new(new[]{"Grooved Spines","Frenzy"});
            Zealot.upgrades=new(0);
            var att = gameModel.towers.First(a => a.name.Contains("DartMonkey")).behaviors.First(a => a.GetIl2CppType()==Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();
            for(var i=0;i<Zealot.behaviors.Count;i++){
                var b=Zealot.behaviors[i];
                if(b.GetIl2CppType()==Il2CppType.Of<AttackModel>()){
                    att.weapons[0].rate=0.82f;
                    att.weapons[0].rateFrames=1;
                    att.range=50;
                    att.weapons[0].projectile.display="ZealotSpine";
                    for(var j=0;j<att.weapons[0].projectile.behaviors.Length;j++){
                        var pb=att.weapons[0].projectile.behaviors[j];
                        if(pb.GetIl2CppType()==Il2CppType.Of<DamageModel>()){
                            var d=pb.Cast<DamageModel>();
                            d.damage=2;
                            pb=d;
                        }
                    }
                }
            }
            var ab=gameModel.towers.First(a=>a.name.Equals("BoomerangMonkey-040")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AbilityModel>()).Clone().Cast<AbilityModel>();
            ab.name="Frenzy";
            ab.displayName="Frenzy";
            ab.icon=new("ZealotFrenzyIcon");
            ab.cooldown=10;
            Zealot.behaviors=Zealot.behaviors.Add(att,ab);
            return Zealot;
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("ZealotModel")){
                    var udn=GetZealot(__instance.PrototypeRoot);
                    udn.name="Zealot";
                    var a=Assets.LoadAsset("ZealotMaterial");
                    udn.genericRenderers[0].material=a.Cast<Material>();
                    udn.RecalculateGenericRenderers();
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(protos.ContainsKey(objectId)){
                    onComplete.Invoke(protos[objectId]);
                    return false;
                }
                return true;
            }
        }
        private static AssetBundle __asset;
        public static AssetBundle Assets{
            get=>__asset;
            set=>__asset=value;
        }
        public static UnityDisplayNode GetZealot(Transform transform){
            var udn=Object.Instantiate(Assets.LoadAsset("ZealotModel").Cast<GameObject>(),transform).AddComponent<UnityDisplayNode>();
            udn.Active=false;
            udn.transform.position=new(-3000,0);
            return udn;
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.ProtoFlush))]
        public class PrototypeFlushUDN_Patch{
            [HarmonyPostfix]
            public static void Postfix(){
                foreach(var proto in PrototypeUDN_Patch.protos.Values)Object.Destroy(proto.gameObject);
                PrototypeUDN_Patch.protos.Clear();
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),nameof(ResourceLoader.LoadSpriteFromSpriteReferenceAsync))]
        public record ResourceLoader_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.Equals("ZealotIcon")){
                    var b=Assets.LoadAsset("ZealotIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("ZealotFrenzyIcon")){
                    var b=Assets.LoadAsset("ZealotFrenzyIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("ZealotGroovedSpinesIcon")){
                    var b=Assets.LoadAsset("ZealotGroovedSpinesIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),nameof(Weapon.SpawnDart))]
        public static class WI{
            [HarmonyPrefix]
            public static void Prefix(ref Weapon __instance)=>RunAnimations(__instance);
            private static async Task RunAnimations(Weapon __instance){
                if(__instance.weaponModel.name.Contains("ZealotSpine")){
                    MelonLogger.Msg(__instance.attack.tower.namedMonkeyKey);
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("Armature Object|Armature ObjectAttack");
                }
            }
        }
    }
}