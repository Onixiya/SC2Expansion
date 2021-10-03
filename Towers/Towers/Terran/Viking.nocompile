namespace SC2Expansion.Towers{
    public class Viking:ModTower{
        public override string DisplayName=>"Viking";
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"SniperMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Terran ground fire support, fire's 2 powerful Gatling Cannons. Cannot target Moab's";
        public override void ModifyBaseTowerModel(TowerModel Viking){
            Viking.display="VikingGroundPrefab";
            Viking.portrait=new("VikingGroundPortrait");
            Viking.icon=new("VikingGroundIcon");
            Viking.emoteSpriteLarge=new("Terran");
            Viking.radius=5;
            Viking.cost=400;
            Viking.range=45;
            var Gatling=Viking.behaviors.First(a=>a.name.Contains("AttackModel")).Cast<AttackModel>();
            Gatling.range=Viking.range;
            Gatling.weapons[0].name="VikingGatling";
            Gatling.weapons[0].projectile.GetDamageModel().damage=2;
            Gatling.weapons[0].ejectX=7.5f;
            Gatling.weapons[0].ejectY=10;
            Gatling.weapons[0].rate=1;
            Gatling.AddWeapon(Gatling.weapons[0].Duplicate());
            Gatling.weapons[1].ejectX=-Gatling.weapons[0].ejectX;
            Viking.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display=Viking.display;
        }
        public override int GetTowerIndex(List<TowerDetailsModel>towerSet){
            return towerSet.First(model=>model.towerId==TowerType.BoomerangMonkey).towerIndex+1;
        }
        public class AirrMode:ModUpgrade<Viking>{
            public override string Name=>"AirMode";
            public override string DisplayName=>"Fighter mode";
            public override string Description=>"Trains the pilot to transform and make use of the Fighter mode gaining bonus damage against Moab's for a short time. Can only target Moab's";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingAirIcon");
                var AirMode=Game.instance.model.towers.First(a=>a.name.Contains("Alchemist-040")).GetBehavior<AbilityModel>();
                AirMode.GetBehavior<ActivateAttackModel>().attacks[0]=Game.instance.model.towers.First(a=>a.name.Contains("BombShooter-030")).GetAttackModel();
                var Lanzer=AirMode.GetBehavior<ActivateAttackModel>().attacks[0];
                AirMode.icon=new("VikingAirIcon");
                AirMode.cooldown=0.1f;
                AirMode.name="AirMode";
                AirMode.GetBehavior<SwitchDisplayModel>().display="VikingAirPrefab";
                AirMode.GetBehavior<IncreaseRangeModel>().addative=45;
                Lanzer.weapons[0].projectile.display="VikingMissilePrefab";
                Lanzer.weapons[0].ejectZ=50;
                Lanzer.weapons[0].projectile.AddBehavior(new TrackTargetModel("TrackTargetModel",999,false,false,90,false,360,false,false));
                Viking.AddBehavior(AirMode);
            }
        }
        public class PhobosWeapons:ModUpgrade<Viking>{
            public override string Name=>"PhobosWeapons";
            public override string DisplayName=>"Phobos Class weapons systems";
            public override string Description=>"Increases range and damage in all modes";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingPhobosWeaponsIcon");
                Viking.range=55;
            }
        }
        public class Deimos:ModUpgrade<Viking>{
            public override string Name=>"Deimos";
            public override string DisplayName=>"Deimos Viking";
            public override string Description=>"Dominion Vikings modified with uh, \'legal\' mercenary equipment, more pierce and damage on the ground and more rockets in the air";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingDeimosGroundIcon");
            }
        }
        public class SkyFury:ModUpgrade<Viking>{
            public override string Name=>"SkyFury";
            public override string DisplayName=>"Sky Fury";
            public override string Description=>"Elite Dominion Vikings, dramitically increases the time spent in Fighter mode and can now attack Moab's in ground mode";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingSkyFuryGroundIcon");
            }
        }
        public class ArchAngel:ModUpgrade<Viking>{
            public override string Name=>"ArchAngel";
            public override string DisplayName=>"Arch Angel";
            public override string Description=>"\"You ready for war?\"";
            public override int Cost=>9000;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingArchAngelIcon");
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("VikingGroundPrefab")){
                    var udn=GetViking(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-Viking";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("VikingAirPrefab")){
                    var udn=GetViking(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-Viking";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("VikingMissilePrefab")){
                    var udn=GetViking(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-Viking";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("VikingDeimosGroundPrefab")){
                    var udn=GetViking(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-Viking";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("VikingDeimosAirPrefab")){
                    var udn=GetViking(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-Viking";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("VikingSkyFuryGroundPrefab")){
                    var udn=GetViking(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-Viking";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("VikingSkyFuryAirPrefab")){
                    var udn=GetViking(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-Viking";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("VikingArchAngelGroundPrefab")){
                    var udn=GetViking(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-Viking";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("VikingArchAngelAirPrefab")){
                    var udn=GetViking(__instance.PrototypeRoot,objectId);
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
                if(reference!=null&&reference.guidRef.Equals("VikingGroundIcon")){
                    var text = Assets.LoadAsset("VikingGroundIcon").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingAirIcon")){
                    var text = Assets.LoadAsset("VikingAirIcon").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingGroundPortrait")){
                    var text = Assets.LoadAsset("VikingGroundPortrait").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingAirPortrait")){
                    var text = Assets.LoadAsset("VikingAirPortrait").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingDeimosGroundIcon")){
                    var text = Assets.LoadAsset("VikingDeimosGroundIcon").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingDeimosAirIcon")){
                    var text = Assets.LoadAsset("VikingDeimosAirIcon").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingDeimosGroundPortrait")){
                    var text = Assets.LoadAsset("VikingDeimosGroundPortrait").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingDeimosAirPortrait")){
                    var text = Assets.LoadAsset("VikingDeimosAirPortrait").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingSkyFuryGroundIcon")){
                    var text = Assets.LoadAsset("VikingSkyFuryGroundIcon").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingSkyFuryAirIcon")){
                    var text = Assets.LoadAsset("VikingSkyFuryAirIcon").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingSkyFuryGroundPortrait")){
                    var text = Assets.LoadAsset("VikingSkyFuryGroundPortrait").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingSkyFuryAirPortrait")){
                    var text = Assets.LoadAsset("VikingSkyFuryAirPortrait").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingArchAngelIcon")){
                    var text = Assets.LoadAsset("VikingArchAngelIcon").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingArchAngelGroundPortrait")){
                    var text=Assets.LoadAsset("VikingArchAngelGroundPortrait").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingArchAngelAirPortrait")){
                    var text=Assets.LoadAsset("VikingArchAngelAirPortrait").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("VikingPhobosWeaponsIcon")){
                    var text=Assets.LoadAsset("VikingPhobosWeaponsIcon").Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),nameof(Weapon.SpawnDart))]
        public static class WI{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance)=>RunAnimations(__instance);
            private static async Task RunAnimations(Weapon __instance){
                if(__instance.weaponModel.name.Equals("WeaponModel_VikingGatling")){
                    MelonLogger.Msg(__instance.weaponModel.name);
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().StopPlayback();
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("VikingGroundAttack");
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack",true);
                    float wait=1000;
                    await Task.Run(()=>{
                        while(wait>0){
                        wait-=TimeManager.timeScaleWithoutNetwork+1;
                        Task.Delay(1);}
                        return;
                    });
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack", false);
                }
            }
        }
    }
}