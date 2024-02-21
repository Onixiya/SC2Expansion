//only left it here in case someone wants to try to understand the vanilla hero screen
namespace SC2ExpansionLoader.HeroPatches{
	[HarmonyPatch(typeof(CosmeticHelper),"ApplyTowerSkinToTowerModel")]
	public class CosmeticHelperApplyTowerSkinToTowerModel_Patch{
		public static bool Prefix(TowerModel towerModel){
			if(TowerTypes.ContainsKey(towerModel.baseId)){
				return false;
			}else{
				return true;
			}
		}
	}
	//most of this specific patch is kosmics code, ty for figuring it out
	/*[HarmonyPatch(typeof(HeroUpgradeDetails),"BindDetails")]
	public class HeroUpgradeDetailsBindDetails_Patch{
		public static bool Prefix(ref HeroUpgradeDetails __instance,ref bool __result,string heroIdToUse,bool showingSelected,bool forceUpdate){
			if(TowerTypes.ContainsKey(heroIdToUse)){
				try{
					__instance.selectedHeroId=heroIdToUse;
					Btd6Player Player=DailyChallengeManager.Player;
					__instance.skinData=SkinManager.GetSelectedSkin(Player,__instance.selectedHeroId);
					Transform heroBackgroundBannerTransform=__instance.heroBackgroundBanner.transform;
					heroBackgroundBannerTransform.DestroyAllChildren();
					SkinData skin=__instance.skinData;
					_=ResourceLoader.InstantiateAsync(skin.backgroundBanner.guidRef,heroBackgroundBannerTransform).Result;
					__instance.StartBackgroundColourSwap();
					__instance.SetHeroFontMaterial();
					string guidRef=skin.GetPortraitForLevel(__instance.selectedUpgradeIndex+1).guidRef;
					ResourceLoader.LoadSpriteFromSpriteReferenceAsync(new(){guidRef=guidRef},__instance.heroPortrait,false);
					__instance.selectedHeroPortraitSpriteId.guidRef=guidRef;
					__instance.heroName.text=GameMenu.Locs.GetText(__instance.selectedHeroId);
					__instance.heroShortDescription.text=GameMenu.Locs.GetText(skin.skinName);
					__instance.heroDescription.text=GameMenu.Locs.GetText(skin.description);
					__instance.buyHeroButton.gameObject.SetActive(false);
					__instance.selectButton.gameObject.SetActive(true);
					__instance.inGameCostPanel.SetActive(true);
					__instance.heroRequirements.SetActive(false);
					__instance.saleObj.SetActive(false);
					__instance.saleOrigAmountTxt.gameObject.SetActive(false);
					if(Player.Data.primaryHero==__instance.selectedHeroId){
						__instance.selectedText.gameObject.SetActive(true);
						__instance.selectButton.gameObject.SetActive(false);
					}else{
						__instance.selectedText.gameObject.SetActive(false);
						__instance.selectButton.gameObject.SetActive(true);
					}
					__instance.GetSelectedButton(__instance.selectedHeroId).hasPurchasedHero=true;
					__instance.UpdateAllButtons(forceUpdate);
					foreach(TextMeshProUGUI cost in __instance.heroCost){
						Log(1);
						cost.text=gameModel.GetTowersWithBaseId(heroIdToUse)[0].cost.ToString();
						Log(2);
					}
					var towers=gameModel.GetTowersWithBaseId(__instance.selectedHeroId);
					var upgradeButtons=__instance.heroUpgrades;
					for(int i=0;i<upgradeButtons.Length;i++){
						TowerModel tower=null;
						if(i+1>=towers.Count){
							tower=towers[i];
						}else{
							tower=towers[i+1];
						}
						if(tower.GetDescendant<AbilityModel>()!=null){
							Log("ability");
							Log(tower.name+" "+tower.GetDescendants<AbilityModel>().Last().addedViaUpgrade);
							if(tower.GetDescendants<AbilityModel>().Last().addedViaUpgrade.Split(' ')[2]==(i+1).ToString()){
								Log("ability1");
								upgradeButtons[i].Init(__instance.selectedHeroId,i+1,tower.GetDescendants<AbilityModel>().Last(),__instance.selectedUpgradeIndex==i,tower.portrait,null,__instance);
							}
						}else{
							Log("no ability");
							upgradeButtons[i].Init(__instance.selectedHeroId,i+1,null,__instance.selectedUpgradeIndex==i,tower.portrait,null,__instance);
						}
					}
				}catch(Exception error){
					Log("Is addedViaUpgrade missing?");
					PrintError(error);
				}
				__result=true;
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(HeroUpgradeDetails),"SetupButtonsPanel")]
	public class HeroUpgradeDetailsSetupButtonsPanel_Patch{
		public static bool Prefix(ref HeroUpgradeDetails __instance){
			try{
				var selectedHeroes=__instance.selectedHeroes=new();
				__instance.sortedTowers.Clear();
				var heroButtons=__instance.heroButtons;
				foreach(var heroButton in heroButtons){
					__instance.unusedHeroButtons.Add(heroButton);
					heroButton.gameObject.SetActive(false);
				}
				__instance.SortHerosByUnlockLevel();
				foreach(SC2Tower tower in HeroTypes.Values){
					if(!selectedHeroes.Contains(tower.HeroDetails)){
						selectedHeroes.Add(tower.HeroDetails);
					}
				}
				foreach(var thing in selectedHeroes){
					var limitedHeroSelection=__instance.limitedHeroSelection;
					if(limitedHeroSelection!=null&&limitedHeroSelection.Contains(thing.towerId)){
						break;
					}
					var NextButton=__instance.GetNextButton();
					heroButtons.Add(NextButton);
					NextButton.gameObject.SetActive(true);
					NextButton.Init(__instance,thing,thing.monkeyMoneyCost,false,false);
				}
			}catch(Exception error){
				PrintError(error);
			}
			return false;
		}
	}
	[HarmonyPatch(typeof(HeroInGameScreen),"Open")]
	public class HeroInGameScreenOpen_Patch{
		public static bool Prefix(ref HeroInGameScreen __instance,Il2CppSystem.Object data){
			var heroData=data.Cast<Il2CppSystem.Tuple<string,TowerToSimulation>>();
			if(HeroTypes.ContainsKey(heroData.Item1)){
				try{
					__instance.heroId=heroData.Item1;
					__instance.selectedHero=heroData.Item2;
					var instance=InGame.instance;
					var bridge=instance.bridge;
					bridge.GetFirstTowerWithBaseID(__instance.heroId,bridge.GetInputId());
					__instance.UpdateDisplay(0);
					__instance.unlockHeroPanel.SetActive(false);
					__instance.heroName.text=LocManager.GetText(__instance.heroId);
					__instance.skinData=SkinManager.GetSkin(DailyChallengeManager.Player.GetSelectedTowerSkin(__instance.heroId));
					var skinData=__instance.skinData;
					__instance.StartBackgroundColourSwap(skinData);
					__instance.SetHeroFontMaterial();
					__instance.heroShortDescription.text=LocManager.GetText(skinData.skinName);
					__instance.heroDescription.text=LocManager.GetText(skinData.description);
					var towers=gameModel.GetTowersWithBaseId(__instance.heroId);
					var upgradeButtons=__instance.heroUpgrades;
					for(int i=0;i<upgradeButtons.Length;i++){
						TowerModel tower=null;
						if(i+1>=towers.Count){
							tower=towers[i];
						}else{
							tower=towers[i+1];
						}
						if(tower.GetDescendant<AbilityModel>()!=null&&tower.GetDescendants<AbilityModel>().Last().addedViaUpgrade.Split(' ')[2]==(i+1).ToString()){
							upgradeButtons[i].Init(__instance.heroId,i+1,tower.GetDescendants<AbilityModel>().Last(),__instance.selectedUpgradeIndex==i,tower.portrait);
						}else{
							upgradeButtons[i].Init(__instance.heroId,i+1,null,__instance.selectedUpgradeIndex==i,tower.portrait);
						}
					}
					__instance.heroBoosterCaveatTxt.text=LocManager.GetText("Applies For This Game Only"); 
					NavigationExt.LinkVertical(upgradeButtons[0].transform.parent);
				}catch(Exception error){
					PrintError(error);
				}
				return false;
			}
			return true;
		}
		[HarmonyPatch(typeof(HeroUpgradeButton),"Highlight")]
		public class HeroUpgradeButtonHighlight_Patch{
			public static bool Prefix(ref HeroUpgradeButton __instance){
				string guid=__instance.portraitSprite.guidRef;
				try{
					guid=guid.Split('[')[1].Split(']')[0].Split('-')[0];
					if(HeroTypes.ContainsKey(guid)){
						if(__instance.highlightEffect.activeSelf==false){
							__instance.highlightEffect.SetActive(true);
						}else{
							__instance.highlightEffect.SetActive(false);
						}
						return false;
					}
				}catch{}
				return true;
			}
		}
	}*/
}