namespace SC2Expansion.Towers{
    public class Marine:ModTower{
        public override string DisplayName=>"Marine";
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"SniperMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Basic Terran soldier with automatic gauss rifle";
        public override void ModifyBaseTowerModel(TowerModel Marine){
            Marine.display="MarinePrefab";
            Marine.portrait=new("MarineIcon");
            Marine.icon=new("MarineIcon");
            Marine.emoteSpriteLarge=new("Terran");
            Marine.radius=5;
            Marine.cost=400;
            Marine.range=35;
            var Bullet=Marine.behaviors.First(a=>a.name.Contains("AttackModel")).Cast<AttackModel>();
            Bullet.name="MarineBullet";
            Bullet.weapons[0].name="MarineBullet";
            Bullet.weapons[0].rate=0.25f;
            Bullet.weapons[0].rateFrames=1;
            Bullet.range=35;
            Bullet.weapons[0].projectile.display=null;
            Bullet.weapons[0].projectile.GetDamageModel().damage=2.5f;
            Marine.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="MarinePrefab";
        }
        //not being used rn bc this loads the asset bundle differently then how the icons and portraits need it, i'll enable it whenever the hell i get around to
        //getting everything fully done in mod helper but for now, disabled
        /*public class MarineDisplay:ModTowerCustomDisplay<Marine>{
            public override string AssetBundleName=>"marine";
            public override string PrefabName=>"MarinePrefab";
            public override string MaterialName=>"MarineMaterial";
            public override bool UseForTower(int[] tiers){
                return tiers.Sum()==0;
            }
        }*/
        public class U238Shells:ModUpgrade<Marine>{
            public override string Name=>"U238Shells";
            public override string DisplayName=>"U-238 Shells";
            public override string Description=>"Making the ammunition casing out of depleted Uranium 238 increases range and damage";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineU238ShellsIcon");
                var Bullet=Marine.behaviors.First(a=>a.name.Equals("MarineBullet")).Cast<AttackModel>();
                Bullet.range=45;
                Marine.range=Bullet.range;
                Bullet.weapons[0].projectile.GetDamageModel().damage+=1;
            }
        }
        public class LTS:ModUpgrade<Marine>{
            public override string Name=>"LTS";
            public override string DisplayName=>"Laser Targeting System";
            public override string Description=>"Adding a laser pointer allows targetting camo bloons and slightly increases range";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineLaserTargetingSystemIcon");
                var Bullet=Marine.behaviors.First(a=>a.name.Equals("MarineBullet")).Cast<AttackModel>();
                Bullet.range=50;
                Marine.range=Bullet.range;
                Marine.behaviors=Marine.behaviors.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true));
            }
        }
        public class Stimpacks:ModUpgrade<Marine>{
            public override string Name=>"Stimpacks";
            public override string DisplayName=>"Stimpacks";
            public override string Description=>"Stimpacks increase attack speed by 50% for a short while";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineStimpacksIcon");
                var Stimpacks=Game.instance.model.towers.First(a=>a.name.Equals("BoomerangMonkey-040")).behaviors.First(a=>a.name.Contains("Ability")).Clone().Cast<AbilityModel>();
                Stimpacks.name="Stimpacks";
                Stimpacks.displayName="Stimpacks";
                Stimpacks.icon=new("MarineStimpacksIcon");
                Stimpacks.cooldown=40;
                Stimpacks.maxActivationsPerRound=1;
                Stimpacks.behaviors.First(a=>a.name.Contains("Turbo")).Cast<TurboModel>().extraDamage=0;
                Stimpacks.behaviors.First(a=>a.name.Contains("Turbo")).Cast<TurboModel>().projectileDisplay=null;
                Marine.behaviors=Marine.behaviors.Add(Stimpacks);
            }
        }
        public class Warpig:ModUpgrade<Marine>{
            public override string Name=>"Warpig";
            public override string DisplayName=>"Warpig";
            public override string Description=>"Warpig mercenaries use upgraded (Don't ask if its legal) equipment. Increases damage and attack speed";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineWarpigIcon");
                Marine.display="MarineWarpigPrefab";
                Marine.portrait=new("MarineWarpigPortrait");
                var Bullet=Marine.behaviors.First(a=>a.name.Contains("MarineBullet")).Cast<AttackModel>();
                Bullet.weapons[0].rate=0.17f;
                Bullet.weapons[0].projectile.GetDamageModel().damage+=1.5f;
            }
        }
        public class Raynor:ModUpgrade<Marine>{
            public override string Name=>"Raynor";
            public override string DisplayName=>"James Raynor";
            public override string Description=>"\"Jimmy here!\"";
            public override int Cost=>9000;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineRaynorIcon");
                Marine.display="MarineRaynorPrefab";
                Marine.portrait=new("MarineRaynorIcon");
                var Bullet=Marine.behaviors.First(a=>a.name.Contains("MarineBullet")).Cast<AttackModel>();
                Bullet.weapons[0].rate=0.13f;
                Bullet.weapons[0].projectile.GetDamageModel().damage+=3;
                var FragGrenade=Game.instance.model.towers.First(a=>a.name.Contains("BombShooter-002")).Cast<TowerModel>().behaviors.First(a=>a.name.Contains("Attack")).
                    Clone().Cast<AttackModel>();
                FragGrenade.name="MarineFragGrenade";
                FragGrenade.AddBehavior(new PauseAllOtherAttacksModel("PauseAllOtherAttacksModel",1,true,true));
                FragGrenade.range=Marine.range-10;
                Marine.AddBehavior(FragGrenade);
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Contains("Marine")){
                    var udn=GetMarine(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-Marine";
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
        public static UnityDisplayNode GetMarine(Transform transform,string model){
            var udn=Object.Instantiate(Assets.LoadAsset(model).Cast<GameObject>(),transform).AddComponent<UnityDisplayNode>();
            udn.Active=false;
            udn.transform.position=new(-3000,0);
            return udn;
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public record ResourceLoader_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.Contains("Marine")){
                    var text=Assets.LoadAsset(reference.guidRef).Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),nameof(Weapon.SpawnDart))]
        public static class SpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.towerModel.name.Contains("Marine")){
                    if(__instance.attack.attackModel.name.Contains("Grenade")){
                        __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("MarineGrenade");
                    }else{
                        __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("MarineAttack");
                    }
                }
            }
        }
    }
}