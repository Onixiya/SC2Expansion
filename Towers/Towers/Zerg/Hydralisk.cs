namespace SC2Expansion.Towers{
    public class Hydralisk{
        public static string name="Hydralisk";
        public static UpgradeModel[]GetUpgrades(){
            return new UpgradeModel[]{
                new("Grooved Spines",250,0,new("HydraliskGroovedSpinesIcon"),0,1,0,"","Grooved Spines"),
                new("Frenzy",725,0,new("HydraliskFrenzyIcon"),0,2,0,"","Frenzy"),
                new("Morph into Lurker",975,0,new("HydraliskLurkerIcon"),0,3,0,"","Morph into Lurker"),
                new("Seismic Spines",1250,0,new("HydraliskSeismicSpinesIcon"),0,4,0,"","Seismic Spines")
            };
        }
        public static(TowerModel,TowerDetailsModel,TowerModel[],UpgradeModel[])GetTower(GameModel gameModel){
            var HydraliskDetails=gameModel.towerSet[0].Clone().Cast<TowerDetailsModel>();
            HydraliskDetails.towerId=name;
            HydraliskDetails.towerIndex=1;
            if(!LocalizationManager.Instance.textTable.ContainsKey("Grooved Spines Description"))LocalizationManager.Instance.textTable.
                    Add("Grooved Spines Description","Increases attack range");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Frenzy Description"))LocalizationManager.Instance.textTable.
                    Add("Frenzy Description","Increases attack speed by 50% for 15 seconds");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Morph into Lurker Description"))LocalizationManager.Instance.textTable.
                    Add("Morph into Lurker Description","Morphs into a lurker changing the attack into a straight line that damages everything in its path");
            if(!LocalizationManager.Instance.textTable.ContainsKey("Seismec Spines Description"))LocalizationManager.Instance.textTable.
                    Add("Seismic Spines Description","Increases attack range and damage");
            return (GetT0(gameModel),HydraliskDetails,new[]{GetT0(gameModel),GetT1(gameModel),GetT2(gameModel),GetT3(gameModel),GetT4(gameModel)},GetUpgrades());
        }
        public static TowerModel GetT0(GameModel gameModel){
            var Hydralisk=gameModel.towers.First(a=>a.name.Contains("DartMonkey")).Clone().Cast<TowerModel>();
            Hydralisk.name=name;
            Hydralisk.baseId=name;
            Hydralisk.display="HydraliskPrefab";
            Hydralisk.portrait=new("HydraliskIcon");
            Hydralisk.icon=new("HydraliskIcon");
            Hydralisk.towerSet="Primary";
            Hydralisk.emoteSpriteLarge=new("Zerg");
            Hydralisk.radius=7;
            Hydralisk.cost=500;
            Hydralisk.range=35;
            Hydralisk.towerSize=TowerModel.TowerSize.XL;
            Hydralisk.footprint.ignoresPlacementCheck=true;
            Hydralisk.cachedThrowMarkerHeight=10;
            Hydralisk.areaTypes=new(1);
            Hydralisk.areaTypes[0]=AreaType.land;
            Hydralisk.upgrades=new UpgradePathModel[]{new("Grooved Spines",name+"-100")};
            var Spines=Hydralisk.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            Spines.weapons[0].name="HydraliskSpine";
            Spines.weapons[0].rate=0.7f;
            Spines.range=35;
            Spines.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Damage")).Cast<DamageModel>().damage=2;
            Hydralisk.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="HydraliskPrefab";
            return Hydralisk;
        }
        public static TowerModel GetT1(GameModel gameModel){
            var Hydralisk=gameModel.towers.First(a=>a.name.Contains("DartMonkey")).Clone().Cast<TowerModel>();
            Hydralisk.name=name+"-100";
            Hydralisk.baseId=name;
            Hydralisk.tier=100;
            Hydralisk.tiers=new int[]{1,0,0};
            Hydralisk.display="HydraliskPrefab";
            Hydralisk.portrait=new("HydraliskIcon");
            Hydralisk.icon=new("HydraliskIcon");
            Hydralisk.towerSet="Primary";
            Hydralisk.emoteSpriteLarge=new("Zerg");
            Hydralisk.radius=15;
            Hydralisk.range=45;
            Hydralisk.towerSize=TowerModel.TowerSize.XL;
            Hydralisk.footprint.ignoresPlacementCheck=true;
            Hydralisk.cachedThrowMarkerHeight=10;
            Hydralisk.areaTypes=new(1);
            Hydralisk.areaTypes[0]=AreaType.land;
            Hydralisk.appliedUpgrades=new(new[]{"Grooved Spines"});
            Hydralisk.upgrades=new[]{new UpgradePathModel("Frenzy",name+"-200")};
            var Spines=Hydralisk.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            Spines.weapons[0].name="HydraliskSpine";
            Spines.weapons[0].rate=0.7f;
            Spines.range=45;
            Spines.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Damage")).Cast<DamageModel>().damage=2;
            return Hydralisk;
        }
        public static TowerModel GetT2(GameModel gameModel){
            var Hydralisk=gameModel.towers.First(a=>a.name.Contains("DartMonkey")).Clone().Cast<TowerModel>();
            Hydralisk.name=name+"-200";
            Hydralisk.baseId=name;
            Hydralisk.tier=2;
            Hydralisk.tiers=new int[]{2,0,0};
            Hydralisk.display="HydraliskPrefab";
            Hydralisk.portrait=new("HydraliskIcon");
            Hydralisk.icon=new("HydraliskIcon");
            Hydralisk.towerSet="Primary";
            Hydralisk.emoteSpriteLarge=new("Zerg");
            Hydralisk.radius=15;
            Hydralisk.range=45;
            Hydralisk.towerSize=TowerModel.TowerSize.XL;
            Hydralisk.footprint.ignoresPlacementCheck=true;
            Hydralisk.cachedThrowMarkerHeight=10;
            Hydralisk.areaTypes=new(1);
            Hydralisk.areaTypes[0]=AreaType.land;
            Hydralisk.appliedUpgrades=new(new[]{"Grooved Spines","Frenzy"});
            Hydralisk.upgrades=new[]{new UpgradePathModel("Morph into Lurker",name+"-300")};
            var Spines=Hydralisk.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            Spines.weapons[0].name="HydraliskSpine";
            Spines.weapons[0].rate=0.7f;
            Spines.range=45;
            Spines.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Damage")).Cast<DamageModel>().damage=2;
            var Frenzy=gameModel.towers.First(a=>a.name.Equals("BoomerangMonkey-040")).behaviors.First(a=>a.name.Contains("Ability")).Cast<AbilityModel>();
            var FrenzyEffect=Frenzy.behaviors.First(a=>a.name.Contains("Turbo")).Cast<TurboModel>();
            Frenzy.name="Frenzy";
            Frenzy.displayName="Frenzy";
            Frenzy.icon=new("HydraliskFrenzyIcon");
            Frenzy.cooldown=10;
            Frenzy.behaviors.First(a=>a.name.Contains("CreateEffect")).Cast<CreateEffectOnAbilityModel>().effectModel.lifespan=15;
            FrenzyEffect.projectileDisplay=null;
            FrenzyEffect.lifespan=15;
            Hydralisk.behaviors=Hydralisk.behaviors.Add(Frenzy);
            return Hydralisk;
        }
        public static TowerModel GetT3(GameModel gameModel){
            var Hydralisk=gameModel.towers.First(a=>a.name.Contains("DartMonkey")).Clone().Cast<TowerModel>();
            Hydralisk.name=name+"-300";
            Hydralisk.baseId=name;
            Hydralisk.tier=3;
            Hydralisk.tiers=new int[]{3,0,0};
            Hydralisk.display="HydraliskLurkerPrefab";
            Hydralisk.portrait=new("HydraliskLurkerIcon");
            Hydralisk.icon=new("HydraliskLurkerIcon");
            Hydralisk.towerSet="Primary";
            Hydralisk.emoteSpriteLarge=new("Zerg");
            Hydralisk.radius=7;
            Hydralisk.range=50;
            Hydralisk.towerSize=TowerModel.TowerSize.XL;
            Hydralisk.footprint.ignoresPlacementCheck=true;
            Hydralisk.cachedThrowMarkerHeight=10;
            Hydralisk.areaTypes=new(1);
            Hydralisk.areaTypes[0]=AreaType.land;
            Hydralisk.appliedUpgrades=new(new[]{"Grooved Spines","Frenzy","Morph into Lurker"});
            Hydralisk.upgrades=new UpgradePathModel[]{new("Seismic Spines",name+"-400")};
            Hydralisk.behaviors=Hydralisk.behaviors.Remove(a => a.name.Contains("Attack"));
            Hydralisk.behaviors=Hydralisk.behaviors.Add(gameModel.towers.First(a=>a.name.Contains("WizardMonkey-030")).Clone().Cast<TowerModel>().behaviors.First(a=>a.name.Contains("Dragon")).Cast<AttackModel>());
            var Spines=Hydralisk.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            Spines.name="LurkerSpine";
            Spines.weapons[0].name="LurkerSpine";
            Spines.range=50;
            return Hydralisk;
        }
        public static TowerModel GetT4(GameModel gameModel){
            var Hydralisk=gameModel.towers.First(a=>a.name.Contains("DartMonkey")).Clone().Cast<TowerModel>();
            Hydralisk.name=name+"-400";
            Hydralisk.baseId=name;
            Hydralisk.tier=4;
            Hydralisk.tiers=new int[]{4,0,0};
            Hydralisk.display="HydraliskLurkerPrefab";
            Hydralisk.portrait=new("HydraliskLurkerIcon");
            Hydralisk.icon=new("HydraliskLurkerIcon");
            Hydralisk.towerSet="Primary";
            Hydralisk.emoteSpriteLarge=new("Zerg");
            Hydralisk.radius=7;
            Hydralisk.range=60;
            Hydralisk.towerSize=TowerModel.TowerSize.XL;
            Hydralisk.footprint.ignoresPlacementCheck=true;
            Hydralisk.cachedThrowMarkerHeight=10;
            Hydralisk.areaTypes=new(1);
            Hydralisk.areaTypes[0]=AreaType.land;
            Hydralisk.appliedUpgrades=new(new[]{"Grooved Spines","Frenzy","Morph into Lurker","Seismic Spines"});
            Hydralisk.upgrades=new(0);
            Hydralisk.behaviors=Hydralisk.behaviors.Remove(a=>a.name.Contains("Attack"));
            Hydralisk.behaviors=Hydralisk.behaviors.Add(gameModel.towers.First(a=>a.name.Contains("WizardMonkey-030")).Cast<TowerModel>().behaviors.First(a=>a.name.Contains("Dragon")).Clone().Cast<AttackModel>());
            var Spines=Hydralisk.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            Spines.name="LurkerSpine";
            Spines.weapons[0].name="LurkerSpine";
            Spines.range=60;
            return Hydralisk;
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HydraliskPrefab")){
                    var udn=GetHydralisk(__instance.PrototypeRoot,"HydraliskPrefab");
                    udn.name="Hydralisk";
                    var a=Assets.LoadAsset("HydraliskMaterial");
                    udn.genericRenderers[0].material=a.Cast<Material>();
                    udn.RecalculateGenericRenderers();
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HydraliskLurkerPrefab")) {
                    var udn=GetHydralisk(__instance.PrototypeRoot,"HydraliskLurkerPrefab");
                    udn.name="Hydralisk";
                    var a=Assets.LoadAsset("HydraliskLurkerMaterial");
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
                if(reference!=null&&reference.guidRef.Equals("HydraliskLurkerIcon")){
                    var b=Assets.LoadAsset("HydraliskLurkerIcon");
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
            }
        }
        /*[HarmonyPatch(typeof(Weapon),nameof(Weapon.SpawnDart))]
        public static class WI{
            [HarmonyPrefix]
            public static void Prefix(ref Weapon __instance)=>RunAnimations(__instance);
            private static async Task RunAnimations(Weapon __instance){
                if(__instance.weaponModel.name.Contains("HydraliskSpine")){
                    MelonLogger.Msg(__instance.attack.tower.namedMonkeyKey);
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("Armature Object|Armature ObjectAttack");
                }
            }
        }*/
    }
}