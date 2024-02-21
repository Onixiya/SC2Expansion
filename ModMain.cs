[assembly:MelonGame("Ninja Kiwi","BloonsTD6")]
[assembly:MelonInfo(typeof(ModMain),ModHelperData.Name,ModHelperData.Version,"Silentstorm")]
[assembly:MelonOptionalDependencies("SC2ExpansionLoader")]
namespace SC2ExpansionLoader{
    public class ModMain:MelonMod{
        public static readonly Dictionary<string,SC2Tower>TowerTypes=new Dictionary<string,SC2Tower>();
		public static Dictionary<string,SC2Tower>HeroTypes=new Dictionary<string,SC2Tower>();
        public static AbilityModel BlankAbilityModel;
        private static MelonLogger.Instance _mllog;
        public static string BundleDir;
        public static AttackModel CreateTowerAttackModel;
        public static GameModel gameModel;
        public static LocalizationManager LocManager;
        public static void Log(object thing,string type="msg"){
            switch(type){
                case"msg":
	                _mllog.Msg(thing);
                    break;
                case"warn":
	                _mllog.Warning(thing);
                    break;
                 case"error":
	                 _mllog.Error(thing);
                    break;
            }
        }
        public override void OnInitializeMelon(){
	        _mllog=LoggerInstance;
            BundleDir=MelonEnvironment.UserDataDirectory+"/SC2ExpansionBundles/";
            BundleDir=BundleDir.Replace('\\','/');
			if(!Directory.Exists(BundleDir)){
            	Directory.CreateDirectory(BundleDir);
			}
			List<SC2Tower>towerList=new();
			foreach(MelonMod mod in RegisteredMelons.Where(a=>a.OptionalDependencies!=null&&a.OptionalDependencies.AssemblyNames.Contains("SC2ExpansionLoader"))){
                Assembly assembly=mod.MelonAssembly.Assembly;
				string[]resources=assembly.GetManifestResourceNames();
				if(resources.Any()){
					foreach(string bundle in resources){
						try{
							Log(bundle);
							if(!bundle.EndsWith(".bundle"))continue;
							Stream stream=assembly.GetManifestResourceStream(bundle);
							byte[]bytes=new byte[stream.Length];
							stream.Read(bytes);
							File.WriteAllBytes(BundleDir+bundle.Split('.')[2],bytes);
						}catch(Exception error){
							PrintError(error,"Failed to write "+bundle);
						}
					}
				}
				foreach(Type type in assembly.GetTypes()){
					try{
						SC2Tower tower=(SC2Tower)Activator.CreateInstance(type);
						if(tower.Name=="")continue;
						if(tower.Hero){
							//HeroTypes.Add(tower.Name,tower);
							Log("Custom heroes are currently not supported due to a bug, "+tower.Name+" will not be loaded");
							continue;
						}
						towerList.Add(tower);
						if(tower.HasBundle){
							tower.LoadedBundle=UnityEngine.AssetBundle.LoadFromFileAsync(BundleDir+tower.Name).assetBundle;
						}
					}catch{}
				}
			}
			//i really cannot think of any better way to sort this, orderby from a dictionary itself fucks it over
			if(towerList.Any()){
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
		public static void ModelArrayLoop(string type,Model[]array){
			Log("Elements in "+type+" are: ");
			foreach(Model model in array){
				Log(model.name+" "+model.GetIl2CppType().FullName);
			}
		}
        public static T LoadAsset<T>(string assetToLoad,AssetBundle bundle)where T:uObject{
            try{
                return bundle.LoadAssetAsync(assetToLoad,Il2CppType.Of<T>()).asset.Cast<T>();
            }catch(Exception error){
				PrintError(error,"Failed to load "+assetToLoad);
                try{
                    Log("Attempting to get available assets");
                    foreach(string asset in bundle.GetAllAssetNames()){
                        Log(asset);
                    }
                }catch{
                    Log("Bundle is null");
                }
                return null;
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