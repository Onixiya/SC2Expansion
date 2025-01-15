namespace SC2Expansion{
    public static class ModelArrayExtensions{
        public static T GetModel<T>(this Il2CppReferenceArray<Model>array)where T:Model{
            return array.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).Cast<T>();
        }
        public static T GetModel<T>(this Il2CppReferenceArray<Model>array,string modelName)where T:Model{
            return array.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()&&a.name.Contains(modelName)).Cast<T>();
        }
        public static void WriteIl2CppObjToFile(this Il2CppSystem.Object obj,string path){
            try{
                File.WriteAllText(path,Il2CppNewtonsoft.Json.JsonConvert.SerializeObject(obj,new JsonSerializerSettings(){Formatting=Formatting.Indented}));
                Log("object dumped to "+path);
            }catch(Exception error){
                PrintError(error);
            }
        }
        public static Il2CppReferenceArray<T>GetModels<T>(this Il2CppReferenceArray<Model>array)where T:Model{
			Model[]models=array.Where(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).ToArray();
			Il2CppReferenceArray<T>returnArray=new Il2CppReferenceArray<T>(models.Length);
			for(int i=0;i<models.Length;i++){
				returnArray[i]=models[i].Cast<T>();
			}
			return returnArray;
        }
        public static bool HasModel<T>(this Il2CppReferenceArray<Model>array)where T:Model{
            foreach(Model model in array){
                if(model.GetIl2CppType()==Il2CppType.Of<T>()){
                    return true;
                }
            }
            return false;
        }
        public static T GetModelClone<T>(this Il2CppReferenceArray<Model>array)where T:Model{
            return array.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).Clone<T>();
        }
        public static T GetModelClone<T>(this Il2CppReferenceArray<Model>array,string modelName)where T:Model{
            return array.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()&&a.name.Contains(modelName)).Clone<T>();
        }
    }
    public static class Il2CppListBehaviourExtensions{
        public static T GetBehaviour<T>(this Il2CppSystem.Collections.Generic.List<RootBehavior>list)where T:RootBehavior{
            return list._items.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).Cast<T>();
        }
        public static T GetBehaviour<T>(this Il2CppSystem.Collections.Generic.List<RootBehavior>list,string modelName)where T:RootBehavior{
            return list._items.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()&&a.model.name.Contains(modelName)).Cast<T>();
        }
    }
    public static class ModelListExtensions{
        public static T GetModel<T>(this List<Model>list)where T:Model{
            return list.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).Cast<T>();
        }
		public static void RemoveModel<T>(this List<Model>list)where T:Model{
            list.Remove(list.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()));
        }
		public static void RemoveModel(this List<Model>list,string modelName){
            list.Remove(list.First(a=>a.name.Contains(modelName)));
        }
        public static T GetModel<T>(this List<Model>list,string modelName)where T:Model{
            return list.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()&&a.name.Contains(modelName)).Cast<T>();
        }
        public static List<Model>GetModels<T>(this List<Model>list)where T:Model{
            return list.Where(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).ToList();
        }
        public static T GetModelClone<T>(this List<Model>list)where T:Model{
            return list.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).Clone<T>();
        }
        public static T GetModelClone<T>(this List<Model>list,string modelName)where T:Model{
            return list.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()&&a.name.Contains(modelName)).Clone<T>();
        }
        public static bool HasModel<T>(this List<Model>list)where T:Model{
            foreach(Model model in list){
                if(model.GetIl2CppType()==Il2CppType.Of<T>()){
                    return true;
                }
            }
            return false;
        }
    }
    public static class ModelExtensions{
        public static T Clone<T>(this Model model)where T:Model{
            return model.Clone().Cast<T>();
        }
    }
}