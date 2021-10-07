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
            Viking.portrait=new("VikingPortrait");
            Viking.icon=new("VikingGroundIcon");
            Viking.emoteSpriteLarge=new("Terran");
            Viking.radius=5;
            Viking.cost=400;
            Viking.range=45;
            var Gatling=Viking.behaviors.First(a=>a.name.Contains("AttackModel")).Cast<AttackModel>();
            Gatling.range=Viking.range;
            Gatling.GetBehavior<AttackFilterModel>().filters=Gatling.GetBehavior<AttackFilterModel>().filters.Add(new FilterOutTagModel("FilterOutTagModel","Moabs",null));
            Gatling.weapons[0].name="VikingGatling";
            Gatling.weapons[0].projectile.GetDamageModel().damage=2;
            Gatling.weapons[0].projectile.GetDamageModel().immuneBloonProperties=(BloonProperties)17;
            Gatling.weapons[0].ejectX=7.5f;
            Gatling.weapons[0].ejectY=10;
            Gatling.weapons[0].rate=1.1f;
            Gatling.AddWeapon(Gatling.weapons[0].Duplicate());
            Gatling.weapons[1].ejectX=-Gatling.weapons[0].ejectX;
            Viking.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display=Viking.display;
        }
        public class AirMode:ModUpgrade<Viking>{
            public override string Name=>"AirMode";
            public override string DisplayName=>"Fighter mode";
            public override string Description=>"Trains the pilot to transform and make use of the Fighter mode for a short time gaining bonus damage against Moab's. Only targets Moab's in Fighter Mode";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingAirIcon");
                var AirMode=Game.instance.model.towers.First(a=>a.name.Contains("Alchemist-040")).GetAbility().Duplicate();
                AirMode.GetBehavior<ActivateAttackModel>().attacks[0]=Game.instance.model.towers.First(a=>a.name.Contains("BombShooter-030")).GetAttackModel();
                var Lanzer=AirMode.GetBehavior<ActivateAttackModel>().attacks[0];
                AirMode.GetBehavior<ActivateAttackModel>().lifespan=15;
                AirMode.GetBehavior<SwitchDisplayModel>().lifespan=15;
                AirMode.GetBehavior<SwitchDisplayModel>().display="VikingAirPrefab";
                AirMode.GetBehavior<IncreaseRangeModel>().addative=30;
                AirMode.cooldown=0.1f;
                AirMode.icon=new("VikingAirIcon");
                Lanzer.GetBehavior<AttackFilterModel>().filters=Lanzer.GetBehavior<AttackFilterModel>().filters.Add(new FilterWithTagModel("FilterWithTagModel","Moabs",false));
                Lanzer.range=Viking.range+AirMode.GetBehavior<IncreaseRangeModel>().addative;
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
                Viking.GetAttackModel().range=Viking.range;
                Viking.GetAbility().GetBehavior<IncreaseRangeModel>().addative=40;
                Viking.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].range=Viking.range+Viking.GetAbility().GetBehavior<IncreaseRangeModel>().addative;
            }
        }
        public class Deimos:ModUpgrade<Viking>{
            public override string Name=>"Deimos";
            public override string DisplayName=>"Deimos Viking";
            public override string Description=>"Dominion Vikings modified with uh, \'legal\' mercenary equipment, more pierce, damage, better fire rate and can pop Lead Bloons on the ground and more rockets in the air";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingDeimosGroundIcon");
                var Gatling=Viking.GetAttackModel();
                var AirMode=Viking.GetAbility();
                AirMode.GetBehavior<ActivateAttackModel>().attacks=AirMode.GetBehavior<ActivateAttackModel>().attacks.Add(Game.instance.model.towers.First(a=>a.name.
                    Contains("BombShooter-020")).GetAttackModel());
                var WILD=AirMode.GetBehavior<ActivateAttackModel>().attacks[1];
                Gatling.weapons[0].projectile.pierce=3;
                Gatling.weapons[0].projectile.GetDamageModel().damage=3;
                Gatling.weapons[0].projectile.GetDamageModel().immuneBloonProperties=0;
                Gatling.weapons[0].rate=0.55f;
                Gatling.weapons[1]=Gatling.weapons[0].Duplicate();
                Gatling.weapons[1].ejectX=-Gatling.weapons[0].ejectX;
                AirMode.GetBehavior<SwitchDisplayModel>().display="VikingDeimosAirPrefab";
                AirMode.icon=new("VikingDeimosAirIcon");
                WILD.weapons[0].emission=new RandomArcEmissionModel("RandomArcEmissionModel",8,30,90,10,10,null);
                WILD.weapons[0].projectile.RemoveBehavior<TravelStraitModel>();
                WILD.weapons[0].projectile.AddBehavior(Game.instance.model.towers.First(a=>a.name.Contains("DartlingGunner-050")).GetAbility().GetBehavior<ActivateAttackModel>().
                    attacks[0].weapons[0].projectile.GetBehavior<TravelCurvyModel>().Duplicate());
                WILD.weapons[0].projectile.GetBehavior<TravelCurvyModel>().speed=200;
                WILD.weapons[0].projectile.RemoveBehaviors<TrackTargetModel>();
                WILD.range=Viking.range+AirMode.GetBehavior<IncreaseRangeModel>().addative;
                WILD.weapons[0].projectile.display="VikingMissilePrefab";
                WILD.weapons[0].ejectZ=50;
                WILD.GetBehavior<AttackFilterModel>().filters=WILD.GetBehavior<AttackFilterModel>().filters.Add(new FilterWithTagModel("FilterWithTagModel","Moabs",false));
                Viking.portrait=new("VikingDeimosPortrait");
                Viking.display="VikingDeimosGroundPrefab";
            }
        }
        public class SkyFury:ModUpgrade<Viking>{
            public override string Name=>"SkyFury";
            public override string DisplayName=>"Sky Fury";
            public override string Description=>"Elite Dominion Vikings, doubles the time spent in Fighter mode and can now attack Moab's in ground mode";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingSkyFuryGroundIcon");
                var AirMode=Viking.GetAbility();
                AirMode.icon=new("VikingSkyFuryAirIcon");
                AirMode.GetBehavior<SwitchDisplayModel>().display="VikingSkyFuryAirPrefab";
                AirMode.GetBehavior<SwitchDisplayModel>().lifespan=30;
                AirMode.GetBehavior<ActivateAttackModel>().lifespan=30;
                Viking.portrait=new("VikingSkyFuryPortrait");
                Viking.display="VikingSkyFuryGroundPrefab";
                Viking.GetAttackModel().GetBehavior<AttackFilterModel>().filters=Viking.GetAttackModel().GetBehavior<AttackFilterModel>().filters.Remove(a=>a.name.Contains("TagModel"));
            }
        }
        public class Archangel:ModUpgrade<Viking>{
            public override string Name=>"Archangel";
            public override string DisplayName=>"Archangel";
            public override string Description=>"The pride of the Dominion Engineering Corps, a Archangel is the solution to almost any threat";
            public override int Cost=>9000;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Viking){
                GetUpgradeModel().icon=new("VikingArchAngelIcon");
                var WILD=Viking.GetAbility().GetBehavior<ActivateAttackModel>().attacks[1].Duplicate();
                var Gatling=Viking.GetAttackModel();
                WILD.range=85;
                WILD.weapons[0].projectile.RemoveBehavior<CreateProjectileOnContactModel>();
                WILD.weapons[0].projectile.AddBehavior(Game.instance.model.towers.First(a=>a.name.Contains("BombShooter-022")).GetAttackModel().weapons[0].projectile.
                    GetBehavior<CreateProjectileOnContactModel>().Duplicate());
                Gatling.weapons[0].rate=0.1f;
                Gatling.range=110;
                Gatling.weapons[0].ejectX=13;
                Gatling.weapons[0].ejectY=20;
                Gatling.weapons[0].ejectZ=15;
                Gatling.weapons[0].projectile.GetDamageModel().damage=15;
                Gatling.weapons[1]=Gatling.weapons[0].Duplicate();
                Gatling.weapons[1].ejectX=-Gatling.weapons[0].ejectX;
                Viking.RemoveBehavior<AbilityModel>();
                Viking.portrait=new("VikingArchangelPortrait");
                Viking.display="VikingArchangelPrefab";
                Viking.range=Gatling.range;
                Viking.AddBehavior(WILD);
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Contains("Viking")){
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
        [HarmonyPatch(typeof(ResourceLoader),nameof(ResourceLoader.LoadSpriteFromSpriteReferenceAsync))]
        public record ResourceLoader_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.Contains("Viking")){
                    var text=Assets.LoadAsset(reference.guidRef).Cast<Texture2D>();
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
                var TowerName=__instance.attack.tower.towerModel.name;
                if(__instance.weaponModel.name.Equals("WeaponModel_VikingGatling")){
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().StopPlayback();
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("VikingGroundAttack");
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack",true);
                    float wait=1120;
                    if(TowerName.Contains("3")||TowerName.Contains("4")){
                        wait=550;
                    }
                    if(TowerName.Contains("5")){
                        wait=100;
                    }
                    await Task.Run(()=>{
                        while(wait>0){
                        wait-=TimeManager.timeScaleWithoutNetwork+1;
                        Task.Delay(1);}
                        return;
                    });
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack",false);
                }
            }
        }
    }
}