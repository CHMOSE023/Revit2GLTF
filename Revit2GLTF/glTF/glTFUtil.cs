﻿using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit2Gltf.glTF
{
    public class glTFUtil
    {
        public static void addVec3BufferViewAndAccessor(GLTF gltf, glTFBinaryData bufferData)
        {
            var v3ds = bufferData.vertexBuffer;
            var byteOffset = 0;
            if (gltf.bufferViews.Count > 0)
            {
                byteOffset = gltf.bufferViews[gltf.bufferViews.Count - 1].byteLength + gltf.bufferViews[gltf.bufferViews.Count - 1].byteOffset;
            }
            var bufferIndex = 0;
            var vec3View = glTFUtil.addBufferView(bufferIndex, byteOffset, 4 * v3ds.Count);
            vec3View.target = Targets.ARRAY_BUFFER;
            gltf.bufferViews.Add(vec3View);
            var vecAccessor = glTFUtil.addAccessor(gltf.bufferViews.Count - 1, 0, ComponentType.FLOAT, v3ds.Count / 3, AccessorType.VEC3);
            var minAndMax = glTFUtil.GetVec3MinMax(v3ds);
            vecAccessor.min = new List<double>() { minAndMax[0], minAndMax[1], minAndMax[2] };
            vecAccessor.max = new List<double>() { minAndMax[3], minAndMax[4], minAndMax[5] };
            gltf.accessors.Add(vecAccessor);
        }

        public static void addNormalBufferViewAndAccessor(GLTF gltf, glTFBinaryData bufferData)
        {
            var v3ds = bufferData.normalBuffer;
            var byteOffset = 0;
            if (gltf.bufferViews.Count > 0)
            {
                byteOffset = gltf.bufferViews[gltf.bufferViews.Count - 1].byteLength + gltf.bufferViews[gltf.bufferViews.Count - 1].byteOffset;
            }
            var bufferIndex = 0;
            var vec3View = glTFUtil.addBufferView(bufferIndex, byteOffset, 4 * v3ds.Count);
            vec3View.target = Targets.ARRAY_BUFFER;
            gltf.bufferViews.Add(vec3View);
            var vecAccessor = glTFUtil.addAccessor(gltf.bufferViews.Count - 1, 0, ComponentType.FLOAT, v3ds.Count / 3, AccessorType.VEC3);
            gltf.accessors.Add(vecAccessor);
        }


        public static void addIndexsBufferViewAndAccessor(GLTF gltf,glTFBinaryData bufferData)
        {
            var byteOffset = 0;
            if (gltf.bufferViews.Count > 0)
            {
                byteOffset = gltf.bufferViews[gltf.bufferViews.Count - 1].byteLength + gltf.bufferViews[gltf.bufferViews.Count - 1].byteOffset;
            }
            var bufferIndex = 0;
            glTFBufferView faceView;
            glTFAccessor faceAccessor;
            var length = bufferData.indexBuffer.Count;
            if (bufferData.indexMax > 65535)
            {
                faceView = glTFUtil.addBufferView(bufferIndex, byteOffset, 4 * length);
                faceView.target = Targets.ELEMENT_ARRAY_BUFFER;
                gltf.bufferViews.Add(faceView);
                faceAccessor = glTFUtil.addAccessor(gltf.bufferViews.Count - 1, 0, ComponentType.UNSIGNED_INT, length, AccessorType.SCALAR);
            }
            else
            {
                var align = 0;
                if ((2 * length) % 4 != 0)
                {
                    align = 2;
                    bufferData.indexAlign = 0x20;
                }
                faceView = glTFUtil.addBufferView(bufferIndex, byteOffset, 2 * length + align);
                faceView.target = Targets.ELEMENT_ARRAY_BUFFER;
                gltf.bufferViews.Add(faceView);
                faceAccessor = glTFUtil.addAccessor(gltf.bufferViews.Count - 1, 0, ComponentType.UNSIGNED_SHORT, length, AccessorType.SCALAR);
            }
            gltf.accessors.Add(faceAccessor);
        }

        public static void addUvBufferViewAndAccessor(GLTF gltf, glTFBinaryData bufferData)
        {
            var uvs = bufferData.uvBuffer;
            var byteOffset = 0;
            if (gltf.bufferViews.Count > 0)
            {
                byteOffset = gltf.bufferViews[gltf.bufferViews.Count - 1].byteLength + gltf.bufferViews[gltf.bufferViews.Count - 1].byteOffset;
            }
            var bufferIndex = 0;
            var vec3View = glTFUtil.addBufferView(bufferIndex, byteOffset, 4 * uvs.Count);
            vec3View.target = Targets.ARRAY_BUFFER;
            gltf.bufferViews.Add(vec3View);
            var vecAccessor = glTFUtil.addAccessor(gltf.bufferViews.Count - 1, 0, ComponentType.FLOAT, uvs.Count / 2, AccessorType.VEC2);
            gltf.accessors.Add(vecAccessor);
        }

        public static glTFBufferView addBufferView(int bufferIndex, int byteOffset, int byteLength)
        {
            var bufferView = new glTFBufferView();
            bufferView.buffer = bufferIndex;
            bufferView.byteOffset = byteOffset;
            bufferView.byteLength = byteLength;
            return bufferView;
        }


        public static glTFAccessor addAccessor(int bufferView, int byteOffset, ComponentType componentType,int count, string type)
        {
            var accessor = new glTFAccessor();
            accessor.bufferView = bufferView;
            accessor.byteOffset = byteOffset;
            accessor.componentType = componentType;
            accessor.count = count;
            accessor.type = type;
            return accessor;
        }


        public static float[] GetVec3MinMax(List<float> vec3)
        {
            List<float> xValues = new List<float>();
            List<float> yValues = new List<float>();
            List<float> zValues = new List<float>();
            for (int i = 0; i < vec3.Count; i++)
            {
                if ((i % 3) == 0) xValues.Add(vec3[i]);
                if ((i % 3) == 1) yValues.Add(vec3[i]);
                if ((i % 3) == 2) zValues.Add(vec3[i]);
            }
            float maxX = xValues.Max();
            float minX = xValues.Min();
            float maxY = yValues.Max();
            float minY = yValues.Min();
            float maxZ = zValues.Max();
            float minZ = zValues.Min();
            return new float[] { minX, minY, minZ, maxX, maxY, maxZ };
        }



        public static void Align(Stream stream, byte pad = 0)
        {
            var count = 3 - ((stream.Position - 1) & 3);
            while (count != 0)
            {
                stream.WriteByte(pad);
                count--;
            }
        }

        public static List<glTFParameterGroup> GetParameter(Element element)
        {
            var parameterGroupMap = new Dictionary<string, glTFParameterGroup>();
            IList<Parameter> parameters = element.GetOrderedParameters();
            foreach (Parameter p in parameters)
            {
                string GroupName = LabelUtils.GetLabelFor(p.Definition.ParameterGroup);
                var parameter = new glTFParameter();
                parameter.name = p.Definition.Name;
                if (StorageType.String == p.StorageType)
                {
                    parameter.value = p.AsString();
                }
                else
                {
                    parameter.value = p.AsValueString();
                }

                if (parameterGroupMap.ContainsKey(GroupName))
                {
                    var propertySet = parameterGroupMap[GroupName];
                    propertySet.Parameters.Add(parameter);
                }
                else
                {
                    var propertySet = new glTFParameterGroup();
                    propertySet.GroupName = GroupName;
                    propertySet.Parameters = new List<glTFParameter>();
                    propertySet.Parameters.Add(parameter);
                    parameterGroupMap.Add(GroupName, propertySet);
                }
            }

            var parameterGroups = new List<glTFParameterGroup>();
            foreach (var item in parameterGroupMap.Values)
            {
                parameterGroups.Add(item);
            }
            return parameterGroups;
        }



        public static string FromFileExtension(string fileExtension)
        {
            switch (Path.GetExtension(fileExtension).ToLower())
            {
                case ".png":
                    return "image/png";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".dds":
                    return "image/vnd-ms.dds";
            }
            return "image/png";
        }


        public static string ReadAssetProperty(AssetProperty prop)
        {
            switch (prop.Type)
            {
#if  Revit2016 || Revit2017
                case AssetPropertyType.APT_String:
#else
                case AssetPropertyType.String:
#endif
                    AssetPropertyString val = prop as AssetPropertyString;
                    if (val.Name == "unifiedbitmap_Bitmap")
                    {
                        return val.Value;
                    }
                    break;
                // The APT_List contains a list of sub asset properties with same type.
#if  Revit2016 || Revit2017
                case AssetPropertyType.APT_List:
#else
                case AssetPropertyType.List:
#endif
                    AssetPropertyList propList = prop as AssetPropertyList;
                    IList<AssetProperty> subProps = propList.GetValue();
                    if (subProps.Count == 0)
                        break;
                    switch (subProps[0].Type)
                    {
#if Revit2016 || Revit2017
               case AssetPropertyType.APT_Integer:
#else
               case AssetPropertyType.Integer:
#endif
                            AssetPropertyString val2 = prop as AssetPropertyString;
                            if (val2!=null &&val2.Name == "unifiedbitmap_Bitmap")
                            {
                                return val2.Value;
                            }
                            break;
                    }
                    break;
#if Revit2016 || Revit2017
                case AssetPropertyType.APT_Asset:
#else
                case AssetPropertyType.Asset:
#endif
                    Asset propAsset = prop as Asset;
                    var value = ReadAsset(propAsset);
                    if (!string.IsNullOrEmpty(value))
                    {
                        return value;
                    }
                    break;
            }
            if (prop.NumberOfConnectedProperties == 0)
                return null;

            if (prop.Name == "generic_diffuse")
            {
                foreach (AssetProperty connectedProp in prop.GetAllConnectedProperties())
                {
                    var value = ReadAssetProperty(connectedProp);
                    if (!string.IsNullOrEmpty(value))
                    {
                        return value;
                    }
                }
            }
            return null;
        }
        public static string ReadAsset(Asset asset)
        {
            for (int idx = 0; idx < asset.Size; idx++)
            {
                AssetProperty prop = asset[idx];
                var value = ReadAssetProperty(prop);
                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }
            }
            return null ;
        }

        public static List<double> MakeQuaternion(XYZ u, XYZ v)
        {

            var n = u.CrossProduct(v);
            double s, tr = 1 + u.X + v.Y + n.Z;
            double X, Y, Z, W;
            if (tr > 1e-4)
            {
                s = 2 * Math.Sqrt(tr);
                W = s / 4;
                X = (v.Z - n.Y) / s;
                Y = (n.X - u.Z) / s;
                Z = (u.Y - v.X) / s;

            }
            else
            {
                if (u.X > v.Y && u.X > n.Z)
                {
                    s = 2 * Math.Sqrt(1 + u.X - v.Y - n.Z);
                    W = (v.Z - n.Y) / s;
                    X = s / 4;
                    Y = (u.Y + v.X) / s;
                    Z = (n.X + u.Z) / s;
                }
                else if (v.Y > n.Z)
                {
                    s = 2 * Math.Sqrt(1 - u.X + v.Y - n.Z);
                    W = (n.X - u.Z) / s;
                    X = (u.Y + v.X) / s;
                    Y = s / 4;
                    Z = (v.Z + n.Y) / s;
                }
                else
                {
                    s = 2 * Math.Sqrt(1 - u.X - v.Y + n.Z);
                    W = (u.Y - v.X) / s;
                    X = (n.X + u.Z) / s;
                    Y = (v.Z + n.Y) / s;
                    Z = s / 4;
                }
            }


            var magnitude = Math.Sqrt(W * W + X * X + Y * Y + Z * Z);
            var scale = 1 / magnitude;
            W = W * scale;
            X = X * scale;
            Y = Y * scale;
            Z = Z * scale;
            return new List<double>() { X, Y, Z, W };
        }

        /// <summary>
        /// 自定义方法，用递归找到包含贴图信息：
        /// </summary>
        /// <param name="ap"></param>
        /// <returns></returns>
        public static Asset FindTextureAsset(AssetProperty ap)
        {
            Asset result = null;
            if (ap.Type == AssetPropertyType.Asset)
            {
                if (!IsTextureAsset(ap as Asset))
                {
                    for (int i = 0; i < (ap as Asset).Size; i++)
                    {
                        if (null != FindTextureAsset((ap as Asset)[i]))
                        {
                            result = FindTextureAsset((ap as Asset)[i]);
                            break;
                        }
                    }
                }
                else
                {
                    result = ap as Asset;
                }
                return result;
            }
            else
            {
                for (int j = 0; j < ap.NumberOfConnectedProperties; j++)
                {
                    if (null != FindTextureAsset(ap.GetConnectedProperty(j)))
                    {
                        result = FindTextureAsset(ap.GetConnectedProperty(j));
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 自定义方法，判断Asset是否包含贴图信息：
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        private static bool IsTextureAsset(Asset asset)
        {
            AssetProperty assetProprty = GetAssetProprty(asset, "assettype");
            if (assetProprty != null && (assetProprty as AssetPropertyString).Value == "texture")
            {
                return true;
            }
            return GetAssetProprty(asset, "unifiedbitmap_Bitmap") != null;
        }

        /// <summary>
        /// 自定义方法，根据名字获取对应的AssetProprty：
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private static AssetProperty GetAssetProprty(Asset asset, string propertyName)
        {
            for (int i = 0; i < asset.Size; i++)
            {
                if (asset[i].Name == propertyName)
                {
                    return asset[i];
                }
            }
            return null;
        }

    }
}
