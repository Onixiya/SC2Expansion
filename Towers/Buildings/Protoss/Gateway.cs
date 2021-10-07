namespace SC2Expansion.Towers{
    public class Gateway:ModTower{
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"WizardMonkey-005";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Warps in Zealots. Frontline Protoss melee troops";
        public override void ModifyBaseTowerModel(TowerModel Gateway){
            Gateway.display="GatewayPrefab";
            Gateway.portrait=new("GatewayPortrait");
            Gateway.icon=new("GatewayIcon");
            Gateway.emoteSpriteLarge=new("Protoss");
            Gateway.radius=30;
            Gateway.range=31;
            Gateway.behaviors=Gateway.behaviors.Remove(a=>a.name.Contains("Shimmer"));
            Gateway.behaviors=Gateway.behaviors.Remove(a=>a.name.Equals("AttackModel_Attack_"));
            Gateway.behaviors=Gateway.behaviors.Remove(a=>a.name.Contains("Buff"));
            var ZealotWarp=Gateway.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            ZealotWarp.weapons[1].projectile.display="GatewayZealotPrefab";
            ZealotWarp.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().minPiercePerBloon=3;
            //tried removing the first weapon entirely, it didn't like it at all and kept crashing, setting maxrbespawned prevents the normal necrobloons from spawning at all
            ZealotWarp.weapons[0].emission.Cast<NecromancerEmissionModel>().maxRbeSpawnedPerSecond=0;
            ZealotWarp.weapons[1].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().lifespanFrames=99999;
            ZealotWarp.weapons[1].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().speedFrames=0.525f;
            ZealotWarp.weapons[1].projectile.GetDamageModel().damage=2;
            ZealotWarp.weapons[1].projectile.radius=5;
            ZealotWarp.name="ZealotWarp";
            ZealotWarp.weapons[1].projectile.pierce=3;
            ZealotWarp.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().alternateProjectile=ZealotWarp.weapons[1].projectile;
            ZealotWarp.weapons[1].rate=4.7f;
            Gateway.behaviors.First(a=>a.name.Contains("Zone")).Cast<NecromancerZoneModel>().attackUsedForRangeModel.range=999;
            Gateway.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="GatewayPrefab";
        }
        public class Charge:ModUpgrade<Gateway> {
            public override string Name=>"Charge";
            public override string DisplayName=>"Charge";
            public override string Description=>"Cybernetic enhancements to the legs allow Zealots to move faster";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Gateway){
                GetUpgradeModel().icon=new("GatewayChargeIcon");
                var ZealotWarp=Gateway.behaviors.First(a=>a.name.Equals("ZealotWarp")).Cast<AttackModel>();
                ZealotWarp.weapons[1].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().speedFrames=0.9f;
            }
        }
        public class Solarite:ModUpgrade<Gateway> {
            public override string Name=>"Solarite";
            public override string DisplayName=>"Solarite Reaper";
            public override string Description=>"Solarite Reapers do more damage and have a larger radius";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Gateway) {
                GetUpgradeModel().icon=new("GatewaySolariteIcon");
                var ZealotWarp=Gateway.behaviors.First(a=>a.name.Equals("ZealotWarp")).Cast<AttackModel>();
                ZealotWarp.weapons[1].projectile.GetDamageModel().damage=5;
                ZealotWarp.weapons[1].projectile.radius=8;
                ZealotWarp.weapons[1].projectile.display="GatewaySolaritePrefab";
            }
        }
        public class Sentinel:ModUpgrade<Gateway> {
            public override string Name=>"Sentinel";
            public override string DisplayName=>"Sentinel";
            public override string Description=>"Purifier Sentinels are more durable then their biological counterparts but have timed life";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Gateway){
                GetUpgradeModel().icon=new("GatewaySentinelIcon");
                Gateway.display="GatewayPurifierPrefab";
                var ZealotWarp=Gateway.behaviors.First(a=>a.name.Equals("ZealotWarp")).Cast<AttackModel>();
                ZealotWarp.weapons[1].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().lifespanFrames=350;
                ZealotWarp.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().minPiercePerBloon=13;
                ZealotWarp.weapons[1].projectile.pierce=13;
                ZealotWarp.weapons[1].projectile.display="GatewaySentinelPrefab";
            }
        }
        public class Legionnaire:ModUpgrade<Gateway> {
            public override string Name=>"Legionnaire";
            public override string DisplayName=>"Legionnaires";
            public override string Description=>"The Sentinels used by Talandars forces are much more powerful then their regular models";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Gateway){
                GetUpgradeModel().icon=new("GatewayLegionnaireIcon");
                var ZealotWarp=Gateway.behaviors.First(a=>a.name.Equals("ZealotWarp")).Cast<AttackModel>();
                ZealotWarp.weapons[1].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().lifespanFrames=550;
                ZealotWarp.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().minPiercePerBloon=25;
                ZealotWarp.weapons[1].projectile.GetDamageModel().damage=10;
                ZealotWarp.weapons[1].projectile.pierce=25;
                ZealotWarp.weapons[1].projectile.display="GatewayLegionnairePrefab";
            }
        }
        public class Kaldalis:ModUpgrade<Gateway>{
            public override string Name=>"Kaldalis";
            public override string DisplayName=>"Kaldalis";
            public override string Description=>"Allows the cloning of Kaldalis's personality matrix into Legionnaires";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Gateway){
                //i really need to figure out what that ring thing is on the back of hero purifier units and how it works, would be fucking amazing for this
                GetUpgradeModel().icon=new("GatewayKaldalisIcon");
                var ZealotWarp=Gateway.behaviors.First(a=>a.name.Equals("ZealotWarp")).Cast<AttackModel>();
                ZealotWarp.weapons[1].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().lifespanFrames=99999;
                ZealotWarp.weapons[1].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().speedFrames=1.1f;
                ZealotWarp.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().minPiercePerBloon=50;
                ZealotWarp.weapons[1].rate=6;
                ZealotWarp.weapons[1].projectile.pierce=50;
                ZealotWarp.weapons[1].projectile.GetDamageModel().damage=35;
                ZealotWarp.weapons[1].projectile.display="GatewayKaldalisPrefab";
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
                if(!protos.ContainsKey(objectId)&&objectId.Equals("GatewayPurifierPrefab")){
                    var udn=GetGateway(__instance.PrototypeRoot,"GatewayPurifierPrefab");
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
                if(reference!=null&&reference.guidRef.Equals("GatewaySolariteIcon")){
                    var b=Assets.LoadAsset("GatewaySolariteIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("GatewaySentinelIcon")){
                    var b=Assets.LoadAsset("GatewaySentinelIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("GatewayLegionnaireIcon")){
                    var b=Assets.LoadAsset("GatewayLegionnaireIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("GatewayKaldalisIcon")){
                    var b=Assets.LoadAsset("GatewayKaldalisIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("GatewayPortrait")){
                    var b=Assets.LoadAsset("GatewayPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
    }
}