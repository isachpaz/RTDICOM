using itk.simple;
using System.Threading.Tasks;

namespace RtDicom.Pet3DVolume;

public class Program
{
    public static void Main()
    {
        string dicomDirectory = @"F:\FMISOX1\PET\";

        var files = Directory.GetFiles(dicomDirectory).ToList();

        // Read DICOM series (for reference)
        ImageSeriesReader reader = new ImageSeriesReader();
        VectorString vs = new VectorString();
        files.ForEach(x => vs.Add(x));

        reader.SetFileNames(vs);
        VectorString dicomNames = reader.GetFileNames();
        
        Image image3D = reader.Execute();
        var origin = image3D.GetOrigin();
        Console.WriteLine("Hello World!");

        string rtStructureFile = "path_to_your_rt_structure_file.dcm";
        ReadRTStructure(rtStructureFile, image3D);
    }

    static void ReadRTStructure(string rtStructureFile, Image referenceImage)
    {
        // Load RT Structure set
        RtStructureSet rtStructSet = new RtStructureSet();
        rtStructSet.Load(rtStructureFile);

        // Get the structure names
        string[] structureNames = rtStructSet.GetStructureNames();

        // Select a structure (example: using the first structure found)
        string structureName = structureNames[0];

        // Get the contour data for the selected structure
        ContourData contourData = rtStructSet.GetContours(structureName);

        // Convert contour data to a binary mask
        Image maskImage = ConvertContourDataToMask(contourData, referenceImage);

        // Save the mask image to a file (optional)
        string outputMaskFile = "output_mask.nii";
        ImageFileWriter writer = new ImageFileWriter();
        writer.SetFileName(outputMaskFile);
        writer.Execute(maskImage);

        Console.WriteLine("Mask created and saved as " + outputMaskFile);
    }

    static Image ConvertContourDataToMask(ContourData contourData, Image referenceImage)
    {
        // Initialize a new image with the same size and spacing as the reference image
        Image maskImage = new Image(referenceImage.GetSize(), PixelIDValueEnum.UInt8);
        maskImage.SetSpacing(referenceImage.GetSpacing());
        maskImage.SetOrigin(referenceImage.GetOrigin());

        // Set all voxels in the mask to 0 (background)
        maskImage.Fill(0);

        // Iterate over each contour
        foreach (var contour in contourData.Contours)
        {
            // Create a polygon from the contour points
            Polygon polygon = new Polygon();
            foreach (var point in contour.Points)
            {
                polygon.AddPoint(point.x, point.y);
            }

            // Convert the polygon to a binary mask on the maskImage
            Mask.CreateMaskFromPolygon(polygon, maskImage, 1);
        }

        return maskImage;
    }
}