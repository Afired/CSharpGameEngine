using System.IO;
using System.Linq;

namespace GameEngine.Core.AssetManagement;

public abstract class AssetManager {
    
    public static AssetManager Instance { get; protected set; }
    
    public abstract string[] GetAllFilePathsOfAssetsWithExtension(string fileExtension);
    
}
