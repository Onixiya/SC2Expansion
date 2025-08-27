namespace SC2Expansion{
    public class SC2Content{
        public virtual string Name=>"";
        public AssetBundle Bundle=null;
        public virtual string BundleName=>Name.ToLower()+".bundle";
		public virtual bool HasBundle=>true;
        public virtual Dictionary<string,Il2CppSystem.Type>Components=>new();
		public virtual int Order=>0;
    }
}
