﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Revit2Gltf.glTF
{

    public class GLTF
    {
        public List<string> extensionsUsed;
        public List<string> extensionsRequired;
        public glTFVersion asset;
        public List<glTFScene> scenes;
        public List<glTFCameras> cameras;
        public List<glTFNode> nodes;
        public List<glTFMesh> meshes;
        public List<glTFAccessor> accessors;
        public List<glTFBufferView> bufferViews;
        public List<glTFBuffer> buffers;
        public List<glTFMaterial> materials;
        public List<glTFImage> images;
        public List<glTFTexture> textures;
        public List<glTFSampler> samplers;
        public string toJson()
        {
            string jsonStr = JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                //Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });
            return jsonStr;
        }
    }

    public class GLB
    {

        public const uint Magic = 0x46546C67;

        public const uint Version = 2;

        public const uint HeaderLength = sizeof(uint) + sizeof(uint) + sizeof(int);

        public const uint ChunkHeaderLength = sizeof(uint) + sizeof(uint);

        public const uint ChunkFormatJson = 0x4E4F534A;

        public const uint ChunkFormatBin = 0x004E4942;
    }



    public class glTFVersion
    {
        public string generator = "exportGLTF by:https://github.com/cowboy1997/Revit2GLTF";
        public string version = "2.0";
        public Dictionary<string, object> extras { get; set; }
        public Dictionary<string, object> extensions { get; set; }
    }

    public class glTFScene
    {
        public List<int> nodes = new List<int>();
    }


    public class glTFNode
    {

        public string name { get; set; }

        public int? mesh { get; set; } = null;

        public int? camera { get; set; } = null;

        public List<double> rotation { get; set; }

        public List<double> translation { get; set; }


        public List<double> matrix { get; set; }


        public List<int> children { get; set; }

        public Dictionary<string, object> extensions { get; set; }
        public Dictionary<string, object> extras { get; set; }
    }

    public class glTFParameterGroup
    {
        public string GroupName { get; set; }
        public List<glTFParameter> Parameters { get; set; }
    }
    public class glTFParameter
    {
        public string value { get; set; }
        public string name { get; set; }
    }



    public class glTFMesh
    {

        public List<glTFMeshPrimitive> primitives { get; set; }
    }


    public class glTFMeshPrimitive
    {

        public glTFAttribute attributes { get; set; } = new glTFAttribute();

        public int indices { get; set; }

        public int? material { get; set; } = null;
        public ModeEnum mode { get; set; } = ModeEnum.TRIANGLES;

        public glTFPrimitiveExtensions extensions { get; set; }
    }


    public enum ModeEnum
    {
        POINTS = 0,
        LINES = 1,
        LINE_LOOP = 2,
        LINE_STRIP = 3,
        TRIANGLES = 4,
        TRIANGLE_STRIP = 5,
        TRIANGLE_FAN = 6
    }

    public class glTFAttribute
    {
        public int? POSITION { get; set; }
        public int? NORMAL { get; set; }

        public int? TEXCOORD_0 { get; set; }
        public int? _BATCHID { get; set; }
    }


    public class glTFPrimitiveExtensions
    {
        public glTFDracoMesh KHR_draco_mesh_compression { get; set; } = new glTFDracoMesh();
    }

    public class glTFDracoMesh
    {
        public int? bufferView { get; set; } = null;
        public glTFAttribute attributes { get; set; } = new glTFAttribute();
    }



    public class glTFBuffer
    {
        public string uri { get; set; }
        public int byteLength { get; set; }
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class glTFBufferView
    {
        public string name { get; set; }

        public int buffer { get; set; }
        public int byteOffset { get; set; }
        public int byteLength { get; set; }
        public Targets? target { get; set; }
        public int? byteStride { get; set; }

        [JsonIgnore]
        public string Base64 { get; set; }
    }
    public enum Targets
    {
        ARRAY_BUFFER = 34962, // 代表顶点数据
        ELEMENT_ARRAY_BUFFER = 34963 // 代表顶点索引数据
    }



    public class glTFAccessor
    {

        public string name { get; set; }
        public int? bufferView { get; set; }

        public int? byteOffset { get; set; }
        public ComponentType componentType { get; set; }
        public int count { get; set; }
        public string type { get; set; }
        public List<double> max { get; set; }
        public List<double> min { get; set; }

    }

    public class AccessorType
    {
        public static string VEC3 = "VEC3";
        public static string VEC2 = "VEC2";
        public static string SCALAR = "SCALAR";
    }



    public enum ComponentType
    {
        BYTE = 5120,
        UNSIGNED_BYTE = 5121,
        SHORT = 5122,
        UNSIGNED_SHORT = 5123,
        UNSIGNED_INT = 5125,
        FLOAT = 5126
    }


    public class glTFMaterial
    {

        public string name { get; set; }

        public glTFPBR pbrMetallicRoughness { get; set; }

        public string alphaMode { get; set; }

        public bool? doubleSided { get; set; }


        [JsonIgnore]
        public int index { get; set; }
    }
    public class glTFPBR
    {

        public glTFbaseColorTexture baseColorTexture { get; set; }

        public List<double> baseColorFactor { get; set; }
        /// <summary>
        /// 金属度，范围从0（非金属）到1（金属）
        /// </summary>
        public double? metallicFactor { get; set; }
        /// <summary>
        /// 粗糙度，范围从0.0（平滑）到1.0（粗糙）。
        /// </summary>
        public double? roughnessFactor { get; set; }
    }


    public class glTFbaseColorTexture
    {
        public int? index { get; set; } = null;
    }



    public class glTFTexture
    {

        public int? source { get; set; } = null;

        public int? sampler { get; set; } = null;

    }
    public class glTFImage
    {

        public string uri { get; set; }

        public int? bufferView { get; set; }

        public string mimeType { get; set; }

        public string name { get; set; }
    }

    public class glTFSampler
    {
        public double magFilter { get; set; }

        public double minFilter { get; set; }
        public double wrapS { get; set; }
        public double wrapT { get; set; }
    }


    public class glTFCameras
    {
        public string type { get; set; }

        public glTFPerspectiveCamera perspective { get; set; } = null;

        public glTFOrthographicCamera orthographic { get; set; } = null;
    }

    public class CameraType
    {
        public static string perspective = "perspective";
        public static string orthographic = "orthographic";
    }

    public class glTFPerspectiveCamera
    {
        public double aspectRatio { get; set; }

        public double yfov { get; set; }
        public double zfar { get; set; }
        public double znear { get; set; }
    }

    public class glTFOrthographicCamera
    {
        public double xmag { get; set; }

        public double ymag { get; set; }
        public double zfar { get; set; }
        public double znear { get; set; }
    }

    public class glTFBinaryData
    {

        public List<float> vertexBuffer { get; set; } = new List<float>();


        public List<float> normalBuffer { get; set; } = new List<float>();

        public List<int> indexBuffer { get; set; } = new List<int>();

        public List<float> uvBuffer { get; set; } = new List<float>();

        public List<int> batchidBuffer { get; set; } = new List<int>();
        public int? indexMax { get; set; }
        public int? indexAlign { get; set; }


        public IntPtr dracoData { get; set; }
        public int dracoSize { get; set; }
    }
}



