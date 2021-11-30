using Assets.Scripts.Simulation.Behaviors;

namespace SC2Expansion.Towers{
    public class PhotonCannon:ModTower<ProtossSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.photoncannon);
        public override string BaseTower=>"WizardMonkey";
        public override int Cost=>650;
        public override int TopPathUpgrades=>4;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ProtossEnabled;
        public override string Description=>"Primary Protoss defensive structure, requires power from a nearby Pylon";
        public override void ModifyBaseTowerModel(TowerModel PhotonCannon){
            var Cannon=PhotonCannon.GetAttackModel();
            PhotonCannon.display="PhotonCannonBasePrefab";
            PhotonCannon.portrait=new("PhotonCannonPortrait");
            PhotonCannon.icon=new("PhotonCannonIcon");
            PhotonCannon.emoteSpriteLarge=new("Protoss");
            PhotonCannon.range=50;
            Cannon.AddBehavior(PhotonCannon.GetBehavior<DisplayModel>().Duplicate());
            Cannon.GetBehavior<DisplayModel>().display="PhotonCannonCannonPrefab";
            Cannon.range=PhotonCannon.range;
            Cannon.weapons[0].rate=1;
            Cannon.weapons[0].projectile.GetDamageModel().damage+=1;
            Cannon.weapons[0].projectile.pierce=1;
            Cannon.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed*=2;
            PhotonCannon.GetBehavior<DisplayModel>().display=PhotonCannon.display;
            PhotonCannon.GetBehavior<DisplayModel>().ignoreRotation=true;
        }
        public class EnhancedTargeting:ModUpgrade<PhotonCannon>{
            public override string DisplayName=>"Enhanced Targeting";
            public override string Description=>"Better targeting AI allows for a longer attack range";
            public override int Cost=>575;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel PhotonCannon){
                GetUpgradeModel().icon=new("PhotonCannonEnhancedTargetingIcon");
                PhotonCannon.range+=10;
                PhotonCannon.GetAttackModel().range=PhotonCannon.range;
            }
        }
        public class OptimizedOrdnance:ModUpgrade<PhotonCannon>{
            public override string DisplayName=>"Optimized Ordnance";
            public override string Description=>"Increasing the antimatter density even more before firing increases damage by a lot";
            public override int Cost=>1210;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel PhotonCannon){
                GetUpgradeModel().icon=new("PhotonCannonOptimizedOrdnanceIcon");
                PhotonCannon.GetAttackModel().weapons[0].projectile.GetDamageModel().damage+=5;
            }
        }
        public class KhaydarinMonolith:ModUpgrade<PhotonCannon>{
            public override string DisplayName=>"Khaydarin Monolith";
            public override string Description=>"Nerazim defensive structure, long range and powerful attack but slow to fire";
            public override int Cost=>3650;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel PhotonCannon){
                GetUpgradeModel().icon=new("PhotonCannonKhaydarinMonolithIcon");
                var Cannon=PhotonCannon.GetAttackModel();
                PhotonCannon.display="PhotonCannonKhaydarinMonolithBasePrefab";
                PhotonCannon.range+=50;
                Cannon.range=PhotonCannon.range;
                Cannon.GetBehavior<DisplayModel>().display="PhotonCannonKhaydarinMonolithCannonPrefab";
                Cannon.weapons[0]=Game.instance.model.GetTowerFromId("Druid-200").GetAttackModel().weapons[1].Duplicate();
                Cannon.weapons[0].projectile.GetDamageModel().damage+=20;
                Cannon.weapons[0].projectile.GetDamageModel().distributeToChildren=true;
                Cannon.weapons[0].projectile.GetBehavior<LightningModel>().splits=0;
                Cannon.weapons[0].rate=2;
                Cannon.weapons[0].ejectZ=10;
            }
        }
        public class TesseractMonolith:ModUpgrade<PhotonCannon>{
            public override string DisplayName=>"Tesseract Monolith";
            public override string Description=>"Khaydarin Monolith heavily modified with Xel'Naga technology, attacks now stun targets and small boost to damage";
            public override int Cost=>9450;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel PhotonCannon){
                GetUpgradeModel().icon=new("PhotonCannonTesseractMonolithIcon");
                var Cannon=PhotonCannon.GetAttackModel();
                var Stun=Game.instance.model.GetTowerFromId("GlueGunner").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
                PhotonCannon.display="PhotonCannonTesseractMonolithBasePrefab";
                Cannon.GetBehavior<DisplayModel>().display="PhotonCannonTesseractMonolithCannonPrefab";
                Cannon.weapons[0].projectile.GetDamageModel().damage+=5;
                Stun.Lifespan=2.5f;
                Stun.Mutator.multiplier=0;
                Stun.name="SlowModel_Stun";
                Stun.Mutator.mutationId="MonolithStun";
                foreach(var temp in Stun.Mutator.overlays){
                    temp.Value.assetPath=null;
                }
                Cannon.weapons[0].projectile.AddBehavior(Stun);
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
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.StartsWith("PhotonCannon")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public class WeaponSpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.namedMonkeyKey.Contains("PhotonCannon")){
                    __instance.attack.entity.GetDisplayNode().graphic.GetComponent<Animator>().Play("PhotonCannonAttack");
                }
            }
        }
    }
}