namespace SC2Expansion.Towers{
    public class Archon:ModTower{
        public override string DisplayName=>"Archon";
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
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
            Lightning.weapons[0]=Game.instance.model.towers.First(a=>a.name.Contains("Druid-200")).GetAttackModel().weapons[1].Duplicate();
            Lightning.weapons[0].name="ArchonLightning";
            Lightning.weapons[0].rate=0.65f;
            Lightning.weapons[0].ejectY=20;
            Lightning.weapons[0].ejectZ=20;
            Lightning.weapons[0].projectile.GetBehavior<CreateLightningEffectModel>().lifeSpan=1.25f;
            Lightning.weapons[0].projectile.GetBehavior<LightningModel>().splits=0;
            Lightning.weapons[0].projectile.GetDamageModel().damage=15;
            Lightning.weapons[0].projectile.GetDamageModel().distributeToChildren=true;
            Lightning.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel","Moabs",0,-10,false,false));
            Archon.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="ArchonPrefab";
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
                Lightning.range+=15;
                Archon.range=Lightning.range;
                Lightning.weapons[0].projectile.GetDamageModel().damage+=5;
                Lightning.weapons[0].projectile.GetBehavior<DamageModifierForTagModel>().damageAddative=-15;
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
                var PsiStorm=Game.instance.model.towers.First(a=>a.name.Contains("WizardMonkey-020")).behaviors.First(a=>a.name.Contains("Wall")).Clone().Cast<AttackModel>();
                PsiStorm.name="PsiStorm";
                PsiStorm.weapons[0].projectile.display="88399aeca4ae48a44aee5b08eb16cc61";
                PsiStorm.weapons[0].projectile.RemoveBehaviors<CreateEffectOnExhaustedModel>();
                PsiStorm.range=Archon.range;
                Archon.AddBehavior(PsiStorm);
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
                var Confusion=Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).GetAttackModel().Duplicate();
                var ConfusionProjectile=Archon.behaviors.First(a=>a.name.Contains("PsiStorm")).Cast<AttackModel>().weapons[0].projectile.Duplicate();
                Confusion.weapons[0].projectile.RemoveBehavior<DamageModel>();
                Confusion.weapons[0].projectile.display=null;
                ConfusionProjectile.RemoveBehavior<DamageModel>();
                ConfusionProjectile.AddBehavior(new WindModel("WindModel",20,150,1,false,null,0,null));
                ConfusionProjectile.GetBehavior<AgeModel>().lifespan=0.1f;
                ConfusionProjectile.display=null;
                Confusion.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("CreateProjectileOnContactModel",ConfusionProjectile,new SingleEmissionModel("SingleEmissionModel",null),
                    false,false,false));
                Archon.AddBehavior(Confusion);
            }
        }
        public class MindControl:ModUpgrade<Archon>{
            public override string Name=>"MindControl";
            public override string DisplayName=>"Mind Control";
            public override string Description=>"Completely controls the strongest target on screen excluding BAD's and forces it go back along the track";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Archon){
                GetUpgradeModel().icon=new("ArchonMindControlIcon");
                var MindControl=Game.instance.model.towers.First(a=>a.name.Contains("MonkeyBuccaneer-040")).GetAbility().Duplicate();
                var MindControlAttack=Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).GetAttackModel().Duplicate();
                var MindControlProj=Game.instance.model.towers.First(a=>a.name.Contains("WizardMonkey-005")).behaviors.First(a=>a.name.Equals("AttackModel_Attack Necromancer_")).Cast<AttackModel>().
                    weapons[1].projectile.Duplicate();
                MindControl.icon=new("ArchonMindControlIcon");
                MindControl.cooldown=1;
                MindControlAttack.weapons[0].projectile.RemoveBehavior<DamageModel>();
                MindControlAttack.weapons[0].projectile.display=null;
                MindControlAttack.weapons[0].projectile.name="MindControlCreateMoab";
                MindControlProj.name="MindControlMoab";
                MindControlAttack.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("CreateProjectileOnContactModel",MindControlProj,new 
                    SingleEmissionModel("SingleEmissionModel",null),false,false,false));
                MindControl.GetBehavior<ActivateAttackModel>().attacks[0]=MindControlAttack;
                Archon.AddBehavior(MindControl);
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
                Archon.display="ArchonUlrezajPrefab";
                Archon.portrait=new("ArchonUlrezajPortrait");
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Contains("Archon")){
                    var udn=GetArchon(__instance.PrototypeRoot,objectId);
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
        private static AssetBundle __asset;
        public static AssetBundle Assets{
            get=>__asset;
            set=>__asset=value;
        }
        public static UnityDisplayNode GetArchon(Transform transform,string model){
            var udn=Object.Instantiate(Assets.LoadAsset(model).Cast<GameObject>(),transform).AddComponent<UnityDisplayNode>();
            udn.Active=false;
            udn.transform.position=new(-3000,0);
            return udn;
        }
        [HarmonyPatch(typeof(ResourceLoader),nameof(ResourceLoader.LoadSpriteFromSpriteReferenceAsync))]
        public record ResourceLoader_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.Contains("Archon")){
                    var text=Assets.LoadAsset(reference.guidRef).Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),nameof(Weapon.SpawnDart))]
        public static class SpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance)=>RunAnimations(__instance);
            private static async Task RunAnimations(Weapon __instance){
                if(__instance.attack.tower.namedMonkeyKey.Contains("Archon")){
                    if(__instance.attack.target.bloon.HasTag("Moab")){
                        __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("ArchonAttackMoab");
                    }else{
                        __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("ArchonAttack");
                    }
                    __instance.attack.tower.Node.graphic.GetComponentInParent<AudioSource>().PlayOneShot(Assets.LoadAsset("ArchonAttack").Cast<AudioClip>(),Ext.ModVolume);
                }
            }
        }
    }
}