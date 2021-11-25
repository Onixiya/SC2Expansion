namespace SC2Expansion.Towers{
    public class PhotonCannon:ModTower<ProtossSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.PhotonCannon);
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>500;
        public override int TopPathUpgrades=>4;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ProtossEnabled;
        public override string Description=>"Primary Protoss defensive structure, requires power from a nearby Pylon";
        public override void ModifyBaseTowerModel(TowerModel PhotonCannon){
            PhotonCannon.display="PhotonCannonPrefab";
            PhotonCannon.portrait=new("PhotonCannonPortrait");
            PhotonCannon.icon=new("PhotonCannonIcon");
            PhotonCannon.emoteSpriteLarge=new("Protoss");
            PhotonCannon.range=50;
            PhotonCannon.RemoveBehavior<AttackModel>();
            PhotonCannon.AddBehavior(Game.instance.model.GetTowerFromId("SniperMonkey-050").GetBehavior<RateSupportModel>().Duplicate());
            PhotonCannon.GetBehavior<RateSupportModel>().multiplier=0.0001f;
            PhotonCannon.GetBehavior<RateSupportModel>().isGlobal=false;
            PhotonCannon.GetBehavior<RateSupportModel>().showBuffIcon=false;
            PhotonCannon.GetBehavior<RateSupportModel>().filters[0].Cast<FilterInBaseTowerIdModel>().baseIds=new[]{"SC2Expansion-Gateway"};
            PhotonCannon.GetBehavior<RateSupportModel>().name="RateSupportBuildings";
            PhotonCannon.GetBehavior<DisplayModel>().display=PhotonCannon.display;
            PhotonCannon.RemoveBehavior<RotateToTargetModel>();
        }
        public class EnhancedTargeting:ModUpgrade<PhotonCannon>{
            public override string DisplayName=>"Refined Crystal";
            public override string Description=>"Using a purer khaydarin crystal allows for a larger area to be powered";
            public override int Cost=>675;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel PhotonCannon){
                GetUpgradeModel().icon=new("PhotonCannonIcon");
                PhotonCannon.range+=10;
            }
        }
        public class Stabilizers:ModUpgrade<PhotonCannon>{
            
            public override string DisplayName=>"Stabilizers";
            public override string Description=>"Stablizing the power output boosts the speeds of nearby Protoss structures";
            public override int Cost=>925;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel PhotonCannon){
                GetUpgradeModel().icon=new("PhotonCannonStableIcon");
                PhotonCannon.portrait=new("PhotonCannonStablePortrait");
                PhotonCannon.display="PhotonCannonStablePrefab";
                PhotonCannon.GetBehavior<RateSupportModel>().multiplier=0.00008f;
            }
        }
        public class Overcharged:ModUpgrade<PhotonCannon>{
            
            public override string DisplayName=>"Overcharged";
            public override string Description=>"Lacing small amounts of bloodshard crystals when forming allows for a fast attack";
            public override int Cost=>1350;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel PhotonCannon){
                GetUpgradeModel().icon=new("PhotonCannonChargedIcon");
                PhotonCannon.portrait=new("PhotonCannonChargedPortrait");
                PhotonCannon.display="PhotonCannonChargedPrefab";
                PhotonCannon.AddBehavior(Game.instance.model.GetTowerFromId("WizardMonkey-100").GetAttackModel());
                PhotonCannon.GetAttackModel().range=PhotonCannon.range;
                PhotonCannon.GetAttackModel().weapons[0].rate=0.5f;
                PhotonCannon.GetAttackModel().weapons[0].projectile.pierce=1;
                PhotonCannon.GetAttackModel().weapons[0].projectile.GetDamageModel().damage+=1;
                PhotonCannon.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().speed*=1.6f;
                PhotonCannon.GetAttackModel().GetBehavior<RotateToTargetModel>().rotateTower=false;
            }
        }
        public class VoidPhotonCannon:ModUpgrade<PhotonCannon>{
            
            public override string DisplayName=>"Dark PhotonCannon";
            public override string Description=>"Nerazim PhotonCannons are capable of powering and cloaking large areas. Cloaked towers get boosts to pierce and attack speed";
            public override int Cost=>7650;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel PhotonCannon){
                GetUpgradeModel().icon=new("PhotonCannonVoidIcon");
                PhotonCannon.portrait=new("PhotonCannonVoidPortrait");
                PhotonCannon.display="PhotonCannonVoidPrefab";
                PhotonCannon.range+=20;
                PhotonCannon.GetAttackModel().range=PhotonCannon.range;
                RateSupportModel CloakField=Game.instance.model.GetTowerFromId("SniperMonkey-050").GetBehavior<RateSupportModel>();
                CloakField.multiplier=1.5f;
                CloakField.name="CloakField";
                CloakField.showBuffIcon=false;
                CloakField.filters.First().Cast<FilterInBaseTowerIdModel>().baseIds=new[]{"SC2Expansion-HighTemplar","SC2Expansion-VoidRay","SC2Expansion-Archon"};
                PhotonCannon.AddBehavior(CloakField);
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!DisplayDict.ContainsKey(objectId)&&objectId.Contains("PhotonCannon")){
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
                if(reference!=null&&reference.guidRef.StartsWith("PhotonCannon")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
    }
}