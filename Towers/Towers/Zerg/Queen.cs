namespace SC2Expansion.Towers{
    public class Queen:ModTower<ZergSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.queen);
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>550;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ZergEnabled;
        public override string Description=>"Ranged Zerg support, requires creep";
        public override void ModifyBaseTowerModel(TowerModel Queen){
            Queen.display="QueenPrefab";
            Queen.portrait=new("QueenPortrait");
            Queen.icon=new("QueenIcon");
            Queen.radius=7;
            Queen.range=50;
            var Spines=Queen.GetAttackModel();
            Spines.weapons[0].rate=7000f;
            Spines.range=Queen.range;
            Spines.weapons[0].projectile.GetDamageModel().damage=2;
            Queen.GetBehavior<DisplayModel>().display=Queen.display;
            Queen.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId="QueenBirth";
            Queen.GetBehavior<CreateSoundOnTowerPlaceModel>().sound2=Queen.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1;
            SetUpgradeSounds(Queen,"QueenUpgrade");
        }
        public class QueenGroovedSpines:ModUpgrade<Queen>{
            public override string DisplayName=>"Grooved Spines";
            public override string Description=>"Evolving small grooves into the spines increases their range";
            public override int Cost=>475;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Queen){
                GetUpgradeModel().icon=new("HydraliskGroovedSpinesIcon");
                Queen.range+=15;
                Queen.GetAttackModel().range=Queen.range;
                SetUpgradeSounds(Queen,"QueenUpgrade1");
            }
        }
        public class CreepTumor:ModUpgrade<Queen>{
            public override string DisplayName=>"Creep Tumor";
            public override string Description=>"Spawns a Creep Tumor, generating creep around it";
            public override int Cost=>785;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Queen){
                GetUpgradeModel().icon=new("QueenCreepTumorIcon");
                var SpawnCreepTumor=Game.instance.model.GetTowerFromId("EngineerMonkey-100").behaviors.First(a=>a.name.Contains("Spawner")).Cast<AttackModel>().Duplicate();
                var CreepTumor=SpawnCreepTumor.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                var CreepBuff=Game.instance.model.GetTowerFromId("SniperMonkey-050").GetBehavior<RateSupportModel>().Duplicate();
                SpawnCreepTumor.name="SpawnCreepTumor";
                SpawnCreepTumor.RemoveBehavior<RandomPositionModel>();
                SpawnCreepTumor.AddBehavior(new RandomPositionBasicModel("RandomPositionBasicModel",15,20,10,false));
                SpawnCreepTumor.targetProvider=SpawnCreepTumor.GetBehavior<RandomPositionBasicModel>();
                SpawnCreepTumor.GetBehavior<RotateToTargetModel>().additionalRotation=180;
                SpawnCreepTumor.weapons[0].rate=300000;
                CreepTumor.display="QueenCreepTumorPrefab";
                CreepTumor.RemoveBehavior<AttackModel>();
                CreepTumor.GetBehavior<TowerExpireModel>().lifespan=25;
                CreepBuff.multiplier=0.0001f;
                CreepBuff.isGlobal=false;
                CreepBuff.buffIconName=null;
                string[] ZergBuildings=new string[4]{"SC2Expansion-SpawningPool","SC2Expansion-UltraliskCavern","SC2Expansion-BanelingNest","SC2Expansion-Queen"};
                CreepBuff.filters[0].Cast<FilterInBaseTowerIdModel>().baseIds=ZergBuildings;
                CreepTumor.AddBehavior(CreepBuff);
                Queen.AddBehavior(SpawnCreepTumor);
                SetUpgradeSounds(Queen,"QueenUpgrade2");
            }
        }
        public class WildMutation:ModUpgrade<Queen>{
            public override string DisplayName=>"WildMutation";
            public override string Description=>"Temporalily buff's the attack speed and damage of all nearby Zerg. Upgrades damage";
            public override int Cost=>2350;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Queen){
                GetUpgradeModel().icon=new("QueenWildMutationIcon");
                var WildMutation=Game.instance.model.GetTowerFromId("AdmiralBrickell 10").behaviors.First(a=>a.name.Contains("AbilityModel_FirstAbility")).Cast<AbilityModel>().Duplicate();
                WildMutation.icon=new("QueenWildMutationIcon");
                WildMutation.name="WildMutation";
                WildMutation.displayName="Wild Mutation";
                WildMutation.GetBehavior<ActivateRateSupportZoneModel>().lifespan=20;
                WildMutation.GetBehavior<ActivateRateSupportZoneModel>().range=Queen.range+20;
                WildMutation.AddBehavior(new ActivateTowerDamageSupportZoneModel("ActivateTowerDamageSupportZoneModel","Queen:WildMutation",true,2,Queen.range+20,99999,false,20,null,"WildMutation",
                    "QueenWildMutationIcon",0,false,false,null));
                Queen.AddBehavior(WildMutation);
                Queen.display="QueenLeviathanPrefab";
                Queen.GetBehaviors<AttackModel>().First(a=>a.name.Contains("Attack")).weapons[0].projectile.GetDamageModel().damage+=2;
                SetUpgradeSounds(Queen,"QueenUpgrade3");
            }
        }
        public class BroodMother:ModUpgrade<Queen>{
            public override string DisplayName=>"Evolve into a Brood Mother";
            public override string Description=>"Brood Mother's control entire Broods of Zerg. Can call down a Drop Pod containing 6 Hydralisks";
            public override int Cost=>8950;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Queen){
                    GetUpgradeModel().icon=new("QueenBroodMotherIcon");
                    var DropPodAbility=Game.instance.model.towers.First(a=>a.name.Contains("MonkeyBuccaneer-040")).GetBehavior<AbilityModel>().Duplicate();
                    var DropPod=DropPodAbility.GetBehavior<ActivateAttackModel>();
                    DropPod.attacks[0]=Game.instance.model.towers.First(a=>a.name.Contains("EngineerMonkey-100")).behaviors.First(a=>a.name.Contains("Spawner")).Cast<AttackModel>().Duplicate();
                    var DropPodStartTower=DropPod.attacks[0].weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                    DropPodStartTower.GetAttackModel().weapons[0].projectile.AddBehavior(DropPod.attacks[0].weapons[0].projectile.GetBehavior<CreateTowerModel>().Duplicate());
                    var DropPodEndTower=DropPodStartTower.GetAttackModel().weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                    var SpawnHydralisk=Game.instance.model.towers.First(a=>a.name.Contains("EngineerMonkey-100")).behaviors.First(a=>a.name.Contains("Spawner")).Cast<AttackModel>().Duplicate();
                    var Hydralisk=SpawnHydralisk.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                    var Spines=Hydralisk.GetAttackModel();
                    DropPodAbility.name="ZergDropPod";
                    DropPodAbility.icon=new("QueenDropPodIcon");
                    DropPodAbility.GetBehavior<CreateSoundOnAbilityModel>().sound.assetId="QueenAbility";
                    DropPodStartTower.GetAttackModel().weapons[0].projectile.RemoveBehavior<DamageModel>();
                    DropPodStartTower.GetAttackModel().weapons[0].rate=22000f;
                    DropPodStartTower.GetAttackModel().targetProvider=new RandomPositionModel("RandomPositionModel",0,1,1,false,1,false,false,"Land",false,false,0);
                    DropPodStartTower.GetBehavior<TowerExpireModel>().lifespan=2.3f;
                    DropPodStartTower.display="QueenDropPodStartPrefab";
                    DropPodEndTower.display="QueenDropPodEndPrefab";
                    DropPodEndTower.GetBehavior<TowerExpireModel>().lifespan=20;
                    DropPodEndTower.RemoveBehavior<AttackModel>();
                    SpawnHydralisk.name="SpawnHydralisk";
                    SpawnHydralisk.weapons[0].rate=33333;
                    SpawnHydralisk.range=40;
                    SpawnHydralisk.targetProvider.Cast<RandomPositionModel>().minDistance=10;
                    SpawnHydralisk.targetProvider.Cast<RandomPositionModel>().maxDistance=25;
                    SpawnHydralisk.RemoveBehavior<RotateToTargetModel>();
                    SpawnHydralisk.weapons[0].projectile.display=null;
                    Hydralisk.behaviors=Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).behaviors.Duplicate();
                    Hydralisk.AddBehavior(new TowerExpireModel("TowerExpireModel",20,false,false));
                    Hydralisk.name="HydraliskSpawned";
                    Hydralisk.baseId="Hydralisk";
                    Hydralisk.display="HydraliskPrefab";
                    Hydralisk.portrait=new("HydraliskPortrait");
                    Hydralisk.towerSet="Primary";
                    Hydralisk.radius=7;
                    Hydralisk.range=40;
                    Spines.weapons[0].rate=0.55f;
                    Spines.range=Hydralisk.range;
                    Spines.weapons[0].projectile.GetDamageModel().damage=6;
                    DropPodEndTower.AddBehavior(SpawnHydralisk);
                    Queen.AddBehavior(DropPodAbility);
                    Queen.GetAttackModel().weapons[0].projectile.GetDamageModel().damage+=5;
                    Queen.display="QueenBroodMotherPrefab";
                SetUpgradeSounds(Queen,"QueenUpgrade4");
            }
        }
        public class Zagara:ModUpgrade<Queen>{
            public override string DisplayName=>"Zagara";
            public override string Description=>"The first Brood Mother, now controls the entire Swarm. Can buff everything's attack speed and upgrade Drop Pods to call down multiple Drop Pods with Hydralisks and Roaches";
            public override int Cost=>43750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Queen){
                GetUpgradeModel().icon=new("QueenZagaraIcon");
                var DropPod=Queen.GetAbilites().First(a=>a.name.Equals("ZergDropPod"));
                Queen.display="QueenZagaraPrefab";
            }
        }
        [HarmonyPatch(typeof(AudioFactory),"Start")]
        public class AudioFactoryStart_Patch{
            [HarmonyPostfix]
            public static void Prefix(ref AudioFactory __instance){
                if(ZergEnabled){
                    AudioFactoryInstance=__instance;
                    __instance.RegisterAudioClip("QueenBirth",TowerAssets.LoadAsset("QueenBirth").Cast<AudioClip>());
                    __instance.RegisterAudioClip("QueenUpgrade",TowerAssets.LoadAsset("QueenUpgrade").Cast<AudioClip>());
                    __instance.RegisterAudioClip("QueenUpgrade1",TowerAssets.LoadAsset("QueenUpgrade1").Cast<AudioClip>());
                    __instance.RegisterAudioClip("QueenUpgrade2",TowerAssets.LoadAsset("QueenUpgrade2").Cast<AudioClip>());
                    __instance.RegisterAudioClip("QueenUpgrade3",TowerAssets.LoadAsset("QueenUpgrade3").Cast<AudioClip>());
                    __instance.RegisterAudioClip("QueenUpgrade4",TowerAssets.LoadAsset("QueenUpgrade4").Cast<AudioClip>());
                    __instance.RegisterAudioClip("QueenAbility",TowerAssets.LoadAsset("QueenAbility").Cast<AudioClip>());
                    __instance.RegisterAudioClip("QueenSpawnCreepTumor",TowerAssets.LoadAsset("QueenSpawnCreepTumor").Cast<AudioClip>());
                    __instance.RegisterAudioClip("QueenDropPod",TowerAssets.LoadAsset("QueenDropPod").Cast<AudioClip>());
                    __instance.RegisterAudioClip("QueenDropPodVO",TowerAssets.LoadAsset("QueenDropPodVO").Cast<AudioClip>());
                    __instance.RegisterAudioClip("QueenDropPodVO1",TowerAssets.LoadAsset("QueenDropPodVO1").Cast<AudioClip>());
                    __instance.RegisterAudioClip("QueenDropPodVO2",TowerAssets.LoadAsset("QueenDropPodVO2").Cast<AudioClip>());
                    __instance.RegisterAudioClip("QueenDropPodVO3",TowerAssets.LoadAsset("QueenDropPodVO3").Cast<AudioClip>());
                    __instance.RegisterAudioClip("QueenDropPodVO4",TowerAssets.LoadAsset("QueenDropPodVO4").Cast<AudioClip>());
                    __instance.RegisterAudioClip("QueenDropPodVO5",TowerAssets.LoadAsset("QueenDropPodVO5").Cast<AudioClip>());
                }
            }
        }
        /*[HarmonyPatch(typeof(Tower),"Initialise")]
        public class TowerInitialize_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Model modelToUse){
                if(modelToUse.Cast<TowerModel>().display=="QueenDropPodStartPrefab"){
                    PlaceDropPodEnd(modelToUse.Cast<TowerModel>());
                }
                static async Task PlaceDropPodEnd(TowerModel towerModel){
                    var SpawnHydralisk=Game.instance.model.towers.First(a=>a.name.Contains("EngineerMonkey-100")).behaviors.First(a=>a.name.Contains("Spawner")).Cast<AttackModel>().Duplicate();
                    var Hydralisk=SpawnHydralisk.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                    var Spines=Hydralisk.GetAttackModel();
                    SpawnHydralisk.name="SpawnHydralisk";
                    SpawnHydralisk.weapons[0].rate=50000;
                    SpawnHydralisk.range=40;
                    SpawnHydralisk.targetProvider.Cast<RandomPositionModel>().minDistance=10;
                    SpawnHydralisk.targetProvider.Cast<RandomPositionModel>().maxDistance=25;
                    SpawnHydralisk.RemoveBehavior<RotateToTargetModel>();
                    SpawnHydralisk.weapons[0].projectile.display=null;
                    Hydralisk.behaviors=Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).behaviors.Duplicate();
                    Hydralisk.AddBehavior(new TowerExpireModel("TowerExpireModel",20,false,false));
                    Hydralisk.name="HydraliskSpawned";
                    Hydralisk.baseId="Hydralisk";
                    Hydralisk.display="HydraliskPrefab";
                    Hydralisk.portrait=new("HydraliskPortrait");
                    Hydralisk.towerSet="Primary";
                    Hydralisk.radius=7;
                    Hydralisk.range=40;
                    Spines.weapons[0].rate=0.55f;
                    Spines.range=Hydralisk.range;
                    Spines.weapons[0].projectile.GetDamageModel().damage=6;
                    await Task.Delay(2200);
                    towerModel.display="QueenDropPodEndPrefab";
                    towerModel.AddBehavior(SpawnHydralisk);
                }
            }
        }*/
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                return LoadModel(TowerAssets,objectId,"Queen",__instance,onComplete);
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public class ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.StartsWith("Queen")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public class WeaponSpawnDart_Patch{
            static bool DropPodFiredOnce;
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.towerModel.name.StartsWith("SC2Expansion-Queen")){
                    if(__instance.attack.attackModel.name.Contains("CreepTumor")){
                        __instance.attack.tower.Node.graphic.GetComponent<Animator>().Play("QueenSpawnCreepTumorStart");
                    }
                    if(__instance.attack.attackModel.name.Contains("AttackModel_Attack_")){
                        __instance.attack.tower.Node.graphic.GetComponent<Animator>().Play("QueenAttack");
                    }
                }
                if(__instance.attack.tower.towerModel.display=="QueenDropPodStartPrefab"&&DropPodFiredOnce==false){
                    __instance.newProjectiles.Clear();
                    __instance.newProjectiles2.Clear();
                    DropPodFiredOnce=true;
                }
            }
        }
        [HarmonyPatch(typeof(Ability),"Activate")]
        public static class Activate_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Ability __instance){
                if(__instance.tower.namedMonkeyKey.Contains("Queen")){
                    __instance.tower.Node.graphic.GetComponent<Animator>().Play("QueenAbility");
                }
            }
        }
    }
}