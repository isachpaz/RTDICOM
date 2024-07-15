namespace RtDicom.RtStructureLib
{
    public partial class RtStructureSet
    {
        private readonly List<Structure> _structures;
        public FileInfo FileInfo { get; }
        public string InstanceUID { get; }
        public List<string> FORs { get; }

        public IEnumerable<Structure> Structures => _structures;

        public string Id { get; }

        internal RtStructureSet(FileInfo fileInfo, string instanceUid, IEnumerable<string> forUids, List<Structure> structures, string id)
        {
            FileInfo = fileInfo;
            InstanceUID = instanceUid;
            FORs = new List<string>(forUids);
            _structures = structures;
            Id = id;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, Structures: {string.Join(", ", _structures.Select(x=>x.Id))}";
        }
    }
}
