namespace SC2Expansion.Towers{
    public class HighTemplar:ModTower<ProtossSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.hightemplar);
        public override string DisplayName=>"High Templar";
        public override string BaseTower=>"WizardMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ProtossEnabled;
        public override string Description=>"High ranking ranged Protoss Caster";
        public override void ModifyBaseTowerModel(TowerModel HighTemplar){
            HighTemplar.display="HighTemplarPrefab";
            HighTemplar.portrait=new("HighTemplarIcon");
            HighTemplar.icon=new("HighTemplarIcon");
            HighTemplar.emoteSpriteLarge=new("Protoss");
            HighTemplar.radius=7;
            HighTemplar.range=40;
            var PsiBolt=HighTemplar.GetAttackModel();
            PsiBolt.range=HighTemplar.range;
            PsiBolt.weapons[0].projectile.GetDamageModel().damage=2;
            HighTemplar.GetBehavior<DisplayModel>().display=HighTemplar.display;
            HighTemplar.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId="HighTemplarBirth";
            HighTemplar.GetBehavior<CreateSoundOnTowerPlaceModel>().sound2.assetId=HighTemplar.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId;
            SetUpgradeSounds(HighTemplar,"HighTemplarUpgrade");
        }
        //Ik the khala isn't used now but i cba adding nerve cords to the model, sc2 itself handles these in a special way
        public class KhaydarinAmulet:ModUpgrade<HighTemplar>{
            public override string Name=>"KhaydarinAmulet";
            public override string DisplayName=>"Khaydarin Amulet";
            public override string Description=>"Khaydarin crystals help regulate the flow of energy from the Khala, letting the High Templar fire further";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel HighTemplar){
                GetUpgradeModel().icon=new("HighTemplarKhaydarinAmuletIcon");
                HighTemplar.range+=10;
                HighTemplar.GetAttackModel().range=HighTemplar.range;
                SetUpgradeSounds(HighTemplar,"HighTemplarUpgrade1");
            }
        }
        public class MiniPsiStorms:ModUpgrade<HighTemplar>{
            public override string Name=>"MiniPsiStorms";
            public override string DisplayName=>"Mini Psi Storms";
            public override string Description=>"Further training allows the casting of Psionic Storms into the track damaging everything that passes through";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel HighTemplar){
                GetUpgradeModel().icon=new("HighTemplarPsiStormIcon");
                var PsiStorm=Game.instance.model.GetTowerFromId("WizardMonkey-020").behaviors.First(a=>a.name.Contains("Wall")).Cast<AttackModel>().Duplicate();
                PsiStorm.name="PsiStorms";
                PsiStorm.weapons[0].projectile.display="88399aeca4ae48a44aee5b08eb16cc61";
                PsiStorm.weapons[0].projectile.RemoveBehaviors<CreateEffectOnExhaustedModel>();
                HighTemplar.AddBehavior(PsiStorm);
                SetUpgradeSounds(HighTemplar,"HighTemplarUpgrade2");
            }
        }
        public class PlasmaSurge:ModUpgrade<HighTemplar>{
            public override string Name=>"PlasmasSurge";
            public override string DisplayName=>"Plasma Surge";
            public override string Description=>"Focuses more power when casting Psi Storms increasing their radius and damage";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel HighTemplar){
                GetUpgradeModel().icon=new("HighTemplarPlasmaSurgeIcon");
                var PsiStorm=HighTemplar.behaviors.First(a=>a.name.Equals("PsiStorms")).Cast<AttackModel>();
                PsiStorm.weapons[0].projectile.radius=50;
                PsiStorm.weapons[0].projectile.GetDamageModel().damage+=1;
                SetUpgradeSounds(HighTemplar,"HighTemplarUpgrade3");
            }
        }
        public class Ascendant:ModUpgrade<HighTemplar>{
            public override string Name=>"Ascendant";
            public override string DisplayName=>"Ascendant";
            public override string Description=>"Ascendants are high ranking Tal'darim and have grown very powerful after years of absorbing terrazine";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel HighTemplar){
                GetUpgradeModel().icon=new("HighTemplarAscendantIcon");
                var PsiOrb=Game.instance.model.GetTowerFromId("Druid-400").GetAttackModel().Duplicate();
                var Sacrifice=Game.instance.model.GetTowerFromId("MonkeyBuccaneer-040").GetBehavior<AbilityModel>().Duplicate();
                var SacrificeBonus=Game.instance.model.GetTowerFromId("BoomerangMonkey-040").GetBehavior<AbilityModel>().GetBehavior<TurboModel>().Duplicate();
                var MindBlast=Game.instance.model.GetTowerFromId("PatFusty 10").behaviors.First(a=>a.name.Contains("Big")).Cast<AbilityModel>().Duplicate();
                var PsiBolt=HighTemplar.GetAttackModel();
                var WeaponToRemove=PsiOrb.weapons.GetEnumerator();
                while(WeaponToRemove.MoveNext()){
                    switch(WeaponToRemove.Current.name){
                        case "WeaponModel_Weapon"or"WeaponModel_Tornado"or"WeaponModel_Lightning":
                            PsiOrb.RemoveWeapon(WeaponToRemove.Current);
                            break;
                    }
                }
                PsiOrb.name="PsiOrb";
                Sacrifice.name="Sacrifice";
                Sacrifice.displayName="Sacrifice";
                Sacrifice.cooldown=60;
                SacrificeBonus.extraDamage=6;
                SacrificeBonus.projectileDisplay.assetPath=null;
                Sacrifice.icon=new("HighTemplarSacrificeIcon");
                Sacrifice.GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<CreateRopeEffectModel>().assetId=null;
                Sacrifice.GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<CreateRopeEffectModel>().endAssetId=null;
                Sacrifice.GetBehavior<CreateSoundOnAbilityModel>().sound.assetId="HighTemplarSacrificeVO";
                Sacrifice.maxActivationsPerRound=1;
                Sacrifice.AddBehavior(SacrificeBonus);
                MindBlast.name="MindBlast";
                MindBlast.displayName="Mind Blast";
                MindBlast.GetBehavior<ActivateAttackModel>().attacks[0]=Game.instance.model.GetTowerFromId("SniperMonkey-500").GetAttackModel().Duplicate();
                MindBlast.GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetDamageModel().damage=120;
                MindBlast.GetBehavior<CreateSoundOnAbilityModel>().sound.assetId="HighTemplarMindBlastVO";
                MindBlast.cooldown=80;
                MindBlast.icon=new("HighTemplarMindBlastIcon");
                MindBlast.maxActivationsPerRound=1;
                PsiBolt.weapons[0].projectile.GetDamageModel().damage=5;
                PsiBolt.name="PsiBolt";
                HighTemplar.display="HighTemplarAscendantPrefab";
                HighTemplar.RemoveBehavior(HighTemplar.GetBehaviors<AttackModel>().First(a=>a.name.Contains("PsiStorm")));
                HighTemplar.AddBehavior(MindBlast);
                HighTemplar.AddBehavior(Sacrifice);
                HighTemplar.AddBehavior(PsiOrb);
                SetUpgradeSounds(HighTemplar,"HighTemplarUpgrade4");
            }
        }
        public class Jinara:ModUpgrade<HighTemplar>{
            public override string Name=>"Jinara";
            public override string DisplayName=>"Ji'nara";
            public override string Description=>"As the first Ascendant of the Tal'darim, her power is second only to the highlord";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel HighTemplar){
                GetUpgradeModel().icon=new("HighTemplarJinaraPortrait");
                HighTemplar.portrait=new("HighTemplarJinaraPortrait");
                HighTemplar.display="HighTemplarJinaraPrefab";
                var PsiOrb=HighTemplar.behaviors.First(a=>a.name.Equals("PsiOrb")).Cast<AttackModel>();
                var Sacrifice=HighTemplar.behaviors.First(a=>a.name.Equals("Sacrifice")).Cast<AbilityModel>();
                var SacrificeAttack=Sacrifice.GetBehavior<ActivateAttackModel>().attacks[0];
                var MindBlast=HighTemplar.behaviors.First(a=>a.name.Equals("MindBlast")).Cast<AbilityModel>();
                var MindBlastAttack=MindBlast.GetBehavior<ActivateAttackModel>().attacks[0];
                var PsiBolt=HighTemplar.behaviors.First(a=>a.name.Equals("PsiBolt")).Cast<AttackModel>();
                PsiOrb.weapons[0].rate=0.8f;
                MindBlast.cooldown=50;
                Sacrifice.cooldown=50;
                MindBlast.maxActivationsPerRound=-1;
                Sacrifice.maxActivationsPerRound=-1;
                Sacrifice.GetBehavior<CreateSoundOnAbilityModel>().sound.assetId="HighTemplarJinaraSacrificeVO";
                SacrificeAttack.GetBehavior<TargetGrapplableModel>().canHitZomg=true;
                SacrificeAttack.RemoveBehavior<AttackFilterModel>();
                MindBlast.cooldown=40;
                MindBlastAttack.weapons[0].projectile.GetDamageModel().damage=400;
                MindBlast.GetBehavior<CreateSoundOnAbilityModel>().sound.assetId="HighTemplarJinaraMindBlastVO";
                PsiBolt.weapons[0].projectile.GetDamageModel().damage=40;
            }
        }
        [HarmonyPatch(typeof(AudioFactory),"Start")]
        public class AudioFactoryStart_Patch{
            [HarmonyPostfix]
            public static void Prefix(ref AudioFactory __instance){
                if(ProtossEnabled){
                    __instance.RegisterAudioClip("HighTemplarBirth",TowerAssets.LoadAsset("HighTemplarBirth").Cast<AudioClip>());
                    __instance.RegisterAudioClip("HighTemplarUpgrade",TowerAssets.LoadAsset("HighTemplarUpgrade").Cast<AudioClip>());
                    __instance.RegisterAudioClip("HighTemplarUpgrade1",TowerAssets.LoadAsset("HighTemplarUpgrade1").Cast<AudioClip>());
                    __instance.RegisterAudioClip("HighTemplarUpgrade2",TowerAssets.LoadAsset("HighTemplarUpgrade2").Cast<AudioClip>());
                    __instance.RegisterAudioClip("HighTemplarUpgrade3",TowerAssets.LoadAsset("HighTemplarUpgrade3").Cast<AudioClip>());
                    __instance.RegisterAudioClip("HighTemplarUpgrade4",TowerAssets.LoadAsset("HighTemplarUpgrade4").Cast<AudioClip>());
                    __instance.RegisterAudioClip("HighTemplarSacrificeVO",TowerAssets.LoadAsset("HighTemplarSacrificeVO").Cast<AudioClip>());
                    __instance.RegisterAudioClip("HighTemplarMindBlastVO",TowerAssets.LoadAsset("HighTemplarMindBlastVO").Cast<AudioClip>());
                    __instance.RegisterAudioClip("HighTemplarJinaraSacrificeVO",TowerAssets.LoadAsset("HighTemplarJinaraSacrificeVO").Cast<AudioClip>());
                    __instance.RegisterAudioClip("HighTemplarJinaraMindBlastVO",TowerAssets.LoadAsset("HighTemplarJinaraMindBlastVO").Cast<AudioClip>());
                }
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!DisplayDict.ContainsKey(objectId)&&objectId.Contains("HighTemplar")){
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
                if(reference!=null&&reference.guidRef.StartsWith("HighTemplar")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public class WeaponSpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.towerModel.name.Contains("HighTemplar")){
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("HighTemplarAttack");
                }
            }
        }
    }
}