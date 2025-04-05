

function geturl() {
    let url = window.location.hostname;
    if (url == 'localhost') {
        return '';
    }
    else
        return '/itinventory';
}
let myurl = geturl();
var srnomain = 1;
var srnomain2 = 1;
function updateTableRows() {

 
    const quantity = parseInt(document.getElementById('reqnQuantity').value);
    const assetType = $("#AssetItem option:selected").text();
    const assetTypeid = $("#AssetItem").val();
 
    // tbody.innerHTML = '';
    
    const isComputerAsset = assetType === 'Desktop' || assetType === 'Laptop' || assetType === 'Server';

    if (isComputerAsset) {
        const tbody = document.getElementById('itemsTableBody');
        for (let i = 0; i < 1; i++) {
            const row = document.createElement('tr');
            row.innerHTML = `
                    <td>${srnomain}</td>
                            <td>${assetType}</td>
                             <td class='d-none'>
                                      <input type="text"
                               name="${assetType} AssetItem Name"  value='${assetTypeid}'
                               class="form-control AssetItemId" 
                                />

                                  </td>
                    <td>
                        <input type="text" 
                               name="N${assetType} SerialNo" 
                               class="form-control SerialNo mandatory" 
                               required />
                    </td>
                 
                    <td>
                        <div class="input-group">
                            <input type="date"  
                                   name="${assetType} WarrantyDate" 
                                   class="form-control  warranty-date mandatory" 
                                    />
                            
                        </div>
                    </td>
                    <td>
                        <input type="text" 
                               name="${assetType} ModelNo" 
                               class="form-control ModelNo mandatory" 
                               required />
                    </td>
                    <td id="tdGeneration${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="text" 
                               name="${assetType} Generation" 
                               class="form-control Generation mandatory"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdCPU${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="text" 
                               name="${assetType} CPUCapacity" 
                               class="form-control CPUCapacity mandatory"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdHDD${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="text" 
                               name="${assetType} RAMCapacity" 
                               class="form-control RAMCapacity mandatory"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdRAM${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="text" 
                               name="${assetType} HardDisk" 
                               class="form-control HardDisk"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdSSD${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="text" 
                               name="${assetType} SSDCapacity" 
                               class="form-control SSDCapacity" 
                               value="" />
                    </td>
                    <td>
                        <input type="text" 
                               name="${assetType} Other" 
                               class="form-control Other" 
                               placeholder="Enter other details" />
                    </td>
                    <td>
    <button class='btn btn-danger' onclick="removeRow(this)"> 
        <i class="fas fa-trash-alt"></i> 
    </button>
</td>
                `;
            tbody.append(row);
           
        }
        setsrno();
    }
    if (assetType=="Others") {
        const tbody = document.getElementById('itemsTableBody2');
        for (let i = 0; i < 1; i++) {
            const row = document.createElement('tr');
            row.innerHTML = `
                    <td id='serno'></td>
                            <td>${assetType}</td>
                                    <td class='d-none'>
                                      <input type="text" 
                               name="${assetType} AssetItemId"  value='${assetTypeid}'
                               class="form-control AssetItemId" 
                                />

                                  </td>
                    <td>
                        <input type="text" 
                               name="SerialNo" 
                               class="form-control SerialNo mandatory" 
                               required />
                    </td>
                    <td id="tdItemName${i}">
                        <input type="text" 
                               name="${assetType} ItemName" 
                               class="form-control ItemName mandatory"
                               ${!isComputerAsset ? 'required' : ''} />
                    </td>
                    <td>
                        <div class="input-group">
                            <input type="date" 
                                   name="${assetType} WarrantyDate" 
                                   class="form-control  warranty-date mandatory" 
                                 />
                           
                        </div>
                    </td>
                    <td>
                        <input type="text" 
                               name="${assetType} ModelNo" 
                               class="form-control ModelNo mandatory" 
                               required />
                    </td>
                    <td id="tdGeneration${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="text" 
                               name="${assetType} Generation" 
                               class="form-control"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdCPU${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="number" 
                               name="${assetType} CPUCapacity" 
                               class="form-control"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdHDD${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="number" 
                               name="HardDisk" 
                               class="form-control"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdRAM${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="number" 
                               name="RAMCapacity" 
                               class="form-control"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdSSD${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="text" 
                               name="SSDCapacity" 
                               class="form-control" 
                               value="" />
                    </td>
                    <td>
                        <input type="text" 
                               name="Other" 
                               class="form-control" 
                               placeholder="Enter other details" />
                    </td>
                      <td>
    <button class='btn btn-danger' onclick="removeRow2(this)">
        <i class="fas fa-trash-alt"></i> 
    </button>
</td>
                `;
            tbody.append(row);
            setsrno2()
        }
    }

    var dataRowCount = $('#itemsTableBody2  tr').length;
    var dataRowCount2 = $('#itemsTableBody  tr').length;

    $("#reqnQuantity").val(dataRowCount+dataRowCount2)

   
    // Initialize date pickers for warranty dates
 

     
    //$(".date-picker").datepicker(); // Initialize Datepicker


    // Update validation when date changes
    $('.warranty-date').on('changeDate', function () {
        $(this).datepicker('hide');
       // validateDate($(this));
    });
}
function removeRow(button) {
    // This removes the row that contains the button
    var row = button.closest('tr');

    row.remove();
    const input = document.getElementById('reqnQuantity');
    input.value = parseInt(input.value) - 1;
    setsrno()
}
function removeRow2(button) {
    // This removes the row that contains the button
    var row = button.closest('tr');

    row.remove();
    const input = document.getElementById('reqnQuantity');
    input.value = parseInt(input.value) - 1;
    setsrno2()
}


