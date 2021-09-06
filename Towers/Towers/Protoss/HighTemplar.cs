﻿namespace SC2Expansion.Towers{
    public class HighTemplar:ModTower{
        public override string DisplayName=>"High Templar";
        public override string TowerSet=>MAGIC;
        public override string BaseTower=>"WizardMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Ranged Protoss Caster";
        public override void ModifyBaseTowerModel(TowerModel HighTemplar){
            HighTemplar.display="HighTemplarPrefab";
            HighTemplar.portrait=new("HighTemplarIcon");
            HighTemplar.icon=new("HighTemplarIcon");
            HighTemplar.emoteSpriteLarge=new("Protoss");
            HighTemplar.radius=7;
            HighTemplar.range=40;
            var PsiBolt=HighTemplar.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            PsiBolt.name="HighTemplarPsiBolt";
            PsiBolt.range=40;
            HighTemplar.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="HighTemplarPrefab";
        }
        public override int GetTowerIndex(List<TowerDetailsModel>towerSet){
            return towerSet.First(model=>model.towerId==TowerType.BoomerangMonkey).towerIndex+1;
        }
        public class KhaydarinAmulet:ModUpgrade<HighTemplar>{
            public override string Name=>"KhaydarinAmulet";
            public override string DisplayName=>"Khaydarin Amulet";
            public override string Description=>"Khaydarin crystals help regulate the flow of energy from the Khala, letting the High Templar fire further";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel HighTemplar){
                GetUpgradeModel().icon=new("HighTemplarKhaydarinAmuletIcon");
                var Psibolt=HighTemplar.GetAttackModel();
                Psibolt.range+=10;
                HighTemplar.range=Psibolt.range;
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
                var PsiStorm=Game.instance.model.towers.First(a=>a.name.Contains("WizardMonkey-020")).behaviors.First(a=>a.name.Contains("Wall")).Clone().Cast<AttackModel>();
                PsiStorm.name="PsiStorms";
                //PsiStorm.weapons[0].projectile.display="88399aeca4ae48a44aee5b08eb16cc61";
                PsiStorm.weapons[0].projectile.RemoveBehaviors<CreateEffectOnExhaustedModel>();
                HighTemplar.AddBehavior(PsiStorm);
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
                var PsiStorm=HighTemplar.behaviors.First(a=>a.name.Contains("PsiStorms")).Cast<AttackModel>();
                PsiStorm.weapons[0].projectile.radius=50;
                PsiStorm.weapons[0].projectile.GetDamageModel().damage+=1;
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
                var PsiOrb=Game.instance.model.towers.First(a=>a.name.Equals("Druid-400")).behaviors.First(a=>a.name.Contains("Attack")).Clone().Cast<AttackModel>();
                //i would love to use adoras sacrifice thing but afaik, it would require assembly editing to get it to not crash
                var Sacrifice=Game.instance.model.towers.First(a=>a.name.Contains("MonkeyBuccaneer-040")).behaviors.First(a=>a.name.Contains("Take")).Clone().Cast<AbilityModel>();
                //originally was directly copying pats squeeze ability but idfk how to make the bloons not go to the tower and stay on the track
                var MindBlast=Game.instance.model.towers.First(a=>a.name.Equals("PatFusty 10")).behaviors.First(a=>a.name.Contains("Big")).Clone().Cast<AbilityModel>();
                //compiler didn't like me using a enumerator on it, next best thing ig
                PsiOrb.weapons=PsiOrb.weapons.Remove(a=>a.name.Equals("WeaponModel_Weapon"));
                PsiOrb.weapons=PsiOrb.weapons.Remove(a=>a.name.Equals("WeaponModel_Tornado"));
                PsiOrb.weapons=PsiOrb.weapons.Remove(a=>a.name.Equals("WeaponModel_Lightning"));
                var temp=PsiOrb.weapons[0].projectile.behaviors.GetEnumerator();
                PsiOrb.name="HighTemplarPsiOrb";
                Sacrifice.name="Sacrifice";
                Sacrifice.displayName="Sacrifice";
                Sacrifice.cooldown=60f;
                var AtkSpd=Game.instance.model.towers.First(a=>a.name.Equals("BoomerangMonkey-040")).behaviors.First(a=>a.name.Contains("Ability")).Cast<AbilityModel>().behaviors.
                    First(a=>a.name.Contains("Turbo")).Clone().Cast<TurboModel>();
                AtkSpd.extraDamage=6;
                AtkSpd.projectileDisplay.assetPath=null;
                Sacrifice.icon=new("HighTemplarSacrificeIcon");
                Sacrifice.behaviors.First(a=>a.name.Contains("Activate")).Cast<ActivateAttackModel>().attacks[0].weapons[0].projectile.behaviors.
                    First(a=>a.name.Contains("RopeEffect")).Cast<CreateRopeEffectModel>().assetId=null;
                Sacrifice.behaviors.First(a=>a.name.Contains("Activate")).Cast<ActivateAttackModel>().attacks[0].weapons[0].projectile.behaviors.
                    First(a=>a.name.Contains("RopeEffect")).Cast<CreateRopeEffectModel>().endAssetId=null;
                Sacrifice.maxActivationsPerRound=1;
                Sacrifice.behaviors=Sacrifice.behaviors.Add(AtkSpd);
                MindBlast.name="MindBlast";
                MindBlast.displayName="Mind Blast";
                MindBlast.behaviors.First(a=>a.name.Contains("Activate")).Cast<ActivateAttackModel>().attacks[0]=
                    Game.instance.model.towers.First(a=>a.name.Equals("SniperMonkey-500")).behaviors.First(a=>a.name.Contains("Attack")).Clone().Cast<AttackModel>();
                MindBlast.behaviors.First(a=>a.name.Contains("Activate")).Cast<ActivateAttackModel>().attacks[0].weapons[0].projectile.behaviors.
                    First(a=>a.name.Contains("DamageModel")).Cast<DamageModel>().damage=400;
                MindBlast.cooldown=80f;
                MindBlast.icon=new("HighTemplarMindBlastIcon");
                MindBlast.maxActivationsPerRound=1;
                HighTemplar.display="HighTemplarAscendantPrefab";
                HighTemplar.behaviors=HighTemplar.behaviors.Remove(a=>a.name.Contains("PsiStorm"));
                HighTemplar.behaviors=HighTemplar.behaviors.Add(MindBlast,Sacrifice,PsiOrb);
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
                var PsiOrb=HighTemplar.behaviors.First(a=>a.name.Contains("PsiOrb")).Cast<AttackModel>();
                var Sacrifice=HighTemplar.behaviors.First(a=>a.name.Contains("Sacrifice")).Cast<AbilityModel>();
                var SacrificeAttack=Sacrifice.behaviors.First(a=>a.name.Contains("Activate")).Cast<ActivateAttackModel>().attacks[0];
                SacrificeAttack.behaviors.First(a=>a.name.Contains("Filter")).Cast<AttackFilterModel>();
                var MindBlast=HighTemplar.behaviors.First(a=>a.name.Contains("MindBlast")).Cast<AbilityModel>();
                var PsiBolt=HighTemplar.behaviors.First(a=>a.name.Contains("PsiBolt")).Cast<AttackModel>();
                PsiOrb.weapons[0].rate=0.8f;
                MindBlast.cooldown=50;
                Sacrifice.cooldown=40;
                MindBlast.maxActivationsPerRound=-1;
                Sacrifice.maxActivationsPerRound=-1;
                SacrificeAttack.behaviors.First(a=>a.name.Contains("Grapp")).Cast<TargetGrapplableModel>().canHitZomg=true;
                SacrificeAttack.behaviors=SacrificeAttack.behaviors.Remove(a=>a.name.Contains("AttackFilterModel"));
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HighTemplarPrefab")){
                    var udn=GetHighTemplar(__instance.PrototypeRoot,"HighTemplarPrefab");
                    udn.name="HighTemplar";
                    var a=Assets.LoadAsset("HighTemplarMaterial");
                    udn.genericRenderers[0].material=a.Cast<Material>();
                    udn.RecalculateGenericRenderers();
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HighTemplarAscendantPrefab")){
                    var udn=GetHighTemplar(__instance.PrototypeRoot,"HighTemplarAscendantPrefab");
                    udn.name="HighTemplar";
                    var a=Assets.LoadAsset("HighTemplarAscendantMaterial");
                    udn.genericRenderers[0].material=a.Cast<Material>();
                    udn.RecalculateGenericRenderers();
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HighTemplarJinaraPrefab")){
                    var udn=GetHighTemplar(__instance.PrototypeRoot,"HighTemplarJinaraPrefab");
                    udn.name="HighTemplar";
                    var a=Assets.LoadAsset("HighTemplarJinaraMaterial");
                    udn.genericRenderers[0].material=a.Cast<Material>();
                    udn.RecalculateGenericRenderers();
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
        public static UnityDisplayNode GetHighTemplar(Transform transform,string model){
            var udn=Object.Instantiate(Assets.LoadAsset(model).Cast<GameObject>(),transform).AddComponent<UnityDisplayNode>();
            udn.Active=false;
            udn.transform.position=new(-3000,0);
            return udn;
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.ProtoFlush))]
        public class PrototypeFlushUDN_Patch{
            [HarmonyPostfix]
            public static void Postfix(){
                foreach(var proto in PrototypeUDN_Patch.protos.Values)Object.Destroy(proto.gameObject);
                PrototypeUDN_Patch.protos.Clear();
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),nameof(ResourceLoader.LoadSpriteFromSpriteReferenceAsync))]
        public record ResourceLoader_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.Equals("HighTemplarIcon")){
                    var b=Assets.LoadAsset("HighTemplarIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HighTemplarPsiStormIcon")){
                    var b=Assets.LoadAsset("HighTemplarPsiStormIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HighTemplarPlasmaSurgeIcon")){
                    var b=Assets.LoadAsset("HighTemplarPlasmaSurgeIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HighTemplarKhaydarinAmuletIcon")){
                    var b=Assets.LoadAsset("HighTemplarKhaydarinAmuletIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HighTemplarAscendantIcon")){
                    var b=Assets.LoadAsset("HighTemplarAscendantIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HighTemplarAscendantPortrait")) {
                    var b=Assets.LoadAsset("HighTemplarAscendantPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HighTemplarMindBlastIcon")){
                    var b=Assets.LoadAsset("HighTemplarMindBlastIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HighTemplarSacrificeIcon")){
                    var b=Assets.LoadAsset("HighTemplarSacrificeIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HighTemplarJinaraPortrait")){
                    var b=Assets.LoadAsset("HighTemplarJinaraPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
        /*[HarmonyPatch(typeof(Weapon),nameof(Weapon.SpawnDart))]
        public static class WI{
        [HarmonyPrefix]
        public static void Prefix(ref Weapon __instance)=>RunAnimations(__instance);
        private static async Task RunAnimations(Weapon __instance){
                MelonLogger.Msg("test");
                var temp=__instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().runtimeAnimatorController.animationClips.GetEnumerator();
                while(temp.MoveNext()){
                    MelonLogger.Msg("name: "+temp.Current.name);
                }
                __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("Armature Object|Armature ObjectSpell");
                MelonLogger.Msg("test1");
                __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack",true);
                var wait=2300f;
                await Task.Run(()=>{
                    while(wait>0){
                        wait-=TimeManager.timeScaleWithoutNetwork+1;
                        Task.Delay(1);
                    }
                    return;
                });
                __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack",false);
            }
        }*/
    }
}