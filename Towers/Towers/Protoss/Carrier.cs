namespace SC2Expansion.Towers{
    public class Carrier:ModTower<ProtossSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.carrier);
        public override string DisplayName=>"Carrier";
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>2455;
        public override int TopPathUpgrades=>2;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ProtossEnabled;
        public override string Description=>"Protoss capital ships, signature of the Golden Armada and the Protoss race in general. Attacks by sending out Inteceptors";
        public override void ModifyBaseTowerModel(TowerModel Carrier){
            var InteceptorSpawn=Game.instance.model.GetTowerFromId("MonkeyBuccaneer-400").GetBehaviors<AttackModel>().First(a=>a.name=="AttackModel_Plane Spawner_").Duplicate();
            var Inteceptor=InteceptorSpawn.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
            var InteceptorAttack=Inteceptor.GetBehaviors<AttackModel>().First(a=>a.name=="AttackModel_Attack_");
            Carrier.display="CarrierPrefab";
            Carrier.portrait=new("CarrierPortrait");
            Carrier.icon=new("CarrierIcon");
            Carrier.emoteSpriteLarge=new("Protoss");
            Carrier.radius=15;
            Carrier.range=30;
            Carrier.areaTypes=new(4);
            Carrier.areaTypes[0]=AreaType.land;
            Carrier.areaTypes[1]=AreaType.track;
            Carrier.areaTypes[2]=AreaType.ice;
            Carrier.areaTypes[3]=AreaType.water;
            Carrier.RemoveBehavior<AttackModel>();
            InteceptorSpawn.weapons[0].GetBehavior<SubTowerFilterModel>().maxNumberOfSubTowers=4;
            Inteceptor.GetBehavior<TowerExpireModel>().expireOnRoundComplete=true;
            Inteceptor.GetBehavior<AirUnitModel>().display="CarrierInteceptorPrefab";
            Inteceptor.GetBehavior<AirUnitModel>().behaviors.First().Cast<FighterMovementModel>().maxSpeed=60;
            Inteceptor.RemoveBehavior(Inteceptor.GetBehaviors<AttackModel>().First(a=>a.name.Contains("Anti-Moab")));
            Inteceptor.RemoveBehavior(Inteceptor.GetBehaviors<AttackModel>().First(a=>a.name.Contains("Radial")));
            InteceptorAttack.GetBehavior<AttackFilterModel>().filters.First(a=>a.GetIl2CppType().Name=="FilterTargetAngleModel").Cast<FilterTargetAngleModel>().fieldOfView=150;
            InteceptorAttack.weapons[0].rate=0.4f;
            InteceptorAttack.weapons[0].projectile.display="7c001a7fcad3c6d48b14aacba03a98bc";
            InteceptorAttack.weapons[0].emission=new SingleEmmisionTowardsTargetModel("SingleEmmisionTowardsTargetModel",null,0);
            Carrier.AddBehavior(InteceptorSpawn);
            Carrier.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display=Carrier.display;
            Carrier.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().positionOffset=new(0,0,100);
            Carrier.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId="CarrierBirth";
            Carrier.GetBehavior<CreateSoundOnTowerPlaceModel>().sound2.assetId=Carrier.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId;
            SetUpgradeSounds(Carrier,"CarrierUpgrade");
        }
        public class GravitonCatapult:ModUpgrade<Carrier>{
            public override string DisplayName=>"Graviton Catapult";
            public override string Description=>"Increases Inteceptor deployment speed and attack speed";
            public override int Cost=>2130;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Carrier){
                GetUpgradeModel().icon=new("CarrierGravitonCatapultIcon");
                Carrier.GetAttackModel().weapons[0].rate/=2;
                Carrier.GetAttackModel().weapons[0].projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().weapons[0].rate/=2;
                SetUpgradeSounds(Carrier,"CarrierUpgrade1");
            }
        }
        public class InteceptorHold:ModUpgrade<Carrier>{
            public override string DisplayName=>"Inteceptor";
            public override string Description=>"Bigger hold allows for more Inteceptors to be manafactured";
            public override int Cost=>4785;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Carrier){
                GetUpgradeModel().icon=new("CarrierHoldIcon");
                Carrier.GetAttackModel().weapons[0].GetBehavior<SubTowerFilterModel>().maxNumberOfSubTowers=8;
                SetUpgradeSounds(Carrier,"CarrierUpgrade2");
            }
        }
        /*public class Interdictors:ModUpgrade<Carrier>{
            public override string DisplayName=>"Interdictor's";
            public override string Description=>"Modified Inteceptors that lay short lived plasma charges everywhere";
            public override int Cost=>9825;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Carrier){
                GetUpgradeModel().icon=new("CarrierInterdictorIcon");
                SetUpgradeSounds(Carrier,"CarrierUpgrade3");
            }
        }
        public class SolarBeam:ModUpgrade<Carrier>{
            public override string DisplayName=>"Solar Beam";
            public override string Description=>"Weaker version of the Carrier's orbital purification beam, deals massive damage but can only target Moab's";
            public override int Cost=>15670;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Carrier){
                GetUpgradeModel().icon=new("CarrierSolarBeamIcon");
                var Beam=Game.instance.model.GetTowerFromId("Adora 10").behaviors.First(a=>a.name.Contains("Ball")).Cast<AbilityModel>().GetBehavior<AbilityCreateTowerModel>().towerModel.
                    GetAttackModel().Duplicate();
                //attack filter in this is literally null
                Beam.RemoveBehavior<AttackFilterModel>();
                Beam.AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().GetBehavior<AttackFilterModel>().Duplicate());
                Beam.GetBehavior<AttackFilterModel>().filters.AddItem(new FilterWithTagModel("FilterWithTagModel","Moabs",false));
                Carrier.RemoveBehavior<AttackModel>();
                Carrier.AddBehavior(Beam);
                SetUpgradeSounds(Carrier,"CarrierUpgrade4");
            }
        }
        public class Gantrithor:ModUpgrade<Carrier>{
            public override string DisplayName=>"Gantrithor";
            public override string Description=>"Legendary Super Carrier, was the flagship of the Conclave before being driven into the Overmind. Upgrades Solar beam, Inteceptors and Interdictors";
            public override int Cost=>45940;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Carrier){
                GetUpgradeModel().icon=new("CarrierGantrithorIcon");
            }
        }*/
        [HarmonyPatch(typeof(AudioFactory),"Start")]
        public class AudioFactoryStart_Patch{
            [HarmonyPostfix]
            public static void Prefix(ref AudioFactory __instance){
                if(ProtossEnabled){
                    __instance.RegisterAudioClip("CarrierBirth",TowerAssets.LoadAsset("CarrierBirth").Cast<AudioClip>());
                    __instance.RegisterAudioClip("CarrierUpgrade",TowerAssets.LoadAsset("CarrierUpgrade").Cast<AudioClip>());
                    __instance.RegisterAudioClip("CarrierUpgrade1",TowerAssets.LoadAsset("CarrierUpgrade1").Cast<AudioClip>());
                    __instance.RegisterAudioClip("CarrierUpgrade2",TowerAssets.LoadAsset("CarrierUpgrade2").Cast<AudioClip>());
                    __instance.RegisterAudioClip("CarrierUpgrade3",TowerAssets.LoadAsset("CarrierUpgrade3").Cast<AudioClip>());
                    __instance.RegisterAudioClip("CarrierUpgrade4",TowerAssets.LoadAsset("CarrierUpgrade4").Cast<AudioClip>());
                }
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                return LoadModel(TowerAssets,objectId,"Carrier",__instance,onComplete);
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public class ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.StartsWith("Carrier")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
    }
}