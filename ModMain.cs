[assembly:MelonGame("Ninja Kiwi","BloonsTD6")]
[assembly:MelonInfo(typeof(ModMain),SC2Expansion.Data.ModHelperData.Name,SC2Expansion.Data.ModHelperData.Version,"Silentstorm")]
namespace SC2Expansion{
    public class ModMain:MelonMod{
        public static Dictionary<string,SC2Tower>TowerTypes=new();
        public static Dictionary<string,SC2Map>MapTypes=new();
        public static Dictionary<string,Il2CppSystem.Type>ComponentList=new();
        public static AbilityModel BlankAbilityModel;
        private static MelonLogger.Instance _mllog;
        public static AttackModel CreateTowerAttackModel;
        public static GameModel gameModel;
        public static LocalizationManager LocManager;
		public static Il2CppStructArray<AreaType>FlyingAreaTypes;
        public static AudioFactory Audio;
        public static AssetPoolBehaviour AssetPool;
        public static CreateSoundOnSelectedModel SelectedSoundModel;
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
                Assembly assembly=mod.MelonAssembly.Assembly;
				foreach(Type type in assembly.GetTypes().Where(a=>a.BaseType.BaseType==typeof(SC2Content))){
					try{
                        SC2Content content=(SC2Content)Activator.CreateInstance(type);
						if(content.Name==""){
                            continue;
                        }
                        if(content.HasBundle){
                            content.Bundle=AssetBundle.LoadFromMemory(GetBytesFromStream(assembly.GetManifestResourceStream(
                                assembly.GetManifestResourceNames().First(a=>a.Contains(content.BundleName)))));
                        }
                        switch(content.GetType().BaseType){
                            case Type tower when tower==typeof(SC2Tower):
                                towerList.Add((SC2Tower)content);
                                break;
                            case Type map when map==typeof(SC2Map):
                                MapTypes.Add(content.Name,(SC2Map)content);
                                break;
                        }
					}catch(Exception ex){
                        foreach(string resource in assembly.GetManifestResourceNames()){
                            Log(resource);
                        }
                        PrintError(ex,"Failed to process "+mod.Info.Name);
                        break;
                    }
				}
			}
			//i really cannot think of any better way to sort this,orderby from a dictionary itself fucks it over
			if(towerList.Any()){
				towerList=towerList.OrderBy(a=>a.Order).ToList();
				foreach(SC2Tower tower in towerList){
					TowerTypes.Add(tower.Name,tower);
                    foreach(KeyValuePair<string,Il2CppSystem.Type>keyValue in tower.Components){
                        ComponentList.Add(keyValue.Key,keyValue.Value);
                    }
				}
			}
		}
        public static void SetUpgradeSounds(CreateSoundOnUpgradeModel model,string id){
            model.sound=new(id+"Birth",new(id+"Birth"));
            model.sound1=model.sound;
            model.sound2=model.sound;
            model.sound3=model.sound;
            model.sound4=model.sound;
            model.sound5=model.sound;
            model.sound6=model.sound;
            model.sound7=model.sound;
            model.sound8=model.sound;
        }
        public static void SetSounds(TowerModel model,string id,bool place,bool select,bool upgrade,bool sameSound){
            if(place){
                CreateSoundOnTowerPlaceModel csontpm=model.behaviors.GetModel<CreateSoundOnTowerPlaceModel>();
                string sound=id+new System.Random().Next(1,10);
                if(model.behaviors.HasModel<HeroModel>()){
                    if(sameSound){
                        csontpm.heroSound1=new(sound,new(sound));
                        csontpm.heroSound2=csontpm.heroSound2;
                    }else{
                        csontpm.heroSound1=new(id+"Birth",new(id+"Birth"));
                        csontpm.heroSound2=csontpm.heroSound1;
                    }
                }else{
                    if(sameSound){
                        csontpm.sound1=new(sound,new(sound));
                    }else{
                        csontpm.sound1=new(id+"Birth",new(id+"Birth"));
                    }
                    csontpm.sound2=csontpm.sound1;
                    csontpm.waterSound1=csontpm.sound1;
                    csontpm.waterSound2=csontpm.sound1;
                }
            }
            if(select){
                CreateSoundOnSelectedModel csosm=model.behaviors.GetModel<CreateSoundOnSelectedModel>();
                if(sameSound){
                    csosm.sound1=new(id+"1",new(id+"1"));
                    csosm.sound2=new(id+"2",new(id+"2"));
                    csosm.sound3=new(id+"3",new(id+"3"));
                    csosm.sound4=new(id+"4",new(id+"4"));
                    csosm.sound5=new(id+"5",new(id+"5"));
                    csosm.sound6=new(id+"6",new(id+"6"));
                    csosm.altSound1=new(id+"7",new(id+"7"));
                    csosm.altSound2=new(id+"8",new(id+"8"));
                }else{
                    csosm.sound1=new(id+"Select1",new(id+"Select1"));
                    csosm.sound2=new(id+"Select2",new(id+"Select2"));
                    csosm.sound3=new(id+"Select3",new(id+"Select3"));
                    csosm.sound4=new(id+"Select4",new(id+"Select4"));
                    csosm.sound5=new(id+"Select5",new(id+"Select5"));
                    csosm.sound6=new(id+"Select6",new(id+"Select6"));
                    csosm.altSound1=new(id+"Select7",new(id+"Select7"));
                    csosm.altSound2=new(id+"Select8",new(id+"Select8"));
                }
            }
            if(upgrade){
                CreateSoundOnUpgradeModel csoum=model.behaviors.GetModel<CreateSoundOnUpgradeModel>();
                if(sameSound){
                    csoum.sound=new(id+"1",new(id+"1"));
                    csoum.sound1=new(id+"2",new(id+"2"));
                    csoum.sound2=new(id+"3",new(id+"3"));
                    csoum.sound3=new(id+"4",new(id+"4"));
                    csoum.sound4=new(id+"5",new(id+"5"));
                    csoum.sound5=new(id+"6",new(id+"6"));
                    csoum.sound6=new(id+"7",new(id+"7"));
                    csoum.sound7=new(id+"8",new(id+"8"));
                    csoum.sound8=new(id+"9",new(id+"9"));
                }else{
                    csoum.sound=new(id+"Upgrade1",new(id+"Upgrade1"));
                    csoum.sound1=new(id+"Upgrade2",new(id+"Upgrade2"));
                    csoum.sound2=new(id+"Upgrade3",new(id+"Upgrade3"));
                    csoum.sound3=new(id+"Upgrade4",new(id+"Upgrade4"));
                    csoum.sound4=new(id+"Upgrade5",new(id+"Upgrade5"));
                    csoum.sound5=new(id+"Upgrade6",new(id+"Upgrade6"));
                    csoum.sound6=new(id+"Upgrade7",new(id+"Upgrade7"));
                    csoum.sound7=new(id+"Upgrade8",new(id+"Upgrade8"));
                    csoum.sound8=new(id+"Upgrade9",new(id+"Upgrade9"));
                }
            }
        }
        public static byte[]GetBytesFromStream(Stream stream){
            byte[]bytes=new byte[stream.Length];
            stream.Read(bytes);
            stream.Dispose();
            return bytes;
        }
        public static void StackWalk(){
            Log("~-----<=====[ Native Stack Walk ]=====>-----~");
            foreach(NativeStackWalk.NativeStackFrame function in NativeStackWalk.GetNativeStackFrames()){
                if(function.Function!=null){
                    Log("Function: "+function.Function);
                }else{
                    Log("Function null");
                }
            }
        }
		public static void PrintError(Exception exception,string message=null,bool stackWalk=false){
			if(message!=null){
				Log(message);
			}
            if(stackWalk){
                StackWalk();
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
						foreach(ParameterInfo funcParameters in method.GetParameters()){
							Log(funcParameters.Name+" "+funcParameters.ParameterType.FullName);
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