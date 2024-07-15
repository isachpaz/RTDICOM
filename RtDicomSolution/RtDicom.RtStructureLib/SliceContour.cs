using FellowOakDicom.Imaging.Mathematics;

namespace RtDicom.RtStructureLib;

public class SliceContour
{
    private List<Point2D> _contourPoints = new List<Point2D>();
    public IEnumerable<Point2D> ContourPoints => _contourPoints;

    public double Z { get; private set; } = float.NaN;

    internal void AddPoint(Point3D pt)
    {
        var z = pt.Z;
        if (double.IsNaN(Z))
        {
            Z = z;
        }

        if (Math.Abs(z - Z) > 1e-9)
        {
            throw new ArgumentException("Point not in same plane as other points in contour!");
        }

        _contourPoints.Add(new Point2D(pt.X, pt.Y));
    }

    public override string ToString()
    {
        return $"{nameof(Z)}: {Z}, Number of points on plain: {_contourPoints.Count}";
    }
}