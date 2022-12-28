[assembly:MelonGame("Ninja Kiwi","BloonsTD6")]
[assembly:MelonInfo(typeof(SC2ExpansionLoader.ModMain),ModHelperData.Name,ModHelperData.Version,"Silentstorm")]
namespace SC2ExpansionLoader{
    public class ModMain:MelonMod{
        public static Dictionary<string,SC2Tower>TowerTypes=new Dictionary<string,SC2Tower>();
        public static AbilityModel BlankAbilityModel;
        private static MelonLogger.Instance mllog;
        public static string BundleDir;
        public static void Log(object thingtolog,string type="msg"){
            switch(type){
                case"msg":
                    mllog.Msg(thingtolog);
                    break;
                case"warn":
                    mllog.Warning(thingtolog);
                    break;
                 case"error":
                    mllog.Error(thingtolog);
                    break;
            }
        }
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
                    __instance.unlockedTowers.Add(tower);
                    foreach(UpgradeModel upgrade in TowerTypes[tower].Upgrades()){
                        __instance.acquiredUpgrades.Add(upgrade.name);
                    }
                }
            }
        }
        public static void SaveIl2CppObj(string fileName,Il2CppSystem.Object obj){
            System.IO.File.WriteAllText(fileName,JsonConvert.SerializeObject(obj,Formatting.Indented));
        }
        public override void OnInitializeMelon(){
            mllog=LoggerInstance;
            BundleDir=MelonEnvironment.UserDataDirectory+"/SC2ExpansionBundles/";
            Directory.CreateDirectory(BundleDir);
            foreach(MelonMod mod in RegisteredMelons){
                if(mod.Info.Name.StartsWith("SC2Expansion")){
                    Assembly assembly=mod.MelonAssembly.Assembly;
                    foreach(Type type in assembly.GetTypes()){
                        try{
                            SC2Tower tower=(SC2Tower)Activator.CreateInstance(type);
                            if(tower.Name!=""){
                                TowerTypes.Add(tower.Name,tower);
                                TowerTypes[tower.Name].LoadedBundle=UnityEngine.AssetBundle.LoadFromFileAsync(BundleDir+tower.Name.ToLower()).assetBundle;
                            }
                        }catch{}
                    }
                    foreach(string bundle in assembly.GetManifestResourceNames()){
                        Stream stream=assembly.GetManifestResourceStream(bundle);
                        byte[]bytes=new byte[stream.Length];
                        stream.Read(bytes);
                        File.WriteAllBytes(BundleDir+bundle.Split('.')[2],bytes);
                    }                     
                }
            }
        }
        public static uObject LoadAsset<T>(string Asset,AssetBundle Bundle){
            try{
                return Bundle.LoadAssetAsync(Asset,Il2CppType.Of<T>()).asset;
            }catch{
                foreach(KeyValuePair<string,SC2Tower>tower in TowerTypes){
                    tower.Value.LoadedBundle=UnityEngine.AssetBundle.LoadFromFileAsync(BundleDir+tower.Key.ToLower()).assetBundle;
                    /*foreach(var thing in tower.Value.LoadedBundle.GetAllAssetNames()){
                        Log(thing);
                    }*/
                }
                return Bundle.LoadAssetAsync(Asset,Il2CppType.Of<T>()).asset;
            }
        }
        [HarmonyPatch(typeof(TitleScreen),"Start")]
        public class TitleScreenStart_Patch{
            [HarmonyPostfix]
            public static void Postfix(){
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
                List<TowerModel>towers=Game.instance.model.towers.ToList();
                List<TowerDetailsModel>towerSet=Game.instance.model.towerSet.ToList();
                List<UpgradeModel>upgrades=Game.instance.model.upgrades.ToList();
                List<string>towerNames=new();
                try{
                    foreach(SC2Tower tower in TowerTypes.Values){
                        towerNames.Add(tower.Name);
                        foreach(TowerModel towerModel in tower.TowerModels()){
                            Log(1);
                            towers.Add(towerModel);
                            Log(2);
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
                }catch{
                    Log("Failed to add "+towerNames.Last());
                }
            }
        }
        [HarmonyPatch(typeof(Factory.__c__DisplayClass21_0),"_CreateAsync_b__0")]
        public class FactoryCreateAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(ref Factory.__c__DisplayClass21_0 __instance,ref UnityDisplayNode prototype){
                string towerName=__instance.objectId.guidRef.Split('-')[0];
                if(towerName!=null&&TowerTypes.ContainsKey(towerName)){
                    SC2Tower tower=TowerTypes[towerName];
                    GameObject gObj=uObject.Instantiate(LoadAsset<GameObject>(__instance.objectId.guidRef,tower.LoadedBundle).Cast<GameObject>(),
                        __instance.__4__this.DisplayRoot);
                    gObj.name=__instance.objectId.guidRef;
                    gObj.transform.position=new(0,0,30000);
                    gObj.AddComponent<UnityDisplayNode>();
                    gObj.AddComponent<SC2Sound>();
                    prototype=gObj.GetComponent<UnityDisplayNode>();
                    __instance.__4__this.active.Add(prototype);
                    __instance.onComplete.Invoke(prototype);
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
                    SC2Tower tower=TowerTypes[towerName];
                    Texture2D texture=LoadAsset<Texture2D>(name,tower.LoadedBundle).Cast<Texture2D>();
                    __result=Sprite.Create(texture,new(0,0,texture.width,texture.height),new());
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public class WeaponSpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(Weapon __instance){
                string towerName=__instance.attack.tower.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    TowerTypes[towerName].Attack(__instance);
                }
            }
        }
        [HarmonyPatch(typeof(Tower),"OnPlace")]
        public class TowerOnPlace_Patch{
            [HarmonyPostfix]
            public static void Postfix(Tower __instance){
                string towerName=__instance.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    TowerTypes[towerName].Create();
                }
            }
        }
        [HarmonyPatch(typeof(Ability),"Activate")]
        public class AbilityActivate_Patch{
            [HarmonyPostfix]
            public static void Postfix(Ability __instance){
                string towerName=__instance.tower.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    TowerTypes[towerName].Ability(__instance.abilityModel.name,__instance.tower);
                }
            }
        }
        [HarmonyPatch(typeof(AudioFactory),nameof(AudioFactory.Start))]
        public class AudioFactoryStart_Patch{
            [HarmonyPostfix]
            public static void Postfix(AudioFactory __instance){
                foreach(string bundlePath in Directory.GetFiles(BundleDir)){
                    if(bundlePath.EndsWith("clips")){
                        AssetBundleRequest bundle=AssetBundle.LoadFromFileAsync(bundlePath).assetBundle.LoadAllAssetsAsync<AudioClip>();
                        foreach(uObject asset in bundle.allAssets){
                            __instance.RegisterAudioClip(asset.name,asset.Cast<AudioClip>());
                        }
                    }
                }
            }
        }
        [HarmonyPatch(typeof(TowerManager),"UpgradeTower")]
        public class TowerManagerUpgradeTower_Patch{
            [HarmonyPostfix]
            public static void Postfix(Tower tower,int pathIndex){
                string towerName=tower.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    TowerTypes[towerName].Upgrade(pathIndex,tower);
                }
            }
        }
        public static void PlaySound(string name){
            Game.instance.audioFactory.PlaySoundFromUnity(null,name,"FX",1,1);
        }
        public static void PlayAnimation(UnityDisplayNode udn,string anim){
            udn.GetComponent<Animator>().Play(anim);
        }
        [HarmonyPatch(typeof(TowerSelectionMenu),"SelectTower")]
        public class TowerSelectionMenuSelectTower_Patch{
            [HarmonyPostfix]
            public static void Postfix(TowerSelectionMenu __instance){
                string towerName=__instance.selectedTower.tower.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    TowerTypes[towerName].Select(__instance.selectedTower.tower);
                }
            }
        }
    }
}