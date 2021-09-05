//Stuff's called SC2Marine to avoid any potential conflict with the already existing marine
namespace SC2Expansion.Towers{
    public class SC2Marine:ModTower{
        public static string name="Marine "; //Space in the name to avoid messing with the existing marine, closest we get to any conflict
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"SniperMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Basic soldier with automatic gauss rifle";
        public override void ModifyBaseTowerModel(TowerModel SC2Marine){
            SC2Marine.display="SC2MarinePrefab";
            SC2Marine.portrait=new("SC2MarineIcon");
            SC2Marine.icon=new("SC2MarineIcon");
            SC2Marine.emoteSpriteLarge=new("Terran");
            SC2Marine.radius=5;
            SC2Marine.cost=400;
            SC2Marine.range=35;
            SC2Marine.footprint.ignoresPlacementCheck=true;
            SC2Marine.cachedThrowMarkerHeight=10;
            SC2Marine.areaTypes=new(1);
            SC2Marine.areaTypes[0]=AreaType.land;
            var Bullet=SC2Marine.behaviors.First(a=>a.name.Contains("AttackModel")).Cast<AttackModel>();
            Bullet.weapons[0].name="SC2MarineBullet";
            Bullet.weapons[0].rate=0.25f;
            Bullet.weapons[0].rateFrames=1;
            Bullet.range=35;
            Bullet.weapons[0].projectile.display=null;
            SC2Marine.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="SC2MarinePrefab";
        }
        public override int GetTowerIndex(List<TowerDetailsModel>towerSet){
            return towerSet.First(model=>model.towerId==TowerType.BoomerangMonkey).towerIndex+1;
        }
        public class U238Shells:ModUpgrade<SC2Marine> {
            public override string Name=>"U238Shells";
            public override string DisplayName=>"U-238 Shells";
            public override string Description=>"Making the ammunition casing out of depleted Uranium 238 increases range and damage";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel SC2Marine){
                var Bullet=SC2Marine.behaviors.First(a=>a.name.Contains("AttackModel")).Cast<AttackModel>();
                Bullet.range=45;
                Bullet.weapons[0].projectile.behaviors[0].Cast<DamageModel>().damage=2;
        }
        public class LTS:ModUpgrade<SC2Marine> {
            public override string Name=>"LTS";
            public override string DisplayName=>"Laser Targetting System";
            public override string Description=>"Adding laser pointer allows targetting camo bloons and slightly increases range";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel SC2Marine){
                SC2Marine.range=50;
                SC2Marine.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>().range=50;
                SC2Marine.behaviors=SC2Marine.behaviors.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true));
        }
        public class Stimpacks:ModUpgrade<SC2Marine> {
            public override string Name=>"Stimpacks";
            public override string DisplayName=>"Stimpacks";
            public override string Description=>"Stimpacks increase attack speed by 50% for a short while";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel SC2Marine){
                var Stimpacks=Game.instance.model.towers.First(a=>a.name.Equals("BoomerangMonkey-040")).behaviors.First(a=>a.name.Contains("Ability")).Clone().Cast<AbilityModel>();
                Stimpacks.name="Stimpacks";
                Stimpacks.displayName="Stimpacks";
                Stimpacks.icon=new("SC2MarineStimpacksIcon");
                Stimpacks.cooldown=40;
                Stimpacks.maxActivationsPerRound=1;
                Stimpacks.behaviors.First(a=>a.name.Contains("Turbo")).Cast<TurboModel>().projectileDisplay=null;
                SC2Marine.behaviors=SC2Marine.behaviors.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true),Stimpacks);
        }
        public class Warpig:ModUpgrade<SC2Marine> {
            public override string Name=>"Warpig";
            public override string DisplayName=>"Warpig";
            public override string Description=>"Warpigs use upgraded (Don't ask if its legal) equipment. Increases damage and attack speed";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel SC2Marine){
                SC2Marine.display="SC2MarineWarpigPrefab";
                SC2Marine.portrait=new("SC2MarineWarpigPortrait");
                var Bullet=SC2Marine.behaviors.First(a=>a.name.Contains("AttackModel")).Cast<AttackModel>();
                Bullet.weapons[0].rate=0.17f;
                Bullet.weapons[0].projectile.GetDamageModel().damage=3;
        }
        public class Tychus:ModUpgrade<SC2Marine> {
            public override string Name=>"Tychus";
            public override string DisplayName=>"Tychus Findlay";
            public override string Description=>"\"I'm a bad man\"";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel SC2Marine){
                SC2Marine.display="SC2MarineWarpigPrefab";
                SC2Marine.portrait=new("SC2MarineWarpigPortrait");
                var Bullet=SC2Marine.behaviors.First(a=>a.name.Contains("AttackModel")).Cast<AttackModel>();
                Bullet.weapons[0].rate=0.1f;
                Bullet.weapons[0].projectile.GetDamageModel().damage=3;
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("SC2MarinePrefab")){
                    var udn=GetSC2Marine(__instance.PrototypeRoot,"SC2MarinePrefab");
                    udn.name="SC2Marine";
                    udn.genericRenderers[0].material=Assets.LoadAsset("SC2MarineMaterial").Cast<Material>();
                    udn.RecalculateGenericRenderers();
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("SC2MarineWarpigPrefab")){
                    var udn=GetSC2Marine(__instance.PrototypeRoot,"SC2MarineWarpigPrefab");
                    udn.name="SC2Marine";
                    udn.genericRenderers[0].material=Assets.LoadAsset("SC2MarineWarpigMaterial").Cast<Material>();
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
        public static UnityDisplayNode GetSC2Marine(Transform transform,string model){
            var udn=Object.c(Assets.LoadAsset(model).Cast<GameObject>(),transform).AddComponent<UnityDisplayNode>();
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
                if(reference!=null&&reference.guidRef.Equals("SC2MarineIcon")){
                    var b=Assets.LoadAsset("SC2MarineIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("SC2MarineWarpigPortrait")){
                    var b=Assets.LoadAsset("SC2MarineWarpigPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("SC2MarineWarpigIcon")){
                    var b=Assets.LoadAsset("SC2MarineWarpigIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("SC2MarineU238ShellsIcon")){
                    var b=Assets.LoadAsset("SC2Marineu238ShellsIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("SC2MarineLaserTargetingSystemIcon")){
                    var b=Assets.LoadAsset("SC2MarineLaserTargetingSystemIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("SC2MarineStimpacksIcon")){
                    var b=Assets.LoadAsset("SC2MarineStimpacksIcon");
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