using System;

namespace MathHelper {
    public struct Vector2F {
        private float x, y;

        #region Properties
        public float X {
            get { return x; }
            set { x = value; }
        }
        public float Y {
            get { return y; }
            set { y = value; }
        }

        public float Length { get { return (float) Math.Sqrt (x * x + y * y); } }
        public float LengthSquared { get { return x * x + y * y; } }
        #endregion

        #region Constructors
        public Vector2F (float newX, float newY) {
            x = newX;
            y = newY;
        }
        public Vector2F (Vector2F vec) {
            x = vec.x;
            y = vec.y;
        }
        #endregion

        #region Functions
        #region Translate
        public void Translate (float xOff, float yOff) {
            x += xOff;
            y += yOff;
        }
        public void Translate (float offset) { Translate (offset, offset); }
        public void Translate (Vector2F vec) { Translate (vec.x, vec.y); }
        public void Translate (Vector3F vec) { Translate (vec.X, vec.Y); }
        #endregion

        #region Multiply
        public void Multiply (float xOff, float yOff) {
            x *= xOff;
            y *= yOff;
        }
        public void Multiply (float offset) { Multiply (offset, offset); }
        public void Multiply (Vector2F vec) { Multiply (vec.x, vec.y); }
        public void Multiply (Vector3F vec) { Multiply (vec.X, vec.Y); }
        #endregion

        #region Divide
        public void Divide (float xOff, float yOff) {
            x /= xOff;
            y /= yOff;
        }
        public void Divide (float offset) { Divide (offset, offset); }
        public void Divide (Vector2F vec) { Divide (vec.x, vec.y); }
        public void Divide (Vector3F vec) { Divide (vec.X, vec.Y); }
        #endregion

        #region Rotate
        public void Rotate (float angle) {
            float newX = (float) (x * Math.Cos (angle) - y * Math.Sin (angle));
            float newY = (float) (x * Math.Sin (angle) + y * Math.Cos (angle));
            x = newX;
            y = newY;
        }
        #endregion

        #region Stuff
        public static Vector2F operator - (Vector2F vec) { return new Vector2F (-vec.x, -vec.y); }
        public override bool Equals (object rhs) { return Equals ((Vector3F) rhs); }
        public override string ToString () { return String.Format ("X: {0} Y: {1}", x, y); }
        #endregion

        public bool Equals (Vector2F rhs) { return this.LengthSquared == rhs.LengthSquared; }
        
        public Vector2F Copy () { return new Vector2F (x, y); }
        public Vector2F YX () { return new Vector2F (y, x); }
        #endregion
    }

    public struct Vector3F {
        float x, y, z;

        #region Properties
        public float X {
            get { return x; }
            set { x = value; }
        }
        public float Y {
            get { return y; }
            set { y = value; }
        }
        public float Z {
            get { return z; }
            set { z = value; }
        }

        public float Length { get { return (float) Math.Sqrt (x * x + y * y + z * z); } }
        public float LengthSquared { get { return x * x + y * y + z * z; } }
        #endregion

        #region Constructors
        public Vector3F (float newX, float newY, float newZ) {
            x = newX;
            y = newY;
            z = newZ;
        }
        public Vector3F (Vector2F vec) {
            x = vec.X;
            y = vec.Y;
            z = 0f;
        }
        public Vector3F (Vector3F vec) {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }
        #endregion

        #region Functions
        #region Translate
        public void Translate (float xOff, float yOff, float zOff) {
            x += xOff;
            y += yOff;
            z += zOff;
        }
        public void Translate (float offset) { Translate (offset, offset, offset); }
        public void Translate (Vector2F vec) { Translate (vec.X, vec.Y, 0f); }
        public void Translate (Vector3F vec) { Translate (vec.x, vec.y, vec.z); }
        #endregion

        #region Multiply
        public void Multiply (float scaleX, float scaleY, float scaleZ) {
            x *= scaleX;
            y *= scaleY;
            z *= scaleZ;
        }
        public void Multiply (float    scale) { Multiply (scale, scale, scale); }
        public void Multiply (Vector2F scale) { Multiply (scale.X, scale.Y, 1f); }
        public void Multiply (Vector3F scale) { Multiply (scale.x, scale.y, scale.z); }
        #endregion

        #region Divide
        public void Divide (float scaleX, float scaleY, float scaleZ) {
            x /= scaleX;
            y /= scaleY;
            z /= scaleZ;
        }
        public void Divide (float scale) { Divide (scale, scale, scale); }
        public void Divide (Vector2F scale) { Divide (scale.X, scale.Y, 1f); }
        public void Divide (Vector3F scale) { Divide (scale.x, scale.y, scale.z); }
        #endregion

        #region Rotation
        public void Yaw (float angle) {
            float newX = (float) (x * Math.Cos (angle) - y * Math.Sin (angle));
            float newY = (float) (x * Math.Sin (angle) + y * Math.Cos (angle));
            x = newX;
            y = newY;
        }

        public void Pitch (float pitch) {
            float newX = (float) (x * Math.Cos (pitch) - z * Math.Sin (pitch));
            float newZ = (float) (x * Math.Sin (pitch) + z * Math.Cos (pitch));
            x = newX;
            z = newZ;
        }

        public void Roll (float roll) {
            float newY = (float) (y * Math.Cos (roll) - z * Math.Sin (roll));
            float newZ = (float) (y * Math.Sin (roll) + z * Math.Cos (roll));
            y = newY;
            z = newZ;
        }
        #endregion

        #region Stuff
        public static Vector3F operator - (Vector3F vec) { return new Vector3F (-vec.x, -vec.y, -vec.z); }
        public override bool Equals (object   rhs) { return Equals ((Vector3F) rhs); }
        public override string ToString () { return String.Format ("X: {0} Y: {1} Z: {2}", x, y, z); }
        #endregion

        public bool Equals (Vector3F rhs) { return this.LengthSquared == rhs.LengthSquared; }
        
        public Vector3F Copy () { return new Vector3F (x, y, z); }
        #region Vector2F Conversions
        public Vector2F XY () { return new Vector2F (x, y); }
        public Vector2F XZ () { return new Vector2F (x, z); }
        public Vector2F YX () { return new Vector2F (y, x); }
        public Vector2F YZ () { return new Vector2F (y, z); }
        public Vector2F ZX () { return new Vector2F (z, x); }
        public Vector2F ZY () { return new Vector2F (z, y); }
        #endregion
        #endregion
    }
}
