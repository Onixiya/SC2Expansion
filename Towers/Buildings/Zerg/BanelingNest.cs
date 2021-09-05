﻿using Assets.Scripts.Models.Towers.Projectiles;

namespace SC2Expansion.Towers{
    public class BanelingNest:ModTower{
        public override string TowerSet=>PRIMARY;
        public override string BaseTower=>"WizardMonkey-004";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string Description=>"Spawns Banelings. Small zerg, very explosive and suicidal. Do not poke";
        public override void ModifyBaseTowerModel(TowerModel BanelingNest){
            BanelingNest.display="BanelingNestPrefab";
            BanelingNest.portrait=new("BanelingNestIcon");
            BanelingNest.icon=new("BanelingNestIcon");
            BanelingNest.emoteSpriteLarge=new("Zerg");
            BanelingNest.emoteSpriteSmall=new("Zerg");
            BanelingNest.radius=11.5f;
            BanelingNest.range=15;
            BanelingNest.towerSize=TowerModel.TowerSize.XL;
            BanelingNest.areaTypes=new(1);
            BanelingNest.areaTypes[0]=AreaType.land;
            BanelingNest.behaviors=BanelingNest.behaviors.Remove(a=>a.name.Contains("Shimmer"));
            BanelingNest.behaviors=BanelingNest.behaviors.Remove(a=>a.name.Equals("AttackModel_Attack_"));
            var SpawnBanelings=BanelingNest.behaviors.First(a=>a.name.Contains("Attack")).Cast<AttackModel>();
            var SpawnBanelingsZone=BanelingNest.behaviors.First(a=>a.name.Contains("Zone")).Cast<NecromancerZoneModel>();
            //Cloning the 005 wizards moab track path model thing as that actually tracks what way the track is going so the banelings don't all look in one direction
            var GhostMoabPath=Game.instance.model.GetTowerFromId("WizardMonkey-005").Cast<TowerModel>().behaviors.
                First(a=>a.name.Equals("AttackModel_Attack Necromancer_")).Clone().Cast<AttackModel>().weapons[1].projectile.behaviors.First(a=>a.name.Contains("Path")).Cast<TravelAlongPathModel>();
            SpawnBanelings.weapons[0].projectile.behaviors=SpawnBanelings.weapons[0].projectile.behaviors.Remove(a=>a.name.Contains("Path"));
            SpawnBanelings.weapons[0].projectile.behaviors=SpawnBanelings.weapons[0].projectile.behaviors.Add(GhostMoabPath);
            SpawnBanelings.weapons[0].projectile.display="BanelingNestBanelingPrefab";
            SpawnBanelings.weapons[0].emission.Cast<NecromancerEmissionModel>().maxRbeStored=2147483646;
            //Pierce acts as the hp for the unit here, setting it really low 'cus banelings aren't exactly the sorta thing you reuse after they've attacked
            SpawnBanelings.weapons[0].emission.Cast<NecromancerEmissionModel>().maxPiercePerBloon=1;
            SpawnBanelings.weapons[0].rate=4.5f;
            SpawnBanelings.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().lifespanFrames=2147483646;
            SpawnBanelings.weapons[0].projectile.GetDamageModel().damage=4;
            SpawnBanelings.name="SpawnBanelings";
            SpawnBanelingsZone.attackUsedForRangeModel.range=500;
            SpawnBanelingsZone.name="SpawnBanelingsZone";
            BanelingNest.behaviors.First(a=>a.name.Contains("Display")).Cast<DisplayModel>().display="BanelingNestPrefab";
        }
        public override int GetTowerIndex(List<TowerDetailsModel> towerSet) {
            return towerSet.First(model => model.towerId==TowerType.BoomerangMonkey).towerIndex+1;
        }
        public class CentrifugalHooks:ModUpgrade<BanelingNest> {
            public override string Name=>"CentrifugalHooks";
            public override string DisplayName=>"Centrifugal Hooks";
            public override string Description=>"Evolving powerful hooks allows Banelings to roll and move faster";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel BanelingNest){
                GetUpgradeModel().icon=new("BanelingNestCentrifugalHooksIcon");
                var SpawnBanelings=BanelingNest.behaviors.First(a=>a.name.Equals("SpawnBanelings")).Cast<AttackModel>();
                SpawnBanelings.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().speedFrames=0.7f;
                SpawnBanelings.weapons[0].projectile.display="BanelingNestBanelingRollPrefab";
            }
        }
        public class Rupture:ModUpgrade<BanelingNest> {
            public override string Name=>"Rupture";
            public override string DisplayName=>"Rupture";
            public override string Description=>"Adding more volatile chemicals and more internel pressure increases damage and radius";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel BanelingNest) {
                GetUpgradeModel().icon=new("BanelingNestRuptureIcon");
                var SpawnBanelings=BanelingNest.behaviors.First(a=>a.name.Equals("SpawnBanelings")).Cast<AttackModel>();
                SpawnBanelings.weapons[0].projectile.GetDamageModel().damage=6;
                SpawnBanelings.weapons[0].projectile.radius=7;
            }
        }
        public class CorrosiveAcid:ModUpgrade<BanelingNest> {
            public override string Name=>"CorrosiveAcid";
            public override string DisplayName=>"Corrosive Acid";
            public override string Description=>"Increasing the water content of the acid allows a small amount to be left on the track that strips camo and fortification on non Moabs";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel BanelingNest) {
                GetUpgradeModel().icon=new("BanelingNestCorrosiveAcidIcon");
                var SpawnBanelings=BanelingNest.behaviors.First(a=>a.name.Equals("SpawnBanelings")).Cast<AttackModel>();
                SpawnBanelings.weapons[0].projectile.behaviors=SpawnBanelings.weapons[0].projectile.behaviors.Add(Game.instance.model.towers.
                    First(a=>a.name.Contains("EngineerMonkey-030")).Cast<TowerModel>().behaviors.First(a=>a.name.Contains("CleansingFoam")).Cast<AttackModel>()
                    .weapons[0].projectile.behaviors.First(a=>a.name.Contains("Exhaust")));
                SpawnBanelings.weapons[0].projectile.display="BanelingNestBaneling3Prefab";
                var AcidPool=SpawnBanelings.weapons[0].projectile.behaviors.First(a=>a.name.Contains("CreateProj")).Cast<CreateProjectileOnExhaustFractionModel>();
                AcidPool.projectile.behaviors.First(a=>a.name.Contains("Modifier")).Cast<RemoveBloonModifiersModel>().cleanseFortified=true;
                AcidPool.projectile.behaviors.First(a=>a.name.Contains("Modifier")).Cast<RemoveBloonModifiersModel>().cleanseLead=false;
                AcidPool.projectile.behaviors.First(a=>a.name.Contains("Modifier")).Cast<RemoveBloonModifiersModel>().blacklistTagModFilter.Add("Moabs");
            }
        }
        public class Splitter:ModUpgrade<BanelingNest> {
            public override string Name=>"BanelingRateIncrease";
            public override string DisplayName=>"Growth Enzymes";
            public override string Description=>"Adding more nutrients when Banelings are evolving lets them grow faster and bigger";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel BanelingNest){
                GetUpgradeModel().icon=new("BanelingNestRateIncreaseIcon");
                var SpawnBanelings=BanelingNest.behaviors.First(a=>a.name.Equals("SpawnBanelings")).Cast<AttackModel>();
                SpawnBanelings.weapons[0].rate=2;
                SpawnBanelings.weapons[0].projectile.GetDamageModel().damage=9;
            }
        }
        public class Kaboomer:ModUpgrade<BanelingNest>{
            public override string Name=>"Kaboomer";
            public override string DisplayName=>"Kaboomer";
            public override string Description=>"Kaboomers are a heavy strain of Banelings that do massive damage";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel BanelingNest){
                GetUpgradeModel().icon=new("BanelingNestKaboomerIcon");
                var SpawnBanelings=BanelingNest.behaviors.First(a=>a.name.Equals("SpawnBanelings")).Cast<AttackModel>();
                SpawnBanelings.weapons[0].projectile.behaviors.First(a=>a.name.Contains("Travel")).Cast<TravelAlongPathModel>().speedFrames=0.4f;
                SpawnBanelings.weapons[0].rate=6;
                SpawnBanelings.weapons[0].projectile.GetDamageModel().damage=100;
                SpawnBanelings.weapons[0].projectile.display="BanelingNestKaboomerPrefab";
            }
        }
        [HarmonyPatch(typeof(Factory),nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch{
            public static Dictionary<string,UnityDisplayNode>protos=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!protos.ContainsKey(objectId)&&objectId.Equals("BanelingNestPrefab")){
                    var udn=GetBanelingNest(__instance.PrototypeRoot,"BanelingNestPrefab");
                    udn.name="SC2Expansion-BanelingNest";
                    var a=Assets.LoadAsset("BanelingNestMaterial");
                    udn.genericRenderers[0].material=a.Cast<Material>();
                    udn.RecalculateGenericRenderers();
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("BanelingNestBanelingPrefab")){
                    var udn=GetBanelingNest(__instance.PrototypeRoot,"BanelingNestBanelingPrefab");
                    udn.name="SC2Expansion-BanelingNest";
                    var a=Assets.LoadAsset("BanelingNestBanelingMaterial");
                    udn.genericRenderers[0].material=a.Cast<Material>();
                    udn.RecalculateGenericRenderers();
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("BanelingNestBanelingRollPrefab")) {
                    var udn=GetBanelingNest(__instance.PrototypeRoot,"BanelingNestBanelingRollPrefab");
                    udn.name="SC2Expansion-BanelingNest";
                    var a=Assets.LoadAsset("BanelingNestBanelingMaterial");
                    udn.genericRenderers[0].material=a.Cast<Material>();
                    udn.RecalculateGenericRenderers();
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("BanelingNestBaneling3Prefab")) {
                    var udn=GetBanelingNest(__instance.PrototypeRoot,"BanelingNestBaneling3Prefab");
                    udn.name="SC2Expansion-BanelingNest";
                    var a=Assets.LoadAsset("BanelingNestBaneling3Material");
                    udn.genericRenderers[0].material=a.Cast<Material>();
                    udn.RecalculateGenericRenderers();
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId,udn);
                    return false;
                }
                if(!protos.ContainsKey(objectId)&&objectId.Equals("BanelingNestKaboomerPrefab")) {
                    var udn=GetBanelingNest(__instance.PrototypeRoot,"BanelingNestKaboomerPrefab");
                    udn.name="SC2Expansion-BanelingNest";
                    var a=Assets.LoadAsset("BanelingNestBanelingMaterial");
                    udn.genericRenderers[0].material=a.Cast<Material>();
                    udn.RecalculateGenericRenderers();
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
        public static UnityDisplayNode GetBanelingNest(Transform transform,string model){
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
                if(reference!=null&&reference.guidRef.Equals("BanelingNestIcon")){
                    var b=Assets.LoadAsset("BanelingNestIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BanelingNestRuptureIcon")){
                    var b=Assets.LoadAsset("BanelingNestRuptureIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BanelingNestCentrifugalHooksIcon")){
                    var b=Assets.LoadAsset("BanelingNestCentrifugalHooksIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BanelingNestCorrosiveAcidIcon")){
                    var b=Assets.LoadAsset("BanelingNestCorrosiveAcidIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BanelingNestKaboomerIcon")){
                    var b=Assets.LoadAsset("BanelingNestKaboomerIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
                if(reference!=null&&reference.guidRef.Equals("BanelingNestRateIncreaseIcon")){
                    var b=Assets.LoadAsset("BanelingNestRateIncreaseIcon");
                    var text=b.Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
    }
}