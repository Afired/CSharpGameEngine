using System;

namespace GameEngine.Core.AssetManagement;

public interface IAsset {
    public static abstract IAsset Default { get; }
}
