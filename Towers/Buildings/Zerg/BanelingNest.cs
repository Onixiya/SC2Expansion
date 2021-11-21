namespace SC2Expansion.Towers{
    public class BanelingNest:ModTower<ZergSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.banelingnest);
        public override string BaseTower=>"WizardMonkey-005";
        public override int Cost=>400;
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
            BanelingNest.radius=20;
            BanelingNest.range=15;
            BanelingNest.RemoveBehavior(BanelingNest.GetBehaviors<AttackModel>().First(a=>a.name.Contains("Shimmer")));
            BanelingNest.RemoveBehavior(BanelingNest.GetBehaviors<AttackModel>().First(a=>a.name.Equals("AttackModel_Attack_")));
            BanelingNest.RemoveBehavior<PrinceOfDarknessZombieBuffModel>();
            var SpawnBaneling=BanelingNest.GetAttackModel();
            SpawnBaneling.weapons[1].projectile.display="BanelingNestBanelingPrefab";
            SpawnBaneling.weapons[0].emission.Cast<NecromancerEmissionModel>().maxRbeSpawnedPerSecond=0;
            SpawnBaneling.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().minPiercePerBloon=1;
            SpawnBaneling.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=99999;
            SpawnBaneling.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection=false;
            SpawnBaneling.weapons[1].projectile.GetDamageModel().damage=3;
            SpawnBaneling.weapons[1].projectile.radius=4;
            SpawnBaneling.weapons[1].projectile.pierce=1;
            SpawnBaneling.weapons[1].rate=17000;
            SpawnBaneling.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().alternateProjectile=SpawnBaneling.weapons[1].projectile;
            SpawnBaneling.range=BanelingNest.range;
            BanelingNest.GetBehavior<NecromancerZoneModel>().attackUsedForRangeModel.range=999;
            BanelingNest.GetBehavior<DisplayModel>().display=BanelingNest.display;
        }
        public class CentrifugalHooks:ModUpgrade<BanelingNest> {
            public override string Name=>"CentrifugalHooks";
            public override string DisplayName=>"Centrifugal Hooks";
            public override string Description=>"Evolving powerful hooks allows Banelings to roll and move faster";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel BanelingNest){
                GetUpgradeModel().icon=new("BanelingNestCentrifugalHooksIcon");
                var SpawnBanelings=BanelingNest.GetAttackModel();
                SpawnBanelings.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=0.7f;
                SpawnBanelings.weapons[0].projectile.display="BanelingNestBanelingRollPrefab";
            }
        }
        public class Rupture:ModUpgrade<BanelingNest> {
            public override string Name=>"Rupture";
            public override string DisplayName=>"Rupture";
            public override string Description=>"Adding more volatile chemicals and more internel pressure increases damage and radius";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel BanelingNest) {
                GetUpgradeModel().icon=new("BanelingNestRuptureIcon");
                var SpawnBanelings=BanelingNest.GetAttackModel();
                SpawnBanelings.weapons[0].projectile.GetDamageModel().damage=6;
                SpawnBanelings.weapons[0].projectile.radius=7;
            }
        }
        public class CorrosiveAcid:ModUpgrade<BanelingNest> {
            public override string Name=>"CorrosiveAcid";
            public override string DisplayName=>"Corrosive Acid";
            public override string Description=>"Increasing the water content of the acid allows a small amount to be left on the track that strips camo and fortification on non Moabs";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel BanelingNest) {
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
        public class GrowthEnzymes:ModUpgrade<BanelingNest> {
            public override string Name=>"GrowthEnzymes";
            public override string DisplayName=>"Growth Enzymes";
            public override string Description=>"Adding more nutrients when Banelings are evolving lets them grow faster and bigger";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel BanelingNest){
                GetUpgradeModel().icon=new("BanelingNestRateIncreaseIcon");
                var SpawnBanelings=BanelingNest.GetAttackModel();
                SpawnBanelings.weapons[0].rate=11500f;
                SpawnBanelings.weapons[0].projectile.GetDamageModel().damage=9;
            }
        }
        public class Kaboomer:ModUpgrade<BanelingNest>{
            public override string Name=>"Kaboomer";
            public override string DisplayName=>"Kaboomer";
            public override string Description=>"Kaboomers are a heavy strain of Banelings that do massive damage";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel BanelingNest){
                GetUpgradeModel().icon=new("BanelingNestKaboomerIcon");
                var SpawnBanelings=BanelingNest.GetAttackModel();
                SpawnBanelings.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=0.5f;
                SpawnBanelings.weapons[1].rate=32500f;
                SpawnBanelings.weapons[1].projectile.GetDamageModel().damage=100;
                SpawnBanelings.weapons[1].projectile.display="BanelingNestKaboomerPrefab";
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!DisplayDict.ContainsKey(objectId)&&objectId.Contains("BanelingNest")){
                    var udn=uObject.Instantiate(TowerAssets.LoadAsset(objectId).Cast<GameObject>(),__instance.PrototypeRoot).AddComponent<UnityDisplayNode>();
                    udn.transform.position=new(-3000,0);
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
        public class ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref uImage image){
                if(reference!=null&&reference.guidRef.Contains("BanelingNest")){
                    var text=TowerAssets.LoadAsset(reference.guidRef).Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
    }
}