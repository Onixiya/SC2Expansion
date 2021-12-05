namespace SC2Expansion.Towers{
    public class BanelingNest:ModTower<ZergSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.banelingnest);
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>575;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ZergEnabled;
        public override string Description=>"Spawns Banelings. Small zerg, very explosive and suicidal. Do not poke";
        public override void ModifyBaseTowerModel(TowerModel BanelingNest){
            BanelingNest.display="BanelingNestPrefab";
            BanelingNest.portrait=new("BanelingNestIcon");
            BanelingNest.icon=new("BanelingNestIcon");
            BanelingNest.emoteSpriteLarge=new("Zerg");
            BanelingNest.radius=12;
            BanelingNest.range=15;
            BanelingNest.RemoveBehavior(BanelingNest.GetAttackModel());
            BanelingNest.AddBehavior(Game.instance.model.GetTowerFromId("WizardMonkey-004").GetBehavior<NecromancerZoneModel>().Duplicate());
            BanelingNest.AddBehavior(Game.instance.model.GetTowerFromId("WizardMonkey-004").GetBehaviors<AttackModel>().First(a=>a.name=="AttackModel_Attack Necromancer_").Duplicate());
            var SpawnBaneling=BanelingNest.GetAttackModel();
            SpawnBaneling.weapons[0].projectile.display="BanelingNestBanelingPrefab";
            SpawnBaneling.weapons[0].emission.Cast<NecromancerEmissionModel>().maxPiercePerBloon=1;
            SpawnBaneling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=99999;
            SpawnBaneling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection=false;
            SpawnBaneling.weapons[0].projectile.GetDamageModel().damage=3;
            SpawnBaneling.weapons[0].projectile.radius=4;
            SpawnBaneling.weapons[0].projectile.pierce=1;
            SpawnBaneling.weapons[0].rate=17000;
            SpawnBaneling.range=999;
            BanelingNest.GetBehavior<NecromancerZoneModel>().attackUsedForRangeModel.range=999;
            BanelingNest.GetBehavior<DisplayModel>().display=BanelingNest.display;
        }
        public class CentrifugalHooks:ModUpgrade<BanelingNest>{
            public override string DisplayName=>"Centrifugal Hooks";
            public override string Description=>"Evolving powerful hooks allows Banelings to roll and move faster";
            public override int Cost=>650;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel BanelingNest){
                GetUpgradeModel().icon=new("BanelingNestCentrifugalHooksIcon");
                var SpawnBanelings=BanelingNest.GetAttackModel();
                SpawnBanelings.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=0.7f;
                SpawnBanelings.weapons[0].projectile.display="BanelingNestBanelingRollPrefab";
            }
        }
        public class Rupture:ModUpgrade<BanelingNest>{
            public override string DisplayName=>"Rupture";
            public override string Description=>"Adding more volatile chemicals and more internel pressure increases damage and radius";
            public override int Cost=>925;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel BanelingNest){
                GetUpgradeModel().icon=new("BanelingNestRuptureIcon");
                var SpawnBanelings=BanelingNest.GetAttackModel();
                SpawnBanelings.weapons[0].projectile.GetDamageModel().damage=6;
                SpawnBanelings.weapons[0].projectile.radius=7;
            }
        }
        public class CorrosiveAcid:ModUpgrade<BanelingNest>{
            public override string DisplayName=>"Corrosive Acid";
            public override string Description=>"Increasing the water content of the acid allows a small amount to be left on the track that strips camo and fortification on non Moabs";
            public override int Cost=>1355;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel BanelingNest){
                GetUpgradeModel().icon=new("BanelingNestCorrosiveAcidIcon");
                var SpawnBanelings=BanelingNest.GetAttackModel();
                SpawnBanelings.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("EngineerMonkey-030").behaviors.First(a=>a.name.Contains("CleansingFoam")).
                    Cast<AttackModel>().weapons[0].projectile.behaviors.First(a=>a.name.Contains("Exhaust")).Duplicate());
                SpawnBanelings.weapons[0].projectile.display="BanelingNestBaneling3Prefab";
                var AcidPool=SpawnBanelings.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>();
                var AcidPoolModifiers=AcidPool.projectile.GetBehavior<RemoveBloonModifiersModel>();
                AcidPoolModifiers.cleanseFortified=true;
                AcidPoolModifiers.cleanseLead=false;
                AcidPoolModifiers.bloonTagExcludeList.Add("Moabs");
            }
        }
        public class GrowthEnzymes:ModUpgrade<BanelingNest>{
            public override string DisplayName=>"Growth Enzymes";
            public override string Description=>"Adding more nutrients when Banelings are evolving lets them grow faster and bigger";
            public override int Cost=>4590;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel BanelingNest){
                GetUpgradeModel().icon=new("BanelingNestRateIncreaseIcon");
                var SpawnBanelings=BanelingNest.GetAttackModel();
                SpawnBanelings.weapons[0].rate=11500;
                SpawnBanelings.weapons[0].projectile.GetDamageModel().damage=9;
            }
        }
        public class Kaboomer:ModUpgrade<BanelingNest>{
            public override string DisplayName=>"Kaboomer";
            public override string Description=>"Kaboomers are a heavy strain of Banelings that do massive damage";
            public override int Cost=>11145;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel BanelingNest){
                GetUpgradeModel().icon=new("BanelingNestKaboomerIcon");
                var SpawnBanelings=BanelingNest.GetAttackModel();
                SpawnBanelings.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=0.5f;
                SpawnBanelings.weapons[0].rate=32500;
                SpawnBanelings.weapons[0].projectile.GetDamageModel().damage=100;
                SpawnBanelings.weapons[0].projectile.display="BanelingNestKaboomerPrefab";
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                return LoadModel(TowerAssets,objectId,"BanelingNest",__instance,onComplete);
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public class ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.StartsWith("BanelingNest")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
    }
}