namespace RtDicom.RtStructureLib;

public class StructureMetaData
{
    public string StructureId { get; }
    public int RoiNumber { get; }

    public StructureMetaData(string structureId, int roiNumber)
    {
        StructureId = structureId;
        RoiNumber = roiNumber;
    }

    public override string ToString()
    {
        return $"{nameof(StructureId)}: {StructureId}, {nameof(RoiNumber)}: {RoiNumber}";
    }
}