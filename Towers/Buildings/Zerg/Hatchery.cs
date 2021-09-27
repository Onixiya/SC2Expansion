namespace SC2Expansion.Towers{
    public class Hatchery:ModTower{
        public override string DisplayName=>"Hatchery";
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"BananaFarm-003";
        public override int Cost=>750;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Primary Zerg hub, provides income";
        public override void ModifyBaseTowerModel(TowerModel Hatchery){
            Hatchery.display="CommandCenterPrefab";
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
            Income.weapons[0].rate=4;
            Hatchery.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display=Hatchery.display;
            //Hatchery.AddBehavior(Game.instance.model.towers.First(a=>a.name.Contains("Alchemist-300")).behaviors.First(a=>a.name.Equals("AttackModel_BeserkerBrewAttack_")).Duplicate());
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
                GetUpgradeModel().icon=new("HatcheryLairIcon");
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
                GetUpgradeModel().icon=new("HatcheryLairIcon");
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
                Hatchery.AddBehavior(Game.instance.model.towers.First(a=>a.name.Contains("WizardMonkey-004")).behaviors.First(a=>a.name.Contains("Zone")));
                Hatchery.AddBehavior(Game.instance.model.towers.First(a=>a.name.Contains("WizardMonkey-004")).behaviors.First(a=>a.name.Equals("AttackModel_Attack Necromancer_")));
                Hatchery.GetBehavior<NecromancerZoneModel>().attackUsedForRangeModel.range=999;
            }
        }
        public class Nydus:ModUpgrade<Hatchery>{
            public override string Name=>"Nydus";
            public override string DisplayName=>"Nydus Network";
            public override string Description=>"Connects Lair to the Nydus Worm network. Allows spawning of short lived Nydus Worms anywhere sending out lots of Zerg";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Hatchery){
                GetUpgradeModel().icon=new("HatcheryLairIcon");
            }
        }
        public class Hive:ModUpgrade<Hatchery>{
            public override string Name=>"Hive";
            public override string DisplayName=>"Morph into Hive";
            public override string Description=>"Morphes into Hive, now spawns T3 Ultralisks and upgrades Zerglings spawned to T5, decreases Nydus worm cooldown";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Hatchery){
                GetUpgradeModel().icon=new("HatcheryHiveIcon");
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HatcheryPrefab")){
                    var udn=GetHatchery(__instance.PrototypeRoot,"HatcheryPrefab");
                    udn.name="SC2Expansion-Hatchery";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HatcheryNoCreepPrefab")){
                    var udn=GetHatchery(__instance.PrototypeRoot,"HatcheryNoCreepPrefab");
                    udn.name="SC2Expansion-Hatchery";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HatcheryLairPrefab")){
                    var udn=GetHatchery(__instance.PrototypeRoot,"HatcheryLairPrefab");
                    udn.name="SC2Expansion-Hatchery";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("HatcheryHivePrefab")){
                    var udn=GetHatchery(__instance.PrototypeRoot,"HatcheryHivePrefab");
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
            MelonLogger.Msg("==================================");
            var temp=Assets.GetAllAssetNames().GetEnumerator();
            while(temp.MoveNext()){
                MelonLogger.Msg("Name: "+temp.Current);
            }
            MelonLogger.Msg(model);
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
            }
        }
    }
}