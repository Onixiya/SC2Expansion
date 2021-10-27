namespace SC2Expansion.Towers{
    public class Battlecruiser:ModTower{
        public static AssetBundle Assets=AssetBundle.LoadFromMemory(Models.Models.battlecruiser);
        public override string DisplayName=>"Battlecruiser";
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"SuperMonkey-100";
        public override int Cost=>2750;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Terran capital ship, shoots lasers very fast";
        public override void ModifyBaseTowerModel(TowerModel Battlecruiser){
            Battlecruiser.display="BattlecruiserPrefab";
            Battlecruiser.portrait=new("BattlecruiserPortrait");
            Battlecruiser.icon=new("BattlecruiserIcon");
            Battlecruiser.emoteSpriteLarge=new("Terran");
            Battlecruiser.radius=16.7f;
            Battlecruiser.range=70;
            Battlecruiser.areaTypes=new(4);
            Battlecruiser.areaTypes[0]=AreaType.land;
            Battlecruiser.areaTypes[1]=AreaType.track;
            Battlecruiser.areaTypes[2]=AreaType.ice;
            Battlecruiser.areaTypes[3]=AreaType.water;
            Battlecruiser.GetBehavior<DisplayModel>().positionOffset=new(0,0,100);
            Battlecruiser.GetBehavior<DisplayModel>().display=Battlecruiser.display;
            var Fire=Battlecruiser.GetAttackModel();
            Fire.name="BattlecruiserFire";
            Fire.weapons[0].name="BattlecruiserFire";
            Fire.range=Battlecruiser.range;
            Fire.weapons[0].projectile.GetDamageModel().damage=1;
        }
        public class TacJump:ModUpgrade<Battlecruiser>{
            public override string Name=>"TacJump";
            public override string DisplayName=>"Tactical Jump";
            public override string Description=>"Allows the Battlecruiser to make use of its short range warp drive, instantly teleporting to any where on the map";
            public override int Cost=>2000;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Battlecruiser){
                GetUpgradeModel().icon=new("BattlecruiserTacJumpIcon");
                var TacJump=Game.instance.model.GetTowerFromId("SuperMonkey-003").GetBehavior<AbilityModel>();
                TacJump.name="TacJump";
                TacJump.cooldown=80;
                TacJump.GetBehavior<DarkshiftModel>().restrictToTowerRadius=false;
                TacJump.GetBehavior<DarkshiftModel>().disappearEffectModel.assetId=null;
                TacJump.GetBehavior<DarkshiftModel>().reappearEffectModel.assetId=null;
                TacJump.icon=new("BattlecruiserTacJumpIcon");
                TacJump.maxActivationsPerRound=1;
                Battlecruiser.AddBehavior(TacJump);
            }
        }
        public class Yamato:ModUpgrade<Battlecruiser>{
            public override string Name=>"Yamato";
            public override string DisplayName=>"Yamato Cannon";
            public override string Description=>"Upgrades to the colossus reactor lets the Yamato cannon be fired dealing massive damage to a single target";
            public override int Cost=>2000;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Battlecruiser){
                GetUpgradeModel().icon=new("BattlecruiserYamatoIcon");
                var Yamato=Game.instance.model.towers.First(a=>a.name.Equals("PatFusty 10")).behaviors.First(a=>a.name.Contains("Big")).Clone().Cast<AbilityModel>();
                Yamato.name="Yamato";
                Yamato.displayName="Yamato Cannon";
                Yamato.GetBehavior<ActivateAttackModel>().attacks[0]=Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel();
                Yamato.GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage=225;
                Yamato.GetBehavior<ActivateAttackModel>().attacks[0].range=9999;
                Yamato.GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<TravelStraitModel>().speed=500;//who the fuck spells it like strait?
                Yamato.GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.display="88399aeca4ae48a44aee5b08eb16cc61";
                Yamato.icon=new("BattlecruiserYamatoIcon");
                Yamato.cooldown=70f;
                Yamato.GetBehavior<PauseAllOtherAttacksModel>().lifespan=7;
                Yamato.maxActivationsPerRound=1;
                Battlecruiser.AddBehavior(Yamato);
            }
        }
        public class Sovereign:ModUpgrade<Battlecruiser>{
            public override string Name=>"Sovereign";
            public override string DisplayName=>"Sovereign Class";
            public override string Description=>"Sovereign Class Battlecruisers can fire the Yamato cannon regularly with reduced damage";
            public override int Cost=>2000;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Battlecruiser){
                GetUpgradeModel().icon=new("BattlecruiserSovereignIcon");
                Battlecruiser.display="BattlecruiserSovereignPrefab";
                var MiniYamato=Battlecruiser.behaviors.First(a=>a.name.Contains("Yamato")).Cast<AbilityModel>().GetBehavior<ActivateAttackModel>().attacks[0].Duplicate();
                MiniYamato.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage=50;
                MiniYamato.range=90;
                MiniYamato.weapons[0].rate=3.5f;
                MiniYamato.name="MiniYamato";
                Battlecruiser.GetBehavior<DisplayModel>().display="BattlecruiserSovereignPrefab";
                Battlecruiser.RemoveBehavior(Battlecruiser.behaviors.First(a=>a.name.Contains("Yamato")));
                Battlecruiser.AddBehavior(MiniYamato);
            }
        }
        public class POA:ModUpgrade<Battlecruiser>{
            public override string Name=>"POA";
            public override string DisplayName=>"Pride of Augustgrad";
            public override string Description=>"Elite Dominion Battlecruiser, can Tac jump much more frequently, increased damage and camo detection";
            public override int Cost=>2000;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Battlecruiser){
                GetUpgradeModel().icon=new("BattlecruiserPOAIcon");
                Battlecruiser.display="BattlecruiserPOAPrefab";
                Battlecruiser.GetAttackModel().weapons[0].projectile.GetDamageModel().damage+=2;
                Battlecruiser.GetBehavior<DisplayModel>().display="BattlecruiserPOAPrefab";
                Battlecruiser.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true));
                Battlecruiser.behaviors.First(a=>a.name.Equals("TacJump")).Cast<AbilityModel>().cooldown=40;
            }
        }
        public class Hyperion:ModUpgrade<Battlecruiser>{
            public override string Name=>"Hyperion";
            public override string DisplayName=>"Hyperion";
            public override string Description=>"A ship hailing from the days of the Confederacy, it has seen many crews and battles. Currently the flagship of the Dominion";
            public override int Cost=>2000;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Battlecruiser){
                var Fire=Battlecruiser.GetAttackModel();
                var TacJump=Battlecruiser.behaviors.First(a=>a.name.Equals("TacJump")).Cast<AbilityModel>();
                var Yamato=Battlecruiser.GetAttackModel();
                GetUpgradeModel().icon=new("BattlecruiserHyperionIcon");
                Battlecruiser.display="BattlecruiserHyperionPrefab";
                Battlecruiser.portrait=new("BattlecruiserHyperionPortrait");
                Battlecruiser.range+=15;
                Fire.weapons[0].rate-=0.03f;
                Fire.weapons[0].projectile.GetDamageModel().damage+=3;
                Yamato.range+=15;
                Yamato.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage=150;
                Yamato.weapons[0].rate=2.25f;
                TacJump.cooldown=20;
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            public static Dictionary<string,UnityDisplayNode>DisplayDict=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!DisplayDict.ContainsKey(objectId)&&objectId.Contains("Battlecruiser")){
                    var udn=uObject.Instantiate(Assets.LoadAsset(objectId).Cast<GameObject>(),__instance.PrototypeRoot).AddComponent<UnityDisplayNode>();
                    udn.transform.position=new(-3000,0);
                    udn.name="SC2Expansion-Battlecruiser";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    DisplayDict.Add(objectId,udn);
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
        public record ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference.guidRef.Contains("Battlecruiser")){
                    var text=Assets.LoadAsset(reference.guidRef).Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public static class WeaponSpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.towerModel.name.Contains("Battlecruiser")&&__instance.attack.attackModel.name.Contains("Yamato")){
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("BattlecruiserYamato");
                }
            }
        }
    }
}