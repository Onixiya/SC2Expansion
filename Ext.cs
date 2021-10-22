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
        public static float ModVolume{
            get;
            set;
        }
        public static bool ModHelperLoaded{
            get;
            set;
        }
        public class Settings{
            public static bool ProtossEnabled{
                get;
                set;
            }
            public static bool TerranEnabled{
                get;
                set;
            }
            public static bool ZergEnabled{
                get;
                set;
            }
            public static bool RemoveBaseTowers{
                get;
                set;
            }
            public static bool HeroesEnabled{
                get;
                set;
            }
        }
        public static void LoadSettings(){
            if(File.Exists(MelonHandler.ModsDirectory+"\\SC2Expansion.json")){
                JsonConvert.DeserializeObject<Settings>(File.ReadAllText(MelonHandler.ModsDirectory+"\\SC2ExpansionSettings.json"));
            }else{
                MelonLogger.Msg("Settings does not exist, creating");
            }
        }
    }
}