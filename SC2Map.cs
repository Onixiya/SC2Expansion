using Il2CppAssets.Scripts.Models.ContentBrowser;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Map.Gizmos;
using Il2CppAssets.Scripts.Models.Map.Spawners;
using Il2CppAssets.Scripts.Models.Map.Triggers;
using Il2CppAssets.Scripts.Models.SimulationBehaviors;
using Il2CppAssets.Scripts.Simulation.SimulationBehaviors;
using Il2CppAssets.Scripts.Simulation.Track;
using Il2CppAssets.Scripts.Unity.Map.Gizmos;
using Il2CppAssets.Scripts.Unity.Map.Triggers;
using Il2CppAssets.Scripts.Unity.UI_New.Nexus;
using UnityEngine.UIElements.Internal;

namespace SC2Expansion{
	public abstract class SC2Map:SC2Content{
        public abstract MapDetails Details();
		[HarmonyPatch(typeof(MainMenu),nameof(MainMenu.Start))]
		public class mapselectscreenloadmap_patch{
			public static void Postfix(){
				InGameData.Editable.selectedDifficulty="Easy";
				InGameData.Editable.selectedMode="Sandbox";
				InGameData.Editable.selectedMap="SC2ZergMap01";//SC2ZergMap01 //Tutorial
			    UI.instance.LoadGame();
				Log("mainmenu start "+InGameData.Editable.selectedMap);
			}
		}
        /*[HarmonyPatch(typeof(Il2CppAssets.Scripts.Simulation.Track.Map),nameof(Il2CppAssets.Scripts.Simulation.Track.Map.Initialise))]
        public class mapinitialize_patch{
            public static void Prefix(ref Model modelToUse){
                //modelToUse.WriteIl2CppObjToFile(MelonEnvironment.ModsDirectory+"\\map.json");
       but          if(MapTypes.ContainsKey(modelToUse.name)){
                    modelToUse=MapTypes[modelToUse.name].Model;
                    gameModel.map=modelToUse.Cast<MapModel>();
                }
            }
        }*/
        [HarmonyPatch(typeof(Il2CppAssets.Scripts.Simulation.Track.Splitter),nameof(Il2CppAssets.Scripts.Simulation.Track.Splitter.SpawnRout))]
        public class splitterspawnrout_patch{
            public static void Prefix(Il2CppAssets.Scripts.Simulation.Track.Splitter __instance,int emissionIndex,int roundNumber){
                Log("spawnrout");
                Log(__instance.GetIl2CppType().FullName+" "+emissionIndex+" "+roundNumber+" "+__instance.activePaths.Count);
            }
        }
        [HarmonyPatch(typeof(Il2CppAssets.Scripts.Simulation.Track.Splitter),nameof(Il2CppAssets.Scripts.Simulation.Track.Splitter.UpdateActivePaths))]
        public class splitterupdateactivepaths_patch{
            public static bool Prefix(Il2CppAssets.Scripts.Simulation.Track.Splitter __instance){
                List<Il2CppAssets.Scripts.Simulation.Track.Path>paths=new();
                Log(__instance.paths==null);
                Log(__instance.paths.Count);
                foreach(var path in __instance.paths){
                    Log(path.pointInfos.Count);
                    if(path.isActive){
                        paths.Add(path);
                    }
                }
                __instance.activePaths=paths.ToArray();
                return false;
            }
        }
        public virtual bool VisualizeMap=>false;
        [HarmonyPatch(typeof(MapLoader),nameof(MapLoader.Load))]
        public class MapLoaderLoad_Patch{
            public static bool Prefix(ref MapLoader __instance,CoopDivision coopDivisionType,CustomMapModel customMapModel,ref MapModel __result){
                Il2CppAssets.Scripts.Unity.Map.Map map;
                if(InGame.instance.IsEditingMap){
                    map=__instance.currentMap;
                    map.transform.FindChild("Data").gameObject.SetActive(true);
                }
                MapDetails mapDetails=GameData.Instance.mapSet.GetMapDetails(__instance.currentMapName);
                __instance.paths.Clear();
                __instance.areas.Clear();
                __instance.events.Clear();
                __instance.blockersList.Clear();
                __instance.unityRemoveables.Clear();
                __instance.removeableModels.Clear();
                __instance.coopAreaLayoutList.Clear();
                __instance.regenRemovableModels.Clear();
                __instance.simBehaviorModels.Clear();
                CoopDivision coopMapDivisionType;
                if(mapDetails!=null){
                    __instance.currentMapDifficulty=mapDetails.difficulty;
                    coopMapDivisionType=mapDetails.coopMapDivisionType;
                }else{
                    __instance.currentMapDifficulty=MapDifficulty.Intermediate;
                    coopMapDivisionType=CoopDivision.FREE_FOR_ALL;
                }
                __instance.coopMapDivisionType=coopMapDivisionType;
                __instance.GatherCoopDivisionType(coopDivisionType);
                map=__instance.currentMap;
                Log(map==null);
                foreach(MapUnityAction action in map.GetComponentsInChildren<MapUnityAction>(true)){
                    action.GetOriginalState();
                }
                __instance.GatherMapRemoveablesData(customMapModel);
                __instance.GatherMapBaseArea(customMapModel);
                __instance.GatherMapAreasAndBlockers();
                if(MapTypes.ContainsKey(__instance.currentMapName)){
                    Il2CppAssets.Scripts.Unity.Map.Path pathObj=__instance.currentMap.transform.GetChild(0).GetChild(0).gameObject.AddComponent<Il2CppAssets.Scripts.Unity.Map.Path>();
                    Log(pathObj.name);
                    pathObj.isActive=true;
                    List<PointInfo>points=new();
                    foreach(Transform transform in pathObj.GetComponentsInChildren<Transform>().OrderBy(a=>a.gameObject.name)){
                        points.Add(new(){
                            bloonScale=1,
                            bloonsInvulnerable=false,
                            bloonSpeedMultiplier=1,
                            distance=0,
                            moabScale=1,
                            moabsInvulnerable=false,
                            point=new(transform.localPosition.x,-transform.localPosition.z,0),
                            rotation=0
                        });
                    }
                    PathModel pathModel=new("Path",points.ToArray(),true,false,points[0].point,points[points.Count-1].point,null,null);
                    pathObj.def=pathModel;
                }
                foreach(Il2CppAssets.Scripts.Unity.Map.Path path in map.GetComponentsInChildren<Il2CppAssets.Scripts.Unity.Map.Path>()){
                    Log("path "+path.name);
                    __instance.paths.Add(MapLoader.__c.__9._GatherMapPaths_b__27_0(path));
                }
                __instance.GatherMapCustomPaths(customMapModel);
                MapGizmo[]gizmos=map.GetComponentsInChildren<MapGizmo>();
                if(gizmos.Count()<=0){
                    gizmos=new MapGizmo[0];
                    __instance.gizmos=new(0);
                }else{
                    __instance.gizmos=Il2CppAssets.Scripts.Models.ModelUtils.ToDefArray<MapGizmoModel>(gizmos).ToArray();
                }
                __instance.GatherMapBloonPathSpawners(customMapModel);
                foreach(MapEvent mapEvent in map.GetComponentsInChildren<MapEvent>()){
                    __instance.events.Add(MapLoader.__c.__9._GatherMapEventsTriggersAndActions_b__30_0(mapEvent));
                }
                __instance.SupportLegacyRemoveables(__instance.unityRemoveables,__instance.events);
                foreach(var animBehav in map.GetComponentsInChildren<AnimatorBehaviors>()){
                    animBehav.run(); //taking a guess, unknown method
                }
                MapMods mods=map.GetComponentInChildren<MapMods>();
                float speed=1;
                if(mods!=null){
                    speed=mods.bloonSpeed;
                }
                __instance.mapWideBloonSpeed=speed;
                foreach(var simBehav in map.GetComponentsInChildren<Il2CppAssets.Scripts.Unity.Map.MapSimulationBehavior>()){
                    __instance.simBehaviorModels.Add(simBehav.Def);
                }
                __instance.CheckAndLoadHolidaySkins();
                __instance.CheckDeviceQuality();
                if(Application.isPlaying){
                    goto LABEL_126;
                }
                Transform data=map.transform.FindChild("Data");
                if(!InGame.instance.IsEditingMap){
                    uObject.Destroy(data);
                    goto LABEL_126;
                }
                if(data!=null){
                    data.gameObject.SetActive(false);
                }
                LABEL_126:
                AreaModel[]areasArr=__instance.areas.ToArray();
                BlockerModel[]blockersArr=__instance.blockersList.ToArray();
                CoopAreaLayoutModel[]coopAreaLayoutArr=__instance.coopAreaLayoutList.ToArray();
                PathModel[]pathArr=__instance.paths.ToArray();
                RemoveableModel[]removeableArr=__instance.removeableModels.ToArray();
                RegenRemovableModel[]regenRemoveable=__instance.regenRemovableModels.ToArray();
                MapEventModel[]mapEventArr=__instance.events.ToArray();
                SimulationBehaviorModel[]simBehavArr=__instance.simBehaviorModels.ToArray();
                __result=new(__instance.currentMapName,areasArr,blockersArr,coopAreaLayoutArr,pathArr,removeableArr,__instance.gizmos,regenRemoveable,
                    (int)__instance.currentMapDifficulty,__instance.pathSpawnerModel,mapEventArr,__instance.mapWideBloonSpeed,simBehavArr,
                    __instance.currentMap.overrideBossSpawnDistance);
                return false;
            }
        }
	}
}
//https://github.com/Void-n-Null/QuickGame/blob/main/QuickGame.cs