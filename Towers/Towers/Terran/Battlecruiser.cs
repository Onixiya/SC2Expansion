namespace SC2Expansion.Towers{
    public class Battlecruiser:ModTower{
        public override string DisplayName=>"Battlecruiser";
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"SuperMonkey-100";
        public override int Cost=>2750;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Terran capital ship, shoots lasers very fast";
        public override void ModifyBaseTowerModel(TowerModel Battlecruiser){
            Battlecruiser.display="BattlecruiserPrefab";
            Battlecruiser.portrait=new("BattlecruiserPortrait");
            Battlecruiser.icon=new("BattlecruiserIcon");
            Battlecruiser.emoteSpriteLarge=new("Terran");
            Battlecruiser.radius=16.7f;
            Battlecruiser.range=70;
            Battlecruiser.areaTypes=new(4);
            Battlecruiser.areaTypes[0]=AreaType.land;
            Battlecruiser.areaTypes[1]=AreaType.track;
            Battlecruiser.areaTypes[2]=AreaType.ice;
            Battlecruiser.areaTypes[3]=AreaType.water;
            var Fire=Battlecruiser.behaviors.First(a=>a.name.Contains("AttackModel")).Cast<AttackModel>();
            Fire.name="BattlecruiserFire";
            Fire.weapons[0].name="BattlecruiserFire";
            Fire.range=70;
            Fire.weapons[0].projectile.GetDamageModel().damage=1;
            Battlecruiser.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="BattlecruiserPrefab";
        }
        public class TacJump:ModUpgrade<Battlecruiser>{
            public override string Name=>"TacJump";
            public override string DisplayName=>"Tactical Jump";
            public override string Description=>"Allows the Battlecruiser to make use of its short range warp drive, instantly teleporting to any where on the map";
            public override int Cost=>2000;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Battlecruiser){
                GetUpgradeModel().icon=new("BattlecruiserTacJumpIcon");
                var TacJump=Game.instance.model.towers.First(a=>a.name.Contains("SuperMonkey-003")).behaviors.First(a=>a.name.Contains("Darkshift")).Clone().Cast<AbilityModel>();
                TacJump.name="TacJump";
                TacJump.cooldown=80;
                TacJump.behaviors.First().Cast<DarkshiftModel>().restrictToTowerRadius=false;
                TacJump.behaviors.First().Cast<DarkshiftModel>().disappearEffectModel.assetId=null;
                TacJump.behaviors.First().Cast<DarkshiftModel>().reappearEffectModel.assetId=null;
                TacJump.icon=new("BattlecruiserTacJumpIcon");
                //TacJump.maxActivationsPerRound=1;
                Battlecruiser.AddBehavior(TacJump);
            }
        }
        public class Yamato:ModUpgrade<Battlecruiser>{
            public override string Name=>"Yamato";
            public override string DisplayName=>"Yamato Cannon";
            public override string Description=>"Upgrades to the colossus reactor lets the Yamato cannon be fired dealing massive damage to a single target";
            public override int Cost=>2000;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Battlecruiser){
                GetUpgradeModel().icon=new("BattlecruiserYamatoIcon");
                var Yamato=Game.instance.model.towers.First(a=>a.name.Equals("PatFusty 10")).behaviors.First(a=>a.name.Contains("Big")).Clone().Cast<AbilityModel>();
                Yamato.name="Yamato";
                Yamato.displayName="Yamato Cannon";
                Yamato.behaviors.First(a=>a.name.Contains("Activate")).Cast<ActivateAttackModel>().attacks[0]=
                    Game.instance.model.towers.First(a=>a.name.Equals("BombShooter")).behaviors.First(a=>a.name.Contains("Attack")).Clone().Cast<AttackModel>();
                Yamato.behaviors.First(a=>a.name.Contains("Activate")).Cast<ActivateAttackModel>().attacks[0].weapons[0].projectile.behaviors.First(a=>a.name.Contains("Contact")).
                    Cast<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage=225;
                Yamato.behaviors.First(a=>a.name.Contains("Activate")).Cast<ActivateAttackModel>().attacks[0].range=9999;                 //who the fuck spells it like strait?
                Yamato.behaviors.First(a=>a.name.Contains("Activate")).Cast<ActivateAttackModel>().attacks[0].weapons[0].projectile.behaviors.First(a=>a.name.Contains("Strait")).
                    Cast<TravelStraitModel>().speed=500;
                Yamato.behaviors.First(a=>a.name.Contains("Activate")).Cast<ActivateAttackModel>().attacks[0].weapons[0].projectile.display="88399aeca4ae48a44aee5b08eb16cc61";
                Yamato.icon=new("BattlecruiserYamatoIcon");
                Yamato.cooldown=70f;
                Yamato.RemoveBehavior(Yamato.behaviors.First(a=>a.name.Contains("Pause")));
                Yamato.maxActivationsPerRound=1;
                Battlecruiser.AddBehavior(Yamato);
            }
        }
        public class Sovereign:ModUpgrade<Battlecruiser>{
            public override string Name=>"Sovereign";
            public override string DisplayName=>"Sovereign Class";
            public override string Description=>"Sovereign Class Battlecruisers can fire the Yamato cannon regularly with reduced damage";
            public override int Cost=>2000;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Battlecruiser){
                GetUpgradeModel().icon=new("BattlecruiserSovereignIcon");
                Battlecruiser.display="BattlecruiserSovereignPrefab";
                var MiniYamato=Battlecruiser.behaviors.First(a=>a.name.Contains("Yamato")).Cast<AbilityModel>().behaviors.First(a=>a.name.Contains("Activate")).
                    Clone().Cast<ActivateAttackModel>().attacks[0];
                MiniYamato.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Contact")).Cast<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage=70;
                MiniYamato.range=90;
                MiniYamato.weapons[0].rate=3.5f;
                MiniYamato.name="MiniYamato";
                Battlecruiser.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="BattlecruiserSovereignPrefab";
                Battlecruiser.RemoveBehavior(Battlecruiser.behaviors.First(a=>a.name.Contains("Yamato")));
                Battlecruiser.AddBehavior(MiniYamato);
            }
        }
        public class POA:ModUpgrade<Battlecruiser>{
            public override string Name=>"POA";
            public override string DisplayName=>"Pride of Augustgrad";
            public override string Description=>"Elite Dominion Battlecruiser, can Tac jump much more frequently, increased damage and camo detection";
            public override int Cost=>2000;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Battlecruiser){
                GetUpgradeModel().icon=new("BattlecruiserPOAIcon");
                Battlecruiser.display="BattlecruiserPOAPrefab";
                var BattlecruiserFire=Battlecruiser.behaviors.First(a=>a.name.Contains("Fire")).Cast<AttackModel>();
                var TacJump=Battlecruiser.behaviors.First(a=>a.name.Equals("TacJump")).Cast<AbilityModel>();
                BattlecruiserFire.weapons[0].projectile.GetDamageModel().damage+=2;
                Battlecruiser.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="BattlecruiserPOAPrefab";
                Battlecruiser.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true));
                TacJump.cooldown=40;
            }
        }
        public class Hyperion:ModUpgrade<Battlecruiser>{
            public override string Name=>"Hyperion";
            public override string DisplayName=>"Hyperion";
            public override string Description=>"A ship hailing from the days of the Confederacy, it has seen many crews and battles. Currently the flagship of the Dominion";
            public override int Cost=>2000;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Battlecruiser){
                var BattlecruiserFire=Battlecruiser.behaviors.First(a=>a.name.Contains("Fire")).Cast<AttackModel>();
                var TacJump=Battlecruiser.behaviors.First(a=>a.name.Equals("TacJump")).Cast<AbilityModel>();
                var Yamato=Battlecruiser.behaviors.First(a=>a.name.Contains("Yamato")).Cast<AttackModel>();
                GetUpgradeModel().icon=new("BattlecruiserHyperionIcon");
                Battlecruiser.display="BattlecruiserHyperionPrefab";
                Battlecruiser.portrait=new("BattlecruiserHyperionPortrait");
                BattlecruiserFire.range+=15;
                Battlecruiser.range=BattlecruiserFire.range;
                BattlecruiserFire.weapons[0].rate-=0.03f;
                BattlecruiserFire.weapons[0].projectile.GetDamageModel().damage+=3;
                Yamato.range+=15;
                Yamato.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Contact")).Cast<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage=150;
                Yamato.weapons[0].rate=2.25f;
                TacJump.cooldown=20;
                Battlecruiser.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="BattlecruiserHyperionPrefab";
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("BattlecruiserPrefab")){
                    var udn=GetBattlecruiser(__instance.PrototypeRoot,"BattlecruiserPrefab");
                    udn.name="SC2Expansion-Battlecruiser";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("BattlecruiserSovereignPrefab")){
                    var udn=GetBattlecruiser(__instance.PrototypeRoot,"BattlecruiserSovereignPrefab");
                    udn.name="SC2Expansion-Battlecruiser";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("BattlecruiserHyperionPrefab")){
                    var udn=GetBattlecruiser(__instance.PrototypeRoot,"BattlecruiserHyperionPrefab");
                    udn.name="SC2Expansion-Battlecruiser";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("BattlecruiserPOAPrefab")){
                    var udn=GetBattlecruiser(__instance.PrototypeRoot,"BattlecruiserPOAPrefab");
                    udn.name="SC2Expansion-Battlecruiser";
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
        public static UnityDisplayNode GetBattlecruiser(Transform transform,string model){
            var udn=Object.Instantiate(Assets.LoadAsset(model).Cast<GameObject>(),transform).AddComponent<UnityDisplayNode>();
            udn.Active=false;
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
                if(reference!=null&&reference.guidRef.Equals("BattlecruiserIcon")){
                    var b=Assets.LoadAsset("BattlecruiserIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BattlecruiserPortrait")){
                    var b=Assets.LoadAsset("BattlecruiserPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BattlecruiserSovereignIcon")){
                    var b=Assets.LoadAsset("BattlecruiserSovereignIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BattlecruiserYamatoIcon")){
                    var b=Assets.LoadAsset("BattlecruiserYamatoIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BattlecruiserPOAIcon")){
                    var b=Assets.LoadAsset("BattlecruiserPOAIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BattlecruiserHyperionIcon")){
                    var b=Assets.LoadAsset("BattlecruiserHyperionIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BattlecruiserHyperionPortrait")){
                    var b=Assets.LoadAsset("BattlecruiserHyperionPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BattlecruiserPOAPortrait")){
                    var b=Assets.LoadAsset("BattlecruiserHyperionPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BattlecruiserSovereignPortrait")){
                    var b=Assets.LoadAsset("BattlecruiserSovereignPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BattlecruiserTacJumpIcon")){
                    var b=Assets.LoadAsset("BattlecruiserTacJumpIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
    }
}