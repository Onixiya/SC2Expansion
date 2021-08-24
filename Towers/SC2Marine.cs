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
using SC2Towers.Utils;
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
namespace SC2Towers.Towers{
    public class SC2Marine{
        public static string name="Marine "; //Space in the name here to avoid conflicting with the marine already ingame
        public static UpgradeModel[]GetUpgrades(){
            return new UpgradeModel[]{
                new("U-238 Shells",350,0,new("SC2MarineU238ShellsIcon"),0,1,0,"","U-238 Shells"),
                new("Laser Targeting System",850,0,new("SC2MarineLaserTargetingSystemIcon"),0,2,0,"","Laser Targeting System")
            };
        }
        public static(TowerModel,TowerDetailsModel,TowerModel[],UpgradeModel[])GetTower(GameModel gameModel){
            var SC2MarineDetails=gameModel.towerSet[0].Clone().Cast<TowerDetailsModel>();
            SC2MarineDetails.towerId=name;
            SC2MarineDetails.towerIndex=32;
            if(!LocalizationManager.Instance.textTable.ContainsKey("U-238 Shells Description"))LocalizationManager.Instance.textTable.
                    Add("U-238 Shells Description","Increases damage and attack range");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Laser Targeting System Description"))LocalizationManager.Instance.textTable.
                    Add("Laser Targeting System Description","Increases attack range slightly and provides camo detection");
            /*if(!LocalizationManager.Instance.textTable.ContainsKey("Stimpacks Description"))LocalizationManager.Instance.textTable.
                    Add("Stimpacks Description","Increases attack speed by 50% for 10 seconds");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Warpig Description"))LocalizationManager.Instance.textTable.
                    Add("Warpig Description","Increases damage and attack speed slightly");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Super Stimpack Description"))LocalizationManager.Instance.textTable.
                    Add("Super Stimpack Description","Stimpacks last x5 longer and increases attack speed by 100%");*/
            return (GetT000(gameModel),SC2MarineDetails,new[]{GetT000(gameModel),GetT100(gameModel),GetT200(gameModel)},GetUpgrades());
        }
        public static TowerModel GetT000(GameModel gameModel){
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
                    for(var j=0;j<att.weapons[0].projectile.behaviors.Length;j++){
                        var pb=att.weapons[0].projectile.behaviors[j];
                        if(pb.GetIl2CppType()==Il2CppType.Of<DamageModel>()){
                            var d=pb.Cast<DamageModel>();
                            d.damage=1;
                            d.maxDamage=3;
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
        public static TowerModel GetT100(GameModel gameModel){
            var SC2Marine=gameModel.towers[0].Clone().Cast<TowerModel>();
            SC2Marine.name=name+"-100";
            SC2Marine.baseId=name;
            SC2Marine.tier=100;
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
                    att.weapons[0].rate=0.7f;
                    att.weapons[0].rateFrames=1;
                    att.range=45;
                    att.weapons[0].projectile.display="SC2MarineBullet";
                    for(var j=0;j<att.weapons[0].projectile.behaviors.Length;j++){
                        var pb=att.weapons[0].projectile.behaviors[j];
                        if(pb.GetIl2CppType()==Il2CppType.Of<DamageModel>()){
                            var d=pb.Cast<DamageModel>();
                            d.damage=2;
                            d.maxDamage=5;
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
        public static TowerModel GetT200(GameModel gameModel){
            var SC2Marine=gameModel.towers[0].Clone().Cast<TowerModel>();
            SC2Marine.name=name+"-200";
            SC2Marine.baseId=name;
            SC2Marine.tier=200;
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
            SC2Marine.upgrades=new(0);
            var att = gameModel.towers.First(a => a.name.Contains("DartMonkey")).behaviors.First(a => a.GetIl2CppType()==Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();
            for(var i=0;i<SC2Marine.behaviors.Count;i++) {
                var b=SC2Marine.behaviors[i];
                att.weapons[0].rate=0.7f;
                att.weapons[0].rateFrames=1;
                att.range=50;
                att.weapons[0].projectile.display="SC2MarineBullet";
                for(var j=0;j<att.weapons[0].projectile.behaviors.Length;j++) {
                    var pb=att.weapons[0].projectile.behaviors[j];
                    if(pb.GetIl2CppType()==Il2CppType.Of<DamageModel>()) {
                        var d=pb.Cast<DamageModel>();
                        d.damage=2;
                        d.maxDamage=5;
                        pb=d;
                    }
                }
            }
            SC2Marine.behaviors=SC2Marine.behaviors.Add(att);
            SC2Marine.behaviors=SC2Marine.behaviors.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true));
            return SC2Marine;
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("SC2MarineModel")){
                    var udn=GetSC2Marine(__instance.PrototypeRoot);
                    udn.name="SC2Marine";
                    var a=Assets.LoadAsset("SC2MarineMaterial");
                    udn.genericRenderers[0].material=a.Cast<Material>();
                    udn.RecalculateGenericRenderers();
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(objectId.Equals("SC2MarineBullet")){
                    UnityDisplayNode udn=null;
                    __instance.FindAndSetupPrototypeAsync("bdbeaa256e6c63b45829535831843376",
                        new Action<UnityDisplayNode>(oudn=>{
                            var nudn=Object.Instantiate(oudn,__instance.PrototypeRoot);
                            nudn.name=objectId+"(Clone)";
                            nudn.isSprite=true;
                            nudn.RecalculateGenericRenderers();
                            for(var i=0;i<nudn.genericRenderers.Length;i++){
                                if(nudn.genericRenderers[i].GetIl2CppType()==Il2CppType.Of<SpriteRenderer>()){
                                    var smr=nudn.genericRenderers[i].Cast<SpriteRenderer>();
                                    var text=Assets.LoadAsset("Bone").Cast<Texture2D>();
                                    smr.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new(0.5f,0.5f),5.4f);
                                    nudn.genericRenderers[i]=smr;
                                }
                            }
                            udn=nudn;
                            onComplete.Invoke(udn);
                        }));
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
        public static UnityDisplayNode GetSC2Marine(Transform transform){
            var udn=Object.Instantiate(Assets.LoadAsset("SC2MarineModel").Cast<GameObject>(),transform).AddComponent<UnityDisplayNode>();
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