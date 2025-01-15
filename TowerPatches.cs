namespace SC2Expansion{
    public class TowerPatches{
        [HarmonyPatch(typeof(Weapon),nameof(Weapon.SpawnDart))]
        public class WeaponSpawnDart_Patch{
            public static void Postfix(ref Weapon __instance){
                string towerName=__instance.attack.tower.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    try{
                        TowerTypes[towerName].Attack(__instance);
                    }catch(Exception error){
                        PrintError(error,"Failed to run Attack for "+towerName);
                    }
                }
            }
            public static bool Prefix(ref Weapon __instance){
               string towerName=__instance.attack.tower.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    try{
                        return TowerTypes[towerName].PreAttack(__instance);
                    }catch(Exception error){
                        PrintError(error,"Failed to run PreAttack for "+towerName);
                    }
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(Attack),nameof(Attack.FilterTarget))]
        public class AttackFilterTarget_patch{
            public static bool Prefix(ref Attack __instance){
                string id=__instance.tower.towerModel.baseId;
                if(TowerTypes.ContainsKey(id)){
                    return TowerTypes[id].AttackTargetFilter(__instance);
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(Simulation),nameof(Simulation.RoundStart))]
        public class SimulationRoundStart_Patch{
            public static void Postfix(){
                foreach(SC2Tower tower in TowerTypes.Values){
                    try{
                        tower.RoundStart();
                    }catch(Exception error){
                        PrintError(error,"Failed to run RoundStart for "+tower.Name);
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Simulation),nameof(Simulation.RoundEnd))]
        public class SimulationRoundEnd_Patch{
            public static void Postfix(){
                foreach(SC2Tower tower in TowerTypes.Values){
                    try{
                        tower.RoundEnd();
                    }catch(Exception error){
                        PrintError(error,"Failed to run RoundEnd for "+tower.Name);
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Tower),nameof(Tower.OnPlace))]
        public class TowerOnPlace_Patch{
            public static void Postfix(ref Tower __instance){
                string towerName=__instance.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    try{
                        SC2Tower tower=TowerTypes[towerName];
                        tower.SimTowers.Add(__instance);
                        tower.Create(__instance);
                    }catch(Exception error){
                        PrintError(error,"Failed to run Create for "+towerName);
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Ability),nameof(Ability.Activate))]
        public class AbilityActivate_Patch{
            public static bool Prefix(ref Ability __instance){
                string towerName=__instance.tower.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    try{
                        return TowerTypes[towerName].Ability(__instance.abilityModel.name,__instance.tower);
                    }catch(Exception error){
                        PrintError(error,"Failed to run Ability for "+towerName);
						return false;
                    }
                }
				return true;
            }
        }
        [HarmonyPatch(typeof(TowerManager),nameof(TowerManager.DestroyTower))]
        public class TowerManagerDestroyTower_Patch{
            public static void Postfix(ref Tower tower){
                string towerName=tower.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    try{
                        TowerTypes[towerName].Sell(tower);
                    }catch(Exception error){
                        PrintError(error,"Failed to run Sell for "+towerName);
                    }
                }
            }
        }
		[HarmonyPatch(typeof(TowerManager),nameof(TowerManager.UpgradeTower))]
        public class TowerManagerUpgradeTower_Patch{
            public static void Postfix(ref Tower tower,ref TowerModel def){
                string towerName=tower.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    try{
                        TowerTypes[towerName].Upgrade(def.tier,tower);
                    }catch(Exception error){
                        PrintError(error,"Failed to run Upgrade for "+towerName);
                    }
                }
            }
        }
		/*[HarmonyPatch(typeof(UnityToSimulation),"UpgradeTower_Impl")]
        public class UnityToSimulationUpgradeTowerImpl_Patch{
            [HarmonyPrefix]
            public static bool Prefix(ref UnityToSimulation __instance,Il2CppAssets.Scripts.ObjectId id,int pathIndex,int inputId){
				TowerManager towerManager=__instance.simulation.towerManager;
				Tower tower=towerManager.GetTowerById(id);
				TowerModel towerModel=tower.towerModel;
				if(TowerTypes.ContainsKey(towerModel.baseId)){
					if(!TowerTypes[towerModel.baseId].Hero){
						int cost=gameModel.upgrades.First(a=>a.name==towerModel.upgrades[pathIndex].upgrade).cost;
						if(__instance.simulation.GetCash(inputId)>cost){
							towerManager.UpgradeTower(inputId,tower,gameModel.GetTowerFromId(towerModel.upgrades[pathIndex].tower),pathIndex,cost);
						}
						return false;
					}
				}
				return true;
			}
		}*/
        [HarmonyPatch(typeof(TowerSelectionMenu),nameof(TowerSelectionMenu.SelectTower))]
        public class TowerSelectionMenuSelectTower_Patch{
            public static void Postfix(TowerSelectionMenu __instance){
                string towerName=__instance.selectedTower.tower.towerModel.baseId;
                if(TowerTypes.ContainsKey(towerName)){
                    try{
                        TowerTypes[towerName].Select(__instance.selectedTower.tower);
                    }catch(Exception error){
                        PrintError(error,"Failed to run Select for "+towerName);
                    }
                }
            }
        }
    }
}