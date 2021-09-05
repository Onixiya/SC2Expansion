namespace SC2Expansion.Towers{
    public class HighTemplar{
        public static string name="High Templar";
        public static UpgradeModel[]GetUpgrades(){
            return new UpgradeModel[]{
                new("Khaydarin Amulet",300,0,new("HighTemplarKhaydarinAmuletIcon"),0,1,0,"","Khaydarin Amulet"),
                new("Psi Storms",950,0,new("HighTemplarPsiStormIcon"),0,2,0,"","Psi Storms"),
                new("Plasma Surge",1125,0,new("HighTemplarPlasmaSurgeIcon"),0,3,0,"","Plasma Surge"),
                new("Ascendant",1500,0,new("HighTemplarAscendantIcon"),0,4,0,"","Ascendant")
            };
        }
        public static(TowerModel,TowerDetailsModel,TowerModel[],UpgradeModel[])GetTower(GameModel gameModel){
            var HighTemplarDetails=gameModel.towerSet[0].Clone().Cast<TowerDetailsModel>();
            HighTemplarDetails.towerId=name;
            HighTemplarDetails.towerIndex=0;
            if(!LocalizationManager.Instance.textTable.ContainsKey("Khaydarin Amulet Description"))LocalizationManager.Instance.textTable.
                    Add("Khaydarin Amulet Description","Increases attack range");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Psi Storms Description"))LocalizationManager.Instance.textTable.
                    Add("Psi Storms Description","Casts mini Psi Storms into the track damaging everything that goes through it");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Plasma Surge Description"))LocalizationManager.Instance.textTable.
                    Add("Plasma Surge Description","Psi Storms are bigger and do more damage");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Ascendant Description"))LocalizationManager.Instance.textTable.
                    Add("Ascendant Description","Gains the Sacrifice and Mind Blast abilites. Replaces Psi Storms with Psionic Orb");
            return (GetT0(gameModel),HighTemplarDetails,new[]{GetT0(gameModel),GetT1(gameModel),GetT2(gameModel),GetT3(gameModel),GetT4(gameModel)},GetUpgrades());
        }
        public static TowerModel GetT0(GameModel gameModel){
            var HighTemplar=gameModel.towers.First(a=>a.name.Contains("WizardMonkey")).Clone().Cast<TowerModel>();
            HighTemplar.name=name;
            HighTemplar.baseId=name;
            HighTemplar.display="HighTemplarPrefab";
            HighTemplar.portrait=new("HighTemplarPortrait");
            HighTemplar.icon=new("HighTemplarPortrait");
            HighTemplar.towerSet="Magic";
            HighTemplar.emoteSpriteLarge=new("Protoss");
            HighTemplar.radius=7;
            HighTemplar.cost=500;
            HighTemplar.range=40;
            HighTemplar.towerSize=TowerModel.TowerSize.XL;
            HighTemplar.footprint.ignoresPlacementCheck=true;
            HighTemplar.cachedThrowMarkerHeight=10;
            HighTemplar.areaTypes=new(1);
            HighTemplar.areaTypes[0]=AreaType.land;
            HighTemplar.upgrades=new UpgradePathModel[]{new("Khaydarin Amulet",name+"-100")};
            var PsiBolt=HighTemplar.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            PsiBolt.weapons[0].projectile.name="HighTemplarPsiBolt";
            PsiBolt.range=40;
            HighTemplar.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="HighTemplarPrefab";
            return HighTemplar;
        }
        public static TowerModel GetT1(GameModel gameModel){
            var HighTemplar=gameModel.towers.First(a=>a.name.Contains("WizardMonkey")).Clone().Cast<TowerModel>();
            HighTemplar.name=name+"-100";
            HighTemplar.baseId=name;
            HighTemplar.tier=100;
            HighTemplar.tiers=new int[]{1,0,0};
            HighTemplar.display="HighTemplarPrefab";
            HighTemplar.portrait=new("HighTemplarPortrait");
            HighTemplar.icon=new("HighTemplarPortrait");
            HighTemplar.towerSet="Magic";
            HighTemplar.emoteSpriteLarge=new("Protoss");
            HighTemplar.radius=15;
            HighTemplar.range=52;
            HighTemplar.towerSize=TowerModel.TowerSize.XL;
            HighTemplar.footprint.ignoresPlacementCheck=true;
            HighTemplar.cachedThrowMarkerHeight=10;
            HighTemplar.areaTypes=new(1);
            HighTemplar.areaTypes[0]=AreaType.land;
            HighTemplar.appliedUpgrades=new(new[]{"Khaydarin Amulet"});
            HighTemplar.upgrades=new[]{new UpgradePathModel("Psi Storms",name+"-200")};
            var PsiBolt=HighTemplar.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            PsiBolt.weapons[0].projectile.name="HighTemplarPsiBolt";
            PsiBolt.range=52;
            return HighTemplar;
        }
        public static TowerModel GetT2(GameModel gameModel){
            var HighTemplar=gameModel.towers.First(a=>a.name.Contains("WizardMonkey-020")).Clone().Cast<TowerModel>();
            HighTemplar.name=name+"-200";
            HighTemplar.baseId=name;
            HighTemplar.tier=200;
            HighTemplar.tiers=new int[]{2,0,0};
            HighTemplar.display="HighTemplarPrefab";
            HighTemplar.portrait=new("HighTemplarPortrait");
            HighTemplar.icon=new("HighTemplarPortrait");
            HighTemplar.towerSet="Magic";
            HighTemplar.emoteSpriteLarge=new("Protoss");
            HighTemplar.radius=15;
            HighTemplar.range=52;
            HighTemplar.towerSize=TowerModel.TowerSize.XL;
            HighTemplar.footprint.ignoresPlacementCheck=true;
            HighTemplar.cachedThrowMarkerHeight=10;
            HighTemplar.areaTypes=new(1);
            HighTemplar.areaTypes[0]=AreaType.land;
            HighTemplar.appliedUpgrades=new(new[]{"Khaydarin Amulet","Psi Storms"});
            HighTemplar.upgrades=new[]{new UpgradePathModel("Plasma Surge",name+"-300")};
            var PsiStorm=HighTemplar.behaviors.First(a=>a.name.Contains("Wall")).Cast<AttackModel>();
            PsiStorm.weapons[0].projectile.name="HighTemplarMiniPsiStorm";
            PsiStorm.weapons[0].projectile.display="88399aeca4ae48a44aee5b08eb16cc61";
            PsiStorm.weapons[0].projectile.behaviors=PsiStorm.weapons[0].projectile.behaviors.Remove(a=>a.name.Contains("CreateEffectOnExhausted"));
            var PsiBolt=HighTemplar.behaviors.First(a=>a.name.Equals("AttackModel_Attack_")).Cast<AttackModel>();
            PsiBolt.weapons[0].projectile.name="HighTemplarPsiBolt";
            PsiBolt.range=52;
            HighTemplar.behaviors=HighTemplar.behaviors.Remove(a=>a.name.Contains("Fireball"));
            return HighTemplar;
        }
        public static TowerModel GetT3(GameModel gameModel){
            var HighTemplar=gameModel.towers.First(a=>a.name.Contains("WizardMonkey-020")).Clone().Cast<TowerModel>();
            HighTemplar.name=name+"-300";
            HighTemplar.baseId=name;
            HighTemplar.tier=300;
            HighTemplar.tiers=new int[]{3,0,0};
            HighTemplar.display="HighTemplarPrefab";
            HighTemplar.portrait=new("HighTemplarPortrait");
            HighTemplar.icon=new("HighTemplarPortrait");
            HighTemplar.towerSet="Magic";
            HighTemplar.emoteSpriteLarge=new("Protoss");
            HighTemplar.radius=15;
            HighTemplar.range=52;
            HighTemplar.towerSize=TowerModel.TowerSize.XL;
            HighTemplar.footprint.ignoresPlacementCheck=true;
            HighTemplar.cachedThrowMarkerHeight=10;
            HighTemplar.areaTypes=new(1);
            HighTemplar.areaTypes[0]=AreaType.land;
            HighTemplar.appliedUpgrades=new(new[]{"Khaydarin Amulet","Psi Storms","Plasma Surge"});
            HighTemplar.upgrades=new[]{new UpgradePathModel("Ascendant",name+"-400")};
            var PsiStorm=HighTemplar.behaviors.First(a=>a.name.Contains("Wall")).Cast<AttackModel>();
            PsiStorm.weapons[0].projectile.name="HighTemplarMiniPsiStorm";
            PsiStorm.weapons[0].projectile.display="88399aeca4ae48a44aee5b08eb16cc61";
            PsiStorm.weapons[0].projectile.behaviors=PsiStorm.weapons[0].projectile.behaviors.Remove(a=>a.name.Contains("CreateEffectOnExhausted"));
            PsiStorm.weapons[0].projectile.radius=50;
            PsiStorm.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Damage")).Cast<DamageModel>().damage=2;
            var PsiBolt=HighTemplar.behaviors.First(a=>a.name.Equals("AttackModel_Attack_")).Cast<AttackModel>();
            PsiBolt.weapons[0].projectile.name="HighTemplarPsiBolt";
            PsiBolt.range=52;
            HighTemplar.behaviors=HighTemplar.behaviors.Remove(a=>a.name.Equals("AttackModel_Attack Fireball_"));
            return HighTemplar;
        }
        public static TowerModel GetT4(GameModel gameModel){
            var HighTemplar=gameModel.towers.First(a=>a.name.Contains("WizardMonkey")).Clone().Cast<TowerModel>();
            HighTemplar.name=name+"-400";
            HighTemplar.baseId=name;
            HighTemplar.tier=400;
            HighTemplar.tiers=new int[]{4,0,0};
            HighTemplar.display="HighTemplarAscendantPrefab";
            HighTemplar.portrait=new("HighTemplarAscendantPortrait");
            HighTemplar.icon=new("HighTemplarAscendantIcon");
            HighTemplar.towerSet="Magic";
            HighTemplar.emoteSpriteLarge=new("Protoss");
            HighTemplar.radius=15;
            HighTemplar.range=52;
            HighTemplar.towerSize=TowerModel.TowerSize.XL;
            HighTemplar.footprint.ignoresPlacementCheck=true;
            HighTemplar.cachedThrowMarkerHeight=10;
            HighTemplar.areaTypes=new(1);
            HighTemplar.areaTypes[0]=AreaType.land;
            HighTemplar.appliedUpgrades=new(new[]{"Khaydarin Amulet","Psi Storms","Plasma Surge","Ascendant"});
            HighTemplar.upgrades=new(0);
            var PsiOrb=gameModel.towers.First(a=>a.name.Equals("Druid-400")).behaviors.First(a=>a.name.Contains("Attack")).Clone().Cast<AttackModel>();
            var PsiBolt=HighTemplar.behaviors.First(a=>a.name.Equals("AttackModel_Attack_")).Cast<AttackModel>();
            //i would love to use adoras sacrifice thing but afaik, it would require assembly editing to get it to not crash
            var Sacrifice=gameModel.towers.First(a=>a.name.Contains("MonkeyBuccaneer-040")).behaviors.First(a=>a.name.Contains("Take")).Clone().Cast<AbilityModel>();
            //originally was directly copying pats squeeze ability but idfk how to make the bloons not go to the tower and stay on the track
            var MindBlast=gameModel.towers.First(a=>a.name.Equals("PatFusty 10")).behaviors.First(a=>a.name.Contains("Big")).Clone().Cast<AbilityModel>();
            PsiOrb.weapons[0].projectile.name="HighTemplarPsiOrb";
            Sacrifice.name="Sacrifice";
            Sacrifice.displayName="Sacrifice";
            Sacrifice.cooldown=0.1f;
            var temp=gameModel.towers.First(a=>a.name.Equals("BoomerangMonkey-040")).behaviors.First(a=>a.name.Contains("Ability")).Cast<AbilityModel>().behaviors.
                First(a=>a.name.Contains("Turbo")).Clone().Cast<TurboModel>();
            temp.extraDamage=6;
            temp.projectileDisplay.assetPath=null;
            Sacrifice.icon=new("HighTemplarSacrificeIcon");
            Sacrifice.behaviors.First(a=>a.name.Contains("Activate")).Cast<ActivateAttackModel>().attacks[0].weapons[0].projectile.behaviors.
                First(a=>a.name.Contains("RopeEffect")).Cast<CreateRopeEffectModel>().assetId=null;
            Sacrifice.behaviors.First(a=>a.name.Contains("Activate")).Cast<ActivateAttackModel>().attacks[0].weapons[0].projectile.behaviors.
                First(a=>a.name.Contains("RopeEffect")).Cast<CreateRopeEffectModel>().endAssetId=null;
            Sacrifice.behaviors=Sacrifice.behaviors.Add(temp);
            PsiBolt.weapons[0].projectile.name="HighTemplarPsiBolt";
            PsiBolt.range=52;
            PsiBolt.weapons[0].projectile.behaviors.First(a=>a.name.Contains("DamageModel")).Cast<DamageModel>().damage=2;
            MindBlast.name="Mind Blast";
            MindBlast.displayName="Mind Blast";
            MindBlast.behaviors.First(a=>a.name.Contains("Activate")).Cast<ActivateAttackModel>().attacks[0]=
                gameModel.towers.First(a=>a.name.Equals("SniperMonkey-500")).behaviors.First(a=>a.name.Contains("Attack")).Clone().Cast<AttackModel>();
            MindBlast.behaviors.First(a=>a.name.Contains("Activate")).Cast<ActivateAttackModel>().attacks[0].weapons[0].projectile.behaviors.
                First(a=>a.name.Contains("DamageModel")).Cast<DamageModel>().damage=400;
            MindBlast.cooldown=5f;
            MindBlast.icon=new("HighTemplarMindBlastIcon");
            MindBlast.maxActivationsPerRound=1;
            HighTemplar.behaviors=HighTemplar.behaviors.Remove(a=>a.name.Equals("AttackModel_Attack Fireball_"));
            HighTemplar.behaviors=HighTemplar.behaviors.Add(MindBlast,Sacrifice,PsiOrb);
            return HighTemplar;
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
                if(reference!=null&&reference.guidRef.Equals("HighTemplarPortrait")){
                    var b=Assets.LoadAsset("HighTemplarPortrait");
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