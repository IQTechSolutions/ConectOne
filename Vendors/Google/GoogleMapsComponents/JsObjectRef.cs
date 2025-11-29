using Microsoft.JSInterop;
using System.Text.Json.Serialization;
using OneOf;


namespace GoogleMapsComponents;

/// <summary>
/// Represents a reference to a JavaScript object identified by a globally unique identifier (GUID).
/// </summary>
/// <remarks>This class provides a way to uniquely identify and compare JavaScript object references across
/// different contexts using a GUID. Instances are considered equal if their GUIDs match. This type is intended for
/// internal use and is not intended to be used directly in application code.</remarks>
internal class JsObjectRef1 : IJsObjectRef
{
    protected Guid _guid;

    /// <summary>
    /// Gets the unique identifier associated with this instance.
    /// </summary>
    public Guid Guid
    {
        get { return _guid; }
    }

    /// <summary>
    /// Gets the string representation of the GUID associated with this instance.
    /// </summary>
    public string GuidString
    {
        get { return _guid.ToString(); }
    }

    /// <summary>
    /// Initializes a new instance of the JsObjectRef1 class with the specified unique identifier.
    /// </summary>
    /// <param name="guid">The unique identifier to associate with this JavaScript object reference.</param>
    public JsObjectRef1(Guid guid)
    {
        _guid = guid;
    }

    /// <summary>
    /// Initializes a new instance of the JsObjectRef1 class using the specified GUID string.
    /// </summary>
    /// <param name="guidString">A string representation of the GUID that uniquely identifies the JavaScript object reference. Must be a valid
    /// GUID format.</param>
    [JsonConstructor] public JsObjectRef1(string guidString)
    {
        _guid = new Guid(guidString);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current JsObjectRef instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current JsObjectRef instance.</param>
    /// <returns>true if the specified object is a JsObjectRef and has the same GUID as the current instance; otherwise, false.</returns>
    public override bool Equals(object obj)
    {
        var other = obj as JsObjectRef;

        if (other == null)
        {
            return false;
        }
        else
        {
            return other.Guid == _guid;
        }
    }

    public override int GetHashCode()
    {
        return _guid.GetHashCode();
    }
}

/// <summary>
/// Represents a reference to a JavaScript object that enables invoking JavaScript functions and accessing properties
/// from .NET code.
/// </summary>
/// <remarks>Use this class to manage the lifetime and interaction with JavaScript objects in Blazor applications.
/// Each instance is associated with a unique identifier and an underlying JavaScript runtime. The class provides
/// methods for invoking JavaScript functions, retrieving property values, and disposing of the referenced JavaScript
/// object. Instances should be disposed when no longer needed to release associated JavaScript resources.</remarks>
public class JsObjectRef : IJsObjectRef, IDisposable
{
    protected readonly Guid _guid;
    protected readonly IJSRuntime _jsRuntime;

    /// <summary>
    /// Gets the unique identifier for this instance.
    /// </summary>
    public Guid Guid
    {
        get { return _guid; }
    }
    
    /// <summary>
    /// Gets the JavaScript runtime instance used to invoke JavaScript functions from .NET code.
    /// </summary>
    public IJSRuntime JSRuntime
    {
        get { return _jsRuntime; }
    }

    /// <summary>
    /// Initializes a new instance of the JsObjectRef class with the specified JavaScript runtime and object identifier.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript runtime instance used to interact with JavaScript objects.</param>
    /// <param name="guid">The unique identifier for the JavaScript object reference.</param>
    public JsObjectRef(IJSRuntime jsRuntime, Guid guid)
    {
        _jsRuntime = jsRuntime;
        _guid = guid;
    }

    /// <summary>
    /// Asynchronously creates a new JavaScript object reference by invoking the specified constructor function with the
    /// provided arguments.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript runtime instance used to invoke the constructor function.</param>
    /// <param name="constructorFunctionName">The fully qualified name of the JavaScript constructor function to invoke.</param>
    /// <param name="args">An array of arguments to pass to the JavaScript constructor function. Each argument is marshaled to JavaScript.
    /// May be empty if the constructor does not require parameters.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a reference to the newly created
    /// JavaScript object.</returns>
    public static Task<JsObjectRef> CreateAsync(
        IJSRuntime jsRuntime,
        string constructorFunctionName,
        params object?[] args)
    {
        return CreateAsync(jsRuntime, Guid.NewGuid(), constructorFunctionName, args);
    }

