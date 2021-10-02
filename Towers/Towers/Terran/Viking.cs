namespace SC2Expansion.Towers{
    public class Viking:ModTower{
        public override string DisplayName=>"Viking";
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"SniperMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Basic Terran soldier with automatic gauss rifle";
        public override void ModifyBaseTowerModel(TowerModel Viking){
            Viking.display="VikingPrefab";
            Viking.portrait=new("VikingIcon");
            Viking.icon=new("VikingIcon");
            Viking.emoteSpriteLarge=new("Terran");
            Viking.radius=5;
            Viking.cost=400;
            Viking.range=35;
            var Bullet=Viking.behaviors.First(a=>a.name.Contains("AttackModel")).Cast<AttackModel>();
            Bullet.name="VikingBullet";
            Bullet.weapons[0].name="VikingBullet";
            Bullet.weapons[0].rate=0.25f;
            Bullet.weapons[0].rateFrames=1;
            Bullet.range=35;
            Bullet.weapons[0].projectile.display=null;
            Bullet.weapons[0].projectile.GetDamageModel().damage=2.5f;
            Viking.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="VikingPrefab";
        }
        public override int GetTowerIndex(List<TowerDetailsModel>towerSet){
            return towerSet.First(model=>model.towerId==TowerType.BoomerangMonkey).towerIndex+1;
        }
        //not being used rn bc this loads the asset bundle differently then how the icons and portraits need it, i'll enable it whenever the hell i get around to
        //getting everything fully done in mod helper but for now, disabled
        /*public class VikingDisplay:ModTowerCustomDisplay<Viking>{
            public override string AssetBundleName=>"Viking";
            public override string PrefabName=>"VikingPrefab";
            public override string MaterialName=>"VikingMaterial";
            public override bool UseForTower(int[] tiers){
                return tiers.Sum()==0;
            }
        }*/
        public class U238Shells:ModUpgrade<Viking>{
            public override string Name=>"U238Shells";
            public override string DisplayName=>"U-238 Shells";
            public override string Description=>"Making the ammunition casing out of depleted Uranium 238 increases range and damage";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingU238ShellsIcon");
                var Bullet=Viking.behaviors.First(a=>a.name.Equals("VikingBullet")).Cast<AttackModel>();
                Bullet.range=45;
                Viking.range=Bullet.range;
                Bullet.weapons[0].projectile.GetDamageModel().damage+=1;
            }
        }
        public class LTS:ModUpgrade<Viking>{
            public override string Name=>"LTS";
            public override string DisplayName=>"Laser Targeting System";
            public override string Description=>"Adding a laser pointer allows targetting camo bloons and slightly increases range";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingLaserTargetingSystemIcon");
                var Bullet=Viking.behaviors.First(a=>a.name.Equals("VikingBullet")).Cast<AttackModel>();
                Bullet.range=50;
                Viking.range=Bullet.range;
                Viking.behaviors=Viking.behaviors.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true));
            }
        }
        public class Stimpacks:ModUpgrade<Viking>{
            public override string Name=>"Stimpacks";
            public override string DisplayName=>"Stimpacks";
            public override string Description=>"Stimpacks increase attack speed by 50% for a short while";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingStimpacksIcon");
                var Stimpacks=Game.instance.model.towers.First(a=>a.name.Equals("BoomerangMonkey-040")).behaviors.First(a=>a.name.Contains("Ability")).Clone().Cast<AbilityModel>();
                Stimpacks.name="Stimpacks";
                Stimpacks.displayName="Stimpacks";
                Stimpacks.icon=new("VikingStimpacksIcon");
                Stimpacks.cooldown=40;
                Stimpacks.maxActivationsPerRound=1;
                Stimpacks.behaviors.First(a=>a.name.Contains("Turbo")).Cast<TurboModel>().extraDamage=0;
                Stimpacks.behaviors.First(a=>a.name.Contains("Turbo")).Cast<TurboModel>().projectileDisplay=null;
                Viking.behaviors=Viking.behaviors.Add(Stimpacks);
            }
        }
        public class Warpig:ModUpgrade<Viking>{
            public override string Name=>"Warpig";
            public override string DisplayName=>"Warpig";
            public override string Description=>"Warpig mercenaries use upgraded (Don't ask if its legal) equipment. Increases damage and attack speed";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingWarpigIcon");
                Viking.display="VikingWarpigPrefab";
                Viking.portrait=new("VikingWarpigPortrait");
                var Bullet=Viking.behaviors.First(a=>a.name.Contains("VikingBullet")).Cast<AttackModel>();
                Bullet.weapons[0].rate=0.17f;
                Bullet.weapons[0].projectile.GetDamageModel().damage+=1.5f;
            }
        }
        public class Raynor:ModUpgrade<Viking>{
            public override string Name=>"Raynor";
            public override string DisplayName=>"James Raynor";
            public override string Description=>"\"Jimmy here!\"";
            public override int Cost=>9000;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingRaynorIcon");
                Viking.display="VikingRaynorPrefab";
                Viking.portrait=new("VikingRaynorIcon");
                var Bullet=Viking.behaviors.First(a=>a.name.Contains("VikingBullet")).Cast<AttackModel>();
                Bullet.weapons[0].rate=0.13f;
                Bullet.weapons[0].projectile.GetDamageModel().damage+=3;
                var FragGrenade=Game.instance.model.towers.First(a=>a.name.Contains("BombShooter-002")).Cast<TowerModel>().behaviors.First(a=>a.name.Contains("Attack")).
                    Clone().Cast<AttackModel>();
                Viking.AddBehavior(FragGrenade);
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("VikingPrefab")){
                    var udn=GetViking(__instance.PrototypeRoot,"VikingPrefab");
                    udn.name="SC2Expansion-Viking";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("VikingWarpigPrefab")){
                    var udn=GetViking(__instance.PrototypeRoot,"VikingWarpigPrefab");
                    udn.name="SC2Expansion-Viking";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("VikingRaynorPrefab")){
                    var udn=GetViking(__instance.PrototypeRoot,"VikingRaynorPrefab");
                    udn.name="SC2Expansion-Viking";
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
        public static UnityDisplayNode GetViking(Transform transform,string model){
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
                if(reference!=null&&reference.guidRef.Equals("VikingIcon")){
                    var b=Assets.LoadAsset("VikingIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingWarpigPortrait")){
                    var b=Assets.LoadAsset("VikingWarpigPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingWarpigIcon")){
                    var b=Assets.LoadAsset("VikingWarpigIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingU238ShellsIcon")){
                    var b=Assets.LoadAsset("Vikingu238ShellsIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingLaserTargetingSystemIcon")){
                    var b=Assets.LoadAsset("VikingLaserTargetingSystemIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingStimpacksIcon")){
                    var b=Assets.LoadAsset("VikingStimpacksIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingRaynorIcon")){
                    var b=Assets.LoadAsset("VikingRaynorIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
    }
}