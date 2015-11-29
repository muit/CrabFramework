/////////////////////////////////////////////////////////////////////////
// 
// PicaVoxel - The tiny voxel engine for Unity - http://picavoxel.com
// By Gareth Williams - @garethiw - http://gareth.pw
// 
// Source code distributed under standard Asset Store licence:
// http://unity3d.com/legal/as_terms
//
/////////////////////////////////////////////////////////////////////////
using System;
using UnityEngine;

namespace PicaVoxel
{
    /// <summary>
    /// Represents a single voxel in a volume.
    /// </summary>
    /// You may extend this struct with extra fields beyond Active, Color and Value but be warned that every added byte will increase memory usage exponentially
    /// When adding fields, you must ensure that you also extend the constructor to deserialise your new fields from the supplied byte array, and extend ToBytes() to 
    /// serialise your new fields to the byte array.
    [Serializable]
    public struct Voxel
    {
        public const int BYTE_SIZE = 5;

        public bool Active;
        public byte Value;
        public Color32 Color;

        /// <summary>
        /// Deserialise this voxel from a byte array
        /// </summary>
        /// <param name="bytes">A byte array representing a single voxel</param>
        public Voxel(byte[] bytes)
        {
            if (bytes.Length != BYTE_SIZE)
            {
                Active = false;
                Value = 128;
                Color = UnityEngine.Color.black;
            }

            Active = bytes[0] == 1;
            Value = bytes[1];
            Color = new Color32(bytes[2], bytes[3], bytes[4], 255);
        }

        /// <summary>
        /// Serialise this voxel to a byte array
        /// </summary>
        /// <returns>A byte array representing this voxel for serialisation</returns>
        public byte[] ToBytes()
        {
            byte[] bytes = new byte[BYTE_SIZE];
            //Color32 bCol = Color;

            bytes[0] = Active ? (byte) 1 : (byte) 0;
            bytes[1] = Value;
            bytes[2] = Color.r;
            bytes[3] = Color.g;
            bytes[4] = Color.b;

            return bytes;
        }

    }


}