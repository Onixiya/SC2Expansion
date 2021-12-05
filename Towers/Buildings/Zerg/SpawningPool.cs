namespace SC2Expansion.Towers{
    public class SpawningPool:ModTower<ZergSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.spawningpool);
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>620;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ZergEnabled;
        public override string Description=>"Spawns Zerglings, small melee Zerg, very short life expentancy";
        public override void ModifyBaseTowerModel(TowerModel SpawningPool){
            SpawningPool.display="SpawningPoolPrefab";
            SpawningPool.portrait=new("SpawningPoolPortrait");
            SpawningPool.icon=new("SpawningPoolIcon");
            SpawningPool.emoteSpriteLarge=new("Zerg");
            SpawningPool.radius=15;
            SpawningPool.range=15;
            SpawningPool.RemoveBehavior(SpawningPool.GetAttackModel());
            SpawningPool.AddBehavior(Game.instance.model.GetTowerFromId("WizardMonkey-004").GetBehavior<NecromancerZoneModel>().Duplicate());
            SpawningPool.AddBehavior(Game.instance.model.GetTowerFromId("WizardMonkey-004").GetBehaviors<AttackModel>().First(a=>a.name=="AttackModel_Attack Necromancer_").Duplicate());
            var SpawnZergling=SpawningPool.GetAttackModel();
            SpawnZergling.weapons[0].projectile.display="SpawningPoolZerglingPrefab";
            SpawnZergling.weapons[0].emission.Cast<NecromancerEmissionModel>().maxPiercePerBloon=20;
            SpawnZergling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=99999;
            SpawnZergling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection=false;
            SpawnZergling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=0.35f;
            SpawnZergling.weapons[0].projectile.GetDamageModel().damage=1;
            SpawnZergling.weapons[0].projectile.radius=4;
            SpawnZergling.weapons[0].projectile.pierce=7;
            SpawnZergling.weapons[0].rate=15000;
            SpawnZergling.range=SpawningPool.range;
            SpawningPool.GetBehavior<NecromancerZoneModel>().attackUsedForRangeModel.range=999;
            SpawningPool.GetBehavior<DisplayModel>().display=SpawningPool.display;
        }
        public class HardendCarapace:ModUpgrade<SpawningPool>{
            public override string DisplayName=>"Hardend Carapace";
            public override string Description=>"Increasing the carapace density lets Zerglings take more hits before dying";
            public override int Cost=>530;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel SpawningPool){
                GetUpgradeModel().icon=new("SpawningPoolHardendCarapaceIcon");
                var SpawnZergling=SpawningPool.GetAttackModel();
                SpawnZergling.weapons[0].emission.Cast<NecromancerEmissionModel>().maxPiercePerBloon*=2;
                SpawnZergling.weapons[0].projectile.pierce=13;
            }
        }
        public class MetabolicBoost:ModUpgrade<SpawningPool>{
            public override string DisplayName=>"Metabolic Boost";
            public override string Description=>"Improving Zergling metabolism and evolving wings massively increases Zergling's speed";
            public override int Cost=>775;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel SpawningPool){
                GetUpgradeModel().icon=new("SpawningPoolMetabolicBoostIcon");
                var SpawnZergling=SpawningPool.GetAttackModel();
                SpawnZergling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=1;
                SpawnZergling.weapons[0].projectile.display="SpawningPoolZerglingWingPrefab";
            }
        }
        public class AdrenalGlands:ModUpgrade<SpawningPool>{
            public override string DisplayName=>"Adrenal Glands";
            public override string Description=>"Making even more adrenaline increases Zergling damage and speed slightly";
            public override int Cost=>1080;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel SpawningPool){
                GetUpgradeModel().icon=new("SpawningPoolAdrenalGlandsIcon");
                var SpawnZergling=SpawningPool.GetAttackModel();
                SpawnZergling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=1.2f;
                SpawnZergling.weapons[0].projectile.GetDamageModel().damage=3;
            }
        }
        public class Primal:ModUpgrade<SpawningPool>{
            public override string DisplayName=>"Primal Evolution";
            public override string Description=>"Gains a random upgrade to speed, damage or health";
            public override int Cost=>2985;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel SpawningPool){
                GetUpgradeModel().icon=new("SpawningPoolPrimalIcon");
                SpawningPool.GetAttackModel().weapons[0].projectile.display="SpawningPoolPrimalPrefab";
            }
        }
        public class Swarmling:ModUpgrade<SpawningPool>{
            public override string DisplayName=>"Swarmling strain";
            public override string Description=>"Swarmling strain spawns multiple Zerglings at a time";
            public override int Cost=>9670;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel SpawningPool){
                GetUpgradeModel().icon=new("SpawningPoolSwarmlingIcon");
                var SpawnZergling=SpawningPool.GetAttackModel();
                SpawnZergling.weapons[0].projectile.GetDamageModel().damage+=2;
                SpawnZergling.weapons[0].rate=3500f;
                SpawnZergling.weapons[0].projectile.display="SpawningPoolSwarmlingPrefab";
            }
        }
        [HarmonyPatch(typeof(TowerManager),"UpgradeTower")]
        public class TowerManagerUpgradeTower_Patch{
            [HarmonyPostfix]
            public static void Postfix(Tower tower,TowerModel def,string __state){
                if(__state!=null&&__state.Contains("Primal")&&tower.namedMonkeyKey.Contains("SpawningPool")){
                    int RandNum=new System.Random().Next(1,4);
                    switch(RandNum){
                        case 1:
                            def.GetAttackModel().weapons[0].projectile.pierce+=4;
                            break;
                        case 2:
                            def.GetAttackModel().weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames+=0.15f;
                            break;
                        case 3:
                            def.GetAttackModel().weapons[0].projectile.GetDamageModel().damage+=2;
                            break;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                return LoadModel(TowerAssets,objectId,"SpawningPool",__instance,onComplete);
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public class ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.StartsWith("SpawningPool")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
    }
}