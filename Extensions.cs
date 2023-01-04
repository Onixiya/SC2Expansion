namespace SC2ExpansionLoader{
    public static class ModelArrayExtensions{
        public static T GetModel<T>(this Il2CppReferenceArray<Model>array)where T:Model{
            try{
                return array.First(a=>a.GetIl2CppType()==Il2CppType.Of<T>()).Cast<T>();
            }catch(Exception error){
                Log("Failed to get model type from array");
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
                Log("Failed to get model "+modelName+" from array");
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
                Log("Failed to get model type from list");
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
                Log("Failed to get model "+modelName+" from list");
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