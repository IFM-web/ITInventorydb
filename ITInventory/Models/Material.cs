using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace ContentManagementSystem.Models
{
    public class Material
    {
        private DateTime _billDate;

        public Material()
        {
            MaterialItems = new List<MaterialItem>();
           // RecordDate = DateTime.Now;  // Set default value
        }

        
        public string Id { get; set; }
        
        [Required]
        public int CompanyId { get; set; }
        
        [Required]
        public int AssetItemId { get; set; }
        
        public int? VendorId { get; set; }
        
        public int? ManufacturerId { get; set; }

        
        // Custom names for Other types
        [Display(Name = "Custom Vendor")]
        public string CustomVendorName { get; set; }
        
        [Display(Name = "Custom Manufacturer")]
        public string CustomManufacturerName { get; set; }
        
        [Display(Name = "Custom Asset")]
        public string CustomAssetName { get; set; }
        
        [Required(ErrorMessage = "Invoice Number is required")]
        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }
        
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        public string BillDate 
        { 
            get;
            set ;
        }
        
        [NotMapped]
        public string BillDateString
        {
            get => _billDate.ToString("dd/MM/yyyy");
            set
            {
                if (DateTime.TryParseExact(value, "dd/MM/yyyy", 
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, 
                    out DateTime date))
                {
                    _billDate = date;
                }
            }
        }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        [Display(Name = "REQN Quantity")]
        public int ReqnQuantity { get; set; }
        
        public string ImagePath { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string RecordDate { get; set; }

        // Navigation properties
        public string Company { get; set; }
        public string AssetItem { get; set; }
        public string Vendor { get; set; }
        public string Manufacturer { get; set; }
        public string TotalItem { get; set; }
        public virtual ICollection<MaterialItem> MaterialItems { get; set; }
    }


    


    /*
    public class MaterialItem
    {
        public MaterialItem()
        {
            Status = "UnAssigned";  // Set default value
        }

        public int Id { get; set; }
        public int MaterialId { get; set; }
        
        public string Status { get; set; }
        
        // Common fields
        [Required]
        public string SerialNo { get; set; }
        [Required]
        public string ModelNo { get; set; }
        
        // Fields for Desktop/Laptop
        public string Generation { get; set; }
        public int? CPUCapacity { get; set; }
        public int? HardDisk { get; set; }
        public int? RAMCapacity { get; set; }
        public int? SSDCapacity { get; set; }
        public string Other { get; set; }
        
        // Fields for Other items
        public string ItemName { get; set; }
        [Display(Name = "Warranty Date")]
        public DateTime? WarrantyDate { get; set; }

        public Material Material { get; set; }
    }
    */

    
} 