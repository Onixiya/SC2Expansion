namespace SC2Expansion.Towers{
    public class PhotonCannon:ModTower<ProtossSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.photoncannon);
        public override string BaseTower=>"WizardMonkey";
        public override int Cost=>600;
        public override int TopPathUpgrades=>4;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ProtossEnabled;
        public override string Description=>"Primary Protoss defensive structure, requires power from a nearby Pylon";
        public override void ModifyBaseTowerModel(TowerModel PhotonCannon){
            PhotonCannon.display="PhotonCannonBasePrefab";
            PhotonCannon.portrait=new("PhotonCannonPortrait");
            PhotonCannon.icon=new("PhotonCannonIcon");
            PhotonCannon.emoteSpriteLarge=new("Protoss");
            PhotonCannon.range=50;
            PhotonCannon.GetAttackModel().AddBehavior(PhotonCannon.GetBehavior<DisplayModel>().Duplicate());
            PhotonCannon.GetAttackModel().GetBehavior<DisplayModel>().display="PhotonCannonCannonPrefab";
            PhotonCannon.GetBehavior<DisplayModel>().display=PhotonCannon.display;
            PhotonCannon.RemoveBehavior<RotateToTargetModel>();
        }
        public class EnhancedTargeting:ModUpgrade<PhotonCannon>{
            public override string DisplayName=>"Enhanced Targeting";
            public override string Description=>"Better targeting AI allows for a longer attack range";
            public override int Cost=>675;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel PhotonCannon){
                GetUpgradeModel().icon=new("PhotonCannonIcon");
                PhotonCannon.range+=10;
                PhotonCannon.GetAttackModel().range=PhotonCannon.range;
            }
        }
        public class OptimizedOrdnance:ModUpgrade<PhotonCannon>{
            public override string DisplayName=>"Optimized Ordnance";
            public override string Description=>"";
            public override int Cost=>925;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel PhotonCannon){
                GetUpgradeModel().icon=new("PhotonCannonIcon");
            }
        }
        public class KhaydarinMonolith:ModUpgrade<PhotonCannon>{
            public override string DisplayName=>"Khaydarin Monolith";
            public override string Description=>"Nerazim defensive structure, long range and extremely powerful attack but slow to fire";
            public override int Cost=>1350;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel PhotonCannon){
                GetUpgradeModel().icon=new("PhotonCannonIcon");
            }
        }
        public class TesseractMonolith:ModUpgrade<PhotonCannon>{
            public override string DisplayName=>"Tesseract Monolith";
            public override string Description=>"Khaydarin Monolith heavily modified with Xel'Naga technology, attacks now stun and slow targets";
            public override int Cost=>7650;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel PhotonCannon){
                GetUpgradeModel().icon=new("PhotonCannonIcon");
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