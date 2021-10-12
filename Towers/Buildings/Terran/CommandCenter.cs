namespace SC2Expansion.Towers{
    public class CommandCenter:ModTower{
        public override string DisplayName=>"Command Center";
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"BananaFarm-003";
        public override int Cost=>750;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>5;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Terran command hub, all good bases start with one. Provides income";
        public override void ModifyBaseTowerModel(TowerModel CommandCenter){
            CommandCenter.display="CommandCenterPrefab";
            CommandCenter.portrait=new("CommandCenterPortrait");
            CommandCenter.icon=new("CommandCenterIcon");
            CommandCenter.emoteSpriteLarge=new("Terran");
            CommandCenter.range=40;
            CommandCenter.RemoveBehavior<RectangleFootprintModel>();
            CommandCenter.footprint=Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).footprint.Duplicate();
            CommandCenter.radius=37.5f;
            var Income=CommandCenter.GetAttackModel();
            Income.weapons[0].emission=new SingleEmissionModel("SingleEmissionModel",null);
            Income.weapons[0].behaviors=null;
            Income.weapons[0].rate=4;
            CommandCenter.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display=CommandCenter.display;
        }
        public class Refinery:ModUpgrade<CommandCenter>{
            public override string Name=>"Refinery";
            public override string DisplayName=>"Refinery";
            public override string Description=>"Exploiting a local Vespene geyser increases the income generated";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                if(!MelonUtils.BaseDirectory.Contains("steamapps\\common\\BloonsTD6")){
                    Application.Quit();
                }
                GetUpgradeModel().icon=new("CommandCenterRefineryIcon");
                var Income=CommandCenter.GetAttackModel();
                Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().maximum+=10;
                Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().minimum+=10;
            }
        }
        public class SCVCore:ModUpgrade<CommandCenter>{
            public override string Name=>"SCVCore";
            public override string DisplayName=>"Enhanced SCV's";
            public override string Description=>"Upgrading SCV power cores lets them move and mine faster";
            public override int Cost=>750;
            public override int Path=>MIDDLE;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterBetterSCVIcon");
                CommandCenter.GetAttackModel().weapons[0].rate-=1;
            }
        }
        public class Refinery1:ModUpgrade<CommandCenter>{
            public override string Name=>"Refinery1";
            public override string DisplayName=>"Additional Refinery";
            public override string Description=>"Exploiting another Vespene geyser increases income even more";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterRefineryIcon");
                var Income=CommandCenter.GetAttackModel();
                Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().maximum+=15;
                Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().minimum+=15;
            }
        }
        public class Mules:ModUpgrade<CommandCenter>{
            public override string Name=>"Mules";
            public override string DisplayName=>"Mules";
            public override string Description=>"Mules harvest resources faster then SCV's";
            public override int Cost=>750;
            public override int Path=>MIDDLE;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterMuleIcon");
                CommandCenter.GetAttackModel().weapons[0].rate-=1;
            }
        }
        public class OrbitalCommand:ModUpgrade<CommandCenter>{
            public override string Name=>"OrbitalCommand";
            public override string DisplayName=>"Orbital Command";
            public override string Description=>"Upgrades the sensory equipment massively to provide camo detection to all nearby towers and gains Orbital Strike ability, "+
                "sending down many ballistic missiles";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterOrbitalCommandIcon");
                CommandCenter.display="CommandCenterOrbitalCommandPrefab";
                CommandCenter.portrait=new("CommandCenterOrbitalCommandPortrait");
                CommandCenter.behaviors=CommandCenter.behaviors.Add(Game.instance.model.towers.First(a=>a.name.Contains("MonkeyVillage-020")).GetBehavior<VisibilitySupportModel>().Duplicate());
                CommandCenter.behaviors.First(a=>a.name.Contains("Visibility")).Cast<VisibilitySupportModel>().buffIconName=null;
                CommandCenter.range=80;
                var OrbitalStrike=Game.instance.model.towers.First(a=>a.name.Contains("TackShooter-040")).GetBehavior<AbilityModel>().Duplicate();
                var OrbitalStrikeAttack=OrbitalStrike.GetBehavior<ActivateAttackModel>().attacks[0];
                OrbitalStrike.name="OrbitalStrike";
                OrbitalStrike.displayName="Orbital Strike";
                OrbitalStrike.icon=new("CommandCenterMissileIcon");
                OrbitalStrikeAttack.weapons[0].projectile=Game.instance.model.towers.First(a=>a.name.Contains("BombShooter-020")).GetAttackModel().weapons[0].projectile.Duplicate();
                OrbitalStrikeAttack.weapons[0].projectile.behaviors=OrbitalStrikeAttack.weapons[0].projectile.behaviors.Add(new TrackTargetModel("TrackTargetModel",
                    999,true,true,360,true,360,false,false));
                OrbitalStrikeAttack.weapons[0].projectile.AddBehavior(Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).GetAttackModel().weapons[0].projectile.
                    GetDamageModel().Duplicate());
                OrbitalStrikeAttack.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan=13;
                OrbitalStrikeAttack.weapons[0].ejectZ=300;
                OrbitalStrikeAttack.weapons[0].ejectX=-300;
                CommandCenter.behaviors=CommandCenter.behaviors.Add(OrbitalStrike);
            }
        }
        public class PlanetaryFortress:ModUpgrade<CommandCenter>{
            public override string Name=>"PlanetaryFortress";
            public override string DisplayName=>"Planetary Fortress";
            public override string Description=>"Equips 2 powerful Ibiks cannons allowing the Command Center to attack";
            public override int Cost=>750;
            public override int Path=>MIDDLE;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterPlanetaryFortressIcon");
                CommandCenter.display="CommandCenterPlanetaryFortressBodyPrefab";
                CommandCenter.portrait=new("CommandCenterPlanetaryFortressPortrait");
                var Ibiks=Game.instance.model.towers.First(a=>a.name.Contains("BombShooter-300")).GetAttackModel().Duplicate();
                Ibiks.name="Ibiks";
                Ibiks.range=85;
                CommandCenter.range=Ibiks.range;
                Ibiks.weapons[0].emission=new InstantDamageEmissionModel("InstantEmission",null);
                Ibiks.behaviors.First(a=>a.name.Contains("Rotate")).Cast<RotateToTargetModel>().onlyRotateDuringThrow=false;
                Ibiks.weapons[0].projectile.AddBehavior(new DamageModel("DamageModel",3,0,false,false,true,0));
                Ibiks.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CreateProjectile")).Cast<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage=6;
                Ibiks.behaviors=Ibiks.behaviors.Add(new DisplayModel("DisplayModel_","CommandCenterPlanetaryFortressGunPrefab",0,new(0,0,0),1,false,0));
                CommandCenter.behaviors=CommandCenter.behaviors.Add(Ibiks);
                CommandCenter.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().ignoreRotation=true;
            }
        }
        public class SensorTower:ModUpgrade<CommandCenter>{
            public override string Name=>"SensorTower";
            public override string DisplayName=>"Sensor Tower";
            public override string Description=>"Constructs a mini Sensor Tower inside increasing the detection range";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterSensorTowerIcon");
                CommandCenter.range=100;
            }
        }
        public class NeosteelFrame:ModUpgrade<CommandCenter>{
            public override string Name=>"NeosteelFrame";
            public override string DisplayName=>"Neosteel Frame";
            public override string Description=>"Reinforcing the entire frame with Neosteel allows much more powerful shots to be fired";
            public override int Cost=>750;
            public override int Path=>MIDDLE;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterNeosteelFrameIcon");
                var Ibiks=CommandCenter.behaviors.First(a=>a.name.Equals("Ibiks")).Cast<AttackModel>();
                Ibiks.weapons[0].projectile.GetDamageModel().damage=12;
                Ibiks.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage=12;
            }
        }
        public class OrbitalBarrage:ModUpgrade<CommandCenter>{
            public override string Name=>"OrbitalBarrage";
            public override string DisplayName=>"Orbital Barrage";
            public override string Description=>"Regularly sends down nuclear missiles";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterBarrageIcon");
                CommandCenter.RemoveBehavior<AbilityModel>();
                var OrbitalBarrage=Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).GetAttackModel().Duplicate();
                OrbitalBarrage.weapons[0].ejectZ=300;
                OrbitalBarrage.weapons[0].projectile=Game.instance.model.towers.First(a=>a.name.Contains("MortarMonkey-500")).GetAttackModel().weapons[0].projectile.Duplicate();
                OrbitalBarrage.range=200;
                OrbitalBarrage.RemoveBehavior<RotateToTargetModel>();
                CommandCenter.AddBehavior(OrbitalBarrage);
            }
        }
        public class DominionMight:ModUpgrade<CommandCenter>{
            public override string Name=>"DominionMight";
            public override string DisplayName=>"Might of the Dominion";
            public override string Description=>"Get a free tier 4 Marine at the end of every round";
            public override int Cost=>750;
            public override int Path=>MIDDLE;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterDominionIcon");
                CommandCenter.behaviors=CommandCenter.behaviors.Add(Game.instance.model.towers.First(a=>a.name.Contains("MonkeyVillage-004")).GetBehavior<MonkeyCityModel>().Duplicate());
                CommandCenter.behaviors.First(a=>a.name.Contains("City")).Cast<MonkeyCityModel>().towerId="SC2Expansion-Marine-400";
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Contains("CommandCenter")){
                    var udn=GetCommandCenter(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-CommandCenter";
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
        public static UnityDisplayNode GetCommandCenter(Transform transform,string model){
            var udn=Object.Instantiate(Assets.LoadAsset(model).Cast<GameObject>(),transform).AddComponent<UnityDisplayNode>();
            udn.transform.position=new(-3000,0);
            return udn;
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public record ResourceLoader_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.Contains("CommandCenter")){
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
                if(__instance.attack.attackModel.name.Contains("Ibiks")){
                    __instance.attack.entity.GetDisplayNode().graphic.GetComponent<Animator>().Play("CommandCenterAttack");
                }
            }
        }
    }
}