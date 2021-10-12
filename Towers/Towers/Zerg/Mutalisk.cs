namespace SC2Expansion.Towers{
    public class Mutalisk:ModTower{
        public override string DisplayName=>"Mutalisk";
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>4;
        public override int MiddlePathUpgrades=>4;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Primary Zerg flyer, able to bounce its shot to hit multiple targets";
        public override void ModifyBaseTowerModel(TowerModel Mutalisk){
            Mutalisk.display="MutaliskPrefab";
            Mutalisk.portrait=new("MutaliskPortrait");
            Mutalisk.icon=new("MutaliskIcon");
            Mutalisk.emoteSpriteLarge=new("Zerg");
            Mutalisk.radius=5;
            Mutalisk.cost=700;
            Mutalisk.range=40;
            Mutalisk.areaTypes=new(4);
            Mutalisk.areaTypes[0]=AreaType.land;
            Mutalisk.areaTypes[1]=AreaType.track;
            Mutalisk.areaTypes[2]=AreaType.ice;
            Mutalisk.areaTypes[3]=AreaType.water;
            var Glaive=Mutalisk.behaviors.First(a=>a.name.Contains("AttackModel")).Cast<AttackModel>();
            Glaive.name="MutaliskGlaive";
            Glaive.weapons[0].projectile.behaviors=Glaive.weapons[0].projectile.behaviors.Add(Game.instance.model.towers.First(a=>a.name.Contains("SniperMonkey-030")).GetAttackModel().
                weapons[0].projectile.behaviors.First(a=>a.name.Contains("Retarget")).Duplicate());
            Glaive.weapons[0].projectile.pierce=3;
            Glaive.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Strait")).Cast<TravelStraitModel>().Lifespan=0.75f;
            Glaive.weapons[0].projectile.display="MutaliskGlaivePrefab";
            Glaive.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Retarget")).Cast<RetargetOnContactModel>().maxBounces=2;
            Glaive.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Retarget")).Cast<RetargetOnContactModel>().distance=30;
            Glaive.range=Mutalisk.range;
            Glaive.weapons[0].rate=1.65f;
            Mutalisk.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="MutaliskPrefab";
        }
        public class ViciousGlaive:ModUpgrade<Mutalisk>{
            public override string Name=>"ViciousGlaive";
            public override string DisplayName=>"Vicious Glaive";
            public override string Description=>"Increasing the bone density of glaive wurms before they are fired allows them to survive for longer and hit more targets";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskViciousGlaiveIcon");
                var Glaive=Mutalisk.behaviors.First(a=>a.name.Contains("Glaive")).Cast<AttackModel>();
                Glaive.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Retarget")).Cast<RetargetOnContactModel>().maxBounces+=2;
                Glaive.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Retarget")).Cast<RetargetOnContactModel>().distance+=10;
            }
        }
        public class RapidRegeneration:ModUpgrade<Mutalisk>{
            public override string Name=>"RapidRegeneration";
            public override string DisplayName=>"Rapid Regeneration";
            public override string Description=>"Decreases the time required to fully grow a glaive before firing it effectively increasing fire rate";
            public override int Cost=>750;
            public override int Path=>MIDDLE;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskRapidRegenerationIcon");
                var Glaive=Mutalisk.behaviors.First(a=>a.name.Contains("Glaive")).Cast<AttackModel>();
                Glaive.weapons[0].rate-=0.3f;
            }
        }
        public class SlicingGlaive:ModUpgrade<Mutalisk>{
            public override string Name=>"SlicingGlaive";
            public override string DisplayName=>"Slicing Glaive";
            public override string Description=>"Evolving the glaive to pierce through armour allows popping leads and increases damage to moabs and ceremics";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskSlicingGlaiveIcon");
                var Glaive=Mutalisk.behaviors.First(a=>a.name.Contains("Glaive")).Cast<AttackModel>();
                Glaive.weapons[0].projectile.GetDamageModel().immuneBloonProperties=0;
            }
        }
        public class AeroGlaive:ModUpgrade<Mutalisk>{
            public override string Name=>"AeroGlaive";
            public override string DisplayName=>"Aerodynamic Glaive";
            public override string Description=>"Growing more aerodynamic glaives allows them to be fired further";
            public override int Cost=>750;
            public override int Path=>MIDDLE;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("HydraliskFrenzyIcon");
                var Glaive=Mutalisk.behaviors.First(a=>a.name.Contains("Glaive")).Cast<AttackModel>();
                Glaive.range+=12.5f;
                Mutalisk.range=Glaive.range;
            }
        }
        public class SunderingGlaive:ModUpgrade<Mutalisk>{
            public override string Name=>"SunderingGlaive";
            public override string DisplayName=>"Sundering Glaive";
            public override string Description=>"Evolving the glaive to explode when hitting a target increases damage by a lot, can no longer bounce";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskSunderingGlaiveIcon");
                Mutalisk.display="MutaliskWebbyPrefab";
                var Glaive=Mutalisk.behaviors.First(a=>a.name.Contains("Glaive")).Cast<AttackModel>();
                Glaive.weapons[0].projectile.behaviors=Glaive.weapons[0].projectile.behaviors.Remove(a=>a.name.Contains("Retarget"));
                Glaive.weapons[0].projectile.pierce=1;
                Glaive.weapons[0].projectile.behaviors=Glaive.weapons[0].projectile.behaviors.Add(Game.instance.model.towers.First(a=>a.name.Contains("BombShooter-030")).
                behaviors.First(a=>a.name.Contains("Attack")).Clone().Cast<AttackModel>().weapons[0].projectile.behaviors.First(a=>a.name.Contains("Contact")).Duplicate());
                Glaive.weapons[0].projectile.GetDamageModel().damage+=5;
            }
        }
        public class Primal:ModUpgrade<Mutalisk>{
            public override string Name=>"MutaliskPrimal";
            public override string DisplayName=>"Primal Evolution";
            public override string Description=>"Gains a random bonus to attack speed, range or damage";
            public override int Cost=>750;
            public override int Path=>MIDDLE;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskPrimalIcon");
                Mutalisk.portrait=new("MutaliskPrimalPortrait");
                Mutalisk.display="MutaliskPrimalPrefab";
            }
        }
        public class Devourer:ModUpgrade<Mutalisk>{
            public override string Name=>"Devourer";
            public override string DisplayName=>"Morph into Devourer";
            public override string Description=>"Devourers are heavy anti air flyers, dealing great damage against MOAB class bloons";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskDevourerIcon");
                Mutalisk.portrait=new("MutaliskDevourerPortrait");
                Mutalisk.display="MutaliskDevourerPrefab";
                var Glaive=Mutalisk.behaviors.First(a=>a.name.Contains("Glaive")).Cast<AttackModel>();
                Glaive.weapons[0].projectile.behaviors=Glaive.weapons[0].projectile.behaviors.Add(Game.instance.model.towers.First(a=>a.name.Contains("SniperMonkey-400")).
                    behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>().weapons[0].projectile.behaviors.First(a=>a.name.Contains("DamageModifierForTagModel")).Duplicate());
                var GlaiveExtraDamage=Glaive.weapons[0].projectile.behaviors.First(a=>a.name.Contains("DamageModifier")).Cast<DamageModifierForTagModel>();
                GlaiveExtraDamage.tags[0]=null;
                GlaiveExtraDamage.tags.AddItem("Moab");
                GlaiveExtraDamage.damageMultiplier=1.5f;
                GlaiveExtraDamage.damageAddative=10;
                Glaive.weapons[0].projectile.GetDamageModel().damage+=5;
                Glaive.range+=10;
                Mutalisk.range=Glaive.range;
                Glaive.weapons[0].rate=1.4f;
            }
        }
        public class BroodLord:ModUpgrade<Mutalisk>{
            public override string Name=>"BroodLordPrimal";
            public override string DisplayName=>"Morph into Brood Lord";
            public override string Description=>"Heavy Zerg seige flyer based off from Guardians, attacks by shooting Broodlings at its target";
            public override int Cost=>750;
            public override int Path=>MIDDLE;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskBroodLordIcon");
                Mutalisk.portrait=new("MutaliskBroodLordPortrait");
                Mutalisk.display="MutaliskBroodLordPrefab";
                var Glaive=Mutalisk.behaviors.First(a=>a.name.Contains("Glaive")).Cast<AttackModel>();
                Glaive.weapons[0].projectile=Game.instance.model.towers.First(a=>a.name.Contains("WizardMonkey-004")).behaviors.First(a=>a.name.
                    Contains("AttackModel_Attack Necromancer_")).Clone().Cast<AttackModel>().weapons[0].projectile.Duplicate();
                Glaive.weapons[0].projectile.pierce=5;
                Glaive.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Path")).Cast<TravelAlongPathModel>().disableRotateWithPathDirection=false;
                Glaive.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Path")).Cast<TravelAlongPathModel>().speedFrames=0.6f;
                Glaive.weapons[0].projectile.display="MutaliskBroodlingPrefab";
                Glaive.range+=25;
                Glaive.weapons[0].projectile.GetDamageModel().damage+=10;
                Mutalisk.range=Glaive.range;
                Mutalisk.radius=20;
            }
        }
        /*public class Leviathan:ModUpgrade<Mutalisk>{
            public override string Name=>"Leviathan";
            public override string DisplayName=>"Morph into Leviathan";
            public override string Description=>"\"Leviathan. Largest of Zerg. Sky will belong to Swarm\"";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Mutalisk){
                GetUpgradeModel().icon=new("MutaliskLeviathanIcon");
                Mutalisk.portrait=new("MutaliskLeviathanPortrait");
                Mutalisk.display="MutaliskLeviathanPrefab";
                Mutalisk.behaviors=Mutalisk.behaviors.Remove(a=>a.name.Contains("Glaive"));
                var Tentacle=Game.instance.model.towers.First(a=>a.name.Contains("DartMonkey")).GetAttackModel();
                var test=Game.instance.model.towers.First(a=>a.name.Contains("MonkeyBuccaneer-040")).behaviors.First(a=>a.name.Contains("Take")).Cast<AbilityModel>().
                    behaviors.First(a=>a.name.Contains("Activate")).Clone().Cast<ActivateAttackModel>().attacks[0];
                var Bile=Game.instance.model.towers.First(a=>a.name.Contains("SuperMonkey-100")).behaviors.First(a=>a.name.Contains("Attack")).Clone().Cast<AttackModel>();
                Tentacle=test;
                Tentacle.weapons[0].rate=0.01f;
                
                //Tentacle.weapons[0].projectile.behaviors.Remove(a=>a.name.Contains("Take"));
                Mutalisk.behaviors=Mutalisk.behaviors.Add(Tentacle,new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true));
            }
        }*/
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Contains("Mutalisk")){
                    var udn=GetMutalisk(__instance.PrototypeRoot,objectId);
                    udn.name="SC2Expansion-Mutalisk";
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
        public static UnityDisplayNode GetMutalisk(Transform transform,string model){
            var udn=Object.Instantiate(Assets.LoadAsset(model).Cast<GameObject>(),transform).AddComponent<UnityDisplayNode>();
            udn.Active=false;
            udn.transform.position=new(-3000,0);
            return udn;
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public record ResourceLoader_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.Contains("Mutalisk")){
                    var text=Assets.LoadAsset(reference.guidRef).Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),nameof(Weapon.SpawnDart))]
        public static class WI{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.towerModel.name.Contains("Mutalisk")){
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("MutaliskAttack");
                }
            }
        }
    }
}