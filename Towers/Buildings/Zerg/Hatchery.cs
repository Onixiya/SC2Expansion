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
            Hatchery.range=40;
            Hatchery.RemoveBehavior<RectangleFootprintModel>();
            Hatchery.footprint=Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).footprint;
            Hatchery.radius=37.5f;
            var Income=Hatchery.GetAttackModel();
            Income.weapons[0].emission=new SingleEmissionModel("SingleEmissionModel",null);
            Income.weapons[0].behaviors=null;
            Income.weapons[0].rate=3.65f;
            Income.name="Income";
            Hatchery.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="HatcheryNoCreepPrefab";
            var CreepBuff=Game.instance.model.towers.First(a=>a.name.Contains("Alchemist-300")).behaviors.First(a=>a.name.Equals("AttackModel_BeserkerBrewAttack_")).Cast<AttackModel>().Duplicate();
            CreepBuff.range=65;
            CreepBuff.weapons[0].projectile.GetBehavior<AddBerserkerBrewToProjectileModel>().damageUp=0;
            CreepBuff.weapons[0].projectile.GetBehavior<AddBerserkerBrewToProjectileModel>().pierceUp=0;
            CreepBuff.weapons[0].projectile.GetBehavior<AddBerserkerBrewToProjectileModel>().rangeUp=0;
            CreepBuff.weapons[0].projectile.GetBehavior<AddBerserkerBrewToProjectileModel>().rateUp=0.0001f;
            CreepBuff.weapons[0].projectile.GetBehavior<AddBerserkerBrewToProjectileModel>().rebuffBlockTime=0;
            CreepBuff.weapons[0].projectile.display=null;
            CreepBuff.weapons[0].rate=0;
            Hatchery.AddBehavior(CreepBuff);
        }
        public override int GetTowerIndex(List<TowerDetailsModel>towerSet){
            return towerSet.First(model=>model.towerId==TowerType.BoomerangMonkey).towerIndex+1;
        }
        public class Drones:ModUpgrade<Hatchery>{
            public override string Name=>"Drones";
            public override string DisplayName=>"Additional Drones";
            public override string Description=>"Morphing more drones provides income faster";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Hatchery){
                GetUpgradeModel().icon=new("HatcheryDroneIcon");
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
                GetUpgradeModel().icon=new("HatcheryExtractorsIcon");
                var Income=Hatchery.GetAttackModel();
                Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().maximum+=10;
                Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().minimum+=10;
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
                var SpawnZergling=Game.instance.model.towers.First(a=>a.name.Contains("WizardMonkey-005")).behaviors.First(a=>a.name.Equals("AttackModel_Attack Necromancer_")).
                    Cast<AttackModel>().Duplicate();
                var NecZone=Game.instance.model.towers.First(a=>a.name.Contains("WizardMonkey-005")).GetBehavior<NecromancerZoneModel>().Duplicate();
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
                var Income=Hatchery.behaviors.First(a=>a.name.Equals("Income")).Cast<AttackModel>();
                Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().maximum+=15;
                Income.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CashModel")).Cast<CashModel>().minimum+=15;
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
                GetUpgradeModel().icon=new("HatcheryNydusNetworkIcon");
                var Nydus=Game.instance.model.towers.First(a=>a.name.Contains("MonkeyBuccaneer-040")).GetBehavior<AbilityModel>().Duplicate();
                var NydusAttack=Nydus.GetBehavior<ActivateAttackModel>();
                NydusAttack.attacks[0]=Game.instance.model.towers.First(a=>a.name.Contains("EngineerMonkey-100")).behaviors.First(a=>a.name.Contains("Spawner")).Cast<AttackModel>().Duplicate();
                var NydusWorm=NydusAttack.attacks[0].weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                Nydus.icon=new("HatcheryNydusWormIcon");
                Nydus.cooldown=0.1f;
                NydusWorm.display="HatcheryNydusWormPrefab";
                NydusWorm.GetAttackModel().GetBehavior<DisplayModel>().display=null;
                NydusWorm.RemoveBehavior<AttackModel>();
                MelonLogger.Msg(GetTowerModel<Hydralisk>(4,0,0).display);
                NydusWorm.behaviors=NydusWorm.behaviors.Add(Hatchery.GetBehavior<NecromancerZoneModel>().Duplicate(),Hatchery.behaviors.First(a=>a.name.Contains("Zergling")).Duplicate());
                NydusAttack.attacks[0].range=150;
                NydusAttack.attacks[0].targetProvider.Cast<RandomPositionModel>().minDistance=100;
                NydusAttack.attacks[0].targetProvider.Cast<RandomPositionModel>().maxDistance=150;
                NydusAttack.attacks[0].GetBehavior<RandomPositionModel>().minDistance=100;
                NydusAttack.attacks[0].GetBehavior<RandomPositionModel>().maxDistance=150;
                Hatchery.AddBehavior(Nydus);
                Hatchery.behaviors=Hatchery.behaviors.Remove(a=>a.name.Contains("Zergling"));
            }
        }
        public class Hive:ModUpgrade<Hatchery>{
            public override string Name=>"Hive";
            public override string DisplayName=>"Morph into Hive";
            public override string Description=>"Morphes into Hive, now spawns T3 Ultralisks and upgrades Zerglings spawned to T4, Nydus Worm's spawns Ultralisks and upgrades Hydralisks to Hunter Killers";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Hatchery){
                GetUpgradeModel().icon=new("HatcheryHiveIcon");
                Hatchery.display="HatcheryHivePrefab";
                Hatchery.portrait=new("HatcheryHivePortrait");
            }
        }
        //look in the ultralisk source for the prefab loading, it constantly crashes if its done in here
        private static AssetBundle __asset;
        public static AssetBundle Assets{
            get=>__asset;
            set=>__asset=value;
        }
        [HarmonyPatch(typeof(ResourceLoader),nameof(ResourceLoader.LoadSpriteFromSpriteReferenceAsync))]
        public record ResourceLoader_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.Equals("HatcheryIcon")){
                    var b=Assets.LoadAsset("HatcheryIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HatcheryPortrait")){
                    var b=Assets.LoadAsset("HatcheryPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HatcheryLairIcon")){
                    var b=Assets.LoadAsset("HatcheryLairIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HatcheryHiveIcon")){
                    var b=Assets.LoadAsset("HatcheryHiveIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HatcheryLairPortrait")){
                    var b=Assets.LoadAsset("HatcheryLairPortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HatcheryHivePortrait")){
                    var b=Assets.LoadAsset("HatcheryHivePortrait");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HatcheryNydusNetworkIcon")){
                    var b=Assets.LoadAsset("HatcheryNydusNetworkIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("HatcheryNydusWormIcon")){
                    var b=Assets.LoadAsset("HatcheryNydusWormIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
    }
}