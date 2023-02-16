namespace SC2ExpansionLoader{
    public class SC2Tower{
        public virtual string Name=>"";
        public virtual byte[]TowerBundle=>null;
        public virtual ShopTowerDetailsModel ShopDetails()=>null;
        public virtual TowerModel[]GenerateTowerModels()=>null;
        public TowerModel[]TowerModels;
        public virtual int MaxTier=>0;
        public virtual string Description=>"";
        public virtual UpgradeModel[]GenerateUpgradeModels()=>null;
        public UpgradeModel[]UpgradeModels;
        public AssetBundle LoadedBundle=null;
        public virtual void Attack(Weapon weapon){}
        public virtual void Upgrade(int tier,Tower tower){}
        public virtual void Ability(string ability,Tower tower){}
        public virtual void Create(Tower tower){}
        public virtual void Select(Tower tower){}
        public virtual void Sell(Tower tower){}
        public virtual void RoundStart(){}
        public virtual void RoundEnd(){}
        public virtual int MaxSelectQuote=>0;
        public virtual int MaxUpgradeQuote=>0;
        public virtual Dictionary<string,string>SoundNames=>null;
        public virtual Dictionary<string,Il2CppSystem.Type>Behaviours=>new();
        public virtual bool AddToShop=>true;
        public virtual bool Upgradable=>true;
        public virtual string UpgradeScreenSound=>null;
        public enum Faction{
            Protoss,
            Terran,
            Zerg,
            NotSet
        }
        public virtual Faction TowerFaction=>Faction.NotSet;
    }
    [RegisterTypeInIl2Cpp]
    public class SC2Sound:MonoBehaviour{
        public SC2Sound(IntPtr ptr):base(ptr){}
        public int LastSelectQuote=0;
        public int LastUpgradeQuote=0;
        public int MaxSelectQuote;
        public int MaxUpgradeQuote;
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
