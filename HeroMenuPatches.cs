namespace SC2ExpansionLoader.HeroPatches{
	[HarmonyPatch(typeof(CosmeticHelper),nameof(CosmeticHelper.ApplyTowerSkinToTowerModel))]
	public class CosmeticHelperApplyTowerSkinToTowerModel_Patch{
		public static bool Prefix(TowerModel towerModel){
			foreach(string key in TowerTypes.Keys){
                if(towerModel.baseId.Contains(key)){
				    return false;
                }
            }
		    return true;
		}
	}
    [HarmonyPatch(typeof(Helpers),nameof(Helpers.GetHeroMmCost))]
    public class HelpersGetHeroMmCost{
        public static void Prefix(ref string baseHeroId,ref string skinName){
            if(TowerTypes.ContainsKey(baseHeroId)){
                baseHeroId="Quincy";
                skinName=null;
            }
        }
    }
    [HarmonyPatch(typeof(HeroUpgradeDetails),nameof(HeroUpgradeDetails.SortHerosByUnlockLevel))]
    public class HeroUpgradeDetailsSortHerosByUnlockLevel{
        public static void Postfix(ref HeroUpgradeDetails __instance){
            foreach(SC2Tower tower in TowerTypes.Values.Where(a=>a.Hero)){
                __instance.selectedHeroes.Add(tower.HeroDetails);
            }
        }
    }
}