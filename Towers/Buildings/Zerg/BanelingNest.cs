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
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Utils;
using SC2Expansion.Utils;
using HarmonyLib;
using NinjaKiwi.Common;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object=UnityEngine.Object;
using MelonLoader;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Simulation.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;

namespace SC2Expansion.Towers{
    public class BanelingNest{
        public static string name="Baneling Nest"; //Space in the name to avoid messing with the existing marine, closest we get to any conflict
        public static UpgradeModel[]GetUpgrades(){
            return new UpgradeModel[]{
                new("Centrifugal Hooks",350,0,new("BanelingNestCentrifugalHooksIcon"),0,1,0,"","Centrifugal Hooks"),
                new("Rupture",500,0,new("BanelingNestRuptureIcon"),0,2,0,"","Rupture"),
                new("Corrosive Acid",875,0,new("BanelingNestCorrosiveAcidIcon"),0,3,0,"","Corrosive Acid"),
                new("Splitter",1400,0,new("BanelingNestSplitterIcon"),0,4,0,"","Splitter")
                //new("Tychus Findlay",2500,0,new("BanelingNestTychusIcon"),0,5,0,"","Tychus Findlay")
            };
        }
        public static(TowerModel,TowerDetailsModel,TowerModel[],UpgradeModel[])GetTower(GameModel gameModel){
            var BanelingNestDetails=gameModel.towerSet.First(a=>a.name.Contains("DartMonkey")).Clone().Cast<TowerDetailsModel>();
            BanelingNestDetails.towerId=name;
            BanelingNestDetails.towerIndex=0;
            if(!LocalizationManager.Instance.textTable.ContainsKey("Centrifugal Hooks Description"))LocalizationManager.Instance.textTable.
                    Add("Centrifugal Hooks Description","Faster Banelings");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Rupture Description"))LocalizationManager.Instance.textTable.
                    Add("Rupture Description","Increases attack area");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Corrosive Acid Description"))LocalizationManager.Instance.textTable.
                    Add("Corrosive Acid Description","Increases damage to primary target, strips camo and fortification on all non MOAB's affected");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Splitter Strain Description"))LocalizationManager.Instance.textTable.
                    Add("Spliiter Strain Description","Splits into 2 smaller banelings upon death, deals 50% damage of parent");
            return (GetT0(gameModel),BanelingNestDetails,new[]{GetT0(gameModel)/*,GetT1(gameModel),GetT2(gameModel),GetT3(gameModel),GetT4(gameModel)/*,GetT5(gameModel)*/},GetUpgrades());
        }
        public static TowerModel GetT0(GameModel gameModel){
            var BanelingNest=gameModel.towers.First(a=>a.name.Equals("WizardMonkey-004")).Clone().Cast<TowerModel>();
            BanelingNest.name=name;
            BanelingNest.baseId=name;
            BanelingNest.display="BanelingNestPrefab";
            BanelingNest.portrait=new("BanelingNestIcon");
            BanelingNest.icon=new("BanelingNestIcon");
            BanelingNest.emoteSpriteLarge=new("Zerg");
            BanelingNest.radius=5;
            BanelingNest.cost=400;
            BanelingNest.range=35;
            BanelingNest.footprint.ignoresPlacementCheck=true;
            BanelingNest.cachedThrowMarkerHeight=10;
            BanelingNest.areaTypes=new(1);
            BanelingNest.areaTypes[0]=AreaType.land;
            //BanelingNest.upgrades=new UpgradePathModel[]{new("U-238 Shells",name+"-100")};
            BanelingNest.upgrades=new(0);
            /*BanelingNest.behaviors=BanelingNest.behaviors.Remove(a=>a.name.Equals("AttackModel_Attack_"));
            BanelingNest.behaviors=BanelingNest.behaviors.Remove(a=>a.name.Contains("Shimmer"));
            var temp=BanelingNest.behaviors.First(a=>a.name.Contains("Zone")).Cast<NecromancerZoneModel>();
            var temp1=BanelingNest.behaviors.First(a=>a.name.Contains("Necromancer_")).Cast<AttackModel>().behaviors.First(a=>a.name.Contains("Necro")).Cast<NecromancerTargetTrackWithinRangeModel>();
            //temp.attackUsedForRangeModel.range=999;
            /*while(temp2.MoveNext()){
                MelonLogger.Msg(temp2.Current.name);
            }*/
            //var SpawnBanelings=BanelingNest.behaviors.First(a=>a.name.Contains("AttackModel")).Cast<AttackModel>();
            //MelonLogger.Msg("test: "+temp.attackUsedForRangeModel.behaviors.GetEnumerator());
            BanelingNest.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="BanelingNestPrefab";
            return BanelingNest;
        }
        /*public static TowerModel GetT1(GameModel gameModel){
            var BanelingNest=gameModel.towers[0].Clone().Cast<TowerModel>();
            BanelingNest.name=name+"-100";
            BanelingNest.baseId=name;
            BanelingNest.tier=1;
            BanelingNest.tiers=new int[]{1,0,0};
            BanelingNest.display="BanelingNestPrefab";
            BanelingNest.portrait=new("BanelingNestIcon");
            BanelingNest.icon=new("BanelingNestIcon");
            BanelingNest.towerSet="Primary";
            BanelingNest.emoteSpriteLarge=new("Terran");
            BanelingNest.radius=5;
            BanelingNest.range=45;
            BanelingNest.footprint.ignoresPlacementCheck=true;
            BanelingNest.cachedThrowMarkerHeight=10;
            BanelingNest.areaTypes=new(1);
            BanelingNest.areaTypes[0]=AreaType.land;
            BanelingNest.appliedUpgrades=new(new[]{"U-238 Shells"});
            BanelingNest.upgrades=new[]{new UpgradePathModel("Laser Targeting System",name+"-200")};
            var Bullet=BanelingNest.behaviors.First(a=>a.name.Contains("AttackModel")).Cast<AttackModel>();
            Bullet.weapons[0].name="BanelingNestBullet";
            Bullet.weapons[0].rate=0.7f;
            Bullet.weapons[0].rateFrames=1;
            Bullet.range=45;
            Bullet.weapons[0].projectile.display=null;
            Bullet.weapons[0].projectile.behaviors[0].Cast<DamageModel>().damage=2;
            return BanelingNest;
        }
        public static TowerModel GetT2(GameModel gameModel){
            var BanelingNest=gameModel.towers[0].Clone().Cast<TowerModel>();
            BanelingNest.name=name+"-200";
            BanelingNest.baseId=name;
            BanelingNest.tier=2;
            BanelingNest.tiers=new int[]{2,0,0};
            BanelingNest.display="BanelingNestPrefab";
            BanelingNest.portrait=new("BanelingNestIcon");
            BanelingNest.icon=new("BanelingNestIcon");
            BanelingNest.towerSet="Primary";
            BanelingNest.emoteSpriteLarge=new("Terran");
            BanelingNest.radius=5;
            BanelingNest.range=50;
            BanelingNest.footprint.ignoresPlacementCheck=true;
            BanelingNest.cachedThrowMarkerHeight=10;
            BanelingNest.areaTypes=new(1);
            BanelingNest.areaTypes[0]=AreaType.land;
            BanelingNest.appliedUpgrades=new(new[]{"U-238 Shells","Laser Targeting System"});
            BanelingNest.upgrades=new[]{new UpgradePathModel("Stimpacks",name+"-300")};
            var Bullet=BanelingNest.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            Bullet.range=50;
            Bullet.name="BanelingNestBullet";
            Bullet.weapons[0].rate=0.7f;
            Bullet.weapons[0].projectile.behaviors.First().Cast<DamageModel>().damage=2;
            BanelingNest.behaviors=BanelingNest.behaviors.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true));
            return BanelingNest;
        }
        public static TowerModel GetT3(GameModel gameModel){
            var BanelingNest=gameModel.towers[0].Clone().Cast<TowerModel>();
            BanelingNest.name=name+"-300";
            BanelingNest.baseId=name;
            BanelingNest.tier=3;
            BanelingNest.tiers=new int[]{3,0,0};
            BanelingNest.display="BanelingNestPrefab";
            BanelingNest.portrait=new("BanelingNestIcon");
            BanelingNest.icon=new("BanelingNestIcon");
            BanelingNest.towerSet="Primary";
            BanelingNest.emoteSpriteLarge=new("Terran");
            BanelingNest.radius=5;
            BanelingNest.range=50;
            BanelingNest.footprint.ignoresPlacementCheck=true;
            BanelingNest.cachedThrowMarkerHeight=10;
            BanelingNest.areaTypes=new(1);
            BanelingNest.areaTypes[0]=AreaType.land;
            BanelingNest.appliedUpgrades=new(new[]{"U-238 Shells","Laser Targeting System","Stimpacks"});
            BanelingNest.upgrades=new[]{new UpgradePathModel("Warpig",name+"-400")};
            var Bullet=BanelingNest.behaviors.First(a=>a.name.Contains("Attack")).Clone().Cast<AttackModel>();
            Bullet.name="BanelingNestBullet";
            Bullet.range=50;
            Bullet.weapons[0].rate=0.7f;
            Bullet.range=50;
            Bullet.weapons[0].projectile.display=null;
            Bullet.weapons[0].projectile.behaviors.First().Cast<DamageModel>().damage=2;
            var Stimpacks=gameModel.towers.First(a=>a.name.Equals("BoomerangMonkey-040")).behaviors.First(a=>a.name.Contains("Ability")).Clone().Cast<AbilityModel>();
            Stimpacks.name="Stimpacks";
            Stimpacks.displayName="Stimpacks";
            Stimpacks.icon=new("BanelingNestStimpacksIcon");
            Stimpacks.cooldown=40;
            Stimpacks.maxActivationsPerRound=1;
            Stimpacks.behaviors.First(a=>a.name.Contains("Turbo")).Cast<TurboModel>().projectileDisplay=null;
            BanelingNest.behaviors=BanelingNest.behaviors.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true),Stimpacks);
            return BanelingNest;
        }
        public static TowerModel GetT4(GameModel gameModel){
            var BanelingNest=gameModel.towers[0].Clone().Cast<TowerModel>();
            BanelingNest.name=name+"-400";
            BanelingNest.baseId=name;
            BanelingNest.tier=4;
            BanelingNest.tiers=new int[]{4,0,0};
            BanelingNest.display="BanelingNestWarpigPrefab";
            BanelingNest.portrait=new("BanelingNestWarpigPortrait");
            BanelingNest.icon=new("BanelingNestWarpigIcon");
            BanelingNest.towerSet="Primary";
            BanelingNest.emoteSpriteLarge=new("Terran");
            BanelingNest.radius=5;
            BanelingNest.range=50;
            BanelingNest.footprint.ignoresPlacementCheck=true;
            BanelingNest.cachedThrowMarkerHeight=10;
            BanelingNest.areaTypes=new(1);
            BanelingNest.areaTypes[0]=AreaType.land;
            BanelingNest.appliedUpgrades=new(new[]{"U-238 Shells","Laser Targeting System","Stimpacks","Warpig"});
            BanelingNest.upgrades=new(0);
            var Bullet=BanelingNest.behaviors.First(a=>a.name.Contains("Attack")).Clone().Cast<AttackModel>();
            Bullet.name="BanelingNestBullet";
            Bullet.range=50;
            Bullet.weapons[0].rate=0.55f;
            Bullet.weapons[0].projectile.display=null;
            Bullet.weapons[0].projectile.behaviors.First().Cast<DamageModel>().damage=4;
            var Stimpacks=gameModel.towers.First(a=>a.name.Equals("BoomerangMonkey-040")).behaviors.First(a=>a.name.Contains("Ability")).Clone().Cast<AbilityModel>();
            Stimpacks.name="Stimpacks";
            Stimpacks.displayName="Stimpacks";
            Stimpacks.icon=new("BanelingNestStimpacksIcon");
            Stimpacks.cooldown=40;
            Stimpacks.maxActivationsPerRound=1;
            Stimpacks.behaviors.First(a=>a.name.Contains("Turbo")).Cast<TurboModel>().projectileDisplay=null;
            BanelingNest.behaviors=BanelingNest.behaviors.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true),Stimpacks);
            return BanelingNest;
        }*/
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("BanelingNestPrefab")){
                    var udn=GetBanelingNest(__instance.PrototypeRoot,"BanelingNestPrefab");
                    udn.name="BanelingNest";
                    var a=Assets.LoadAsset("BanelingNestMaterial");
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
        public static UnityDisplayNode GetBanelingNest(Transform transform,string model){
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
                if(reference!=null&&reference.guidRef.Equals("BanelingNestIcon")){
                    var text=Assets.LoadAsset("BanelingNestIcon").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                /*if(reference!=null&&reference.guidRef.Equals("BanelingNestWarpigPortrait")){
                    var b=Assets.LoadAsset("BanelingNestWarpigPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BanelingNestWarpigIcon")){
                    var b=Assets.LoadAsset("BanelingNestWarpigIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BanelingNestU238ShellsIcon")){
                    var b=Assets.LoadAsset("BanelingNestu238ShellsIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BanelingNestLaserTargetingSystemIcon")){
                    var b=Assets.LoadAsset("BanelingNestLaserTargetingSystemIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BanelingNestStimpacksIcon")){
                    var b=Assets.LoadAsset("BanelingNestStimpacksIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BanelingNestSuperStimpacksIcon")){
                    var b=Assets.LoadAsset("BanelingNestSuperStimpacksIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }*/
            }
        }
        /*[HarmonyPatch(typeof(Weapon),nameof(Weapon.SpawnDart))]
        public static class WI{
            [HarmonyPrefix]
            public static void Prefix(ref Weapon __instance)=>RunAnimations(__instance);
            private static async Task RunAnimations(Weapon __instance){
                if(__instance.weaponModel.name.Contains("BanelingNestBullet")){
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().StopPlayback();
                    __instance.attack.tower.Node.graphic.GetComponent<Animation>().Play();
                    MelonLogger.Msg("hydra fire");
                    __instance.attack.tower.Node.Graphic.gameObject.GetComponent<Animator>().Play("BanelingNestPrefab//Armature Object|Armature ObjectAttack",-1,0f);
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