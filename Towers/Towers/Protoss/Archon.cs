namespace SC2Expansion.Towers{
    public class Archon:ModTower<ProtossSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.archon);
        public override string DisplayName=>"Archon";
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ProtossEnabled;
        public override string Description=>"Powerful psionic attacker, obliterates almost all non Moabs with ease";
        public override void ModifyBaseTowerModel(TowerModel Archon){
            Archon.display="ArchonPrefab";
            Archon.portrait=new("ArchonPortrait");
            Archon.icon=new("ArchonIcon");
            Archon.emoteSpriteLarge=new("Protoss");
            Archon.radius=5;
            Archon.cost=400;
            Archon.range=30;
            var Lightning=Archon.GetAttackModel().Cast<AttackModel>();
            Lightning.range=Archon.range;
            Lightning.weapons[0]=Game.instance.model.GetTowerFromId("Druid-200").GetAttackModel().weapons[1].Duplicate();
            Lightning.weapons[0].rate=0.9f;
            Lightning.weapons[0].ejectY=20;
            Lightning.weapons[0].ejectZ=20;
            Lightning.weapons[0].projectile.GetBehavior<CreateLightningEffectModel>().lifeSpan=1.25f;
            Lightning.weapons[0].projectile.GetBehavior<LightningModel>().splits=0;
            Lightning.weapons[0].projectile.GetDamageModel().damage=15;
            Lightning.weapons[0].projectile.GetDamageModel().distributeToChildren=true;
            Lightning.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel","Moabs",0,-10,false,false));
            Archon.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="ArchonPrefab";
            Archon.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId="ArchonBirth";
            Archon.GetBehavior<CreateSoundOnTowerPlaceModel>().sound2.assetId=Archon.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId;
            SetUpgradeSounds(Archon,"ArchonUpgrade");
        }
        public class ArchonKhaydarinAmulet:ModUpgrade<Archon>{
            public override string Name=>"ArchonKhaydarinAmulet";
            public override string DisplayName=>"Khaydarin Amulet";
            public override string Description=>"Preserving the Khaydarin Amulet before merging increases range and damage";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Archon){
                GetUpgradeModel().icon=new("HighTemplarKhaydarinAmuletIcon");
                var Lightning=Archon.GetAttackModel();
                Archon.range+=10;
                Lightning.range=Archon.range;
                Lightning.weapons[0].projectile.GetDamageModel().damage+=5;
                Lightning.weapons[0].projectile.GetBehavior<DamageModifierForTagModel>().damageAddative=-15;
                SetUpgradeSounds(Archon,"ArchonUpgrade1");
            }
        }
        public class HighArchon:ModUpgrade<Archon>{
            public override string Name=>"HighArchon";
            public override string DisplayName=>"High Archon";
            public override string Description=>"Regularly focus's great amounts of psionic energy into small areas damaging everything that passes through";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Archon){
                GetUpgradeModel().icon=new("ArchonHighArchonIcon");
                var PsiStorm=Game.instance.model.GetTowerFromId("WizardMonkey-020").behaviors.First(a=>a.name.Contains("Wall")).Clone().Cast<AttackModel>();
                PsiStorm.name="PsiStorm";
                PsiStorm.weapons[0].projectile.display="88399aeca4ae48a44aee5b08eb16cc61";
                PsiStorm.weapons[0].projectile.RemoveBehaviors<CreateEffectOnExhaustedModel>();
                PsiStorm.range=Archon.range;
                Archon.AddBehavior(PsiStorm);
                SetUpgradeSounds(Archon,"ArchonUpgrade2");
            }
        }
        public class DarkArchon:ModUpgrade<Archon>{
            public override string Name=>"DarkArchon";
            public override string DisplayName=>"Dark Archon";
            public override string Description=>"Dark Archon's draw incredible amounts of power from the Void, gains Confusion attack confusing all bloons in a small area and forcing them to go back for short time";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Archon){
                GetUpgradeModel().icon=new("ArchonDarkArchonIcon");
                Archon.display="ArchonDarkArchonPrefab";
                Archon.portrait=new("ArchonDarkArchonPortrait");
                var Confusion=Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
                var ConfusionProjectile=Archon.behaviors.First(a=>a.name.Contains("PsiStorm")).Cast<AttackModel>().weapons[0].projectile.Duplicate();
                Confusion.name="Confusion";
                Confusion.weapons[0].projectile.RemoveBehavior<DamageModel>();
                Confusion.weapons[0].projectile.display=null;
                ConfusionProjectile.RemoveBehavior<DamageModel>();
                ConfusionProjectile.AddBehavior(new WindModel("WindModel",20,150,1,false,null,0,null));
                ConfusionProjectile.GetBehavior<AgeModel>().lifespan=0.1f;
                ConfusionProjectile.display=null;
                Confusion.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("CreateProjectileOnContactModel",ConfusionProjectile,new SingleEmissionModel("SingleEmissionModel",null),
                    false,false,false));
                Archon.RemoveBehavior(Archon.GetBehaviors<AttackModel>().First(a=>a.name.Contains("PsiStorm")));
                Archon.AddBehavior(Confusion);
                SetUpgradeSounds(Archon,"ArchonUpgrade3");
            }
        }
        public class MindControl:ModUpgrade<Archon>{
            public override string Name=>"MindControl";
            public override string DisplayName=>"Mind Control";
            public override string Description=>"Completely controls the strongest target on screen and forces it go back along the track. Cannot target anything higher then a ZOMG";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Archon){
                GetUpgradeModel().icon=new("ArchonMindControlIcon");
                var MindControl=Game.instance.model.GetTowerFromId("MonkeyBuccaneer-040").GetAbility().Duplicate();
                var MindControlAttack=Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
                var MindControlAttackFilter=MindControlAttack.GetBehavior<AttackFilterModel>();
                var MindControlProj=Game.instance.model.GetTowerFromId("WizardMonkey-005").behaviors.First(a=>a.name.Equals("AttackModel_Attack Necromancer_")).Cast<AttackModel>().
                    weapons[1].projectile.Duplicate();
                MindControl.icon=new("ArchonMindControlIcon");
                MindControl.cooldown=120;
                MindControl.name="MindControl";
                MindControl.displayName="MindControl";
                MindControl.description="Completely dominates a bloons mind and forces it to fight bloons";
                MindControl.GetBehavior<CreateSoundOnAbilityModel>().sound.assetId="ArchonMindControlVO";
                MindControlAttack.RemoveBehavior<TargetCloseModel>();
                MindControlAttack.RemoveBehavior<TargetFirstModel>();
                MindControlAttack.RemoveBehavior<TargetLastModel>();
                MindControlAttack.targetProvider=new TargetStrongModel("TargetStrongModel",false,false);
                MindControlAttack.range=100;
                MindControlAttack.name="MindControl";
                MindControlAttackFilter.filters=MindControlAttackFilter.filters.AddTo(new FilterOutTagModel("FilterOutTagModelBad","Bad",null));
                MindControlAttackFilter.filters=MindControlAttackFilter.filters.AddTo(new FilterOutTagModel("FilterOutTagModelDdt","Ddt",null));
                MindControlAttack.weapons[0].rate=9999;
                MindControlAttack.weapons[0].projectile.GetDamageModel().damage=99999999;
                MindControlAttack.weapons[0].projectile.display=null;
                MindControlAttack.weapons[0].projectile.AddBehavior(new TrackTargetModel("TrackTargetModel",999,true,true,360,true,180,false,false));
                MindControlAttack.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("CreateProjectileOnContactModel",MindControlProj,new 
                    SingleEmissionModel("SingleEmissionModel",null),false,false,false));
                MindControl.GetBehavior<ActivateAttackModel>().attacks[0]=MindControlAttack;
                MindControl.GetBehavior<ActivateAttackModel>().lifespan=0.1f;
                Archon.AddBehavior(MindControl);
                SetUpgradeSounds(Archon,"ArchonUpgrade4");
            }
        }
        public class Ulrezaj:ModUpgrade<Archon>{
            public override string Name=>"Ulrezaj";
            public override string DisplayName=>"Ulrezaj";
            public override string Description=>"The combined bodies and minds of 8 Dark Templar, most nearby things simply cease to exist";
            public override int Cost=>9000;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Archon){
                GetUpgradeModel().icon=new("ArchonUlrezajIcon");
                var Lightning=Archon.GetAttackModel();
                var MindControl=Archon.GetAbility();
                Archon.range+=10;
                Lightning.range=Archon.range;
                Lightning.weapons[0].rate=0.5f;
                Lightning.weapons[0].projectile.GetDamageModel().damage+=5;
                Lightning.weapons[0].projectile.RemoveBehavior<DamageModifierForTagModel>();
                MindControl.cooldown=60;
                MindControl.RemoveBehavior(MindControl.GetBehavior<ActivateAttackModel>().attacks[0].GetBehavior<AttackFilterModel>());
                Archon.display="ArchonUlrezajPrefab";
                Archon.portrait=new("ArchonUlrezajPortrait");
            }
        }

        [HarmonyPatch(typeof(AudioFactory),"Start")]
        public class AudioFactoryStart_Patch{
            [HarmonyPostfix]
            public static void Prefix(ref AudioFactory __instance){
                __instance.RegisterAudioClip("ArchonBirth",TowerAssets.LoadAsset("ArchonBirth").Cast<AudioClip>());
                __instance.RegisterAudioClip("ArchonUpgrade",TowerAssets.LoadAsset("ArchonUpgrade").Cast<AudioClip>());
                __instance.RegisterAudioClip("ArchonUpgrade1",TowerAssets.LoadAsset("ArchonUpgrade1").Cast<AudioClip>());
                __instance.RegisterAudioClip("ArchonUpgrade2",TowerAssets.LoadAsset("ArchonUpgrade2").Cast<AudioClip>());
                __instance.RegisterAudioClip("ArchonUpgrade3",TowerAssets.LoadAsset("ArchonUpgrade3").Cast<AudioClip>());
                __instance.RegisterAudioClip("ArchonUpgrade4",TowerAssets.LoadAsset("ArchonUpgrade4").Cast<AudioClip>());
                __instance.RegisterAudioClip("ArchonMindControlVO",TowerAssets.LoadAsset("ArchonMindControlVO").Cast<AudioClip>());
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Contains("Archon")){
                    var udn=uObject.Instantiate(TowerAssets.LoadAsset(objectId).Cast<GameObject>(),__instance.PrototypeRoot).AddComponent<UnityDisplayNode>();
                    udn.transform.position=new(-3000,0);
                    udn.name="SC2Expansion-Archon";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(protos.ContainsKey(objectId)){
                    onComplete.Invoke(protos[objectId]);
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(Factory),"ProtoFlush")]
        public class FactoryProtoFlush_Patch{
            [HarmonyPostfix]
            public static void Postfix() {
                foreach (var proto in FactoryFindAndSetupPrototypeAsync_Patch.protos.Values)
                    uObject.Destroy(proto.gameObject);
                FactoryFindAndSetupPrototypeAsync_Patch.protos.Clear();
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public record ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref uImage image){
                if(reference!=null&&reference.guidRef.Contains("Archon")){
                    var text=TowerAssets.LoadAsset(reference.guidRef).Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public static class WeaponSpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.namedMonkeyKey.Contains("Archon")){
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("ArchonAttack");
                    if(__instance.attack.attackModel.name.Contains("MindControl")){
                        var MindControlProj=__instance.newProjectiles2.First().projectileModel.GetBehavior<CreateProjectileOnContactModel>().projectile;
                        MindControlProj.display=__instance.attack.target.bloon.display.displayModel.display;
                        MindControlProj.GetDamageModel().damage=__instance.attack.target.bloon.health/3;
                        MindControlProj.pierce=__instance.attack.target.bloon.health/2;
                        MindControlProj.GetBehavior<TravelAlongPathModel>().speedFrames=__instance.attack.target.bloon.bloonModel.speedFrames;
                        __instance.attack.target.bloon.childrenCreatedOut.Clear();
                    }
                }
            }
        }
    }
}