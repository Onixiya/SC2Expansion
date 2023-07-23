namespace SC2ExpansionLoader{
    public static class ModelArrayExtensions{
        public static T GetModel<T>(this Il2CppReferenceArray<Model>array)where T:Model{
            try{
                return array.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).Cast<T>();
            }catch(Exception error){
                PrintError(error,"Failed to get model "+Il2CppType.Of<T>().Name+" from array");
                return null;
            }
        }
        public static T GetModel<T>(this Il2CppReferenceArray<Model>array,string modelName)where T:Model{
            try{
                return array.First(a=>a.name.Contains(modelName)).Cast<T>();
            }catch(Exception error){
                PrintError(error,"Failed to get model "+modelName+" with type "+Il2CppType.Of<T>().Name+" from array");
                return null;
            }
        }
        public static Il2CppReferenceArray<T>GetModels<T>(this Il2CppReferenceArray<Model>array)where T:Model{
            try{
				Model[]models=array.Where(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).ToArray();
				Il2CppReferenceArray<T>returnArray=new Il2CppReferenceArray<T>(models.Count());
				for(int i=0;i<models.Count();i++){
					returnArray[i]=models[i].Cast<T>();
				}
				return returnArray;
            }catch(Exception error){
                PrintError(error,"Failed to get models "+Il2CppType.Of<T>().Name+" from array");
                return null;
            }
        }
    }
    public static class ModelListExtensions{
        public static T GetModel<T>(this List<Model>list)where T:Model{
            try{
                return list.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).Cast<T>();
            }catch(Exception error){
                PrintError(error,"Failed to get model "+Il2CppType.Of<T>().Name+" from list");
                return null;
            }
        }
		public static void RemoveModel<T>(this List<Model>list)where T:Model{
            try{
                list.Remove(list.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()));
            }catch(Exception error){
                PrintError(error,"Failed to remove model "+Il2CppType.Of<T>().Name+" from list");
                return;
            }
        }
		public static void RemoveModel(this List<Model>list,string modelName){
            try{
                list.Remove(list.First(a=>a.name.Contains(modelName)));
            }catch(Exception error){
                PrintError(error,"Failed to remove model "+modelName+" from list");
                return;
            }
        }
        public static T GetModel<T>(this List<Model>list,string modelName)where T:Model{
            try{
                return list.First(a=>a.name.Contains(modelName)).Cast<T>();
            }catch(Exception error){
                PrintError(error,"Failed to get model "+modelName+" with type "+Il2CppType.Of<T>().Name+" from list");
                return null;
            }
        }
        public static List<Model>GetModels<T>(this List<Model>list)where T:Model{
            try{
                return list.Where(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).ToList();
            }catch(Exception error){
                PrintError(error,"Failed to get models "+Il2CppType.Of<T>().Name+" from list");
                return null;
            }
        }
    }
    public static class ModelExtensions{
        public static T Clone<T>(this Model model)where T:Model{
            try{
                return model.Clone().Cast<T>();
            }catch(Exception error){
                PrintError(error,"Failed to clone "+Il2CppType.Of<T>().Name);
                return null;
            }
        }
    }
}