function uploadInvoice() {
    document.getElementById('invoiceUpload').click();
}
function showDateError(input, message) {
    input.addClass('is-invalid').removeClass('is-valid');
    if (!input.next('.invalid-feedback').length) {
        input.after(`<div class="invalid-feedback">${message}</div>`);
    } else {
        input.next('.invalid-feedback').text(message);
    }
}

function incrementQuantity() {
    const input = document.getElementById('reqnQuantity');
    input.value = parseInt(input.value) + 1;
    updateTableRows();
}

function decrementQuantity() {
    const input = document.getElementById('reqnQuantity');
    if (parseInt(input.value) > 1) {
        input.value = parseInt(input.value) - 1;
        updateTableRows();
    }
}

function uploadInvoice() {
    document.getElementById('invoiceUpload').click();
}

// Initialize table on page load
document.addEventListener('DOMContentLoaded', function () {
    updateTableRows();
});

// Update table when REQN quantity changes
document.getElementById('reqnQuantity').addEventListener('change', updateTableRows);

// Add event listener for asset item change
$(document).ready(function () {
    $('#AssetItem, #Vendor, #Manufacturer').change(function () {
        const assetType = $("#AssetItem option:selected").text();
        const vendorType = $("#Vendor option:selected").text();
        const manufacturerType = $("#Manufacturer option:selected").text();

        // Show/hide custom name fields
        $('#customAssetNameDiv').toggle(assetType === 'Others');
        $('#customVendorNameDiv').toggle(vendorType === 'Others');
        $('#customManufacturerNameDiv').toggle(manufacturerType === 'Others');

        // Clear and regenerate material items
        //updateTableRows();
    });


    $('.warranty-date').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        todayHighlight: true,
        clearBtn: true,
        startDate: '01/01/2000',
        endDate: '31/12/2100'
    });
});

// Add success message handling

