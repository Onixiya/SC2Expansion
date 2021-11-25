namespace SC2Expansion.Towers{
    public class Viking:ModTower<TerranSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.viking);
        public override string DisplayName=>"Viking";
        public override string BaseTower=>"SniperMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!TerranEnabled;
        public override string Description=>"Terran ground fire support, fire's 2 powerful Gatling Cannons. Cannot target Moab's";
        public override void ModifyBaseTowerModel(TowerModel Viking){
            Viking.display="VikingGroundPrefab";
            Viking.portrait=new("VikingPortrait");
            Viking.icon=new("VikingGroundIcon");
            Viking.emoteSpriteLarge=new("Terran");
            Viking.radius=5;
            Viking.cost=400;
            Viking.range=45;
            var Gatling=Viking.GetAttackModel();
            Gatling.range=Viking.range;
            Gatling.GetBehavior<AttackFilterModel>().filters=Gatling.GetBehavior<AttackFilterModel>().filters.AddTo(new FilterOutTagModel("FilterOutTagModel","Moabs",null));
            Gatling.weapons[0].name="VikingGatling";
            Gatling.weapons[0].projectile.GetDamageModel().damage=2;
            Gatling.weapons[0].projectile.GetDamageModel().immuneBloonProperties=(BloonProperties)17;
            Gatling.weapons[0].ejectX=7.5f;
            Gatling.weapons[0].ejectY=10;
            Gatling.weapons[0].rate=1.1f;
            Gatling.AddWeapon(Gatling.weapons[0].Duplicate());
            Gatling.weapons[1].ejectX=-Gatling.weapons[0].ejectX;
            Viking.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display=Viking.display;
            Viking.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId="VikingBirth";
            Viking.GetBehavior<CreateSoundOnTowerPlaceModel>().sound2=Viking.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1;
            SetUpgradeSounds(Viking,"VikingUpgrade");
        }
        public class AirMode:ModUpgrade<Viking>{
            public override string Name=>"AirMode";
            public override string DisplayName=>"Fighter mode";
            public override string Description=>"Trains the pilot to transform and make use of the Fighter mode for a short time gaining bonus damage against Moab's. Only targets Moab's in Fighter Mode";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingAirIcon");
                var AirMode=Game.instance.model.GetTowerFromId("Alchemist-040").GetAbility().Duplicate();
                AirMode.GetBehavior<ActivateAttackModel>().attacks[0]=Game.instance.model.GetTowerFromId("BombShooter-030").GetAttackModel().Duplicate();
                var Lanzer=AirMode.GetBehavior<ActivateAttackModel>().attacks[0];
                AirMode.GetBehavior<ActivateAttackModel>().lifespan=15;
                AirMode.GetBehavior<SwitchDisplayModel>().lifespan=15;
                AirMode.GetBehavior<SwitchDisplayModel>().display="VikingAirPrefab";
                AirMode.GetBehavior<IncreaseRangeModel>().addative=30;
                AirMode.cooldown=40f;
                AirMode.icon=new("VikingAirIcon");
                AirMode.name="AirMode";
                AirMode.GetBehavior<CreateSoundOnAbilityModel>().sound.assetId="VikingTransform";
                Lanzer.GetBehavior<AttackFilterModel>().filters=Lanzer.GetBehavior<AttackFilterModel>().filters.AddTo(new FilterWithTagModel("FilterWithTagModel","Moabs",false));
                Lanzer.range=Viking.range+AirMode.GetBehavior<IncreaseRangeModel>().addative;
                Lanzer.weapons[0].projectile.display="VikingMissilePrefab";
                Lanzer.weapons[0].ejectZ=50;
                Lanzer.weapons[0].projectile.AddBehavior(new TrackTargetModel("TrackTargetModel",999,false,false,90,false,360,false,false));
                Viking.AddBehavior(AirMode);
                SetUpgradeSounds(Viking,"VikingUpgrade1");
            }
        }
        public class PhobosWeapons:ModUpgrade<Viking>{
            public override string Name=>"PhobosWeapons";
            public override string DisplayName=>"Phobos Class weapons systems";
            public override string Description=>"Increases range and damage in all modes";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingPhobosWeaponsIcon");
                var Gatling=Viking.GetAttackModel();
                Viking.range+=10;
                Gatling.range=Viking.range;
                Gatling.weapons[0].projectile.GetDamageModel().damage+=2;
                Gatling.weapons[1].projectile.GetDamageModel().damage+=2;
                Viking.GetAbility().GetBehavior<IncreaseRangeModel>().addative=40;
                Viking.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].range=Viking.range+Viking.GetAbility().GetBehavior<IncreaseRangeModel>().addative;
                SetUpgradeSounds(Viking,"VikingUpgrade2");
            }
        }
        public class Deimos:ModUpgrade<Viking>{
            public override string Name=>"Deimos";
            public override string DisplayName=>"Deimos Viking";
            public override string Description=>"Dominion Vikings modified with uh, \'legal\' mercenary equipment, more pierce, damage, better fire rate and can pop Lead Bloons on the ground and"+
                " more rockets in the air";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingDeimosGroundIcon");
                var Gatling=Viking.GetAttackModel();
                var AirMode=Viking.GetAbility();
                AirMode.GetBehavior<ActivateAttackModel>().attacks=AirMode.GetBehavior<ActivateAttackModel>().attacks.AddTo(Game.instance.model.GetTowerFromId("BombShooter-020").
                    GetAttackModel().Duplicate());
                var WILD=AirMode.GetBehavior<ActivateAttackModel>().attacks[1];
                Gatling.weapons[0].projectile.pierce=3;
                Gatling.weapons[0].projectile.GetDamageModel().damage+=1;
                Gatling.weapons[0].projectile.GetDamageModel().immuneBloonProperties=0;
                Gatling.weapons[0].rate=0.55f;
                Gatling.weapons[1]=Gatling.weapons[0].Duplicate();
                Gatling.weapons[1].ejectX=-Gatling.weapons[0].ejectX;
                AirMode.GetBehavior<SwitchDisplayModel>().display="VikingDeimosAirPrefab";
                AirMode.icon=new("VikingDeimosAirIcon");
                WILD.weapons[0].emission=new RandomArcEmissionModel("RandomArcEmissionModel",8,30,90,10,10,null);
                WILD.weapons[0].projectile.RemoveBehavior<TravelStraitModel>();
                WILD.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("DartlingGunner-050").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].
                    projectile.GetBehavior<TravelCurvyModel>().Duplicate());
                WILD.weapons[0].projectile.GetBehavior<TravelCurvyModel>().speed=200;
                WILD.weapons[0].projectile.RemoveBehaviors<TrackTargetModel>();
                WILD.range=Viking.range+AirMode.GetBehavior<IncreaseRangeModel>().addative;
                WILD.weapons[0].projectile.display="VikingMissilePrefab";
                WILD.weapons[0].ejectZ=50;
                WILD.GetBehavior<AttackFilterModel>().filters=WILD.GetBehavior<AttackFilterModel>().filters.AddTo(new FilterWithTagModel("FilterWithTagModel","Moabs",false));
                Viking.portrait=new("VikingDeimosPortrait");
                Viking.display="VikingDeimosGroundPrefab";
                SetUpgradeSounds(Viking,"VikingUpgrade3");
            }
        }
        public class SkyFury:ModUpgrade<Viking>{
            public override string Name=>"SkyFury";
            public override string DisplayName=>"Sky Fury";
            public override string Description=>"Elite Dominion Vikings, doubles the time spent in Fighter mode and can now attack Moab's in ground mode";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingSkyFuryGroundIcon");
                var AirMode=Viking.GetAbility();
                AirMode.icon=new("VikingSkyFuryAirIcon");
                AirMode.GetBehavior<SwitchDisplayModel>().display="VikingSkyFuryAirPrefab";
                AirMode.GetBehavior<SwitchDisplayModel>().lifespan=30;
                AirMode.GetBehavior<ActivateAttackModel>().lifespan=30;
                Viking.portrait=new("VikingSkyFuryPortrait");
                Viking.display="VikingSkyFuryGroundPrefab";
                Viking.GetAttackModel().RemoveBehavior(Viking.GetAttackModel().GetBehavior<AttackFilterModel>());
                SetUpgradeSounds(Viking,"VikingUpgrade4");
            }
        }
        public class Archangel:ModUpgrade<Viking>{
            public override string Name=>"Archangel";
            public override string DisplayName=>"Archangel";
            public override string Description=>"The pride of the Dominion Engineering Corps, a Archangel is the solution to almost any threat";
            public override int Cost=>9000;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingArchAngelIcon");
                var WILD=Viking.GetAbility().GetBehavior<ActivateAttackModel>().attacks[1].Duplicate();
                var Gatling=Viking.GetAttackModel();
                Viking.range=110;
                WILD.range=Viking.range-25;
                WILD.weapons[0].projectile.RemoveBehavior<CreateProjectileOnContactModel>();
                WILD.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-022").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().
                    Duplicate());
                Gatling.range=Viking.range;
                Gatling.weapons[0].rate=0.1f;
                Gatling.weapons[0].ejectX=13;
                Gatling.weapons[0].ejectY=20;
                Gatling.weapons[0].ejectZ=15;
                Gatling.weapons[0].projectile.GetDamageModel().damage=15;
                Gatling.weapons[1]=Gatling.weapons[0].Duplicate();
                Gatling.weapons[1].ejectX=-Gatling.weapons[0].ejectX;
                Viking.RemoveBehavior<AbilityModel>();
                Viking.portrait=new("VikingArchangelPortrait");
                Viking.display="VikingArchangelPrefab";
                Viking.AddBehavior(WILD);
            }
        }
        [HarmonyPatch(typeof(AudioFactory),"Start")]
        public class AudioFactoryStart_Patch{
            [HarmonyPostfix]
            public static void Prefix(ref AudioFactory __instance){
                if(TerranEnabled){
                    AudioFactoryInstance=__instance;
                    __instance.RegisterAudioClip("VikingBirth",TowerAssets.LoadAsset("VikingBirth").Cast<AudioClip>());
                    __instance.RegisterAudioClip("VikingUpgrade",TowerAssets.LoadAsset("VikingUpgrade").Cast<AudioClip>());
                    __instance.RegisterAudioClip("VikingUpgrade1",TowerAssets.LoadAsset("VikingUpgrade1").Cast<AudioClip>());
                    __instance.RegisterAudioClip("VikingUpgrade2",TowerAssets.LoadAsset("VikingUpgrade2").Cast<AudioClip>());
                    __instance.RegisterAudioClip("VikingUpgrade3",TowerAssets.LoadAsset("VikingUpgrade3").Cast<AudioClip>());
                    __instance.RegisterAudioClip("VikingUpgrade4",TowerAssets.LoadAsset("VikingUpgrade4").Cast<AudioClip>());
                    __instance.RegisterAudioClip("VikingTransform",TowerAssets.LoadAsset("VikingTransform").Cast<AudioClip>());
                }
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!DisplayDict.ContainsKey(objectId)&&objectId.Contains("Viking")){
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
                if(reference!=null&&reference.guidRef.StartsWith("Viking")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public class WeaponSpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.towerModel.name.Contains("Viking")){
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("VikingGroundAttack");
                }
            }
        }
    }
}