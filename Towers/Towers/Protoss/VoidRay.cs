namespace SC2Expansion.Towers{
    public class VoidRay:ModTower{
        public override string DisplayName=>"Void Ray";
        public override string TowerSet=>MAGIC;
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>1900;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Flying Protoss ranged craft. Deals double damage against Moab's";
        public override void ModifyBaseTowerModel(TowerModel VoidRay){
            VoidRay.display="VoidRayPrefab";
            VoidRay.portrait=new("VoidRayPortrait");
            VoidRay.icon=new("VoidRayIcon");
            VoidRay.emoteSpriteLarge=new("Protoss");
            VoidRay.radius=7;
            VoidRay.range=55;
            VoidRay.areaTypes=new(4);
            VoidRay.areaTypes[0]=AreaType.land;
            VoidRay.areaTypes[1]=AreaType.track;
            VoidRay.areaTypes[2]=AreaType.ice;
            VoidRay.areaTypes[3]=AreaType.water;
            var Beam=VoidRay.GetAttackModel();
            Beam.weapons[0]=Game.instance.model.towers.First(a=>a.name.Contains("Adora 10")).behaviors.First(a=>a.name.Contains("Ball")).Cast<AbilityModel>().behaviors.
                First(a=>a.name.Contains("CreateTower")).Cast<AbilityCreateTowerModel>().towerModel.GetAttackModel().weapons[0].Duplicate();
            Beam.name="VoidRayBeam";
            Beam.range=VoidRay.range;
            Beam.weapons[0].projectile.GetDamageModel().damage=1.5f;
            Beam.weapons[0].projectile.pierce=1;
            Beam.weapons[0].rate=0.1f;
            Beam.weapons[0].projectile.behaviors=Beam.weapons[0].projectile.behaviors.Add(Game.instance.model.towers.First(a=>a.name.Contains("BombShooter-030")).GetAttackModel().
                    weapons[0].projectile.behaviors.First(a=>a.name.Contains("CreateProjectile")).Cast<CreateProjectileOnContactModel>().projectile.behaviors.First(a=>a.name.
                    Contains("DamageModifierForTag")).Duplicate());
            Beam.weapons[0].projectile.behaviors.First(a=>a.name.Contains("DamageModifier")).Cast<DamageModifierForTagModel>().damageAddative=0;
            Beam.weapons[0].projectile.behaviors.First(a=>a.name.Contains("DamageModifier")).Cast<DamageModifierForTagModel>().damageMultiplier=2;
            VoidRay.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="VoidRayPrefab";
        }
        public class PrismaticAlignment:ModUpgrade<VoidRay>{
            public override string Name=>"PrismaticAlignment";
            public override string DisplayName=>"Prismatic Alignment";
            public override string Description=>"Training a better void lens adds more damage";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel VoidRay){
                GetUpgradeModel().icon=new("VoidRayPrismaticAlignmentIcon");
                var Beam=VoidRay.GetAttackModel();
                Beam.weapons[0].projectile.GetDamageModel().damage=2.25f;
            }
        }
        public class FluxVanes:ModUpgrade<VoidRay>{
            public override string Name=>"FluxVanes";
            public override string DisplayName=>"Flux Vanes";
            public override string Description=>"Better flux vanes allow for a faster attack";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel VoidRay){
                GetUpgradeModel().icon=new("VoidRayFluxVanesIcon");
                var Beam=VoidRay.GetAttackModel();
                Beam.weapons[0].rate=0.06f;
            }
        }
        public class PrismaticRange:ModUpgrade<VoidRay>{
            public override string Name=>"PrismaticRange";
            public override string DisplayName=>"Prismatic Range";
            public override string Description=>"Refining the prismatic core even more lets it fire further";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel VoidRay){
                GetUpgradeModel().icon=new("VoidRayPrismaticRangeIcon");
                var Beam=VoidRay.GetAttackModel();
                Beam.range=65;
                VoidRay.range=Beam.range;
            }
        }
        //i tried for 2 and a half days to get the beam to bounce, it didn't wanna fucking bounce at all
        public class Destroyer:ModUpgrade<VoidRay>{
            public override string Name=>"Destroyer";
            public override string DisplayName=>"Destroyer";
            public override string Description=>"Stolen Void Rays that have had their prismatic cores replaced with bloodshard crystals. Beam sometimes explodes"+
                " upon hitting a target sending seeking shards everywhere";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel VoidRay){
                GetUpgradeModel().icon=new("VoidRayDestroyerIcon");
                VoidRay.portrait=new("VoidRayDestroyerPortrait");
                VoidRay.display="VoidRayDestroyerPrefab";
                var Shards=Game.instance.model.towers.First(a=>a.name.Contains("BombShooter-002")).GetAttackModel().Duplicate();
                VoidRay.behaviors=VoidRay.behaviors.Add();
                Shards.weapons[0].projectile.behaviors.First(a=>a.name.Contains("frag")).Cast<CreateProjectileOnContactModel>().projectile=
                    Game.instance.model.towers.First(a=>a.name.Contains("MonkeyAce-003")).GetAttackModel().weapons[0].projectile.Duplicate();
                Shards.weapons[0].projectile.behaviors.First(a=>a.name.Contains("frag")).Cast<CreateProjectileOnContactModel>().projectile.behaviors.First(a=>a.name.
                    Contains("Damage")).Cast<DamageModel>().immuneBloonProperties=0;
                var ShardsProj=Shards.weapons[0].projectile.behaviors.First(a=>a.name.Contains("frag")).Cast<CreateProjectileOnContactModel>().projectile;
                var ShardsTrack=ShardsProj.behaviors.First(a=>a.name.Contains("Track")).Cast<TrackTargetModel>();
                Shards.weapons[0].projectile.behaviors.First(a=>a.name.Equals("CreateProjectileOnContactModel_")).Cast<CreateProjectileOnContactModel>().projectile.behaviors.
                    First(a=>a.name.Contains("Damage")).Cast<DamageModel>().immuneBloonProperties=0;
                ShardsTrack.maxSeekAngle=360;
                ShardsTrack.ignoreSeekAngle=true;
                ShardsTrack.TurnRate=999;
                ShardsTrack.trackNewTargets=true;
                ShardsTrack.constantlyAquireNewTarget=true;
                Shards.range=VoidRay.range;
                Shards.weapons[0].projectile.display=null;
                ShardsProj.pierce=2;
                ShardsProj.display="dfc16ec49f4894148bce0161ebb0bd32";
                Shards.name="Shards";
                VoidRay.behaviors=VoidRay.behaviors.Add(Shards);
            }
        }
        public class Mohandar:ModUpgrade<VoidRay>{
            public override string Name=>"Mohandar";
            public override string DisplayName=>"Mohandar";
            public override string Description=>"\"We linger in twilight\"";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel VoidRay){
                GetUpgradeModel().icon=new("VoidRayMohandarPortrait");
                VoidRay.portrait=new("VoidRayMohandarPortrait");
                VoidRay.display="VoidRayMohandarPrefab";
                VoidRay.behaviors=VoidRay.behaviors.Remove(a=>a.name.Equals("Shards"));
                var Beam=VoidRay.GetAttackModel();
                Beam.weapons[0].projectile.GetDamageModel().damage=3.5f;
                Beam.range=80;
                VoidRay.range=Beam.range;
                Beam.weapons[0].projectile.pierce=1;
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("VoidRayPrefab")){
                    var udn=GetVoidRay(__instance.PrototypeRoot,"VoidRayPrefab");
                    udn.name="VoidRay";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("VoidRayDestroyerPrefab")){
                    var udn=GetVoidRay(__instance.PrototypeRoot,"VoidRayDestroyerPrefab");
                    udn.name="VoidRay";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("VoidRayMohandarPrefab")){
                    var udn=GetVoidRay(__instance.PrototypeRoot,"VoidRayMohandarPrefab");
                    udn.name="VoidRay";
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
        public static UnityDisplayNode GetVoidRay(Transform transform,string model){
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
                if(reference!=null&&reference.guidRef.Equals("VoidRayIcon")){
                    var b=Assets.LoadAsset("VoidRayIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VoidRayPortrait")){
                    var b=Assets.LoadAsset("VoidRayPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VoidRayPrismaticAlignmentIcon")){
                    var b=Assets.LoadAsset("VoidRayPrismaticAlignmentIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VoidRayFluxVanesIcon")){
                    var b=Assets.LoadAsset("VoidRayFluxVanesIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VoidRayPrismaticRangeIcon")){
                    var b=Assets.LoadAsset("VoidRayPrismaticRangeIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VoidRayDestroyerIcon")){
                    var b=Assets.LoadAsset("VoidRayDestroyerIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VoidRayMohandarPortrait")) {
                    var b=Assets.LoadAsset("VoidRayMohandarPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VoidRayDestroyerPortrait")){
                    var b=Assets.LoadAsset("VoidRayDestroyerPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
    }
}