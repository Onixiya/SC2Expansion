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
            Bullet.weapons[0].projectile.GetDamageModel().damage=1;
            Marine.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="MarinePrefab";
        }
        public override int GetTowerIndex(List<TowerDetailsModel>towerSet){
            return towerSet.First(model=>model.towerId==TowerType.BoomerangMonkey).towerIndex+1;
        }
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
                Bullet.weapons[0].projectile.GetDamageModel().damage=2;
            }
        }
        public class LTS:ModUpgrade<Marine> {
            public override string Name=>"LTS";
            public override string DisplayName=>"Laser Targeting System";
            public override string Description=>"Adding a laser pointer allows targetting camo bloons and slightly increases range";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineLaserTargetingSystemIcon");
                Marine.range=50;
                Marine.behaviors.First(a=>a.name.Equals("MarineBullet")).Cast<AttackModel>().range=50;
                Marine.behaviors=Marine.behaviors.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true));
            }
        }
        public class Stimpacks:ModUpgrade<Marine> {
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
                Stimpacks.behaviors.First(a=>a.name.Contains("Turbo")).Cast<TurboModel>().projectileDisplay=null;
                Marine.behaviors=Marine.behaviors.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true),Stimpacks);
            }
        }
        public class Warpig:ModUpgrade<Marine> {
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
                Bullet.weapons[0].projectile.GetDamageModel().damage=3;
            }
        }
        public class Raynor:ModUpgrade<Marine>{
            public override string Name=>"Raynor";
            public override string DisplayName=>"James Raynor";
            public override string Description=>"\"Jimmy here!\"";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineRaynorIcon");
                Marine.display="MarineRaynorPrefab";
                Marine.portrait=new("MarineRaynorIcon");
                var Bullet=Marine.behaviors.First(a=>a.name.Contains("MarineBullet")).Cast<AttackModel>();
                Bullet.weapons[0].rate=0.13f;
                Bullet.weapons[0].projectile.GetDamageModel().damage=5;
                var FragGrenade=Game.instance.model.towers.First(a=>a.name.Contains("BombShooter-002")).Cast<TowerModel>().behaviors.First(a=>a.name.Contains("Attack")).
                    Clone().Cast<AttackModel>();
                Marine.AddBehavior(FragGrenade);
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("MarinePrefab")){
                    var udn=GetMarine(__instance.PrototypeRoot,"MarinePrefab");
                    udn.name="SC2Expansion-Marine";
                    udn.genericRenderers[0].material=Assets.LoadAsset("MarineMaterial").Cast<Material>();
                    udn.RecalculateGenericRenderers();
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("MarineWarpigPrefab")){
                    var udn=GetMarine(__instance.PrototypeRoot,"MarineWarpigPrefab");
                    udn.name="SC2Expansion-Marine";
                    udn.genericRenderers[0].material=Assets.LoadAsset("MarineWarpigMaterial").Cast<Material>();
                    udn.RecalculateGenericRenderers();
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("MarineRaynorPrefab")){
                    var udn=GetMarine(__instance.PrototypeRoot,"MarineRaynorPrefab");
                    udn.name="SC2Expansion-Marine";
                    udn.genericRenderers[0].material=Assets.LoadAsset("MarineRaynorMaterial").Cast<Material>();
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
        public static UnityDisplayNode GetMarine(Transform transform,string model){
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
                if(reference!=null&&reference.guidRef.Equals("MarineIcon")){
                    var b=Assets.LoadAsset("MarineIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("MarineWarpigPortrait")){
                    var b=Assets.LoadAsset("MarineWarpigPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("MarineWarpigIcon")){
                    var b=Assets.LoadAsset("MarineWarpigIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("MarineU238ShellsIcon")){
                    var b=Assets.LoadAsset("Marineu238ShellsIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("MarineLaserTargetingSystemIcon")){
                    var b=Assets.LoadAsset("MarineLaserTargetingSystemIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("MarineStimpacksIcon")){
                    var b=Assets.LoadAsset("MarineStimpacksIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("MarineRaynorIcon")){
                    var b=Assets.LoadAsset("MarineRaynorIcon");
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
                if(__instance.weaponModel.name.Contains("SC2MarineBullet")){
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().StopPlayback();
                    __instance.attack.tower.Node.graphic.GetComponent<Animation>().Play();
                    MelonLogger.Msg("hydra fire");
                    __instance.attack.tower.Node.Graphic.gameObject.GetComponent<Animator>().Play("SC2MarinePrefab//Armature Object|Armature ObjectAttack",-1,0f);
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack",true);
                    var wait=23000f;
                    await Task.Run(()=>{
                        while(wait>0){
                            wait-=TimeManager.timeScaleWithoutNetwork+1;
                            Task.Delay(1);
                        }
                        return;
                    });
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack",false);
                }
            }
        }*/
    }
}