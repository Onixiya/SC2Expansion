namespace SC2Expansion.Towers{
    public class Hatchery:ModTower<ZergSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.hatchery);
        public override string DisplayName=>"Hatchery";
        public override string BaseTower=>"BananaFarm-003";
        public override int Cost=>800;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ZergEnabled;
        public override string Description=>"Primary Zerg hub, generates creep and provides income";
        public override void ModifyBaseTowerModel(TowerModel Hatchery){
            Hatchery.display="HatcheryPrefab";
            Hatchery.portrait=new("HatcheryPortrait");
            Hatchery.icon=new("HatcheryIcon");
            Hatchery.range=65;
            Hatchery.RemoveBehavior<RectangleFootprintModel>();
            Hatchery.footprint=Game.instance.model.GetTowerFromId("DartMonkey").footprint.Duplicate();
            Hatchery.radius=37.5f;
            var Income=Hatchery.GetAttackModel();
            Income.weapons[0].emission=new SingleEmissionModel("SingleEmissionModel",null);
            Income.weapons[0].behaviors=null;
            Income.weapons[0].rate=3.65f;
            Income.weapons[0].projectile.GetBehavior<CashModel>().maximum+=20;
            Income.weapons[0].projectile.GetBehavior<CashModel>().minimum+=20;
            Income.name="Income";
            Hatchery.GetBehavior<DisplayModel>().display=Hatchery.display;
            var CreepBuff=Game.instance.model.GetTowerFromId("SniperMonkey-050").GetBehavior<RateSupportModel>().Duplicate();
            CreepBuff.multiplier=0.0001f;
            CreepBuff.isGlobal=false;
            CreepBuff.buffIconName=null;
            CreepBuff.filters[0].Cast<FilterInBaseTowerIdModel>().baseIds=new string[]{"SC2Expansion-SpawningPool","SC2Expansion-UltraliskCavern","SC2Expansion-SpawningPool","SC2Expansion-Queen"};
            Hatchery.AddBehavior(CreepBuff);
        }
        public class Drones:ModUpgrade<Hatchery>{
            public override string DisplayName=>"Additional Drones";
            public override string Description=>"Morphing more drones provides income faster";
            public override int Cost=>700;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Hatchery){
                GetUpgradeModel().icon=new("HatcheryDronesIcon");
                Hatchery.GetAttackModel().weapons[0].rate=2.75f;
            }
        }
        public class Extractors:ModUpgrade<Hatchery>{
            public override string DisplayName=>"Extractors";
            public override string Description=>"Morphing Extractors over local Vespene geysers increases the income made";
            public override int Cost=>930;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Hatchery){
                GetUpgradeModel().icon=new("HatcheryExtractorIcon");
                var Income=Hatchery.GetAttackModel();
                Income.weapons[0].projectile.GetBehavior<CashModel>().maximum+=30;
                Income.weapons[0].projectile.GetBehavior<CashModel>().minimum+=30;
            }
        }
        public class Lair:ModUpgrade<Hatchery>{
            public override string DisplayName=>"Morph into Lair";
            public override string Description=>"Morphes into a Lair, automatically spawns T2 Zerglings and increases income made";
            public override int Cost=>1825;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Hatchery){
                GetUpgradeModel().icon=new("HatcheryLairIcon");
                Hatchery.display="HatcheryLairPrefab";
                Hatchery.portrait=new("HatcheryLairPortrait");
                Hatchery.range=75;
                var SpawnZergling=Game.instance.model.GetTowerFromId("WizardMonkey-004").GetBehaviors<AttackModel>().First(a=>a.name=="AttackModel_Attack Necromancer_").Duplicate();
                var NecZone=Game.instance.model.GetTowerFromId("WizardMonkey-004").GetBehavior<NecromancerZoneModel>().Duplicate();
                var Income=Hatchery.GetAttackModel();
                SpawnZergling.weapons[0].projectile.display="SpawningPoolZerglingWingPrefab";
                SpawnZergling.weapons[0].emission.Cast<NecromancerEmissionModel>().maxPiercePerBloon=30;
                SpawnZergling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=99999;
                SpawnZergling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection=false;
                SpawnZergling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=1;
                SpawnZergling.weapons[0].projectile.GetDamageModel().damage=1;
                SpawnZergling.weapons[0].projectile.radius=4;
                SpawnZergling.name="SpawnZergling";
                SpawnZergling.weapons[0].projectile.pierce=13;
                SpawnZergling.weapons[0].rate=1.5f;
                SpawnZergling.range=999;
                NecZone.attackUsedForRangeModel.range=999;
                Hatchery.AddBehavior(SpawnZergling);
                Hatchery.AddBehavior(NecZone);
                Income.weapons[0].projectile.GetBehavior<CashModel>().maximum+=25;
                Income.weapons[0].projectile.GetBehavior<CashModel>().minimum+=25;
            }
        }
        public class Nydus:ModUpgrade<Hatchery>{
            public override string DisplayName=>"Nydus Network";
            public override string Description=>"Connects Lair to the local Nydus Network. Allows spawning of short lived Nydus Worms anywhere sending out lots of Zerglings and Hydralisks";
            public override int Cost=>3450;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Hatchery){
                //gettowermodel isn't working for the hydralisk at all, its just returning the base tower without any changes being done to it
                GetUpgradeModel().icon=new("HatcheryNydusNetworkIcon");
                var Nydus=Game.instance.model.GetTowerFromId("MonkeyBuccaneer-040").GetBehavior<AbilityModel>().Duplicate();
                var NydusAttack=Nydus.GetBehavior<ActivateAttackModel>();
                NydusAttack.attacks[0]=Game.instance.model.GetTowerFromId("EngineerMonkey-100").behaviors.First(a=>a.name.Contains("Spawner")).Cast<AttackModel>().Duplicate();
                var NydusWorm=NydusAttack.attacks[0].weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                var SpawnHydralisk=Game.instance.model.GetTowerFromId("EngineerMonkey-100").behaviors.First(a=>a.name.Contains("Spawner")).Cast<AttackModel>().Duplicate();
                var Hydralisk=SpawnHydralisk.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                var Spines=Hydralisk.GetAttackModel();
                Nydus.icon=new("HatcheryNydusWormIcon");
                Nydus.cooldown=85;
                Nydus.name="SpawnNydus";
                Nydus.RemoveBehavior<RotateToTargetModel>();
                NydusAttack.attacks[0].range=150;
                NydusAttack.attacks[0].targetProvider.Cast<RandomPositionModel>().minDistance=100;
                NydusAttack.attacks[0].targetProvider.Cast<RandomPositionModel>().maxDistance=150;
                NydusAttack.attacks[0].GetBehavior<RandomPositionModel>().minDistance=100;
                NydusAttack.attacks[0].GetBehavior<RandomPositionModel>().maxDistance=150;
                NydusAttack.attacks[0].RemoveBehavior<RotateToTargetModel>();
                NydusAttack.attacks[0].weapons[0].projectile.display=null;
                NydusWorm.name="NydusWorm";
                NydusWorm.display="HatcheryNydusWormPrefab";
                NydusWorm.GetAttackModel().GetBehavior<DisplayModel>().display=null;
                NydusWorm.RemoveBehavior<AttackModel>();
                NydusWorm.AddBehavior(Hatchery.GetBehavior<NecromancerZoneModel>().Duplicate());
                NydusWorm.AddBehavior(Hatchery.behaviors.First(a=>a.name.Contains("Zergling")).Duplicate());
                NydusWorm.AddBehavior(SpawnHydralisk);
                NydusWorm.behaviors.First(a=>a.name.Contains("Zergling")).Cast<AttackModel>().weapons[0].rate=0.65f;
                SpawnHydralisk.name="SpawnHydralisk";
                SpawnHydralisk.weapons[0].rate=5;
                SpawnHydralisk.range=40;
                SpawnHydralisk.targetProvider.Cast<RandomPositionModel>().minDistance=40;
                SpawnHydralisk.targetProvider.Cast<RandomPositionModel>().maxDistance=60;
                SpawnHydralisk.GetBehavior<RandomPositionModel>().minDistance=40;
                SpawnHydralisk.GetBehavior<RandomPositionModel>().maxDistance=60;
                SpawnHydralisk.RemoveBehavior<RotateToTargetModel>();
                SpawnHydralisk.weapons[0].projectile.display=null;
                Hydralisk.behaviors=Game.instance.model.GetTowerFromId("DartMonkey").behaviors.Duplicate();
                Hydralisk.AddBehavior(new TowerExpireModel("TowerExpireModel",30,false,false));
                Hydralisk.name="HydraliskSpawned";
                Hydralisk.baseId="Hydralisk";
                Hydralisk.display="HydraliskPrefab";
                Hydralisk.portrait=new("HydraliskPortrait");
                Hydralisk.radius=7;
                Hydralisk.range=40;
                Spines.weapons[0].rate=0.55f;
                Spines.range=Hydralisk.range;
                Spines.weapons[0].projectile.GetDamageModel().damage=6;
                Hatchery.RemoveBehavior<RotateToTargetModel>();
                Hatchery.AddBehavior(Nydus);
            }
        }
        public class Hive:ModUpgrade<Hatchery>{
            public override string DisplayName=>"Morph into Hive";
            public override string Description=>"Morphes into Hive, now spawns Noxious Ultralisks and upgrades Zerglings. Upgrades all units from Nydus Worms and decreases the cooldown. Increases income made massively";
            public override int Cost=>10780;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Hatchery){
                GetUpgradeModel().icon=new("HatcheryHiveIcon");
                var Nydus=Hatchery.GetBehavior<AbilityModel>();
                var NydusWorm=Nydus.GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                var Hydralisk=NydusWorm.behaviors.First(a=>a.name.Contains("Hydralisk")).Cast<AttackModel>().weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                NydusWorm.RemoveBehavior(NydusWorm.GetBehaviors<AttackModel>().First(a=>a.name.Contains("Zergling")));
                NydusWorm.AddBehavior(Game.instance.model.towers.First(a=>a.name.Contains("WizardMonkey-004")).behaviors.First(a=>a.name.Equals("AttackModel_Attack Necromancer_")).Duplicate());
                var SpawnZerglingNydus=NydusWorm.behaviors.First(a=>a.name.Equals("AttackModel_Attack Necromancer_")).Cast<AttackModel>();
                var SpawnZergling=Hatchery.behaviors.First(a=>a.name.Contains("Zergling")).Cast<AttackModel>();
                //UltraliskWeapon.projectile=Hatchery.behaviors.First(a=>a.name.Contains("Zergling")).Cast<AttackModel>().weapons[0].projectile.Duplicate();
                var GasCloud=Game.instance.model.GetTowerFromId("EngineerMonkey-030").behaviors.First(a=>a.name.Contains("CleansingFoam")).Cast<AttackModel>().weapons[0].projectile.
                    GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                var Income=Hatchery.behaviors.First(a=>a.name.Equals("Income")).Cast<AttackModel>();
                Nydus.icon=new("HatcheryOmegaWormIcon");
                Nydus.cooldown=55;
                NydusWorm.name="OmegaWorm";
                NydusWorm.display="HatcheryOmegaWormPrefab";
                Hydralisk.display="HydraliskHunterKillerPrefab";
                Hydralisk.portrait=new("HydraliskHunterKillerPortrait");
                Hydralisk.range=45;
                Hydralisk.GetAttackModel().range=Hydralisk.range;
                Hydralisk.GetAttackModel().weapons[0].rate=0.325f;
                Hydralisk.GetAttackModel().weapons[0].projectile.GetDamageModel().damage=12;
                Hydralisk.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties=0;
                SpawnZerglingNydus.name="SpawnZergling";
                SpawnZerglingNydus.weapons[0].projectile.pierce=17;
                SpawnZerglingNydus.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=1.2f;
                SpawnZerglingNydus.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection=false;
                SpawnZerglingNydus.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=9999;
                SpawnZerglingNydus.weapons[0].projectile.GetDamageModel().damage=6;
                SpawnZerglingNydus.weapons[0].projectile.display="SpawningPoolSwarmlingPrefab";
                SpawnZergling.weapons[0].projectile.display="SpawningPoolZerglingWingPrefab";
                SpawnZergling.weapons[0].emission.Cast<NecromancerEmissionModel>().maxPiercePerBloon=30;
                SpawnZergling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=99999;
                SpawnZergling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection=false;
                SpawnZergling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=1.2f;
                SpawnZergling.weapons[0].projectile.GetDamageModel().damage=3;
                SpawnZergling.weapons[0].projectile.radius=4;
                SpawnZergling.weapons[0].projectile.pierce=13;
                SpawnZergling.weapons[0].rate=1.3f;
                SpawnZergling.weapons[0].emission.Cast<NecromancerEmissionModel>().maxRbeSpawnedPerSecond=200;
                /*UltraliskWeapon.projectile.display="UltraliskCavernNoxiousPrefab";
                UltraliskWeapon.projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=99999;
                UltraliskWeapon.projectile.GetBehavior<TravelAlongPathModel>().speedFrames=0.6f;
                UltraliskWeapon.projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection=false;
                UltraliskWeapon.projectile.GetDamageModel().damage=6;
                UltraliskWeapon.projectile.radius=7;
                UltraliskWeapon.projectile.pierce=25;
                GasCloud.projectile.RemoveBehavior<RemoveBloonModifiersModel>();
                GasCloud.projectile.display="GasPrefab";
                GasCloud.projectile.AddBehavior(new DamageModel("DamageModel",1,1,false,false,true,0));
                GasCloud.projectile.pierce=9999;
                GasCloud.projectile.GetBehavior<AgeModel>().lifespan=6;
                UltraliskWeapon.projectile.AddBehavior(GasCloud);*/
                Income.weapons[0].projectile.GetBehavior<CashModel>().maximum=1000;
                Income.weapons[0].projectile.GetBehavior<CashModel>().minimum=1000;
                Hatchery.display="HatcheryHivePrefab";
                Hatchery.portrait=new("HatcheryHivePortrait");
                Hatchery.range=83;
                //for some reason, its not cloning the dart monkeys behaviors when changing the hydralisk here, ty amman#2583 for bringing this up
                Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].rate=0.95f;
                Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.GetDamageModel().damage=1;
                Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties=(BloonProperties)17;
            }
        }
        /*[HarmonyPatch(typeof(NecromancerEmission),nameof(NecromancerEmission.BaseEmit))]
        public class test{
            [HarmonyPrefix]
            public static void Prefix(){
                MelonLogger.Msg("test");
                /*if(__instance.Weapon.attack.tower.towerModel.name.StartsWith("SC2Expansion-Hatchery")){
                    /*if(new System.Random().Next(1,5)<3){
                        __instance.newProjectiles2.First().projectileModel=Hive.UltraliskWeapon.projectile;
                        MelonLogger.Msg("ultralisk");
                    }else{
                        __instance.newProjectiles2.First().projectileModel=Hive.ZerglingWeapon.projectile;
                        MelonLogger.Msg("zergling");
                    }
                    MelonLogger.Msg("test");
                }
            }
        }*/
        [HarmonyPatch(typeof(AudioFactory),"Start")]
        public class AudioFactoryStart_Patch{
            [HarmonyPostfix]
            public static void Prefix(ref AudioFactory __instance){
                if(ZergEnabled){
                    AudioFactoryInstance=__instance;
                    __instance.RegisterAudioClip("HatcheryNydusWormBirth",TowerAssets.LoadAsset("HatcheryNydusWormBirth").Cast<AudioClip>());
                    __instance.RegisterAudioClip("HatcheryOmegaWormBirth",TowerAssets.LoadAsset("HatcheryOmegaWormBirth").Cast<AudioClip>());
                }
            }
        }
        [HarmonyPatch(typeof(Tower),"Initialise")]
        public class TowerInitialize_Patch{
            [HarmonyPostfix]
            public static void Postfix(Model modelToUse){
                switch(modelToUse.Cast<TowerModel>().name){
                    case "NydusWorm":
                        AudioFactoryInstance.PlaySoundFromUnity(null,"HatcheryNydusWormBirth","FX",2,1);
                        break;
                    case "OmegaWorm":
                        AudioFactoryInstance.PlaySoundFromUnity(null,"HatcheryOmegaWormBirth","FX",2,1);
                        break;
                }
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                return LoadModel(TowerAssets,objectId,"Hatchery",__instance,onComplete);
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public class ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.StartsWith("Hatchery")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
    }
}