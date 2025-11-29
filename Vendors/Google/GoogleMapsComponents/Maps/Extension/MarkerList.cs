using Microsoft.JSInterop;
using OneOf;

namespace GoogleMapsComponents.Maps.Extension;

/// <summary>
/// A class able to manage a lot of Marker objects and get / set their
/// properties at the same time, eventually with different values
/// Main concept is that each Marker to can be distinguished by other ones need
/// to have a "unique key" with a "external world mean", so not necessary it's GUID
///
/// All properties should be called with a Dictionary<string, {property type}> indicating for each Marker(related to that key) the corresponding related property value
/// </summary>
public class MarkerList : ListableEntityListBase<Marker, MarkerOptions>
{
    public Dictionary<string, Marker> Markers => BaseListableEntities;

    /// <summary>
    /// Create markers list
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="opts">Dictionary of desired Marker keys and MarkerOptions values. Key as any type unique key. Not nessary Guid</param>
    /// <returns>new instance of MarkerList class will be returned with its Markers dictionary member populated with the corresponding results</returns>
    public static async Task<MarkerList> CreateAsync(IJSRuntime jsRuntime, Dictionary<string, MarkerOptions> opts)
    {
        JsObjectRef jsObjectRef = new JsObjectRef(jsRuntime, Guid.NewGuid());

        MarkerList obj;
        Dictionary<string, JsObjectRef> jsObjectRefs = await JsObjectRef.CreateMultipleAsync(
            jsRuntime,
            "google.maps.Marker",
            opts.ToDictionary(e => e.Key, e => (object)e.Value));

        Dictionary<string, Marker> objs = jsObjectRefs.ToDictionary(e => e.Key, e => new Marker(e.Value));
        obj = new MarkerList(jsObjectRef, objs);

        return obj;
    }

    /// <summary>
    /// Sync list over lifetime: Create and remove list depending on entity count; 
    /// entities will be removed, added or changed to mirror the given set.
    /// </summary>
    /// <param name="list">
    /// The list to manage. May be null.
    /// </param>
    /// <param name="jsRuntime"></param>
    /// <param name="opts"></param>
    /// <param name="clickCallback"></param>
    /// <returns>
    /// The managed list. Assign to the variable you used as parameter.
    /// </returns>
    public static async Task<MarkerList> SyncAsync(MarkerList? list,
        IJSRuntime jsRuntime,
        Dictionary<string, MarkerOptions> opts,
        Action<MouseEvent, string, Marker>? clickCallback = null)
    {
        if (opts.Count == 0)
        {
            if (list != null)
            {
                await list.SetMultipleAsync(opts);
                list = null;
            }
        }
        else
        {
            if (list == null)
            {
                list = await MarkerList.CreateAsync(jsRuntime, new Dictionary<string, MarkerOptions>());
                if (clickCallback != null)
                {
                    list.EntityClicked += (sender, e) =>
                    {
                        clickCallback(e.MouseEvent, e.Key, e.Entity);
                    };
                }
            }
            await list.SetMultipleAsync(opts);
        }
        return list;
    }

    private MarkerList(JsObjectRef jsObjectRef, Dictionary<string, Marker> markers)
        : base(jsObjectRef, markers)
    {
    }

    /// <summary>
    /// Set the set of entities; entities will be removed, added or changed to mirror the given set.
    /// </summary>
    /// <param name="opts"></param>
    /// <returns></returns>
    public async Task SetMultipleAsync(Dictionary<string, MarkerOptions> opts)
    {
        await base.SetMultipleAsync(opts, "google.maps.Marker");
    }

    /// <summary>
    /// only keys not matching with existent Marker keys will be created
    /// </summary>
    /// <param name="opts"></param>
    /// <returns></returns>
    public async Task AddMultipleAsync(Dictionary<string, MarkerOptions> opts)
    {
        await base.AddMultipleAsync(opts, "google.maps.Marker");
    }

