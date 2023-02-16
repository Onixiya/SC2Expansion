[assembly:MelonGame("Ninja Kiwi","BloonsTD6")]
[assembly:MelonInfo(typeof(SC2ExpansionLoader.ModMain),ModHelperData.Name,ModHelperData.Version,"Silentstorm")]
namespace SC2ExpansionLoader{
    public class ModMain:MelonMod{
        public static Dictionary<string,SC2Tower>TowerTypes=new Dictionary<string,SC2Tower>();
        public static AbilityModel BlankAbilityModel;
        private static MelonLogger.Instance mllog;
        public static string BundleDir;
        public static AttackModel CreateTowerAttackModel;
        public static GameModel gameModel;
        public static LocalizationManager LocManager;
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
        public static void SaveIl2CppObj(string fileName,Il2CppSystem.Object obj){
            System.IO.File.WriteAllText(fileName,JsonConvert.SerializeObject(obj,Formatting.Indented));
        }
        public override void OnInitializeMelon(){
            mllog=LoggerInstance;
            BundleDir=MelonEnvironment.UserDataDirectory+"/SC2ExpansionBundles/";
            BundleDir=BundleDir.Replace('\\','/');
            Directory.CreateDirectory(BundleDir);
            foreach(MelonMod mod in RegisteredMelons){
                if(mod.Info.Name.StartsWith("SC2Expansion")){
                    Assembly assembly=mod.MelonAssembly.Assembly;
                    foreach(Type type in assembly.GetTypes()){
                        try{
                            SC2Tower tower=(SC2Tower)Activator.CreateInstance(type);
                            if(tower.Name!=""){
                                TowerTypes.Add(tower.Name,tower);
                                tower.LoadedBundle=UnityEngine.AssetBundle.LoadFromFileAsync(BundleDir+tower.Name.ToLower()).assetBundle;
                                if(tower.TowerFaction==SC2Tower.Faction.NotSet){
                                    Log(tower.Name+"'s faction not set!","warn");
                                }
                            }
                        }catch{}
                    }
                    foreach(string bundle in assembly.GetManifestResourceNames()){
                        try{
                            Stream stream=assembly.GetManifestResourceStream(bundle);
                            byte[]bytes=new byte[stream.Length];
                            stream.Read(bytes);
                            File.WriteAllBytes(BundleDir+bundle.Split('.')[2],bytes);
                        }catch(Exception error){
                            Log("Failed to write "+bundle);
                            string message=error.Message;
                            message+="@\n"+error.StackTrace;
                            Log(message,"error");
                        }
                    }                     
                }
            }
        }
        public static T LoadAsset<T>(string Asset,AssetBundle Bundle)where T:uObject{
            try{
                return Bundle.LoadAssetAsync(Asset,Il2CppType.Of<T>()).asset.Cast<T>();
            }catch{
                foreach(KeyValuePair<string,SC2Tower>tower in TowerTypes){
                    tower.Value.LoadedBundle=UnityEngine.AssetBundle.LoadFromFileAsync(BundleDir+tower.Key.ToLower()).assetBundle;
                }
                try{
                    return Bundle.LoadAssetAsync(Asset,Il2CppType.Of<T>()).asset.Cast<T>();
                }catch(Exception error){
                    Log("Failed to load "+Asset+" from "+Bundle.name);
                    try{
                        Log("Attempting to get available assets");
                        foreach(string asset in Bundle.GetAllAssetNames()){
                            Log(asset);
                        }
                    }catch{
                        Log("Bundle is null");
                    }
                    string message=error.Message;
                    message+="@\n"+error.StackTrace;
                    Log(message,"error");
                    return null;
                }
            }
        }
        public static void PlaySound(string name){
            try{
                Game.instance.audioFactory.PlaySoundFromUnity(null,name,"FX",1,1);
            }catch(Exception error){
                Log("Failed to play clip "+name);
                string message=error.Message;
                message+="@\n"+error.StackTrace;
                Log(message,"error");
            }
        }
        public static void PlayAnimation(UnityDisplayNode udn,string anim,float duration=0.2f){
            try{
                if(udn!=null){
                    udn.GetComponent<Animator>().CrossFade(anim,duration,0,0);
                }
            }catch(Exception error){
                Log("Failed to play animation "+anim);
                string message=error.Message;
                message+="@\n"+error.StackTrace;
                Log(message,"error");
            }
        }
    }
}