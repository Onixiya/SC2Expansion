namespace SC2Expansion.Towers{
    public class Roach:ModTower<ZergSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.roach);
        public override string DisplayName=>"Roach";
        public override string BaseTower=>"GlueGunner";
        public override int Cost=>400;
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
        }
        public class HydriodicBile:ModUpgrade<Roach>{
            public override string Name=>"HydriodicBile";
            public override string DisplayName=>"Hydriodic Bile";
            public override string Description=>"Increases damage";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Roach){
                GetUpgradeModel().icon=new("RoachHydriodicBileIcon");
                Roach.GetAttackModel().weapons[0].projectile.GetDamageModel().damage+=2;
            }
        }
        public class Vile:ModUpgrade<Roach>{
            public override string Name=>"Vile";
            public override string DisplayName=>"Vile strain";
            public override string Description=>"Evolving into the vile strain allows the acid to harden upon hitting a target and slows it down";
            public override int Cost=>750;
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
            }
        }
        public class Ravager:ModUpgrade<Roach>{
            public override string Name=>"Ravager";
            public override string DisplayName=>"Morph into Ravager";
            public override string Description=>"Morphes into a Ravager, buffing range and damage";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Roach){
                GetUpgradeModel().icon=new("RoachRavagerIcon");
                var Bile=Roach.GetAttackModel();
                Roach.range+=20;
                Bile.range=Roach.range;
                Bile.weapons[0].rate=2.5f;
                Bile.weapons[0].projectile.display=Game.instance.model.GetTowerFromId("GlueGunner").GetAttackModel().weapons[0].projectile.display;
                Bile.weapons[0].projectile.GetDamageModel().damage+=6;
                Bile.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed=600;
                Bile.weapons[0].projectile.RemoveBehavior<SlowModel>();
                Roach.display="RoachRavagerPrefab";
            }
        }
        public class CorrosiveBile:ModUpgrade<Roach>{
            public override string Name=>"CorrosiveBile";
            public override string DisplayName=>"Corrosive Bile";
            public override string Description=>"Changes attack into a slow but powerful one that can target anywhere on the map";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Roach){
                GetUpgradeModel().icon=new("RoachCorrosiveBileIcon");
                Roach.range=9999;
                var Bile=Roach.GetAttackModel();
                Bile.range=Roach.range;
                Bile.weapons[0].projectile=Game.instance.model.GetTowerFromId("MortarMonkey-300").GetAttackModel().weapons[0].projectile;
                Bile.weapons[0].projectile.RemoveBehavior<CreateProjectileOnExhaustFractionModel>();
                Bile.GetBehavior<AttackFilterModel>().filters=Bile.GetBehavior<AttackFilterModel>().filters.AddTo(new FilterWithTagModel("FilterWithTagModel","Moabs",false));
            }
        }
        public class Brutalisk:ModUpgrade<Roach>{
            public override string Name=>"Brutalisk";
            public override string DisplayName=>"Morph into Brutalisk";
            public override string Description=>"\"Evolving brutalisk. Threat maximized\"";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Roach){
                GetUpgradeModel().icon=new("RoachBrutaliskIcon");
                Roach.display="RoachBrutaliskPrefab";
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            public static Dictionary<string,UnityDisplayNode>DisplayDict=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!DisplayDict.ContainsKey(objectId)&&objectId.Contains("Roach")){
                    var udn=uObject.Instantiate(TowerAssets.LoadAsset(objectId).Cast<GameObject>(),__instance.PrototypeRoot).AddComponent<UnityDisplayNode>();
                    udn.transform.position=new(-3000,0);
                    udn.name="SC2Expansion-Roach";
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
            public static void Postfix(SpriteReference reference,ref uImage image){
                if(reference!=null&&reference.guidRef.Contains("Roach")){
                    var text=TowerAssets.LoadAsset(reference.guidRef).Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public static class SpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.towerModel.name.Contains("Roach")){
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("RoachAttack");
                }
            }
        }
    }
}