    /// <summary>
    /// Creates multiple JavaScript object references asynchronously using the specified constructor function and
    /// argument mappings.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript runtime instance used to invoke JavaScript functions.</param>
    /// <param name="constructorFunctionName">The name of the JavaScript constructor function to invoke for each object.</param>
    /// <param name="args">A dictionary mapping unique string keys to argument objects to be passed to each constructor invocation. Each
    /// key identifies a separate JavaScript object to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary mapping the original
    /// string keys to their corresponding JavaScript object references.</returns>
    public async static Task<Dictionary<string, JsObjectRef>> CreateMultipleAsync(
        IJSRuntime jsRuntime,
        string constructorFunctionName,
        Dictionary<string, object> args)
    {
        Dictionary<string, Guid> internalMapping = args.ToDictionary(e => e.Key, e => Guid.NewGuid());
        Dictionary<Guid, object> dictArgs = internalMapping.ToDictionary(e => e.Value, e => args[e.Key]);
        Dictionary<Guid, JsObjectRef> result = await CreateMultipleAsync(
            jsRuntime,
            constructorFunctionName,
            dictArgs);

        return internalMapping.ToDictionary(e => e.Key, e => result[e.Value]);
    }

    /// <summary>
    /// Creates multiple JavaScript object references asynchronously using the specified constructor function and
    /// argument sets.
    /// </summary>
    /// <remarks>Each key in the input dictionary is associated with a newly created JavaScript object. The
    /// returned dictionary preserves the original keys, allowing callers to correlate input arguments with their
    /// resulting object references.</remarks>
    /// <param name="constructorFunctionName">The name of the JavaScript constructor function to invoke for each object creation.</param>
    /// <param name="args">A dictionary mapping unique keys to argument objects. Each entry represents the arguments to pass to a separate
    /// instance of the constructor function.</param>
    /// <returns>A dictionary mapping the original keys from <paramref name="args"/> to their corresponding <see
    /// cref="JsObjectRef"/> instances.</returns>
    public async Task<Dictionary<string, JsObjectRef>> AddMultipleAsync(
        string constructorFunctionName,
        Dictionary<string, object> args)
    {
        Dictionary<string, Guid> internalMapping = args.ToDictionary(e => e.Key, e => Guid.NewGuid());
        Dictionary<Guid, object> dictArgs = internalMapping.ToDictionary(e => e.Value, e => args[e.Key]);
        Dictionary<Guid, JsObjectRef> result = await CreateMultipleAsync(
            _jsRuntime,
            constructorFunctionName,
            dictArgs);

        return internalMapping.ToDictionary(e => e.Key, e => result[e.Value]);
    }

    /// <summary>
    /// Asynchronously creates a new JavaScript object reference by invoking a specified JavaScript function with the
    /// provided arguments.
    /// </summary>
    /// <remarks>The returned object reference can be used to interact with the created JavaScript object in
    /// subsequent calls. The method must be called from a context where JavaScript interop is available.</remarks>
    /// <param name="jsRuntime">The JavaScript runtime instance used to invoke the JavaScript function.</param>
    /// <param name="guid">The unique identifier for the JavaScript object to be created.</param>
    /// <param name="functionName">The name of the JavaScript function to invoke for object creation.</param>
    /// <param name="args">An array of arguments to pass to the JavaScript function. Each argument is forwarded in order after the function
    /// name.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a reference to the newly created
    /// JavaScript object.</returns>
    public async static Task<JsObjectRef> CreateAsync(
        IJSRuntime jsRuntime,
        Guid guid,
        string functionName,
        params object?[] args)
    {
        var jsObjectRef = new JsObjectRef(jsRuntime, guid);

        await jsRuntime.MyInvokeAsync<object>(
            "blazorGoogleMaps.objectManager.createObject",
            new object[] { guid.ToString(), functionName }
                .Concat(args).ToArray()
        );

        return jsObjectRef;
    }