    /// <summary>
    /// Retrieves a dictionary of available animations, optionally filtered by the specified keys.
    /// </summary>
    /// <param name="filterKeys">A list of animation keys to filter the results. If null or empty, all available animations are returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary mapping animation keys
    /// to their corresponding <see cref="Animation"/> values. The dictionary is empty if no matching animations are
    /// found.</returns>
    public Task<Dictionary<string, Animation>> GetAnimations(List<string>? filterKeys = null)
    {
        List<string> matchingKeys = ComputeMathingKeys(filterKeys);

        if (matchingKeys.Any())
        {
            Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
            Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

            return _jsObjectRef.InvokeMultipleAsync<string>(
                "getAnimation",
                dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => Helper.ToEnum<Animation>(r.Value)));
        }
        else
        {
            return ComputeEmptyResult<Animation>();
        }
    }

    /// <summary>
    /// Asynchronously retrieves a dictionary indicating which items are clickable, optionally filtered by the specified
    /// keys.
    /// </summary>
    /// <param name="filterKeys">A list of keys to filter the items for which clickability is checked. If null or empty, all available items are
    /// considered.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary mapping each item key
    /// to a boolean value indicating whether the item is clickable.</returns>
    public Task<Dictionary<string, bool>> GetClickables(List<string>? filterKeys = null)
    {
        List<string> matchingKeys = ComputeMathingKeys(filterKeys);

        if (matchingKeys.Any())
        {
            Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
            Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

            return _jsObjectRef.InvokeMultipleAsync<bool>(
                "getClickable",
                dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
        }
        else
        {
            return ComputeEmptyResult<bool>();
        }
    }

    /// <summary>
    /// Retrieves a mapping of keys to their associated cursor values, optionally filtered by a specified set of keys.
    /// </summary>
    /// <param name="filterKeys">A list of keys to filter the results by. If null or empty, all available cursors are returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary mapping each key to its
    /// corresponding cursor value. If no matching cursors are found, the dictionary is empty.</returns>
    public Task<Dictionary<string, string>> GetCursors(List<string>? filterKeys = null)
    {
        List<string> matchingKeys = ComputeMathingKeys(filterKeys);

        if (matchingKeys.Any())
        {
            Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
            Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

            return _jsObjectRef.InvokeMultipleAsync<string>(
                "getCursor",
                dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
        }
        else
        {
            return ComputeEmptyResult<string>();
        }
    }

    /// <summary>
    /// Retrieves a dictionary of icons matching the specified filter keys, with each value representing the icon as a
    /// string, an Icon object, or a Symbol object.
    /// </summary>
    /// <param name="filterKeys">A list of keys to filter the icons to retrieve. If null or empty, no icons are returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary mapping each matching
    /// key to its corresponding icon, which may be a string, an Icon object, or a Symbol object. The dictionary is
    /// empty if no keys match.</returns>
    public Task<Dictionary<string, OneOf<string, Icon, Symbol>>> GetIcons(List<string>? filterKeys = null)
    {
        List<string> matchingKeys = ComputeMathingKeys(filterKeys);

        if (matchingKeys.Any())
        {
            Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
            Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

            return _jsObjectRef.InvokeMultipleAsync<OneOf<string, Icon, Symbol>>(
                "getIcon",
                dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
        }
        else
        {
            return ComputeEmptyResult<OneOf<string, Icon, Symbol>>();
        }
    }

    /// <summary>
    /// Retrieves a dictionary of labels for the specified keys.
    /// </summary>
    /// <param name="filterKeys">A list of keys to filter the labels by. If null or empty, no labels are returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary mapping each matching
    /// key to its corresponding label. The dictionary is empty if no keys match the filter.</returns>
    public Task<Dictionary<string, string>> GetLabels(List<string>? filterKeys = null)
    {
        List<string> matchingKeys = ComputeMathingKeys(filterKeys);

        if (matchingKeys.Any())
        {
            Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
            Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

            return _jsObjectRef.InvokeMultipleAsync<string>(
                "getLabel",
                dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
        }
        else
        {
            return ComputeEmptyResult<string>();
        }
    }

    /// <summary>
    /// Retrieves the positions associated with the specified keys.
    /// </summary>
    /// <param name="filterKeys">A list of keys to filter the positions to retrieve. If null or empty, no positions are returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary mapping each key to its
    /// corresponding position as a <see cref="LatLngLiteral"/>. The dictionary is empty if no matching keys are found.</returns>
    public Task<Dictionary<string, LatLngLiteral>> GetPositions(List<string>? filterKeys = null)
    {
        List<string> matchingKeys = ComputeMathingKeys(filterKeys);

        if (matchingKeys.Any())
        {
            Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
            Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

            return _jsObjectRef.InvokeMultipleAsync<LatLngLiteral>(
                "getPosition",
                dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
        }
        else
        {
            return ComputeEmptyResult<LatLngLiteral>();
        }
    }

    /// <summary>
    /// Retrieves a dictionary of marker shapes, optionally filtered by the specified keys.
    /// </summary>
    /// <param name="filterKeys">A list of keys to filter the marker shapes to retrieve. If null or empty, all available marker shapes are
    /// returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary mapping each key to its
    /// corresponding marker shape. If no shapes match the filter, the dictionary is empty.</returns>
    public Task<Dictionary<string, MarkerShape>> GetShapes(List<string>? filterKeys = null)
    {
        List<string> matchingKeys = ComputeMathingKeys(filterKeys);

        if (matchingKeys.Any())
        {
            Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
            Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

            return _jsObjectRef.InvokeMultipleAsync<MarkerShape>(
                "getShape",
                dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
        }
        else
        {
            return ComputeEmptyResult<MarkerShape>();
        }
    }

    /// <summary>
    /// Asynchronously retrieves a dictionary mapping keys to their corresponding titles, optionally filtered by a
    /// specified set of keys.
    /// </summary>
    /// <param name="filterKeys">A list of keys to filter the results by. If null or empty, all available keys are included.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary where each key is a
    /// string identifier and each value is the corresponding title. The dictionary is empty if no matching keys are
    /// found.</returns>
    public Task<Dictionary<string, string>> GetTitles(List<string>? filterKeys = null)
    {
        List<string> matchingKeys = ComputeMathingKeys(filterKeys);

        if (matchingKeys.Any())
        {
            Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
            Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

            return _jsObjectRef.InvokeMultipleAsync<string>(
                "getTitle",
                dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
        }
        else
        {
            return ComputeEmptyResult<string>();
        }
    }

    /// <summary>
    /// Retrieves the z-index values for the specified keys.
    /// </summary>
    /// <param name="filterKeys">A list of keys to filter the results. If null or empty, no z-index values are returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary mapping each key to its
    /// corresponding z-index value. The dictionary is empty if no matching keys are found.</returns>
    public Task<Dictionary<string, int>> GetZIndexes(List<string>? filterKeys = null)
    {
        List<string> matchingKeys = ComputeMathingKeys(filterKeys);

        if (matchingKeys.Any())
        {
            Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
            Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

            return _jsObjectRef.InvokeMultipleAsync<int>(
                "getZIndex",
                dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
        }
        else
        {
            return ComputeEmptyResult<int>();
        }
    }

    /// <summary>
    /// Start an animation. 
    /// Any ongoing animation will be cancelled. 
    /// Currently supported animations are: BOUNCE, DROP. 
    /// Passing in null will cause any animation to stop.
    /// </summary>
    /// <param name="animations"></param>
    public Task SetAnimations(Dictionary<string, Animation?> animations)
    {
        Dictionary<Guid, object?> dictArgs = animations.ToDictionary(e => Markers[e.Key].Guid, e => (object?)GetAnimationCode(e.Value));
        return _jsObjectRef.InvokeMultipleAsync(
            "setAnimation",
            dictArgs);
    }

    /// <summary>
    /// Gets the integer code corresponding to the specified animation, if defined.
    /// </summary>
    /// <param name="animation">The animation for which to retrieve the code. Can be null.</param>
    /// <returns>An integer code representing the specified animation, or null if <paramref name="animation"/> is null. Returns 0
    /// if the animation is not recognized.</returns>
    public int? GetAnimationCode(Animation? animation)
    {
        switch (animation)
        {
            case null: return null;
            case Animation.Bounce: return 1;
            case Animation.Drop: return 2;
            default: return 0;
        }
    }

    /// <summary>
    /// Sets the Clickable flag of one or more Markers to match a dictionary of marker keys and flag values.
    /// </summary>
    /// <param name="flags"></param>
    /// <returns></returns>
    public Task SetClickables(Dictionary<string, bool> flags)
    {
        Dictionary<Guid, object> dictArgs = flags.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setClickable",
            dictArgs);
    }

    /// <summary>
    /// Sets the cursor style for multiple elements based on the specified mapping.
    /// </summary>
    /// <remarks>If a key in the dictionary does not correspond to a known marker, an exception may be thrown.
    /// The operation is performed asynchronously and may not complete immediately.</remarks>
    /// <param name="cursors">A dictionary that maps element identifiers to cursor style values. Each key should correspond to a valid element
    /// marker, and each value specifies the cursor style to apply.</param>
    /// <returns>A task that represents the asynchronous operation of setting the cursors.</returns>
    public Task SetCursors(Dictionary<string, string> cursors)
    {
        Dictionary<Guid, object> dictArgs = cursors.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setCursor",
            dictArgs);
    }

    /// <summary>
    /// Set Icon on each Marker matching a param dictionary key to the param value with single JSInterop call.
    /// </summary>
    /// <param name="icons"></param>
    /// <returns></returns>
    public Task SetIcons(Dictionary<string, OneOf<string, Icon, Symbol>> icons)
    {
        Dictionary<Guid, object> dictArgs = icons.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setIcon",
            dictArgs);
    }

    /// <summary>
    /// Sets the icons for multiple markers using the specified icon URLs.
    /// </summary>
    /// <param name="icons">A dictionary mapping marker identifiers to icon URLs. Each key represents a marker's identifier, and each value
    /// is the URL of the icon to assign to that marker. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetIcons(Dictionary<string, string> icons)
    {
        Dictionary<Guid, object> dictArgs = icons.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setIcon",
            dictArgs);
    }

    /// <summary>
    /// Asynchronously sets the icons for multiple markers using the specified icon mappings.
    /// </summary>
    /// <param name="icons">A dictionary that maps marker identifiers to their corresponding icons. Each key represents the marker's
    /// identifier, and each value specifies the icon to assign to that marker. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetIcons(Dictionary<string, Icon> icons)
    {
        Dictionary<Guid, object> dictArgs = icons.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setIcon",
            dictArgs);
    }


    // <inheritdoc cref="SetLabels(Dictionary{string, OneOf{string, MarkerLabel}})"/>
    //[Obsolete("Use overloads that take string, MarkerLabel, or OneOf<string, MarkerLabel> as dictionary value type.")]
    //public Task SetLabels(Dictionary<string, Symbol> labels)
    //{
    //    Dictionary<Guid, object> dictArgs = labels.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
    //    return _jsObjectRef.InvokeMultipleAsync(
    //        "setLabel",
    //        dictArgs);
    //}

    /// <summary>
    /// Set Label on each Marker matching a param dictionary key to the param value with single JSInterop call.
    /// </summary>
    /// <param name="labels"></param>
    /// <returns></returns>
    public Task SetLabels(Dictionary<string, OneOf<string, MarkerLabel>> labels)
    {
        Dictionary<Guid, object> dictArgs = labels.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setLabel",
            dictArgs);
    }

    /// <summary>
    /// Asynchronously sets labels for the specified markers.
    /// </summary>
    /// <param name="labels">A dictionary mapping marker identifiers to their corresponding label text. Each key must match an existing
    /// marker; each value specifies the label to assign.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetLabels(Dictionary<string, string> labels)
    {
        Dictionary<Guid, object> dictArgs = labels.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setLabel",
            dictArgs);
    }

    /// <summary>
    /// Asynchronously sets the labels for the specified markers.
    /// </summary>
    /// <param name="labels">A dictionary mapping marker identifiers to their corresponding labels. Each key represents a marker ID, and each
    /// value specifies the label to assign to that marker. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetLabels(Dictionary<string, MarkerLabel> labels)
    {
        Dictionary<Guid, object> dictArgs = labels.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setLabel",
            dictArgs);
    }

    /// <summary>
    /// Asynchronously sets the opacity values for multiple markers.
    /// </summary>
    /// <remarks>If a marker identifier in the dictionary does not correspond to an existing marker, an
    /// exception may be thrown. Opacity values outside the range 0.0 to 1.0 may result in undefined behavior.</remarks>
    /// <param name="opacities">A dictionary mapping marker identifiers to their desired opacity values. Each key is the marker's string
    /// identifier, and each value is a float between 0.0 (fully transparent) and 1.0 (fully opaque).</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetOpacities(Dictionary<string, float> opacities)
    {
        Dictionary<Guid, object> dictArgs = opacities.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setOpacity",
            dictArgs);
    }

    /// <summary>
    /// Asynchronously sets the positions of multiple markers using the specified latitude and longitude values.
    /// </summary>
    /// <param name="latLngs">A dictionary mapping marker identifiers to their corresponding latitude and longitude coordinates. Each key
    /// should be the marker's identifier, and each value should specify the new position for that marker.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetPositions(Dictionary<string, LatLngLiteral> latLngs)
    {
        Dictionary<Guid, object> dictArgs = latLngs.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setPosition",
            dictArgs);
    }

    /// <summary>
    /// Asynchronously updates the shapes of multiple markers based on the specified mapping.
    /// </summary>
    /// <remarks>If a marker name in <paramref name="shapes"/> does not correspond to an existing marker, an
    /// exception may be thrown. The operation is performed asynchronously and may not complete immediately.</remarks>
    /// <param name="shapes">A dictionary that maps marker names to their corresponding <see cref="MarkerShape"/> values. Each entry
    /// specifies the new shape to apply to the marker identified by the key.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetShapes(Dictionary<string, MarkerShape> shapes)
    {
        Dictionary<Guid, object> dictArgs = shapes.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setShape",
            dictArgs);
    }

    /// <summary>
    /// Asynchronously sets the titles for multiple markers using the specified title mappings.
    /// </summary>
    /// <param name="titles">A dictionary that maps marker identifiers to their corresponding titles. Each key should be the marker's
    /// identifier, and each value is the title to assign to that marker. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetTitles(Dictionary<string, string> titles)
    {
        Dictionary<Guid, object> dictArgs = titles.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setTitle",
            dictArgs);
    }

    /// <summary>
    /// Sets the z-index values for multiple markers based on the specified mapping.
    /// </summary>
    /// <param name="zIndexes">A dictionary mapping marker identifiers to their desired z-index values. Each key should correspond to a valid
    /// marker identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when all z-index values have been set.</returns>
    public Task SetZIndexes(Dictionary<string, int> zIndexes)
    {
        Dictionary<Guid, object> dictArgs = zIndexes.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setZIndex",
            dictArgs);
    }
}