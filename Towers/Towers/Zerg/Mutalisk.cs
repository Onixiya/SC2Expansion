namespace SC2Expansion.Towers{
    public class Mutalisk:ModTower<ZergSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.mutalisk);
        public override string DisplayName=>"Mutalisk";
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>525;
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
            Mutalisk.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId="MutaliskBirth";
            Mutalisk.GetBehavior<CreateSoundOnTowerPlaceModel>().sound2=Mutalisk.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1;
            SetUpgradeSounds(Mutalisk,"MutaliskUpgrade");
        }
        public class ViciousGlaive:ModUpgrade<Mutalisk>{
            public override string DisplayName=>"Vicious Glaive";
            public override string Description=>"Increasing the bone density of glaive wurms before they are fired allows them to survive for longer and hit more targets";
            public override int Cost=>550;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskViciousGlaiveIcon");
                var Glaive=Mutalisk.GetAttackModel();
                Glaive.weapons[0].projectile.GetBehavior<RetargetOnContactModel>().maxBounces+=2;
                Glaive.weapons[0].projectile.GetBehavior<RetargetOnContactModel>().distance+=10;
                SetUpgradeSounds(Mutalisk,"MutaliskUpgrade1");
            }
        }
        public class RapidRegeneration:ModUpgrade<Mutalisk>{
            public override string DisplayName=>"Rapid Regeneration";
            public override string Description=>"Decreases the time required to fully grow a glaive before firing it effectively increasing fire rate";
            public override int Cost=>475;
            public override int Path=>MIDDLE;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskRapidRegenerationIcon");
                Mutalisk.GetAttackModel().weapons[0].rate-=0.3f;
                SetUpgradeSounds(Mutalisk,"MutaliskUpgrade2");
            }
        }
        public class SlicingGlaive:ModUpgrade<Mutalisk>{
            public override string DisplayName=>"Slicing Glaive";
            public override string Description=>"Evolving the glaive to pierce through armour allows popping leads and increases damage to Moabs and Ceremics";
            public override int Cost=>865;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskSlicingGlaiveIcon");
                var Glaive=Mutalisk.GetAttackModel();
                Glaive.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModelMoab","Moabs",2,0,false,false));
                Glaive.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModelCeremic","Ceremic",2,0,false,false));
                Glaive.weapons[0].projectile.GetDamageModel().immuneBloonProperties=0;
                SetUpgradeSounds(Mutalisk,"MutaliskUpgrade3");
            }
        }
        public class AeroGlaive:ModUpgrade<Mutalisk>{
            public override string DisplayName=>"Aerodynamic Glaive";
            public override string Description=>"Growing more aerodynamic glaives allows them to be fired further";
            public override int Cost=>790;
            public override int Path=>MIDDLE;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("HydraliskFrenzyIcon");
                Mutalisk.range+=10;
                Mutalisk.GetAttackModel().range=Mutalisk.range;
                SetUpgradeSounds(Mutalisk,"MutaliskUpgrade4");
            }
        }
        public class SunderingGlaive:ModUpgrade<Mutalisk>{
            public override string DisplayName=>"Sundering Glaive";
            public override string Description=>"Evolving the glaive to explode when hitting a target increases damage by a lot, can no longer bounce";
            public override int Cost=>1650;
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
                SetUpgradeSounds(Mutalisk,"MutaliskUpgrade5");
            }
        }
        public class MutaliskPrimal:ModUpgrade<Mutalisk>{
            public override string DisplayName=>"Primal Evolution";
            public override string Description=>"Gains a random bonus to attack speed, range or damage";
            public override int Cost=>1740;
            public override int Path=>MIDDLE;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskPrimalIcon");
                Mutalisk.portrait=new("MutaliskPrimalPortrait");
                Mutalisk.display="MutaliskPrimalPrefab";
                SetUpgradeSounds(Mutalisk,"MutaliskUpgrade6");
            }
        }
        public class Devourer:ModUpgrade<Mutalisk>{
            public override string DisplayName=>"Morph into Devourer";
            public override string Description=>"Devourers are heavy anti air flyers, dealing great damage against MOAB class bloons";
            public override int Cost=>7670;
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
                SetUpgradeSounds(Mutalisk,"MutaliskUpgrade5");
            }
        }
        public class BroodLord:ModUpgrade<Mutalisk>{
            public override string DisplayName=>"Morph into Brood Lord";
            public override string Description=>"Heavy Zerg seige flyer based off from Guardians, attacks by shooting Broodlings at its target";
            public override int Cost=>6220;
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
                SetUpgradeSounds(Mutalisk,"MutaliskUpgrade6");
            }
        }
        /*public class Leviathan:ModUpgrade<Mutalisk>{
            public override string DisplayName=>"Morph into Leviathan";
            public override string Description=>"\"Leviathan. Largest of Zerg. Sky will belong to Swarm\"";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                if(UnfinishedWork==true){
                    GetUpgradeModel().icon=new("MutaliskLeviathanIcon");
                    Mutalisk.portrait=new("MutaliskLeviathanPortrait");
                    Mutalisk.display="MutaliskLeviathanPrefab";
                    Mutalisk.RemoveBehavior(Mutalisk.GetBehaviors<AttackModel>().First(a=>a.name.Contains("Glaive")));
                    var Tentacle=Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).GetAttackModel();
                    var test=Game.instance.model.towers.First(a=>a.name.Contains("MonkeyBuccaneer-040")).behaviors.First(a=>a.name.Contains("Take")).Cast<AbilityModel>().
                        behaviors.First(a=>a.name.Contains("Activate")).Clone().Cast<ActivateAttackModel>().attacks[0];
                    var Bile=Game.instance.model.towers.First(a=>a.name.Contains("SuperMonkey-100")).behaviors.First(a=>a.name.Contains("Attack")).Clone().Cast<AttackModel>();
                    Tentacle=test;
                    Tentacle.weapons[0].rate=0.01f;
                    //Tentacle.weapons[0].projectile.behaviors.Remove(a=>a.name.Contains("Take"));
                    Mutalisk.AddBehavior(Tentacle);
                    Mutalisk.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true));
                }
            }
        }*/
        [HarmonyPatch(typeof(AudioFactory),"Start")]
        public class AudioFactoryStart_Patch{
            [HarmonyPostfix]
            public static void Prefix(ref AudioFactory __instance){
                if(ZergEnabled){
                    AudioFactoryInstance=__instance;
                    __instance.RegisterAudioClip("MutaliskBirth",TowerAssets.LoadAsset("MutaliskBirth").Cast<AudioClip>());
                    __instance.RegisterAudioClip("MutaliskUpgrade",TowerAssets.LoadAsset("MutaliskUpgrade").Cast<AudioClip>());
                    __instance.RegisterAudioClip("MutaliskUpgrade1",TowerAssets.LoadAsset("MutaliskUpgrade1").Cast<AudioClip>());
                    __instance.RegisterAudioClip("MutaliskUpgrade2",TowerAssets.LoadAsset("MutaliskUpgrade2").Cast<AudioClip>());
                    __instance.RegisterAudioClip("MutaliskUpgrade3",TowerAssets.LoadAsset("MutaliskUpgrade3").Cast<AudioClip>());
                    __instance.RegisterAudioClip("MutaliskUpgrade4",TowerAssets.LoadAsset("MutaliskUpgrade4").Cast<AudioClip>());
                    __instance.RegisterAudioClip("MutaliskUpgrade5",TowerAssets.LoadAsset("MutaliskUpgrade5").Cast<AudioClip>());
                    __instance.RegisterAudioClip("MutaliskUpgrade6",TowerAssets.LoadAsset("MutaliskUpgrade6").Cast<AudioClip>());
                }
            }
        }
        [HarmonyPatch(typeof(TowerManager),"UpgradeTower")]
        public static class TowerManagerUpgradeTower_Patch{
            [HarmonyPostfix]
            public static void Postfix(Tower tower,TowerModel def,string __state){
                if(__state!=null&&__state.Contains("Primal")&&tower.namedMonkeyKey.Contains("Mutalisk")){
                    int RandNum=new System.Random().Next(1,4);
                    switch(RandNum){
                        case 1:
                            def.GetAttackModel().range+=8;
                            def.range=def.GetAttackModel().range;
                            break;
                        case 2:
                            def.GetAttackModel().weapons[0].rate-=0.25f;
                            break;
                        case 3:
                            def.GetAttackModel().weapons[0].projectile.GetDamageModel().damage+=3;
                            break;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                return LoadModel(TowerAssets,objectId,"Mutalisk",__instance,onComplete);
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public class ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.StartsWith("Mutalisk")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public class WeaponSpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.towerModel.name.StartsWith("SC2Expansion-Mutalisk")){
                    __instance.attack.tower.Node.graphic.GetComponent<Animator>().Play("MutaliskAttack");
                }
            }
        }
    }
}