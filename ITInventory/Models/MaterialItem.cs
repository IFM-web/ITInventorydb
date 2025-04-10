using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace ContentManagementSystem.Models
{
    public class MaterialItem
    {
        private DateTime? _warrantyDate;

      
        public string InvoiceNo { get; set; }
        public string CustomAssetName { get; set; }
        public string CustomVendorName { get; set; }
        public string Manufacturer { get; set; }
        public string CustomManufacturerName { get; set; }
        public string Vendor { get; set; }
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public string AssetItem { get; set; }  // Add this property
        public string Status { get; set; }

        // Common fields
        [Required]
        public string SerialNo { get; set; }
        [Required]
        public string ModelNo { get; set; }
        //public string AssetTypeid { get; set; }
        // Fields for Desktop/Laptop
        public string Generation { get; set; }
        public string? CPUCapacity { get; set; }
        public string? HardDisk { get; set; }
        public string? RAMCapacity { get; set; }
        public string? SSDCapacity { get; set; }
        public string Other { get; set; }

        // Fields for Other items
        public string ItemName { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        public DateTime? WarrantyDate
        {
            get => _warrantyDate;
            set => _warrantyDate = value;
        }

        string SearchTerm { get; set; }

        [NotMapped]
        public string WarrantyDateString
        {
            get => _warrantyDate?.ToString("dd/MM/yyyy");
            set
            {
                if (DateTime.TryParseExact(value, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime date))
                {
                    _warrantyDate = date;
                }
            }
        }

        public string  BillDate { get; set; } 

        public string RecordDate { get; set; } 
        public string WindowsKey { get; set; }
        public string MSOfficeKey { get; set; }
        public string ReceiverName { get; set; }
        // Navigation properties
        //public virtual Material Material { get; set; }

    }


}