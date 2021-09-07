/*namespace SC2Expansion.Towers{
    public class Gateway:ModTower{
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"WizardMonkey-004";
        public override int Cost=>400;
        public override int TopPathUpgrades=>0;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Warps in Zealots. Frontline Protoss melee troops";
        public override void ModifyBaseTowerModel(TowerModel Gateway){
            Gateway.display="GatewayPrefab";
            Gateway.portrait=new("GatewayIcon");
            Gateway.icon=new("GatewayIcon");
            Gateway.emoteSpriteLarge=new("Protoss");
            Gateway.radius=15;
            Gateway.range=20;
            Gateway.behaviors=Gateway.behaviors.Remove(a=>a.name.Contains("Shimmer"));
            Gateway.behaviors=Gateway.behaviors.Remove(a=>a.name.Equals("AttackModel_Attack_"));
            var ZealotWarp=Gateway.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            var ZealotWarpZone=Gateway.behaviors.First(a=>a.name.Contains("Zone")).Cast<NecromancerZoneModel>();
            var GhostMoabPath=Game.instance.model.GetTowerFromId("WizardMonkey-005").Cast<TowerModel>().behaviors.
                First(a=>a.name.Equals("AttackModel_Attack Necromancer_")).Clone().Cast<AttackModel>().weapons[1].projectile.behaviors.First(a=>a.name.Contains("Path")).Cast<TravelAlongPathModel>();
            GhostMoabPath.speedFrames=0.5f;
            ZealotWarp.weapons[0].projectile.behaviors=ZealotWarp.weapons[0].projectile.behaviors.Remove(a=>a.name.Contains("Path"));
            ZealotWarp.weapons[0].projectile.behaviors=ZealotWarp.weapons[0].projectile.behaviors.Add(GhostMoabPath);
            ZealotWarp.weapons[0].projectile.display="GatewayZealotPrefab";
            ZealotWarp.weapons[0].emission.Cast<NecromancerEmissionModel>().maxRbeStored=2147483646;
            ZealotWarp.weapons[0].emission.Cast<NecromancerEmissionModel>().maxPiercePerBloon=40;
            ZealotWarp.weapons[0].rate=5.5f;
            ZealotWarp.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().lifespanFrames=2147483646;
            ZealotWarp.weapons[0].projectile.GetDamageModel().damage=1;
            ZealotWarp.weapons[0].projectile.radius=5;
            ZealotWarp.name="ZealotWarp";
            ZealotWarpZone.attackUsedForRangeModel.range=999;
            ZealotWarpZone.name="ZealotWarpZone";
            Gateway.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="GatewayPrefab";
        }
        public override int GetTowerIndex(List<TowerDetailsModel> towerSet) {
            return towerSet.First(model => model.towerId==TowerType.BoomerangMonkey).towerIndex+1;
        }
        public class CentrifugalHooks:ModUpgrade<Gateway> {
            public override string Name=>"CentrifugalHooks";
            public override string DisplayName=>"Centrifugal Hooks";
            public override string Description=>"Evolving powerful hooks allows Banelings to roll and move faster";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Gateway){
                GetUpgradeModel().icon=new("GatewayCentrifugalHooksIcon");
                var ZealotWarp=Gateway.behaviors.First(a=>a.name.Equals("ZealotWarp")).Cast<AttackModel>();
                ZealotWarp.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().speedFrames=0.7f;
                ZealotWarp.weapons[0].projectile.display="GatewayBanelingRollPrefab";
            }
        }
        public class Rupture:ModUpgrade<Gateway> {
            public override string Name=>"Rupture";
            public override string DisplayName=>"Rupture";
            public override string Description=>"Adding more volatile chemicals and more internel pressure increases damage and radius";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Gateway) {
                GetUpgradeModel().icon=new("GatewayRuptureIcon");
                var ZealotWarp=Gateway.behaviors.First(a=>a.name.Equals("ZealotWarp")).Cast<AttackModel>();
                ZealotWarp.weapons[0].projectile.GetDamageModel().damage=6;
                ZealotWarp.weapons[0].projectile.radius=7;
            }
        }
        public class CorrosiveAcid:ModUpgrade<Gateway> {
            public override string Name=>"CorrosiveAcid";
            public override string DisplayName=>"Corrosive Acid";
            public override string Description=>"Increasing the water content of the acid allows a small amount to be left on the track that strips camo and fortification on non Moabs";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Gateway) {
                GetUpgradeModel().icon=new("GatewayCorrosiveAcidIcon");
                var ZealotWarp=Gateway.behaviors.First(a=>a.name.Equals("ZealotWarp")).Cast<AttackModel>();
                ZealotWarp.weapons[0].projectile.behaviors=ZealotWarp.weapons[0].projectile.behaviors.Add(Game.instance.model.towers.
                    First(a=>a.name.Contains("EngineerMonkey-030")).Cast<TowerModel>().behaviors.First(a=>a.name.Contains("CleansingFoam")).Cast<AttackModel>()
                    .weapons[0].projectile.behaviors.First(a=>a.name.Contains("Exhaust")));
                ZealotWarp.weapons[0].projectile.display="GatewayBaneling3Prefab";
                var AcidPool=ZealotWarp.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CreateProj")).Cast<CreateProjectileOnExhaustFractionModel>();
                AcidPool.projectile.behaviors.First(a=>a.name.Contains("Modifier")).Cast<RemoveBloonModifiersModel>().cleanseFortified=true;
                AcidPool.projectile.behaviors.First(a=>a.name.Contains("Modifier")).Cast<RemoveBloonModifiersModel>().cleanseLead=false;
                AcidPool.projectile.behaviors.First(a=>a.name.Contains("Modifier")).Cast<RemoveBloonModifiersModel>().blacklistTagModFilter.Add("Moabs");
            }
        }
        public class Splitter:ModUpgrade<Gateway> {
            public override string Name=>"BanelingRateIncrease";
            public override string DisplayName=>"Growth Enzymes";
            public override string Description=>"Adding more nutrients when Banelings are evolving lets them grow faster and bigger";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Gateway){
                GetUpgradeModel().icon=new("GatewayRateIncreaseIcon");
                var ZealotWarp=Gateway.behaviors.First(a=>a.name.Equals("ZealotWarp")).Cast<AttackModel>();
                ZealotWarp.weapons[0].rate=2;
                ZealotWarp.weapons[0].projectile.GetDamageModel().damage=9;
            }
        }
        public class Kaboomer:ModUpgrade<Gateway>{
            public override string Name=>"Kaboomer";
            public override string DisplayName=>"Kaboomer";
            public override string Description=>"Kaboomers are a heavy strain of Banelings that do massive damage";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Gateway){
                GetUpgradeModel().icon=new("GatewayKaboomerIcon");
                var ZealotWarp=Gateway.behaviors.First(a=>a.name.Equals("ZealotWarp")).Cast<AttackModel>();
                ZealotWarp.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().speedFrames=0.4f;
                ZealotWarp.weapons[0].rate=6;
                ZealotWarp.weapons[0].projectile.GetDamageModel().damage=100;
                ZealotWarp.weapons[0].projectile.display="GatewayKaboomerPrefab";
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("GatewayPrefab")){
                    var udn=GetGateway(__instance.PrototypeRoot,"GatewayPrefab");
                    udn.name="SC2Expansion-Gateway";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("GatewayZealotPrefab")){
                    var udn=GetGateway(__instance.PrototypeRoot,"GatewayZealotPrefab");
                    udn.name="SC2Expansion-Gateway";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("GatewaySentinelPrefab")) {
                    var udn=GetGateway(__instance.PrototypeRoot,"GatewaySentinelPrefab");
                    udn.name="SC2Expansion-Gateway";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("GatewaySolaritePrefab")) {
                    var udn=GetGateway(__instance.PrototypeRoot,"GatewaySolaritePrefab");
                    udn.name="SC2Expansion-Gateway";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("GatewayLegionnairePrefab")) {
                    var udn=GetGateway(__instance.PrototypeRoot,"GatewayLegionnairePrefab");
                    udn.name="SC2Expansion-Gateway";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("GatewayKaldalisPrefab")) {
                    var udn=GetGateway(__instance.PrototypeRoot,"GatewayKaldalisPrefab");
                    udn.name="SC2Expansion-Gateway";
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
        public static UnityDisplayNode GetGateway(Transform transform,string model){
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
                if(reference!=null&&reference.guidRef.Equals("GatewayIcon")){
                    var b=Assets.LoadAsset("GatewayIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("GatewayChargeIcon")){
                    var b=Assets.LoadAsset("GatewayChargeIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("GatewayCentrifugalHooksIcon")){
                    var b=Assets.LoadAsset("GatewayCentrifugalHooksIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("GatewayCorrosiveAcidIcon")){
                    var b=Assets.LoadAsset("GatewayCorrosiveAcidIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("GatewayKaboomerIcon")){
                    var b=Assets.LoadAsset("GatewayKaboomerIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("GatewayRateIncreaseIcon")){
                    var b=Assets.LoadAsset("GatewayRateIncreaseIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
    }
}*/