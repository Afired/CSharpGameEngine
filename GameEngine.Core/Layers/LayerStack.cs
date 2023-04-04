using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace GameEngine.Core.Layers; 

public class LayerStack : IEnumerable<Layer> {
    
    private readonly List<Layer> _normalLayers;
    private readonly List<Layer> _overlayLayers;

    public DefaultNormalLayer DefaultNormalLayer { get; private set; }
    public DefaultOverlayLayer DefaultOverlayLayer { get; private set; }
    

    internal LayerStack() {
        _normalLayers = new List<Layer>();
        _overlayLayers = new List<Layer>();

        DefaultNormalLayer = new DefaultNormalLayer();
        Push(DefaultNormalLayer, LayerType.Normal);
        DefaultOverlayLayer = new DefaultOverlayLayer();
        Push(DefaultOverlayLayer, LayerType.Overlay);
    }
    
    /// <summary>
    /// Adds a layer to the end of the specified layer type in the layer stack.
    /// </summary>
    /// <param name="layer">The layer to be added to the layer stack.</param>
    /// <param name="layerType">The layer type the layer should be added to in the layer stack.</param>
    public void Push(Layer layer, LayerType layerType) {
        GetLayerListFromEnum(layerType).Add(layer);
        Count++;
    }
    
    /// <summary>
    /// Removes the first occurrence of a specific layer from the layer stack.
    /// </summary>
    /// <param name="layer">The layer to remove from the layer stack.</param>
    /// <returns>true if layer is successfully removed; otherwise, false. This method also returns false if the layer was not found in the layer stack.</returns>
    public bool Pop(Layer layer) {
        if(_normalLayers.Remove(layer)) {
            Count--;
            return true;
        }
        if(_overlayLayers.Remove(layer)) {
            Count--;
            return true;
        }
        return false;
    }
    
    private List<Layer> GetLayerListFromEnum(LayerType layerType) => layerType switch {
        LayerType.Normal => _normalLayers,
        LayerType.Overlay => _overlayLayers,
        _ => throw new InvalidEnumArgumentException("The provided layer type is not implemented")
    };

    public IEnumerable<Layer> GetNormalLayers() {
        foreach(Layer normalLayer in _normalLayers)
            yield return normalLayer;
    }

    public IEnumerable<Layer> GetOverlayLayers() {
        foreach(Layer overlayLayer in _overlayLayers)
            yield return overlayLayer;
    }

    public IEnumerator<Layer> GetEnumerator() {
        foreach(Layer normalLayer in _normalLayers)
            yield return normalLayer;
        foreach(Layer overlayLayer in _overlayLayers)
            yield return overlayLayer;
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
    
    /// <summary>
    /// Gets the number of layers contained in the layer stack.
    /// </summary>
    public int Count { get; private set; }
    
    public Layer this[int index] {
        get {
            if(index > Count - 2)
                throw new ArgumentOutOfRangeException();
            if(index < _normalLayers.Count)
                return _normalLayers[index];
            return _overlayLayers[index];
        }
    }
    
}
