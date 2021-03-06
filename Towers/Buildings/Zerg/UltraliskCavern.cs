namespace SC2Expansion.Towers{
    public class UltraliskCavern:ModTower<ZergSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.ultraliskcavern);
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>1560;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ZergEnabled;
        public override string Description=>"Spawns Ultralisks, basically living tanks and very hard to kill";
        public override void ModifyBaseTowerModel(TowerModel UltraliskCavern){
            UltraliskCavern.display="UltraliskCavernPrefab";
            UltraliskCavern.portrait=new("UltraliskCavernPortrait");
            UltraliskCavern.icon=new("UltraliskCavernIcon");
            UltraliskCavern.emoteSpriteLarge=new("Zerg");
            UltraliskCavern.radius=15;
            UltraliskCavern.range=15;
            UltraliskCavern.RemoveBehavior(UltraliskCavern.GetAttackModel());
            UltraliskCavern.AddBehavior(Game.instance.model.GetTowerFromId("WizardMonkey-004").GetBehavior<NecromancerZoneModel>().Duplicate());
            UltraliskCavern.AddBehavior(Game.instance.model.GetTowerFromId("WizardMonkey-004").GetBehaviors<AttackModel>().First(a=>a.name=="AttackModel_Attack Necromancer_").Duplicate());
            var SpawnUltralisk=UltraliskCavern.GetAttackModel();
            SpawnUltralisk.weapons[0].projectile.display="UltraliskCavernUltraliskPrefab";
            SpawnUltralisk.weapons[0].emission.Cast<NecromancerEmissionModel>().maxPiercePerBloon=70;
            SpawnUltralisk.weapons[0].emission.Cast<NecromancerEmissionModel>().maxRbeSpawnedPerSecond=1;
            SpawnUltralisk.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=99999;
            SpawnUltralisk.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=0.45f;
            SpawnUltralisk.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection=false;
            SpawnUltralisk.weapons[0].projectile.GetDamageModel().damage=6;
            SpawnUltralisk.weapons[0].projectile.radius=7;
            SpawnUltralisk.name="SpawnUltralisk";
            SpawnUltralisk.weapons[0].projectile.pierce=20;
            SpawnUltralisk.weapons[0].rate=50000;
            SpawnUltralisk.range=999;
            UltraliskCavern.GetBehavior<NecromancerZoneModel>().attackUsedForRangeModel.range=999;
            UltraliskCavern.GetBehavior<DisplayModel>().display=UltraliskCavern.display;
        }
        public class ChitinousPlating:ModUpgrade<UltraliskCavern>{
            public override string DisplayName=>"Chitinous Plating";
            public override string Description=>"Reinforcing Ultralisks weakpoints with more armour increases the amount of hits they can take";
            public override int Cost=>1300;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel UltraliskCavern){
                GetUpgradeModel().icon=new("UltraliskCavernChitinousPlatingIcon");
                var SpawnUltralisk=UltraliskCavern.GetAttackModel();
                SpawnUltralisk.weapons[0].emission.Cast<NecromancerEmissionModel>().maxPiercePerBloon=125;
                SpawnUltralisk.weapons[0].projectile.pierce=30;
            }
        }
        public class AnabolicSynthesis:ModUpgrade<UltraliskCavern>{
            public override string DisplayName=>"Anabolic Synthesis";
            public override string Description=>"Synthesising even more adrenaline makes Ultralisks move a lot faster";
            public override int Cost=>1785;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel UltraliskCavern){
                GetUpgradeModel().icon=new("UltraliskCavernAnabolicSynthesisIcon");
                UltraliskCavern.GetAttackModel().weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=0.6f;
            }
        }
        public class Noxious:ModUpgrade<UltraliskCavern>{
            public override string DisplayName=>"Noxious Strain";
            public override string Description=>"Evolving Ultralisks into the Noxious strain leaves damaging gas clouds on death";
            public override int Cost=>2560;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel UltraliskCavern){
                GetUpgradeModel().icon=new("UltraliskCavernNoxiousIcon");
                var SpawnUltralisk=UltraliskCavern.GetAttackModel();
                SpawnUltralisk.weapons[0].projectile.display="UltraliskCavernNoxiousPrefab";
                var GasCloud=Game.instance.model.GetTowerFromId("EngineerMonkey-030").GetBehaviors<AttackModel>().First(a=>a.name.Contains("CleansingFoam")).weapons[0].projectile.
                    GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                GasCloud.projectile.RemoveBehavior<RemoveBloonModifiersModel>();
                GasCloud.projectile.display="GasPrefab";
                GasCloud.projectile.AddBehavior(new DamageModel("DamageModel",1,1,false,false,true,0));
                GasCloud.projectile.pierce=9999;
                GasCloud.projectile.GetBehavior<AgeModel>().lifespan=6;
                SpawnUltralisk.weapons[0].projectile.AddBehavior(GasCloud);
            }
        }
        public class UltraliskPrimal:ModUpgrade<UltraliskCavern>{
            public override string DisplayName=>"Evolve into Tyrannozor";
            public override string Description=>"Evolves into a Tyrannozor, gaining a upgrade to either health, damage or speed and swaps gas cloud for Barrage of Spikes attack";
            public override int Cost=>5780;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel UltraliskCavern){
                GetUpgradeModel().icon=new("UltraliskCavernPrimalIcon");
                var SpawnUltralisk=UltraliskCavern.GetAttackModel();
                SpawnUltralisk.weapons[0].projectile.display="UltraliskCavernPrimalPrefab";
                SpawnUltralisk.weapons[0].projectile.RemoveBehavior<CreateProjectileOnExhaustFractionModel>();
                SpawnUltralisk.weapons[0].projectile.AddBehavior(new CreateProjectileOnIntervalModel("CreateProjectileOnIntervalModel",Game.instance.model.GetTowerFromId("DartMonkey").
                    GetAttackModel().weapons[0].projectile.Duplicate(),Game.instance.model.GetTowerFromId("MonkeyAce-003").GetAttackModel().weapons[0].emission.Duplicate(),60,true,30,null));
                SpawnUltralisk.weapons[0].projectile.GetBehavior<CreateProjectileOnIntervalModel>().emission.Cast<ArcEmissionModel>().Count=12;
                SpawnUltralisk.weapons[0].rate=55000f;
            }
        }
        public class Apocalisk:ModUpgrade<UltraliskCavern>{
            public override string DisplayName=>"Apocalisk";
            public override string Description=>"\"I call this the Apocalisk! See it in action!\"";
            public override int Cost=>12560;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel UltraliskCavern){
                GetUpgradeModel().icon=new("UltraliskCavernApocaliskIcon");
                var SpawnUltralisk=UltraliskCavern.GetAttackModel();
                SpawnUltralisk.weapons[0].projectile.display="UltraliskCavernApocaliskPrefab";
                SpawnUltralisk.weapons[0].projectile.pierce+=25;
                SpawnUltralisk.weapons[0].projectile.GetDamageModel().damage+=6;
                SpawnUltralisk.weapons[0].rate=100000;
                var ClusterRockets=SpawnUltralisk.weapons[0].projectile.GetBehavior<CreateProjectileOnIntervalModel>();
                ClusterRockets.projectile=Game.instance.model.GetTowerFromId("BombShooter-020").GetAttackModel().weapons[0].projectile.Duplicate();
                ClusterRockets.projectile.RemoveBehavior<TravelStraitModel>();
                ClusterRockets.projectile.AddBehavior(Game.instance.model.GetTowerFromId("DartlingGunner-050").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.
                    GetBehavior<TravelCurvyModel>().Duplicate());
                ClusterRockets.projectile.GetBehavior<TravelCurvyModel>().speed=150;
                ClusterRockets.projectile.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyAce-003").GetAttackModel().weapons[0].projectile.GetBehavior<TrackTargetModel>().Duplicate());
            }
        }
        [HarmonyPatch(typeof(TowerManager),"UpgradeTower")]
        public static class TowerManagerUpgradeTower_Patch{
            [HarmonyPostfix]
            public static void Postfix(Tower tower,TowerModel def,string __state){
                if(__state!=null&&__state.Contains("Primal")&&tower.namedMonkeyKey.Contains("Ultralisk")){
                    int RandNum=new System.Random().Next(1,3);
                    switch(RandNum){
                        case 1:
                            def.GetAttackModel().weapons[1].projectile.pierce+=5;
                            break;
                        case 2:
                            def.GetAttackModel().weapons[1].projectile.GetBehavior<TravelAlongPathModel>().speedFrames+=0.2f;
                            break;
                        case 3:
                            def.GetAttackModel().weapons[1].projectile.GetDamageModel().damage+=3;
                            break;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                return LoadModel(TowerAssets,objectId,"UltraliskCavern",__instance,onComplete);
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public class ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.StartsWith("UltraliskCavern")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
    }
}