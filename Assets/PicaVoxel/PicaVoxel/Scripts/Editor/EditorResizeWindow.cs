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
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


namespace PicaVoxel
{
    public class EditorResizeWindow : EditorWindow
    {
        private Volume voxelObject;

        private int xSize = 0;
        private int ySize = 0;
        private int zSize = 0;

        private AnchorX anchorX;
        private AnchorY anchorY;
        private AnchorZ anchorZ;

        private bool fillVoxels = true;

        public void Init(Volume vo)
        {
            voxelObject = vo;

            xSize = voxelObject.XSize;
            ySize = voxelObject.YSize;
            zSize = voxelObject.ZSize;

            title = "Resize";
            
        }

        public void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(
                "Resize: " + voxelObject.name + " (" + voxelObject.XSize + "," + voxelObject.YSize + "," +
                voxelObject.ZSize + ")", new GUIStyle() {fontStyle = FontStyle.Bold});
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("X:", new[] {GUILayout.Width(30)});
            xSize = EditorGUILayout.IntField(xSize);
            EditorGUILayout.LabelField("Y:", new[] {GUILayout.Width(30)});
            ySize = EditorGUILayout.IntField(ySize);
            EditorGUILayout.LabelField("Z:", new[] {GUILayout.Width(30)});
            zSize = EditorGUILayout.IntField(zSize);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            anchorX = (AnchorX) EditorGUILayout.EnumPopup("X Anchor: ", anchorX);
            anchorY = (AnchorY) EditorGUILayout.EnumPopup("Y Anchor: ", anchorY);
            anchorZ = (AnchorZ) EditorGUILayout.EnumPopup("Z Anchor: ", anchorZ);

            EditorGUILayout.Space();
            fillVoxels = EditorGUILayout.ToggleLeft(" Fill any added space", fillVoxels);

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Resize"))
            {
                if (xSize < 1) xSize = 1;
                if (ySize < 1) ySize = 1;
                if (zSize < 1) zSize = 1;
               // int totalSize = xSize*ySize*zSize;

                //if (totalSize > 64*64*64)
                //{
                //    UnityEditor.EditorUtility.DisplayDialog("PicaVoxel",
                //        "The largest size of an individual PicaVoxel Volume is 64*64*64 voxels TOTAL. Decrease size in one axis to increase the other two. If you wish to make a larger volume, select PicaVoxel Multi-Part Volume from the Create menu.",
                //        "OK");
                //}
                //else
                //{
                    List<Object> undoObjects = new List<Object>();
                    
                    foreach (Frame frame in voxelObject.Frames)
                    {
                        undoObjects.Add(frame);
                    }
                    undoObjects.Add(voxelObject);
                    
                    Undo.RecordObjects(undoObjects.ToArray(), "Resize Voxel Object");
                    foreach (Frame frame in voxelObject.Frames) UnityEditor.EditorUtility.SetDirty(frame);
                    UnityEditor.EditorUtility.SetDirty(voxelObject);

                    PicaVoxelBox copyDestBox =
                        new PicaVoxelBox(
                            anchorX == AnchorX.Left
                                ? 0
                                : anchorX == AnchorX.Center
                                    ? (xSize/2) - (voxelObject.XSize/2)
                                    : xSize - voxelObject.XSize,
                            anchorY == AnchorY.Bottom
                                ? 0
                                : anchorY == AnchorY.Center
                                    ? (ySize/2) - (voxelObject.YSize/2)
                                    : ySize - voxelObject.YSize,
                            anchorZ == AnchorZ.Front
                                ? 0
                                : anchorZ == AnchorZ.Center
                                    ? (zSize/2) - (voxelObject.ZSize/2)
                                    : zSize - voxelObject.ZSize,
                            (anchorX == AnchorX.Left
                                ? 0
                                : anchorX == AnchorX.Center
                                    ? (xSize/2) - (voxelObject.XSize/2)
                                    : xSize - voxelObject.XSize) +
                            (voxelObject.XSize - 1),
                            (anchorY == AnchorY.Bottom
                                ? 0
                                : anchorY == AnchorY.Center
                                    ? (ySize/2) - (voxelObject.YSize/2)
                                    : ySize - voxelObject.YSize) +
                            (voxelObject.YSize - 1),
                            (anchorZ == AnchorZ.Front
                                ? 0
                                : anchorZ == AnchorZ.Center
                                    ? (zSize/2) - (voxelObject.ZSize/2)
                                    : zSize - voxelObject.ZSize) +
                            (voxelObject.ZSize - 1));


                    foreach (Frame frame in voxelObject.Frames)
                    {
                        
                        Voxel[] newVox = new Voxel[xSize* ySize* zSize];

                        if (fillVoxels)
                        {
                            for (int x = 0; x < xSize; x++)
                                for (int y = 0; y < ySize; y++)
                                    for (int z = 0; z < zSize; z++)
                                        newVox[x + xSize * (y + ySize * z)] = new Voxel()
                                        {
                                            Active = true,
                                            Color = voxelObject.PaletteColors[0],
                                            Value = 128
                                        };
                        }

                        int destX = copyDestBox.BottomLeftFront.X;
                        int destY = copyDestBox.BottomLeftFront.Y;
                        int destZ = copyDestBox.BottomLeftFront.Z;
                        for (int x = 0; x < voxelObject.XSize; x++)
                        {
                            for (int y = 0; y < voxelObject.YSize; y++)
                            {
                                for (int z = 0; z < voxelObject.ZSize; z++)
                                {
                                    if (destX < 0 || destY < 0 || destZ < 0 || destX >= xSize ||
                                        destY >= ySize || destZ >= zSize)
                                    {
                                        destZ++;
                                        continue;
                                    }

                                    newVox[destX + xSize *(destY + ySize * destZ)] = frame.Voxels[x +frame.XSize * (y + frame.YSize* z)];
                                    destZ++;
                                }
                                destZ = copyDestBox.BottomLeftFront.Z;
                                destY++;
                            }
                            destY = copyDestBox.BottomLeftFront.Y;
                            destX++;
                        }

                        frame.XSize = xSize;
                        frame.YSize = ySize;
                        frame.ZSize = zSize;
                        frame.EditingVoxels = null;
                        frame.Voxels = newVox;
                    }


                    voxelObject.XSize = xSize;
                    voxelObject.YSize = ySize;
                    voxelObject.ZSize = zSize;
                    voxelObject.CreateChunks();

                    voxelObject.SaveForSerialize();

                    //EditorUtility.SetDirty(voxelObject);


                    Close();
                }
           // }
            if (GUILayout.Button("Cancel")) Close();
            EditorGUILayout.EndHorizontal();
        }
    }

}