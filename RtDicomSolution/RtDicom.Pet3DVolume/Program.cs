using itk.simple;

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
    }
}