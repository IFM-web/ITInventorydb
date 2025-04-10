using System;
using System.Collections.Generic;
using ContentManagementSystem.Models;

namespace ContentManagementSystem.Models
{
    public class AssignedAssetsViewModel
    {
        public List<AssignedAssetItem> AssignedAssets { get; set; } = new List<AssignedAssetItem>();
    }

    public class AssignedAssetItem
    {
        public int MaterialOutId { get; set; }
        public string IssuanceDate { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string AssetName { get; set; }
        public string SerialNo { get; set; }
        public string ModelNo { get; set; }
        public string WarrantyDate { get; set; }
        public string AssignmentDate { get; set; }
        public string Status { get; set; }

        public string Generation { get; set; }
        public string? CPUCapacity { get; set; }
        public string? HardDisk { get; set; }
        public string? RAMCapacity { get; set; }
        public string? SSDCapacity { get; set; }
        public string? Processor { get; set; }
        public string? ItemName { get; set; }
        public string? Other { get; set; }
    }
} 