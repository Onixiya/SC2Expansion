namespace SC2Expansion.Towers{
    public class Roach:ModTower{
        public override string DisplayName=>"Roach";
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"GlueGunner";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Medium range Zerg armoured destroyer, spews out acid that does double damage to Lead and Ceremic bloons";
        public override void ModifyBaseTowerModel(TowerModel Roach){
            Roach.display="RoachPrefab";
            Roach.portrait=new("RoachPortrait");
            Roach.icon=new("RoachIcon");
            Roach.emoteSpriteLarge=new("Zerg");
            Roach.radius=8;
            Roach.cost=700;
            Roach.range=40;
            var Acid=Roach.behaviors.First(a=>a.name.Contains("AttackModel")).Cast<AttackModel>();
            Acid.range=Roach.range;
            Acid.weapons[0].rate=1;
            Acid.weapons[0].projectile.AddBehavior(new DamageModel("DamageModel",2,0,true,false,true,0));
            Acid.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel","Lead",2,0,true,false));
            Acid.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel","Ceremic",2,0,true,false));
            Acid.weapons[0].projectile.RemoveBehavior<SlowModel>();
            Roach.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display=Roach.display;
        }
        public class HydriodicBile:ModUpgrade<Roach>{
            public override string Name=>"HydriodicBile";
            public override string DisplayName=>"Hydriodic Bile";
            public override string Description=>"Increases damage done to non Moabs";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Roach){
                GetUpgradeModel().icon=new("RoachHydriodicBileIcon");
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
                Roach.display="RoachVilePrefab";
            }
        }
        public class Ravager:ModUpgrade<Roach>{
            public override string Name=>"Ravager";
            public override string DisplayName=>"Morph into Ravager";
            public override string Description=>"Ravagers are Zerg ground seige units with a slow but high range attack";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Roach){
                GetUpgradeModel().icon=new("RoachRavagerIcon");
                Roach.display="RoachRavagerPrefab";
            }
        }
        public class CorrosiveBile:ModUpgrade<Roach>{
            public override string Name=>"CorrosiveBile";
            public override string DisplayName=>"Corrosive Bile";
            public override string Description=>"";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Roach){
                GetUpgradeModel().icon=new("RoachCorrosiveBileIcon");
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
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Contains("Roach")){
                    var udn=GetRoach(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-Roach";
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
        public static UnityDisplayNode GetRoach(Transform transform,string model){
            var udn=Object.Instantiate(Assets.LoadAsset(model).Cast<GameObject>(),transform).AddComponent<UnityDisplayNode>();
            udn.Active=false;
            udn.transform.position=new(-3000,0);
            return udn;
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public record ResourceLoader_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.Contains("Roach")){
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
                if(__instance.attack.tower.towerModel.name.Contains("Roach")){
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("RoachAttack");
                }
            }
        }
    }
}