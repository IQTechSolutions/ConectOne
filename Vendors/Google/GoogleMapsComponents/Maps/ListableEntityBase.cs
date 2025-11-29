using GoogleMapsComponents.Maps.Extension;
using OneOf;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Provides a base class for entities that can be listed and rendered on a map or panorama, supporting JavaScript
/// interop and map association functionality.
/// </summary>
/// <remarks>This class enables interaction with JavaScript-based map entities, allowing derived types to invoke
/// JavaScript functions and manage their association with maps or panoramas. It is intended to be used as a base for
/// entities that require both event handling and the ability to be rendered or removed from a map context.</remarks>
/// <typeparam name="TEntityOptions">The type of options used to configure the listable entity. Must derive from ListableEntityOptionsBase.</typeparam>
public class ListableEntityBase<TEntityOptions> : EventEntityBase, IJsObjectRef
    where TEntityOptions : ListableEntityOptionsBase
{
    public Guid Guid => _jsObjectRef.Guid;

    /// <summary>
    /// Initializes a new instance of the ListableEntityBase class with the specified JavaScript object reference.
    /// </summary>
    /// <param name="jsObjectRef">A reference to the underlying JavaScript object that represents the entity. Cannot be null.</param>
    internal ListableEntityBase(JsObjectRef jsObjectRef) : base(jsObjectRef)
    {
    }

    /// <summary>
    /// Asynchronously retrieves the current map instance associated with this object.
    /// </summary>
    /// <remarks>The returned <see cref="Map"/> instance provides access to map properties and methods. Await
    /// the returned task to obtain the map object when the operation completes.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Map"/> instance
    /// representing the current map.</returns>
    public virtual Task<Map> GetMap()
    {
        return _jsObjectRef.InvokeAsync<Map>("getMap");
    }

    /// <summary>
    /// Renders the map entity on the specified map or panorama. 
    /// If map is set to null, the map entity will be removed.
    /// </summary>
    /// <param name="map"></param>
    public virtual async Task SetMap(Map? map)
    {
        await _jsObjectRef.InvokeAsync("setMap", map);

        //_map = map;
    }

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously with the provided arguments.
    /// </summary>
    /// <param name="functionName">The name of the JavaScript function to invoke. Cannot be null or empty.</param>
    /// <param name="args">An array of arguments to pass to the JavaScript function. The arguments are serialized and passed in order.</param>
    /// <returns>A task that represents the asynchronous invocation operation.</returns>
    public Task InvokeAsync(string functionName, params object[] args)
    {
        return _jsObjectRef.InvokeAsync(functionName, args);
    }

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously and returns the result as the specified type.
    /// </summary>
    /// <typeparam name="T">The type to which the result of the JavaScript function call is deserialized.</typeparam>
    /// <param name="functionName">The name of the JavaScript function to invoke. Cannot be null or empty.</param>
    /// <param name="args">An array of arguments to pass to the JavaScript function. Each argument is serialized before being sent.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized value returned by
    /// the JavaScript function.</returns>
    public Task<T> InvokeAsync<T>(string functionName, params object[] args)
    {
        return _jsObjectRef.InvokeAsync<T>(functionName, args);
    }

    /// <summary>
    /// Invokes a JavaScript function asynchronously and returns a result of one of two possible types.
    /// </summary>
    /// <typeparam name="T">The type of the first possible result returned by the JavaScript function.</typeparam>
    /// <typeparam name="U">The type of the second possible result returned by the JavaScript function.</typeparam>
    /// <param name="functionName">The name of the JavaScript function to invoke.</param>
    /// <param name="args">An array of arguments to pass to the JavaScript function.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a value of type <see cref="OneOf{T,
    /// U}"/>, representing either a value of type <typeparamref name="T"/> or <typeparamref name="U"/> returned from
    /// the JavaScript function.</returns>
    public Task<OneOf<T, U>> InvokeAsync<T, U>(string functionName, params object[] args)
    {
        return _jsObjectRef.InvokeAsync<T, U>(functionName, args);
    }

    /// <summary>
    /// Invokes a JavaScript function asynchronously and returns a result that can be one of three specified types.
    /// </summary>
    /// <typeparam name="T">The first possible return type of the JavaScript function.</typeparam>
    /// <typeparam name="U">The second possible return type of the JavaScript function.</typeparam>
    /// <typeparam name="V">The third possible return type of the JavaScript function.</typeparam>
    /// <param name="functionName">The name of the JavaScript function to invoke.</param>
    /// <param name="args">An array of arguments to pass to the JavaScript function.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a value of type <see cref="OneOf{T,
    /// U, V}"/>, representing the result returned by the JavaScript function as one of the specified types.</returns>
    public Task<OneOf<T, U, V>> InvokeAsync<T, U, V>(string functionName, params object[] args)
    {
        return _jsObjectRef.InvokeAsync<T, U, V>(functionName, args);
    }
}