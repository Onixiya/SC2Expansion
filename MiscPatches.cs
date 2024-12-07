using Il2CppInterop.Runtime.Injection;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.AddressableAssets;
using Il2CppSystem.Linq;
namespace SC2ExpansionLoader{
    public static class HarmonyPatches{
        [HarmonyPatch(typeof(Btd6Player),"CheckForNewParagonPipEvent")]
        public class Btd6Player_CheckForNewParagonPipEvent_Patch{
            [HarmonyPrefix]
            public static bool Prefix(){
                return false;
            }
        }
        [HarmonyPatch(typeof(ProfileModel),"Validate")]
        public class ProfileModel_Validate_Patch{
            public static void Prefix(ProfileModel __instance){
				if(gameModel.GetTowerFromId(__instance.primaryHero)==null){
					Log("Selected hero is NULL! Resetting to Quincy");
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
                        //if(!tower.Hero)continue;
                        __instance.unlockedHeroes.AddIfNotPresent(tower.Name);
                        __instance.seenUnlockedHeroes.AddIfNotPresent(tower.Name);
                        __instance.seenNewHeroNotification.AddIfNotPresent(tower.Name);
                    }catch(Exception error){
                        PrintError(error,"Failed to add "+sc2Tower.Key+" to unlocked towers or upgrades");
                    }
                }
            }
        }
        [HarmonyPatch(typeof(GameModelUtil.__c__DisplayClass4_0),"_LoadGameModelAsync_b__0")]
        public class GameModelUtil_LoadGameModelAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref GameModel __result){
                gameModel=__result;
                LocManager=LocalizationManager.Instance;
				try{
					//mostly suited for protoss warp things
                    CreateTowerAttackModel=gameModel.GetTowerFromId("EngineerMonkey-100").behaviors.GetModel<AttackModel>("Spawner").Clone<AttackModel>();
                    List<Model>createTowerBehav=CreateTowerAttackModel.behaviors.ToList();
                    createTowerBehav.Remove(createTowerBehav.First(a=>a.GetIl2CppType().Name=="RotateToTargetModel"));
                    createTowerBehav.GetModel<RandomPositionModel>().minDistance=70;
                    createTowerBehav.GetModel<RandomPositionModel>().maxDistance=90;
                    createTowerBehav.GetModel<RandomPositionModel>().idealDistanceWithinTrack=0;
                    createTowerBehav.GetModel<RandomPositionModel>().useInverted=false;
                    CreateTowerAttackModel.behaviors=createTowerBehav.ToArray();
                    ProjectileModel proj=CreateTowerAttackModel.weapons[0].projectile;
                    //proj.display=new(){guidRef=""};
                    proj.display=new("");
                    Il2CppReferenceArray<Model>projBehav=proj.behaviors;
                    ArriveAtTargetModel arriveAtTargetModel=projBehav.GetModel<ArriveAtTargetModel>();
                    arriveAtTargetModel.expireOnArrival=false;
                    arriveAtTargetModel.altSpeed=400;
                    DisplayModel displayModel=projBehav.GetModel<DisplayModel>();
                    displayModel.delayedReveal=1;
                    displayModel.positionOffset=new(0,0,190);
				}catch(Exception error){
					PrintError(error,"Failed to create CreateTowerAttackModel");
				}
				try{
                    BlankAbilityModel=gameModel.GetTowerFromId("Quincy 4").behaviors.GetModel<AbilityModel>().Clone<AbilityModel>();
                    BlankAbilityModel.description="AbilityDescription";
                    BlankAbilityModel.displayName="AbilityDisplayName";
                    BlankAbilityModel.name="AbilityName";
                    List<Model>behaviors=BlankAbilityModel.behaviors.ToList();
                    behaviors.Remove(behaviors.First(a=>a.GetIl2CppType().Name=="TurboModel"));
                    behaviors.Remove(behaviors.First(a=>a.GetIl2CppType().Name=="CreateEffectOnAbilityModel"));
                    behaviors.Remove(behaviors.First(a=>a.GetIl2CppType().Name=="CreateSoundOnAbilityModel"));
                    BlankAbilityModel.behaviors=behaviors.ToArray();
                }catch(Exception error){
                    PrintError(error,"Failed to create BlankAbilityModel");
                }
                List<TowerModel>towers=gameModel.towers.ToList();
                List<TowerDetailsModel>towerSet=gameModel.towerSet.ToList();
                List<UpgradeModel>upgrades=gameModel.upgrades.ToList();
				List<TowerDetailsModel>heroSet=gameModel.heroSet.ToList();
                List<string>towerNames=new();
                try{
                    foreach(SC2Tower tower in TowerTypes.Values){
                        towerNames.Add(tower.Name);
                        if(tower.AddToShop){
                            towerSet.Add(tower.ShopDetails());
                        }
						if(tower.Hero){
							tower.HeroDetails=tower.GenerateHeroDetails();
							heroSet.Add(tower.HeroDetails);
							GameData.Instance.skinsData.AddSkins(new(new[]{tower.HeroSkin()}));
						}
						var towerTypes=TowerType.towers.ToList();
						towerTypes.Add(tower.Name);
						TowerType.towers=towerTypes.ToArray();
                        if(tower.Upgradable){
                            tower.UpgradeModels=tower.GenerateUpgradeModels();
                            upgrades.AddRange(tower.UpgradeModels);
                        }
						tower.TowerModels=tower.GenerateTowerModels();
                        towers.AddRange(tower.TowerModels);
                        gameModel.towers=towers.ToArray();
                        gameModel.towerSet=towerSet.ToArray();
                        gameModel.upgrades=upgrades.ToArray();
						gameModel.heroSet=heroSet.ToArray();
                        Log("Loaded "+tower.Name);
                    }
                }catch(Exception error){
                    PrintError(error,"Failed to add "+towerNames.Last());
                }
            }
        }
		[HarmonyPatch(typeof(TowerInventory),"CreatedTower")]
        public class TowerInventory_CreatedTower_Patch{
            [HarmonyPrefix]
            public static bool Prefix(TowerInventory __instance,TowerModel def){
                if(!__instance.towerCounts.ContainsKey(def.baseId)){
                    __instance.towerCounts.Add(def.baseId,0);
                }
		        __instance.towerCounts[def.baseId]=__instance.towerCounts[def.baseId]+1;
		        return false;
            }
        }
        [HarmonyPatch(typeof(TowerInventory),"DestroyedTower")]
        public class TowerInventoryDestroyedTower_Patch{
            [HarmonyPrefix]
            public static bool Prefix(ref TowerInventory __instance,ref TowerModel def){
                if(!__instance.towerCounts.ContainsKey(def.baseId)){
                    __instance.towerCounts.Add(def.baseId,0);
                }
		        __instance.towerCounts[def.baseId]=__instance.towerCounts[def.baseId]-1;
		        return false;
            }
        }
        [HarmonyPatch(typeof(FighterMovement),"Process")]
        public class FighterMovementProcess_Patch{
            [HarmonyPrefix]
            public static bool Prefix(ref FighterMovement __instance,int elapsed){
                __instance.timer++;
		        if(__instance.flyoverEngaged==false){
                    __instance.timer=0;
		        }
		        __instance.ApplyMovement(new(0,0),elapsed);
                return false;
            }
        }
        [HarmonyPatch(typeof(SubTowerFilter),"FilterEmission")]
        public class SubTowerFilter_FilterEmission_Patch{
            [HarmonyPrefix]
            public static bool Prefix(SubTowerFilter __instance,ref bool __result){
                if(__instance.createdSubTowers.Count>=__instance.subTowerFilterModel.maxNumberOfSubTowers){
                    __result=false;
                }else{
                    __result=true;
                }
                return false;
            }
        }
        [HarmonyPatch(typeof(CreateSoundOnUpgrade),"OnUpgrade")]
        public class CreateSoundOnUpgrade_OnUpgrade_Patch{
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