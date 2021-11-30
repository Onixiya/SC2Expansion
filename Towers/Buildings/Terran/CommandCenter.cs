namespace SC2Expansion.Towers{
    public class CommandCenter:ModTower<TerranSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.commandcenter);
        public override string DisplayName=>"Command Center";
        public override string BaseTower=>"BananaFarm-003";
        public override int Cost=>850;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>5;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!TerranEnabled;
        public override string Description=>"Terran command hub, all good bases start with one. Provides income";
        public override void ModifyBaseTowerModel(TowerModel CommandCenter){
            CommandCenter.display="CommandCenterPrefab";
            CommandCenter.portrait=new("CommandCenterPortrait");
            CommandCenter.icon=new("CommandCenterIcon");
            CommandCenter.emoteSpriteLarge=new("Terran");
            CommandCenter.range=40;
            CommandCenter.footprint=Game.instance.model.GetTowerFromId("DartMonkey").footprint.Duplicate();
            CommandCenter.radius=37.5f;
            var Income=CommandCenter.GetAttackModel();
            Income.name="Income";
            Income.weapons[0].emission=new SingleEmissionModel("SingleEmissionModel",null);
            Income.weapons[0].behaviors=null;
            Income.weapons[0].rate=4;
            CommandCenter.GetBehavior<DisplayModel>().display=CommandCenter.display;
        }
        public class Refinery:ModUpgrade<CommandCenter>{
            public override string DisplayName=>"Refinery";
            public override string Description=>"Exploiting a local Vespene geyser increases the income generated";
            public override int Cost=>770;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                if(!MelonUtils.BaseDirectory.Contains("steamapps\\common\\BloonsTD6"))Application.Quit();
                GetUpgradeModel().icon=new("CommandCenterRefineryIcon");
                var Income=CommandCenter.GetAttackModel();
                Income.weapons[0].projectile.GetBehavior<CashModel>().maximum+=10;
                Income.weapons[0].projectile.GetBehavior<CashModel>().minimum+=10;
            }
        }
        public class SCVCore:ModUpgrade<CommandCenter>{
            public override string DisplayName=>"Enhanced SCV's";
            public override string Description=>"Upgrading SCV power cores lets them move and mine faster";
            public override int Cost=>655;
            public override int Path=>MIDDLE;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterBetterSCVIcon");
                CommandCenter.GetAttackModel().weapons[0].rate-=1;
            }
        }
        public class Refinery1:ModUpgrade<CommandCenter>{
            public override string DisplayName=>"Additional Refinery";
            public override string Description=>"Exploiting another Vespene geyser increases income even more";
            public override int Cost=>1070;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterRefineryIcon");
                var Income=CommandCenter.GetAttackModel();
                Income.weapons[0].projectile.GetBehavior<CashModel>().maximum+=15;
                Income.weapons[0].projectile.GetBehavior<CashModel>().minimum+=15;
            }
        }
        public class Mules:ModUpgrade<CommandCenter>{
            public override string DisplayName=>"Mules";
            public override string Description=>"Mules harvest resources faster then SCV's";
            public override int Cost=>935;
            public override int Path=>MIDDLE;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterMuleIcon");
                CommandCenter.GetAttackModel().weapons[0].rate-=1;
            }
        }
        public class OrbitalCommand:ModUpgrade<CommandCenter>{
            public override string DisplayName=>"Orbital Command";
            public override string Description=>"Upgrades the sensory equipment massively to provide camo detection to all nearby towers and gains Orbital Strike ability, "+
                "sending down many ballistic missiles";
            public override int Cost=>1855;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterOrbitalCommandIcon");
                CommandCenter.display="CommandCenterOrbitalCommandPrefab";
                CommandCenter.portrait=new("CommandCenterOrbitalCommandPortrait");
                CommandCenter.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyVillage-020").GetBehavior<VisibilitySupportModel>().Duplicate());
                CommandCenter.GetBehavior<VisibilitySupportModel>().buffIconName=null;
                CommandCenter.range=80;
                var OrbitalStrike=Game.instance.model.GetTowerFromId("TackShooter-040").GetBehavior<AbilityModel>().Duplicate();
                var OrbitalStrikeAttack=OrbitalStrike.GetBehavior<ActivateAttackModel>().attacks[0];
                OrbitalStrike.name="OrbitalStrike";
                OrbitalStrike.displayName="Orbital Strike";
                OrbitalStrike.icon=new("CommandCenterMissileIcon");
                OrbitalStrikeAttack.weapons[0].projectile=Game.instance.model.GetTowerFromId("BombShooter-020").GetAttackModel().weapons[0].projectile.Duplicate();
                OrbitalStrikeAttack.weapons[0].projectile.AddBehavior(new TrackTargetModel("TrackTargetModel",999,true,true,360,true,360,false,false));
                OrbitalStrikeAttack.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.GetDamageModel().Duplicate());
                OrbitalStrikeAttack.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan=13;
                OrbitalStrikeAttack.weapons[0].ejectZ=300;
                OrbitalStrikeAttack.weapons[0].ejectX=-300;
                CommandCenter.AddBehavior(OrbitalStrike);
            }
        }
        public class PlanetaryFortress:ModUpgrade<CommandCenter>{
            public override string DisplayName=>"Planetary Fortress";
            public override string Description=>"Equips 2 powerful Ibiks cannons allowing the Command Center to attack";
            public override int Cost=>2160;
            public override int Path=>MIDDLE;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterPlanetaryFortressIcon");
                CommandCenter.display="CommandCenterPlanetaryFortressBodyPrefab";
                CommandCenter.portrait=new("CommandCenterPlanetaryFortressPortrait");
                var Ibiks=Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().Duplicate();
                Ibiks.name="Ibiks";
                CommandCenter.range=85;
                Ibiks.range=CommandCenter.range;
                Ibiks.weapons[0].emission=new InstantDamageEmissionModel("InstantEmission",null);
                Ibiks.GetBehavior<RotateToTargetModel>().onlyRotateDuringThrow=false;
                Ibiks.weapons[0].projectile.AddBehavior(new DamageModel("DamageModel",3,0,false,false,true,0));
                Ibiks.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage=6;
                Ibiks.AddBehavior(new DisplayModel("DisplayModel_","CommandCenterPlanetaryFortressGunPrefab",0,new(0,0,0),1,false,0));
                CommandCenter.AddBehavior(Ibiks);
                CommandCenter.GetBehavior<DisplayModel>().ignoreRotation=true;
            }
        }
        public class SensorTower:ModUpgrade<CommandCenter>{
            public override string DisplayName=>"Sensor Tower";
            public override string Description=>"Constructs a mini Sensor Tower inside increasing the detection range";
            public override int Cost=>7630;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterSensorTowerIcon");
                CommandCenter.range=100;
            }
        }
        public class NeosteelFrame:ModUpgrade<CommandCenter>{
            public override string DisplayName=>"Neosteel Frame";
            public override string Description=>"Reinforcing the entire frame with Neosteel allows much more powerful shots to be fired";
            public override int Cost=>6170;
            public override int Path=>MIDDLE;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterNeosteelFrameIcon");
                var Ibiks=CommandCenter.behaviors.First(a=>a.name=="Ibiks").Cast<AttackModel>();
                Ibiks.weapons[0].projectile.GetDamageModel().damage=12;
                Ibiks.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage=12;
            }
        }
        public class OrbitalBarrage:ModUpgrade<CommandCenter>{
            public override string DisplayName=>"Orbital Barrage";
            public override string Description=>"Regularly sends down nuclear missiles";
            public override int Cost=>13570;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterBarrageIcon");
                CommandCenter.RemoveBehavior<AbilityModel>();
                var OrbitalBarrage=Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
                OrbitalBarrage.name="OrbitalBarrage";
                OrbitalBarrage.weapons[0].ejectZ=300;
                OrbitalBarrage.weapons[0].projectile=Game.instance.model.GetTowerFromId("MortarMonkey-500").GetAttackModel().weapons[0].projectile.Duplicate();
                OrbitalBarrage.range=200;
                OrbitalBarrage.RemoveBehavior<RotateToTargetModel>();
                CommandCenter.AddBehavior(OrbitalBarrage);
            }
        }
        public class DominionMight:ModUpgrade<CommandCenter>{
            public override string DisplayName=>"Might of the Dominion";
            public override string Description=>"Get a free tier 4 Marine at the end of every round";
            public override int Cost=>16700;
            public override int Path=>MIDDLE;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel CommandCenter){
                GetUpgradeModel().icon=new("CommandCenterDominionIcon");
                CommandCenter.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyVillage-004").GetBehavior<MonkeyCityModel>().Duplicate());
                CommandCenter.GetBehavior<MonkeyCityModel>().towerId="SC2Expansion-Marine-400";
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!DisplayDict.ContainsKey(objectId)&&objectId.Contains("CommandCenter")){
                    LoadModel(TowerAssets,objectId,__instance,onComplete);
                    return false;
                }
                if(DisplayDict.ContainsKey(objectId)){
                    onComplete.Invoke(DisplayDict[objectId]);
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public class ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.StartsWith("CommandCenter")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public class WeaponSpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.attackModel.name=="Ibiks"){
                    __instance.attack.entity.GetDisplayNode().graphic.GetComponent<Animator>().Play("CommandCenterAttack");
                }
            }
        }
    }
}