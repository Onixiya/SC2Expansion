namespace SC2Expansion.Towers{
    public class SpawningPool:ModTower{
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"WizardMonkey-005";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Spawns Zerglings, small melee Zerg, very short life expentancy";
        public override void ModifyBaseTowerModel(TowerModel SpawningPool){
            SpawningPool.display="SpawningPoolPrefab";
            SpawningPool.portrait=new("SpawningPoolPortrait");
            SpawningPool.icon=new("SpawningPoolIcon");
            SpawningPool.emoteSpriteLarge=new("Zerg");
            SpawningPool.radius=20;
            SpawningPool.range=15;
            SpawningPool.behaviors=SpawningPool.behaviors.Remove(a=>a.name.Contains("Shimmer"));
            SpawningPool.behaviors=SpawningPool.behaviors.Remove(a=>a.name.Equals("AttackModel_Attack_"));
            SpawningPool.behaviors=SpawningPool.behaviors.Remove(a=>a.name.Contains("Buff"));
            var SpawnZergling=SpawningPool.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            SpawnZergling.weapons[1].projectile.display="SpawningPoolZerglingPrefab";
            SpawnZergling.weapons[0].emission.Cast<NecromancerEmissionModel>().maxRbeSpawnedPerSecond=0;
            SpawnZergling.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().minPiercePerBloon=7;
            SpawnZergling.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().alternateProjectile=SpawnZergling.weapons[1].projectile;
            SpawnZergling.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=99999;
            SpawnZergling.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection=false;
            SpawnZergling.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=0.35f;
            SpawnZergling.weapons[1].projectile.GetDamageModel().damage=1;
            SpawnZergling.weapons[1].projectile.radius=4;
            SpawnZergling.name="SpawnZergling";
            SpawnZergling.weapons[1].projectile.pierce=7;
            SpawnZergling.weapons[1].rate=15000;
            SpawningPool.behaviors.First(a=>a.name.Contains("Zone")).Cast<NecromancerZoneModel>().attackUsedForRangeModel.range=999;
            SpawningPool.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="SpawningPoolPrefab";
        }
        public class HardendCarapace:ModUpgrade<SpawningPool> {
            public override string Name=>"HardendCarapace";
            public override string DisplayName=>"Hardend Carapace";
            public override string Description=>"Increasing the carapace density lets Zerglings take more hits before dying";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel SpawningPool){
                GetUpgradeModel().icon=new("SpawningPoolHardendCarapaceIcon");
                var SpawnZergling=SpawningPool.behaviors.First(a=>a.name.Equals("SpawnZergling")).Cast<AttackModel>();
                SpawnZergling.weapons[1].emission.Cast<PrinceOfDarknessEmissionModel>().minPiercePerBloon=13;
                SpawnZergling.weapons[1].projectile.pierce=13;
            }
        }
        public class MetabolicBoost:ModUpgrade<SpawningPool> {
            public override string Name=>"MetabolicBoost";
            public override string DisplayName=>"Metabolic Boost";
            public override string Description=>"Improving Zergling metabolism and evolving wings massively increases Zergling's speed";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel SpawningPool) {
                GetUpgradeModel().icon=new("SpawningPoolMetabolicBoostIcon");
                var SpawnZergling=SpawningPool.behaviors.First(a=>a.name.Equals("SpawnZergling")).Cast<AttackModel>();
                SpawnZergling.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=1;
                SpawnZergling.weapons[1].projectile.display="SpawningPoolZerglingWingPrefab";
            }
        }
        public class AdrenalGlands:ModUpgrade<SpawningPool> {
            public override string Name=>"AdrenalGlands";
            public override string DisplayName=>"Adrenal Glands";
            public override string Description=>"Making even more adrenaline increases Zergling damage and speed slightly";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel SpawningPool) {
                GetUpgradeModel().icon=new("SpawningPoolAdrenalGlandsIcon");
                var SpawnZergling=SpawningPool.behaviors.First(a=>a.name.Equals("SpawnZergling")).Cast<AttackModel>();
                SpawnZergling.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=1.2f;
                SpawnZergling.weapons[1].projectile.GetDamageModel().damage=3;
            }
        }
        public class Primal:ModUpgrade<SpawningPool> {
            public override string Name=>"ZerglingPrimal";
            public override string DisplayName=>"Primal Evolution";
            public override string Description=>"Gains a random upgrade to speed, damage or health";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel SpawningPool){
                GetUpgradeModel().icon=new("SpawningPoolPrimalIcon");
                SpawningPool.behaviors.First(a=>a.name.Equals("SpawnZergling")).Cast<AttackModel>().weapons[1].projectile.display="SpawningPoolPrimalPrefab";
            }
        }
        public class Swarmling:ModUpgrade<SpawningPool>{
            public override string Name=>"Swarmling";
            public override string DisplayName=>"Swarmling strain";
            public override string Description=>"Swarmling strain spawns multiple Zerglings at a time";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel SpawningPool){
                GetUpgradeModel().icon=new("SpawningPoolSwarmlingIcon");
                var SpawnZergling=SpawningPool.behaviors.First(a=>a.name.Equals("SpawnZergling")).Cast<AttackModel>();
                float Pierce=SpawnZergling.weapons[1].projectile.pierce;
                float Speed=SpawnZergling.weapons[1].projectile.GetBehavior<TravelAlongPathModel>().speedFrames;
                float Damage=SpawnZergling.weapons[1].projectile.GetDamageModel().damage+2;
                SpawningPool.RemoveBehavior<AttackModel>();
                SpawnZergling=null;
                SpawningPool.AddBehavior(Game.instance.model.towers.First(a=>a.name.Contains("WizardMonkey-004")).behaviors.First(a=>a.name.Equals("AttackModel_Attack Necromancer_")).Duplicate());
                SpawnZergling=SpawningPool.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
                SpawnZergling.name="SpawnZergling";
                SpawnZergling.weapons[0].projectile.pierce=Pierce;
                SpawnZergling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames=Speed;
                SpawnZergling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection=false;
                SpawnZergling.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames=9999;
                SpawnZergling.weapons[0].projectile.GetDamageModel().damage=Damage;
                SpawnZergling.weapons[0].rate=6500f;
                SpawnZergling.weapons[0].projectile.display="SpawningPoolSwarmlingPrefab";
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Contains("SpawningPool")){
                    var udn=GetSpawningPool(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-SpawningPool";
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
        public static UnityDisplayNode GetSpawningPool(Transform transform,string model){
            var udn=Object.Instantiate(Assets.LoadAsset(model).Cast<GameObject>(),transform).AddComponent<UnityDisplayNode>();
            udn.Active=false;
            udn.transform.position=new(-3000,0);
            return udn;
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public record ResourceLoader_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.Contains("SpawningPool")){
                    var text=Assets.LoadAsset(reference.guidRef).Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
    }
}