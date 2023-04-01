using System;
using System.Collections;
using System.Linq;

namespace AcmeTube.Application.Extensions;

/// <summary>Type extension methods.</summary>
public static class TypeExtensions
{
	/// <summary>
	/// Verify if type is a DateTime.
	/// </summary>
	/// <returns>True if is a Date</returns>
	public static bool IsDate(this Type type) => typeof(DateTime) == type || typeof(DateTime?) == type;

	/// <summary>
	/// Verify if type is a collection.
	/// </summary>
	/// <returns>True if is a collection</returns>
	public static bool IsPropertyACollection(this Type type)
	{
		return (typeof(string) != type && typeof(IEnumerable).IsAssignableFrom(type));
	}

	public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
	{
		var interfaceTypes = givenType.GetInterfaces();

		if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
		{
			return true;
		}

		if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
			return true;

		Type baseType = givenType.BaseType;
		if (baseType == null) return false;

		return IsAssignableToGenericType(baseType, genericType);
	}
}