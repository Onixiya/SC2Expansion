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
            [HarmonyPostfix]
            public static void Postfix(ProfileModel __instance){
                foreach(KeyValuePair<string,SC2Tower>sc2tower in TowerTypes){
                    try{
                        SC2Tower tower=sc2tower.Value;
                        __instance.unlockedTowers.Add(tower.Name);
                        if(tower.Upgradable){
                            tower.UpgradeModels=tower.GenerateUpgradeModels();
                            foreach(UpgradeModel upgrade in tower.UpgradeModels){
                                __instance.acquiredUpgrades.Add(upgrade.name);
                            }
                        }
                    }catch(Exception error){
                        Log("Failed to add "+sc2tower.Key+" to unlocked towers or upgrades");
                        string message=error.Message;
                        message+="@\n"+error.StackTrace;
                        Log(message,"error");
                    }
                }
            }
        }
        [HarmonyPatch(typeof(TitleScreen),"Start")]
        public class TitleScreenStart_Patch{
            [HarmonyPostfix]
            public static void Postfix(){
                try{
                    gameModel=Game.instance.model;
                    LocManager=LocalizationManager.Instance;
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
                    Log("Failed to create BlankAbilityModel");
                    string message=error.Message;
                    message+="@\n"+error.StackTrace;
                    Log(message,"error");
                }
                List<TowerModel>towers=gameModel.towers.ToList();
                List<TowerDetailsModel>towerSet=gameModel.towerSet.ToList();
                List<UpgradeModel>upgrades=gameModel.upgrades.ToList();
                List<string>towerNames=new();
                try{
                    foreach(SC2Tower tower in TowerTypes.Values){
                        towerNames.Add(tower.Name);
                        tower.TowerModels=tower.GenerateTowerModels();
                        tower.UpgradeModels=tower.GenerateUpgradeModels();
                        foreach(TowerModel towerModel in tower.TowerModels){
                            towers.Add(towerModel);
                        }
                        if(tower.AddToShop){
                            towerSet.Add(tower.ShopDetails());
                        }
                        if(tower.Upgradable){
                            foreach(UpgradeModel upgrade in tower.UpgradeModels){
                                upgrades.Add(upgrade);
                            }
                        }
                        gameModel.towers=towers.ToArray();
                        gameModel.towerSet=towerSet.ToArray();
                        gameModel.upgrades=upgrades.ToArray();
                        Log("Loaded "+tower.Name);
                    }
                }catch(Exception error){
                    Log("Failed to add "+towerNames.Last());
                    string message=error.Message;
                    message+="@\n"+error.StackTrace;
                    Log(message,"error");
                }
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