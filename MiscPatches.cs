namespace SC2Expansion{
    public static class HarmonyPatches{
        [HarmonyPatch(typeof(Btd6Player),nameof(Btd6Player.CheckForNewParagonPipEvent))]
        public class Btd6PlayerCheckForNewParagonPipEvent_Patch{
            public static bool Prefix(){
                return false;
            }
        }
		[HarmonyPatch(typeof(TowerInventory),nameof(TowerInventory.CreatedTower))]
        public class TowerInventoryCreatedTower_Patch{
            public static bool Prefix(TowerInventory __instance,TowerModel def){
                if(!__instance.towerCounts.ContainsKey(def.baseId)){
                    __instance.towerCounts.Add(def.baseId,0);
                }
		        __instance.towerCounts[def.baseId]=__instance.towerCounts[def.baseId]+1;
		        return false;
            }
        }
        [HarmonyPatch(typeof(TowerInventory),nameof(TowerInventory.DestroyedTower))]
        public class TowerInventoryDestroyedTower_Patch{
            public static bool Prefix(ref TowerInventory __instance,ref TowerModel def){
                if(!__instance.towerCounts.ContainsKey(def.baseId)){
                    __instance.towerCounts.Add(def.baseId,0);
                }
		        __instance.towerCounts[def.baseId]=__instance.towerCounts[def.baseId]-1;
		        return false;
            }
        }
        [HarmonyPatch(typeof(FighterMovement),nameof(FighterMovement.Process))]
        public class FighterMovementProcess_Patch{
            public static bool Prefix(ref FighterMovement __instance,int elapsed){
                __instance.timer++;
		        if(__instance.flyoverEngaged==false){
                    __instance.timer=0;
		        }
		        __instance.ApplyMovement(new(0,0),elapsed);
                return false;
            }
        }
        [HarmonyPatch(typeof(SubTowerFilter),nameof(SubTowerFilter.FilterEmission))]
        public class SubTowerFilterFilterEmission_Patch{
            public static bool Prefix(SubTowerFilter __instance,ref bool __result){
                if(__instance.createdSubTowers.Count>=__instance.subTowerFilterModel.maxNumberOfSubTowers){
                    __result=false;
                }else{
                    __result=true;
                }
                return false;
            }
        }
        [HarmonyPatch(typeof(CreateSoundOnUpgrade),nameof(CreateSoundOnUpgrade.OnUpgrade))]
        public class CreateSoundOnUpgradeOnUpgrade_Patch{
            public static bool Prefix(ref CreateSoundOnUpgrade __instance){
                if(TowerTypes.ContainsKey(__instance.tower.towerModel.baseId)){
                    __instance.PlayHeroUpgradeSound();
                    return false;
                }
                return true;
            }
        }
        /*[HarmonyPatch(typeof(Pet),nameof(Pet.Initialise))]
        public class petinit{
            [HarmonyPrefix]
            public static void Prefix(Model modelToUse){
                WanderModel wander=modelToUse.Cast<PetModelUnsynced>().behaviors.First(a=>a.GetIl2CppType().Name=="WanderModel").Cast<WanderModel>();
                Log("Name "+wander.name+",idletimemax "+wander.IdleTimeMax+",idletimemin "+wander.IdleTimeMin+",innerradius "+wander.InnerRadius+
                    ",outerradius "+wander.OuterRadius+",speed "+wander.Speed+",startattower "+wander.startAtTower+
                    ",stayinarea "+wander.StayInArea+",usesyncedrandom "+wander.useSyncedRandom);
                foreach(var thing in wander.MotionCurve){
                    Log("intangent "+thing.inTangent+",inweight "+thing.inWeight+",outtangent "+thing.outTangent+",outweight "+thing.outWeight+
                        ",tangentmode "+thing.tangentMode+",tangentmodeinternal "+thing.tangentModeInternal+",time "+thing.time+
                        ",value "+thing.value+",weightedmode "+thing.weightedMode);
                }
            }
        }*/
    }
}