
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

namespace ContentManagementSystem.Controllers
{
    public class MaterialController : Controller
    {
        
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<MaterialController> _logger;
        private object 

        public MaterialController( IWebHostEnvironment webHostEnvironment, ILogger<MaterialController> logger)
        {
           
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;

            // Add default branches if none exist
            if (!Branches.Any())
            {
                var defaultBranches = new List<Branch>
                {
                    new Branch { Name = "Main Branch", CompanyId = 1 },
                    new Branch { Name = "North Branch", CompanyId = 1 },
                    new Branch { Name = "South Branch", CompanyId = 1 }
                };
                Branches.AddRange(defaultBranches);
                SaveChanges();
            }

            // Add default employees if none exist
            if (!Employees.Any())
            {
                var defaultEmployees = new List<Employee>
                {
                    new Employee
                    {
                        EmployeeId = "EMP001",
                        Name = "John Doe",
                        Department = "IT",
                        Email = "john.doe@company.com",
                        PhoneNo = "1234567890"
                    },
                    new Employee
                    {
                        EmployeeId = "EMP002",
                        Name = "Jane Smith",
                        Department = "HR",
                        Email = "jane.smith@company.com",
                        PhoneNo = "2345678901"
                    },
                    new Employee
                    {
                        EmployeeId = "EMP003",
                        Name = "Mike Johnson",
                        Department = "Finance",
                        Email = "mike.johnson@company.com",
                        PhoneNo = "3456789012"
                    },
                    new Employee
                    {
                        EmployeeId = "EMP004",
                        Name = "Sarah Williams",
                        Department = "Operations",
                        Email = "sarah.williams@company.com",
                        PhoneNo = "4567890123"
                    }
                };
                Employees.AddRange(defaultEmployees);
                SaveChanges();
            }
        }

        public IActionResult Index()
        {
            try
            {
                if (!Companies.Any())
                {
                    var defaultCompany = new Company { Name = "ASP Securities" };
                    Companies.Add(defaultCompany);
                    SaveChanges();
                }

                if (!Manufacturers.Any())
                {
                    var defaultManufacturers = new List<Manufacturer>
                    {
                        new Manufacturer { Name = "Dell" },
                        new Manufacturer { Name = "Lenovo" },
                        new Manufacturer { Name = "HP" }
                    };
                    Manufacturers.AddRange(defaultManufacturers);
                    SaveChanges();
                }

                if (!Vendors.Any())
                {
                    var defaultVendors = new List<Vendor>
                    {
                        new Vendor { Name = "TCS" },
                        new Vendor { Name = "Infosys" },
                        new Vendor { Name = "HCL" }
                    };
                    Vendors.AddRange(defaultVendors);
                    SaveChanges();
                }

                if (!AssetItems.Any())
                {
                    var defaultAssets = new List<AssetItem>
                    {
                        new AssetItem { Name = "Laptop" },
                        new AssetItem { Name = "Desktop" },
                        new AssetItem { Name = "Others" }
                    };
                    AssetItems.AddRange(defaultAssets);
                    SaveChanges();
                }

                SetupViewBag();
                ViewBag.ShowSidebar = true;
                return View(GetMaterialViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Index action: {ex.Message}");
                return View("Error");
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
                _logger.LogError($"Error parsing date '{dateStr}': {ex.Message}");
            }
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MaterialViewModel model, IFormFile ImageFile)
        {
            try
            {
                if (model?.NewMaterial == null)
                {
                    ModelState.AddModelError("", "Invalid form submission");
                    SetupViewBag();
                    return View("Index", GetMaterialViewModel());
                }

                var material = model.NewMaterial;
                var form = Request.Form;

                // Parse Bill Date
                var billDateStr = form["NewMaterial.BillDate"].ToString();
                if (!string.IsNullOrEmpty(billDateStr))
                {
                    if (DateTime.TryParseExact(billDateStr, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime billDate))
                    {
                        material.BillDate = billDate;
                    }
                    else
                    {
                        ModelState.AddModelError("NewMaterial.BillDate", "Please enter date in DD/MM/YYYY format");
                        SetupViewBag();
                        return View("Index", GetMaterialViewModel(model?.NewMaterial));
                    }
                }

                var isOtherAsset = AssetItems.Find(material.AssetItemId)?.Name == "Others";
                var othersVendor = Vendors.FirstOrDefault(v => v.Name == "Others");
                var othersManufacturer = Manufacturers.FirstOrDefault(m => m.Name == "Others");

                // Remove validation for non-required fields when asset is Others
                if (isOtherAsset)
                {
                    ModelState.Remove("NewMaterial.VendorId");
                    ModelState.Remove("NewMaterial.ManufacturerId");
                    ModelState.Remove("NewMaterial.BillDate");

                    // Set values for non-required fields
                    material.VendorId = othersVendor?.Id;
                    material.ManufacturerId = othersManufacturer?.Id;
                    material.BillDate = DateTime.Now;  // Or any default date you prefer
                }
                else
                {
                    // Validate required fields for non-Other assets
                    if (!material.VendorId.HasValue)
                        ModelState.AddModelError("NewMaterial.VendorId", "Vendor is required");
                    if (!material.ManufacturerId.HasValue)
                        ModelState.AddModelError("NewMaterial.ManufacturerId", "Manufacturer is required");
                    if (material.BillDate == default)
                        ModelState.AddModelError("NewMaterial.BillDate", "Bill Date is required");
                }

                _logger.LogInformation($"Attempting to create material with Invoice No: {material.InvoiceNo}");

                // Log all incoming data
                _logger.LogInformation($"Form Data: CompanyId={material.CompanyId}, " +
                    $"AssetItemId={material.AssetItemId}, " +
                    $"VendorId={material.VendorId}, " +
                    $"ManufacturerId={material.ManufacturerId}, " +
                    $"ReqnQuantity={material.ReqnQuantity}");

                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage));
                    _logger.LogWarning($"Model validation failed: {errors}");

                    SetupViewBag();
                    return View("Index", GetMaterialViewModel(material));
                }

