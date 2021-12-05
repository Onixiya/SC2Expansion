namespace SC2Expansion.Towers{
    public class Marine:ModTower<TerranSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.marine);
        public override string DisplayName=>"Marine";
        public override string BaseTower=>"SniperMonkey";
        public override int Cost=>250;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!TerranEnabled;
        public override string Description=>"Basic Terran soldier with automatic gauss rifle";
        public override void ModifyBaseTowerModel(TowerModel Marine){
            Marine.display="MarinePrefab";
            Marine.portrait=new("MarineIcon");
            Marine.icon=new("MarineIcon");
            Marine.emoteSpriteLarge=new("Terran");
            Marine.radius=5;
            Marine.cost=400;
            Marine.range=35;
            var C14=Marine.GetAttackModel();
            C14.weapons[0].rate=0.25f;
            C14.weapons[0].rateFrames=1;
            C14.range=Marine.range;
            C14.weapons[0].projectile.display=null;
            C14.weapons[0].projectile.GetDamageModel().damage=2.5f;
            Marine.GetBehavior<DisplayModel>().display=Marine.display;
            Marine.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId="MarineBirth";
            Marine.GetBehavior<CreateSoundOnTowerPlaceModel>().sound2=Marine.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1;
            SetUpgradeSounds(Marine,"MarineUpgrade");
        }
        public class U238Shells:ModUpgrade<Marine>{
            public override string DisplayName=>"U-238 Shells";
            public override string Description=>"Making the ammunition casing out of depleted Uranium 238 increases range and damage";
            public override int Cost=>175;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineU238ShellsIcon");
                Marine.range+=10;
                var C14=Marine.GetAttackModel();
                C14.range=Marine.range;
                C14.weapons[0].projectile.GetDamageModel().damage+=1;
                SetUpgradeSounds(Marine,"MarineUpgrade1");
            }
        }
        public class LTS:ModUpgrade<Marine>{
            public override string DisplayName=>"Laser Targeting System";
            public override string Description=>"Adding a laser pointer allows targetting camo bloons and slightly increases range";
            public override int Cost=>450;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineLaserTargetingSystemIcon");
                Marine.range+=5;
                Marine.GetAttackModel().range=Marine.range;
                Marine.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true));
                SetUpgradeSounds(Marine,"MarineUpgrade2");
            }
        }
        public class Stimpacks:ModUpgrade<Marine>{
            public override string DisplayName=>"Stimpacks";
            public override string Description=>"Stimpacks increase attack speed by 50% for a short while";
            public override int Cost=>830;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineStimpacksIcon");
                var Stimpacks=Game.instance.model.GetTowerFromId("BoomerangMonkey-040").GetBehavior<AbilityModel>().Duplicate();
                Stimpacks.GetBehavior<CreateSoundOnAbilityModel>().sound.assetId="MarineStimpack";
                Stimpacks.name="Stimpacks";
                Stimpacks.displayName="Stimpacks";
                Stimpacks.icon=new("MarineStimpacksIcon");
                Stimpacks.cooldown=40;
                Stimpacks.maxActivationsPerRound=1;
                Stimpacks.GetBehavior<TurboModel>().extraDamage=0;
                Stimpacks.GetBehavior<TurboModel>().projectileDisplay=null;
                Marine.AddBehavior(Stimpacks);
                SetUpgradeSounds(Marine,"MarineUpgrade3");
            }
        }
        public class Warpig:ModUpgrade<Marine>{
            public override string DisplayName=>"Warpig";
            public override string Description=>"Warpig mercenaries use upgraded (Don't ask if its legal) equipment. Increases damage and attack speed";
            public override int Cost=>1475;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineWarpigIcon");
                Marine.display="MarineWarpigPrefab";
                Marine.portrait=new("MarineWarpigPortrait");
                var C14=Marine.GetAttackModel();
                C14.weapons[0].rate=0.17f;
                C14.weapons[0].projectile.GetDamageModel().damage+=2;
                SetUpgradeSounds(Marine,"MarineUpgrade4");
            }
        }
        public class Raynor:ModUpgrade<Marine>{
            public override string DisplayName=>"James Raynor";
            public override string Description=>"\"Jimmy here!\"";
            public override int Cost=>8755;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineRaynorIcon");
                Marine.display="MarineRaynorPrefab";
                Marine.portrait=new("MarineRaynorIcon");
                var C14=Marine.GetAttackModel();
                C14.weapons[0].rate=0.13f;
                C14.weapons[0].projectile.GetDamageModel().damage+=3;
                var FragGrenade=Game.instance.model.GetTowerFromId("BombShooter-002").GetAttackModel();
                FragGrenade.name="MarineFragGrenade";
                FragGrenade.AddBehavior(new PauseAllOtherAttacksModel("PauseAllOtherAttacksModel",1,true,true));
                FragGrenade.range=Marine.range-10;
                Marine.GetAbility().GetBehavior<CreateSoundOnAbilityModel>().sound.assetId="MarineStimpack1";
                Marine.AddBehavior(FragGrenade);
            }
        }
        [HarmonyPatch(typeof(AudioFactory),"Start")]
        public class AudioFactoryStart_Patch{
            [HarmonyPostfix]
            public static void Prefix(ref AudioFactory __instance){
                if(TerranEnabled){
                    AudioFactoryInstance=__instance;
                    __instance.RegisterAudioClip("MarineBirth",TowerAssets.LoadAsset("MarineBirth").Cast<AudioClip>());
                    __instance.RegisterAudioClip("MarineUpgrade",TowerAssets.LoadAsset("MarineUpgrade").Cast<AudioClip>());
                    __instance.RegisterAudioClip("MarineUpgrade1",TowerAssets.LoadAsset("MarineUpgrade1").Cast<AudioClip>());
                    __instance.RegisterAudioClip("MarineUpgrade2",TowerAssets.LoadAsset("MarineUpgrade2").Cast<AudioClip>());
                    __instance.RegisterAudioClip("MarineUpgrade3",TowerAssets.LoadAsset("MarineUpgrade3").Cast<AudioClip>());
                    __instance.RegisterAudioClip("MarineUpgrade4",TowerAssets.LoadAsset("MarineUpgrade4").Cast<AudioClip>());
                    __instance.RegisterAudioClip("MarineStimpack",TowerAssets.LoadAsset("MarineStimpack").Cast<AudioClip>());
                    __instance.RegisterAudioClip("MarineStimpack1",TowerAssets.LoadAsset("MarineStimpack1").Cast<AudioClip>());
                }
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                return LoadModel(TowerAssets,objectId,"Marine",__instance,onComplete);
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public class ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.StartsWith("Marine")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public class WeaponSpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.towerModel.name.StartsWith("SC2Expansion-Marine")){
                    __instance.attack.tower.Node.graphic.GetComponent<Animator>().Play("MarineAttack");
                }
            }
        }
    }
}