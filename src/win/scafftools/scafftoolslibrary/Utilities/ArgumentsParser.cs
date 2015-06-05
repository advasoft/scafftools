

namespace scafftools.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ArgumentsParser
	{
		public static T ParseArguments<T>(string[] args) where T : class
		{
			Type resultType = typeof(T);
            Dictionary<string, Tuple<string, bool, bool, bool, Type>> props = GetObjectProps(resultType);
			T result = Activator.CreateInstance<T>();

			int argLength = args.Length;
			for (int i = 0; i < argLength;)
			{
				var arg = args[i].Trim();
				if (arg.StartsWith("-"))
				{
					var preparedArgs = arg.Replace("-", "");
					if (!props.Keys.Contains(preparedArgs))
						throw new ConsoleArgumentsParseException(string.Format("invalid argument key -{0}", preparedArgs));

					var prop = props[preparedArgs];
					if (prop.Item3) //it's a flag
					{
                        //resultType.InvokeMember(prop.Item1, BindingFlags.SetProperty | BindingFlags.Instance,
                        //    null, result, new object[] { true });
                        var property = resultType.GetProperty(prop.Item1, BindingFlags.Public | BindingFlags.Instance);
                        property.SetValue(result, true);

						i++;
					}
					else //it's not a flag
					{
						var parameterValue = args[i+1].Trim();
						if (prop.Item4)	//is enum
						{
							var enumValue = Enum.Parse(prop.Item5, parameterValue);
						    var property = resultType.GetProperty(prop.Item1, BindingFlags.Public | BindingFlags.Instance);
                            property.SetValue(result, enumValue);
                            //resultType.InvokeMember(prop.Item1, BindingFlags.SetProperty | BindingFlags.Instance,
                            //    null, result, new object[] { enumValue });

						}
						else
						{
                            var property = resultType.GetProperty(prop.Item1, BindingFlags.Public | BindingFlags.Instance);
                            property.SetValue(result, parameterValue);

                            //resultType.InvokeMember(prop.Item1, BindingFlags.SetProperty | BindingFlags.Instance,
                            //    null, result, new object[] { parameterValue });
						}
						i += 2;
					}
				}
				else
				{
					i++;
				}
			}

			return result;
        }

		private static Dictionary<string, Tuple<string, bool, bool, bool, Type>> GetObjectProps(Type type)
		{
			Dictionary<string, Tuple<string, bool, bool, bool, Type>> result = new Dictionary<string, Tuple<string, bool, bool, bool, Type>>();

			Type enumType = default(Type);

			var properties = type.GetProperties();
			foreach (PropertyInfo property in properties)
			{
				if (property.PropertyType.IsEnum)
					enumType = property.PropertyType;

                var attribute = property.GetCustomAttribute<OptionAttribute>();
				if (attribute != null)
				{
					result.Add(attribute.OptionName, 
						new Tuple<string, bool, bool, bool, Type>(property.Name, attribute.Required, attribute.IsFlag, property.PropertyType.IsEnum, enumType));

                }
            }

			return result;
        }
    }
}
