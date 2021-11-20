﻿namespace SC2Expansion.Towers{
    public class Mutalisk:ModTower<ZergSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.mutalisk);
        public override string DisplayName=>"Mutalisk";
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>4;
        public override int MiddlePathUpgrades=>4;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ZergEnabled;
        public override string Description=>"Primary Zerg flyer, able to bounce its shot to hit multiple targets";
        public override void ModifyBaseTowerModel(TowerModel Mutalisk){
            Mutalisk.display="MutaliskPrefab";
            Mutalisk.portrait=new("MutaliskPortrait");
            Mutalisk.icon=new("MutaliskIcon");
            Mutalisk.emoteSpriteLarge=new("Zerg");
            Mutalisk.radius=5;
            Mutalisk.cost=700;
            Mutalisk.range=40;
            Mutalisk.areaTypes=new(4);
            Mutalisk.areaTypes[0]=AreaType.land;
            Mutalisk.areaTypes[1]=AreaType.track;
            Mutalisk.areaTypes[2]=AreaType.ice;
            Mutalisk.areaTypes[3]=AreaType.water;
            var Glaive=Mutalisk.GetAttackModel();
            Glaive.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("SniperMonkey-030").GetAttackModel().weapons[0].projectile.GetBehavior<RetargetOnContactModel>().Duplicate());
            Glaive.weapons[0].projectile.pierce=3;
            Glaive.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan=0.75f;
            Glaive.weapons[0].projectile.display="MutaliskGlaivePrefab";
            Glaive.weapons[0].projectile.GetBehavior<RetargetOnContactModel>().maxBounces=2;
            Glaive.weapons[0].projectile.GetBehavior<RetargetOnContactModel>().distance=30;
            Glaive.range=Mutalisk.range;
            Glaive.weapons[0].rate=1.65f;
            Mutalisk.GetBehavior<DisplayModel>().display=Mutalisk.display;
        }
        public class ViciousGlaive:ModUpgrade<Mutalisk>{
            public override string Name=>"ViciousGlaive";
            public override string DisplayName=>"Vicious Glaive";
            public override string Description=>"Increasing the bone density of glaive wurms before they are fired allows them to survive for longer and hit more targets";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskViciousGlaiveIcon");
                var Glaive=Mutalisk.GetAttackModel();
                Glaive.weapons[0].projectile.GetBehavior<RetargetOnContactModel>().maxBounces+=2;
                Glaive.weapons[0].projectile.GetBehavior<RetargetOnContactModel>().distance+=10;
            }
        }
        public class RapidRegeneration:ModUpgrade<Mutalisk>{
            public override string Name=>"RapidRegeneration";
            public override string DisplayName=>"Rapid Regeneration";
            public override string Description=>"Decreases the time required to fully grow a glaive before firing it effectively increasing fire rate";
            public override int Cost=>750;
            public override int Path=>MIDDLE;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskRapidRegenerationIcon");
                Mutalisk.GetAttackModel().weapons[0].rate-=0.3f;
            }
        }
        public class SlicingGlaive:ModUpgrade<Mutalisk>{
            public override string Name=>"SlicingGlaive";
            public override string DisplayName=>"Slicing Glaive";
            public override string Description=>"Evolving the glaive to pierce through armour allows popping leads and increases damage to Moabs and Ceremics";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskSlicingGlaiveIcon");
                var Glaive=Mutalisk.GetAttackModel();
                Glaive.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModelMoab","Moabs",2,0,false,false));
                Glaive.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModelCeremic","Ceremic",2,0,false,false));
                Glaive.weapons[0].projectile.GetDamageModel().immuneBloonProperties=0;
            }
        }
        public class AeroGlaive:ModUpgrade<Mutalisk>{
            public override string Name=>"AeroGlaive";
            public override string DisplayName=>"Aerodynamic Glaive";
            public override string Description=>"Growing more aerodynamic glaives allows them to be fired further";
            public override int Cost=>750;
            public override int Path=>MIDDLE;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("HydraliskFrenzyIcon");
                Mutalisk.range+=10;
                Mutalisk.GetAttackModel().range=Mutalisk.range;
            }
        }
        public class SunderingGlaive:ModUpgrade<Mutalisk>{
            public override string Name=>"SunderingGlaive";
            public override string DisplayName=>"Sundering Glaive";
            public override string Description=>"Evolving the glaive to explode when hitting a target increases damage by a lot, can no longer bounce";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskSunderingGlaiveIcon");
                Mutalisk.display="MutaliskWebbyPrefab";
                var Glaive=Mutalisk.GetAttackModel();
                Glaive.weapons[0].projectile.RemoveBehavior<RetargetOnContactModel>();
                Glaive.weapons[0].projectile.pierce=1;
                Glaive.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-030").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>());
                Glaive.weapons[0].projectile.GetDamageModel().damage+=5;
            }
        }
        public class Primal:ModUpgrade<Mutalisk>{
            public override string Name=>"MutaliskPrimal";
            public override string DisplayName=>"Primal Evolution";
            public override string Description=>"Gains a random bonus to attack speed, range or damage";
            public override int Cost=>750;
            public override int Path=>MIDDLE;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskPrimalIcon");
                Mutalisk.portrait=new("MutaliskPrimalPortrait");
                Mutalisk.display="MutaliskPrimalPrefab";
            }
        }
        public class Devourer:ModUpgrade<Mutalisk>{
            public override string Name=>"Devourer";
            public override string DisplayName=>"Morph into Devourer";
            public override string Description=>"Devourers are heavy anti air flyers, dealing great damage against MOAB class bloons";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskDevourerIcon");
                Mutalisk.portrait=new("MutaliskDevourerPortrait");
                Mutalisk.display="MutaliskDevourerPrefab";
                var Glaive=Mutalisk.GetAttackModel();
                Glaive.weapons[0].projectile.GetDamageModel().damage+=5;
                try{Glaive.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Moabs")).Cast<DamageModifierForTagModel>().damageAddative=10;}
                    catch{Glaive.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel","Moabs",2,0,false,false));}
                Mutalisk.range+=10;
                Glaive.range=Mutalisk.range;
                Glaive.weapons[0].rate=1.4f;
            }
        }
        public class BroodLord:ModUpgrade<Mutalisk>{
            public override string Name=>"BroodLordPrimal";
            public override string DisplayName=>"Morph into Brood Lord";
            public override string Description=>"Heavy Zerg seige flyer based off from Guardians, attacks by shooting Broodlings at its target";
            public override int Cost=>750;
            public override int Path=>MIDDLE;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskBroodLordIcon");
                Mutalisk.portrait=new("MutaliskBroodLordPortrait");
                Mutalisk.display="MutaliskBroodLordPrefab";
                Mutalisk.range+=25;
                Mutalisk.radius=20;
                var Broodling=Mutalisk.GetAttackModel();
                Broodling.range=Mutalisk.range;
                Broodling.weapons[0].projectile=Game.instance.model.GetTowerFromId("WizardMonkey-004").behaviors.First(a=>a.name.Contains("AttackModel_Attack Necromancer_")).
                    Cast<AttackModel>().weapons[0].projectile.Duplicate();
                Broodling.weapons[0].projectile.pierce=5;
                Broodling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection=false;
                Broodling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=0.6f;
                Broodling.weapons[0].projectile.display="MutaliskBroodlingPrefab";
                Broodling.weapons[0].projectile.GetDamageModel().damage+=10;
            }
        }
        /*public class Leviathan:ModUpgrade<Mutalisk>{
            public override string Name=>"Leviathan";
            public override string DisplayName=>"Morph into Leviathan";
            public override string Description=>"\"Leviathan. Largest of Zerg. Sky will belong to Swarm\"";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskLeviathanIcon");
                Mutalisk.portrait=new("MutaliskLeviathanPortrait");
                Mutalisk.display="MutaliskLeviathanPrefab";
                Mutalisk.behaviors=Mutalisk.behaviors.Remove(a=>a.name.Contains("Glaive"));
                var Tentacle=Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).GetAttackModel();
                var test=Game.instance.model.towers.First(a=>a.name.Contains("MonkeyBuccaneer-040")).behaviors.First(a=>a.name.Contains("Take")).Cast<AbilityModel>().
                    behaviors.First(a=>a.name.Contains("Activate")).Clone().Cast<ActivateAttackModel>().attacks[0];
                var Bile=Game.instance.model.towers.First(a=>a.name.Contains("SuperMonkey-100")).behaviors.First(a=>a.name.Contains("Attack")).Clone().Cast<AttackModel>();
                Tentacle=test;
                Tentacle.weapons[0].rate=0.01f;
                
                //Tentacle.weapons[0].projectile.behaviors.Remove(a=>a.name.Contains("Take"));
                Mutalisk.behaviors=Mutalisk.behaviors.Add(Tentacle,new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true));
            }
        }*/
        [HarmonyPatch(typeof(TowerManager),"UpgradeTower")]
        public static class TowerManagerUpgradeTower_Patch{
            [HarmonyPostfix]
            public static void Postfix(Tower tower,TowerModel def,string __state){
                if(__state!=null&&__state.Contains("Primal")&&tower.namedMonkeyKey.Contains("Mutalisk")){
                    int RandNum=new System.Random().Next(1,3);
                    if(RandNum==1){
                        def.GetAttackModel().range+=8;
                        def.range=def.GetAttackModel().range;
                    }
                    if(RandNum==2)def.GetAttackModel().weapons[0].rate-=0.25f;
                    if(RandNum==3)def.GetAttackModel().weapons[0].projectile.GetDamageModel().damage+=3;
                }
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            public static Dictionary<string,UnityDisplayNode>DisplayDict=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!DisplayDict.ContainsKey(objectId)&&objectId.Contains("Mutalisk")){
                    var udn=uObject.Instantiate(TowerAssets.LoadAsset(objectId).Cast<GameObject>(),__instance.PrototypeRoot).AddComponent<UnityDisplayNode>();
                    udn.name="SC2Expansion-Mutalisk";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    DisplayDict.Add(objectId,udn);
                    return false;
                }
                if(DisplayDict.ContainsKey(objectId)){
                    onComplete.Invoke(DisplayDict[objectId]);
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public record ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref uImage image){
                if(reference!=null&&reference.guidRef.Contains("Mutalisk")){
                    var text=TowerAssets.LoadAsset(reference.guidRef).Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public static class WeaponSpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.towerModel.name.Contains("Mutalisk")){
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("MutaliskAttack");
                }
            }
        }
    }
}