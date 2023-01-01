namespace SC2ExpansionLoader{
    public class SC2Tower{
        public virtual string Name=>"";
        public virtual byte[]TowerBundle=>null;
        public virtual ShopTowerDetailsModel ShopDetails()=>null;
        public virtual TowerModel[]TowerModels()=>null;
        public virtual UpgradeModel[]Upgrades()=>null;
        public AssetBundle LoadedBundle=null;
        public virtual void Attack(Weapon weapon){}
        public virtual void Upgrade(int tier,Tower tower){}
        public virtual void Ability(string ability,Tower tower){}
        public virtual void Create(){}
        public virtual void Select(Tower tower){}
        public virtual void Sell(Tower tower){}
        public virtual Dictionary<string,string>SoundNames=>null;
        public virtual Dictionary<string,Il2CppSystem.Type>Behaviours=>new();
    }
    [RegisterTypeInIl2Cpp]
    public class SC2Sound:MonoBehaviour{
        public SC2Sound(IntPtr ptr):base(ptr){}
        public int LastSelectQuote=0;
        public int LastUpgradeQuote=0;
        public int MaxSelectQuote=7;
        public int MaxUpgradeQuote=5;
        public string TowerName;
        public void Start(){
            string[]name=gameObject.name.Split('-');
            try{
                TowerName=TowerTypes[name[0]].SoundNames[name[1]];
            }catch{}
        }
        public void PlaySelectSound(){
            LastSelectQuote++;
            if(LastSelectQuote==MaxSelectQuote){
                LastSelectQuote=1;
            }
            PlaySound(TowerName+"Select"+LastSelectQuote);
        }
        public void PlayUpgradeSound(){
            LastUpgradeQuote++;
            if(LastUpgradeQuote==MaxUpgradeQuote){
                LastUpgradeQuote=1;
            }
            LastSelectQuote=1;
            PlaySound(TowerName+"Upgrade"+LastUpgradeQuote);
        }
    }
}
