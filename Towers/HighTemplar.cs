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
    public class HighTemplar{
        public static string name="High Templar";
        public static UpgradeModel[]GetUpgrades(){
            return new UpgradeModel[]{
                new("Khaydarin Amulet",250,0,new("HighTemplarKhaydarinAmuletIcon"),0,1,0,"","Khaydarin Amulet"),
                new("Psi Storm",800,0,new("HighTemplarPsiStormIcon"),0,2,0,"","Psi Storm")
            };
        }
        public static(TowerModel,TowerDetailsModel,TowerModel[],UpgradeModel[])GetTower(GameModel gameModel){
            var HighTemplarDetails=gameModel.towerSet[0].Clone().Cast<TowerDetailsModel>();
            HighTemplarDetails.towerId=name;
            HighTemplarDetails.towerIndex=34;
            if(!LocalizationManager.Instance.textTable.ContainsKey("Khaydarin Amulet Description"))LocalizationManager.Instance.textTable.
                    Add("Khaydarin Amulet Description","Decreases cooldown of abilities and increases attack range");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Psi Storm Description"))LocalizationManager.Instance.textTable.
                    Add("Psi Storm Description","Casts a Psionic Storm into the track damaging everything that goes through it");
            /*if(!LocalizationManager.Instance.textTable.ContainsKey("Plasma Surge Description"))LocalizationManager.Instance.textTable.
                    Add("Plasma Surge Description","Increases the radius and damage of Psi Storm");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Archon Description"))LocalizationManager.Instance.textTable.
                    Add("Archon Description","Merges with the nearest High Templar in a Archon, Archon's cannot use abilities but have high damage");
            if(!LocalizationManager.Instance.textTable.ContainsKey("High Archon Description"))LocalizationManager.Instance.textTable.
                    Add("High Archon Description","Lets the Archon use abilities again with buffed damage and range");*/
            return (GetT000(gameModel),HighTemplarDetails,new[]{GetT000(gameModel),GetT100(gameModel),GetT200(gameModel)},GetUpgrades());
        }
        public static TowerModel GetT000(GameModel gameModel){
            var HighTemplar=gameModel.towers[0].Clone().Cast<TowerModel>();
            HighTemplar.name=name;
            HighTemplar.baseId=name;
            HighTemplar.display="HighTemplarModel";
            HighTemplar.portrait=new("HighTemplarIcon");
            HighTemplar.icon=new("HighTemplarIcon");
            HighTemplar.towerSet="Magic";
            HighTemplar.emoteSpriteLarge=new("Protoss");
            HighTemplar.radius=7;
            HighTemplar.cost=500;
            HighTemplar.range=45;
            HighTemplar.towerSize=TowerModel.TowerSize.XL;
            HighTemplar.footprint.ignoresPlacementCheck=true;
            HighTemplar.cachedThrowMarkerHeight=10;
            HighTemplar.areaTypes=new(1);
            HighTemplar.areaTypes[0]=AreaType.land;
            HighTemplar.upgrades=new UpgradePathModel[]{new("Khaydarin Amulet",name+"-100")};
            for(var i=0;i<HighTemplar.behaviors.Count;i++){
                var b=HighTemplar.behaviors[i];
                if(b.GetIl2CppType()==Il2CppType.Of<AttackModel>()){
                    var att=gameModel.towers.First(a=>a.name.Contains("Wizard")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();
                    att.weapons[0].name="HighTemplarPsiBolt";
                    att.weapons[0].rate=0.7f;
                    att.weapons[0].rateFrames=200;
                    att.range=45;
                    for(var j=0;j<att.weapons[0].projectile.behaviors.Length;j++){
                        var pb=att.weapons[0].projectile.behaviors[j];
                        if(pb.GetIl2CppType()==Il2CppType.Of<DamageModel>()){
                            var d=pb.Cast<DamageModel>();
                            d.damage=1;
                            d.maxDamage=2;
                            pb=d;
                        }
                    }
                    HighTemplar.behaviors[i]=att;
                }
                if(b.GetIl2CppType()==Il2CppType.Of<DisplayModel>()){
                    var display=b.Cast<DisplayModel>();
                    display.display="HighTemplarModel";
                    b=display;
                }
            }
            return HighTemplar;
        }
        public static TowerModel GetT100(GameModel gameModel){
            var HighTemplar=gameModel.towers[0].Clone().Cast<TowerModel>();
            HighTemplar.name=name+"-100";
            HighTemplar.baseId=name;
            HighTemplar.tier=100;
            HighTemplar.tiers=new int[]{1,0,0};
            HighTemplar.display="HighTemplarModel";
            HighTemplar.portrait=new("HighTemplarIcon");
            HighTemplar.icon=new("HighTemplarIcon");
            HighTemplar.towerSet="Magicc";
            HighTemplar.emoteSpriteLarge=new("Protoss");
            HighTemplar.radius=15;
            HighTemplar.range=60;
            HighTemplar.towerSize=TowerModel.TowerSize.XL;
            HighTemplar.footprint.ignoresPlacementCheck=true;
            HighTemplar.cachedThrowMarkerHeight=10;
            HighTemplar.areaTypes=new(1);
            HighTemplar.areaTypes[0]=AreaType.land;
            HighTemplar.appliedUpgrades=new(new[]{"Khaydarin Amulet"});
            HighTemplar.upgrades=new[]{new UpgradePathModel("Psi Storm",name+"-200")};
            for(var i=0;i<HighTemplar.behaviors.Count;i++){
                var b=HighTemplar.behaviors[i];
                if(b.GetIl2CppType()==Il2CppType.Of<AttackModel>()){
                    var att=gameModel.towers.First(a=>a.name.Contains("DartMonkey-001")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();
                    att.weapons[0].rate=0.95f*5;
                    att.weapons[0].rateFrames=56*5;
                    att.range=60;
                    att.weapons[0].projectile.display="HighTemplarSpine";
                    for(var j=0;j<att.weapons[0].projectile.behaviors.Length;j++){
                        var pb=att.weapons[0].projectile.behaviors[j];
                        if(pb.GetIl2CppType()==Il2CppType.Of<DamageModel>()){
                            var d=pb.Cast<DamageModel>();
                            d.damage=1;
                            d.maxDamage=2;
                            pb=d;
                        }
                    }
                }
                if(b.GetIl2CppType()==Il2CppType.Of<DisplayModel>()){
                    var display=b.Cast<DisplayModel>();
                    display.display="HighTemplarModel";
                    b=display;
                }
            }
            return HighTemplar;
        }
        public static TowerModel GetT200(GameModel gameModel){
            var HighTemplar=gameModel.towers[0].Clone().Cast<TowerModel>();
            var aa=GetT000(gameModel).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();
            HighTemplar.name=name+"-200";
            HighTemplar.baseId=name;
            HighTemplar.tier=200;
            HighTemplar.tiers=new int[]{2,0,0};
            HighTemplar.display="HighTemplarModel";
            HighTemplar.portrait=new("HighTemplarIcon");
            HighTemplar.icon=new("HighTemplarIcon");
            HighTemplar.towerSet="Magic";
            HighTemplar.emoteSpriteLarge=new("Protoss");
            HighTemplar.radius=15;
            HighTemplar.range=150;
            HighTemplar.towerSize=TowerModel.TowerSize.XL;
            HighTemplar.footprint.ignoresPlacementCheck=true;
            HighTemplar.cachedThrowMarkerHeight=10;
            HighTemplar.areaTypes=new(1);
            HighTemplar.areaTypes[0]=AreaType.land;
            HighTemplar.appliedUpgrades=new(new[]{"Khaydarin Amulet","Psi Storm"});
            HighTemplar.upgrades=new(0);
            for(var i=0;i<HighTemplar.behaviors.Count;i++){
                var b=HighTemplar.behaviors[i];
                if(b.GetIl2CppType()==Il2CppType.Of<AttackModel>()){
                    var att=gameModel.towers.First(a=>a.name.Contains("DartMonkey")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();
                    att.weapons[0].rate=0.95f*3;
                    att.weapons[0].rateFrames=200;
                    att.range=150;
                    att.weapons[0].projectile.display="HighTemplarPsiBolt";
                    for(var j=0;j<att.weapons[0].projectile.behaviors.Length;j++){
                        var pb=att.weapons[0].projectile.behaviors[j];
                        if(pb.GetIl2CppType()==Il2CppType.Of<DamageModel>()){
                            var d=pb.Cast<DamageModel>();
                            d.damage=350;
                            d.maxDamage=500;
                            pb=d;
                        }
                    }
                }
                if(b.GetIl2CppType()==Il2CppType.Of<DisplayModel>()){
                    var display=b.Cast<DisplayModel>();
                    display.display="HighTemplarModel";
                    b=display;
                }
            }
            var ab=gameModel.towers.First(a=>a.name.Equals("BoomerangMonkey-040")).behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<AbilityModel>()).Clone().Cast<AbilityModel>();
            ab.name="HighTemplarPsiStorm";
            ab.displayName="Psi Storm";
            ab.icon=new("HighTemplarPsiStormIcon");
            var aam=ab.behaviors[0].Cast<CreateEffectOnAbilityModel>();
            HighTemplar.behaviors.Add(aam);
            return HighTemplar;
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HighTemplarModel")){
                    var udn=GetHighTemplar(__instance.PrototypeRoot);
                    udn.name="HighTemplar";
                    var a=Assets.LoadAsset("HighTemplarMaterial");
                    udn.genericRenderers[0].material=a.Cast<Material>();
                    udn.RecalculateGenericRenderers();
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(objectId.Equals("HighTemplarPsiBolt")){
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
        public static UnityDisplayNode GetHighTemplar(Transform transform){
            var udn=Object.Instantiate(Assets.LoadAsset("HighTemplarModel").Cast<GameObject>(),transform).AddComponent<UnityDisplayNode>();
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
                if(reference!=null&&reference.guidRef.Equals("HighTemplarIcon")){
                    var b=Assets.LoadAsset("HighTemplarIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HighTemplarPsiStormIcon")){
                    var b=Assets.LoadAsset("HighTemplarPsiStormIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HighTemplarKhaydarinAmuletIcon")){
                    var b=Assets.LoadAsset("HighTemplarKhaydarinAmuletIcon");
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
                if(__instance.weaponModel.name.Contains("HighTemplarPsiBolt")){
                    //__instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().StopPlayback();
                    //__instance.attack.tower.Node.graphic.GetComponent<Animation>().Play();
                    MelonLogger.Msg("templar fire");
                    __instance.attack.tower.Node.Graphic.gameObject.GetComponent<Animator>().Play("HighTemplarModel//Armature Object|Armature ObjectAttack",-1,0f);
                    /*__instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack",true);
                    var wait=23000f;
                    await Task.Run(()=>{
                        while(wait>0){
                            wait-=TimeManager.timeScaleWithoutNetwork+1;
                            Task.Delay(1);
                        }
                        return;
                    });
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack",false);*/
                }
            }
        }
    }
}