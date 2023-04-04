//using System.Numerics;
//using GameEngine.Numerics;
//
//namespace GameEngine.Editor.PropertyDrawers; 
//
//public class PropertyDrawerMatrix<T> : PropertyDrawer<Matrix<T>> where T : struct, IFloatingPointIeee754<T> {
//    
//    protected override void DrawProperty(ref Matrix<T> matrix, Property property) {
//        
//        matrix.Decompose(out Vec3<T> scale, out Quaternion<T> rotation, out Vec3<T> position);
//        
//        scale = (Vec3<T>) DrawDirect(typeof(Vec3<T>), scale, property);
//        rotation = (Quaternion<T>) DrawDirect(typeof(Quaternion<T>), rotation, property);
//        position = (Vec3<T>) DrawDirect(typeof(Vec3<T>), position, );
//        
//        matrix = Matrix<T>.CreateTranslation(position) * Matrix<T>.CreateFromQuaternion(rotation) * Matrix<T>.CreateScale(scale);
//    }
//    
//}
