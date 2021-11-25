namespace SC2Expansion.Towers{
    public class VoidRay:ModTower<ProtossSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.voidray);
        public override string DisplayName=>"Void Ray";
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>1900;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ProtossEnabled;
        public override string Description=>"Flying Protoss precision ranged craft. Deals double damage against Moab's";
        public override void ModifyBaseTowerModel(TowerModel VoidRay){
            VoidRay.display="VoidRayPrefab";
            VoidRay.portrait=new("VoidRayPortrait");
            VoidRay.icon=new("VoidRayIcon");
            VoidRay.emoteSpriteLarge=new("Protoss");
            VoidRay.radius=7;
            VoidRay.range=55;
            VoidRay.areaTypes=new(4);
            VoidRay.areaTypes[0]=AreaType.land;
            VoidRay.areaTypes[1]=AreaType.track;
            VoidRay.areaTypes[2]=AreaType.ice;
            VoidRay.areaTypes[3]=AreaType.water;
            var Beam=VoidRay.GetAttackModel();
            Beam.weapons[0]=Game.instance.model.GetTowerFromId("Adora 10").behaviors.First(a=>a.name.Contains("Ball")).Cast<AbilityModel>().GetBehavior<AbilityCreateTowerModel>().
                towerModel.GetAttackModel().weapons[0].Duplicate();
            Beam.range=VoidRay.range;
            Beam.weapons[0].projectile.GetDamageModel().damage=1;
            Beam.weapons[0].projectile.pierce=1;
            Beam.weapons[0].rate=0.1f;
            Beam.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-030").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().
                projectile.GetBehavior<DamageModifierForTagModel>().Duplicate());
            Beam.weapons[0].projectile.GetBehavior<DamageModifierForTagModel>().damageAddative=0;
            Beam.weapons[0].projectile.GetBehavior<DamageModifierForTagModel>().damageMultiplier=2;
            VoidRay.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display=VoidRay.display;
            VoidRay.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId="VoidRayBirth";
            VoidRay.GetBehavior<CreateSoundOnTowerPlaceModel>().sound2.assetId=VoidRay.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId;
            SetUpgradeSounds(VoidRay,"VoidRayUpgrade");
        }
        public class PrismaticAlignment:ModUpgrade<VoidRay>{
            public override string Name=>"PrismaticAlignment";
            public override string DisplayName=>"Prismatic Alignment";
            public override string Description=>"Training a better void lens adds more damage";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel VoidRay){
                GetUpgradeModel().icon=new("VoidRayPrismaticAlignmentIcon");
                VoidRay.GetAttackModel().weapons[0].projectile.GetDamageModel().damage=2;
                SetUpgradeSounds(VoidRay,"VoidRayUpgrade1");
            }
        }
        public class FluxVanes:ModUpgrade<VoidRay>{
            public override string Name=>"FluxVanes";
            public override string DisplayName=>"Flux Vanes";
            public override string Description=>"Better flux vanes allow for a faster attack";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel VoidRay){
                GetUpgradeModel().icon=new("VoidRayFluxVanesIcon");
                VoidRay.GetAttackModel().weapons[0].rate=0.075f;
                SetUpgradeSounds(VoidRay,"VoidRayUpgrade2");
            }
        }
        public class PrismaticRange:ModUpgrade<VoidRay>{
            public override string Name=>"PrismaticRange";
            public override string DisplayName=>"Prismatic Range";
            public override string Description=>"Refining the prismatic core even more lets it fire further";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel VoidRay){
                GetUpgradeModel().icon=new("VoidRayPrismaticRangeIcon");
                VoidRay.range=65;
                VoidRay.GetAttackModel().range=VoidRay.range;
                SetUpgradeSounds(VoidRay,"VoidRayUpgrade3");
            }
        }
        //i tried for 2 and a half days to get the beam to bounce, it didn't wanna fucking bounce at all
        public class Destroyer:ModUpgrade<VoidRay>{
            public override string Name=>"Destroyer";
            public override string DisplayName=>"Destroyer";
            public override string Description=>"Stolen Void Rays that have had their prismatic cores replaced with bloodshard crystals. Beam sometimes explodes"+
                " upon hitting a target sending seeking shards everywhere";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel VoidRay){
                GetUpgradeModel().icon=new("VoidRayDestroyerIcon");
                VoidRay.portrait=new("VoidRayDestroyerPortrait");
                VoidRay.display="VoidRayDestroyerPrefab";
                var Shards=Game.instance.model.towers.First(a=>a.name.Contains("BombShooter-002")).GetAttackModel().Duplicate();
                var ShardsProj=Shards.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile;
                ShardsProj.GetBehavior<DamageModel>().immuneBloonProperties=0;
                Shards.weapons[0].projectile.AddBehavior(new TrackTargetModel("TargetTrackModel",999,true,true,360,true,999,false,true));
                Shards.range=VoidRay.range;
                Shards.weapons[0].projectile.display=null;
                ShardsProj.pierce=2;
                ShardsProj.display="dfc16ec49f4894148bce0161ebb0bd32";
                Shards.name="Shards";
                VoidRay.AddBehavior(Shards);
                SetUpgradeSounds(VoidRay,"VoidRayUpgrade4");
            }
        }
        public class Mohandar:ModUpgrade<VoidRay>{
            public override string Name=>"Mohandar";
            public override string DisplayName=>"Mohandar";
            public override string Description=>"\"We linger in twilight\"";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel VoidRay){
                GetUpgradeModel().icon=new("VoidRayMohandarPortrait");
                VoidRay.portrait=new("VoidRayMohandarPortrait");
                VoidRay.display="VoidRayMohandarPrefab";
                VoidRay.RemoveBehavior(VoidRay.GetBehaviors<AttackModel>().First(a=>a.name.Equals("Shards")));
                VoidRay.range=85;
                var Beam=VoidRay.GetAttackModel();
                Beam.weapons[0].projectile.GetDamageModel().damage=4;
                Beam.range=VoidRay.range;
            }
        }
        [HarmonyPatch(typeof(AudioFactory),"Start")]
        public class AudioFactoryStart_Patch{
            [HarmonyPostfix]
            public static void Prefix(ref AudioFactory __instance){
                if(ProtossEnabled){
                    __instance.RegisterAudioClip("VoidRayBirth",TowerAssets.LoadAsset("VoidRayBirth").Cast<AudioClip>());
                    __instance.RegisterAudioClip("VoidRayUpgrade",TowerAssets.LoadAsset("VoidRayUpgrade").Cast<AudioClip>());
                    __instance.RegisterAudioClip("VoidRayUpgrade1",TowerAssets.LoadAsset("VoidRayUpgrade1").Cast<AudioClip>());
                    __instance.RegisterAudioClip("VoidRayUpgrade2",TowerAssets.LoadAsset("VoidRayUpgrade2").Cast<AudioClip>());
                    __instance.RegisterAudioClip("VoidRayUpgrade3",TowerAssets.LoadAsset("VoidRayUpgrade3").Cast<AudioClip>());
                    __instance.RegisterAudioClip("VoidRayUpgrade4",TowerAssets.LoadAsset("VoidRayUpgrade4").Cast<AudioClip>());
                }
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!DisplayDict.ContainsKey(objectId)&&objectId.Contains("VoidRay")){
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
                if(reference!=null&&reference.guidRef.StartsWith("VoidRay")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
    }
}