function validateForm() {
    let isValid = true;

    // Set the quantity value explicitly
    var quantity = parseInt($('#reqnQuantity').val());
    if (isNaN(quantity) || quantity < 1) {
        toastr.error('Quantity must be greater than 0');
        return false;
    }

    // Log form data
    console.log('Form Data:');
    console.log('Invoice No:', $('#NewMaterial_InvoiceNo').val());
    console.log('Company:', $('#NewMaterial_CompanyId').val());
    console.log('Asset Item:', $('#NewMaterial_AssetItemId').val());
    console.log('Vendor:', $('#NewMaterial_VendorId').val());
    console.log('Manufacturer:', $('#NewMaterial_ManufacturerId').val());
    console.log('Bill Date:', $('#NewMaterial_BillDate').val());
    console.log('Quantity:', quantity);

    // Validate required fields
    $('#materialForm input[required], #materialForm select[required]').each(function () {
        if (!$(this).val()) {
            isValid = false;
            $(this).addClass('is-invalid');
            toastr.error(`${$(this).attr('placeholder') || $(this).attr('name')} is required`);
        } else {
            $(this).removeClass('is-invalid');
        }
    });

    // Validate material items
    $('#itemsTableBody input[required]').each(function () {
        if (!$(this).val()) {
            isValid = false;
            $(this).addClass('is-invalid');
        } else {
            $(this).removeClass('is-invalid');
        }
    });

    // Validate warranty dates
    $('.warranty-date').each(function () {
        const value = $(this).val();
        if (value && !validateDate($(this))) {
            isValid = false;
        }
    });

    if (!isValid) {
        toastr.error('Please fill in all required fields');
        return false;
    }

    // Set hidden field for quantity before submit
    $('<input>').attr({
        type: 'hidden',
        name: 'NewMaterial.ReqnQuantity',
        value: quantity
    }).appendTo('#materialForm');

    return true;
}

$('#NewMaterial_VendorId').change(function () {
    const vendorType = $(this).find("option:selected").text();
    $('#customVendorNameDiv').toggle(vendorType === 'Others');
    if (vendorType === 'Others') {
        $('#customVendorName').prop('required', true);
    } else {
        $('#customVendorName').prop('required', false).val('');
    }
});

$('#NewMaterial_ManufacturerId').change(function () {
    const manufacturerType = $(this).find("option:selected").text();
    $('#customManufacturerNameDiv').toggle(manufacturerType === 'Others');
    if (manufacturerType === 'Others') {
        $('#customManufacturerName').prop('required', true);
    } else {
        $('#customManufacturerName').prop('required', false).val('');
    }
});

$(document).ready(function () {
   
});





// Handle Invoice Number validation
$('#NewMaterial_InvoiceNo').on('input', function () {
    const value = $(this).val();
    const regex = /^[a-zA-Z0-9-_/\s]+$/;

    if (!regex.test(value)) {
        $(this).addClass('is-invalid');
        if (!$(this).next('.invalid-feedback').length) {
            $(this).after('<div class="invalid-feedback">Invoice number can only contain letters, numbers, hyphens, underscores, forward slashes and spaces</div>');
        }
    } else {
        $(this).removeClass('is-invalid').addClass('is-valid');
        $(this).next('.invalid-feedback').remove();
    }
});

// Set max date for Received Date
$(document).ready(function () {
    const today = new Date().toISOString().split('T')[0];
    $('input[name="ReceivedDate"]').attr('max', today);

    // Set default value to today
    $('input[name="ReceivedDate"]').val(today);

    // Prevent future date selection
    $('input[name="ReceivedDate"]').on('input', function () {
        const selectedDate = new Date($(this).val());
        const currentDate = new Date();

        // Reset to today if future date is selected
        if (selectedDate > currentDate) {
            $(this).val(today);
            toastr.warning('Future dates are not allowed');
        }
    });
});

// Date handling functions
function initializeDateInputs() {
    // Initialize all date inputs
    $('.bill-date, .received-date, .warranty-date').each(function () {
        initializeDateInput($(this));
    });
}

function initializeDateInput(input) {
    input.off('input').on('input', function () {
        let value = $(this).val();

        // Remove any non-numeric characters except /
        value = value.replace(/[^\d/]/g, '');

        // Automatically add slashes
        if (value.length >= 2 && value.charAt(2) !== '/') {
            value = value.slice(0, 2) + '/' + value.slice(2);
        }
        if (value.length >= 5 && value.charAt(5) !== '/') {
            value = value.slice(0, 5) + '/' + value.slice(5);
        }

        // Limit the length
        if (value.length > 10) {
            value = value.slice(0, 10);
        }

        $(this).val(value);

        // Validate the date
        if (value.length === 10) {
            validateDate($(this));
        }
    });

    // Add blur validation
    input.off('blur').on('blur', function () {
        const value = $(this).val();
        if (value && value.length !== 10) {
            showDateError($(this), 'Please enter a complete date in DD/MM/YYYY format');
        }
    });
}

