namespace SC2Expansion.Towers{
    public class Queen:ModTower<ZergSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.queen);
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>3;
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
        }
        public class QueenGroovedSpines:ModUpgrade<Queen>{
            public override string Name=>"QueenGroovedSpines";
            public override string DisplayName=>"Grooved Spines";
            public override string Description=>"Evolving small grooves into the spines increases their range";
            public override int Cost=>750;
            public override int Path=>0;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Queen){
                GetUpgradeModel().icon=new("HydraliskGroovedSpinesIcon");
                Queen.range+=15;
                Queen.GetAttackModel().range=Queen.range;
            }
        }
        public class CreepTumor:ModUpgrade<Queen>{
            public override string Name=>"CreepTumor";
            public override string DisplayName=>"Creep Tumor";
            public override string Description=>"Spawns a Creep Tumor, generating creep around it";
            public override int Cost=>750;
            public override int Path=>0;
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
            }
        }
        public class WildMutation:ModUpgrade<Queen>{
            public override string Name=>"WildMutation";
            public override string DisplayName=>"WildMutation";
            public override string Description=>"Temporalily buff's the attack speed and damage of all nearby Zerg. Upgrades damage";
            public override int Cost=>750;
            public override int Path=>0;
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
            }
        }
        /*public class BroodMother:ModUpgrade<Queen>{
            public override string Name=>"BroodMother";
            public override string DisplayName=>"Evolve into a Brood Mother";
            public override string Description=>"Brood Mother's control entire Broods of Zerg. Can call down a Drop Pod containing Hydralisks and Zerglings";
            public override int Cost=>750;
            public override int Path=>0;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Queen){
                GetUpgradeModel().icon=new("QueenBroodMotherIcon");
                var DropPodAbility=Game.instance.model.towers.First(a=>a.name.Contains("MonkeyBuccaneer-040")).GetBehavior<AbilityModel>().Duplicate();
                var DropPodEnd=DropPodAbility.GetBehavior<ActivateAttackModel>();
                DropPodEnd.attacks[0]=Game.instance.model.towers.First(a=>a.name.Contains("EngineerMonkey-100")).behaviors.First(a=>a.name.Contains("Spawner")).Cast<AttackModel>().Duplicate();
                var DropPodEndTower=DropPodEnd.attacks[0].weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                var SpawnHydralisk=Game.instance.model.towers.First(a=>a.name.Contains("EngineerMonkey-100")).behaviors.First(a=>a.name.Contains("Spawner")).Cast<AttackModel>().Duplicate();
                var SpawningPool=GetTowerModel<SpawningPool>(3,0,0).Duplicate();
                var Hydralisk=SpawnHydralisk.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                var Spines=Hydralisk.GetAttackModel();
                DropPodAbility.icon=new("QueenDropPodIcon");
                DropPodAbility.cooldown=85;
                DropPodAbility.name="ZergDropPod";
                DropPodEnd.attacks[0].range=150;
                DropPodEnd.attacks[0].targetProvider.Cast<RandomPositionModel>().minDistance=50;
                DropPodEnd.attacks[0].targetProvider.Cast<RandomPositionModel>().maxDistance=100;
                DropPodEnd.attacks[0].RemoveBehavior<RotateToTargetModel>();
                DropPodEnd.attacks[0].weapons[0].projectile.display=null;
                DropPodEndTower.name="DropPodEnd";
                DropPodEndTower.display="QueenDropPodEndPrefab";
                DropPodEndTower.GetAttackModel().GetBehavior<DisplayModel>().display=null;
                DropPodEndTower.RemoveBehavior<AttackModel>();
                SpawningPool.name="whatdoicallthis";
                DropPodEndTower.behaviors=DropPodEndTower.behaviors.Add(SpawnHydralisk,SpawningPool.GetAttackModel(),SpawningPool.GetBehavior<NecromancerZoneModel>());
                SpawningPool.GetAttackModel().weapons[1].rate=15000;
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
                Queen.AddBehavior(DropPodAbility);
                Queen.display="QueenBroodMotherPrefab";
            }
        }
        public class Zagara:ModUpgrade<Queen>{
            public override string Name=>"Zagara";
            public override string DisplayName=>"Zagara";
            public override string Description=>"The first Brood Mother, now controls the entire Swarm. Can buff everything's attack speed and upgrade Drop Pods to call down multiple Drop Pods with Hydralisks and Roaches";
            public override int Cost=>750;
            public override int Path=>0;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Queen){
                GetUpgradeModel().icon=new("QueenZagaraIcon");
                var DropPod=Queen.GetAbilites().First(a=>a.name.Contains("DropPod"));
                Queen.display="QueenZagaraPrefab";
            }
        }*/
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            public static Dictionary<string,UnityDisplayNode>DisplayDict=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!DisplayDict.ContainsKey(objectId)&&objectId.Contains("Queen")){
                    var udn=uObject.Instantiate(TowerAssets.LoadAsset(objectId).Cast<GameObject>(),__instance.PrototypeRoot).AddComponent<UnityDisplayNode>();
                    udn.transform.position=new(-3000,0);
                    udn.name="SC2Expansion-Queen";
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
                if(reference!=null&&reference.guidRef.Contains("Queen")){
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
                if(__instance.attack.tower.towerModel.name.Contains("Queen")){
                    if(__instance.attack.attackModel.name.Contains("CreepTumor")){
                       // __instance.attack.tower.Node.graphic.GetComponentInParent<AudioSource>().PlayOneShot(Assets.LoadAsset("QueenSpawnCreepTumorClip").Cast<AudioClip>(),ModVolume);
                        __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("QueenSpawnCreepTumorStart");
                    }
                    if(__instance.attack.attackModel.name.Contains("AttackModel_Attack_")){
                        __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("QueenAttack");
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Ability),"Activate")]
        public static class Activate_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Ability __instance){
                if(__instance.tower.namedMonkeyKey.Contains("Queen")){
                    __instance.tower.Node.graphic.GetComponentInParent<Animator>().Play("QueenAbility");
                    //__instance.tower.Node.graphic.GetComponentInParent<AudioSource>().PlayOneShot(Assets.LoadAsset("QueenAbilityClip").Cast<AudioClip>(),ModVolume);
                }
            }
        }
    }
}