namespace SC2Expansion.Towers{
    public class Roach:ModTower<ZergSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.roach);
        public override string DisplayName=>"Roach";
        public override string BaseTower=>"GlueGunner";
        public override int Cost=>650;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>!ZergEnabled;
        public override string Description=>"Medium range Zerg armoured destroyer, spews out acid that does double damage to Lead and Ceremic bloons";
        public override void ModifyBaseTowerModel(TowerModel Roach){
            Roach.display="RoachPrefab";
            Roach.portrait=new("RoachPortrait");
            Roach.icon=new("RoachIcon");
            Roach.emoteSpriteLarge=new("Zerg");
            Roach.radius=8;
            Roach.cost=700;
            Roach.range=40;
            var Acid=Roach.GetAttackModel();
            Acid.range=Roach.range;
            Acid.weapons[0].rate=1.1f;
            Acid.weapons[0].ejectX=0;
            Acid.weapons[0].ejectY=0;
            Acid.weapons[0].projectile.AddBehavior(new DamageModel("DamageModel",2,0,true,false,true,0));
            Acid.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel","Lead",2,0,true,false));
            Acid.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel","Ceremic",2,0,true,false));
            Acid.weapons[0].projectile.RemoveBehavior<SlowModel>();
            Acid.weapons[0].projectile.filters=Acid.weapons[0].projectile.filters.RemoveItem(Acid.weapons[0].projectile.filters.First(a=>a.name.Contains("Glue")));
            Acid.weapons[0].projectile.filters=Acid.weapons[0].projectile.filters.RemoveItem(Acid.weapons[0].projectile.filters.First(a=>a.name.Contains("TagModel")));
            Acid.weapons[0].projectile.display="97f2427a81f436547b0a59f37fb689da";
            Roach.GetBehavior<DisplayModel>().display=Roach.display;
            Roach.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId="RoachBirth";
            Roach.GetBehavior<CreateSoundOnTowerPlaceModel>().sound2=Roach.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1;
            SetUpgradeSounds(Roach,"RoachUpgrade");
        }
        public class HydriodicBile:ModUpgrade<Roach>{
            public override string DisplayName=>"Hydriodic Bile";
            public override string Description=>"Increases damage";
            public override int Cost=>450;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Roach){
                GetUpgradeModel().icon=new("RoachHydriodicBileIcon");
                Roach.GetAttackModel().weapons[0].projectile.GetDamageModel().damage+=2;
                SetUpgradeSounds(Roach,"RoachUpgrade1");
            }
        }
        public class Vile:ModUpgrade<Roach>{
            public override string DisplayName=>"Vile strain";
            public override string Description=>"Evolving into the vile strain allows the acid to harden upon hitting a target and slows it down";
            public override int Cost=>1275;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Roach){
                GetUpgradeModel().icon=new("RoachVileIcon");
                var Acid=Roach.GetAttackModel();
                var Slow=Game.instance.model.GetTowerFromId("GlueGunner-300").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
                Roach.display="RoachVilePrefab";
                Slow.glueLevel=1;
                Acid.weapons[0].projectile.AddBehavior(Slow);
                Roach.display="RoachVilePrefab";
                SetUpgradeSounds(Roach,"RoachUpgrade2");
            }
        }
        public class Ravager:ModUpgrade<Roach>{
            public override string DisplayName=>"Morph into Ravager";
            public override string Description=>"Morphes into a Ravager, buffing range and damage";
            public override int Cost=>3450;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Roach){
                GetUpgradeModel().icon=new("RoachRavagerIcon");
                var Bile=Roach.GetAttackModel();
                Roach.range+=20;
                Roach.radius+=7;
                Bile.range=Roach.range;
                Bile.weapons[0].projectile.display=Game.instance.model.GetTowerFromId("GlueGunner").GetAttackModel().weapons[0].projectile.display;
                Bile.weapons[0].projectile.GetDamageModel().damage+=6;
                Bile.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed=600;
                Bile.weapons[0].projectile.RemoveBehavior<SlowModel>();
                Roach.display="RoachRavagerPrefab";
                SetUpgradeSounds(Roach,"RoachUpgrade3");
            }
        }
        public class CorrosiveBile:ModUpgrade<Roach>{
            public override string DisplayName=>"Corrosive Bile";
            public override string Description=>"Changes attack into a slow but powerful one that can target anywhere on the map";
            public override int Cost=>7775;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Roach){
                GetUpgradeModel().icon=new("RoachCorrosiveBileIcon");
                Roach.range=9999;
                var Bile=Roach.GetAttackModel();
                Bile.range=Roach.range;
                Bile.weapons[0].rate=2;
                Bile.weapons[0].projectile=Game.instance.model.GetTowerFromId("MortarMonkey-300").GetAttackModel().weapons[0].projectile;
                Bile.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage=20;
                Bile.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().immuneBloonProperties=(BloonProperties)17;
                Bile.weapons[0].projectile.radius=20;
                Bile.GetBehavior<AttackFilterModel>().filters=new Il2CppReferenceArray<FilterModel>(new FilterModel[]{new FilterInvisibleModel("FilterInvisibleModel",true,false)});
                SetUpgradeSounds(Roach,"RoachUpgrade4");
            }
        }
        public class Brutalisk:ModUpgrade<Roach>{
            public override string DisplayName=>"Morph into Brutalisk";
            public override string Description=>"\"Evolving brutalisk. Threat maximized\"";
            public override int Cost=>15465;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Roach){
                GetUpgradeModel().icon=new("RoachBrutaliskIcon");
                Roach.display="RoachBrutaliskPrefab";
                Roach.RemoveBehavior<AttackModel>();
                Roach.AddBehavior(Game.instance.model.GetTowerFromId("Sauda").GetAttackModel().Duplicate());
                var Brutalize=Roach.GetAttackModel();
                Roach.range=50;
                Brutalize.range=Roach.range;
                Brutalize.weapons[0].projectile.GetDamageModel().damage=80;
            }
        }
        [HarmonyPatch(typeof(AudioFactory),"Start")]
        public class AudioFactoryStart_Patch{
            [HarmonyPostfix]
            public static void Prefix(ref AudioFactory __instance){
                if(ZergEnabled){
                    AudioFactoryInstance=__instance;
                    __instance.RegisterAudioClip("RoachBirth",TowerAssets.LoadAsset("RoachBirth").Cast<AudioClip>());
                    __instance.RegisterAudioClip("RoachUpgrade",TowerAssets.LoadAsset("RoachUpgrade").Cast<AudioClip>());
                    __instance.RegisterAudioClip("RoachUpgrade1",TowerAssets.LoadAsset("RoachUpgrade1").Cast<AudioClip>());
                    __instance.RegisterAudioClip("RoachUpgrade2",TowerAssets.LoadAsset("RoachUpgrade2").Cast<AudioClip>());
                    __instance.RegisterAudioClip("RoachUpgrade3",TowerAssets.LoadAsset("RoachUpgrade3").Cast<AudioClip>());
                    __instance.RegisterAudioClip("RoachUpgrade4",TowerAssets.LoadAsset("RoachUpgrade4").Cast<AudioClip>());
                }
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                return LoadModel(TowerAssets,objectId,"Roach",__instance,onComplete);
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public class ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.StartsWith("Roach")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public class SpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.towerModel.name.StartsWith("SC2Expansion-Roach")){
                    if(__instance.attack.tower.towerModel.tier==5){
                        int RandNum=new System.Random().Next(1,3);
                        __instance.attack.tower.Node.graphic.GetComponent<Animator>().Play("BrutaliskAttack"+RandNum);
                    }else{
                        __instance.attack.tower.Node.graphic.GetComponent<Animator>().Play("RoachAttack");
                    }
                }
            }
        }
    }
}