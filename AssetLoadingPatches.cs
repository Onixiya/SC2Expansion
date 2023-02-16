namespace SC2ExpansionLoader{
    public class PrefabSpriteLoading{
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
                        Texture2D texture=LoadAsset<Texture2D>(name,TowerTypes[towerName].LoadedBundle).Cast<Texture2D>();
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

        [HarmonyPatch(typeof(AudioFactory),"Start")]
        public class AudioFactoryStart_Patch{
            [HarmonyPostfix]
            public static void Postfix(AudioFactory __instance){
                foreach(string bundlePath in Directory.GetFiles(BundleDir)){
                    if(bundlePath.EndsWith("clips")){
                        try{
                            foreach(uObject asset in AssetBundle.LoadFromFileAsync(bundlePath).assetBundle.LoadAllAssetsAsync<AudioClip>().allAssets){
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
    }
}