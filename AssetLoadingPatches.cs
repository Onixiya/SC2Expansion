namespace SC2ExpansionLoader{
    public class AssetLoading{
        [HarmonyPatch(typeof(Factory.__c__DisplayClass21_0),nameof(Factory.__c__DisplayClass21_0._CreateAsync_b__0))]
        public class FactoryCreateAsyncPatch{
            public static bool Prefix(ref UnityDisplayNode prototype,ref Factory.__c__DisplayClass21_0 __instance){
                string guid=__instance.objectId.guidRef;
                string tower=guid.Split('-')[0];
                if(TowerTypes.ContainsKey(tower)){
                    SC2Tower sc2Tower=TowerTypes[tower];
                    GameObject gObj;
                    if(!AssetPool.PrefabPool.ContainsKey(guid)){
                        gObj=LoadAsset<GameObject>(guid,sc2Tower.Bundle);
                        gObj.hideFlags=HideFlags.DontUnloadUnusedAsset;
                        AssetPool.PrefabPool.Add(guid,gObj);
                    }
                    gObj=uObject.Instantiate(AssetPool.PrefabPool[guid],__instance.__4__this.DisplayRoot);
                    gObj.transform.position=new(0,9999999);
                    prototype=gObj.AddComponent<UnityDisplayNode>();
                    if(sc2Tower.Components.ContainsKey(guid)){
                        gObj.AddComponent(sc2Tower.Components[guid]);
                    }
                    __instance.__4__this.active.Add(prototype);
                    __instance.onComplete.Invoke(prototype);
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(SpriteAtlas),nameof(SpriteAtlas.GetSprite))]
        public class SpriteAtlasGetSpritePatch{
            public static bool Prefix(string name,ref Sprite __result){
                string tower=name.Split('-')[0];
                if(TowerTypes.ContainsKey(tower)){
                    if(!AssetPool.SpritePool.ContainsKey(name)){
                        Sprite sprite=LoadAsset<Sprite>(name,TowerTypes[tower].Bundle);
                        sprite.hideFlags=HideFlags.DontUnloadUnusedAsset;
                        AssetPool.SpritePool.Add(name,sprite);
                    }
                    __result=Sprite.Instantiate(AssetPool.SpritePool[name]);
                    return false;
                }
                return true;
            }
        }
        //https://github.com/gurrenm3/BTD-Mod-Helper/blob/master/BloonsTD6%20Mod%20Helper/Patches/Resources/AudioFactory_Start.cs
        [HarmonyPatch(typeof(AudioFactory),nameof(AudioFactory.Start))]
        public class AudioFactoryStart{
            public static void Postfix(AudioFactory __instance){
                Audio=__instance;
                AssetPool=Audio.gameObject.AddComponent<AssetPoolBehaviour>();
                foreach(SC2Tower tower in TowerTypes.Values){
                    try{
                        if(tower.HasBundle){
                            foreach(uObject asset in tower.Bundle.LoadAllAssetsAsync<AudioClip>().allAssets){
                                AudioClip clip=asset.Cast<AudioClip>();
                                clip.hideFlags=HideFlags.DontUnloadUnusedAsset;
                                AssetPool.AudioPool.Add(clip);
                                __instance.audioClipHandles.Add(new(clip.name),Addressables.Instance.ResourceManager.CreateCompletedOperation(clip,""));
                            }
                        }
                    }catch(Exception ex){
                        PrintError(ex,"Failed to load "+tower.Name+" audio clips");
                    }
                }
                Log("Audio files loaded");
            }
        }
        [HarmonyPatch(typeof(AudioFactory),nameof(AudioFactory.PlaySoundFromUnity))]
        public class AudioFactoryExecuteTask{
            public static void Prefix(ref AudioFactory __instance){
                if(AssetPool.AudioPool.Count>0&&!__instance.audioClipHandles.ContainsKey(new(AssetPool.AudioPool._items[0].name))){
                    foreach(AudioClip clip in AssetPool.AudioPool){
                        __instance.audioClipHandles.Add(new(clip.name),Addressables.Instance.ResourceManager.CreateCompletedOperation(clip,""));
                    }
                }
            }
        }
    }
}