    /// <summary>
    /// Creates multiple JavaScript object references by invoking a specified JavaScript function for each entry in the
    /// provided argument dictionary.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript runtime instance used to invoke JavaScript functions.</param>
    /// <param name="functionName">The name of the JavaScript function to invoke for each object creation.</param>
    /// <param name="dictArgs">A dictionary mapping unique identifiers to argument objects to be passed to the JavaScript function. Each key
    /// represents the identifier for a JavaScript object to be created.</param>
    /// <returns>A dictionary mapping each unique identifier to its corresponding JavaScript object reference. The dictionary
    /// contains an entry for each key in the input argument dictionary.</returns>
    public async static Task<Dictionary<Guid, JsObjectRef>> CreateMultipleAsync(
        IJSRuntime jsRuntime,
        string functionName,
        Dictionary<Guid, object> dictArgs)
    {
        Dictionary<Guid, JsObjectRef> jsObjectRefs = dictArgs.ToDictionary(e => e.Key, e => new JsObjectRef(jsRuntime, e.Key));

        await jsRuntime.MyInvokeAsync<object>(
            "blazorGoogleMaps.objectManager.createMultipleObject",
            new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), functionName }
                .Concat(dictArgs.Values).ToArray()
        );

        return jsObjectRefs;
    }

    /// <summary>
    /// Releases the resources used by the current instance.
    /// </summary>
    /// <remarks>Call this method when you are finished using the object to release any unmanaged resources.
    /// After calling Dispose, the object should not be used.</remarks>
    public virtual void Dispose()
    {
        DisposeAsync();
    }

    /// <summary>
    /// Asynchronously releases resources associated with the underlying JavaScript object managed by this instance.
    /// </summary>
    /// <remarks>Call this method to ensure that any resources held by the corresponding JavaScript object are
    /// properly released. After calling DisposeAsync, the instance should not be used for further operations.</remarks>
    /// <returns>A ValueTask that represents the asynchronous dispose operation. The result is an object returned by the
    /// JavaScript dispose function, or null if no value is returned.</returns>
    public ValueTask<object> DisposeAsync()
    {
        return _jsRuntime.InvokeAsync<object>(
            "blazorGoogleMaps.objectManager.disposeObject",
            _guid.ToString()
        );
    }

    /// <summary>
    /// Asynchronously disposes multiple objects identified by their GUIDs in the underlying JavaScript object manager.
    /// </summary>
    /// <param name="guids">A list of GUIDs representing the objects to dispose. Cannot be null.</param>
    /// <returns>A ValueTask that represents the asynchronous operation. The result contains the response from the JavaScript
    /// invocation.</returns>
    public ValueTask<object> DisposeMultipleAsync(List<Guid> guids)
    {
        return _jsRuntime.InvokeAsync<object>(
            "blazorGoogleMaps.objectManager.disposeMultipleObjects",
            guids.Select(e => e.ToString()).ToList()
        );
    }

    /// <summary>
    /// Invokes a specified JavaScript function on the associated object asynchronously.
    /// </summary>
    /// <param name="functionName">The name of the JavaScript function to invoke. Cannot be null or empty.</param>
    /// <param name="args">An array of arguments to pass to the JavaScript function. Each element represents a parameter for the function
    /// and can be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(string functionName, params object?[] args)
    {
        await _jsRuntime.MyInvokeAsync(
            "blazorGoogleMaps.objectManager.invoke",
            new object?[] { _guid.ToString(), functionName }
                .Concat(args).ToArray()
        );
    }

    /// <summary>
    /// Invokes a specified JavaScript function on multiple objects identified by their unique keys asynchronously.
    /// </summary>
    /// <remarks>This method is typically used to perform batch operations on multiple objects in a Blazor
    /// application by invoking a JavaScript function for each object. The order of invocation corresponds to the order
    /// of keys in the provided dictionary.</remarks>
    /// <param name="functionName">The name of the JavaScript function to invoke for each object.</param>
    /// <param name="dictArgs">A dictionary mapping unique object identifiers to the arguments to pass to the JavaScript function for each
    /// object. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous invocation operation.</returns>
    public Task InvokeMultipleAsync(string functionName, Dictionary<Guid, object> dictArgs)
    {
        return _jsRuntime.MyInvokeAsync(
            "blazorGoogleMaps.objectManager.invokeMultiple",
            new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), functionName }
                .Concat(dictArgs.Values).ToArray()
        );
    }

    /// <summary>
    /// Asynchronously adds multiple event listeners for the specified event to the associated objects.
    /// </summary>
    /// <param name="eventName">The name of the event to listen for. Cannot be null or empty.</param>
    /// <param name="dictArgs">A dictionary mapping unique object identifiers to argument objects for which the event listeners will be added.
    /// Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task AddMultipleListenersAsync(string eventName, Dictionary<Guid, object> dictArgs)
    {
        return _jsRuntime.MyAddListenerAsync(
            "blazorGoogleMaps.objectManager.addMultipleListeners",
            new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), eventName }
                .Concat(dictArgs.Values).ToArray()
        );
    }

    /// <summary>
    /// Invokes a JavaScript function on the associated object and returns the result asynchronously.
    /// </summary>
    /// <remarks>The JavaScript function is invoked in the context of the associated object identified by this
    /// instance. Ensure that the function name and arguments match the expected signature in the JavaScript
    /// environment. This method is typically used to call custom or dynamic JavaScript methods on a managed object from
    /// .NET code.</remarks>
    /// <typeparam name="T">The type of the value expected to be returned from the JavaScript function.</typeparam>
    /// <param name="functionName">The name of the JavaScript function to invoke on the object.</param>
    /// <param name="args">An array of arguments to pass to the JavaScript function. Arguments are passed in the order provided.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the value returned by the JavaScript
    /// function, deserialized to type T.</returns>
    public Task<T> InvokeAsync<T>(string functionName, params object[] args)
    {
        return _jsRuntime.MyInvokeAsync<T>(
            "blazorGoogleMaps.objectManager.invoke",
            new object[] { _guid.ToString(), functionName }
                .Concat(args).ToArray()
        );
    }

    /// <summary>
    /// Invokes a JavaScript function on multiple objects and returns the results as a dictionary.
    /// </summary>
    /// <typeparam name="T">The type of the result expected from each JavaScript function invocation.</typeparam>
    /// <param name="functionName">The name of the JavaScript function to invoke on each object.</param>
    /// <param name="dictArgs">A dictionary mapping object identifiers to argument objects to be passed to the JavaScript function. Each key
    /// represents the unique identifier of an object, and each value contains the arguments for that object's
    /// invocation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary mapping each object's
    /// identifier (as a string) to the result of type T returned by the JavaScript function.</returns>
    public Task<Dictionary<string, T>> InvokeMultipleAsync<T>(string functionName, Dictionary<Guid, object> dictArgs)
    {
        return _jsRuntime.MyInvokeAsync<Dictionary<string, T>>(
            "blazorGoogleMaps.objectManager.invokeMultiple",
            new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), functionName }
                .Concat(dictArgs.Values).ToArray()
        );
    }

    /// <summary>
    /// Use when returned result will be one of defined types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="functionName"></param>
    /// <param name="args"></param>
    /// <returns>Discriminated union of specified types</returns>
    public async Task<OneOf<T, U>> InvokeAsync<T, U>(string functionName, params object[] args)
    {
        var result = await _jsRuntime.MyInvokeAsync<T, U>(
            "blazorGoogleMaps.objectManager.invoke",
            new object[] { _guid.ToString(), functionName }
                .Concat(args).ToArray()
        );

        return result;
    }

    /// <summary>
    /// Use when returned result will be one of defined types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="functionName"></param>
    /// <param name="args"></param>
    /// <returns>Discriminated union of specified types</returns>
    public Task<OneOf<T, U, V>> InvokeAsync<T, U, V>(string functionName, params object[] args)
    {
        return _jsRuntime.MyInvokeAsync<T, U, V>(
            "blazorGoogleMaps.objectManager.invoke",
            new object[] { _guid.ToString(), functionName }
                .Concat(args).ToArray()
        );
    }

    /// <summary>
    /// Invokes a JavaScript function on the referenced object and returns a new object reference to the result.
    /// </summary>
    /// <remarks>Use this method when the invoked JavaScript function returns a new object that should be
    /// referenced from .NET. The returned <see cref="JsObjectRef"/> can be used to invoke further methods on the
    /// resulting JavaScript object.</remarks>
    /// <param name="functionName">The name of the JavaScript function to invoke on the referenced object. Cannot be null or empty.</param>
    /// <param name="args">An array of arguments to pass to the JavaScript function. Each argument is passed in order to the function.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a new <see cref="JsObjectRef"/>
    /// representing the object returned by the JavaScript function.</returns>
    public async Task<JsObjectRef> InvokeWithReturnedObjectRefAsync(string functionName, params object[] args)
    {
        var guid = await _jsRuntime.MyInvokeAsync<string>(
            "blazorGoogleMaps.objectManager.invokeWithReturnedObjectRef",
            new object[] { _guid.ToString(), functionName }
                .Concat(args).ToArray()
        );

        return new JsObjectRef(_jsRuntime, new Guid(guid));
    }

    /// <summary>
    /// Asynchronously retrieves the value of the specified property from the underlying JavaScript object.
    /// </summary>
    /// <remarks>This method communicates with JavaScript to obtain the property value. Ensure that the
    /// property exists and is accessible on the JavaScript object. The returned value is deserialized using the default
    /// JSON serializer settings.</remarks>
    /// <typeparam name="T">The type to which the property value will be deserialized.</typeparam>
    /// <param name="propertyName">The name of the property to retrieve from the JavaScript object. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the value of the specified property,
    /// deserialized to type <typeparamref name="T"/>.</returns>
    public Task<T> GetValue<T>(string propertyName)
    {
        return _jsRuntime.MyInvokeAsync<T>(
            "blazorGoogleMaps.objectManager.readObjectPropertyValue",
            _guid.ToString(),
            propertyName);
    }

    /// <summary>
    /// Retrieves a reference to a JavaScript object property as a new <see cref="JsObjectRef"/> instance.
    /// </summary>
    /// <remarks>Use this method to obtain a reference to a nested JavaScript object property for further
    /// interop operations. The returned reference should be disposed when no longer needed to release associated
    /// resources.</remarks>
    /// <param name="propertyName">The name of the property whose JavaScript object reference is to be retrieved. Cannot be null or empty.</param>
    /// <returns>A <see cref="JsObjectRef"/> representing the referenced JavaScript object property.</returns>
    public async Task<JsObjectRef> GetObjectReference(string propertyName)
    {
        var guid = await _jsRuntime.MyInvokeAsync<string>(
            "blazorGoogleMaps.objectManager.readObjectPropertyValueWithReturnedObjectRef",
            _guid.ToString(),
            propertyName);

        return new JsObjectRef(_jsRuntime, new Guid(guid));
    }

    /// <summary>
    /// Asynchronously retrieves the value of a specified property and maps it to a value of the specified type, using
    /// one or more possible property names.
    /// </summary>
    /// <remarks>If the property is not found using the primary name, each mapped name is tried in order until
    /// a value is found or all names are exhausted.</remarks>
    /// <typeparam name="T">The type to which the property value will be mapped.</typeparam>
    /// <param name="propertyName">The primary name of the property to retrieve.</param>
    /// <param name="mappedNames">An array of alternative property names to use if the primary property name is not found.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the mapped value of type T.</returns>
    public Task<T> GetMappedValue<T>(string propertyName, params string[] mappedNames)
    {
        return _jsRuntime.MyInvokeAsync<T>(
            "blazorGoogleMaps.objectManager.readObjectPropertyValueAndMapToArray",
            _guid.ToString(),
            propertyName, mappedNames);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current JsObjectRef instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current JsObjectRef instance.</param>
    /// <returns>true if the specified object is a JsObjectRef and has the same Guid as the current instance; otherwise, false.</returns>
    public override bool Equals(object obj)
    {
        if (obj is JsObjectRef other)
        {
            return other.Guid == this.Guid;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Serves as the default hash function for the current object.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code representing the current object.</returns>
    public override int GetHashCode()
    {
        return _guid.GetHashCode();
    }
}