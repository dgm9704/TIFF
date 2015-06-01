﻿namespace Diwen.Tiff
{
    using System;

    public enum Tag : ushort
    {
        Unknown = 0,
        NewSubfileType = 254,
        [Obsolete("NewSubfileType should be used instead.")]
        SubfileType = 255,
        ImageWidth = 256,
        ImageLength = 257,
        BitsPerSample = 258,
        Compression = 259,
        PhotometricInterpretation = 262,
        Threshholding = 263,
        CellWidth = 264,
        CellLength = 265,
        FillOrder = 266,
        DocumentName = 269,
        ImageDescription = 270,
        Make = 271,
        Model = 272,
        StripOffsets = 273,
        Orientation = 274,
        SamplesPerPixel = 277,
        RowsPerStrip = 278,
        StripByteCounts = 279,
        MinSampleValue = 280,
        MaxSampleValue = 281,
        XResolution = 282,
        YResolution = 283,
        PlanarConfiguration = 284,
        PageName = 285,
        XPosition = 286,
        YPosition = 287,
        FreeOffsets = 288,
        FreeByteCounts = 289,
        GrayResponseUnit = 290,
        GrayResponseCurve = 291,
        T4Options = 292,
        T6Options = 293,
        ResolutionUnit = 296,
        PageNumber = 297,
        TransferFunction = 301,
        Software = 305,
        DateTime = 306,
        Artist = 315,
        HostComputer = 316,
        Predictor = 317,
        WhitePoint = 318,
        PrimaryChromaticities = 319,
        ColorMap = 320,
        HalftoneHints = 321,
        TileWidth = 322,
        TileLength = 323,
        TileOffsets = 324,
        TileByteCounts = 325,
        BadFaxLines = 326,
        CleanFaxData = 327,
        ConsecutiveBadFaxLines = 328,
        SubIFDs = 330,
        InkSet = 332,
        InkNames = 333,
        NumberOfInks = 334,
        DotRange = 336,
        TargetPrinter = 337,
        ExtraSamples = 338,
        SampleFormat = 339,
        SMinSampleValue = 340,
        SMaxSampleValue = 341,
        TransferRange = 342,
        ClipPath = 343,
        XClipPathUnits = 344,
        YClipPathUnits = 345,
        Indexed = 346,
        JPEGTables = 347,
        OPIProxy = 351,
        GlobalParametersIFD = 400,
        ProfileType = 401,
        FaxProfile = 402,
        CodingMethods = 403,
        VersionYear = 404,
        ModeNumber = 405,
        Decode = 433,
        DefaultImageColor = 434,
        JPEGProc = 512,
        JPEGInterchangeFormat = 513,
        JPEGInterchangeFormatLength = 514,
        JPEGRestartInterval = 515,
        JPEGLosslessPredictors = 517,
        JPEGPointTransforms = 518,
        JPEGQTables = 519,
        JPEGDCTables = 520,
        JPEGACTables = 521,
        YCbCrCoefficients = 529,
        YCbCrSubSampling = 530,
        YCbCrPositioning = 531,
        ReferenceBlackWhite = 532,
        StripRowCounts = 559,
        XMP = 700,
        ImageID = 32781,
        WangAnnotation = 32932,
        Copyright = 33432,
        ModelPixelScale = 33550,
        IPTC = 33723,
        INGRPacketData = 33918,
        INGRFlagRegisters = 33919,
        IrasBTransformationMatrix = 33920,
        ModelTiepoint = 33922,
        ModelTransformation = 34264,
        Photoshop = 34377,
        ExifIFD = 34665,
        ICCProfile = 34675,
        ImageLayer = 34732,
        GeoKeyDirectory = 34735,
        GeoDoubleParams = 34736,
        GeoAsciiParams = 34737,
        GPSIFD = 34853,
        HylaFAXFaxRecvParams = 34908,
        HylaFAXFaxSubAddress = 34909,
        HylaFAXFaxRecvTime = 34910,
        ImageSourceData = 37724,
        InteroperabilityIFD = 40965,
        GDALMetadata = 42112,
        GDALNoData = 42113,
        OceScanjobDescription = 50215,
        OceApplicationSelector = 50216,
        OceIdentificationNumber = 50217,
        OceImageLogicCharacteristics = 50218,
        DNGVersion = 50706,
        DNGBackwardVersion = 50707,
        UniqueCameraModel = 50708,
        LocalizedCameraModel = 50709,
        CFAPlaneColor = 50710,
        CFALayout = 50711,
        LinearizationTable = 50712,
        BlackLevelRepeatDim = 50713,
        BlackLevel = 50714,
        BlackLevelDeltaH = 50715,
        BlackLevelDeltaV = 50716,
        WhiteLevel = 50717,
        DefaultScale = 50718,
        DefaultCropOrigin = 50719,
        DefaultCropSize = 50720,
        ColorMatrix1 = 50721,
        ColorMatrix2 = 50722,
        CameraCalibration1 = 50723,
        CameraCalibration2 = 50724,
        ReductionMatrix1 = 50725,
        ReductionMatrix2 = 50726,
        AnalogBalance = 50727,
        AsShotNeutral = 50728,
        AsShotWhiteXY = 50729,
        BaselineExposure = 50730,
        BaselineNoise = 50731,
        BaselineSharpness = 50732,
        BayerGreenSplit = 50733,
        LinearResponseLimit = 50734,
        CameraSerialNumber = 50735,
        LensInfo = 50736,
        ChromaBlurRadius = 50737,
        AntiAliasStrength = 50738,
        ShadowScale = 50739,
        DNGPrivateData = 50740,
        MakerNoteSafety = 50741,
        CalibrationIlluminant1 = 50778,
        CalibrationIlluminant2 = 50779,
        BestQualityScale = 50780,
        RawDataUniqueID = 50781,
        AliasLayerMetadata = 50784,
        OriginalRawFileName = 50827,
        OriginalRawFileData = 50828,
        ActiveArea = 50829,
        MaskedAreas = 50830,
        AsShotICCProfile = 50831,
        AsShotPreProfileMatrix = 50832,
        CurrentICCProfile = 50833,
        CurrentPreProfileMatrix = 50834,
        ColorimetricReference = 50879,
        CameraCalibrationSignature = 50931,
        ProfileCalibrationSignature = 50932,
        ExtraCameraProfiles = 50933,
        AsShotProfileName = 50934,
        NoiseReductionApplied = 50935,
        ProfileName = 50936,
        ProfileHueSatMapDims = 50937,
        ProfileHueSatMapData1 = 50938,
        ProfileHueSatMapData2 = 50939,
        ProfileToneCurve = 50940,
        ProfileEmbedPolicy = 50941,
        ProfileCopyright = 50942,
        ForwardMatrix1 = 50964,
        ForwardMatrix2 = 50965,
        PreviewApplicationName = 50966,
        PreviewApplicationVersion = 50967,
        PreviewSettingsName = 50968,
        PreviewSettingsDigest = 50969,
        PreviewColorSpace = 50970,
        PreviewDateTime = 50791,
        RawImageDigest = 50792,
        OriginalRawFileDigest = 50973,
        SubTileBlockSize = 50974,
        RowInterleaveFactor = 50975,
        ProfileLookTableDims = 50981,
        ProfileLookTableData = 50982,
        OpcodeList1 = 51008,
        OpcodeList2 = 51009,
        OpcodeList3 = 51022,
        NoiseProfile = 51041,
    }
}
