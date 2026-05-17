using Il2CppAssets.Scripts;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.StoreMenu;

namespace SC2Expansion{
    public class SC2Tower:SC2Content{
        public virtual ShopTowerDetailsModel ShopDetails()=>null;
		public virtual HeroDetailsModel GenerateHeroDetails()=>null;
		public HeroDetailsModel HeroDetails=null;
        public virtual TowerModel[]GenerateTowerModels()=>null;
        public TowerModel[]TowerModels=null;
        public virtual int MaxTier=>0;
        public virtual UpgradeModel[]GenerateUpgradeModels()=>null;
        public UpgradeModel[]UpgradeModels=null;
        public virtual void Attack(Weapon weapon){}
        public virtual void Upgrade(int tier,Tower tower){}
        public virtual bool Ability(string ability,Tower tower){
			return true;
		}
        public virtual void Create(TowerManager manager,TowerModel model,Il2CppAssets.Scripts.Simulation.SMath.Vector3 position,int inputIndex,
            ObjectId areaPlacedOn,ObjectId parentTowerId){}
		public virtual SkinData HeroSkin()=>null;
        public virtual void Select(Tower tower){}
        public virtual void Sell(Tower tower){}
        public virtual void RoundStart(){}
        public virtual void RoundEnd(){}
        public virtual bool AddToShop=>true;
        public virtual bool Upgradable=>true;
        public virtual string UpgradeScreenSound=>null;
		public virtual bool ShowUpgradeMenu=>true;
		public virtual bool Hero=>false;
        public enum Faction{
            Protoss,
            Terran,
            Zerg,
			Misc,
            NotSet
        }
        public virtual Faction TowerFaction=>Faction.NotSet;
        public virtual bool AttackTargetFilter(Attack attack)=>true;
        public virtual bool PreAttack(Weapon weapon)=>true;
        public virtual Dictionary<string,string>Text=>new();
        public bool Loaded=false;
        public List<Tower>SimTowers=new();
        public virtual Dictionary<string,Il2CppSystem.Type>Beams=>new();
        public virtual Dictionary<string,Tuple<string,int>>SpritePrefabLayers=>new();
        public virtual void TowerButton(TowerPurchaseButton button,TowerModel model){}
    }
}