function validateDate(input) {
    const value = input.val();
    if (!value) return true;

    const [day, month, year] = value.split('/').map(num => parseInt(num, 10));

    // Check if all parts are numbers
    if (isNaN(day) || isNaN(month) || isNaN(year)) {
        showDateError(input, 'Please enter a valid date in DD/MM/YYYY format');
        return false;
    }

    // Check ranges
    if (day < 1 || day > 31 || month < 1 || month > 12 || year < 2000 || year > 2100) {
        showDateError(input, 'Please enter a valid date');
        return false;
    }

    // Create date object for additional validation
    const date = new Date(year, month - 1, day);
    if (date.getDate() !== day || date.getMonth() !== month - 1 || date.getFullYear() !== year) {
        showDateError(input, 'Please enter a valid date');
        return false;
    }

    input.removeClass('is-invalid').addClass('is-valid');
    input.next('.invalid-feedback').remove();
    return true;
}

function showDateError(input, message) {
    input.addClass('is-invalid').removeClass('is-valid');
    if (!input.next('.invalid-feedback').length) {
        input.after(`<div class="invalid-feedback">${message}</div>`);
    } else {
        input.next('.invalid-feedback').text(message);
    }
}

function Validation() {
    var msg = "";
    var charregex = /^[a-zA-Z\s]+$/;
    var intregex = /^[0-9]+$/;
    var charintregex = /^[a-zA-Z0-9\s]+$/;


    $(".mandatory").each(function () {
        if ($(this).val() == "" || $(this).val() == "0") {
            var name = $(this).attr('name')
            console.log($(this).val());
            msg += "" + name + "  Required !!\n";
        }
    });

    $(".checktaxes").each(function () {
        if ($(this).val() == "") {
            var name = $(this).attr('name')
            console.log($(this).val());
            msg += "" + name + "  Required !!\n";
        }
    });
    $(".chkint").each(function () {
        if (!intregex.test($(this).val())) {
            var name = $(this).attr('name')
            msg += "Enter Valid " + name + "!!\n";
        }
    });
    $(".chkcharint").each(function () {
        if (!charintregex.test($(this).val())) {
            var name = $(this).attr('name')
            msg += "Enter Valid " + name + "!!\n";
        }
    });
    $(".chkchar").each(function () {
        if (!charregex.test($(this).val())) {
            var name = $(this).attr('name')
            msg += "Enter Valid " + name + "!!\n";
        }
    });
    return msg;
}

$(document).ready(function () {
    initializeDateInputs();

    // Set today's date for received date
    const today = new Date();
    const formattedToday = `${String(today.getDate()).padStart(2, '0')}/${String(today.getMonth() + 1).padStart(2, '0')}/${today.getFullYear()}`;
    $('.received-date').val(formattedToday);
});

// Update form validation
function validateForm() {
    let isValid = true;

    // Validate all date inputs
    $('.bill-date, .received-date, .warranty-date').each(function () {
        const value = $(this).val();
        if (value && !validateDate($(this))) {
            isValid = false;
        }
    });

    // ... rest of your existing validation code ...

    return isValid;
}

$(document).ready(function () {
    // Initialize all date pickers
    $('.date-picker').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        todayHighlight: true,
        clearBtn: true
    });

    // Set today's date for received date
    const today = new Date();
    const formattedToday = `${String(today.getDate()).padStart(2, '0')}/${String(today.getMonth() + 1).padStart(2, '0')}/${today.getFullYear()}`;
    $('input[name="ReceivedDate"]').val(formattedToday);

    // Prevent future dates for received date
    $('input[name="ReceivedDate"]').datepicker('setEndDate', 'today');

     //Click handler for calendar icons
    $('.input-group-text').click(function () {
        $(this).closest('.input-group').find('.date-picker').datepicker('show');
    });

    // Validate dates when changed
    $('.date-picker').on('changeDate', function () {
        $(this).datepicker('hide');
        validateDate($(this));
    });
});


