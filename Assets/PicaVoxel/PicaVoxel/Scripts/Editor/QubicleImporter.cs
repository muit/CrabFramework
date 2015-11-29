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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace PicaVoxel
{
    public static class QubicleImporter
    {
        private static void FromQubicle(BinaryReader stream, string volumeName, float voxelSize)
        {
            //const int MAX_VOLUME_DIMENSION = 64;

            uint sizex=0;
            uint sizey=0;
            uint sizez=0;

//            uint version;
            uint colorFormat;
            uint zAxisOrientation;
            uint compressed;
//            uint visibilityMaskEncoded;
            uint numMatrices;

            uint i;
            uint j;
            uint x;
            uint y;
            uint z;

            int posX;
            int posY;
            int posZ;
            uint[,,] matrix;
            List<uint[,,]> matrixList = new List<uint[,,]>();
            uint index;
            uint data;
            uint count;
            const uint CODEFLAG = 2;
            const uint NEXTSLICEFLAG = 6;
 
           //version = stream.ReadUInt32();
            stream.ReadUInt32();
           colorFormat = stream.ReadUInt32();
           zAxisOrientation = stream.ReadUInt32();
           compressed = stream.ReadUInt32();
           //visibilityMaskEncoded = stream.ReadUInt32();
            stream.ReadUInt32();
           numMatrices = stream.ReadUInt32();

           var importParentObject = new GameObject();
           importParentObject.name = volumeName;

           for (i = 0; i < numMatrices; i++) // for each matrix stored in file
           {
             // read matrix name
             byte nameLength = stream.ReadByte();
             string name = new string(stream.ReadChars(nameLength));

             // read matrix size 
             sizex = stream.ReadUInt32();
             sizey = stream.ReadUInt32();
             sizez = stream.ReadUInt32();
   
             // read matrix position (in this example the position is irrelevant)
             posX = stream.ReadInt32();
             posY = stream.ReadInt32();
             posZ = stream.ReadInt32();
   
             // create matrix and add to matrix list
             matrix = new uint[sizex,sizey,sizez];
             matrixList.Add(matrix);
   
             if (compressed == 0) // if uncompressd
             {
               for(z = 0; z < sizez; z++)
                 for(y = 0; y < sizey; y++)
                     for (x = 0; x < sizex; x++)
                         matrix[x, y, z] = stream.ReadUInt32();
             }
             else // if compressed
             { 
               z = 0;

               while (z < sizez) 
               {
                
                 index = 0;
       
                 while (true) 
                 {
                   data = stream.ReadUInt32();
         
                   if (data == NEXTSLICEFLAG)
                     break;
                   else if (data == CODEFLAG) 
                   {
                     count = stream.ReadUInt32();
                     data = stream.ReadUInt32();
           
                     for(j = 0; j < count; j++) 
                     {
                       x = index % sizex ; // mod = modulo e.g. 12 mod 8 = 4
                       y = index / sizex ; // div = integer division e.g. 12 div 8 = 1
                       index++;
                       matrix[x ,y ,z] = data;
                     }
                   }
                   else 
                   {
                     x = index % sizex;
                     y = index / sizex ;
                     index++;
                     matrix[x,y ,z] = data;
                   }
                 }
                 z++;
               }
             }

               // Quick and dirty multi-part splitter if size > 32 on any axis
            //if (sizex > MAX_VOLUME_DIMENSION || sizey > MAX_VOLUME_DIMENSION || sizez > MAX_VOLUME_DIMENSION)
            //{
            //    var parentObject = new GameObject();
            //    parentObject.name = (name != "" ? name : volumeName);
            //    parentObject.transform.position = (new Vector3((zAxisOrientation == 0 ? -posX : posX), posY, posZ)*
            //                                       voxelSize);

            //    int xparts = Convert.ToInt32((sizex/MAX_VOLUME_DIMENSION) + (sizex%MAX_VOLUME_DIMENSION > 0 ? 1 : 0));
            //    int yparts = Convert.ToInt32((sizey/MAX_VOLUME_DIMENSION) + (sizey%MAX_VOLUME_DIMENSION > 0 ? 1 : 0));
            //    int zparts = Convert.ToInt32((sizez/MAX_VOLUME_DIMENSION) + (sizez%MAX_VOLUME_DIMENSION > 0 ? 1 : 0));

            //    uint xRem = sizex%MAX_VOLUME_DIMENSION;
            //    uint yRem = sizey % MAX_VOLUME_DIMENSION;
            //    uint zRem = sizez % MAX_VOLUME_DIMENSION;

            //    Volume[,,] volumeRefs = new Volume[xparts,yparts,zparts];

            //    for (x = 0; x < xparts; x++)
            //        for (y = 0; y < yparts; y++)
            //            for (z = 0; z < zparts; z++)
            //            {
            //                var newObject =
            //                    Editor.Instantiate(EditorUtility.VoxelVolumePrefab, Vector3.zero, Quaternion.identity) as
            //                        GameObject;
            //                if (newObject != null)
            //                {
            //                    newObject.name = (name!=""?name:volumeName) + " (" + x + "," + y + "," + z + ")";
            //                    newObject.GetComponent<Volume>().Material = EditorUtility.PicaVoxelDiffuseMaterial;
            //                    newObject.GetComponent<Volume>().VoxelSize = voxelSize;
            //                    newObject.GetComponent<Volume>().GenerateBasic(FillMode.None);
            //                    newObject.transform.parent = parentObject.transform;
            //                    Volume voxelVolume = newObject.GetComponent<Volume>();

            //                    voxelVolume.XSize = Convert.ToInt32(x<xparts-1?MAX_VOLUME_DIMENSION:xRem);
            //                    voxelVolume.YSize = Convert.ToInt32(y < yparts - 1 ? MAX_VOLUME_DIMENSION : yRem);
            //                    voxelVolume.ZSize = Convert.ToInt32(z < zparts - 1 ? MAX_VOLUME_DIMENSION : zRem);
            //                    voxelVolume.Frames[0].XSize = voxelVolume.XSize;
            //                    voxelVolume.Frames[0].YSize = voxelVolume.YSize;
            //                    voxelVolume.Frames[0].ZSize = voxelVolume.ZSize;
            //                    voxelVolume.Frames[0].Voxels = new Voxel[voxelVolume.XSize * voxelVolume.YSize * voxelVolume.ZSize];
            //                    newObject.transform.parent = parentObject.transform;


            //                    newObject.transform.localPosition =
            //                        new Vector3(x * MAX_VOLUME_DIMENSION * voxelSize, y * MAX_VOLUME_DIMENSION * voxelSize, z * MAX_VOLUME_DIMENSION * voxelSize);

            //                    if (zAxisOrientation == 0)
            //                    {
            //                        voxelVolume.Pivot = new Vector3(sizex, 0, 0) * voxelSize;
            //                        voxelVolume.UpdatePivot();
            //                    }
                               

            //                    volumeRefs[x, y, z] = voxelVolume;
            //                }
            //            }

                
            //            for(z = 0; z < sizez; z++)
            //                for(y = 0; y < sizey; y++)
            //                    for (x = 0; x < sizex; x++)
            //                    {
            //                        int xpart = Convert.ToInt32((zAxisOrientation==0?sizex-1-x:x)/MAX_VOLUME_DIMENSION);
            //                        int ypart = Convert.ToInt32(y/MAX_VOLUME_DIMENSION);
            //                        int zpart = Convert.ToInt32(z/MAX_VOLUME_DIMENSION);


            //                        Color col = UIntToColor(matrix[x, y, z], colorFormat);

            //                        if(matrix[x,y,z]!=0)
            //                            volumeRefs[xpart, ypart, zpart].Frames[0].Voxels[((zAxisOrientation == 0 ? sizex - 1 - x : x) - (MAX_VOLUME_DIMENSION * xpart)) + sizex * ((y - (MAX_VOLUME_DIMENSION * ypart)) + sizey * (z - (MAX_VOLUME_DIMENSION * zpart)))] = new Voxel()
            //                                {
            //                                    Active = true,
            //                                    Color = col,
            //                                    Value = 128
            //                                };
            //                    }
                
                                

            //    foreach (Volume v in volumeRefs)
            //    {
            //        v.CreateChunks();
            //        v.SaveForSerialize();
            //    }

            //    parentObject.transform.parent = importParentObject.transform;
            //}
            //else
            //{
                // Single volume
                var newObject =
                    Editor.Instantiate(EditorUtility.VoxelVolumePrefab, Vector3.zero, Quaternion.identity) as
                        GameObject;
                if (newObject != null)
                {
                    newObject.name = name!=""?name:volumeName;
                    newObject.GetComponent<Volume>().Material = EditorUtility.PicaVoxelDiffuseMaterial;
                    newObject.GetComponent<Volume>().GenerateBasic(FillMode.None);
                    Volume voxelVolume = newObject.GetComponent<Volume>();

                    voxelVolume.XSize = Convert.ToInt32(sizex);
                    voxelVolume.YSize = Convert.ToInt32(sizey);
                    voxelVolume.ZSize = Convert.ToInt32(sizez);
                    voxelVolume.Frames[0].XSize = Convert.ToInt32(sizex);
                    voxelVolume.Frames[0].YSize = Convert.ToInt32(sizey);
                    voxelVolume.Frames[0].ZSize = Convert.ToInt32(sizez);
                    voxelVolume.Frames[0].Voxels = new Voxel[sizex * sizey * sizez];
                    voxelVolume.VoxelSize = voxelSize;

                    if (zAxisOrientation == 0)
                    {
                        voxelVolume.Pivot = new Vector3(sizex, 0, 0)*voxelSize;
                        voxelVolume.UpdatePivot();
                    }

                    for (z = 0; z < sizez; z++)
                        for (y = 0; y < sizey; y++)
                            for (x = 0; x < sizex; x++)
                            {

                                Color col = UIntToColor(matrix[x, y, z], colorFormat);

                                if (matrix[x, y, z] != 0)
                                    voxelVolume.Frames[0].Voxels[(zAxisOrientation == 0 ? sizex - 1 - x : x) + sizex * (y + sizey * z)] = new Voxel()
                                    {
                                        Active = true,
                                        Color = col,
                                        Value = 128
                                    };
                            }


                    voxelVolume.CreateChunks();
                    voxelVolume.SaveForSerialize();

                    newObject.transform.position = (new Vector3((zAxisOrientation == 0 ? -posX : posX), posY, posZ) * voxelSize);
                    newObject.transform.parent = importParentObject.transform;
                }
           // }
            }


            
        }

        public static void QubicleImport(string fn, string volumeName, float voxelSize)
        {
            using(BinaryReader stream = new BinaryReader(new FileStream(fn, FileMode.Open)))
            {
                FromQubicle(stream, volumeName != "Qubicle Import" ? volumeName : Path.GetFileNameWithoutExtension(fn), voxelSize);
            }
        }

        private static Color32 UIntToColor(uint color, uint format)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;

            if (format == 0)
            {
                r = (byte)(color >> 0);
                g = (byte)(color >> 8);
                b = (byte)(color >> 16);
            }
            else
            {
                r = (byte)(color >> 16);
                g = (byte)(color >> 8);
                b = (byte)(color >> 0);
            }

            return new Color32(r, g, b, 255);
        }

    }
}
