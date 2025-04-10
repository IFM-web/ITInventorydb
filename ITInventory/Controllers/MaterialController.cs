
using ContentManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Linq.Expressions;
using Microsoft.VisualBasic;
using GuardTour;
using System.Data;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static ContentManagementSystem.Filters.AuthenticationFilter;
using ContentManagementSystem.Filters;

namespace ContentManagementSystem.Controllers
{
    [AuthenticationFilter]
    public class MaterialController : Controller
    {
       db_Utility util=new db_Utility();
       

        public IActionResult Index()
        {
            try

            {
              


                ViewBag.Companies = util.PopulateDropDown("select * from Companies where id=1", util.strElect);
                ViewBag.AssetItems = util.PopulateDropDown("select * from AssetItems", util.strElect);
                ViewBag.Vendors = util.PopulateDropDown("select * from Vendors", util.strElect);
                ViewBag.Manufacturers = util.PopulateDropDown("select * from Manufacturers", util.strElect);





                return View();
            }
            catch (Exception ex)
            {
               
                return View();
            }
        }

       

        private DateTime? ParseDate(string dateStr)
        {
            if (string.IsNullOrEmpty(dateStr))
                return null;

            try
            {
                var dateParts = dateStr.Split('/');
                if (dateParts.Length == 3)
                {
                    int day = int.Parse(dateParts[0]);
                    int month = int.Parse(dateParts[1]);
                    int year = int.Parse(dateParts[2]);
                    return new DateTime(year, month, day);
                }
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }

        [HttpPost]
        public IActionResult Addnew(IFormFile? file)
        {
            try
            {
                string Invoice = Request.Form["Invoice"].ToString();
                string Companyid = Request.Form["Companyid"].ToString();
                string AssetItem = Request.Form["AssetItem"].ToString();
                string customAssetName = Request.Form["customAssetName"].ToString();
                string Vendor = Request.Form["Vendor"].ToString();
                string customVendorName = Request.Form["customVendorName"].ToString();
                string Manufacturer = Request.Form["Manufacturer"].ToString();
                string customManufacturerName = Request.Form["customManufacturerName"].ToString();
                string BillDate = Request.Form["BillDate"].ToString();
               

                
                string ReceivedDate = Request.Form["ReceivedDate"].ToString();
                string reqnQuantity = Request.Form["reqnQuantity"].ToString();
                //string invoiceUpload = Request.Form["FileName"].ToString();
                string JsonData = Request.Form["JsonData"].ToString();
                var objects = JArray.Parse(JsonData);
                string Id = "";
                string Metrailno = "";
                string fileName = "";
                foreach (var e in objects)
                {
                    var ds2 = util.Fill(@$"exec ChekcSerailNo @SerialNo='{e["SerialNo"]}' ", util.strElect);
                   int  no = Convert.ToInt32(ds2.Tables[0].Rows[0][0]);
                    if(no >0)
                    {
                        Id = "Duplicate Serial No : " + e["SerialNo"];
                    }
                    
                    



                }

                
                if (file != null && file.Length > 0)
                {
                    string uploadsFolder = Path.Combine("wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

                    string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    fileName = "uploads/"+uniqueFileName;

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                  
                }

                if (Id == "")
                {
                var ds = util.Fill(@$"exec MatrialInMaster @Invoiceno='{Invoice}',@Company='{Companyid}',@assetitem='{AssetItem}',@cusassetname='{customAssetName}',@vendor='{Vendor}',@cusvendorname='{customVendorName}',@manufacture='{Manufacturer}',@cusmanufacture='{customManufacturerName}',@billdate='{BillDate}',@Receiveddate='{ReceivedDate}',@reciveqty='{reqnQuantity}',@InvoiceImg='{fileName}' ", util.strElect);
                   
                Id = ds.Tables[0].Rows[0][0].ToString();
                    if (Id != "Invoice No Exist")
                    {
                        Metrailno = Id;
                        Id = "";
                    }
                }
                if (Id == "")
                {
                    DataSet ds1 = new DataSet();
                    foreach(var e in objects)
                    {

                   ds1=util.Fill(@$"exec MatrialItems @Materialid='{Metrailno}',@Asstitemid='{e["AssetItemId"]}',@SerialNo='{e["SerialNo"]}',@Modelno='{e["ModelNo"]}',@Gen='{e["Generation"]}',@harddisk='{e["HardDisk"]}',@RamCap='{e["RAMCapacity"]}',@SSD='{e["SSDCapacity"]}',@Other='{e["Other"]}',@ItemName='{e["ItemName"]}',@Warrantydate='{e["warrantydate"]}',@Processs='{e["CPUCapacity"]}' ", util.strElect);
                    }

                    Id = "Matrial In SuccessFully";
                }

                // Save the data to the database, etc.

                return Json(new { success = true, message = Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }   [HttpPost]
        public IActionResult MaterialOutInsert()
        {
            try
            {
                
                string Companyid = Request.Form["Company"].ToString();
                string branch = Request.Form["branch"].ToString();
               
                string Empid = Request.Form["employeeId"].ToString();
                string Empname = Request.Form["EmployeeName"].ToString();
                string Empemail = Request.Form["EmailId"].ToString();
                string Empdpt = Request.Form["Department"].ToString();
                string remark = Request.Form["Remarks"].ToString();
                string Empphone = Request.Form["PhoneNo"].ToString();
                string issrancedate = Request.Form["IssuanceDate"];

                string JsonData = Request.Form["JsonData"].ToString();
                var objects = JArray.Parse(JsonData);
                string Id = "";
                string Metrailno = "";
                

                

                var ds = util.Fill(@$"exec Usp_MatrialOut @branch='{branch}',@Company='{Companyid}',@EmpId='{Empid}',@IssuanceDate='{issrancedate}',@remark='{remark}',@empName='{Empname}',@empDept='{Empdpt}',@empEmail='{Empemail}',@empPhone='{Empphone}' ", util.strElect);
                   
               int outid = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                    if (outid >0)
                    {
                        DataSet ds1 = new DataSet();
                        foreach (var e in objects)
                        {

                            ds1 = util.Fill(@$"exec Usp_Assignment @EmpId='{Empid}' ,@serailno='{e["serailno"]}', @WindowsKey='{e["windowskey"]}',@MSOfficeKey='{e["msofficekey"]}', @id='{outid}',@matreailId='{e["matreailId"]}' ", util.strElect);
                        }

                      

                        Id = "Matrial Out SuccessFully";
                    }
                
                //if (Id == "")
                //{
                //    DataSet ds1 = new DataSet();
                //    foreach(var e in objects)
                //    {

                //   ds1=util.Fill(@$"exec MatrialItems @Materialid='{Metrailno}',@Asstitemid='{e["AssetItemId"]}',@SerialNo='{e["SerialNo"]}',@Modelno='{e["ModelNo"]}',@Gen='{e["Generation"]}',@harddisk='{e["HardDisk"]}',@RamCap='{e["RAMCapacity"]}',@SSD='{e["SSDCapacity"]}',@Other='{e["Other"]}',@ItemName='{e["ItemName"]}',@Warrantydate='{e["warrantydate"]}',@Processs='{e["CPUCapacity"]}' ", util.strElect);
                //    }

                //    Id = "Matrial In SuccessFully";
                //}

                // Save the data to the database, etc.

                return Json(new { success = true, message = Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        public IActionResult ReMaterialOutInsert()
        {
            try
            {
                
                string Companyid = Request.Form["Company"].ToString();
                string branch = Request.Form["branch"].ToString();
               
                string Empid = Request.Form["employeeId"].ToString();
                string Empname = Request.Form["EmployeeName"].ToString();
                string Empemail = Request.Form["EmailId"].ToString();
                string Empdpt = Request.Form["Department"].ToString();
                string remark = Request.Form["Remarks"].ToString();
                string Empphone = Request.Form["PhoneNo"].ToString();
                string issrancedate = Request.Form["IssuanceDate"];

                string JsonData = Request.Form["JsonData"].ToString();
                var objects = JArray.Parse(JsonData);
                string Id = "";
                string Metrailno = "";
                

                
                if(Empid != "")
                {

               
               
            
                        DataSet ds1 = new DataSet();
                        foreach (var e in objects)
                        {

                            ds1 = util.Fill(@$"exec Usp_ReAssignment @EmpId='{Empid}', @serailno='{e["serailno"]}', @WindowsKey='{e["windowskey"]}',@MSOfficeKey='{e["msofficekey"]}', @materialOutId='{e["outId"]}',@matreailId='{e["matreailId"]}' ", util.strElect);
                        }

                      

                        Id = "Matrial Out SuccessFully";
                }
                else
                {
                    Id = "Employee Select";
                }

                //if (Id == "")
                //{
                //    DataSet ds1 = new DataSet();
                //    foreach(var e in objects)
                //    {

                //   ds1=util.Fill(@$"exec MatrialItems @Materialid='{Metrailno}',@Asstitemid='{e["AssetItemId"]}',@SerialNo='{e["SerialNo"]}',@Modelno='{e["ModelNo"]}',@Gen='{e["Generation"]}',@harddisk='{e["HardDisk"]}',@RamCap='{e["RAMCapacity"]}',@SSD='{e["SSDCapacity"]}',@Other='{e["Other"]}',@ItemName='{e["ItemName"]}',@Warrantydate='{e["warrantydate"]}',@Processs='{e["CPUCapacity"]}' ", util.strElect);
                //    }

                //    Id = "Matrial In SuccessFully";
                //}

                // Save the data to the database, etc.

                return Json(new { success = true, message = Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        
    

        private void SetupViewBag()
        {
            //ViewBag.Companies = new SelectList(Companies.Where(c=>c.Id==1).OrderBy(c => c.Name), "Id", "Name");
          //  ViewBag.AssetItems = new SelectList(AssetItems.OrderBy(a => a.Name), "Id", "Name");
          //  ViewBag.Vendors = new SelectList(Vendors.OrderBy(v => v.Name), "Id", "Name");
          //  ViewBag.Manufacturers = new SelectList(Manufacturers.OrderBy(m => m.Name), "Id", "Name");
        }

        private MaterialViewModel GetMaterialViewModel(Material material = null)
        {
            try
            {
                //var materials = Materials
                //    .Include(m => m.Company)
                //    .Include(m => m.Vendor)
                //    .Include(m => m.Manufacturer)
                //    .Include(m => m.AssetItem)
                //    .Include(m => m.MaterialItems)
                //    .ToList() ?? new List<Material>();

                return new MaterialViewModel
                {
                  //  Materials = materials,
                    NewMaterial = material ?? new Material
                    {
                      //  CompanyId = Companies.FirstOrDefault()?.Id ?? 0
                    }
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Error in GetMaterialViewModel: {ex.Message}");
                return new MaterialViewModel
                {
                    Materials = new List<Material>(),
                    NewMaterial = material ?? new Material()
                };
            }
        }
        public IActionResult AvailableStock(string searchTerm, string receivedDateFrom, string receivedDateTo)
        {
            try
            {
                string fromDate = receivedDateFrom;
                string toDate = receivedDateTo;

                //if (!string.IsNullOrEmpty(receivedDateFrom))
                //{
                //    fromDate = DateTime.ParseExact(receivedDateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                //}

                //if (!string.IsNullOrEmpty(receivedDateTo))
                //{
                //     toDate = DateTime.ParseExact(receivedDateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                //}


                var ds = util.Fill("exec Usp_AvailableStock @fromdate='" + fromDate + "',@todate='"+toDate+"'", util.strElect);
                var dt = ds.Tables[0];

                List<MaterialItem> materialItems = new List<MaterialItem>();

                foreach (DataRow dr in dt.Rows)
                {
                    MaterialItem item = new MaterialItem
                    {
                       
                        InvoiceNo = dr["InvoiceNo"].ToString(),
                        SerialNo = dr["SerialNo"].ToString(),
                        ModelNo = dr["ModelNo"].ToString(),
                        Vendor = dr["vendor"].ToString(),
                        BillDate = dr["BillDate"].ToString(),
                        RecordDate = dr["RecordDate"].ToString(),
                        Manufacturer = dr["manufuture"].ToString(),
                        ItemName = dr["ItemName"].ToString(),
                        CustomVendorName = dr["CustomVendorName"].ToString(),
                        CustomAssetName = dr["CustomAssetName"].ToString(),
                        CustomManufacturerName = dr["CustomManufacturerName"].ToString(),
                        AssetItem = dr["AssetItem"].ToString(),
                        Generation = dr["Generation"].ToString(),
                        CPUCapacity = dr["CPUCapacity"].ToString(),
                        HardDisk = dr["HardDisk"].ToString(),
                        RAMCapacity = dr["RAMCapacity"].ToString(),
                        SSDCapacity = dr["SSDCapacity"].ToString(),
                        Other = dr["Other"].ToString(),
                        ReceiverName = dr["ReceiverName"].ToString(),
                        WarrantyDate = dr["WarrantyDate"] != DBNull.Value ? Convert.ToDateTime(dr["WarrantyDate"]) : (DateTime?)null,
                        Status = dr["Status"] != DBNull.Value ? Convert.ToString(dr["Status"]) : string.Empty
                    };

                    materialItems.Add(item);
                }

               
             
           

                var viewModel = new AvailableStockViewModel
                {
                    Materials = materialItems
                };

               

                return View(viewModel);
            }
            catch (Exception ex)
            {
               // _logger.LogError($"Error in AvailableStock: {ex.Message}");
                return View("Error");
            }
        }


        public IActionResult AvailableItems(string searchTerm,
            DateTime? billDateFrom, DateTime? billDateTo,
            DateTime? warrantyDateFrom, DateTime? warrantyDateTo)
        {
            //var query = MaterialItems
            //    .Include(mi => mi.Material)
            //        .ThenInclude(m => m.AssetItem)
            //    .Where(mi => mi.Status == "UnAssigned");

            // Apply search filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                //query = query.Where(mi =>
                //    (mi.Material.AssetItem.Name == "Others" && mi.ItemName.ToLower().Contains(searchTerm)) ||
                //    (mi.Material.AssetItem.Name != "Others" && mi.Material.AssetItem.Name.ToLower().Contains(searchTerm)) ||
                //    mi.SerialNo.ToLower().Contains(searchTerm) ||
                //    mi.ModelNo.ToLower().Contains(searchTerm)
                //);
            }

            // Apply Bill Date filters
            if (billDateFrom.HasValue)
            {
                //query = query.Where(mi => mi.Material.BillDate >= billDateFrom.Value);
            }
            if (billDateTo.HasValue)
            {
                //query = query.Where(mi => mi.Material.BillDate <= billDateTo.Value);
            }

            // Apply Warranty Date filters
            if (warrantyDateFrom.HasValue)
            {
                //query = query.Where(mi => mi.WarrantyDate >= warrantyDateFrom.Value);
            }
            if (warrantyDateTo.HasValue)
            {
                //query = query.Where(mi => mi.WarrantyDate <= warrantyDateTo.Value);
            }

            var model = new AvailableItemViewModel
            {
                SearchTerm = searchTerm,
                BillDateFrom = billDateFrom,
                BillDateTo = billDateTo,
                WarrantyDateFrom = warrantyDateFrom,
                WarrantyDateTo = warrantyDateTo,
                //AvailableItems = query.ToList()
            };

            return View(model);
        }

        public IActionResult InvoiceDashboard(string BillDatefrom, string BillDateto, string receivedDateFrom,string receivedDateTo)
        {
            try
            {
                string receivedFrom = receivedDateFrom;
                string receivedto = receivedDateTo;
                string Billfrom = BillDatefrom;
                string Billto = BillDateto;

             


                var ds = util.Fill("exec InvoiceRecord @fromdate='"+Billfrom+"',@todate='"+Billto+"',@rfromdate='"+receivedFrom+"',@rtodate='"+receivedto+"'", util.strElect);
                var dt = ds.Tables[0];
                List<Material> materialItems = new List<Material>();
  foreach(DataRow dr in dt.Rows)
                {


                    Material Item = new Material
                    {
                        Id = dr["Id"].ToString(),
                        InvoiceNo = dr["InvoiceNo"].ToString(),
                        AssetItem = dr["AssetItem"].ToString(),
                        Vendor = dr["Vendor"].ToString(),
                        Company = dr["company"].ToString(),
                        Manufacturer = dr["Manufacturer"].ToString(),
                        BillDate = dr["BillDate"].ToString(),
                        RecordDate = dr["RecordDate"].ToString(),
                        ImagePath = dr["ImagePath"].ToString(),
                        TotalItem = dr["TotalItem"].ToString(),
                        
                    };
                materialItems.Add(Item);


                }


                var model = new MaterialViewModel
                {
                    Materials = materialItems
                };

                return View(model);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Error in InvoiceDashboard: {ex.Message}");
                return View("Error");
            }
        }

        public IActionResult AssignedAssets(string issuanceDateFrom, string issuanceDateTo)
        {
            try

            {
                string fromDate = "";
                string toDate = "";

                if (!string.IsNullOrEmpty(issuanceDateFrom))
                {
                    fromDate = DateTime.ParseExact(issuanceDateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                }

                if (!string.IsNullOrEmpty(issuanceDateTo))
                {
                    toDate = DateTime.ParseExact(issuanceDateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                }


                var ds = util.Fill("exec AssignedAssets @fromdate='"+fromDate+ "',@todate='"+toDate+"'", util.strElect);
                var dt = ds.Tables[0];



                List<AssignedAssetItem> materialItems = new List<AssignedAssetItem>();

                 foreach (DataRow dr in dt.Rows)
                 {
                    AssignedAssetItem Item = new AssignedAssetItem
                    {


                        //MaterialOutId = dr["InvoiceNo"] != DBNull.Value ? dr["InvoiceNo"].ToString() : string.Empty,
                        IssuanceDate = dr["IssuanceDate"] != DBNull.Value ? dr["IssuanceDate"].ToString() : string.Empty,
                        CompanyName = dr["company"] != DBNull.Value ? dr["company"].ToString() : string.Empty,
                        BranchName = dr["branch"] != DBNull.Value ? dr["branch"].ToString() : string.Empty,
                        EmployeeId = dr["EmployeeNumber"] != DBNull.Value ? dr["EmployeeNumber"].ToString() : string.Empty,
                        EmployeeName = dr["Employee"] != DBNull.Value ? dr["Employee"].ToString() : string.Empty,
                        Department = dr["Department"] != DBNull.Value ? dr["Department"].ToString() : string.Empty,
                        AssetName = dr["AssetItem"] != DBNull.Value ? dr["AssetItem"].ToString() : string.Empty,
                        SerialNo = dr["SerialNo"] != DBNull.Value ? dr["SerialNo"].ToString() : string.Empty,
                        Generation = dr["Generation"].ToString(),
                        HardDisk = dr["HardDisk"].ToString(),
                        RAMCapacity = dr["RAMCapacity"].ToString(),
                        SSDCapacity= dr["SSDCapacity"].ToString(),
                        Processor = dr["Processor"].ToString(),
                        ItemName = dr["ItemName"].ToString(),
                        Other = dr["Other"].ToString(),
                        ModelNo = dr["ModelNo"] != DBNull.Value ? dr["ModelNo"].ToString() : string.Empty,
                        WarrantyDate = dr["WarrantyDate"] != DBNull.Value ? dr["WarrantyDate"].ToString() : string.Empty,
                        AssignmentDate = dr["AssignmentDate"].ToString(),
                        Status = dr["Status"] != DBNull.Value ? dr["Status"].ToString() : string.Empty,

                    };

                     materialItems.Add(Item);
                 }
              

             
                var viewModel = new AssignedAssetsViewModel
                {
                    AssignedAssets = materialItems
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
               // _logger.LogError($"Error in AssignedAssets: {ex.Message}");
                return View("Error");
            }
        }

        public IActionResult InvoiceRecord()
        {
            return View("InvoiceDashboard");
        }
        public IActionResult MaterialOut()
        {
            try
            {

                ViewBag.Companies = util.PopulateDropDown("select * from Companies", util.strElect);
                ViewBag.Employee = util.PopulateDropDown("select employeeid as Id,(name +' ('+employeeid+')')as Name from Employees", util.strElect);
                ViewBag.AssetItems = util.PopulateDropDown("select * from AssetItems where name <> 'Others'", util.strElect);
                ViewBag.AssetItemsother = util.PopulateDropDown("select * from AssetItems where name ='Others'", util.strElect);


                return View();
            }
            catch (Exception ex)
            {
              //  _logger.LogError($"Error in MaterialOut action: {ex.Message}");
                return View("Error");
            }
        }

        [HttpGet]
        public JsonResult GetEmployeeDetails(string empId)
        {
            var ds = util.Fill("select Name, Department, Email, PhoneNo from Employees where EmployeeId = '"+ empId + "'", util.strElect);

            var dt = ds.Tables[0];
            return Json(JsonConvert.SerializeObject(dt));

        }  
        [HttpGet]
        public JsonResult GetMaterialItems(string materialId)
        {
            var ds = util.Fill("exec GetMaterialItems @martialid='"+ materialId + "'", util.strElect);

            var dt = ds.Tables[0];

            List<MaterialItem> materialItems = new List<MaterialItem>();


            MaterialItem Item = new MaterialItem
                {
                   // Id = dt.Rows[0]["Id"].ToString(),
                    SerialNo = dt.Rows[0]["SerialNo"].ToString(),
                    ModelNo = dt.Rows[0]["ModelNo"].ToString(),
                    Generation = dt.Rows[0]["Generation"].ToString(),
                    CPUCapacity = dt.Rows[0]["Processor"].ToString(),
                    RAMCapacity = dt.Rows[0]["RAMCapacity"].ToString(),
                    SSDCapacity = dt.Rows[0]["SSDCapacity"].ToString(),
                    HardDisk = dt.Rows[0]["HardDisk"].ToString(),
                    Other = dt.Rows[0]["Other"].ToString(),
                    WarrantyDate = Convert.ToDateTime(dt.Rows[0]["WarrantyDate"])

                };

               
            
           // return Json(Item);
            return Json(JsonConvert.SerializeObject(dt));

        }

        [HttpGet]
        public JsonResult GetBranches(string? companyId)
        {
            try
            {
                var ds = util.PopulateDropDown("select Id,Name from Branches where CompanyId='" + companyId + "'",util.strElect);



                return Json(JsonConvert.SerializeObject(ds));
            }
            catch (Exception ex)
            {
               // _logger.LogError($"Error getting branches: {ex.Message}");
                return Json(new object[] { });
            }
        }

        [HttpGet]
        public JsonResult GetSearilNO(string? assentid)
        {
            try
            {
                var ds = util.PopulateDropDown("select Id,serialNo from MaterialItems where assetitemid='" + assentid + "' and Status in ('UnAssigned','Returned')", util.strElect);



                return Json(JsonConvert.SerializeObject(ds));
            }
            catch (Exception ex)
            {
               // _logger.LogError($"Error getting branches: {ex.Message}");
                return Json(new object[] { });
            }
        }
        [HttpGet]
        public JsonResult GetoutDetails(string? serialid)
        {
            try
            {
                var ds = util.Fill("exec Usp_GetMaterialOutItems @martialid='" + serialid+"'", util.strElect);
                



                return Json(JsonConvert.SerializeObject(ds.Tables[0]));
            }
            catch (Exception ex)
            {
               // _logger.LogError($"Error getting branches: {ex.Message}");
                return Json(new object[] { });
            }
        } 
        [HttpPost]
        public JsonResult DeleteInvoiceOrItems(string? invoiceNo)
        {
            try
            {
                var ds = util.Fill("exec Usp_DeleteInvoiceOrItems @action='DeleteInvoice', @invoiceNo='" + invoiceNo + "'", util.strElect);

                return Json(JsonConvert.SerializeObject(ds.Tables[0]));
            }
            catch (Exception ex)
            {
               // _logger.LogError($"Error getting branches: {ex.Message}");
                return Json(new object[] { });
            }
        }  
        [HttpPost]
        public JsonResult DeleteAllItems(string? invoiceNo)
        {
            try
            {
                var ds = util.Fill("exec Usp_DeleteInvoiceOrItems @action='DeleteAllItem', @invoiceNo='" + invoiceNo + "'", util.strElect);
                

                return Json(JsonConvert.SerializeObject(ds.Tables[0]));
            }
            catch (Exception ex)
            {
               // _logger.LogError($"Error getting branches: {ex.Message}");
                return Json(new object[] { });
            }
        } 
        [HttpGet]
        public JsonResult bindassent()
        {
            try
            {
                var ds = util.PopulateDropDown("select * from AssetItems", util.strElect);
                



                return Json(JsonConvert.SerializeObject(ds));
            }
            catch (Exception ex)
            {
               // _logger.LogError($"Error getting branches: {ex.Message}");
                return Json(new object[] { });
            }
        }

        [HttpGet]
        public JsonResult GetSubAssets(int assetId)
        {
            return Json(0);
        }

        [HttpGet]
        public JsonResult GetEmployeesList(string search)
        {
            if (string.IsNullOrEmpty(search))
                return Json(new object[] { });

            

            return Json(0);
        }

        [HttpGet]
        public JsonResult GetAvailableSerialNumbers(int assetId)
        {
           /* var items = _db.MaterialItems
                .Include(mi => mi.AssetItem)
                .Where(mi => mi.AssetItemId == assetId && mi.Status == "UnAssigned")
                .Select(mi => new
                {
                    serialNo = mi.SerialNo,
                    modelNo = mi.ModelNo,
                    generation = mi.Generation,
                    cpuCapacity = mi.CPUCapacity,
                    ramCapacity = mi.RAMCapacity,
                    hardDisk = mi.HardDisk,
                    ssdCapacity = mi.SSDCapacity,
                    windowsKey = mi.WindowsKey,
                    msOfficeKey = mi.MSOfficeKey,
                    warrantyDate = mi.WarrantyDate,
                    other = mi.Other
                });

            if (!string.IsNullOrEmpty(search))
            {
                items = items.Where(i => i.serialNo.Contains(search));
            }*/

           // return Json(items.Take(10).ToList());
            return Json(0);
        }

        [HttpGet]
        public JsonResult GetMaterialItemDetails(string serialNo)
        {
          
            return Json(new { modelNo = 0 });
        }

        [HttpPost]
        public IActionResult CreateMaterialOut(MaterialOutViewModel model, string BranchName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                         
                    
                }

                // If we get here, something failed, redisplay form
                ViewBag.Companies = null;
                ViewBag.Assets = null;
                return View("MaterialOut", model);
            }
            catch (Exception ex)
            {
               // _logger.LogError($"Error in CreateMaterialOut: {ex.Message}");
                return View("Error");
            }
        }

        [HttpGet]
        public JsonResult GetBranchesDebug(int? companyId = null)
        {

            return Json(new
            {
                
                branches = ""
            });
        }

        [HttpPost]
        public JsonResult CreateBranch(string name, int companyId)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return Json(new { success = false, message = "Branch name is required" });

                var branch = new Branch
                {
                    Name = name,
                    CompanyId = companyId
                };

                

                return Json(new
                {
                    success = true,
                    branchId = branch.Id,
                    message = "Branch created successfully"
                });
            }
            catch (Exception ex)
            {
               // _logger.LogError($"Error creating branch: {ex.Message}");
                return Json(new { success = false, message = "Error creating branch" });
            }
        }

        public IActionResult UpdateMaterialItem()
        {

            //ViewBag.Companies = util.PopulateDropDown("select * from Companies", util.strElect);
            //ViewBag.Employee = util.PopulateDropDown("select employeeid as Id,(name +' ('+employeeid+')')as Name from Employees", util.strElect);
            ViewBag.AssetItems = util.PopulateDropDown("select * from AssetItems where name <> 'Others'", util.strElect);
            ViewBag.AssetItemsother = util.PopulateDropDown("select * from AssetItems where name ='Others'", util.strElect);
            ViewBag.employee = util.PopulateDropDown("select distinct a.EmployeeId,Name +'('+a.Employeeid+')' as Name from Employees a  join MaterialOuts b on b.EmployeeId=a.EmployeeId join MaterialAssignments mm on b.Id=mm.MaterialOutId", util.strElect);
            return View();
        }

        public JsonResult GetAssignDetails(string EmpId)
        {
            var ds = util.Fill("exec Usp_GetUpdateDetails @empid ='" + EmpId + "'", util.strElect);
            return Json(JsonConvert.SerializeObject(ds.Tables[0]));
        }


        [HttpPost]
        public JsonResult DeleteAssignDetails(string Id,string? empid,string? ReceiverName)
        {
            var msg = util.execQuery("exec Usp_DeleteAssignDetails @id ='" + Id + "',@empid='"+empid+ "',@ReceiverName='"+ ReceiverName + "'", util.strElect);
            
            return Json(JsonConvert.SerializeObject(msg));

        }
        [HttpPost]
        public JsonResult GetDetailsMaterialIn (string InvoiceNo)
        {
            var ds = util.Fill("exec Usp_GetDetailsMaterialIn @InvoiceNo ='" + InvoiceNo + "'", util.strElect);
            var data = new
            {
                dt = ds.Tables[0],
                dt2 = ds.Tables[1],
            };
            return Json(JsonConvert.SerializeObject(data));
        }
        [HttpPost]
        public JsonResult Deletematerial(string Id)
        {
            string mesg = "";
            var ds1 = util.Fill("select count(*) from MaterialItems where id ='" + Id + "' and status='Assigned'", util.strElect);
            var count = Convert.ToInt32(ds1.Tables[0].Rows[0][0].ToString());
            if (count == 0)
            {


                util.execQuery("insert into logs_table (materialItemId,enterydate,status,Assetname)values('" + Id + "',getdate(),'Delete',(select AssetItemId from MaterialItems where id='" + Id + "'))", util.strElect);
                 mesg = util.execQuery("delete from MaterialItems where id ='" + Id + "' and status='Unassigned'", util.strElect);
            }
            else
            {
                mesg = "Item is already assigned. Please unassign it before deleting.";


            }

            return Json(JsonConvert.SerializeObject(mesg));
        }
       

        public IActionResult AssetAssignmentDetails(string? asset,string? serialno)


        {
            ViewBag.AssetItems = util.PopulateDropDown("select * from AssetItems", util.strElect);
            ViewBag.SerialNo = util.PopulateDropDown("select Id,SerialNo from MaterialItems", util.strElect);

            var ds = util.Fill("exec Usp_AssetAssignmentDetails @Asset='"+asset+ "',@SerialNo='" + serialno+"'", util.strElect);

            ViewBag.data = ds.Tables[0];

            return View();
        }


        public IActionResult UpdateInvoice()
        {

            ViewBag.Companies = util.PopulateDropDown("select * from Companies where id=1", util.strElect);
            ViewBag.AssetItems = util.PopulateDropDown("select * from AssetItems", util.strElect);
            ViewBag.Vendors = util.PopulateDropDown("select * from Vendors", util.strElect);
            ViewBag.Manufacturers = util.PopulateDropDown("select * from Manufacturers", util.strElect);
            ViewBag.InvoiceNo = util.PopulateDropDown("select InvoiceNo,InvoiceNo from Materials ", util.strElect);
            return View();
        }




        [HttpPost]
        public IActionResult UpdateIn(IFormFile? file)
        {
            try
            {
               
                string Company = Request.Form["Company"].ToString();
                //string Company = Request.Form["Company"].ToString();
                string VendorName = Request.Form["VendorName"].ToString();
                string Manufacturer = Request.Form["Manufacturer"].ToString();
                string BillDate = Request.Form["BillDate"].ToString();
                string ReceivedDate = Request.Form["ReceivedDate"].ToString();
                string materialid = Request.Form["materialid"].ToString();
                          
                string JsonData = Request.Form["JsonData"].ToString();
                var objects = JArray.Parse(JsonData);
                string Id = "";
                string Metrailno = "";
                string fileName = "";
                //foreach (var e in objects)
                //{
                //    var ds2 = util.Fill(@$"exec ChekcSerailNo @SerialNo='{e["SerialNo"]}' ", util.strElect);
                //    int no = Convert.ToInt32(ds2.Tables[0].Rows[0][0]);
                //    if (no > 0)
                //    {
                //        Id = "Duplicate Serial No : " + e["SerialNo"];
                //    }





                //}


                if (file != null && file.Length > 0 && Id=="")
                {
                    string uploadsFolder = Path.Combine("wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

                    string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    fileName = "uploads/" + uniqueFileName;

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }


                }

                if (Id == "")
                {
                    var ds = util.Fill(@$"update Materials set CompanyId='{Company}',CustomVendorName='{VendorName}',ManufacturerId='{Manufacturer}',BillDate='{BillDate}',RecordDate='{ReceivedDate}', ImagePath=case when '{fileName}'='' then ImagePath else '{fileName}' end where id='{materialid}'", util.strElect);

                   
                }
                if (Id == "")
                {
                    DataSet ds1 = new DataSet();
                    foreach (var e in objects)
                    {

                        ds1 = util.Fill(@$"exec Usp_UpdateMatrialItems @Materialid='{e["matrialitemid"]}',@Asstitemid='{e["AssetItemId"]}',@SerialNo='{e["SerialNo"]}',@Modelno='{e["ModelNo"]}',@Gen='{e["Generation"]}',@harddisk='{e["HardDisk"]}',@RamCap='{e["RAMCapacity"]}',@SSD='{e["SSDCapacity"]}',@Other='{e["Other"]}',@ItemName='{e["ItemName"]}',@Warrantydate='{e["WarrantyDate"]}',@Processs='{e["Processor"]}' ", util.strElect);
                    }

                    Id = "Matrial In Updated SuccessFully";
                }

               

                return Json(new { success = true, message = Id });
            }

           


            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public IActionResult Returnasset()
        {
            return View();
        }



    }
}