using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;

namespace GoogleMapsComponents.Serialization;

/// <summary>
/// Provides a custom System.Text.Json converter factory for serializing and deserializing OneOf types. Supports OneOf
/// types with two or three generic type parameters, enabling polymorphic JSON serialization and deserialization for
/// these union types.
/// </summary>
/// <remarks>This converter factory enables seamless integration of OneOf types with System.Text.Json
/// serialization. Only OneOf types with two or three generic arguments are currently supported; attempts to use other
/// arities will result in a NotSupportedException. The converter can be registered with
/// JsonSerializerOptions.Converters to enable automatic handling of OneOf types during serialization and
/// deserialization. Thread safety is ensured as the factory and its converters do not maintain mutable shared
/// state.</remarks>
public class OneOfConverterFactory : JsonConverterFactory
{
    /// <summary>
    /// Determines whether the specified type can be converted by this converter.
    /// </summary>
    /// <param name="typeToConvert">The type to check for convertibility. Must not be null.</param>
    /// <returns>true if the specified type implements the IOneOf interface; otherwise, false.</returns>
    public override bool CanConvert(Type typeToConvert) => typeof(IOneOf).IsAssignableFrom(typeToConvert);

    /// <summary>
    /// Creates a custom JSON converter for the specified type using the provided serializer options.
    /// </summary>
    /// <param name="typeToConvert">The type of object to convert. This must be a supported type for which a custom converter can be created.</param>
    /// <param name="options">The serialization options to use when creating the converter. These options influence how the converter
    /// serializes and deserializes JSON.</param>
    /// <returns>A <see cref="JsonConverter"/> instance capable of converting the specified type.</returns>
    /// <exception cref="NotSupportedException">Thrown when a converter cannot be created for the specified <paramref name="typeToConvert"/>.</exception>
    public override JsonConverter CreateConverter(Type? typeToConvert, JsonSerializerOptions options)
    {
        var (oneOfGenericType, converterType) = GetTypes(typeToConvert);
        if (oneOfGenericType is null || converterType is null)
        {
            throw new NotSupportedException($"Cannot convert {typeToConvert}");
        }

        var jsonConverter = (JsonConverter)Activator.CreateInstance(
            converterType.MakeGenericType(oneOfGenericType.GenericTypeArguments),
            BindingFlags.Instance | BindingFlags.Public,
            null,
            new object[] { options },
            null)!;

        return jsonConverter;
    }

    /// <summary>
    /// Finds the supported OneOf generic type and its corresponding JSON converter type for the specified type, if
    /// available.
    /// </summary>
    /// <remarks>Currently, only OneOf types with two or three generic parameters are supported. If the
    /// specified type or its base types do not match these patterns, the method returns null values.</remarks>
    /// <param name="type">The type to inspect for a supported OneOf generic type. Can be null.</param>
    /// <returns>A tuple containing the matching OneOf generic type and its associated JSON converter type. Returns (null, null)
    /// if the specified type does not match a supported OneOf generic type.</returns>
    private static (Type? oneOfGenericType, Type? converterType) GetTypes(Type? type)
    {
        while (type is not null)
        {
            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(OneOf<,>) ||
                    genericTypeDefinition == typeof(OneOf<,>))
                {
                    return (type, typeof(OneOf2JsonConverter<,>));
                }

                if (genericTypeDefinition == typeof(OneOf<,,>) ||
                    genericTypeDefinition == typeof(OneOf<,,>))
                {
                    return (type, typeof(OneOf3JsonConverter<,,>));
                }

                // TODO: Not supported (yet).
                // if (genericTypeDefinition == typeof(OneOfBase<,,,>) ||
                //     genericTypeDefinition == typeof(OneOf<,,,>))
                // {
                //   return (type, typeof(OneOfJson<,,,>));
                // }
                //
                // if (genericTypeDefinition == typeof(OneOfBase<,,,,>) ||
                //     genericTypeDefinition == typeof(OneOf<,,,,>))
                // {
                //   return (type, typeof(OneOfJson<,,,,>));
                // }
                //
                // if (genericTypeDefinition == typeof(OneOfBase<,,,,,>) ||
                //     genericTypeDefinition == typeof(OneOf<,,,,,>))
                // {
                //   return (type, typeof(OneOfJson<,,,,,>));
                // }
                //
                // if (genericTypeDefinition == typeof(OneOfBase<,,,,,,>) ||
                //     genericTypeDefinition == typeof(OneOf<,,,,,,>))
                // {
                //   return (type, typeof(OneOfJson<,,,,,,>));
                // }
                //
                // if (genericTypeDefinition == typeof(OneOfBase<,,,,,,,>) ||
                //     genericTypeDefinition == typeof(OneOf<,,,,,,,>))
                // {
                //   return (type, typeof(OneOfJson<,,,,,,,>));
                // }
                //
                // if (genericTypeDefinition == typeof(OneOfBase<,,,,,,,,>) ||
                //     genericTypeDefinition == typeof(OneOf<,,,,,,,,>))
                // {
                //   return (type, typeof(OneOfJson<,,,,,,,,>));
                // }
            }

