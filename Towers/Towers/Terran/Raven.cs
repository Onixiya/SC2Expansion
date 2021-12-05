namespace SC2Expansion.Towers{
    public class Raven:ModTower<TerranSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.raven);
        public override string DisplayName=>"Raven";
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>740;
        public override int TopPathUpgrades=>4;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!TerranEnabled;
        public override string Description=>"Automated Terran flying support, gives camo detection to all towers within range";
        public override void ModifyBaseTowerModel(TowerModel Raven){
            Raven.display="RavenPrefab";
            Raven.portrait=new("RavenPortrait");
            Raven.icon=new("RavenIcon");
            Raven.radius=5;
            Raven.range=40;
            Raven.areaTypes=new(4);
            Raven.areaTypes[0]=AreaType.land;
            Raven.areaTypes[1]=AreaType.track;
            Raven.areaTypes[2]=AreaType.ice;
            Raven.areaTypes[3]=AreaType.water;
            Raven.RemoveBehavior<AttackModel>();
            Raven.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyVillage-020").GetBehavior<VisibilitySupportModel>().Duplicate());
            Raven.GetBehavior<DisplayModel>().display=Raven.display;
            Raven.GetBehavior<DisplayModel>().positionOffset=new(0,0,50);
            Raven.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId="RavenBirth";
            Raven.GetBehavior<CreateSoundOnTowerPlaceModel>().sound2=Raven.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1;
            SetUpgradeSounds(Raven,"RavenUpgrade");
        }
        public class Seeker:ModUpgrade<Raven>{
            public override string DisplayName=>"Seeker Missile";
            public override string Description=>"Occasionally deploys Seeker Missiles, very fast and high damage";
            public override int Cost=>685;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Raven){
                GetUpgradeModel().icon=new("RavenSeekerIcon");
                var Seeker=Game.instance.model.GetTowerFromId("BombShooter-020").GetAttackModel().Duplicate();
                Seeker.name="SeekerMissile";
                Seeker.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed*=2;
                Seeker.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage*=3;
                Seeker.weapons[0].rate=20;
                Seeker.range=Raven.range*2;
                Raven.AddBehavior(Seeker);
                SetUpgradeSounds(Raven,"RavenUpgrade1");
            }
        }
        public class Theia:ModUpgrade<Raven>{
            public override string DisplayName=>"Theia Raven";
            public override string Description=>"Deploys automatic turrets nearby and increases detection range";
            public override int Cost=>1075;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Raven){
                GetUpgradeModel().icon=new("RavenTheiaIcon");
                var Turret=Game.instance.model.GetTowerFromId("EngineerMonkey-100").GetBehaviors<AttackModel>().First(a=>a.name=="AttackModel_Spawner_").Duplicate();
                var TurretTower=Turret.weapons[0].projectile.GetBehaviors<CreateTowerModel>().First().tower;
                Raven.display="RavenTheiaPrefab";
                Raven.portrait=new("RavenTheiaPortrait");
                Raven.range+=20;
                Turret.name="AutoTurret";
                Turret.range=Raven.range;
                Turret.weapons[0].rate=15;
                Turret.weapons[0].projectile.display="RavenAutoTurretProjectilePrefab";
                Turret.targetProvider.Cast<RandomPositionModel>().idealDistanceWithinTrack=20;
                TurretTower.display="RavenAutoTurretBasePrefab";
                TurretTower.name="SC2Expansion-RavenAutoTurret";
                TurretTower.portrait=new("RavenAutoTurretPortrait");
                TurretTower.GetAttackModel().GetBehavior<DisplayModel>().display="RavenAutoTurretGunPrefab";
                TurretTower.GetAttackModel().range=40;
                TurretTower.GetAttackModel().weapons[0].rate=0.4f;
                TurretTower.GetAttackModel().weapons[0].projectile.GetDamageModel().damage=1;
                TurretTower.GetBehavior<TowerExpireModel>().lifespan=30;
                Raven.AddBehavior(Turret);
                Raven.GetBehaviors<AttackModel>().First(a=>a.name=="SeekerMissile").range=Raven.range*1.5f;
                SetUpgradeSounds(Raven,"RavenUpgrade2");
            }
        }
        public class CorvidReactor:ModUpgrade<Raven>{
            public override string DisplayName=>"Corvid Reactor";
            public override string Description=>"Upgrades to the Raven's reactor allows for much quicker production of Seeker Missiles and turrets";
            public override int Cost=>2455;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Raven){
                GetUpgradeModel().icon=new("RavenCorvidReactorIcon");
                Raven.GetBehaviors<AttackModel>().First(a=>a.name=="AutoTurret").weapons[0].rate/=1.5f;
                Raven.GetBehaviors<AttackModel>().First(a=>a.name=="SeekerMissile").weapons[0].rate/=1.5f;
                SetUpgradeSounds(Raven,"RavenUpgrade3");
            }
        }
        public class ScienceVessel:ModUpgrade<Raven>{
            public override string DisplayName=>"Science Vessel";
            public override string Description=>"Manned Terran flying support, replaces Seeker Missile with Irradiate, damaging gas cloud that does big damage to non Moabs";
            public override int Cost=>10930;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Raven){
                GetUpgradeModel().icon=new("RavenScienceVesselIcon");
                var Irradiate=Game.instance.model.GetTowerFromId("GlueGunner-200").GetAttackModel().Duplicate();
                Raven.RemoveBehavior(Raven.GetBehaviors<AttackModel>().First(a=>a.name=="SeekerMissile"));
                Raven.display="RavenScienceVesselPrefab";
                Raven.portrait=new("RavenScienceVesselPortrait");
                Raven.GetBehavior<DisplayModel>().ignoreRotation=true;
                Irradiate.name="Irradiate";
                Irradiate.weapons[0].rate=0.45f;
                Irradiate.weapons[0].projectile.display=null;
                Irradiate.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().lifespan=15;
                Irradiate.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().Interval=1.1f;
                Irradiate.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage=10;
                Irradiate.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damageModifierModels.
                    AddItem(new DamageModifierForTagModel("DamageModifierForTagModel","Moab",0.5f,0,false,true));
                foreach(var temp in Irradiate.weapons[0].projectile.GetBehavior<SlowModel>().Mutator.overlays){
                    temp.value.assetPath="UltraliskCavernGasPrefab";
                }
                Raven.AddBehavior(Irradiate);
            }
        }
        [HarmonyPatch(typeof(AudioFactory),"Start")]
        public class AudioFactoryStart_Patch{
            [HarmonyPostfix]
            public static void Prefix(ref AudioFactory __instance){
                if(TerranEnabled){
                    AudioFactoryInstance=__instance;
                    __instance.RegisterAudioClip("RavenBirth",TowerAssets.LoadAsset("RavenBirth").Cast<AudioClip>());
                    __instance.RegisterAudioClip("RavenUpgrade",TowerAssets.LoadAsset("RavenUpgrade").Cast<AudioClip>());
                    __instance.RegisterAudioClip("RavenUpgrade1",TowerAssets.LoadAsset("RavenUpgrade1").Cast<AudioClip>());
                    __instance.RegisterAudioClip("RavenUpgrade2",TowerAssets.LoadAsset("RavenUpgrade2").Cast<AudioClip>());
                    __instance.RegisterAudioClip("RavenUpgrade3",TowerAssets.LoadAsset("RavenUpgrade3").Cast<AudioClip>());
                }
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                return LoadModel(TowerAssets,objectId,"Raven",__instance,onComplete);
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public class ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.StartsWith("Raven")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public class WeaponSpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.towerModel.name.StartsWith("SC2Expansion-Raven")){
                    if(__instance.attack.tower.towerModel.name.Contains("Turret")){
                        __instance.attack.entity.GetDisplayNode().graphic.GetComponent<Animator>().Play("AutoTurretAttack");
                    }else{
                        __instance.attack.tower.Node.graphic.GetComponent<Animator>().Play("RavenAttack");
                    }
                }
            }
        }
    }
}