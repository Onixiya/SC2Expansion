namespace SC2Expansion.Towers{
    public class Pylon:ModTower<ProtossSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.pylon);
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>500;
        public override int TopPathUpgrades=>4;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ProtossEnabled;
        public override string Description=>"Generates a psionic field that powers Protoss structures";
        public override void ModifyBaseTowerModel(TowerModel Pylon){
            Pylon.display="PylonPrefab";
            Pylon.portrait=new("PylonPortrait");
            Pylon.icon=new("PylonIcon");
            Pylon.emoteSpriteLarge=new("Protoss");
            Pylon.range=50;
            Pylon.RemoveBehavior<AttackModel>();
            Pylon.AddBehavior(Game.instance.model.GetTowerFromId("SniperMonkey-050").GetBehavior<RateSupportModel>().Duplicate());
            Pylon.GetBehavior<RateSupportModel>().multiplier=0.0001f;
            Pylon.GetBehavior<RateSupportModel>().isGlobal=false;
            Pylon.GetBehavior<RateSupportModel>().showBuffIcon=false;
            Pylon.GetBehavior<RateSupportModel>().filters[0].Cast<FilterInBaseTowerIdModel>().baseIds=new[]{"SC2Expansion-Gateway"};
            Pylon.GetBehavior<RateSupportModel>().name="RateSupportBuildings";
            Pylon.GetBehavior<DisplayModel>().display=Pylon.display;
            Pylon.RemoveBehavior<RotateToTargetModel>();
        }
        public class RefinedCrystal:ModUpgrade<Pylon> {
            public override string Name=>"RefinedCrystal";
            public override string DisplayName=>"Refined Crystal";
            public override string Description=>"Using a purer khaydarin crystal allows for a larger area to be powered";
            public override int Cost=>675;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Pylon){
                GetUpgradeModel().icon=new("PylonIcon");
                Pylon.range+=10;
            }
        }
        public class Stabilizers:ModUpgrade<Pylon> {
            public override string Name=>"Stabilizers";
            public override string DisplayName=>"Stabilizers";
            public override string Description=>"Stablizing the power output boosts the speeds of nearby Protoss structures";
            public override int Cost=>925;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Pylon) {
                GetUpgradeModel().icon=new("PylonStableIcon");
                Pylon.portrait=new("PylonStablePortrait");
                Pylon.display="PylonStablePrefab";
                Pylon.GetBehavior<RateSupportModel>().multiplier=0.00008f;
            }
        }
        public class Overcharged:ModUpgrade<Pylon> {
            public override string Name=>"Overcharged";
            public override string DisplayName=>"Overcharged";
            public override string Description=>"Lacing small amounts of bloodshard crystals when forming allows for a fast attack";
            public override int Cost=>1350;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Pylon){
                GetUpgradeModel().icon=new("PylonChargedIcon");
                Pylon.portrait=new("PylonChargedPortrait");
                Pylon.display="PylonChargedPrefab";
                Pylon.AddBehavior(Game.instance.model.GetTowerFromId("WizardMonkey-100").GetAttackModel());
                Pylon.GetAttackModel().range=Pylon.range;
                Pylon.GetAttackModel().weapons[0].rate=0.5f;
                Pylon.GetAttackModel().weapons[0].projectile.pierce=1;
                Pylon.GetAttackModel().weapons[0].projectile.GetDamageModel().damage+=1;
                Pylon.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().speed*=1.5f;
                Pylon.GetAttackModel().GetBehavior<RotateToTargetModel>().rotateTower=false;
            }
        }
        public class VoidPylon:ModUpgrade<Pylon> {
            public override string Name=>"VoidPylon";
            public override string DisplayName=>"Dark Pylon";
            public override string Description=>"Nerazim Pylons are capable of powering and cloaking large areas. Cloaked towers get boosts to pierce and attack speed";
            public override int Cost=>7650;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Pylon) {
                GetUpgradeModel().icon=new("PylonVoidIcon");
                Pylon.portrait=new("PylonVoidPortrait");
                Pylon.display="PylonVoidPrefab";
                Pylon.range+=20;
                Pylon.GetAttackModel().range=Pylon.range;
                RateSupportModel CloakField=Game.instance.model.GetTowerFromId("SniperMonkey-050").GetBehavior<RateSupportModel>();
                CloakField.multiplier=1.5f;
                CloakField.name="CloakField";
                CloakField.showBuffIcon=false;
                CloakField.filters.First().Cast<FilterInBaseTowerIdModel>().baseIds=new[]{"SC2Expansion-HighTemplar","SC2Expansion-VoidRay","SC2Expansion-Archon"};
                Pylon.AddBehavior(CloakField);
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!DisplayDict.ContainsKey(objectId)&&objectId.Contains("Pylon")){
                    LoadModel(TowerAssets,objectId,__instance,onComplete);
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
                if(reference!=null&&reference.guidRef.StartsWith("Pylon")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
    }
}