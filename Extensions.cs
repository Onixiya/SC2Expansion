namespace SC2ExpansionLoader{
    public static class ModelArrayExtensions{
        public static T GetModel<T>(this Il2CppReferenceArray<Model>array)where T:Model{
            try{
                return array.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).Cast<T>();
            }catch(Exception error){
                Log("Failed to get model "+Il2CppType.Of<T>().Name+" from array");
                string message=error.Message;
                message+="@\n"+error.StackTrace;
                Log(message,"error");
                return null;
            }
        }
        public static T GetModel<T>(this Il2CppReferenceArray<Model>array,string modelName)where T:Model{
            try{
                return array.First(a=>a.name.Contains(modelName)).Cast<T>();
            }catch(Exception error){
                Log("Failed to get model "+modelName+" with type "+Il2CppType.Of<T>().Name+" from array");
                string message=error.Message;
                message+="@\n"+error.StackTrace;
                Log(message,"error");
                return null;
            }
        }
        public static Il2CppReferenceArray<Model>GetModels<T>(this Il2CppReferenceArray<Model>array)where T:Model{
            try{
                return array.Where(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).ToArray();
            }catch(Exception error){
                Log("Failed to get models "+Il2CppType.Of<T>().Name+" from array");
                string message=error.Message;
                message+="@\n"+error.StackTrace;
                Log(message,"error");
                return null;
            }
        }
    }
    public static class ModelListExtensions{
        public static T GetModel<T>(this List<Model>list)where T:Model{
            try{
                return list.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).Cast<T>();
            }catch(Exception error){
                Log("Failed to get model "+Il2CppType.Of<T>().Name+" from list");
                string message=error.Message;
                message+="@\n"+error.StackTrace;
                Log(message,"error");
                return null;
            }
        }
        public static T GetModel<T>(this List<Model>list,string modelName)where T:Model{
            try{
                return list.First(a=>a.name.Contains(modelName)).Cast<T>();
            }catch(Exception error){
                Log("Failed to get model "+modelName+" with type "+Il2CppType.Of<T>().Name+" from list");
                string message=error.Message;
                message+="@\n"+error.StackTrace;
                Log(message,"error");
                return null;
            }
        }
        public static List<Model>GetModels<T>(this List<Model>list)where T:Model{
            try{
                return list.Where(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).ToList();
            }catch(Exception error){
                Log("Failed to get models "+Il2CppType.Of<T>().Name+" from list");
                string message=error.Message;
                message+="@\n"+error.StackTrace;
                Log(message,"error");
                return null;
            }
        }
    }
    public static class ModelExtensions{
        public static T Clone<T>(this Model model)where T:Model{
            try{
                return model.Clone().Cast<T>();
            }catch(Exception error){
                Log("Failed to clone "+model.name);
                string message=error.Message;
                message+="@\n"+error.StackTrace;
                Log(message,"error");
                return null;
            }
        }
    }
}