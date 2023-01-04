using Il2CppAssets.Scripts.Data;

namespace SC2ExpansionLoader{
    public class HarmonyPatches{
        [HarmonyPatch(typeof(Btd6Player),"CheckForNewParagonPipEvent")]
        public class Btd6PlayerCheckForNewParagonPipEvent_Patch{
            [HarmonyPrefix]
            public static bool Prefix(){
                return false;
            }
        }
        [HarmonyPatch(typeof(ProfileModel),"Validate")]
        public class ProfileModelValidate_Patch{
            [HarmonyPostfix]
            public static void Postfix(ProfileModel __instance){
                foreach(string tower in TowerTypes.Keys){
                    try{
                        __instance.unlockedTowers.Add(tower);
                        foreach(UpgradeModel upgrade in TowerTypes[tower].Upgrades()){
                            __instance.acquiredUpgrades.Add(upgrade.name);
                        }
                    }catch(Exception error){
                        Log("Failed to add "+tower+" to unlocked towers or upgrades");
                        string message=error.Message;
                        message+="@\n"+error.StackTrace;
                        Log(message,"error");
                    }
                }
            }
        }
        [HarmonyPatch(typeof(TitleScreen),"Start")]
        public class TitleScreenStart_Patch{
            [HarmonyPostfix]
            public static void Postfix(){
                try{
                    BlankAbilityModel=Game.instance.model.GetTowerFromId("Quincy 4").Cast<TowerModel>().behaviors.
                        First(a=>a.GetIl2CppType().Name=="AbilityModel").Clone().Cast<AbilityModel>();
                    BlankAbilityModel.description="AbilityDescription";
                    BlankAbilityModel.displayName="AbilityDisplayName";
                    BlankAbilityModel.name="AbilityName";
                    List<Model>behaviors=BlankAbilityModel.behaviors.ToList();
                    behaviors.Remove(behaviors.First(a=>a.GetIl2CppType().Name=="TurboModel"));
                    behaviors.Remove(behaviors.First(a=>a.GetIl2CppType().Name=="CreateEffectOnAbilityModel"));
                    behaviors.Remove(behaviors.First(a=>a.GetIl2CppType().Name=="CreateSoundOnAbilityModel"));
                    BlankAbilityModel.behaviors=behaviors.ToArray();
                }catch(Exception error){
                    Log("Failed to create BlankAbilityModel");
                    string message=error.Message;
                    message+="@\n"+error.StackTrace;
                    Log(message,"error");
                }
                List<TowerModel>towers=Game.instance.model.towers.ToList();
                List<TowerDetailsModel>towerSet=Game.instance.model.towerSet.ToList();
                List<UpgradeModel>upgrades=Game.instance.model.upgrades.ToList();
                List<string>towerNames=new();
                try{
                    foreach(SC2Tower tower in TowerTypes.Values){
                        towerNames.Add(tower.Name);
                        foreach(TowerModel towerModel in tower.TowerModels()){
                            towers.Add(towerModel);
                        }
                        towerSet.Add(tower.ShopDetails());
                        foreach(UpgradeModel upgrade in tower.Upgrades()){
                            upgrades.Add(upgrade);
                        }
                        Game.instance.model.towers=towers.ToArray();
                        Game.instance.model.towerSet=towerSet.ToArray();
                        Game.instance.model.upgrades=upgrades.ToArray();
                        Log("Loaded "+tower.Name);
                    }
                }catch(Exception error){
                    Log("Failed to add "+towerNames.Last());
                    string message=error.Message;
                    message+="@\n"+error.StackTrace;
                    Log(message,"error");
                }
            }
        }
        [HarmonyPatch(typeof(Factory.__c__DisplayClass21_0),"_CreateAsync_b__0")]
        public class FactoryCreateAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(ref Factory.__c__DisplayClass21_0 __instance,ref UnityDisplayNode prototype){
                string towerName=__instance.objectId.guidRef.Split('-')[0];
                if(towerName!=null&&TowerTypes.ContainsKey(towerName)){
                    try{
                        SC2Tower tower=TowerTypes[towerName];
                        GameObject gObj=uObject.Instantiate(LoadAsset<GameObject>(__instance.objectId.guidRef,tower.LoadedBundle).Cast<GameObject>(),
                            __instance.__4__this.DisplayRoot);
                        gObj.name=__instance.objectId.guidRef;
                        gObj.transform.position=new(0,0,30000);
                        gObj.AddComponent<UnityDisplayNode>();
                        SC2Sound sound=gObj.AddComponent<SC2Sound>();
                        sound.MaxSelectQuote=tower.MaxSelectQuote;
                        sound.MaxUpgradeQuote=tower.MaxUpgradeQuote;
                        if(tower.Behaviours.ContainsKey(gObj.name)){
                            gObj.AddComponent(tower.Behaviours[gObj.name]);
                        }
                        prototype=gObj.GetComponent<UnityDisplayNode>();
                        __instance.__4__this.active.Add(prototype);
                        __instance.onComplete.Invoke(prototype);
                    }catch(Exception error){
                        Log("Failed to set "+__instance.objectId.guidRef+" up");
                        string message=error.Message;
                        message+="@\n"+error.StackTrace;
                        Log(message,"error");
                    }
                    return false;
                }
                return true;                
            }
        }
        [HarmonyPatch(typeof(UnityEngine.U2D.SpriteAtlas),"GetSprite")]
        public class SpriteAtlasGetSprite_Patch{
            [HarmonyPostfix]
            public static void Postfix(string name,ref Sprite __result){
                string towerName="";
                try{
                    towerName=name.Split('-')[0];
                }catch{}
                if(TowerTypes.ContainsKey(towerName)){
                    try{
                        SC2Tower tower=TowerTypes[towerName];
                        Texture2D texture=LoadAsset<Texture2D>(name,tower.LoadedBundle).Cast<Texture2D>();
                        __result=Sprite.Create(texture,new(0,0,texture.width,texture.height),new());
                    }catch(Exception error){
                        Log("Failed to set "+name+" up");
                        string message=error.Message;
                        message+="@\n"+error.StackTrace;
                        Log(message,"error");
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public class WeaponSpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(Weapon __instance){
                string towerName=__instance.attack.tower.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    try{
                        TowerTypes[towerName].Attack(__instance);
                    }catch(Exception error){
                        Log("Failed to run Attack for "+towerName);
                        string message=error.Message;
                        message+="@\n"+error.StackTrace;
                        Log(message,"error");
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Tower),"OnPlace")]
        public class TowerOnPlace_Patch{
            [HarmonyPostfix]
            public static void Postfix(Tower __instance){
                string towerName=__instance.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    try{
                        TowerTypes[towerName].Create();
                    }catch(Exception error){
                        Log("Failed to run Create for "+towerName);
                        string message=error.Message;
                        message+="@\n"+error.StackTrace;
                        Log(message,"error");
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Ability),"Activate")]
        public class AbilityActivate_Patch{
            [HarmonyPostfix]
            public static void Postfix(Ability __instance){
                string towerName=__instance.tower.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    try{
                        TowerTypes[towerName].Ability(__instance.abilityModel.name,__instance.tower);
                    }catch(Exception error){
                        Log("Failed to run Ability for "+towerName);
                        string message=error.Message;
                        message+="@\n"+error.StackTrace;
                        Log(message,"error");
                    }
                }
            }
        }
        [HarmonyPatch(typeof(AudioFactory),nameof(AudioFactory.Start))]
        public class AudioFactoryStart_Patch{
            [HarmonyPostfix]
            public static void Postfix(AudioFactory __instance){
                foreach(string bundlePath in Directory.GetFiles(BundleDir)){
                    if(bundlePath.EndsWith("clips")){
                        try{
                            AssetBundleRequest bundle=AssetBundle.LoadFromFileAsync(bundlePath).assetBundle.LoadAllAssetsAsync<AudioClip>();
                            foreach(uObject asset in bundle.allAssets){
                                __instance.RegisterAudioClip(asset.name,asset.Cast<AudioClip>());
                            }
                        }catch(Exception error){
                            Log("Failed to add audio clips from "+bundlePath);
                            string message=error.Message;
                            message+="@\n"+error.StackTrace;
                            Log(message,"error");
                        }
                    }
                }
            }
        }
        [HarmonyPatch(typeof(TowerManager),"UpgradeTower")]
        public class TowerManagerUpgradeTower_Patch{
            [HarmonyPostfix]
            public static void Postfix(Tower tower,TowerModel def){
                string towerName=tower.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    try{
                        TowerTypes[towerName].Upgrade(def.tier,tower);
                    }catch(Exception error){
                        Log("Failed to run Upgrade for "+towerName);
                        string message=error.Message;
                        message+="@\n"+error.StackTrace;
                        Log(message,"error");
                    }
                }
            }
        }
        [HarmonyPatch(typeof(TowerSelectionMenu),"SelectTower")]
        public class TowerSelectionMenuSelectTower_Patch{
            [HarmonyPostfix]
            public static void Postfix(TowerSelectionMenu __instance){
                string towerName=__instance.selectedTower.tower.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    try{
                        TowerTypes[towerName].Select(__instance.selectedTower.tower);
                    }catch(Exception error){
                        Log("Failed to run Select for "+towerName);
                        string message=error.Message;
                        message+="@\n"+error.StackTrace;
                        Log(message,"error");
                    }
                }
            }
        }
    }
}