function Insert() {
    let val = Validation()
    if (val == "") {

   
    var fdata = new FormData();

    fdata.append("Invoice", $("#Invoice").val());
    fdata.append("Companyid", $("#Companyid").val());
    fdata.append("AssetItem", $("#AssetItem").val());
    //fdata.append("customAssetName", $("#customAssetName").val());
    fdata.append("Vendor", $("#Vendor").val());
    fdata.append("customVendorName", $("#customVendorName").val());
    fdata.append("Manufacturer", $("#Manufacturer").val());
    fdata.append("customManufacturerName", $("#customManufacturerName").val());
    fdata.append("BillDate", $("#BillDate").val());
    fdata.append("ReceivedDate", $("#ReceivedDate").val());
    fdata.append("reqnQuantity", $("#reqnQuantity").val());
    var itemsArray = [];

    $("#itemsTableBody TR").each(function (index, row) {
        var Items = {};

        Items.AssetItemId = $(row).find('.AssetItemId').val();
        Items.SerialNo = $(row).find('.SerialNo').val();
        Items.warrantydate = $(row).find('.warranty-date').val();
        Items.ModelNo = $(row).find('.ModelNo').val();
        Items.Generation = $(row).find('.Generation').val();
        Items.CPUCapacity = $(row).find('.CPUCapacity').val();
        Items.RAMCapacity = $(row).find('.RAMCapacity').val();
        Items.HardDisk = $(row).find('.HardDisk').val();
        Items.SSDCapacity = $(row).find('.SSDCapacity').val();
        Items.Other = $(row).find('.Other').val();
        Items.ItemName = "";
        itemsArray.push(Items);  // Add to array
    });

    $("#itemsTableBody2 TR").each(function (index, row) {
        var Items = {};

        Items.AssetItemId = $(row).find('.AssetItemId').val();
        Items.SerialNo = $(row).find('.SerialNo').val();
        Items.ItemName = $(row).find('.ItemName').val();
        Items.warrantydate = $(row).find('.warranty-date').val();
        Items.ModelNo = $(row).find('.ModelNo').val();
        Items.Generation = $(row).find('.Generation').val();
        Items.CPUCapacity = $(row).find('.CPUCapacity').val();
        Items.RAMCapacity = $(row).find('.RAMCapacity').val();
        Items.HardDisk = $(row).find('.HardDisk').val();
        Items.SSDCapacity = $(row).find('.SSDCapacity').val();
        Items.Other = $(row).find('.Other').val();
        itemsArray.push(Items);  // Add to array
    });

    console.log(itemsArray);  // Final array of objects

    fdata.append("JsonData", JSON.stringify(itemsArray));


   
        //var files = $('#invoiceUpload')[0].files;

        var guestFileUpload = $("#invoiceUpload").get(0);
                    if (guestFileUpload.files.length > 0) {
        fdata.append("file", guestFileUpload.files[0]);
        fdata.append("FileName", guestFileUpload);
                    }

 

    $.ajax({
       
        url: myurl+'/Material/Addnew',
        type: 'post',
        data: fdata,                 
        contentType: false,          
        processData: false,
        success: function (data) {
            if (data.message == 'Matrial In SuccessFully') {
                window.location.reload();
            }
            alert(data.message);
            
        },
        error: function (err) {
            console.log("Error:", err);
            alert("Something went wrong!");
        }
    });
    } else {
        alert(val);
    }
}


function setsrno() {
    var i = 0;
    $("#itemsTableBody TR").each(function () {
        i++;
        var row = $(this);
        row.find('td:eq(0)').html(i);
      //  $("#serno").html(i);
    });
}

function setsrno2() {
    var i = 0;
    $("#itemsTableBody2 TR").each(function () {
        i++;
        var row = $(this);
        row.find('td:eq(0)').html(i);
       // $("#serno").html(i);
    });
}