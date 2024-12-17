[assembly:MelonGame("Ninja Kiwi","BloonsTD6")]
[assembly:MelonInfo(typeof(ModMain),SC2ExpansionLoader.Data.ModHelperData.Name,SC2ExpansionLoader.Data.ModHelperData.Version,"Silentstorm")]
namespace SC2ExpansionLoader{
    public class ModMain:MelonMod{
        public static readonly Dictionary<string,SC2Tower>TowerTypes=new();
        private static AbilityModel _blankAbilityModel;
        public static AbilityModel BlankAbilityModel{
            get{
                return _blankAbilityModel.Clone<AbilityModel>();
            }
            set{
                _blankAbilityModel=value;
            }
        }
        private static MelonLogger.Instance _mllog;
        private static AttackModel _createTowerAttackModel;
        public static AttackModel CreateTowerAttackModel{
            get{
                return _createTowerAttackModel.Clone<AttackModel>();
            }
            set{
                _createTowerAttackModel=value;
            }
        }
        public static GameModel gameModel;
        public static LocalizationManager LocManager;
		public static Il2CppStructArray<AreaType>FlyingAreaTypes;
        public static AudioFactory Audio;
        public static AssetPoolBehaviour AssetPool;
        private static CreateSoundOnSelectedModel _selectedSoundModel;
        public static CreateSoundOnSelectedModel SelectedSoundModel{
            get{
                return _selectedSoundModel.Clone<CreateSoundOnSelectedModel>();
            }
            set{
                _selectedSoundModel=value;
            }
        }
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
        public override void OnLateInitializeMelon(){
	        _mllog=LoggerInstance;
			FlyingAreaTypes=new(new AreaType[4]{0,(AreaType)1,(AreaType)2,(AreaType)4});
			List<SC2Tower>towerList=new();
            foreach(MelonMod mod in RegisteredMelons.Where(a=>a.Info.SystemType.Name=="SC2ModMain")){
			//foreach(MelonMod mod in RegisteredMelons.Where(a=>a.OptionalDependencies!=null&&a.OptionalDependencies.AssemblyNames.Contains("SC2ExpansionLoader"))){
                Assembly assembly=mod.MelonAssembly.Assembly;
				foreach(Type type in assembly.GetTypes().Where(a=>a.BaseType==typeof(SC2Tower))){
					try{
                        SC2Tower tower=(SC2Tower)Activator.CreateInstance(type);
						if(tower.Name==""){
                            continue;
                        }
                        if(tower.HasBundle){
                            tower.Bundle=AssetBundle.LoadFromMemory(GetBytesFromStream(assembly.GetManifestResourceStream(
                                assembly.GetManifestResourceNames().First(a=>a.Contains(tower.BundleName)))));
                        }
						towerList.Add(tower);
					}catch(Exception ex){
                        PrintError(ex,"Failed to process "+mod.Info.Name);
                    }
				}
			}
			//i really cannot think of any better way to sort this,orderby from a dictionary itself fucks it over
			if(towerList.Any()){
				towerList=towerList.OrderBy(a=>a.Order).ToList();
				foreach(SC2Tower tower in towerList){
					TowerTypes.Add(tower.Name,tower);
				}
			}
		}
        public static void SetSounds(TowerModel model,string id,bool place,bool select,bool upgrade,bool sameSound){
            if(place){
                CreateSoundOnTowerPlaceModel csontpm=model.behaviors.GetModel<CreateSoundOnTowerPlaceModel>();
                if(sameSound){
                    string sound=id+"-"+new System.Random().Next(1,10);
                    csontpm.sound1=new(sound,new(sound));
                }else{
                    csontpm.sound1=new(id+"-Birth",new(id+"-Birth"));
                }
                csontpm.sound2=csontpm.sound1;
                csontpm.heroSound1=csontpm.sound1;
                csontpm.heroSound2=csontpm.sound1;
                csontpm.waterSound1=csontpm.sound1;
                csontpm.waterSound2=csontpm.sound1;
            }
            if(select){
                CreateSoundOnSelectedModel csosm=model.behaviors.GetModel<CreateSoundOnSelectedModel>();
                if(sameSound){
                    csosm.sound1=new(id+"-1",new(id+"-1"));
                    csosm.sound2=new(id+"-2",new(id+"-2"));
                    csosm.sound3=new(id+"-3",new(id+"-3"));
                    csosm.sound4=new(id+"-4",new(id+"-4"));
                    csosm.sound5=new(id+"-5",new(id+"-5"));
                    csosm.sound6=new(id+"-6",new(id+"-6"));
                    csosm.altSound1=new(id+"-7",new(id+"-7"));
                    csosm.altSound2=new(id+"-8",new(id+"-8"));
                }else{
                    csosm.sound1=new(id+"-Select1",new(id+"-Select1"));
                    csosm.sound2=new(id+"-Select2",new(id+"-Select2"));
                    csosm.sound3=new(id+"-Select3",new(id+"-Select3"));
                    csosm.sound4=new(id+"-Select4",new(id+"-Select4"));
                    csosm.sound5=new(id+"-Select5",new(id+"-Select5"));
                    csosm.sound6=new(id+"-Select6",new(id+"-Select6"));
                    csosm.altSound1=new(id+"-Select7",new(id+"-Select7"));
                    csosm.altSound2=new(id+"-Select8",new(id+"-Select8"));
                }
            }
            if(upgrade){
                CreateSoundOnUpgradeModel csoum=model.behaviors.GetModel<CreateSoundOnUpgradeModel>();
                if(sameSound){
                    csoum.sound=new(id+"-1",new(id+"-1"));
                    csoum.sound1=new(id+"-2",new(id+"-2"));
                    csoum.sound2=new(id+"-3",new(id+"-3"));
                    csoum.sound3=new(id+"-4",new(id+"-4"));
                    csoum.sound4=new(id+"-5",new(id+"-5"));
                    csoum.sound5=new(id+"-6",new(id+"-6"));
                    csoum.sound6=new(id+"-7",new(id+"-7"));
                    csoum.sound7=new(id+"-8",new(id+"-8"));
                    csoum.sound8=new(id+"-9",new(id+"-9"));
                }else{
                    csoum.sound=new(id+"-Upgrade1",new(id+"-Upgrade1"));
                    csoum.sound1=new(id+"-Upgrade2",new(id+"-Upgrade2"));
                    csoum.sound2=new(id+"-Upgrade3",new(id+"-Upgrade3"));
                    csoum.sound3=new(id+"-Upgrade4",new(id+"-Upgrade4"));
                    csoum.sound4=new(id+"-Upgrade5",new(id+"-Upgrade5"));
                    csoum.sound5=new(id+"-Upgrade6",new(id+"-Upgrade6"));
                    csoum.sound6=new(id+"-Upgrade7",new(id+"-Upgrade7"));
                    csoum.sound7=new(id+"-Upgrade8",new(id+"-Upgrade8"));
                    csoum.sound8=new(id+"-Upgrade9",new(id+"-Upgrade9"));
                }
            }
        }
        public static byte[]GetBytesFromStream(Stream stream){
            byte[]bytes=new byte[stream.Length];
            stream.Read(bytes);
            stream.Dispose();
            return bytes;
        }
		public static void PrintError(Exception exception,string message=null){
			if(message!=null){
				Log(message);
			}
            string error=exception.Message;
			error+="\n"+exception.TargetSite;
            error+="\n"+exception.StackTrace;
			error+="\n"+exception.Source;
            Log(error,"error");
		}
		public static void ModelArrayLoop(string type,Model[]array){
			Log("Elements in "+type+" are: ");
			foreach(Model model in array){
				Log(model.name+" "+model.GetIl2CppType().FullName);
			}
		}
        [RegisterTypeInIl2Cpp]
        public class AssetPoolBehaviour:MonoBehaviour{
            public AssetPoolBehaviour(IntPtr ptr):base(ptr){}
            public Il2CppSystem.Collections.Generic.Dictionary<string,GameObject>PrefabPool=new();
            public Il2CppSystem.Collections.Generic.Dictionary<string,Sprite>SpritePool=new();
            public Il2CppSystem.Collections.Generic.List<AudioClip>AudioPool=new();
        }
        public static T LoadAsset<T>(string assetToLoad,AssetBundle bundle)where T:uObject{
            try{
                return bundle.LoadAsset<T>(assetToLoad);
            }catch(Exception error){
				PrintError(error,"Failed to load "+assetToLoad);
                try{
                    Log("Attempting to get available assets");
                    foreach(string asset in bundle.GetAllAssetNames()){
                        Log(asset);
                        if(assetToLoad==asset){
                            Log("Asset is in the bundle");
                        }
                    }
                }catch{
                    Log("Bundle is null");
                }
                return null;
            }
        }
        public static void PlayAnimation(Animator animator,string anim,float duration=0.2f){
        	animator.CrossFade(anim,duration,0,0);
        }
		public static void ConfigureAnimationBaker<T>(Il2CppSystem.Collections.Generic.List<T>list,int index,int animationIndex,int priority,AnimationClip animationClip,
			InterruptBehaviour interruptBehaviour=InterruptBehaviour.SameOrHigher)where T:AnimationBakerStateConfig{
			var animBaker=list.ToArray()[index];
			animBaker.animationIndex=animationIndex;
			animBaker.priority=priority;
			animBaker.animationClip=animationClip;
			animBaker.interruptibleBehaviour=interruptBehaviour;
		}
		public static unsafe NativeHook<T>CreateNativeHook<T>(Type type,string methodName,ref T patchDelegate,Type[]generics=null,Type[]parameters=null)where T:Delegate{
			if(parameters==null){
				parameters=Array.Empty<Type>();
			}
			NativeHook<T>hook;
			try{
				if(generics==null){
					generics=Array.Empty<Type>();
					hook=new(*(IntPtr*)(IntPtr)Il2CppInteropUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(type.
						GetMethod(methodName,generics.Length,parameters)).GetValue(null),Marshal.GetFunctionPointerForDelegate(patchDelegate));
				}else{
					hook=new(*(IntPtr*)(IntPtr)Il2CppInteropUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(type.
						GetMethod(methodName,generics.Length,parameters).MakeGenericMethod(generics)).GetValue(null),
						Marshal.GetFunctionPointerForDelegate(patchDelegate));
				}
			}catch{
				Log("Native hook failed, method not found, ICall maybe?");
				Log("Available methods are ");
				int match=0;
				foreach(MethodInfo method in type.GetMethods()){
					Log("{---=MethodInfo=---}");
					Log("Name: "+method.Name+", generic: "+method.IsGenericMethod);
					if(methodName==method.Name){
						match+=1;
						Log("Name matches");
					}
					if(method.GetGenericArguments()==generics){
						match+=1;
						Log("Potential generics match");
					}
					if(method.IsGenericMethod){
						Log("-=Generic parameters=-");
						foreach(Type generic in method.GetGenericArguments()){
							if(generic!=null&&generic.FullName!=null){
								Log(generic.FullName);
							}else{
								Log("=====GENERIC NULL=====");
							}
						}
					}
					if(method.GetParameters().Length==parameters.Length){
						match+=1;
						Log("Potential parameter count match");
					}
					if(method.GetParameters().Length>0){
						Log("-=Method parameters=-");
						foreach(var thing in method.GetParameters()){
							Log(thing.Name+" "+thing.ParameterType.FullName);
						}
					}
					if(match==3){
						Log("================Method potentially matches!================");
					}
					match=0;
				}
				return null;
			}
			hook.Attach();
			return hook;
		}
    }
}