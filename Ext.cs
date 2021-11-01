namespace SC2Expansion.Utils{
    public static class Ext{
        public static Il2CppReferenceArray<T>Remove<T>(this Il2CppReferenceArray<T>reference,Func<T,bool>predicate)where T:Model{
            List<T>bases=new List<T>();
            foreach(var tmp in reference)if(!predicate(tmp))bases.Add(tmp);
            return new(bases.ToArray());
        }
        public static Il2CppReferenceArray<T>Add<T>(this Il2CppReferenceArray<T>reference,params T[]newPart)where T:Model{
            var bases=new List<T>();
            foreach(var tmp in reference)bases.Add(tmp);
            foreach(var tmp in newPart)bases.Add(tmp);
            return new(bases.ToArray());
        }
        public static float ModVolume{get;set;}
        public static bool ProtossEnabled{get;set;}
        public static bool TerranEnabled{get;set;}
        public static bool ZergEnabled{get;set;}
        public static bool RemoveBaseTowers{get;set;}
        public static bool HeroesEnabled{get;set;}
    }
    public class ProtossSet:ModTowerSet{
        public override string DisplayName=>"Protoss";
        public override string Container=>"ProtossContainer";
        public override string Button=>"ProtossButton";
        public override string ContainerLarge=>Container;
        public override string Portrait=>"ProtossHex";
    }
    public class TerranSet:ModTowerSet{
        public override string DisplayName=>"Terran";
        public override string Container=>"TerranContainer";
        public override string Button=>"TerranButton";
        public override string ContainerLarge=>Container;
        public override string Portrait=>"TerranPortrait";
    }
    public class ZergSet:ModTowerSet{
        public override string DisplayName=>"Zerg";
        public override string Container=>"ZergContainer";
        public override string Button=>"ZergButton";
        public override string ContainerLarge=>Container;
        public override string Portrait=>"ZergCreep";
    }
}