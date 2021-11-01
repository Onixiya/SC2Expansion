namespace SC2Expansion.Towers{
    public class Marine:ModTower<TerranSet>{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.marine);
        public override string DisplayName=>"Marine";
        public override string BaseTower=>"SniperMonkey";
        public override int Cost=>400;
        public override int TopPathUpgrades=>5;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override bool DontAddToShop=>new ModSettingBool(Ext.TerranEnabled);
        public override string Description=>"Basic Terran soldier with automatic gauss rifle";
        public override void ModifyBaseTowerModel(TowerModel Marine){
            Marine.display="MarinePrefab";
            Marine.portrait=new("MarineIcon");
            Marine.icon=new("MarineIcon");
            Marine.emoteSpriteLarge=new("Terran");
            Marine.radius=5;
            Marine.cost=400;
            Marine.range=35;
            var C14=Marine.GetAttackModel();
            C14.weapons[0].rate=0.25f;
            C14.weapons[0].rateFrames=1;
            C14.range=Marine.range;
            C14.weapons[0].projectile.display=null;
            C14.weapons[0].projectile.GetDamageModel().damage=2.5f;
            Marine.GetBehavior<DisplayModel>().display=Marine.display;
        }
        public class U238Shells:ModUpgrade<Marine>{
            public override string Name=>"U238Shells";
            public override string DisplayName=>"U-238 Shells";
            public override string Description=>"Making the ammunition casing out of depleted Uranium 238 increases range and damage";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>1;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineU238ShellsIcon");
                Marine.range+=10;
                var C14=Marine.GetAttackModel();
                C14.range=Marine.range;
                C14.weapons[0].projectile.GetDamageModel().damage+=1;
            }
        }
        public class LTS:ModUpgrade<Marine>{
            public override string Name=>"LTS";
            public override string DisplayName=>"Laser Targeting System";
            public override string Description=>"Adding a laser pointer allows targetting camo bloons and slightly increases range";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>2;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineLaserTargetingSystemIcon");
                Marine.range+=5;
                Marine.GetAttackModel().range=Marine.range;
                Marine.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_",true));
            }
        }
        public class Stimpacks:ModUpgrade<Marine>{
            public override string Name=>"Stimpacks";
            public override string DisplayName=>"Stimpacks";
            public override string Description=>"Stimpacks increase attack speed by 50% for a short while";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>3;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineStimpacksIcon");
                var Stimpacks=Game.instance.model.GetTowerFromId("BoomerangMonkey-040").GetBehavior<AbilityModel>().Duplicate();
                Stimpacks.name="Stimpacks";
                Stimpacks.displayName="Stimpacks";
                Stimpacks.icon=new("MarineStimpacksIcon");
                Stimpacks.cooldown=40;
                Stimpacks.maxActivationsPerRound=1;
                Stimpacks.GetBehavior<TurboModel>().extraDamage=0;
                Stimpacks.GetBehavior<TurboModel>().projectileDisplay=null;
                Marine.AddBehavior(Stimpacks);
            }
        }
        public class Warpig:ModUpgrade<Marine>{
            public override string Name=>"Warpig";
            public override string DisplayName=>"Warpig";
            public override string Description=>"Warpig mercenaries use upgraded (Don't ask if its legal) equipment. Increases damage and attack speed";
            public override int Cost=>750;
            public override int Path=>TOP;
            public override int Tier=>4;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineWarpigIcon");
                Marine.display="MarineWarpigPrefab";
                Marine.portrait=new("MarineWarpigPortrait");
                var C14=Marine.GetAttackModel();
                C14.weapons[0].rate=0.17f;
                C14.weapons[0].projectile.GetDamageModel().damage+=2;
            }
        }
        public class Raynor:ModUpgrade<Marine>{
            public override string Name=>"Raynor";
            public override string DisplayName=>"James Raynor";
            public override string Description=>"\"Jimmy here!\"";
            public override int Cost=>9000;
            public override int Path=>TOP;
            public override int Tier=>5;
            public override void ApplyUpgrade(TowerModel Marine){
                GetUpgradeModel().icon=new("MarineRaynorIcon");
                Marine.display="MarineRaynorPrefab";
                Marine.portrait=new("MarineRaynorIcon");
                var C14=Marine.GetAttackModel();
                C14.weapons[0].rate=0.13f;
                C14.weapons[0].projectile.GetDamageModel().damage+=3;
                var FragGrenade=Game.instance.model.GetTowerFromId("BombShooter-002").GetAttackModel();
                FragGrenade.name="MarineFragGrenade";
                FragGrenade.AddBehavior(new PauseAllOtherAttacksModel("PauseAllOtherAttacksModel",1,true,true));
                FragGrenade.range=Marine.range-10;
                Marine.AddBehavior(FragGrenade);
            }
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            public static Dictionary<string,UnityDisplayNode>DisplayDict=new();
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!DisplayDict.ContainsKey(objectId)&&objectId.Contains("Marine")){
                    var udn=uObject.Instantiate(TowerAssets.LoadAsset(objectId).Cast<GameObject>(),__instance.PrototypeRoot).AddComponent<UnityDisplayNode>();
                    udn.transform.position=new(-3000,0);
                    udn.name="SC2Expansion-Marine";
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
            public static void Postfix(SpriteReference reference,ref Image image){
                if(reference!=null&&reference.guidRef.Contains("Marine")){
                    var text=TowerAssets.LoadAsset(reference.guidRef).Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public static class WeaponSpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                if(__instance.attack.tower.towerModel.name.Contains("Marine")){
                    if(__instance.attack.attackModel.name.Contains("Grenade")){
                        __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("MarineGrenade");
                    }else{
                        __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("MarineAttack");
                    }
                }
            }
        }
    }
}