                // Load related entities
                material.AssetItem = AssetItems.Find(material.AssetItemId);

                // Handle image upload if a file was provided
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

                    string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(ImageFile.FileName)}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(fileStream);
                    }

                    material.ImagePath = $"/uploads/{uniqueFileName}";
                    _logger.LogInformation($"Invoice image saved: {material.ImagePath}");
                }

                // Set the record date and other calculated fields
                material.RecordDate = DateTime.Now;
                material.Month = material.BillDate.ToString("MMMM");
                material.Year = material.BillDate.Year.ToString();

                // Get the form collection to process material items
                var materialItems = new List<MaterialItem>();

                // Get the number of items from the REQN quantity
                int itemCount = material.ReqnQuantity;

                bool isComputerAsset = material.AssetItem?.Name == "Desktop" || material.AssetItem?.Name == "Laptop" || material.AssetItem?.Name == "Server";

                _logger.LogInformation($"Processing {itemCount} items for {(isComputerAsset ? "Computer" : "Other")} asset");


                string[] SerialNo = form[$"NewMaterial.MaterialItems[0].SerialNo"];
                string[] ModelNo = form[$"NewMaterial.MaterialItems[0].ModelNo"];
                string[] Generation = form[$"NewMaterial.MaterialItems[0].Generation"];
                string[] CPUCapacity = form[$"NewMaterial.MaterialItems[0].CPUCapacity"];
                string[] HardDisk = form[$"NewMaterial.MaterialItems[0].HardDisk"];
                string[] RAMCapacity = form[$"NewMaterial.MaterialItems[0].RAMCapacity"];
                string[] SSDCapacity = form[$"NewMaterial.MaterialItems[0].SSDCapacity"];
                string[] ItemName = form[$"NewMaterial.MaterialItems[0].ItemName"];
                string[] Other = form[$"NewMaterial.MaterialItems[0].Other"];
                string[] WarrantyDate = form[$"NewMaterial.MaterialItems[0].WarrantyDate"];
                string[] AssetItemId = form[$"NewMaterial.MaterialItems[0].AssetItemId"];
                for (int i = 0; i < itemCount; i++)
                {
                    try

                    {


                        var item = new MaterialItem
                        {

                            SerialNo = SerialNo[i],
                            ModelNo = ModelNo[i],
                            AssetItemId = Convert.ToInt32(AssetItemId[i]),
                            Status = "UnAssigned",


                            Other = string.IsNullOrEmpty(Other[i]) ? " " : (Other[i]).ToString(),
                            Generation = Generation[i],
                            CPUCapacity = CPUCapacity[i],
                            HardDisk = HardDisk[i],
                            RAMCapacity = RAMCapacity[i],
                            SSDCapacity = SSDCapacity[i],


                        };

                        _logger.LogInformation($"Processing item {i + 1}: SerialNo={item.SerialNo[i]}, ModelNo={item.ModelNo[i]}, AssetItemId={item.AssetItemId}");

                        if (!isComputerAsset)
                        {
                            item.ItemName = string.IsNullOrEmpty(ItemName[i]) ? " " : (ItemName[i]).ToString();

                        }
                        _logger.LogInformation($"Computer specs: Gen={item.Generation}, CPU={item.CPUCapacity}, HDD={item.HardDisk}, RAM={item.RAMCapacity}, SSD={item.SSDCapacity}");
                        //else
                        //{
                        //    item.ItemName = ItemName[i];
                        //    item.Other = Other[i];
                        //}
                        // Parse warranty date
                        var warrantyDateStr = WarrantyDate[i].ToString();
                        if (!string.IsNullOrEmpty(warrantyDateStr))
                        {
                            if (DateTime.TryParseExact(warrantyDateStr, "dd/MM/yyyy",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out DateTime warrantyDate))
                            {
                                item.WarrantyDate = warrantyDate;
                            }
                            else
                            {
                                ModelState.AddModelError("", $"Invalid warranty date format for item {i + 1}. Use DD/MM/YYYY");
                                SetupViewBag();
                                return View("Index", GetMaterialViewModel(model?.NewMaterial));
                            }
                        }

                        materialItems.Add(item);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error processing item {i}: {ex.Message}");
                        ModelState.AddModelError("", $"Error processing item {i + 1}: {ex.Message}");
                        SetupViewBag();
                        return View("Index", GetMaterialViewModel(model?.NewMaterial));
                    }
                }

                material.MaterialItems = materialItems;

                // Handle custom names for Others
                if (AssetItems.Find(material.AssetItemId)?.Name == "Others")
                {
                    material.CustomAssetName = model.NewMaterial.CustomAssetName;
                }

                // Handle Vendor
                if (material.VendorId == othersVendor?.Id)
                {
                    material.CustomVendorName = form["NewMaterial.CustomVendorName"];
                    _logger.LogInformation($"Setting custom vendor name: {material.CustomVendorName}");
                }

                // Handle Manufacturer
                if (material.ManufacturerId == othersManufacturer?.Id)
                {
                    material.CustomManufacturerName = form["NewMaterial.CustomManufacturerName"];
                    _logger.LogInformation($"Setting custom manufacturer name: {material.CustomManufacturerName}");
                }

                // Parse dates
                if (!string.IsNullOrEmpty(Request.Form["BillDate"]))
                {
                    material.BillDate = ParseDate(Request.Form["BillDate"]) ?? DateTime.Now;
                }

                var receivedDate = ParseDate(Request.Form["ReceivedDate"]);
                if (receivedDate.HasValue)
                {
                    material.RecordDate = receivedDate.Value;
                }

                // Add and save to database
                _logger.LogInformation("Attempting to save to database...");
                Materials.Add(material);
                SaveChanges();

                TempData["Success"] = "Material created successfully!";
                _logger.LogInformation($"Successfully created material with ID: {material.Id}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating material: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Error saving to database: {ex.Message}");
                SetupViewBag();
                return View("Index", GetMaterialViewModel(model?.NewMaterial));
            }
        }

        private void SetupViewBag()
        {
            ViewBag.Companies = new SelectList(Companies.Where(c=>c.Id==1).OrderBy(c => c.Name), "Id", "Name");
            ViewBag.AssetItems = new SelectList(AssetItems.OrderBy(a => a.Name), "Id", "Name");
            ViewBag.Vendors = new SelectList(Vendors.OrderBy(v => v.Name), "Id", "Name");
            ViewBag.Manufacturers = new SelectList(Manufacturers.OrderBy(m => m.Name), "Id", "Name");
        }

        private MaterialViewModel GetMaterialViewModel(Material material = null)
        {
            try
            {
                var materials = Materials
                    .Include(m => m.Company)
                    .Include(m => m.Vendor)
                    .Include(m => m.Manufacturer)
                    .Include(m => m.AssetItem)
                    .Include(m => m.MaterialItems)
                    .ToList() ?? new List<Material>();

                return new MaterialViewModel
                {
                    Materials = materials,
                    NewMaterial = material ?? new Material
                    {
                        CompanyId = Companies.FirstOrDefault()?.Id ?? 0
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetMaterialViewModel: {ex.Message}");
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
                DateTime? fromDate = null;
                DateTime? toDate = null;

                if (!string.IsNullOrEmpty(receivedDateFrom))
                {
                    DateTime.TryParseExact(receivedDateFrom, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedFromDate);
                    fromDate = parsedFromDate;
                }

                if (!string.IsNullOrEmpty(receivedDateTo))
                {
                    DateTime.TryParseExact(receivedDateTo, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedToDate);
                    toDate = parsedToDate;
                }

                // JOIN query: Materials + MaterialItems + AssetItems
                var query =
                    from m in Materials
                    join mi in MaterialItems
                        on m.Id equals mi.MaterialId
                    join ai in AssetItems
                        on mi.AssetItemId equals ai.Id into aiGroup
                    from ai in aiGroup.DefaultIfEmpty() // left join for AssetItem (optional)
                    where mi.Status == "UnAssigned"
                    select new
                    {
                        Material = m,
                        MaterialItem = mi,
                        AssetItem = ai
                    };

                // Apply search term
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();

                    query = query.Where(x =>
                        x.Material.InvoiceNo.ToLower().Contains(searchTerm) ||
                        (x.Material.AssetItem.Name == "Others" && !string.IsNullOrEmpty(x.Material.CustomAssetName) && x.Material.CustomAssetName.ToLower().Contains(searchTerm)) ||
                        (x.Material.Vendor.Name == "Others" && !string.IsNullOrEmpty(x.Material.CustomVendorName) && x.Material.CustomVendorName.ToLower().Contains(searchTerm)) ||
                        (x.Material.Manufacturer.Name == "Others" && !string.IsNullOrEmpty(x.Material.CustomManufacturerName) && x.Material.CustomManufacturerName.ToLower().Contains(searchTerm)) ||
                        x.Material.AssetItem.Name.ToLower().Contains(searchTerm) ||
                        x.Material.Vendor.Name.ToLower().Contains(searchTerm) ||
                        x.Material.Manufacturer.Name.ToLower().Contains(searchTerm)
                    );
                }

                // Apply date filters
                if (fromDate.HasValue)
                {
                    query = query.Where(x => x.Material.RecordDate.Date >= fromDate.Value.Date);
                }

                if (toDate.HasValue)
                {
                    query = query.Where(x => x.Material.RecordDate.Date <= toDate.Value.Date);
                }

                // Group by Material to build a list of MaterialItems per Material
                var groupedResults = query
                    .AsEnumerable() // Switch to LINQ-to-Objects
                    .GroupBy(x => x.Material)
                    .Select(g => new Material
                    {
                        Id = g.Key.Id,
                        InvoiceNo = g.Key.InvoiceNo,
                        Vendor = g.Key.Vendor,
                        AssetItem=g.Key.AssetItem,
                        Manufacturer = g.Key.Manufacturer,
                        CustomAssetName = g.Key.CustomAssetName,
                        CustomVendorName = g.Key.CustomVendorName,
                        CustomManufacturerName = g.Key.CustomManufacturerName,
                        BillDate = g.Key.BillDate,
                        RecordDate = g.Key.RecordDate,

                        // Join + group to get distinct AssetItem names from MaterialItems
                      

                        // MaterialItems list (project with string conversions)
                        MaterialItems = g.Select(x => new MaterialItem
                        {
                            Id = x.MaterialItem.Id,
                            SerialNo = x.MaterialItem.SerialNo,
                            ModelNo = x.MaterialItem.ModelNo,
                            ItemName = x.MaterialItem.ItemName,
                            AssetItem = x.AssetItem,
                            Generation = x.MaterialItem.Generation?.ToString() ?? string.Empty,
                            CPUCapacity = x.MaterialItem.CPUCapacity?.ToString() ?? string.Empty,
                            HardDisk = x.MaterialItem.HardDisk?.ToString() ?? string.Empty,
                            RAMCapacity = x.MaterialItem.RAMCapacity?.ToString() ?? string.Empty,
                            SSDCapacity = x.MaterialItem.SSDCapacity?.ToString() ?? string.Empty,
                            Other = x.MaterialItem.Other,
                            WarrantyDate = x.MaterialItem.WarrantyDate,
                            Status = x.MaterialItem.Status,
                            AssetItemId = x.MaterialItem.AssetItemId
                        }).ToList()
                    })
                    .ToList();

                var model = new AvailableStockViewModel
                {
                    SearchTerm = searchTerm,
                    ReceivedDateFrom = fromDate,
                    ReceivedDateTo = toDate,
                    Materials = groupedResults
                };

                ViewBag.ShowSidebar = true;

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AvailableStock: {ex.Message}");
                return View("Error");
            }
        }


        public IActionResult AvailableItems(string searchTerm,
            DateTime? billDateFrom, DateTime? billDateTo,
            DateTime? warrantyDateFrom, DateTime? warrantyDateTo)
        {
            var query = MaterialItems
                .Include(mi => mi.Material)
                    .ThenInclude(m => m.AssetItem)
                .Where(mi => mi.Status == "UnAssigned");

            // Apply search filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(mi =>
                    (mi.Material.AssetItem.Name == "Others" && mi.ItemName.ToLower().Contains(searchTerm)) ||
                    (mi.Material.AssetItem.Name != "Others" && mi.Material.AssetItem.Name.ToLower().Contains(searchTerm)) ||
                    mi.SerialNo.ToLower().Contains(searchTerm) ||
                    mi.ModelNo.ToLower().Contains(searchTerm)
                );
            }

            // Apply Bill Date filters
            if (billDateFrom.HasValue)
            {
                query = query.Where(mi => mi.Material.BillDate >= billDateFrom.Value);
            }
            if (billDateTo.HasValue)
            {
                query = query.Where(mi => mi.Material.BillDate <= billDateTo.Value);
            }

            // Apply Warranty Date filters
            if (warrantyDateFrom.HasValue)
            {
                query = query.Where(mi => mi.WarrantyDate >= warrantyDateFrom.Value);
            }
            if (warrantyDateTo.HasValue)
            {
                query = query.Where(mi => mi.WarrantyDate <= warrantyDateTo.Value);
            }

            var model = new AvailableItemViewModel
            {
                SearchTerm = searchTerm,
                BillDateFrom = billDateFrom,
                BillDateTo = billDateTo,
                WarrantyDateFrom = warrantyDateFrom,
                WarrantyDateTo = warrantyDateTo,
                AvailableItems = query.ToList()
            };

            return View(model);
        }

        public IActionResult InvoiceDashboard()
        {
            try
            {
                var materials = Materials
                    .Include(m => m.Company)
                    .Include(m => m.AssetItem)
                    .Include(m => m.Vendor)
                    .Include(m => m.Manufacturer)
                    .Include(m => m.MaterialItems)
                    .OrderByDescending(m => m.BillDate)
                    .ToList();

                var model = new MaterialViewModel
                {
                    Materials = materials
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in InvoiceDashboard: {ex.Message}");
                return View("Error");
            }
        }

        public IActionResult AssignedAssets()
        {
            try
            {
                var assignedAssets = MaterialAssignments
                    .Include(ma => ma.MaterialOut)
                        .ThenInclude(mo => mo.Company)
                    .Include(ma => ma.MaterialOut)
                        .ThenInclude(mo => mo.Branch)
                    .Include(ma => ma.MaterialItem)
                        .ThenInclude(mi => mi.AssetItem)
                    .Include(ma => ma.Employee)
                    .Select(ma => new AssignedAssetItem
                    {
                        MaterialOutId = ma.MaterialOutId,
                        IssuanceDate = ma.AssignmentDate,
                        CompanyName = ma.MaterialOut.Company.Name,
                        BranchName = ma.MaterialOut.Branch.Name,
                        EmployeeId = ma.EmployeeNumber,
                        EmployeeName = ma.Employee.Name,
                        Department = ma.Employee.Department,
                        AssetName = ma.MaterialItem.AssetItem.Name,
                        SerialNo = ma.MaterialItem.SerialNo,
                        ModelNo = ma.MaterialItem.ModelNo,
                        WarrantyDate = ma.MaterialItem.WarrantyDate,
                        Status = ma.MaterialItem.Status
                    })
                    .OrderByDescending(a => a.IssuanceDate)
                    .ToList();

                var viewModel = new AssignedAssetsViewModel
                {
                    AssignedAssets = assignedAssets
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AssignedAssets: {ex.Message}");
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
                // Ensure we have at least one company
                if (!Companies.Any())
                {
                    Companies.Add(new Company { Name = "ASP Securities" });
                    SaveChanges();
                }

                // Get all companies and assets
                var companies = Companies.ToList();
                var firstCompany = companies.FirstOrDefault();
                var assets = AssetItems.ToList();

                var model = new MaterialOutViewModel
                {
                    CompanyId = firstCompany?.Id ?? 1,  // Set default CompanyId
                    IssuanceDate = DateTime.Today  // Set default date to today
                };

                // Setup ViewBag
                ViewBag.Companies = new SelectList(companies, "Id", "Name", firstCompany?.Id);
                ViewBag.Assets = new SelectList(assets, "Id", "Name");

                // Get initial branches for the default company
                var branches = Branches
                    .Where(b => b.CompanyId == model.CompanyId)
                    .Select(b => new
                    {
                        id = b.Id.ToString(),
                        text = b.Name
                    })
                    .ToList();

                ViewBag.InitialBranches = branches;

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in MaterialOut action: {ex.Message}");
                return View("Error");
            }
        }

        [HttpGet]
        public JsonResult GetEmployeeDetails(string empId)
        {
            var employee = Employees.FirstOrDefault(e => e.EmployeeId == empId);
            if (employee == null)
                return Json(new { });

            return Json(new
            {
                name = employee.Name,
                department = employee.Department,
                email = employee.Email,
                phoneNo = employee.PhoneNo
            });
        }

        [HttpGet]
        public JsonResult GetBranches(string search = null, int? companyId = null)
        {
            try
            {
                var query = Branches
                    .Include(b => b.Company)
                    .AsQueryable();

                // Filter by company if provided
                if (companyId.HasValue)
                {
                    query = query.Where(b => b.CompanyId == companyId);
                }

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(b => b.Name.ToLower().Contains(search));
                }

                // First get the data
                var branchData = query
                    .Select(b => new
                    {
                        Branch = b,
                        CompanyName = b.Company.Name
                    })
                    .ToList();  // Execute query here

                // Then format the display text
                var branches = branchData
                    .Select(b => new
                    {
                        id = b.Branch.Id.ToString(),
                        text = $"{b.Branch.Name} ({b.CompanyName})"
                    })
                    .OrderBy(b => b.text)
                    .Take(20)
                    .ToList();

                return Json(branches);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting branches: {ex.Message}");
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
        public JsonResult GetAvailableSerialNumbers(int assetId, string search = "")
        {
           

           

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
                _logger.LogError($"Error in CreateMaterialOut: {ex.Message}");
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
                _logger.LogError($"Error creating branch: {ex.Message}");
                return Json(new { success = false, message = "Error creating branch" });
            }
        }
    }
}