            type = type.BaseType;
        }

        return (null, null);
    }

    /// <summary>
    /// Creates an instance of a type implementing the IOneOf interface, deserializing the specified value from a JSON
    /// document based on the provided index.
    /// </summary>
    /// <remarks>The method expects that the oneOfType constructor accepts the index as its first parameter,
    /// followed by the deserialized value at the corresponding position. The caller is responsible for ensuring that
    /// the index and types array are valid and that the oneOfType constructor matches the expected signature.</remarks>
    /// <param name="options">The JsonSerializerOptions to use when deserializing the value from the JSON document. Can be null to use default
    /// options.</param>
    /// <param name="index">The zero-based index indicating which type in the types array to use for deserialization.</param>
    /// <param name="doc">The JsonDocument containing the JSON value to be deserialized.</param>
    /// <param name="oneOfType">The Type representing the concrete implementation of the IOneOf interface to instantiate. Must have a matching
    /// constructor.</param>
    /// <param name="types">An array of possible types that the value can be deserialized into. The type at the specified index is used.</param>
    /// <returns>An instance of IOneOf containing the deserialized value of the type at the specified index.</returns>
    private static IOneOf CreateOneOf(JsonSerializerOptions options,
        int index,
        JsonDocument doc,
        Type oneOfType,
        Type[] types)
    {
        var args = new object[types.Length + 1];
        args[0] = index;
        args[index + 1] = doc.Deserialize(types[index], options);

        var oneOf = Activator.CreateInstance(
            oneOfType,
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            args,
            null
        );

        return (IOneOf)oneOf;
    }

    private const string IndexKey = "$index";

    /// <summary>
    /// Converts OneOf<T0, T1> values to and from JSON using System.Text.Json serialization.
    /// </summary>
    /// <remarks>This converter enables polymorphic serialization and deserialization of OneOf<T0, T1> types,
    /// allowing JSON payloads to represent either of the two possible types. The converter embeds type information in
    /// the JSON to ensure correct deserialization. This class is intended for use with System.Text.Json and is
    /// typically registered via JsonSerializerOptions.</remarks>
    /// <typeparam name="T0">The type of the first possible value in the OneOf union.</typeparam>
    /// <typeparam name="T1">The type of the second possible value in the OneOf union.</typeparam>
    private class OneOf2JsonConverter<T0, T1> : JsonConverter<OneOf<T0, T1>>
    {
        private static readonly Type OneOfType = typeof(OneOf<,>).MakeGenericType(typeof(T0), typeof(T1));
        private static readonly Type[] Types = { typeof(T0), typeof(T1) };

        /// <summary>
        /// Initializes a new instance of the OneOf2JsonConverter class using the specified JSON serializer options.
        /// </summary>
        /// <param name="_">The options to use for customizing JSON serialization and deserialization behavior.</param>
        public OneOf2JsonConverter(JsonSerializerOptions _)
        {
        }

        /// <summary>
        /// Reads and converts the JSON representation of a value into a OneOf<T0, T1> instance.
        /// </summary>
        /// <param name="reader">The reader positioned at the JSON value to read.</param>
        /// <param name="typeToConvert">The type of the object to convert to. Must be assignable to OneOf<T0, T1>.</param>
        /// <param name="options">Options to use for deserialization, such as converters and naming policies.</param>
        /// <returns>A OneOf<T0, T1> instance representing the deserialized value from the JSON input.</returns>
        /// <exception cref="JsonException">Thrown if the JSON does not contain a valid type index or if the type index is not a valid number.</exception>
        public override OneOf<T0, T1> Read(ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            if (!doc.RootElement.TryGetProperty(IndexKey, out var indexElement) ||
                !indexElement.TryGetInt32(out var index) ||
                index is < 0 or > 1)
            {
                throw new JsonException("Cannot not find type index or type index is not a valid number");
            }

            var oneOf = CreateOneOf(options, index, doc, OneOfType, Types);

            return (OneOf<T0, T1>)Activator.CreateInstance(typeToConvert, oneOf);
        }

        /// <summary>
        /// Writes the specified OneOf value as JSON using the provided Utf8JsonWriter.
        /// </summary>
        /// <remarks>If the value is an object, an additional property named "dotnetTypeName" is included
        /// in the output to indicate the runtime type. For non-object values, the value is written as a
        /// string.</remarks>
        /// <param name="writer">The Utf8JsonWriter to which the JSON will be written. Must not be null.</param>
        /// <param name="value">The OneOf value to serialize. Represents a value of either type T0 or T1.</param>
        /// <param name="options">The options to use when serializing the value. May influence formatting and converter behavior.</param>
        public override void Write(Utf8JsonWriter writer,
            OneOf<T0, T1> value,
            JsonSerializerOptions options)
        {
            using var doc = value.Match(
                t0 => JsonSerializer.SerializeToDocument(t0, typeof(T0), options),
                t1 => JsonSerializer.SerializeToDocument(t1, typeof(T1), options)
            );

            if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.ValueKind != JsonValueKind.Null)
            {
                writer.WriteStartObject();
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    prop.WriteTo(writer);
                }
                writer.WritePropertyName("dotnetTypeName");
                writer.WriteStringValue(value.Value.GetType().FullName);
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStringValue(value.Value.ToString());
            }
        }
    }

    /// <summary>
    /// Converts a OneOf<T0, T1, T2> value to or from its JSON representation using System.Text.Json.
    /// </summary>
    /// <remarks>This converter enables serialization and deserialization of OneOf<T0, T1, T2> types,
    /// preserving type information in the JSON output. The converter writes a type discriminator property to the JSON
    /// to indicate which type is present. When deserializing, the converter expects this discriminator to determine
    /// which type to instantiate. This converter is intended for use with System.Text.Json and is typically registered
    /// via JsonSerializerOptions.</remarks>
    /// <typeparam name="T0">The type of the first possible value in the OneOf union.</typeparam>
    /// <typeparam name="T1">The type of the second possible value in the OneOf union.</typeparam>
    /// <typeparam name="T2">The type of the third possible value in the OneOf union.</typeparam>
    private class OneOf3JsonConverter<T0, T1, T2> : JsonConverter<OneOf<T0, T1, T2>>
    {
        private static readonly Type OneOfType = typeof(OneOf<,,>).MakeGenericType(typeof(T0), typeof(T1), typeof(T2));
        private static readonly Type[] Types = { typeof(T0), typeof(T1), typeof(T2) };

        /// <summary>
        /// Initializes a new instance of the OneOf3JsonConverter class using the specified JSON serializer options.
        /// </summary>
        /// <param name="_">The JsonSerializerOptions to use for configuring the converter. This parameter can be used to customize
        /// serialization and deserialization behavior.</param>
        public OneOf3JsonConverter(JsonSerializerOptions _)
        {
        }

        /// <summary>
        /// Reads and converts the JSON representation of a value into a OneOf<T0, T1, T2> instance.
        /// </summary>
        /// <param name="reader">The reader positioned at the JSON value to read. The reader's position will be advanced to the end of the
        /// value after the method completes.</param>
        /// <param name="typeToConvert">The type of object to convert to. This should be a OneOf<T0, T1, T2> type.</param>
        /// <param name="options">Options to control the behavior of the JSON serializer during deserialization.</param>
        /// <returns>A OneOf<T0, T1, T2> instance representing the deserialized value from the JSON input.</returns>
        /// <exception cref="JsonException">Thrown if the JSON does not contain a valid type index or if the type index is not a valid number between 0
        /// and 2.</exception>
        public override OneOf<T0, T1, T2> Read(ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            if (!doc.RootElement.TryGetProperty(IndexKey, out var indexElement) ||
                !indexElement.TryGetInt32(out var index) ||
                index is < 0 or > 2)
            {
                throw new JsonException("Cannot not find type index or type index is not a valid number");
            }

            var oneOfBase = CreateOneOf(options, index, doc, OneOfType, Types);

            return (OneOf<T0, T1, T2>)Activator.CreateInstance(typeToConvert, oneOfBase);
        }

        /// <summary>
        /// Writes the specified OneOf value as JSON using the provided Utf8JsonWriter, including type information when
        /// the value is an object.
        /// </summary>
        /// <remarks>If the value is an object, a property named "dotnetTypeName" is added to the JSON
        /// output to indicate the runtime type. For non-object values, the value is written as a string. This method
        /// does not write null values.</remarks>
        /// <param name="writer">The Utf8JsonWriter to which the JSON will be written. Cannot be null.</param>
        /// <param name="value">The OneOf value to serialize. The value may be of type T0, T1, or T2.</param>
        /// <param name="options">The options to use when serializing the value. May influence formatting and converter behavior.</param>
        public override void Write(Utf8JsonWriter writer,
            OneOf<T0, T1, T2> value,
            JsonSerializerOptions options)
        {

            using var doc = value.Match(
                t0 => JsonSerializer.SerializeToDocument(t0, typeof(T0), options),
                t1 => JsonSerializer.SerializeToDocument(t1, typeof(T1), options),
                t2 => JsonSerializer.SerializeToDocument(t2, typeof(T2), options)
            );

            if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.ValueKind != JsonValueKind.Null)
            {
                writer.WriteStartObject();
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    prop.WriteTo(writer);
                }

                writer.WritePropertyName("dotnetTypeName");
                writer.WriteStringValue(value.Value.GetType().FullName);
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStringValue(value.Value.ToString());
            }
        }
    }
}