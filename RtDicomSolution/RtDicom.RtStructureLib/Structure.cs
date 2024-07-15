using FellowOakDicom.Imaging;

namespace RtDicom.RtStructureLib;

public class Structure
{
    public Color32 Color { get; }
    private readonly StructureMetaData _meta;
    private readonly List<SliceContour> _contours;

    public Structure(StructureMetaData meta, List<SliceContour> contours, Color32 color)
    {
        Color = color;
        _meta = meta;
        _contours = contours;
    }

    public string Id => _meta.StructureId;
    public IEnumerable<SliceContour> Contours => _contours;

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}";
    }
}