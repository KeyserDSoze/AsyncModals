using Microsoft.AspNetCore.Html;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AsyncModals.Code
{
    public class NoPropertyBindingAttribute : Attribute
    {
    }
    public class SecondaryBindingAttribute : Attribute
    {

    }
    public class ModelBinder
    {
        private static Dictionary<string, List<ModelBinder>> Bindings = new Dictionary<string, List<ModelBinder>>();
        private static readonly object TrafficLight = new object();
        private static readonly Type Ignore = typeof(NoPropertyBindingAttribute);
        private static readonly Type Secondary = typeof(SecondaryBindingAttribute);
        private const string Spacer = "<div>{0}</div>";
        public static List<ModelBinder> Instance(dynamic model)
        {
            Type[] type = (Type[])model.GetType().GenericTypeArguments;
            if (type.Length == 1)
            {
                Type realType = type[0];
                if (!Bindings.ContainsKey(realType.FullName))
                {
                    lock (TrafficLight)
                    {
                        if (!Bindings.ContainsKey(realType.FullName))
                        {
                            List<ModelBinder> modelBinders = new List<ModelBinder>();
                            foreach (PropertyInfo propertyInfo in realType.GetProperties())
                                if (propertyInfo.GetCustomAttribute(Ignore) == null)
                                    modelBinders.Add(new ModelBinder() { DisplayName = propertyInfo.Name, PropertyInfo = propertyInfo });
                            Bindings.Add(realType.FullName, modelBinders);
                        }
                    }
                }
                return Bindings[realType.FullName];
            }
            else
                return null;
        }
        public string DisplayName { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        private static Dictionary<string, ModelBinder> Secondaries = new Dictionary<string, ModelBinder>();
        public IHtmlContent GetValue(dynamic value)
        {
            dynamic valueOfObject = this.PropertyInfo.GetValue(value);
            if (value == null)
                return new HtmlString(string.Empty);
            if (!(valueOfObject is string) && valueOfObject is IEnumerable)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (dynamic singleValue in valueOfObject)
                {
                    Type singleValueType = singleValue.GetType();
                    if (CheckPrimitiveList(singleValueType))
                        stringBuilder.Append(string.Format(Spacer, singleValue.ToString()));
                    else
                    {
                        if (!Secondaries.ContainsKey(singleValueType.FullName))
                        {
                            lock (TrafficLight)
                            {
                                if (!Secondaries.ContainsKey(singleValueType.FullName))
                                {
                                    PropertyInfo[] propertyInfos = singleValueType.GetProperties();
                                    if (propertyInfos.Length == 0)
                                        continue;
                                    foreach (PropertyInfo propertyInfo in propertyInfos)
                                    {
                                        if (propertyInfo.GetCustomAttribute(Secondary) != null && !Secondaries.ContainsKey(singleValueType.FullName))
                                            Secondaries.Add(singleValueType.FullName, new ModelBinder() { DisplayName = propertyInfo.Name, PropertyInfo = propertyInfo });
                                    }
                                    if (!Secondaries.ContainsKey(singleValueType.FullName))
                                        Secondaries.Add(singleValueType.FullName, new ModelBinder() { DisplayName = propertyInfos[0].Name, PropertyInfo = propertyInfos[0] });
                                }
                            }
                        }
                        stringBuilder.Append(string.Format(Spacer, Secondaries[singleValueType.FullName].GetValue(singleValue).ToString()));
                    }
                }
                return new HtmlString(stringBuilder.ToString());
            }
            return new HtmlString(valueOfObject.ToString());
        }
        private static bool CheckPrimitiveList(Type typeR)
        {
            foreach (Type type in NormalTypes)
                if (type == typeR) return true;
            return false;
        }
        private static readonly List<Type> NormalTypes = new List<Type>
        {
            typeof(int),
            typeof(bool),
            typeof(char),
            typeof(decimal),
            typeof(double),
            typeof(long),
            typeof(byte),
            typeof(sbyte),
            typeof(float),
            typeof(uint),
            typeof(ulong),
            typeof(short),
            typeof(ushort),
            typeof(string)
        };
    }
}
