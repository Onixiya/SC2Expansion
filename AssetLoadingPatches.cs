namespace SC2Expansion{
    public class AssetLoading{
        [HarmonyPatch(typeof(Factory.__c__DisplayClass21_0),nameof(Factory.__c__DisplayClass21_0._CreateAsync_b__0))]
        public class FactoryCreateAsync_Patch{
            public static bool Prefix(ref UnityDisplayNode prototype,ref Factory.__c__DisplayClass21_0 __instance){
                string guid=__instance.objectId.guidRef;
                string tower=guid.Split('-')[0];
                //Log(guid);
                if(TowerTypes.ContainsKey(tower)){
                    SC2Tower sc2Tower=TowerTypes[tower];
                    GameObject gObj;
                    if(!AssetPool.PrefabPool.ContainsKey(guid)){
                        gObj=LoadAsset<GameObject>(guid,sc2Tower.Bundle);
                        gObj.hideFlags=HideFlags.DontUnloadUnusedAsset;
                        AssetPool.PrefabPool.Add(guid,gObj);
                        //Log("pooled");
                    }
                    gObj=uObject.Instantiate(AssetPool.PrefabPool[guid],__instance.__4__this.DisplayRoot);
                    //Log("instantiated");
                    gObj.transform.position=new(0,9999999);
                    prototype=gObj.AddComponent<UnityDisplayNode>();
                    if(ComponentList.ContainsKey(guid)){
                        gObj.AddComponent(ComponentList[guid]);
                        //Log("component added");
                    }
                    __instance.__4__this.active.Add(prototype);
                    __instance.onComplete.Invoke(prototype);
                    //Log("complete");
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(SpriteAtlas),nameof(SpriteAtlas.GetSprite))]
        public class SpriteAtlasGetSprite_Patch{
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
        public class AudioFactoryStart_Patch{
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
        public class AudioFactoryPlaySoundFromUnity_Patch{
            public static void Prefix(ref AudioFactory __instance){
                if(AssetPool.AudioPool.Count>0&&!__instance.audioClipHandles.ContainsKey(new(AssetPool.AudioPool._items[0].name))){
                    foreach(AudioClip clip in AssetPool.AudioPool){
                        __instance.audioClipHandles.Add(new(clip.name),Addressables.Instance.ResourceManager.CreateCompletedOperation(clip,""));
                    }
                }
            }
        }
        [HarmonyPatch(typeof(GameModelUtil.__c__DisplayClass4_0),nameof(GameModelUtil.__c__DisplayClass4_0._LoadGameModelAsync_b__0))]
        public class GameModelUtilLoadGameModelAsync_Patch{
            public static void Postfix(ref GameModel __result){
                gameModel=__result;
                LocManager=LocalizationManager.Instance;
				try{
					//mostly suited for protoss warp things
                    AttackModel attack=gameModel.GetTowerFromId("EngineerMonkey-100").behaviors.GetModelClone<AttackModel>("Spawner");
                    List<Model>createTowerBehav=attack.behaviors.ToList();
                    createTowerBehav.RemoveModel<RotateToTargetModel>();
                    RandomPositionModel rpm=createTowerBehav.GetModel<RandomPositionModel>();
                    rpm.minDistance=70;
                    rpm.maxDistance=90;
                    rpm.idealDistanceWithinTrack=0;
                    rpm.useInverted=false;
                    attack.behaviors=createTowerBehav.ToArray();
                    ProjectileModel proj=attack.weapons[0].projectile;
                    proj.display=new("");
                    Il2CppReferenceArray<Model>projBehav=proj.behaviors;
                    ArriveAtTargetModel arriveAtTargetModel=projBehav.GetModel<ArriveAtTargetModel>();
                    arriveAtTargetModel.expireOnArrival=false;
                    arriveAtTargetModel.altSpeed=400;
                    DisplayModel displayModel=projBehav.GetModel<DisplayModel>();
                    displayModel.delayedReveal=1;
                    displayModel.positionOffset=new(0,0,190);
                    displayModel.display=proj.display;
                    CreateTowerAttackModel=attack;
				}catch(Exception ex){
					PrintError(ex,"Failed to create CreateTowerAttackModel");
				}
				try{
                    AbilityModel ability=gameModel.GetTowerFromId("Quincy 4").behaviors.GetModelClone<AbilityModel>();
                    ability.description="AbilityDescription";
                    ability.displayName="AbilityDisplayName";
                    ability.name="AbilityName";
                    List<Model>behaviors=ability.behaviors.ToList();
                    behaviors.RemoveModel<TurboModel>();
                    behaviors.RemoveModel<CreateEffectOnAbilityModel>();
                    behaviors.RemoveModel<CreateSoundOnAbilityModel>();
                    ability.behaviors=behaviors.ToArray();
                    BlankAbilityModel=ability;
                }catch(Exception ex){
                    PrintError(ex,"Failed to create BlankAbilityModel");
                }
                try{
                    CreateSoundOnSelectedModel csosm=gameModel.GetTowerFromId("Quincy").behaviors.GetModelClone<CreateSoundOnSelectedModel>();
                    csosm.name="SC2Expansion-Select";
                    SelectedSoundModel=csosm;
                }catch(Exception ex){
                    PrintError(ex,"Failed to create SelectedSoundModel");
                }
                List<TowerModel>towers=gameModel.towers.ToList();
                List<TowerDetailsModel>towerSet=gameModel.towerSet.ToList();
                List<UpgradeModel>upgrades=gameModel.upgrades.ToList();
				List<TowerDetailsModel>heroSet=gameModel.heroSet.ToList();
                List<string>towerNames=new();
                bool error=false;
                foreach(SC2Tower tower in TowerTypes.Values){
                    try{
                        towerNames.Add(tower.Name);
                        if(tower.Upgradable){
                            tower.UpgradeModels=tower.GenerateUpgradeModels();
                            upgrades.AddRange(tower.UpgradeModels);
                        }
                        if(tower.AddToShop){
                            towerSet.Add(tower.ShopDetails());
                        }
					    if(tower.Hero){
						    tower.HeroDetails=tower.GenerateHeroDetails();
						    heroSet.Add(tower.HeroDetails);
					    }
					    List<string>towerTypes=TowerType.towers.ToList();
					    towerTypes.Add(tower.Name);
					    TowerType.towers=towerTypes.ToArray();
					    tower.TowerModels=tower.GenerateTowerModels();
                        towers.AddRange(tower.TowerModels);
                    }catch(Exception ex){
                        PrintError(ex,"Failed to add "+tower.Name+" tower");
                        error=true;
                        break;
                    }
                }
                List<MapDetails>maps=GameData.Instance.mapSet.Maps.items.ToList();
                foreach(SC2Map map in MapTypes.Values){
                    try{
                        maps.Add(map.Details());
                    }catch(Exception ex){
                        PrintError(ex,"Failed to add "+map.Name+" map");
                        error=true;
                    }
                }
                //if no exceptions happened, good to add everything
                if(error==false){
                    foreach(SC2Tower tower in TowerTypes.Values){
                        List<string>towerTypes=TowerType.towers.ToList();
					    towerTypes.Add(tower.Name);
					    TowerType.towers=towerTypes.ToArray();
                        if(tower.Hero){
                            //still has a chance of messing up but unlikely if it got this far
                            GameData.Instance.skinsData.AddSkins(new(new[]{tower.HeroSkin()}));
                        }
                    }
                    gameModel.towers=towers.ToArray();
                    gameModel.towerSet=towerSet.ToArray();
                    gameModel.upgrades=upgrades.ToArray();
                    gameModel.heroSet=heroSet.ToArray();
                    GameData.Instance.mapSet.Maps.items=maps.ToArray();
                    Log("All content loaded");
                }
            }
        }
        [HarmonyPatch(typeof(ProfileModel),nameof(ProfileModel.Validate))]
        public class ProfileModelValidate_Patch{
            public static void Prefix(ProfileModel __instance){
				if(gameModel.GetTowerFromId(__instance.primaryHero)==null){
					Log("Selected hero is NULL! Resetting to Quincy to avoid crashing!");
					__instance.primaryHero="Quincy";
				}
			}
            public static void Postfix(ProfileModel __instance){
                foreach(KeyValuePair<string,SC2Tower>sc2Tower in TowerTypes){
                    try{
                        SC2Tower tower=sc2Tower.Value;
                        __instance.unlockedTowers.AddIfNotPresent(tower.Name);
                        if(tower.Upgradable){
                            tower.UpgradeModels??=tower.GenerateUpgradeModels();
                            foreach(UpgradeModel upgrade in tower.UpgradeModels){
								__instance.acquiredUpgrades.AddIfNotPresent(upgrade.name);
							}
                        }
                        __instance.unlockedHeroes.AddIfNotPresent(tower.Name);
                        __instance.seenUnlockedHeroes.AddIfNotPresent(tower.Name);
                        __instance.seenNewHeroNotification.AddIfNotPresent(tower.Name);
                    }catch(Exception error){
                        PrintError(error,"Failed to add "+sc2Tower.Key+" to unlocked towers or upgrades");
                    }
                }
            }
        }
        [HarmonyPatch(typeof(MapLoader._LoadScene_d__19),nameof(MapLoader._LoadScene_d__19.MoveNext))]
		public class MapLoaderLoadScene_Patch{
			public static bool Prefix(ref MapLoader._LoadScene_d__19 __instance,ref bool __result){
                string mapName=__instance.__4__this.currentMapName;
                if(MapTypes.ContainsKey(mapName)){
                    SC2Map map=MapTypes[mapName];
                    if(!SceneManager.GetSceneByName(mapName).IsValid()){
                        SceneManager.LoadScene(map.Bundle.GetAllScenePaths()[0],LoadSceneMode.Additive);
                        __result=true; //returning true makes movenext run again
                        return false; //scenes require a frame or two to be setup
                    }
                    SceneManager.UnloadScene(SceneManager.GetSceneByName("CommonBackgroundUi"));
                    __instance.__4__this.currentMap=SceneManager.GetSceneByName(mapName).GetRootGameObjects()[0].AddComponent<Map>();
                    /*if(map.VisualizeMap){
                        for(int i=0;i<map.Model.paths[0].points.Count;i++){
                            var point=map.Model.paths[0].points[i];
                            Log("1 "+mapObj.gameObject.name);
                            GameObject rootcube=mapObj.transform.GetChild(0).gameObject;
                            GameObject testCube=uObject.Instantiate(rootcube,mapObj.transform);
                            Log(point.point.x+" "+point.point.y+" "+point.point.z);
                            var pos=InGame.instance.GetUIFromWorld(point.point.ToUnityDisplay());
                            testCube.transform.position=new(pos.x,20,pos.y);
                            Log(testCube.transform.position.ToString()+" "+testCube.transform.localPosition.ToString());
                            var text=testCube.transform.GetChild(0).gameObject;
                            var text1=text.AddComponent<NK_TextMeshProUGUI>();
                            text1.enableWordWrapping=false;
                            text1.color=new(0,1,1);
                            text1.text=i.ToString();
                            text1.alignment=TextAlignmentOptions.Center;
                            text1.horizontalAlignment=HorizontalAlignmentOptions.Center;
                            text1.fontSize=1;
                        }
                    }*/
                    __instance.__4__this.currentMapName=mapName;
                    __result=false;
                    return false;
                }
                return true;
			}
		}
    }
}