namespace SC2Expansion.Towers{
    public class Queen:ModTower{
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Ranged Zerg support, requires creep";
        public override void ModifyBaseTowerModel(TowerModel Queen){
            Queen.display="QueenPrefab";
            Queen.portrait=new("QueenPortrait");
            Queen.icon=new("QueenIcon");
            Queen.towerSet="Primary";
            Queen.emoteSpriteLarge=new("Zerg");
            Queen.radius=7;
            Queen.range=50;
            var Spines=Queen.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            Spines.weapons[0].rate=7000f;
            Spines.range=Queen.range;
            Spines.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Damage")).Cast<DamageModel>().damage=2;
            Queen.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="QueenPrefab";
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
                Queen.range=65;
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
                var SpawnCreepTumor=Game.instance.model.towers.First(a=>a.name.Contains("EngineerMonkey-100")).behaviors.First(a=>a.name.Contains("Spawner")).Cast<AttackModel>().Duplicate();
                var CreepTumor=SpawnCreepTumor.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                var CreepBuff=Game.instance.model.towers.First(a=>a.name.Contains("SniperMonkey-050")).GetBehavior<RateSupportModel>().Duplicate();
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
                var WildMutation=Game.instance.model.towers.First(a=>a.name.Contains("AdmiralBrickell 10")).behaviors.First(a=>a.name.Contains("AbilityModel_FirstAbility")).Cast<AbilityModel>().Duplicate();
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
        public class BroodMother:ModUpgrade<Queen>{
            public override string Name=>"BroodMother";
            public override string DisplayName=>"Evolve into a Brood Mother";
            public override string Description=>"Brood Mother's control entire Broods of Zerg. Can call down a Drop Pod containing Hydralisks and Zerglings";
            public override int Cost=>750;
            public override int Path=>0;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Queen){
                GetUpgradeModel().icon=new("QueenBroodMotherIcon");
                var DropPodAbility=Game.instance.model.GetTowerFromId("MonkeyBuccaneer-040").GetAbility().Duplicate();
                var DropPodAttack=Game.instance.model.GetTowerFromId("EngineerMonkey-100").GetBehaviors<AttackModel>().First(a=>a.name.Contains("Spawner")).Duplicate();
                //var DropPodSpawner=Game.instance.model.GetTowerFromId("EngineerMonkey-100").GetBehaviors<AttackModel>().First(a=>a.name.Contains("Spawner")).Duplicate();
                DropPodAbility.name="ZergDropPod";
                DropPodAbility.cooldown=1;
                DropPodAbility.GetBehavior<ActivateAttackModel>().cancelIfNoTargets=false;
                DropPodAbility.GetBehavior<ActivateAttackModel>().lifespan=10;
                DropPodAttack.targetProvider=new RandomPositionBasicModel("RandomPositionBasicModel",15,100,10,false);
                //DropPodAttack.weapons[0].projectile.display=null;
                DropPodAttack.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower.display="QueenDropPodStartPrefab";
                //DropPodAttack.weapons[0].projectile.AddBehavior(DropPodSpawner);
                DropPodAbility.GetBehavior<ActivateAttackModel>().attacks[0]=DropPodAttack;
                Queen.AddBehavior(DropPodAbility);
                Queen.display="QueenBroodMotherPrefab";
                //Queen.GetBehavior<DisplayModel>().positionOffset=new(0,0,-40);
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
                Queen.display="QueenZagaraPrefab";
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Contains("Queen")){
                    var udn=GetQueen(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-Queen";
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
        public static UnityDisplayNode GetQueen(Transform transform,string model){
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
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public record ResourceLoader_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.Contains("Queen")){
                    var text=Assets.LoadAsset(reference.guidRef).Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),nameof(Weapon.SpawnDart))]
        public static class SpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.towerModel.name.Contains("Queen")){
                    if(__instance.attack.attackModel.name.Contains("CreepTumor")){
                        __instance.attack.tower.Node.graphic.GetComponentInParent<AudioSource>().PlayOneShot(Assets.LoadAsset("QueenSpawnCreepTumorClip").Cast<AudioClip>(),Ext.ModVolume);
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
                    __instance.tower.Node.graphic.GetComponentInParent<AudioSource>().PlayOneShot(Assets.LoadAsset("QueenAbilityClip").Cast<AudioClip>(),Ext.ModVolume);
                }
            }
        }
    }
}