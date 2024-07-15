using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.Mathematics;

namespace RtDicom.RtStructureLib;

public partial class RtStructureSet
{
    public static class Factory
    {
        public static RtStructureSet From(FileInfo rsFileInfo)
        {
            var dicomFile = DicomFile.Open(rsFileInfo.FullName);
            var structureSet = dicomFile.Dataset.GetDicomItem<DicomSequence>(DicomTag.StructureSetROISequence);

            var structureSetMetas = dicomFile
                .Dataset
                .GetSequence(DicomTag.StructureSetROISequence)
                .Items
                .Select(x =>
                {
                    var structureId = x.GetSingleValue<string>(DicomTag.ROIName);
                    var roiNumber = x.GetSingleValue<int>(DicomTag.ROINumber);
                    return new StructureMetaData(structureId, roiNumber);
                });


            List<Structure> structures = new List<Structure>();

            foreach (StructureMetaData meta in structureSetMetas)
            {
                var contourSeqMatch = dicomFile.Dataset.GetSequence(DicomTag.ROIContourSequence).Items
                    .FirstOrDefault(x => x.GetSingleValue<int>(DicomTag.ReferencedROINumber) == meta.RoiNumber);
                var roiSeqMatch = dicomFile.Dataset.GetSequence(DicomTag.RTROIObservationsSequence).Items
                    .FirstOrDefault(x => x.GetSingleValue<int>(DicomTag.ReferencedROINumber) == meta.RoiNumber);
                
                

                if (contourSeqMatch != null)
                {
                    var contours = new List<SliceContour>();

                    var contourSeq = contourSeqMatch.GetSequence(DicomTag.ContourSequence);
                    var colorRGB = contourSeqMatch.GetValues<int>(DicomTag.ROIDisplayColor);

                    Color32 color32 = new Color32()
                    {
                        R = (byte)colorRGB[0],
                        G = (byte)colorRGB[1],
                        B = (byte)colorRGB[2]
                    };
                        
                    foreach (var contourItem in contourSeq.Items)
                    {
                        
                        var contourData = contourItem.GetValues<float>(DicomTag.ContourData);
                        var sliceContour = new SliceContour();

                        for (int i = 0; i < contourData.Length; i += 3)
                        {
                            var point = new Point3D(contourData[i], contourData[i + 1], contourData[i + 2]);
                            sliceContour.AddPoint(point);
                        }

                        contours.Add(sliceContour);
                    }

                    var structure = new Structure(meta, contours, color32);
                    structures.Add(structure);
                }
            }

            var instanceUID = dicomFile.Dataset.GetSingleValue<string>(DicomTag.SOPInstanceUID);

            var fors = dicomFile.Dataset.GetSequence(DicomTag.ReferencedFrameOfReferenceSequence).Items
                .Select(x => x.GetSingleValue<string>(DicomTag.FrameOfReferenceUID));

            string label = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.StructureSetLabel, "");
            string creationDate = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.StructureSetLabel, "");
            return new RtStructureSet(rsFileInfo, instanceUID, fors, structures, label);
        }
    }
}