using FellowOakDicom.Imaging.Mathematics;
using NUnit.Framework;
using RtDicom.RtStructureLib;

namespace RtDicom.RtStructureLibTests;

[TestFixture]
public class RTStructureSetTests
{
    [SetUp]
    public void Init()
    {


    }

    [TestCase("EHA_SCM")]
    [TestCase("GTV")]
    public void StructureNameTests(string id)
    {
        var rsFileInfo = new FileInfo(@"DCMFiles\FMISOX1\RS.FMISOX1.FMISO1.dcm");
        var ss = RtStructureSet.Factory.From(rsFileInfo);
        Assert.That(ss.Structures.Any(x=>x.Id == id));
    }

    [Test]
    public void SopInstanceUIDTest()
    {
        var rsFileInfo = new FileInfo(@"DCMFiles\FMISOX1\RS.FMISOX1.FMISO1.dcm");
        var ss = RtStructureSet.Factory.From(rsFileInfo);
        Assert.That("1.2.246.352.221.52146396471509650504834480941589396142" == ss.InstanceUID);
    } 
    
    [Test]
    public void FrameOfReferenceUIDTest()
    {
        var rsFileInfo = new FileInfo(@"DCMFiles\FMISOX1\RS.FMISOX1.FMISO1.dcm");
        var ss = RtStructureSet.Factory.From(rsFileInfo);
        Assert.That("1.2.246.352.221.568156797422154421513034962935421973380" == ss.FORs.FirstOrDefault());
    }

    [Test]
    public void StructureSetLabelTest()
    {
        var rsFileInfo = new FileInfo(@"DCMFiles\FMISOX1\RS.FMISOX1.FMISO1.dcm");
        var ss = RtStructureSet.Factory.From(rsFileInfo);
        Assert.That("FMISO1" == ss.Id);
    }

    [Test]
    public void StructureSetHowToUseTest()
    {
        var rsFileInfo = new FileInfo(@"DCMFiles\FMISOX1\RS.FMISOX1.FMISO1.dcm");
        var ss = RtStructureSet.Factory.From(rsFileInfo);
        
        var gtv = ss.Structures.FirstOrDefault(x=>x.Id == "GTV");
        var color = gtv?.Color;
        foreach (SliceContour sliceContour in gtv!.Contours)
        {
            
        }
    }
}