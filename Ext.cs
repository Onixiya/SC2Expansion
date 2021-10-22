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
        private static float modvolume;
        public static float ModVolume
        {
            get{return modvolume;}
            set{modvolume=value;}
        }
        private static bool modhelperloaded;
        public static bool ModHelperLoaded
        {
            get{return modhelperloaded;}
            set{modhelperloaded=value;}
        }
        private static bool readsettings;
        public static bool ReadSettings
        {
            get{return readsettings;}
            set{readsettings=value;}
        }
        //this is the json settings code which i was working on til i decided to look at mod helpers settings menu thing, i'll keep it here in case it might be useful for another project or something
        //but for now, its useless
        /*public static T LoadFromFile<T>(string filePath)where T:class{
            string json=File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static void SaveToFile<T>(T jsonObject,string savePath,bool overwriteExisting=true)where T:class{
            CreateDirIfNotFound(savePath);
            string json=JsonConvert.SerializeObject(jsonObject,Formatting.Indented);
            bool keepOriginal=!overwriteExisting;
            StreamWriter serialize=new(savePath,keepOriginal);
            serialize.Write(json);
            serialize.Close();
        }
        private static void CreateDirIfNotFound(string dir) {
            FileInfo f=new(dir);
            Directory.CreateDirectory(f.Directory.FullName);
        }
        internal class Settings{
            public static string settingsFilePath="Mods/SC2Expansion/Settings.json";
            private static Settings settings;
            public bool ProtossEnabled{get;set;}=true;
            public bool TerranEnabled{get;set;}=true;
            public bool ZergEnabled{get;set;}=true;
            public bool HeroesEnabled{get;set;}=true;
            public static Settings LoadedSettings{
                get{
                    if(settings is null)settings=Load();
                    return settings;
                }set{
                    settings=value;
                }
            }
            private static Settings Load(){
                return File.Exists(settingsFilePath)?LoadFromFile<Settings>(settingsFilePath):new Settings();
            }
        }*/
    }
}