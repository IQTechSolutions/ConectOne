using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;

namespace GoogleMapsComponents;

/// <summary>
/// Provides helper methods for JSON serialization and deserialization, enum conversion, and JavaScript interop
/// operations.
/// </summary>
/// <remarks>This static class contains utility methods intended for internal use, including methods to serialize
/// and deserialize objects using custom JSON options, convert strings to enum values, and facilitate advanced
/// JavaScript interop scenarios in Blazor applications. The methods are designed to handle common patterns such as
/// discriminated unions and dynamic type resolution when interacting with JavaScript. Thread safety and performance
/// considerations are managed internally; callers should use these methods as building blocks for higher-level
/// functionality.</remarks>
internal static class Helper
{
    /// <summary>
    /// Provides the default options used for JSON serialization and deserialization within this class.
    /// </summary>
    /// <remarks>The options specify that null values are ignored when writing JSON and that property names
    /// use camel case formatting. These settings ensure consistent JSON output and compatibility with common JavaScript
    /// conventions.</remarks>
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// Initializes static members of the Helper class.
    /// </summary>
    /// <remarks>This static constructor configures the default JSON serialization options for the Helper
    /// class by adding custom converters. These converters enable support for serializing and deserializing union types
    /// and enums using camel case naming. The configuration is applied once when the class is first accessed.</remarks>
    static Helper()
    {
        Options.Converters.Add(new OneOfConverterFactory());
        Options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously without returning a value.
    /// </summary>
    /// <remarks>This method is typically used when the JavaScript function does not return a value. If a
    /// return value is expected, use the generic overload instead.</remarks>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/> instance used to invoke the JavaScript function.</param>
    /// <param name="identifier">The identifier of the JavaScript function to invoke. This should be a dot-separated path to the function.</param>
    /// <param name="args">An array of arguments to pass to the JavaScript function. Each argument is serialized before being passed to
    /// JavaScript.</param>
    /// <returns>A task that represents the asynchronous JavaScript invocation operation.</returns>
    internal static Task MyInvokeAsync(
        this IJSRuntime jsRuntime,
        string identifier,
        params object?[] args)
    {
        return jsRuntime.MyInvokeAsync<object>(identifier, args);
    }

    /// <summary>
    /// Converts the specified string to a nullable enumeration value of type T.
    /// </summary>
    /// <remarks>If the input string is an integer, it is parsed as the underlying value of the enumeration.
    /// If the string matches the value of an EnumMemberAttribute applied to an enumeration member, that member is
    /// returned. If the string is "null", the method returns null. If the string does not match any valid enumeration
    /// value, the method returns the default value for the enumeration type.</remarks>
    /// <typeparam name="T">The enumeration type to convert the string to. Must be a value type that represents an enumeration.</typeparam>
    /// <param name="str">The string representation of the enumeration value to convert. This can be an integer value, the string value of
    /// an EnumMemberAttribute, or the string "null".</param>
    /// <returns>A nullable enumeration value of type T that corresponds to the specified string, or null if the string is "null"
    /// or does not match any enumeration value.</returns>
    internal static T? ToNullableEnum<T>(string? str)
        where T : struct
    {
        var enumType = typeof(T);

        if (int.TryParse(str, out var enumintValue))
        {
            return (T)Enum.Parse(enumType, enumintValue.ToString());
        }


        if (str == "null")
        {
            return null;
        }

        foreach (var name in Enum.GetNames(enumType))
        {
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            if (enumMemberAttribute.Value == str)
            {
                return (T)Enum.Parse(enumType, name);
            }
        }

        //throw exception or whatever handling you want
        return default;
    }

    /// <summary>
    /// Deserializes the specified JSON element to an object of the given type.
    /// </summary>
    /// <param name="json">The JSON element containing the data to deserialize.</param>
    /// <param name="type">The type of object to create from the JSON data. Cannot be null.</param>
    /// <returns>An object representing the deserialized JSON data, or null if the JSON element is null or empty.</returns>
    public static object? DeSerializeObject(JsonElement json, Type type)
    {
        var obj = json.Deserialize(type, Options);
        return obj;
    }

    /// <summary>
    /// Deserializes the specified JSON string to an object of the given type.
    /// </summary>
    /// <param name="json">The JSON string to deserialize. Cannot be null or empty.</param>
    /// <param name="type">The type of the object to create from the JSON string. Cannot be null.</param>
    /// <returns>An object of the specified type deserialized from the JSON string, or null if the JSON is null or empty.</returns>
    public static object? DeSerializeObject(string json, Type type)
    {
        var obj = JsonSerializer.Deserialize(json, type, Options);
        return obj;
    }

    /// <summary>
    /// Deserializes the specified JSON string to an object of type <typeparamref name="TObject"/>.
    /// </summary>
    /// <typeparam name="TObject">The type of the object to deserialize to.</typeparam>
    /// <param name="json">The JSON string to deserialize. Cannot be null.</param>
    /// <returns>An instance of <typeparamref name="TObject"/> deserialized from the JSON string.</returns>
    public static TObject DeSerializeObject<TObject>(string json)
    {
        var value = JsonSerializer.Deserialize<TObject>(json, Options);
        return value;
    }

    /// <summary>
    /// Serializes the specified object to a JSON string.
    /// </summary>
    /// <remarks>The serialization process uses predefined options, which may affect property naming, null
    /// value handling, and formatting. The resulting JSON string does not include type information.</remarks>
    /// <param name="obj">The object to serialize. Can be any serializable .NET object.</param>
    /// <returns>A JSON-formatted string representing the serialized object.</returns>
    public static string SerializeObject(object obj)
    {
        var value = JsonSerializer.Serialize(
            obj,
            Options);
        //Formatting.None,
        //new JsonSerializerSettings
        //{
        //    NullValueHandling = NullValueHandling.Ignore,
        //    ContractResolver = new CamelCasePropertyNamesContractResolver()
        //});

        return value;
    }

    /// <summary>
    /// Converts a collection of .NET arguments into a format suitable for JavaScript interop calls.
    /// </summary>
    /// <remarks>This method handles common .NET types, delegates, and custom interop types, converting them
    /// as necessary for use in JavaScript interop scenarios. Delegates are wrapped in object references to enable
    /// invocation from JavaScript. Complex objects are serialized when required.</remarks>
    /// <param name="jsRuntime">The JavaScript runtime instance used to create object references for delegates or actions.</param>
    /// <param name="args">The collection of arguments to be converted for JavaScript compatibility. Elements may be null or of various
    /// types.</param>
    /// <returns>An enumerable collection of objects that are compatible with JavaScript interop. Each element is transformed as
    /// needed to ensure it can be passed to JavaScript.</returns>
    private static IEnumerable<object> MakeArgJsFriendly(IJSRuntime jsRuntime, IEnumerable<object?> args)
    {
        var jsFriendlyArgs = args
            .Select(arg =>
            {
                if (arg == null)
                {
                    return arg;
                }

                if (arg is IOneOf oneof)
                {
                    arg = oneof.Value;
                }

                var argType = arg.GetType();

                switch (arg)
                {
                    case ElementReference _:
                    case string _:
                    case int _:
                    case long _:
                    case double _:
                    case float _:
                    case decimal _:
                    case DateTime _:
                    case bool _:
                        return arg;
                    case Action action:
                        return DotNetObjectReference.Create(new JsCallableAction(jsRuntime, action));
                    default:
                    {
                        if (argType.IsGenericType
                            && (argType.GetGenericTypeDefinition() == typeof(Action<>)))
                        {
                            var genericArguments = argType.GetGenericArguments();

                            //Debug.WriteLine($"Generic args : {genericArguments.Count()}");

                            return DotNetObjectReference.Create(new JsCallableAction(jsRuntime, (Delegate)arg, genericArguments));
                        }

                        switch (arg)
                        {
                            case JsCallableAction _:
                                return DotNetObjectReference.Create(arg);
                            case IJsObjectRef jsObjectRef:
                            {
                                //Debug.WriteLine("Serialize IJsObjectRef");

                                var guid = jsObjectRef.Guid;
                                return SerializeObject(new JsObjectRef1(guid));
                            }
                            default:
                                return SerializeObject(arg);
                        }
                    }
                }
            });
        return jsFriendlyArgs;
    }

    /// <summary>
    /// Invokes a JavaScript function asynchronously and returns the result as the specified type.
    /// </summary>
    /// <remarks>If TRes implements IJsObjectRef, the method returns a proxy object representing a JavaScript
    /// object reference. If TRes implements IOneOf, the method attempts to deserialize the result into the appropriate
    /// union type. For other types, the result is deserialized directly. This method is intended for internal use and
    /// may not perform all runtime checks present in public APIs.</remarks>
    /// <typeparam name="TRes">The type of the result expected from the JavaScript function. This can be a plain value, an implementation of
    /// IJsObjectRef, or IOneOf for union types.</typeparam>
    /// <param name="jsRuntime">The JavaScript runtime instance used to perform the invocation. Cannot be null.</param>
    /// <param name="identifier">The identifier of the JavaScript function to invoke. This should be a fully qualified function name.</param>
    /// <param name="args">The arguments to pass to the JavaScript function. Arguments are converted to a JavaScript-friendly format as
    /// needed.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the value returned by the JavaScript
    /// function, converted to the specified type.</returns>
    internal static async Task<TRes> MyInvokeAsync<TRes>(
        this IJSRuntime jsRuntime,
        string identifier,
        params object?[] args)
    {
        var jsFriendlyArgs = MakeArgJsFriendly(jsRuntime, args);

        if (typeof(IJsObjectRef).IsAssignableFrom(typeof(TRes)))
        {
            var guid = await jsRuntime.InvokeAsync<string>(identifier, jsFriendlyArgs);

            return guid == null ? default : (TRes)JsObjectRefInstances.GetInstance(guid);
        }

        if (typeof(IOneOf).IsAssignableFrom(typeof(TRes)))
        {
            var resultObject = await jsRuntime.InvokeAsync<string>(identifier, jsFriendlyArgs);
            object? result = null;

            if (resultObject is string someText)
            {
                try
                {
                    var jo = JsonDocument.Parse(someText);
                    var typeToken = jo.RootElement.GetProperty("dotnetTypeName").GetString();
                    if (typeToken != null)
                    {
                        result = DeSerializeObject<TRes>(typeToken);
                        //var typeName = typeToken.Value<string>();
                        //var asm = typeof(Map).Assembly;
                        //var type = asm.GetType(typeName);
                        //result = jo.ToObject(type);
                    }
                    else
                    {
                        result = someText;
                    }
                    //var jo = JsonNode.Parse(someText);
                    //var typeToken = jo.SelectToken("dotnetTypeName");
                    //if (typeToken != null)
                    //{
                    //    var typeName = typeToken.Value<string>();
                    //    var asm = typeof(Map).Assembly;
                    //    var type = asm.GetType(typeName);
                    //    result = jo.ToObject(type);
                    //}
                    //else
                    //{
                    //    result = someText;
                    //}
                }
                catch
                {
                    result = someText;
                }
            }

            return (TRes)result;
        }
        else
        {
            return await jsRuntime.InvokeAsync<TRes>(identifier, jsFriendlyArgs);
        }
    }

    /// <summary>
    /// Invokes a JavaScript function asynchronously using the specified identifier and arguments.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript runtime instance used to invoke the function.</param>
    /// <param name="identifier">The identifier of the JavaScript function to invoke. This should match the name or path of the function in the
    /// JavaScript context.</param>
    /// <param name="args">The arguments to pass to the JavaScript function. Each argument will be converted to a JavaScript-friendly
    /// format as needed.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the value returned by the JavaScript
    /// function.</returns>
    internal static async Task<object> MyAddListenerAsync(
        this IJSRuntime jsRuntime,
        string identifier,
        params object[] args)
    {

        var jsFriendlyArgs = MakeArgJsFriendly(jsRuntime, args);

        return await jsRuntime.InvokeAsync<object>(identifier, jsFriendlyArgs);
    }

    /// <summary>
    /// Invokes a JavaScript function asynchronously and processes the result, supporting custom deserialization based
    /// on a type name returned from JavaScript.
    /// </summary>
    /// <remarks>If the JavaScript function returns a JSON string containing a 'dotnetTypeName' property, the
    /// result is deserialized to the specified .NET type. Otherwise, the raw string or object is returned. This method
    /// is useful for scenarios where JavaScript interop needs to return polymorphic or dynamically-typed
    /// results.</remarks>
    /// <param name="jsRuntime">The JavaScript runtime instance used to invoke the JavaScript function.</param>
    /// <param name="identifier">The identifier of the JavaScript function to invoke.</param>
    /// <param name="args">An array of arguments to pass to the JavaScript function.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized object if a type
    /// name is provided by the JavaScript function; otherwise, the raw result as a string or object.</returns>
    private static async Task<object> InvokeAsync(
        this IJSRuntime jsRuntime,
        string identifier,
        params object[] args)
    {
        var resultObject = await jsRuntime.MyInvokeAsync<object>(identifier, args);
        object result = null;

        if (resultObject is string someText)
        {
            try
            {
                //var jo = JObject.Parse(someText);
                //var typeToken = jo.SelectToken("dotnetTypeName");
                var jo = JsonDocument.Parse(someText);
                var typeToken = jo.RootElement.GetProperty("dotnetTypeName").GetString();
                if (typeToken != null)
                {
                    result = DeSerializeObject<object>(typeToken);
                    //var typeName = typeToken.Value<string>();
                    //var asm = typeof(Map).Assembly;
                    //var type = asm.GetType(typeName);
                    //result = jo.ToObject(type);
                }
                else
                {
                    result = someText;
                }
            }
            catch
            {
                result = someText;
            }
        }

        if (resultObject is JsonElement jsonElement)
        {


        }

        return result;
    }

    /// <summary>
    /// For use when returned result will be one of multiple type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="jsRuntime"></param>
    /// <param name="identifier"></param>
    /// <param name="args"></param>
    /// <returns>Discriminated union of specified types</returns>
    internal static async Task<OneOf<T, U>> MyInvokeAsync<T, U>(
        this IJSRuntime jsRuntime,
        string identifier,
        params object[] args)
    {
        var resultObject = await jsRuntime.MyInvokeAsync<object>(identifier, args);
        object? result = null;

        if (resultObject is JsonElement jsonElement)
        {
            var json = jsonElement.GetString();
            var propArray = Helper.DeSerializeObject<Dictionary<string, object>>(json);
            if (propArray.TryGetValue("dotnetTypeName", out var typeName))
            {
                var asm = typeof(Map).Assembly;
                var typeNameString = typeName.ToString();
                var type = asm.GetType(typeNameString);
                result = Helper.DeSerializeObject(json, type);
            }
        }

        switch (result)
        {
            case T t:
                return t;
            case U u:
                return u;
            default:
                return default;
        }
    }

    /// <summary>
    /// For use when returned result will be one of multiple type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="jsRuntime"></param>
    /// <param name="identifier"></param>
    /// <param name="args"></param>
    /// <returns>Discriminated union of specified types</returns>
    internal static async Task<OneOf<T, U, V>> MyInvokeAsync<T, U, V>(
        this IJSRuntime jsRuntime,
        string identifier,
        params object[] args)
    {
        var resultObject = await jsRuntime.MyInvokeAsync<object>(identifier, args);
        object? result = null;

        if (resultObject is JsonElement jsonElement)
        {
            var json = jsonElement.GetString();
            var propArray = Helper.DeSerializeObject<Dictionary<string, object>>(json);
            if (propArray.TryGetValue("dotnetTypeName", out var typeName))
            {
                var asm = typeof(Map).Assembly;
                var typeNameString = typeName.ToString();
                var type = asm.GetType(typeNameString);
                result = Helper.DeSerializeObject(json, type);
            }
        }

        switch (result)
        {
            case T t:
                return t;
            case U u:
                return u;
            case V v:
                return v;
            default:
                return default;
        }
    }

    /// <summary>
    /// Converts the specified string to the corresponding value of the enumeration type T, matching either the enum
    /// member's name or its EnumMemberAttribute value.
    /// </summary>
    /// <remarks>The comparison is case-insensitive for enum member names. If the enum member is decorated
    /// with an EnumMemberAttribute, its value is also considered for matching. If the string does not match any enum
    /// member name or EnumMemberAttribute value, the method returns the default value of T.</remarks>
    /// <typeparam name="T">The enumeration type to convert the string to. Must be an enum type.</typeparam>
    /// <param name="str">The string representation of the enumeration value. This can be either the enum member's name or the value
    /// specified in its EnumMemberAttribute.</param>
    /// <returns>The enumeration value of type T that corresponds to the specified string. Returns the default value of T if no
    /// match is found.</returns>
    internal static T ToEnum<T>(string str)
    {
        var enumType = typeof(T);
        foreach (var name in Enum.GetNames(enumType))
        {
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            if (enumMemberAttribute.Value == str)
            {
                return (T)Enum.Parse(enumType, name);
            }

            if (string.Equals(name, str, StringComparison.InvariantCultureIgnoreCase))
            {
                return (T)Enum.Parse(enumType, name);
            }
        }

        //throw exception or whatever handling you want
        return default;
    }
}