namespace SC2Expansion.Towers{
    public class Gateway:ModTower<ProtossSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.gateway);
        public override string BaseTower=>"WizardMonkey-005";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ProtossEnabled;
        public override string Description=>"Warps in Zealots. Frontline Protoss melee troops";
        public override void ModifyBaseTowerModel(TowerModel Gateway){
            Gateway.display="GatewayPrefab";
            Gateway.portrait=new("GatewayPortrait");
            Gateway.icon=new("GatewayIcon");
            Gateway.emoteSpriteLarge=new("Protoss");
            Gateway.radius=30;
            Gateway.range=31;
            Gateway.RemoveBehavior(Gateway.GetBehaviors<AttackModel>().First(a=>a.name.Contains("Shimmer")));
            Gateway.RemoveBehavior(Gateway.GetBehaviors<AttackModel>().First(a=>a.name.Equals("AttackModel_Attack_")));
            Gateway.RemoveBehavior<PrinceOfDarknessZombieBuffModel>();
            var ZealotWarp=Gateway.GetAttackModel();
            ZealotWarp.weapons[1].projectile.display="GatewayZealotPrefab";
            ZealotWarp.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().minPiercePerBloon=3;
            //tried removing the first weapon entirely, it didn't like it at all and kept crashing, setting maxrbespawned prevents the normal necrobloons from spawning at all
            ZealotWarp.weapons[0].emission.Cast<NecromancerEmissionModel>().maxRbeSpawnedPerSecond=0;
            ZealotWarp.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=99999;
            ZealotWarp.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=0.525f;
            ZealotWarp.weapons[1].projectile.GetDamageModel().damage=2;
            ZealotWarp.weapons[1].projectile.radius=5;
            ZealotWarp.name="ZealotWarp";
            ZealotWarp.weapons[1].projectile.pierce=3;
            ZealotWarp.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().alternateProjectile=ZealotWarp.weapons[1].projectile;
            ZealotWarp.weapons[1].rate=4.7f;
            ZealotWarp.range=Gateway.range;
            Gateway.GetBehavior<NecromancerZoneModel>().attackUsedForRangeModel.range=999;
            Gateway.GetBehavior<DisplayModel>().display=Gateway.display;
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
                Gateway.GetAttackModel().weapons[1].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=0.9f;
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
                var ZealotWarp=Gateway.GetAttackModel();
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
                var ZealotWarp=Gateway.GetAttackModel();
                ZealotWarp.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=350;
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
                var ZealotWarp=Gateway.GetAttackModel();
                ZealotWarp.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=550;
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
                GetUpgradeModel().icon=new("GatewayKaldalisIcon");
                var ZealotWarp=Gateway.GetAttackModel();
                ZealotWarp.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=99999;
                ZealotWarp.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=1.1f;
                ZealotWarp.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().minPiercePerBloon=50;
                ZealotWarp.weapons[1].rate=6;
                ZealotWarp.weapons[1].projectile.pierce=50;
                ZealotWarp.weapons[1].projectile.GetDamageModel().damage=35;
                ZealotWarp.weapons[1].projectile.display="GatewayKaldalisPrefab";
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!DisplayDict.ContainsKey(objectId)&&objectId.Contains("Gateway")){
                    var udn=uObject.Instantiate(TowerAssets.LoadAsset(objectId).Cast<GameObject>(),__instance.PrototypeRoot).AddComponent<UnityDisplayNode>();
                    udn.transform.position=new(-3000,0);
                    udn.name="SC2Expansion-Gateway";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    DisplayDict.Add(objectId,udn);
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
                if(reference!=null&&reference!=null&&reference.guidRef.Contains("Gateway")){
                    var text=TowerAssets.LoadAsset(reference.guidRef).Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
    }
}