namespace SC2Expansion.Towers{
    public class Queen:ModTower{
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Ranged Zerg support, requires creep";
        public override void ModifyBaseTowerModel(TowerModel Queen){
            Queen.display="QueenPrefab";
            Queen.portrait=new("QueenPortrait");
            Queen.icon=new("QueenIcon");
            Queen.towerSet="Primary";
            Queen.emoteSpriteLarge=new("Zerg");
            Queen.radius=7;
            Queen.range=30;
            var Spines=Queen.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            Spines.weapons[0].rate=0.7f;
            Spines.range=Queen.range;
            Spines.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Damage")).Cast<DamageModel>().damage=2;
            Queen.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="QueenPrefab";
        }
        public class QueenGroovedSpines:ModUpgrade<Queen>{
            public override string Name=>"QueenGroovedSpines";
            public override string DisplayName=>"Grooved Spines";
            public override string Description=>"Evolving small grooves into the spines increases their range";
            public override int Cost=>750;
            public override int Path=>0;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Queen){
                GetUpgradeModel().icon=new("HydraliskGroovedSpinesIcon");
            }
        }
        public class CreepTumor:ModUpgrade<Queen>{
            public override string Name=>"CreepTumor";
            public override string DisplayName=>"Creep Tumor";
            public override string Description=>"Spawns a Creep Tumor, generating creep around it";
            public override int Cost=>750;
            public override int Path=>0;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Queen){
                GetUpgradeModel().icon=new("QueenCreepTumorIcon");
            }
        }
        public class WildMutation:ModUpgrade<Queen>{
            public override string Name=>"WildMutation";
            public override string DisplayName=>"WildMutation";
            public override string Description=>"Temporalily buff's the attack speed and damage of all nearby Zerg";
            public override int Cost=>750;
            public override int Path=>0;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Queen){
                GetUpgradeModel().icon=new("QueenWildMutationIcon");
                Queen.display="QueenLeviathanPrefab";
            }
        }
        public class BroodMother:ModUpgrade<Queen>{
            public override string Name=>"BroodMother";
            public override string DisplayName=>"Evolve into a Brood Mother";
            public override string Description=>"Brood Mother's control entire Broods of Zerg. Can call down a Drop Pod containing Hydralisks and Zerglings";
            public override int Cost=>750;
            public override int Path=>0;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Queen){
                GetUpgradeModel().icon=new("QueenBroodMotherIcon");
                   Queen.display="QueenBroodMotherPrefab";
            }
        }
        public class Zagara:ModUpgrade<Queen>{
            public override string Name=>"Zagara";
            public override string DisplayName=>"Zagara";
            public override string Description=>"The first Brood Mother, now controls the entire Swarm. Can buff everything's attack speed and can call down multiple Drop Pods with Hydralisks and Roaches";
            public override int Cost=>750;
            public override int Path=>0;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Queen){
                GetUpgradeModel().icon=new("QueenZagaraIcon");
                Queen.display="QueenZagaraPrefab";
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Contains("Queen")){
                    var udn=GetQueen(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-Queen";
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
        public static UnityDisplayNode GetQueen(Transform transform,string model){
            var udn=Object.Instantiate(Assets.LoadAsset(model).Cast<GameObject>(),transform).AddComponent<UnityDisplayNode>();
            udn.Active=false;
            udn.transform.position=new(-3000,0);
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
                if(reference!=null&&reference.guidRef.Contains("Queen")){
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
                if(__instance.attack.tower.towerModel.name.Contains("Queen")){
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("QueenAttack");
                }
            }
        }
    }
}