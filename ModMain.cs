[assembly:MelonGame("Ninja Kiwi","BloonsTD6")]
[assembly:MelonInfo(typeof(SC2ExpansionLoader.ModMain),ModHelperData.Name,ModHelperData.Version,"Silentstorm")]
namespace SC2ExpansionLoader{
    public class ModMain:MelonMod{
        public static Dictionary<string,SC2Tower>TowerTypes=new Dictionary<string,SC2Tower>();
		public static Dictionary<string,SC2Tower>HeroTypes=new Dictionary<string,SC2Tower>();
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
        public override void OnInitializeMelon(){
            mllog=LoggerInstance;
            BundleDir=MelonEnvironment.UserDataDirectory+"/SC2ExpansionBundles/";
            BundleDir=BundleDir.Replace('\\','/');
			if(!Directory.Exists(BundleDir)){
            	Directory.CreateDirectory(BundleDir);
			}
			List<SC2Tower>towerList=new();
			foreach(MelonMod mod in RegisteredMelons){
                if(mod.Info.Name.StartsWith("SC2Expansion")){
                    Assembly assembly=mod.MelonAssembly.Assembly;
					foreach(string bundle in assembly.GetManifestResourceNames()){
						try{
							if(bundle.EndsWith(".bundle")){
								Stream stream=assembly.GetManifestResourceStream(bundle);
								byte[]bytes=new byte[stream.Length];
								stream.Read(bytes);
								File.WriteAllBytes(BundleDir+bundle.Split('.')[2],bytes);
							}
						}catch(Exception error){
							PrintError(error,"Failed to write "+bundle);
						}
					}
					foreach(Type type in assembly.GetTypes()){
						try{
							SC2Tower tower=(SC2Tower)Activator.CreateInstance(type);
							if(tower.Name!=""){
								towerList.Add(tower);
								if(tower.HasBundle){
									tower.LoadedBundle=UnityEngine.AssetBundle.LoadFromFileAsync(BundleDir+tower.Name.ToLower()).assetBundle;
								}
								if(tower.Hero){
									HeroTypes.Add(tower.Name,tower);
								}
							}
						}catch{}
					}
				}
			}
			//i really cannot think of any better way to sort a this, orderby from a dictionary itself fucks it over
			if(towerList.Count()>0){
				towerList=towerList.OrderBy(a=>a.Order).ToList();
				foreach(SC2Tower tower in towerList){
					TowerTypes.Add(tower.Name,tower);
				}
			}
		}
		public static void PrintError(Exception exception,string message=null){
			if(message!=null){
				Log(message);
			}
            string error=exception.Message;
			error+="\n"+exception.TargetSite;
            error+="\n"+exception.StackTrace;
            Log(error,"error");
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
					PrintError(error,"Failed to load "+Asset+" from "+Bundle.name);
                    try{
                        Log("Attempting to get available assets");
                        foreach(string asset in Bundle.GetAllAssetNames()){
                            Log(asset);
                        }
                    }catch{
                        Log("Bundle is null");
                    }
                    return null;
                }
            }
        }
        public static void PlaySound(string name){
            Game.instance.audioFactory.PlaySoundFromUnity(null,name,"FX",1,1);
        }
        public static void PlayAnimation(Animator animator,string anim,float duration=0.2f){
        	animator.CrossFade(anim,duration,0,0);
        }
    }
}