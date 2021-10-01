namespace SC2Expansion.Towers{
    public class Hydralisk:ModTower{
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>4;
        public override int MiddlePathUpgrades=>4;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Ranged Zerg shock trooper. Shoots spines";
        public override void ModifyBaseTowerModel(TowerModel Hydralisk){
            Hydralisk.display="HydraliskPrefab";
            Hydralisk.portrait=new("HydraliskPortrait");
            Hydralisk.icon=new("HydraliskIcon");
            Hydralisk.towerSet="Primary";
            Hydralisk.emoteSpriteLarge=new("Zerg");
            Hydralisk.radius=7;
            Hydralisk.range=30;
            var Spines=Hydralisk.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            Spines.name="HydraliskSpine";
            Spines.weapons[0].name="HydraliskSpine";
            Spines.weapons[0].rate=0.7f;
            Spines.range=Hydralisk.range;
            Spines.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Damage")).Cast<DamageModel>().damage=2;
            Hydralisk.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="HydraliskPrefab";
        }
        public override int GetTowerIndex(List<TowerDetailsModel>towerSet){
            return towerSet.First(model=>model.towerId==TowerType.BoomerangMonkey).towerIndex+1;
        }
        public class GroovedSpines:ModUpgrade<Hydralisk>{
            public override string Name=>"GroovedSpines";
            public override string DisplayName=>"Grooved Spines";
            public override string Description=>"Evolving small grooves into the spines increases their range";
            public override int Cost=>750;
            public override int Path=>0;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Hydralisk){
                GetUpgradeModel().icon=new("HydraliskGroovedSpinesIcon");
                var Spines=Hydralisk.behaviors.First(a=>a.name.Equals("HydraliskSpine")).Cast<AttackModel>();
                Spines.range+=10;
                Hydralisk.range=Spines.range;
            }
        }
        public class MuscularAugments:ModUpgrade<Hydralisk>{
            public override string Name=>"MuscularAugments";
            public override string DisplayName=>"Muscular Augments";
            public override string Description=>"More muscles are now used to throw spines increasing damage";
            public override int Cost=>750;
            public override int Path=>1;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Hydralisk){
                GetUpgradeModel().icon=new("HydraliskMuscularAugmentsIcon");
                var Spines=Hydralisk.behaviors.First(a=>a.name.Equals("HydraliskSpine")).Cast<AttackModel>();
                Spines.weapons[0].projectile.GetDamageModel().damage+=2;
            }
        }
        public class Frenzy:ModUpgrade<Hydralisk>{
            public override string Name=>"Frenzy";
            public override string DisplayName=>"Frenzy";
            public override string Description=>"By injecting testosterone, massive increase in agression can be done increasing attack speed for a short time";
            public override int Cost=>750;
            public override int Path=>0;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Hydralisk){
                GetUpgradeModel().icon=new("HydraliskFrenzyIcon");
                var Frenzy=Game.instance.model.towers.First(a=>a.name.Equals("BoomerangMonkey-040")).behaviors.First(a=>a.name.Contains("Ability")).Clone().Cast<AbilityModel>();
                var FrenzyEffect=Frenzy.behaviors.First(a=>a.name.Contains("Turbo")).Cast<TurboModel>();
                Frenzy.name="Frenzy";
                Frenzy.displayName="Frenzy";
                Frenzy.icon=new("HydraliskFrenzyIcon");
                Frenzy.cooldown=60;
                Frenzy.maxActivationsPerRound=1;
                FrenzyEffect.projectileDisplay=null;
                FrenzyEffect.lifespan=15;
                Hydralisk.behaviors=Hydralisk.behaviors.Add(Frenzy);
            }
        }
        //i know this belongs to the lurker but i haven't got a clue on where else to put it, lurker needs to be t3 to avoid any model issues and impalers are the t4
        public class SeismicSpines:ModUpgrade<Hydralisk>{
            public override string Name=>"SeismicSpines";
            public override string DisplayName=>"Seismic Spines";
            public override string Description=>"Even more muscles are used to throw spines, enough to break through Lead Bloons";
            public override int Cost=>750;
            public override int Path=>1;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Hydralisk){
                GetUpgradeModel().icon=new("HydraliskSeismicSpinesIcon");
                var Spines=Hydralisk.behaviors.First(a=>a.name.Equals("HydraliskSpine")).Cast<AttackModel>();
                Spines.weapons[0].projectile.GetDamageModel().damage+=2;
                Spines.weapons[0].projectile.GetDamageModel().immuneBloonProperties=0;
            }
        }
        public class Lurker:ModUpgrade<Hydralisk>{
            public override string Name=>"Lurker";
            public override string DisplayName=>"Evolve into Lurker";
            public override string Description=>"Evolves into a Lurker changing its attack into a long range attack that damages everything in its path";
            public override int Cost=>750;
            public override int Path=>1;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Hydralisk){
                GetUpgradeModel().icon=new("HydraliskLurkerIcon");
                Hydralisk.display="HydraliskLurkerPrefab";
                Hydralisk.portrait=new("HydraliskLurkerPortrait");
                Hydralisk.radius=10;
                Hydralisk.behaviors=Hydralisk.behaviors.Remove(a=>a.name.Contains("Attack"));
                Hydralisk.behaviors=Hydralisk.behaviors.Add(Game.instance.model.towers.First(a=>a.name.Contains("WizardMonkey-030")).Cast<TowerModel>().
                    behaviors.First(a=>a.name.Contains("Dragon")).Clone().Cast<AttackModel>());
                var Spines=Hydralisk.behaviors.First(a=>a.name.Equals("HydraliskSpine")).Cast<AttackModel>();
                Spines.range+=10;
                Hydralisk.range=Spines.range;
                Spines.weapons[0].projectile.display="HydraliskLurkerSpinesPrefab";
                Spines.weapons[0].projectile.pierce=9999999;
                Spines.weapons[0].projectile.GetDamageModel().damage=1;
            }
        }
        //see loader.cs for the main code on this one
        public class Primal:ModUpgrade<Hydralisk>{
            public override string Name=>"HydraliskPrimal";
            public override string DisplayName=>"Primal Evolution";
            public override string Description=>"Gains a random bonus to attack speed, damage or range";
            public override int Cost=>750;
            public override int Path=>0;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Hydralisk){
                GetUpgradeModel().icon=new("HydraliskPrimalIcon");
                Hydralisk.display="HydraliskPrimalPrefab";
            }
        }
        public class Impaler:ModUpgrade<Hydralisk>{
            public override string Name=>"Impaler";
            public override string DisplayName=>"Evolve into Impaler";
            public override string Description=>"Impalers are a mobile strain of the sunken colony dealing massive damage to single targets";
            public override int Cost=>750;
            public override int Path=>1;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Hydralisk){
                GetUpgradeModel().icon=new("HydraliskImpalerIcon");
                Hydralisk.display="HydraliskImpalerPrefab";
                Hydralisk.portrait=new("HydraliskImpalerPortrait");
                var Spines=Hydralisk.behaviors.First(a=>a.name.Equals("HydraliskSpine")).Cast<AttackModel>();
                var AttToAdd=Game.instance.model.towers.First(a=>a.name.Contains("SniperMonkey-500")).Cast<TowerModel>().behaviors.
                    First(a=>a.name.Contains("Attack")).Clone().Cast<AttackModel>();
                AttToAdd.range=Spines.range+=10;
                AttToAdd.name="HydraliskSpine";
                AttToAdd.weapons[0].projectile.display="HydraliskImpalerAttackPrefab";
                AttToAdd.weapons[0].projectile.AddBehavior(Game.instance.model.towers.First(a=>a.name.Contains("WizardMonkey-030")).behaviors.
                First(a=>a.name.Contains("Fireball")).Cast<AttackModel>().weapons[0].projectile.behaviors.First(a=>a.name.Contains("CreateEffectOnContact")).
                Cast<CreateEffectOnContactModel>());
                AttToAdd.weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.assetId="HydraliskImpalerAttackPrefab";
                Hydralisk.range=Spines.range;
                Hydralisk.RemoveBehaviors<AttackModel>();
                Hydralisk.AddBehavior(AttToAdd);
            }
        }
        public class HunterKiller:ModUpgrade<Hydralisk>{
            public override string Name=>"HunterKiller";
            public override string DisplayName=>"Hunter Killer";
            public override string Description=>"Hunter Killers are a elite strain of Hydralisks, extremely aggresive and lethal";
            public override int Cost=>750;
            public override int Path=>0;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Hydralisk){
                GetUpgradeModel().icon=new("HydraliskHunterKillerIcon");
                Hydralisk.display="HydraliskHunterKillerPrefab";
                Hydralisk.portrait=new("HydraliskHunterKillerPortrait");
                var Spines=Hydralisk.GetAttackModel();
                Spines.weapons[0].projectile.GetDamageModel().damage+=2;
                Spines.range+=5;
                Spines.weapons[0].rate-=0.3f;
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HydraliskPrefab")){
                    var udn=GetHydralisk(__instance.PrototypeRoot,"HydraliskPrefab");
                    udn.name="Hydralisk";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HydraliskLurkerPrefab")) {
                    var udn=GetHydralisk(__instance.PrototypeRoot,"HydraliskLurkerPrefab");
                    udn.name="Hydralisk";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HydraliskLurkerSpinesPrefab")) {
                    var udn=GetHydralisk(__instance.PrototypeRoot,"HydraliskLurkerSpinesPrefab");
                    udn.name="Hydralisk";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HydraliskPrimalPrefab")) {
                    var udn=GetHydralisk(__instance.PrototypeRoot,"HydraliskPrimalPrefab");
                    udn.name="Hydralisk";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HydraliskImpalerPrefab")) {
                    var udn=GetHydralisk(__instance.PrototypeRoot,"HydraliskImpalerPrefab");
                    udn.name="Hydralisk";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HydraliskImpalerAttackPrefab")) {
                    var udn=GetHydralisk(__instance.PrototypeRoot,"HydraliskImpalerAttackPrefab");
                    udn.name="Hydralisk";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HydraliskHunterKillerPrefab")) {
                    var udn=GetHydralisk(__instance.PrototypeRoot,"HydraliskHunterKillerPrefab");
                    udn.name="Hydralisk";
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
        public static UnityDisplayNode GetHydralisk(Transform transform,string model){
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
                if(reference!=null&&reference.guidRef.Equals("HydraliskIcon")){
                    var b=Assets.LoadAsset("HydraliskIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HydraliskPortrait")){
                    var b=Assets.LoadAsset("HydraliskPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HydraliskFrenzyIcon")){
                    var b=Assets.LoadAsset("HydraliskFrenzyIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HydraliskGroovedSpinesIcon")){
                    var b=Assets.LoadAsset("HydraliskGroovedSpinesIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HydraliskMuscularAugmentsIcon")){
                    var b=Assets.LoadAsset("HydraliskMuscularAugmentsIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HydraliskLurkerIcon")){
                    var b=Assets.LoadAsset("HydraliskLurkerIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HydraliskPrimalIcon")){
                    var b=Assets.LoadAsset("HydraliskPrimalIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HydraliskSeismicSpinesIcon")){
                    var b=Assets.LoadAsset("HydraliskSeismicSpinesIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HydraliskImpalerIcon")){
                    var b=Assets.LoadAsset("HydraliskImpalerIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HydraliskImpalerPortrait")){
                    var b=Assets.LoadAsset("HydraliskImpalerPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HydraliskHunterKillerIcon")){
                    var b=Assets.LoadAsset("HydraliskHunterKillerIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HydraliskHunterKillerPortrait")){
                    var b=Assets.LoadAsset("HydraliskHunterKillerPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HydraliskLurkerPortrait")){
                    var b=Assets.LoadAsset("HydraliskLurkerPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
    }
}