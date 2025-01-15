namespace SC2Expansion{
	public abstract class SC2Map:SC2Content{
        public abstract MapDetails Details();
		/*[HarmonyPatch(typeof(MainMenu),nameof(MainMenu.Start))]
		public class mapselectscreenloadmap_patch{
			public static void Postfix(){
				InGameData.Editable.selectedDifficulty="Easy";
				InGameData.Editable.selectedMode="Sandbox";
				InGameData.Editable.selectedMap="Tutorial";
				UI.instance.LoadGame();
				Log("mainmenu start "+InGameData.Editable.selectedMap);
			}
		}*/
        [HarmonyPatch(typeof(Il2CppAssets.Scripts.Simulation.Track.Map),nameof(Il2CppAssets.Scripts.Simulation.Track.Map.Initialise))]
        public class mapinitialize_patch{
            public static void Prefix(ref Model modelToUse){
                //modelToUse.WriteIl2CppObjToFile(MelonEnvironment.ModsDirectory+"\\map.json");
                if(MapTypes.ContainsKey(modelToUse.name)){
                    modelToUse=MapTypes[modelToUse.name].Model;
                    gameModel.map=modelToUse.Cast<MapModel>();
                }
            }
        }
        public abstract MapModel Model{get;}
        public virtual bool VisualizeMap=>false;
	}
}
//https://github.com/Void-n-Null/QuickGame/blob/main/QuickGame.cs