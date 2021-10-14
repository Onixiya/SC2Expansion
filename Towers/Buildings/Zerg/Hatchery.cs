namespace SC2Expansion.Towers{
    public class Hatchery:ModTower{
        public override string DisplayName=>"Hatchery";
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"BananaFarm-003";
        public override int Cost=>750;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Primary Zerg hub, generates creep and provides income";
        public override void ModifyBaseTowerModel(TowerModel Hatchery){
            Hatchery.display="HatcheryPrefab";
            Hatchery.portrait=new("HatcheryPortrait");
            Hatchery.icon=new("HatcheryIcon");
            Hatchery.emoteSpriteLarge=new("Zerg");
            Hatchery.range=65;
            Hatchery.RemoveBehavior<RectangleFootprintModel>();
            Hatchery.footprint=Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).footprint.Duplicate();
            Hatchery.radius=37.5f;
            var Income=Hatchery.GetAttackModel();
            Income.weapons[0].emission=new SingleEmissionModel("SingleEmissionModel",null);
            Income.weapons[0].behaviors=null;
            Income.weapons[0].rate=3.65f;
            Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().maximum+=20;
            Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().minimum+=20;
            Income.name="Income";
            Hatchery.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="HatcheryNoCreepPrefab";
            var CreepBuff=Game.instance.model.towers.First(a=>a.name.Contains("SniperMonkey-050")).GetBehavior<RateSupportModel>().Duplicate();
            CreepBuff.multiplier=0.0001f;
            CreepBuff.isGlobal=false;
            CreepBuff.buffIconName=null;
            string[] ZergBuildings=new string[4]{"SC2Expansion-SpawningPool","SC2Expansion-UltraliskCavern","SC2Expansion-BanelingNest","SC2Expansion-Queen"};
            CreepBuff.filters[0].Cast<FilterInBaseTowerIdModel>().baseIds=ZergBuildings;
            Hatchery.AddBehavior(CreepBuff);
        }
        public class Drones:ModUpgrade<Hatchery>{
            public override string Name=>"Drones";
            public override string DisplayName=>"Additional Drones";
            public override string Description=>"Morphing more drones provides income faster";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Hatchery){
                GetUpgradeModel().icon=new("HatcheryDronesIcon");
                Hatchery.GetAttackModel().weapons[0].rate=2.75f;
            }
        }
        public class Extractors:ModUpgrade<Hatchery>{
            public override string Name=>"Extractors";
            public override string DisplayName=>"Extractors";
            public override string Description=>"Morphing Extractors over local Vespene geysers increases the income made";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Hatchery){
                GetUpgradeModel().icon=new("HatcheryExtractorIcon");
                var Income=Hatchery.GetAttackModel();
                Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().maximum+=30;
                Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().minimum+=30;
            }
        }
        public class Lair:ModUpgrade<Hatchery>{
            public override string Name=>"Lair";
            public override string DisplayName=>"Morph into Lair";
            public override string Description=>"Morphes into a Lair, automatically spawns T2 Zerglings and increases income made";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Hatchery){
                GetUpgradeModel().icon=new("HatcheryLairIcon");
                Hatchery.display="HatcheryLairPrefab";
                Hatchery.portrait=new("HatcheryLairPortrait");
                Hatchery.range=75;
                var SpawnZergling=Game.instance.model.towers.First(a=>a.name.Contains("WizardMonkey-005")).behaviors.First(a=>a.name.Equals("AttackModel_Attack Necromancer_")).
                    Cast<AttackModel>().Duplicate();
                var NecZone=Game.instance.model.towers.First(a=>a.name.Contains("WizardMonkey-005")).GetBehavior<NecromancerZoneModel>().Duplicate();
                var Income=Hatchery.behaviors.First(a=>a.name.Equals("Income")).Cast<AttackModel>();
                SpawnZergling.weapons[1].projectile.display="SpawningPoolZerglingWingPrefab";
                SpawnZergling.weapons[0].emission.Cast<NecromancerEmissionModel>().maxRbeSpawnedPerSecond=0;
                SpawnZergling.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().minPiercePerBloon=13;
                SpawnZergling.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().alternateProjectile=SpawnZergling.weapons[1].projectile;
                SpawnZergling.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=99999;
                SpawnZergling.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection=false;
                SpawnZergling.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=1;
                SpawnZergling.weapons[1].projectile.GetDamageModel().damage=1;
                SpawnZergling.weapons[1].projectile.radius=4;
                SpawnZergling.name="SpawnZergling";
                SpawnZergling.weapons[1].projectile.pierce=13;
                SpawnZergling.weapons[1].rate=1.5f;
                NecZone.attackUsedForRangeModel.range=999;
                Hatchery.behaviors=Hatchery.behaviors.Add(SpawnZergling,NecZone);
                Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().maximum+=25;
                Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().minimum+=25;
            }
        }
        public class Nydus:ModUpgrade<Hatchery>{
            public override string Name=>"Nydus";
            public override string DisplayName=>"Nydus Network";
            public override string Description=>"Connects Lair to the local Nydus Network. Allows spawning of short lived Nydus Worms anywhere sending out lots of Zerglings and Hydralisks";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Hatchery){
                //gettowermodel isn't working for the hydralisk at all, its just returning the base tower without any changes being done to it
                GetUpgradeModel().icon=new("HatcheryNydusNetworkIcon");
                var Nydus=Game.instance.model.towers.First(a=>a.name.Contains("MonkeyBuccaneer-040")).GetBehavior<AbilityModel>().Duplicate();
                var NydusAttack=Nydus.GetBehavior<ActivateAttackModel>();
                NydusAttack.attacks[0]=Game.instance.model.towers.First(a=>a.name.Contains("EngineerMonkey-100")).behaviors.First(a=>a.name.Contains("Spawner")).Cast<AttackModel>().Duplicate();
                var NydusWorm=NydusAttack.attacks[0].weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                var SpawnHydralisk=Game.instance.model.towers.First(a=>a.name.Contains("EngineerMonkey-100")).behaviors.First(a=>a.name.Contains("Spawner")).Cast<AttackModel>().Duplicate();
                var Hydralisk=SpawnHydralisk.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                var Spines=Hydralisk.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
                Nydus.icon=new("HatcheryNydusWormIcon");
                Nydus.cooldown=85;
                Nydus.name="SpawnNydus";
                Nydus.RemoveBehavior<RotateToTargetModel>();
                NydusAttack.attacks[0].range=150;
                NydusAttack.attacks[0].targetProvider.Cast<RandomPositionModel>().minDistance=100;
                NydusAttack.attacks[0].targetProvider.Cast<RandomPositionModel>().maxDistance=150;
                NydusAttack.attacks[0].GetBehavior<RandomPositionModel>().minDistance=100;
                NydusAttack.attacks[0].GetBehavior<RandomPositionModel>().maxDistance=150;
                NydusAttack.attacks[0].RemoveBehavior<RotateToTargetModel>();
                NydusAttack.attacks[0].weapons[0].projectile.display=null;
                NydusWorm.name="NydusWorm";
                NydusWorm.display="HatcheryNydusWormPrefab";
                NydusWorm.GetAttackModel().GetBehavior<DisplayModel>().display=null;
                NydusWorm.RemoveBehavior<AttackModel>();
                NydusWorm.behaviors=NydusWorm.behaviors.Add(Hatchery.GetBehavior<NecromancerZoneModel>().Duplicate(),Hatchery.behaviors.First(a=>a.name.Contains("Zergling")).Duplicate(),SpawnHydralisk);
                NydusWorm.behaviors.First(a=>a.name.Contains("Zergling")).Cast<AttackModel>().weapons[1].rate=0.65f;
                SpawnHydralisk.name="SpawnHydralisk";
                SpawnHydralisk.weapons[0].rate=5;
                SpawnHydralisk.range=40;
                SpawnHydralisk.targetProvider.Cast<RandomPositionModel>().minDistance=40;
                SpawnHydralisk.targetProvider.Cast<RandomPositionModel>().maxDistance=60;
                SpawnHydralisk.GetBehavior<RandomPositionModel>().minDistance=40;
                SpawnHydralisk.GetBehavior<RandomPositionModel>().maxDistance=60;
                SpawnHydralisk.RemoveBehavior<RotateToTargetModel>();
                SpawnHydralisk.weapons[0].projectile.display=null;
                Hydralisk.behaviors=Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).behaviors.Duplicate();
                Hydralisk.AddBehavior(new TowerExpireModel("TowerExpireModel",30,false,false));
                Hydralisk.name="HydraliskSpawned";
                Hydralisk.baseId="Hydralisk";
                Hydralisk.display="HydraliskPrefab";
                Hydralisk.portrait=new("HydraliskPortrait");
                Hydralisk.towerSet="Primary";
                Hydralisk.radius=7;
                Hydralisk.range=40;
                Spines.weapons[0].rate=0.55f;
                Spines.range=Hydralisk.range;
                Spines.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Damage")).Cast<DamageModel>().damage=6;
                Hatchery.RemoveBehavior<RotateToTargetModel>();
                Hatchery.AddBehavior(Nydus);
            }
        }
        public class Hive:ModUpgrade<Hatchery>{
            public override string Name=>"Hive";
            public override string DisplayName=>"Morph into Hive";
            public override string Description=>"Morphes into Hive, now spawns Noxious Ultralisks and upgrades Zerglings. Upgrades all units from Nydus Worms and decreases the cooldown. Increases income made massively";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Hatchery){
                GetUpgradeModel().icon=new("HatcheryHiveIcon");
                var Nydus=Hatchery.GetBehavior<AbilityModel>();
                var NydusWorm=Nydus.GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                var Hydralisk=NydusWorm.behaviors.First(a=>a.name.Contains("Hydralisk")).Cast<AttackModel>().weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                NydusWorm.behaviors=NydusWorm.behaviors.Remove(a=>a.name.Contains("Zergling"));
                NydusWorm.AddBehavior(Game.instance.model.towers.First(a=>a.name.Contains("WizardMonkey-004")).behaviors.First(a=>a.name.Equals("AttackModel_Attack Necromancer_")).Duplicate());
                var SpawnZerglingNydus=NydusWorm.behaviors.First(a=>a.name.Equals("AttackModel_Attack Necromancer_")).Cast<AttackModel>();
                var SpawnZerglingUltralisk=Hatchery.behaviors.First(a=>a.name.Contains("Zergling")).Cast<AttackModel>();
                var GasCloud=Game.instance.model.towers.First(a=>a.name.Contains("EngineerMonkey-030")).behaviors.First(a=>a.name.Contains("CleansingFoam")).Cast<AttackModel>().
                    weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                var Income=Hatchery.behaviors.First(a=>a.name.Equals("Income")).Cast<AttackModel>();
                Nydus.icon=new("HatcheryOmegaWormIcon");
                Nydus.cooldown=55;
                NydusWorm.display="HatcheryOmegaWormPrefab";
                Hydralisk.display="HydraliskHunterKillerPrefab";
                Hydralisk.portrait=new("HydraliskHunterKillerPortrait");
                Hydralisk.range=45;
                Hydralisk.GetAttackModel().range=Hydralisk.range;
                Hydralisk.GetAttackModel().weapons[0].rate=0.325f;
                Hydralisk.GetAttackModel().weapons[0].projectile.GetDamageModel().damage=12;
                Hydralisk.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties=0;
                SpawnZerglingNydus.name="SpawnZergling";
                SpawnZerglingNydus.weapons[0].projectile.pierce=17;
                SpawnZerglingNydus.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=1.2f;
                SpawnZerglingNydus.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection=false;
                SpawnZerglingNydus.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=9999;
                SpawnZerglingNydus.weapons[0].projectile.GetDamageModel().damage=6;
                SpawnZerglingNydus.weapons[0].projectile.display="SpawningPoolSwarmlingPrefab";
                SpawnZerglingUltralisk.weapons[0].projectile.display="SpawningPoolZerglingWingPrefab";
                SpawnZerglingUltralisk.weapons[0].emission.Cast<NecromancerEmissionModel>().maxPiercePerBloon=30;
                SpawnZerglingUltralisk.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=99999;
                SpawnZerglingUltralisk.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection=false;
                SpawnZerglingUltralisk.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=1.2f;
                SpawnZerglingUltralisk.weapons[0].projectile.GetDamageModel().damage=3;
                SpawnZerglingUltralisk.weapons[0].projectile.radius=4;
                SpawnZerglingUltralisk.weapons[0].projectile.pierce=13;
                SpawnZerglingUltralisk.weapons[0].rate=1.3f;
                SpawnZerglingUltralisk.weapons[0].emission.Cast<NecromancerEmissionModel>().maxRbeSpawnedPerSecond=200;
                SpawnZerglingUltralisk.weapons[1].projectile.display="UltraliskCavernNoxiousPrefab";
                SpawnZerglingUltralisk.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().minPiercePerBloon=25;
                SpawnZerglingUltralisk.weapons[1].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().lifespanFrames=99999;
                SpawnZerglingUltralisk.weapons[1].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().speedFrames=0.6f;
                SpawnZerglingUltralisk.weapons[1].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().disableRotateWithPathDirection=false;
                SpawnZerglingUltralisk.weapons[1].projectile.GetDamageModel().damage=6;
                SpawnZerglingUltralisk.weapons[1].projectile.radius=7;
                SpawnZerglingUltralisk.name="SpawnZerglingUltralisk";
                SpawnZerglingUltralisk.weapons[1].projectile.pierce=25;
                SpawnZerglingUltralisk.weapons[1].rate=5;
                SpawnZerglingUltralisk.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().alternateProjectile=SpawnZerglingUltralisk.weapons[1].projectile;
                GasCloud.projectile.RemoveBehavior<RemoveBloonModifiersModel>();
                GasCloud.projectile.display="GasPrefab";
                GasCloud.projectile.AddBehavior(new DamageModel("DamageModel",1,1,false,false,true,0));
                GasCloud.projectile.pierce=9999;
                GasCloud.projectile.GetBehavior<AgeModel>().lifespan=6;
                SpawnZerglingUltralisk.weapons[1].projectile.AddBehavior(GasCloud);
                Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().maximum=1000;
                Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().minimum=1000;
                Hatchery.display="HatcheryHivePrefab";
                Hatchery.portrait=new("HatcheryHivePortrait");
                Hatchery.range=83;
                //for some reason, its not cloning the dart monkeys behaviors when changing the hydralisk here, ty amman#2583 for bringing this up
                Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).GetAttackModel().weapons[0].rate=0.95f;
                Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).GetAttackModel().weapons[0].projectile.GetDamageModel().damage=1;
                Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties=(BloonProperties)17;
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Contains("Hatchery")){
                    var udn=GetHatchery(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-Hatchery";
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
        public static UnityDisplayNode GetHatchery(Transform transform,string model){
            var udn=Object.Instantiate(Assets.LoadAsset(model).Cast<GameObject>(),transform).AddComponent<UnityDisplayNode>();
            udn.Active=false;
            udn.transform.position=new(-3000,0);
            return udn;
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public record ResourceLoader_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.Contains("Hatchery")){
                    var text=Assets.LoadAsset(reference.guidRef).Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
    }
}