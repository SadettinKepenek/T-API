namespace T_API.Core.DTO.DatabasePackage
{
    public class ListDatabasePackageDto
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public bool IsApiSupport { get; set; }
        public bool IsAuthSupport { get; set; }
        public bool IsStorageSupport { get; set; }
        public bool IsViewSupport { get; set; }
        public bool IsStoredProcedureSupport { get; set; }
        public bool IsUserDefinedFunctionSupport { get; set; }
        public bool IsTriggerSupport { get; set; }
        public bool IsJobSupport { get; set; }
        public int ApiRequestCount { get; set; }
        public int MaxColumnPerTable { get; set; }
        public int MaxTableCount { get; set; }
        public int MaxStoredProcedureCount { get; set; }
        public int MaxUserDefinedFunctionCount { get; set; }
        public int MaxTriggerCount { get; set; }
        public int MaxJobCount { get; set; }
        public int MaxViewCount { get; set; }
    }
}