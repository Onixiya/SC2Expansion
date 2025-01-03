namespace SC2ExpansionLoader{
    public static class HarmonyPatches{
        [HarmonyPatch(typeof(Btd6Player),nameof(Btd6Player.CheckForNewParagonPipEvent))]
        public class Btd6Player_CheckForNewParagonPipEvent_Patch{
            public static bool Prefix(){
                return false;
            }
        }
        [HarmonyPatch(typeof(ProfileModel),nameof(ProfileModel.Validate))]
        public class ProfileModel_Validate_Patch{
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
        [HarmonyPatch(typeof(GameModelUtil.__c__DisplayClass4_0),nameof(GameModelUtil.__c__DisplayClass4_0._LoadGameModelAsync_b__0))]
        public class GameModelUtil_LoadGameModelAsync_Patch{
            public static void Postfix(ref GameModel __result){
                gameModel=__result;
                LocManager=LocalizationManager.Instance;
				try{
					//mostly suited for protoss warp things
                    AttackModel attack=gameModel.GetTowerFromId("EngineerMonkey-100").behaviors.GetModel<AttackModel>("Spawner").Clone<AttackModel>();
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
                    CreateTowerAttackModel=attack;
				}catch(Exception error){
					PrintError(error,"Failed to create CreateTowerAttackModel");
				}
				try{
                    AbilityModel ability=gameModel.GetTowerFromId("Quincy 4").behaviors.GetModel<AbilityModel>().Clone<AbilityModel>();
                    ability.description="AbilityDescription";
                    ability.displayName="AbilityDisplayName";
                    ability.name="AbilityName";
                    List<Model>behaviors=ability.behaviors.ToList();
                    behaviors.RemoveModel<TurboModel>();
                    behaviors.RemoveModel<CreateEffectOnAbilityModel>();
                    behaviors.RemoveModel<CreateSoundOnAbilityModel>();
                    ability.behaviors=behaviors.ToArray();
                    BlankAbilityModel=ability;
                }catch(Exception error){
                    PrintError(error,"Failed to create BlankAbilityModel");
                }
                try{
                    CreateSoundOnSelectedModel csosm=gameModel.GetTowerFromId("Quincy").behaviors.GetModel<CreateSoundOnSelectedModel>().Clone<CreateSoundOnSelectedModel>();
                    csosm.name="SC2Expansion-Select";
                    SelectedSoundModel=csosm;
                }catch(Exception error){
                    PrintError(error,"Failed to create SelectedSoundModel");
                }
                List<TowerModel>towers=gameModel.towers.ToList();
                List<TowerDetailsModel>towerSet=gameModel.towerSet.ToList();
                List<UpgradeModel>upgrades=gameModel.upgrades.ToList();
				List<TowerDetailsModel>heroSet=gameModel.heroSet.ToList();
                List<string>towerNames=new();
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
						    GameData.Instance.skinsData.AddSkins(new(new[]{tower.HeroSkin()}));
					    }
					    var towerTypes=TowerType.towers.ToList();
					    towerTypes.Add(tower.Name);
					    TowerType.towers=towerTypes.ToArray();
					    tower.TowerModels=tower.GenerateTowerModels();
                        towers.AddRange(tower.TowerModels);
                        gameModel.towers=towers.ToArray();
                        gameModel.towerSet=towerSet.ToArray();
                        gameModel.upgrades=upgrades.ToArray();
					    gameModel.heroSet=heroSet.ToArray();
                        Log("Loaded "+tower.Name);
                    }catch(Exception error){
                        PrintError(error,"Failed to add "+towerNames.Last());
                    }
                }
            }
        }
		[HarmonyPatch(typeof(TowerInventory),nameof(TowerInventory.CreatedTower))]
        public class TowerInventory_CreatedTower_Patch{
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
            [HarmonyPrefix]
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
        [HarmonyPatch(typeof(SubTowerFilter),nameof(SubTowerFilter.FilterEmission))]
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
        [HarmonyPatch(typeof(CreateSoundOnUpgrade),nameof(CreateSoundOnUpgrade.OnUpgrade))]
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