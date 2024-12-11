namespace SC2ExpansionLoader.HeroPatches{
	[HarmonyPatch(typeof(CosmeticHelper),nameof(CosmeticHelper.ApplyTowerSkinToTowerModel))]
	public class CosmeticHelperApplyTowerSkinToTowerModel_Patch{
		public static bool Prefix(TowerModel towerModel){
            /*
             towers are keyed by identifier and baseid may be different.
             the lag only happens when loading into a game
             so i think this is fine
            */
			foreach(string key in TowerTypes.Keys){
                if(towerModel.baseId.Contains(key)){
				    return false;
                }
            }
		    return true;
		}
	}
}