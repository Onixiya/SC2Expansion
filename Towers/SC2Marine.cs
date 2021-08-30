using Assets.Scripts.Models;
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
//Stuff's called SC2Marine to avoid any potential conflict with the already existing marine
namespace SC2Expansion.Towers{
    public class SC2Marine{
        public static string name="Marine "; //Space in the name to avoid messing with the existing marine, closest we get to any conflict
        public static UpgradeModel[]GetUpgrades(){
            return new UpgradeModel[]{
                new("U-238 Shells",350,0,new("SC2MarineU238ShellsIcon"),0,1,0,"","U-238 Shells"),
                new("Laser Targeting System",500,0,new("SC2MarineLaserTargetingSystemIcon"),0,2,0,"","Laser Targeting System"),
                new("Stimpacks",875,0,new("SC2MarineStimpacksIcon"),0,3,0,"","Stimpacks"),
                new("Warpig",1400,0,new("SC2MarineWarpigIcon"),0,4,0,"","Warpig")
                //new("Tychus Findlay",2500,0,new("SC2MarineTychusIcon"),0,5,0,"","Tychus Findlay")
            };
        }
        public static(TowerModel,TowerDetailsModel,TowerModel[],UpgradeModel[])GetTower(GameModel gameModel){
            var SC2MarineDetails=gameModel.towerSet.First((a=>a.name.Contains("DartMonkey"))).Clone().Cast<TowerDetailsModel>();
            SC2MarineDetails.towerId=name;
            SC2MarineDetails.towerIndex=32;
            if(!LocalizationManager.Instance.textTable.ContainsKey("U-238 Shells Description"))LocalizationManager.Instance.textTable.
                    Add("U-238 Shells Description","Increases damage and attack range");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Laser Targeting System Description"))LocalizationManager.Instance.textTable.
                    Add("Laser Targeting System Description","Increases attack range slightly and provides camo detection");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Stimpacks Description"))LocalizationManager.Instance.textTable.
                    Add("Stimpacks Description","Increases attack speed by 50% for 10 seconds");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Warpig Description"))LocalizationManager.Instance.textTable.
                    Add("Warpig Description","Increases damage and attack speed slightly");
            /*if(!LocalizationManager.Instance.textTable.ContainsKey("Tychus Findlay Description"))LocalizationManager.Instance.textTable.
                    Add("Tychus Findlay Description","Increases attack speed, increases damage and throws a grenade occasionally");*/
            return (GetT0(gameModel),SC2MarineDetails,new[]{GetT0(gameModel),GetT1(gameModel),GetT2(gameModel),GetT3(gameModel),GetT4(gameModel)/*,GetT5(gameModel)*/},GetUpgrades());
        }
        public static TowerModel GetT0(GameModel gameModel){
            var SC2Marine=gameModel.towers[0].Clone().Cast<TowerModel>();
            SC2Marine.name=name;
            SC2Marine.baseId=name;
            SC2Marine.display="SC2MarineModel";
            SC2Marine.portrait=new("SC2MarineIcon");
            SC2Marine.icon=new("SC2MarineIcon");
            SC2Marine.towerSet="Primary";
            SC2Marine.emoteSpriteLarge=new("Terran");
            SC2Marine.radius=5;
            SC2Marine.cost=400;
            SC2Marine.range=35;
            SC2Marine.footprint.ignoresPlacementCheck=true;
            SC2Marine.cachedThrowMarkerHeight=10;
            SC2Marine.areaTypes=new(1);
            SC2Marine.areaTypes[0]=AreaType.land;
            SC2Marine.upgrades=new UpgradePathModel[]{new("U-238 Shells",name+"-100")};
            for(var i=0;i<SC2Marine.behaviors.Count;i++){
                var b=SC2Marine.behaviors[i];
                if(b.GetIl2CppType()==Il2CppType.Of<AttackModel>()){
                    var att=gameModel.towers.First(a=>a.name.Contains("DartMonkey")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();
                    att.weapons[0].name="SC2MarineBullet";
                    att.weapons[0].rate=0.7f;
                    att.weapons[0].rateFrames=1;
                    att.range=35;
                    att.weapons[0].projectile.display="SC2MarineBulletDisplay";
                    for(var j=0;j<att.weapons[0].projectile.behaviors.Length;j++){
                        var pb=att.weapons[0].projectile.behaviors[j];
                        if(pb.GetIl2CppType()==Il2CppType.Of<DamageModel>()){
                            var d=pb.Cast<DamageModel>();
                            d.damage=1;
                            pb=d;
                        }
                    }
                    SC2Marine.behaviors[i]=att;
                }
                if(b.GetIl2CppType()==Il2CppType.Of<DisplayModel>()){
                    var display=b.Cast<DisplayModel>();
                    display.display="SC2MarineModel";
                    b=display;
                }
            }
            return SC2Marine;
        }
        public static TowerModel GetT1(GameModel gameModel){
            var SC2Marine=gameModel.towers[0].Clone().Cast<TowerModel>();
            SC2Marine.name=name+"-100";
            SC2Marine.baseId=name;
            SC2Marine.tier=1;
            SC2Marine.tiers=new int[]{1,0,0};
            SC2Marine.display="SC2MarineModel";
            SC2Marine.portrait=new("SC2MarineIcon");
            SC2Marine.icon=new("SC2MarineIcon");
            SC2Marine.towerSet="Primary";
            SC2Marine.emoteSpriteLarge=new("Terran");
            SC2Marine.radius=45;
            SC2Marine.range=45;
            SC2Marine.footprint.ignoresPlacementCheck=true;
            SC2Marine.cachedThrowMarkerHeight=10;
            SC2Marine.areaTypes=new(1);
            SC2Marine.areaTypes[0]=AreaType.land;
            SC2Marine.appliedUpgrades=new(new[]{"U-238 Shells"});
            SC2Marine.upgrades=new[]{new UpgradePathModel("Laser Targeting System",name+"-200")};
            var att=gameModel.towers.First(a=>a.name.Contains("DartMonkey")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();
            for(var i=0;i<SC2Marine.behaviors.Count;i++){
                var b=SC2Marine.behaviors[i];
                if(b.GetIl2CppType()==Il2CppType.Of<AttackModel>()){
                    att.weapons[0].name="SC2MarineBullet";
                    att.weapons[0].rate=0.7f;
                    att.weapons[0].rateFrames=1;
                    att.range=45;
                    att.weapons[0].projectile.display="SC2MarineBulletDisplay";
                    for(var j=0;j<att.weapons[0].projectile.behaviors.Length;j++){
                        var pb=att.weapons[0].projectile.behaviors[j];
                        if(pb.GetIl2CppType()==Il2CppType.Of<DamageModel>()){
                            var d=pb.Cast<DamageModel>();
                            d.damage=2;
                            pb=d;
                        }
                    }
                }
                if(b.GetIl2CppType()==Il2CppType.Of<DisplayModel>()){
                    var display=b.Cast<DisplayModel>();
                    display.display="SC2MarineModel";
                    b=display;
                }
            }
            SC2Marine.behaviors=SC2Marine.behaviors.Add(att);
            return SC2Marine;
        }
        public static TowerModel GetT2(GameModel gameModel){
            var SC2Marine=gameModel.towers[0].Clone().Cast<TowerModel>();
            SC2Marine.name=name+"-200";
            SC2Marine.baseId=name;
            SC2Marine.tier=2;
            SC2Marine.tiers=new int[]{2,0,0};
            SC2Marine.display="SC2MarineModel";
            SC2Marine.portrait=new("SC2MarineIcon");
            SC2Marine.icon=new("SC2MarineIcon");
            SC2Marine.towerSet="Primary";
            SC2Marine.emoteSpriteLarge=new("Terran");
            SC2Marine.radius=5;
            SC2Marine.range=50;
            SC2Marine.footprint.ignoresPlacementCheck=true;
            SC2Marine.cachedThrowMarkerHeight=10;
            SC2Marine.areaTypes=new(1);
            SC2Marine.areaTypes[0]=AreaType.land;
            SC2Marine.appliedUpgrades=new(new[]{"U-238 Shells","Laser Targeting System"});
            SC2Marine.upgrades=new[]{new UpgradePathModel("Stimpacks",name+"-300")};
            var att=gameModel.towers.First(a=>a.name.Contains("DartMonkey")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();
            for(var i=0;i<SC2Marine.behaviors.Count;i++){
                var b=SC2Marine.behaviors[i];
                att.weapons[0].name="SC2MarineBullet";
                att.weapons[0].rate=0.7f;
                att.weapons[0].rateFrames=1;
                att.range=50;
                att.weapons[0].projectile.display="SC2MarineBulletDisplay";
                for(var j=0;j<att.weapons[0].projectile.behaviors.Length;j++){
                    var pb=att.weapons[0].projectile.behaviors[j];
                    if(pb.GetIl2CppType()==Il2CppType.Of<DamageModel>()){
                        var d=pb.Cast<DamageModel>();
                        d.damage=2;
                        pb=d;
                    }
                }
            }
            SC2Marine.behaviors=SC2Marine.behaviors.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true),att);
            return SC2Marine;
        }
        public static TowerModel GetT3(GameModel gameModel){
            var SC2Marine=gameModel.towers[0].Clone().Cast<TowerModel>();
            SC2Marine.name=name+"-300";
            SC2Marine.baseId=name;
            SC2Marine.tier=3;
            SC2Marine.tiers=new int[]{3,0,0};
            SC2Marine.display="SC2MarineModel";
            SC2Marine.portrait=new("SC2MarineIcon");
            SC2Marine.icon=new("SC2MarineIcon");
            SC2Marine.towerSet="Primary";
            SC2Marine.emoteSpriteLarge=new("Terran");
            SC2Marine.radius=5;
            SC2Marine.range=50;
            SC2Marine.footprint.ignoresPlacementCheck=true;
            SC2Marine.cachedThrowMarkerHeight=10;
            SC2Marine.areaTypes=new(1);
            SC2Marine.areaTypes[0]=AreaType.land;
            SC2Marine.appliedUpgrades=new(new[]{"U-238 Shells","Laser Targeting System","Stimpacks"});
            SC2Marine.upgrades=new[]{new UpgradePathModel("Warpig",name+"-400")};
            var att=gameModel.towers.First(a=>a.name.Contains("DartMonkey")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();
            for(var i=0;i<SC2Marine.behaviors.Count;i++){
                var b=SC2Marine.behaviors[i];
                att.weapons[0].rate=0.7f;
                att.weapons[0].rateFrames=1;
                att.range=50;
                att.weapons[0].projectile.display="SC2MarineBulletDisplay";
                for(var j=0;j<att.weapons[0].projectile.behaviors.Length;j++){
                    var pb=att.weapons[0].projectile.behaviors[j];
                    if(pb.GetIl2CppType()==Il2CppType.Of<DamageModel>()){
                        var d=pb.Cast<DamageModel>();
                        d.damage=2;
                        pb=d;
                    }
                }
            }
            var ab=gameModel.towers.First(a=>a.name.Equals("BoomerangMonkey-040")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AbilityModel>()).Clone().Cast<AbilityModel>();
            ab.name="Stimpacks";
            ab.displayName="Stimpacks";
            ab.icon=new("SC2MarineStimpacksIcon");
            ab.cooldown=40;
            ab.maxActivationsPerRound=1;
            SC2Marine.behaviors=SC2Marine.behaviors.Add(att,new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true),ab);
            return SC2Marine;
        }
        public static TowerModel GetT4(GameModel gameModel){
            var SC2Marine=gameModel.towers[0].Clone().Cast<TowerModel>();
            SC2Marine.name=name+"-400";
            SC2Marine.baseId=name;
            SC2Marine.tier=4;
            SC2Marine.tiers=new int[]{4,0,0};
            SC2Marine.display="SC2MarineWarpigModel";
            SC2Marine.portrait=new("SC2MarineWarpigIcon");
            SC2Marine.icon=new("SC2MarineWarpigIcon");
            SC2Marine.towerSet="Primary";
            SC2Marine.emoteSpriteLarge=new("Terran");
            SC2Marine.radius=5;
            SC2Marine.range=50;
            SC2Marine.footprint.ignoresPlacementCheck=true;
            SC2Marine.cachedThrowMarkerHeight=10;
            SC2Marine.areaTypes=new(1);
            SC2Marine.areaTypes[0]=AreaType.land;
            SC2Marine.appliedUpgrades=new(new[]{"U-238 Shells","Laser Targeting System","Stimpacks","Warpig"});
            SC2Marine.upgrades=new(0);
            var att=gameModel.towers.First(a=>a.name.Contains("DartMonkey")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();
            for(var i=0;i<SC2Marine.behaviors.Count;i++){
                var b=SC2Marine.behaviors[i];
                att.weapons[0].rate=0.55f;
                att.weapons[0].rateFrames=1;
                att.range=50;
                att.weapons[0].projectile.display=null;
                for(var j=0;j<att.weapons[0].projectile.behaviors.Length;j++){
                    var pb=att.weapons[0].projectile.behaviors[j];
                    if(pb.GetIl2CppType()==Il2CppType.Of<DamageModel>()){
                        var d=pb.Cast<DamageModel>();
                        d.damage=4;
                        pb=d;
                    }
                }
            }
            var ab=gameModel.towers.First(a=>a.name.Equals("BoomerangMonkey-040")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AbilityModel>()).Clone().Cast<AbilityModel>();
            ab.name="Stimpacks";
            ab.displayName="Stimpacks";
            ab.icon=new("SC2MarineStimpacksIcon");
            ab.cooldown=10;
            ab.maxActivationsPerRound=1;
            SC2Marine.behaviors=SC2Marine.behaviors.Add(att,new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true),ab);
            return SC2Marine;
        }
        /*public static TowerModel GetT5(GameModel gameModel){
            var SC2Marine=gameModel.towers[0].Clone().Cast<TowerModel>();
            SC2Marine.name=name+"-500";
            SC2Marine.baseId=name;
            SC2Marine.tier=5;
            SC2Marine.tiers=new int[]{5,0,0};
            SC2Marine.display="SC2MarineWarpigModel";
            SC2Marine.portrait=new("SC2MarineWarpigIcon");
            SC2Marine.icon=new("SC2MarineWarpigIcon");
            SC2Marine.towerSet="Primary";
            SC2Marine.emoteSpriteLarge=new("Terran");
            SC2Marine.radius=5;
            SC2Marine.range=50;
            SC2Marine.footprint.ignoresPlacementCheck=true;
            SC2Marine.cachedThrowMarkerHeight=10;
            SC2Marine.areaTypes=new(1);
            SC2Marine.areaTypes[0]=AreaType.land;
            SC2Marine.appliedUpgrades=new(new[]{"U-238 Shells","Laser Targeting System","Stimpacks","Super Stimpacks"});
            SC2Marine.upgrades=new(0);
            var att=gameModel.towers.First(a=>a.name.Contains("DartMonkey")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();
            for(var i=0;i<SC2Marine.behaviors.Count;i++){
                var b=SC2Marine.behaviors[i];
                att.weapons[0].rate=0.7f;
                att.weapons[0].rateFrames=1;
                att.range=50;
                att.weapons[0].projectile.display="SC2MarineBulletDisplay";
                for(var j=0;j<att.weapons[0].projectile.behaviors.Length;j++){
                    var pb=att.weapons[0].projectile.behaviors[j];
                    if(pb.GetIl2CppType()==Il2CppType.Of<DamageModel>()){
                        var d=pb.Cast<DamageModel>();
                        d.damage=2;
                        pb=d;
                    }
                }
            }
            var ab=gameModel.towers.First(a=>a.name.Equals("BoomerangMonkey-040")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AbilityModel>()).Clone().Cast<AbilityModel>();
            ab.name="Super Stimpacks";
            ab.displayName="Super Stimpacks";
            ab.icon=new("SC2MarineSuperStimpacksIcon");
            ab.cooldown=10;
            ab.maxActivationsPerRound=1;
            SC2Marine.behaviors=SC2Marine.behaviors.Add(att,new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true));
            return SC2Marine;
        }*/
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("SC2MarineModel")){
                    var udn=GetSC2Marine(__instance.PrototypeRoot,"SC2MarineModel");
                    udn.name="SC2Marine";
                    var a=Assets.LoadAsset("SC2MarineMaterial");
                    udn.genericRenderers[0].material=a.Cast<Material>();
                    udn.RecalculateGenericRenderers();
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("SC2MarineWarpigModel")){
                    var udn=GetSC2Marine(__instance.PrototypeRoot,"SC2MarineWarpigModel");
                    udn.name="SC2Marine";
                    var a=Assets.LoadAsset("SC2MarineWarpigMaterial");
                    udn.genericRenderers[0].material=a.Cast<Material>();
                    udn.genericRenderers[1].material=a.Cast<Material>();
                    udn.genericRenderers[2].material=a.Cast<Material>();
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
        public static UnityDisplayNode GetSC2Marine(Transform transform,string model){
            var udn=Object.Instantiate(Assets.LoadAsset(model).Cast<GameObject>(),transform).AddComponent<UnityDisplayNode>();
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
                if(reference!=null&&reference.guidRef.Equals("SC2MarineIcon")){
                    var b=Assets.LoadAsset("SC2MarineIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("SC2MarineWarpigIcon")){
                    var b = Assets.LoadAsset("SC2MarineWarpigIcon");
                    var text = b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("SC2MarineU238ShellsIcon")){
                    var b=Assets.LoadAsset("SC2Marineu238ShellsIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("SC2MarineLaserTargetingSystemIcon")){
                    var b=Assets.LoadAsset("SC2MarineLaserTargetingSystemIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("SC2MarineStimpacksIcon")){
                    var b=Assets.LoadAsset("SC2MarineStimpacksIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("SC2MarineSuperStimpacksIcon")){
                    var b = Assets.LoadAsset("SC2MarineSuperStimpacksIcon");
                    var text = b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
        /*[HarmonyPatch(typeof(Weapon),nameof(Weapon.SpawnDart))]
        public static class WI{
            [HarmonyPrefix]
            public static void Prefix(ref Weapon __instance)=>RunAnimations(__instance);
            private static async Task RunAnimations(Weapon __instance){
                if(__instance.weaponModel.name.Contains("SC2MarineBullet")){
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().StopPlayback();
                    __instance.attack.tower.Node.graphic.GetComponent<Animation>().Play();
                    MelonLogger.Msg("hydra fire");
                    __instance.attack.tower.Node.Graphic.gameObject.GetComponent<Animator>().Play("SC2MarineModel//Armature Object|Armature ObjectAttack",-1,0f);
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack",true);
                    var wait=23000f;
                    await Task.Run(()=>{
                        while(wait>0){
                            wait-=TimeManager.timeScaleWithoutNetwork+1;
                            Task.Delay(1);
                        }
                        return;
                    });
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack",false);
                }
            }
        }*/
    }
}