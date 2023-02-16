namespace SC2ExpansionLoader{
    public class UpgradeMenu{
        public static AssetBundle LoadedUIBundle=null;
        [HarmonyPatch(typeof(InGame),"ShowUpgradeTree")]
        [HarmonyPatch(new Type[]{typeof(TowerModel),typeof(bool)})]
        public class InGameShowUpgradeTree_Patch{
            [HarmonyPrefix]
            public static bool Prefix(TowerModel towerModel){
                try{
                string towerId=towerModel.baseId;
                if(TowerTypes.ContainsKey(towerId)){
                    SC2Tower tower=TowerTypes[towerId];
                    string faction=tower.TowerFaction.ToString();
                    LoadedUIBundle=AssetBundle.LoadFromFile(BundleDir+faction.ToLower()+"upgrademenu");
                    string scene="Assets/Scenes/"+tower.TowerFaction.ToString()+"UiScene.unity";
                    SceneManager.LoadScene(scene,LoadSceneMode.Additive);
                    //must be done in a coroutine as scenes need a frame to get set up;
                    MelonCoroutines.Start(Setup(tower,tower.TowerFaction.ToString()+"UiScene"));
                    return false;
                }
                }catch(Exception error){
                    string message=error.Message;
                    message+="@\n"+error.StackTrace;
                    Log(message,"error");
                }
                return true;
            }
            public static System.Collections.IEnumerator Setup(SC2Tower tower,string scene){
                yield return null;
                SceneManager.GetSceneByName(scene).GetRootGameObjects().First(a=>a.name==tower.TowerFaction.ToString()+"Menu").
                    AddComponent<UpgradeMenuCom>().CurrentTower=tower;
            }
        }
    }
    [RegisterTypeInIl2Cpp]
    public class UpgradeMenuButton:MonoBehaviour{
        public UpgradeMenuButton(IntPtr ptr):base(ptr){}
        public string PortraitAsset;
        public string Description;
        public int Cost;
        public UpgradeMenuCom Menu;
        void Start(){
            GetComponent<Button>().onClick.AddListener(new Action(()=>{
                Menu.UpgradeDescription.text=Description;
                Texture2D texture=LoadAsset<Texture2D>(PortraitAsset,Menu.CurrentTower.LoadedBundle).Cast<Texture2D>();
                Menu.Portrait.sprite=Sprite.Create(texture,new(0,0,texture.width,texture.height),new());
                Menu.Cost.text="Cost: "+Cost;
                Menu.UpgradeDescription.gameObject.SetActive(true);
            }));
        }
    }
    [RegisterTypeInIl2Cpp]
    public class UpgradeMenuCom:MonoBehaviour{
        public UpgradeMenuCom(IntPtr ptr):base(ptr){}
        public SC2Tower CurrentTower;
        public Il2CppTMPro.TextMeshProUGUI Title;
        public Il2CppTMPro.TextMeshProUGUI Description;
        public Il2CppTMPro.TextMeshProUGUI Cost;
        public Il2CppTMPro.TextMeshProUGUI UpgradeDescription;
        public Image Portrait;
        public List<Button>Path1Icons=new();
        public List<Button>Path2Icons=new();
        public Image Background;
        public List<Image>Images=new();
        public List<Il2CppTMPro.TextMeshProUGUI>Texts=new();
        public void Start(){
            try{
                foreach(Image image in GetComponentsInChildren<Image>()){
                    if(image.gameObject!=gameObject){
                        Images.Add(image);
                        image.color=new(255,255,255,0);
                    }else{
                        Background=image;
                        image.color=new(255,255,255,0);
                    }
                }
                foreach(Il2CppTMPro.TextMeshProUGUI text in GetComponentsInChildren<Il2CppTMPro.TextMeshProUGUI>()){
                    Texts.Add(text);
                    text.color=new(255,255,255,0);
                }
                Title=transform.GetChild(0).GetComponent<Il2CppTMPro.TextMeshProUGUI>();
                Description=Title.transform.GetChild(0).GetComponent<Il2CppTMPro.TextMeshProUGUI>();
                Transform container=transform.GetChild(1);
                Cost=container.GetChild(0).GetComponentInChildren<Il2CppTMPro.TextMeshProUGUI>();
                Portrait=container.GetChild(1).GetComponent<Image>();
                Transform upgrades=transform.GetChild(2);
                UpgradeDescription=upgrades.GetChild(2).GetComponent<Il2CppTMPro.TextMeshProUGUI>();
                foreach(Button button in upgrades.GetChild(0).GetComponentsInChildren<Button>()){
                    button.gameObject.SetActive(false);
                    Path1Icons.Add(button);
                }
                foreach(Button button in upgrades.GetChild(1).GetComponentsInChildren<Button>()){
                    button.gameObject.SetActive(false);
                    Path2Icons.Add(button);
                }
                Title.text=CurrentTower.Name;
                Description.text=CurrentTower.Description;
                Cost.text="Cost: "+CurrentTower.TowerModels[0].cost;
                UpgradeDescription.gameObject.SetActive(false);
                string portraitAsset=CurrentTower.TowerModels[0].portrait.guidRef.Split('[')[1];
                portraitAsset=portraitAsset.Remove(portraitAsset.Length-1);
                Texture2D texture=LoadAsset<Texture2D>(portraitAsset,CurrentTower.LoadedBundle).Cast<Texture2D>();
                Portrait.sprite=Sprite.Create(texture,new(0,0,texture.width,texture.height),new());
                switch(CurrentTower.TowerModels[0].upgrades.Length){
                    case 1:
                        upgrades.GetChild(0).localPosition=new(0,0,0);
                        SetRowUp(Path1Icons);
                        break;
                    case 2:
                        SetRowUp(Path1Icons);
                        SetRowUp(Path2Icons);
                        break;
                    /*case 3:
                        SetRowUp(Path1Icons);
                        SetRowUp(Path2Icons);
                        SetRowUp(Path3Icons);
                        break;*/
                }
                transform.GetChild(3).GetComponent<Button>().onClick.AddListener(new Action(()=>{
                    MelonCoroutines.Start(Hide(this));
                }));
                if(CurrentTower.UpgradeScreenSound!=null){
                    PlaySound(CurrentTower.UpgradeScreenSound);
                }else{
                    PlaySound(CurrentTower.Name+"-Birth");
                }
            }catch(Exception error){
                Log("Failed to load "+CurrentTower.TowerFaction.ToString()+" upgrade menu for "+CurrentTower.Name);
                string message=error.Message;
                message+="@\n"+error.StackTrace;
                Log(message,"error");
                uObject.Destroy(gameObject);
                return;
            }
            MelonCoroutines.Start(Show(this));
        }
        [HideFromIl2Cpp]
        public void SetRowUp(List<Button>buttons){
            for(int i=0;i<CurrentTower.MaxTier;i++){
				Log(i+" "+CurrentTower.Name);
                Button button=buttons[i];
				Log(1);
                UpgradeModel upgrade=CurrentTower.UpgradeModels[i];
				Log(2);
                string iconAsset=upgrade.icon.guidRef.Split('[')[1];
				Log(3);
                iconAsset=iconAsset.Remove(iconAsset.Length-1);
                Texture2D texture=LoadAsset<Texture2D>(iconAsset,CurrentTower.LoadedBundle).Cast<Texture2D>();
                button.image.sprite=Sprite.Create(texture,new(0,0,texture.width,texture.height),new());
                button.GetComponentInChildren<Il2CppTMPro.TextMeshProUGUI>().text=upgrade.name;
                UpgradeMenuButton upgradeButton=button.gameObject.AddComponent<UpgradeMenuButton>();
                upgradeButton.Cost=upgrade.cost;
                upgradeButton.Description=LocManager.GetText(upgrade.name+" Description");
				Log(4);
				Log(CurrentTower.TowerModels[i+1].portrait.guidRef);
				string thing=CurrentTower.TowerModels[i+1].portrait.guidRef;
				Log(5);
                string portraitAsset=thing.Split('[')[1];
				Log(6);
                portraitAsset=portraitAsset.Remove(portraitAsset.Length-1);
                upgradeButton.PortraitAsset=portraitAsset;
                upgradeButton.Menu=this;
                button.gameObject.SetActive(true);
				Log(7);
            }
        }
        public static System.Collections.IEnumerator Show(UpgradeMenuCom menu){
            Image background=menu.Background;
            while(menu.Images[0].color.a<1){
                if(background.color.a<1){
                    background.color=new(255,255,255,background.color.a+0.25f);
                }
                foreach(Image image in menu.Images){
                    image.color=new(255,255,255,image.color.a+0.025f);
                }
                foreach(Il2CppTMPro.TextMeshProUGUI text in menu.Texts){
                    text.color=new(255,255,255,text.color.a+0.025f);
                }
                yield return new WaitForSeconds(0.001f);
            }
        }
        public static System.Collections.IEnumerator Hide(UpgradeMenuCom menu){
            Image background=menu.Background;
            while(menu.Images[0].color.a>0){
                if(background.color.a>0){
                    background.color=new(255,255,255,background.color.a-0.025f);
                }
                foreach(Image image in menu.Images){
                    image.color=new(255,255,255,image.color.a-0.025f);
                }
                foreach(Il2CppTMPro.TextMeshProUGUI text in menu.Texts){
                    text.color=new(255,255,255,text.color.a-0.025f);
                }
                yield return new WaitForSeconds(0.001f);
            }
            SceneManager.UnloadScene(menu.Background.transform.SceneName());
        }
    }
}