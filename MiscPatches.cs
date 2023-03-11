using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Simulation.Input;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons.Behaviors;

namespace SC2ExpansionLoader{
    public class HarmonyPatches{
        [HarmonyPatch(typeof(Btd6Player),"CheckForNewParagonPipEvent")]
        public class Btd6PlayerCheckForNewParagonPipEvent_Patch{
            [HarmonyPrefix]
            public static bool Prefix(){
                return false;
            }
        }
        [HarmonyPatch(typeof(ProfileModel),"Validate")]
        public class ProfileModelValidate_Patch{
            public static void Prefix(ProfileModel __instance){
				if(gameModel.GetTowerFromId(__instance.primaryHero)==null){
					Log("Selected hero is NULL! Resetting to Quincy");
					__instance.primaryHero="Quincy";
				}
			}
            public static void Postfix(ProfileModel __instance){
                foreach(KeyValuePair<string,SC2Tower>sc2tower in TowerTypes){
                    try{
                        SC2Tower tower=sc2tower.Value;
                        __instance.unlockedTowers.AddIfNotPresent(tower.Name);
                        if(tower.Upgradable){
							if(tower.UpgradeModels==null){
                            	tower.UpgradeModels=tower.GenerateUpgradeModels();
							}
							foreach(UpgradeModel upgrade in tower.UpgradeModels){
								__instance.acquiredUpgrades.AddIfNotPresent(upgrade.name);
							}
                        }
						if(tower.Hero){
							__instance.unlockedHeroes.AddIfNotPresent(tower.Name);
							__instance.seenUnlockedHeroes.AddIfNotPresent(tower.Name);
							__instance.seenNewHeroNotification.AddIfNotPresent(tower.Name);
						}
                    }catch(Exception error){
                        PrintError(error,"Failed to add "+sc2tower.Key+" to unlocked towers or upgrades");
                    }
                }
            }
        }
        [HarmonyPatch(typeof(TitleScreen),"Start")]
        public class TitleScreenStart_Patch{
            [HarmonyPostfix]
            public static void Postfix(){
                gameModel=Game.instance.model;
                LocManager=LocalizationManager.Instance;
				try{
					//mostly suited for protoss warp things
                    CreateTowerAttackModel=gameModel.GetTowerFromId("EngineerMonkey-100").behaviors.GetModel<AttackModel>().Clone<AttackModel>();
                    List<Model>createTowerBehav=CreateTowerAttackModel.behaviors.ToList();
                    createTowerBehav.Remove(createTowerBehav.First(a=>a.GetIl2CppType().Name=="RotateToTargetModel"));
                    createTowerBehav.GetModel<RandomPositionModel>().minDistance=70;
                    createTowerBehav.GetModel<RandomPositionModel>().maxDistance=90;
                    createTowerBehav.GetModel<RandomPositionModel>().idealDistanceWithinTrack=0;
                    createTowerBehav.GetModel<RandomPositionModel>().useInverted=false;
                    CreateTowerAttackModel.behaviors=createTowerBehav.ToArray();
                    CreateTowerAttackModel.weapons[0].projectile.display=new(){guidRef=""};
                    CreateTowerAttackModel.weapons[0].projectile.behaviors.GetModel<ArriveAtTargetModel>().expireOnArrival=false;
                    CreateTowerAttackModel.weapons[0].projectile.behaviors.GetModel<ArriveAtTargetModel>().altSpeed=400;
                    CreateTowerAttackModel.weapons[0].projectile.behaviors.GetModel<DisplayModel>().delayedReveal=1;
                    CreateTowerAttackModel.weapons[0].projectile.behaviors.GetModel<DisplayModel>().positionOffset=new(0,0,190);
				}catch(Exception error){
					PrintError(error,"Failed to create CreateTowerAttackModel");
				}
				try{
                    BlankAbilityModel=gameModel.GetTowerFromId("Quincy 4").Cast<TowerModel>().behaviors.
                        First(a=>a.GetIl2CppType().Name=="AbilityModel").Clone().Cast<AbilityModel>();
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
							if(tower.UpgradeModels==null){
								tower.UpgradeModels=tower.GenerateUpgradeModels();
							}
                            foreach(UpgradeModel upgrade in tower.UpgradeModels){
                                upgrades.Add(upgrade);
                            }
                        }
						tower.TowerModels=tower.GenerateTowerModels();
                        foreach(TowerModel towerModel in tower.TowerModels){
                            towers.Add(towerModel);
                        }
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
        public class TowerInventoryCreatedTower_Patch{
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
        public class SubTowerFilterFilterEmission_Patch{
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