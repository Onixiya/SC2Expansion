namespace SC2Expansion.Towers{
    public class UltraliskCavern:ModTower{
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"WizardMonkey-005";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Spawns Ultralisks, basically living tanks and very hard to kill";
        public override void ModifyBaseTowerModel(TowerModel UltraliskCavern){
            UltraliskCavern.display="UltraliskCavernPrefab";
            UltraliskCavern.portrait=new("UltraliskCavernPortrait");
            UltraliskCavern.icon=new("UltraliskCavernIcon");
            UltraliskCavern.emoteSpriteLarge=new("Zerg");
            UltraliskCavern.radius=20;
            UltraliskCavern.range=15;
            UltraliskCavern.behaviors=UltraliskCavern.behaviors.Remove(a=>a.name.Contains("Shimmer"));
            UltraliskCavern.behaviors=UltraliskCavern.behaviors.Remove(a=>a.name.Equals("AttackModel_Attack_"));
            UltraliskCavern.behaviors=UltraliskCavern.behaviors.Remove(a=>a.name.Contains("Buff"));
            var SpawnUltralisk=UltraliskCavern.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            SpawnUltralisk.weapons[1].projectile.display="UltraliskCavernUltraliskPrefab";
            SpawnUltralisk.weapons[0].emission.Cast<NecromancerEmissionModel>().maxRbeSpawnedPerSecond=0;
            SpawnUltralisk.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().minPiercePerBloon=15;
            SpawnUltralisk.weapons[1].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().lifespanFrames=99999;
            SpawnUltralisk.weapons[1].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().speedFrames=0.45f;
            SpawnUltralisk.weapons[1].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().disableRotateWithPathDirection=false;
            SpawnUltralisk.weapons[1].projectile.GetDamageModel().damage=6;
            SpawnUltralisk.weapons[1].projectile.radius=7;
            SpawnUltralisk.name="SpawnUltralisk";
            SpawnUltralisk.weapons[1].projectile.pierce=15;
            SpawnUltralisk.weapons[1].rate=4;
            SpawnUltralisk.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().alternateProjectile=SpawnUltralisk.weapons[1].projectile;
            UltraliskCavern.behaviors.First(a=>a.name.Contains("Zone")).Cast<NecromancerZoneModel>().attackUsedForRangeModel.range=999;
            UltraliskCavern.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display=UltraliskCavern.display;
        }
        public class ChitinousPlating:ModUpgrade<UltraliskCavern>{
            public override string Name=>"ChitinousPlating";
            public override string DisplayName=>"Chitinous Plating";
            public override string Description=>"Reinforcing Ultralisks weakpoints with more armour increases the amount of hits they can take";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel UltraliskCavern){
                GetUpgradeModel().icon=new("UltraliskCavernChitinousPlatingIcon");
                var SpawnUltralisk=UltraliskCavern.behaviors.First(a=>a.name.Equals("SpawnUltralisk")).Cast<AttackModel>();
                SpawnUltralisk.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().minPiercePerBloon=25;
                SpawnUltralisk.weapons[1].projectile.pierce=25;
            }
        }
        public class AnabolicSynthesis:ModUpgrade<UltraliskCavern>{
            public override string Name=>"AnabolicSynthesis";
            public override string DisplayName=>"Anabolic Synthesis";
            public override string Description=>"Synthesising even more adrenaline makes Ultralisks move a lot faster";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel UltraliskCavern){
                GetUpgradeModel().icon=new("UltraliskCavernAnabolicSynthesisIcon");
                UltraliskCavern.behaviors.First(a=>a.name.Equals("SpawnUltralisk")).Cast<AttackModel>().weapons[1].projectile.behaviors.First(a=>a.name.Contains("Travel")).
                    Cast<TravelAlongPathModel>().speedFrames=0.6f;
            }
        }
        public class Noxious:ModUpgrade<UltraliskCavern>{
            public override string Name=>"Noxious";
            public override string DisplayName=>"Noxious Strain";
            public override string Description=>"Evolving Ultralisks into the Noxious strain leaves damaging gas clouds on death";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel UltraliskCavern){
                GetUpgradeModel().icon=new("UltraliskCavernNoxiousIcon");
                var SpawnUltralisk=UltraliskCavern.behaviors.First(a=>a.name.Equals("SpawnUltralisk")).Cast<AttackModel>();
                SpawnUltralisk.weapons[1].projectile.display="UltraliskCavernNoxiousPrefab";
                var GasCloud=Game.instance.model.towers.First(a=>a.name.Contains("EngineerMonkey-030")).behaviors.
                    First(a=>a.name.Contains("CleansingFoam")).Cast<AttackModel>().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                GasCloud.projectile.RemoveBehavior<RemoveBloonModifiersModel>();
                GasCloud.projectile.AddBehavior(new DamageModel("DamageModel",1,1,false,false,true,0));
                SpawnUltralisk.weapons[1].projectile.AddBehavior(GasCloud);
                //GasCloud.projectile.display="9ed0d0de732cabe48898f8dddb7023ca";
            }
        }
        public class Primal:ModUpgrade<UltraliskCavern>{
            public override string Name=>"UltraliskPrimal";
            public override string DisplayName=>"Evolve into Tyrannozor";
            public override string Description=>"Evolves into a Tyrannozor, gaining a upgrade to either health, damage or speed and swaps gas cloud for Barrage of Spikes attack";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel UltraliskCavern){
                GetUpgradeModel().icon=new("UltraliskCavernPrimalIcon");
                var SpawnUltralisk=UltraliskCavern.behaviors.First(a=>a.name.Equals("SpawnUltralisk")).Cast<AttackModel>();
                SpawnUltralisk.weapons[1].projectile.display="UltraliskCavernPrimalPrefab";
            }
        }
        public class Apocalisk:ModUpgrade<UltraliskCavern>{
            public override string Name=>"Apocalisk";
            public override string DisplayName=>"Apocalisk";
            public override string Description=>"\"I call this the Apocalisk! See it in action!\"";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel UltraliskCavern){
                GetUpgradeModel().icon=new("UltraliskCavernApocaliskIcon");
                var SpawnUltralisk=UltraliskCavern.behaviors.First(a=>a.name.Equals("SpawnUltralisk")).Cast<AttackModel>();
                SpawnUltralisk.weapons[1].projectile.display="UltraliskCavernApocaliskPrefab";
            }
        }
        public override int GetTowerIndex(List<TowerDetailsModel> towerSet){
            return towerSet.First(model=>model.towerId==TowerType.BoomerangMonkey).towerIndex+1;
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("UltraliskCavernPrefab")){
                    var udn=GetUltraliskCavern(__instance.PrototypeRoot,"UltraliskCavernPrefab");
                    udn.name="SC2Expansion-UltraliskCavern";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("UltraliskCavernUltraliskPrefab")){
                    var udn=GetUltraliskCavern(__instance.PrototypeRoot,"UltraliskCavernUltraliskPrefab");
                    udn.name="SC2Expansion-UltraliskCavern";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("UltraliskCavernNoxiousPrefab")){
                    var udn=GetUltraliskCavern(__instance.PrototypeRoot,"UltraliskCavernNoxiousPrefab");
                    udn.name="SC2Expansion-UltraliskCavern";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("UltraliskCavernPrimalPrefab")){
                    var udn=GetUltraliskCavern(__instance.PrototypeRoot,"UltraliskCavernPrimalPrefab");
                    udn.name="SC2Expansion-UltraliskCavern";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("UltraliskCavernApocaliskPrefab")){
                    var udn=GetUltraliskCavern(__instance.PrototypeRoot,"UltraliskCavernApocaliskPrefab");
                    udn.name="SC2Expansion-UltraliskCavern";
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
        public static UnityDisplayNode GetUltraliskCavern(Transform transform,string model){
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
                if(reference!=null&&reference.guidRef.Equals("UltraliskCavernIcon")){
                    var b=Assets.LoadAsset("UltraliskCavernIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("UltraliskCavernPortrait")){
                    var b=Assets.LoadAsset("UltraliskCavernPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("UltraliskCavernChitinousPlatingIcon")){
                    var b=Assets.LoadAsset("UltraliskCavernChitinousPlatingIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("UltraliskCavernAnabolicSynthesisIcon")){
                    var b=Assets.LoadAsset("UltraliskCavernAnabolicSynthesisIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("UltraliskCavernNoxiousIcon")){
                    var b=Assets.LoadAsset("UltraliskCavernNoxiousIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("UltraliskCavernPrimalIcon")){
                    var b=Assets.LoadAsset("UltraliskCavernPrimalIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("UltraliskCavernApocaliskIcon")){
                    var b=Assets.LoadAsset("UltraliskCavernApocaliskIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
    }
}