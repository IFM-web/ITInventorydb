
function geturl() {
    let url = window.location.hostname;
    if (url == 'localhost') {
        return '';
    }
    else
        return '/itinventory';
}
var myurl1 = geturl();
function GetDetailsMaterialIn() {
    var InvoiceNo = $("#InvoiceNo").val()
    
    $(".serailno2").val(0).trigger("change");
    $(".serailno").val(0).trigger("change");
    $("#itemsTableBody2").empty();
    $("#itemsTableBody").empty();


    $.ajax({
        url: myurl1 + '/Material/GetDetailsMaterialIn',
        type: 'post',
        data: { InvoiceNo: InvoiceNo },
        success: (data) => {
            var data = JSON.parse(data);
            data1 = data.dt
            data2 = data.dt2           
            
            for (var e of data1) {
                $("#materialid").val(e.Id);
                    $("#Companyid").val(e.CompanyId);
                    $("#Manufacturer").val(e.ManufacturerId);
                    $("#customVendorName").val(e.CustomVendorName);
                    /*$("#Companyid").va(e.CustomAssetName);*/
                    $("#BillDate").val(formatDate(e.BillDate));
                    $("#ReceivedDate").val(formatDate(e.RecordDate));
                }

                console.log(data);
                var row1 = '';
                var row2 = '';
                for (var e of data2) {
          
                    if (e.AssetItemId == '4') {
                        row1 += `
                    <tr>
                    <td class='d-none'><span  id='matrialitemid'>${e.Id}</span>></td>
                    <td><select id='AssetItemId'><option value='${e.AssetItemId}'>Others</option></select></td>
                    <td><input type='text' id='SerialNo' disabled name='SerialNo' class='mandatory' value='${e.SerialNo}'</td>
                    <td><input type='text' id='ModelNo' name='ModelNo' class='mandatory' value='${e.ModelNo || ''}'</td>
                    <td><input type='text' id='ItemName' name='ItemName' class='mandatory' value='${e.ItemName || ''}'</td>
                    <td><input type='date' id='WarrantyDate' name='WarrantyDate' class='mandatory' value="${e.WarrantyDate ? e.WarrantyDate.split('T')[0] : ''}"></td>
                    <td><input type='text' id='Other' value='${e.Other || ''}'</td>
                    <td><button class='btn btn-danger' onclick="DeleteItem('${e.Id}',this)">Delete</button></td>
                  
                  </tr>
                    
                    `
                    } else {
                        row2 += `
                    <tr>
                    <td>${createDropdown(e.AssetItemId)}</td>
                     <td class='d-none'><span id='matrialitemid'>${e.Id}</span>></td>
                    <td><input type='text' id='SerialNo' disabled name='SerialNo' class='mandatory' value='${e.SerialNo}'</td>
                    <td><input type='text' id='ModelNo' name='ModelNo' class='mandatory' value='${e.ModelNo || ''}'</td>
                    <td><input type='text' id='Generation' name='Generation' class='mandatory' value='${e.Generation || ''}'</td>
                    <td><input type='text' id='Processor' name='Processor' class='mandatory' value='${e.Processor || ''}'</td>
                    <td><input type='text' id='RAMCapacity' name='RAMCapacity' class='mandatory' value='${e.RAMCapacity || ''}'</td>
                    <td><input type='text' id='HardDisk' name='HardDisk' class='' value='${e.HardDisk || ''}'</td>
                    <td><input type='text' id='SSDCapacity' name='SSDCapacity' class='' value='${e.SSDCapacity || ''}'</td>

                <td>
    <input type='date' id='WarrantyDate' name='WarrantyDate' class='mandatory'
           value="${e.WarrantyDate ? e.WarrantyDate.split('T')[0] : ''}">
</td>
                    <td><input type='text' id='Other' value='${e.Other || ''} '</td>
                    <td><button class='btn btn-danger' onclick="DeleteItem('${e.Id}'),this">Delete</button></td>
                
                  
                  </tr>`
                    }
                }
                $("#itemsTableBody2").append(row1);
                $("#itemsTableBody").append(row2);
            
            

        },
        error: (err) => {
            alert(err)
        }

    })
}


function createDropdown(selectedId) {

    let assetItems = [
        { id: 1, name: "Laptop" },
        { id: 2, name: "Desktop" },
        { id: 3, name: "Server" },
        { id: 4, name: "Others" }
    ];
    
    let dropdown = "<select id='AssetItemId' class='asset-dropdown'>";

    assetItems.forEach(item => {
        let selected = item.id == selectedId ? "selected" : "";
        dropdown += `<option value="${item.id}" ${selected}>${item.name}</option>`;
    });

    dropdown += "</select>";
    return dropdown;
}

function DeleteItem(Id,e) {
    $.ajax({

        url: myurl1 + '/Material/Deletematerial',
        type: 'post',
        data: { Id: Id },
        success: (data) => {
            data = JSON.parse(data);
          
            if (data == "Successfull") {
                removeRow(e)
                alert("Item Has been Deleted Successfull");

            } else {
                alert(data);
            }
           
          


        },
        error: (err) => {
            alert(err)
        }

    })
}


//////////-------------------------add same as material out ---------------------------------

$(document).ready(() => {
    branch();
    var selectedValue = $("#AssetItemsgrid2").val();
    bindserailno(selectedValue, document.getElementById("AssetItemsgrid2"));

})

function geturl() {
    let url = window.location.hostname;
    if (url == 'localhost') {
        return '';
    }
    else
        return '/itinventory';
}
let myurl = geturl();

function branch() {

    $.ajax({
        url: myurl + '/Material/GetBranches',
        type: "get",
        data: { companyId: $('#Company').val() },
        success: function (data) {
            var data = JSON.parse(data);

            var dropdown = $('#branchSelect');
            dropdown.empty();
            dropdown.append($('<option></option>').attr('value', 0).text('Select'));
            for (var i = 0; i < data.length; i++) {

                dropdown.append($('<option></option>').attr('value', data[i].Value).text(data[i].Text));
            }
        },
        error: (err) => {
            alert(err)
        }
    })


};

function bindserailno(e, el) {

    $.ajax({
        url: myurl + '/Material/GetSearilNO',
        type: "get",
        data: { assentid: e },
        success: function (data) {
            var data = JSON.parse(data);
            var row = $(el).closest("tr");

            var dropdown = row.find("#serailno");
            dropdown.empty();
            dropdown.append($('<option></option>').attr('value', 0).text('Select'));
            for (var i = 0; i < data.length; i++) {

                dropdown.append($('<option></option>').attr('value', data[i].Value).text(data[i].Text));
            }
        },
        error: (err) => {
            alert(err)
        }
    })


};

function binddetails(e, el) {

    $.ajax({
        url: myurl + '/Material/GetoutDetails',
        type: "get",
        data: { serialid: e },
        success: function (data) {
            var row = $(el).closest("tr");
            row.find(".model-no-cell").text('');
            row.find(".matreailId").text('');
            row.find(".othername").text('');
            row.find(".generation-cell").text('');
            row.find(".processor-cell").text('')
            row.find(".ram-cell").text('')
            row.find(".hdd-cell").text('')
            row.find(".ssd-cell").text('')
            row.find(".othername").text('')
            row.find(".warranty-date-cell").text('')
            row.find(".other-details-cell").text('')


            var data = JSON.parse(data);

            for (let i = 0; i < data.length; i++) {
                var row = $(el).closest("tr");


                row.find(".model-no-cell").text(data[i].ModelNo);
                row.find(".matreailId").text(data[i].Id);
                row.find(".othername").text(+ data[i].Other || ' ');
                row.find(".generation-cell").text(data[i].Generation || '');
                row.find(".processor-cell").text(data[i].Processor || '')
                row.find(".ram-cell").text(data[i].RAMCapacity || '')
                row.find(".hdd-cell").text(data[i].HardDisk || '')
                row.find(".ssd-cell").text(data[i].SSDCapacity || '')
                row.find(".othername").text(data[i].ItemName || '')
                row.find(".warranty-date-cell").text(data[i].WarrantyDate || '')
                row.find(".other-details-cell").text(data[i].Other || '')


            }



        },
        error: (err) => {
            alert(err)
        }
    })


};


function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('-');
}

function EmployeeDetails(id) {


    $.ajax({
        url: myurl + '/Material/GetEmployeeDetails',
        type: "get",
        data: { empId: id },
        success: function (data) {
            var data = JSON.parse(data);
            $("#EmployeeName").val(data[0].Name);
            $("#Department").val(data[0].Department);
            $("#EmailId").val(data[0].Email);
            $("#PhoneNo").val(data[0].PhoneNo);


        },
        error: (err) => {
            alert(err)
        }
    })

}

initializeRowEvents($('#itemsTableBody11 tr:first'));

function initializeRowEvents(row) {
    const assetSelect = row.find('#AssetItemsgrid');
    const serialNoSelect = row.find('.serialno-select');

    // Initialize Select2 for serial number dropdown
    serialNoSelect.select2({
        tags: true,
        width: '100%',
        ajax: {
            url: myurl + '/Material/GetAvailableSerialNumbers',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    assetId: assetSelect.val(),
                    search: params.term || ''
                };
            },
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.serialNo,
                        text: item.serialNo,
                        modelNo: item.modelNo,
                        generation: item.generation,
                        processor: item.processor,
                        ramCapacity: item.ramCapacity,
                        hardDisk: item.hardDisk,
                        ssdCapacity: item.ssdCapacity,
                        windowsKey: item.windowsKey,
                        msOfficeKey: item.msOfficeKey,
                        warrantyDate: item.warrantyDate,
                        other: item.other
                    }))
                };
                data = e.params.data;
                const selectedAsset = assetSelect.find('option:selected').text();
                const isComputerAsset = ['Desktop', 'Laptop', 'Server'].includes(selectedAsset);

                // Format warranty date to show only date
                const formattedWarrantyDate = data.warrantyDate ? new Date(data.warrantyDate).toLocaleDateString('en-GB') : '';

                // Always display these fields
                row.find('.model-no-cell').text(data.modelNo || '');
                row.find('.warranty-date-cell').text(formattedWarrantyDate);
                row.find('.other-details-cell').text(data.other || '');

                if (isComputerAsset) {
                    // Auto-fill computer-specific fields
                    row.find('.generation-cell').text(data.generation || '');
                    row.find('.processor-cell').text(data.processor || '');
                    row.find('.ram-cell').text(data.ramCapacity || '');
                    row.find('.hdd-cell').text(data.hardDisk || '');
                    row.find('.ssd-cell').text(data.ssdCapacity || '');
                    row.find('.windows-key').val(data.windowsKey || '');
                    row.find('.msoffice-key').val(data.msOfficeKey || '');
                }
            }
        },
        placeholder: 'Search or enter serial number',
        allowClear: true
    });

    // Asset selection change

    assetSelect.change(function () {

        const selectedAsset = $(this).find('option:selected').text();
        const isComputerAsset = ['Desktop', 'Laptop', 'Server'].includes(selectedAsset);

        // Toggle computer-specific columns
        row.closest('table').find('th.computer-column').toggle(isComputerAsset);
        row.find('td.computer-column').toggle(isComputerAsset);

        // Add/remove class for CSS control
        row.toggleClass('computer-asset', isComputerAsset);

        // Clear and reload serial numbers
        serialNoSelect.val(null).trigger('change');

        // Clear all cells
        row.find('td[class$="-cell"]').text('');
    });

    // Serial number selection change
    serialNoSelect.on('select2:select', function (e) {
        const data = e.params.data;
        const selectedAsset = assetSelect.find('option:selected').text();
        const isComputerAsset = ['Desktop', 'Laptop', 'Server'].includes(selectedAsset);

        // Format warranty date to show only date
        const formattedWarrantyDate = data.warrantyDate ? new Date(data.warrantyDate).toLocaleDateString('en-GB') : '';

        // Always display these fields
        row.find('.model-no-cell').text(data.modelNo || '');
        row.find('.warranty-date-cell').text(formattedWarrantyDate);
        row.find('.other-details-cell').text(data.other || '');

        if (isComputerAsset) {
            // Auto-fill computer-specific fields
            row.find('.generation-cell').text(data.generation || '');
            row.find('.processor-cell').text(data.processor || '');
            row.find('.ram-cell').text(data.ramCapacity || '');
            row.find('.hdd-cell').text(data.hardDisk || '');
            row.find('.ssd-cell').text(data.ssdCapacity || '');
            row.find('.windows-key').val(data.windowsKey || '');
            row.find('.msoffice-key').val(data.msOfficeKey || '');
        }
    });

   

    row.find('.remove-row2').click(function () {
        if ($('#itemsTableBody2 tr').length > 1) {
            row.remove();
        }
    });
    row.find('.remove-row').click(function () {
        if ($('#itemsTableBody tr').length > 1) {
            row.remove();
        }
    });

    row.find('.remove-row44').click(function () {
        if ($('#itemsTableBody tr').length > 1) {
            row.remove();
        }
    });
    row.find('.remove-row2').click(function () {
        if ($('#itemsTableBody22 tr').length > 1) {
            row.remove();
        }
    });
}


function addNewRow() {
    const newRow = $('#itemsTableBody11 tr:first').clone(); // Clone the first row

    // Remove old Select2 container
    newRow.find('.select2-container').remove();

    // Find the select element
    newRow.find('select').each(function () {
        $(this).val(0); // Reset selected value
        //$(this).removeAttr('data-select2-id'); // Remove Select2 ID
        //$(this).removeClass('select2-hidden-accessible'); // Remove Select2 hidden class
        ///$(this).next('.select2-container').remove(); // Remove any existing Select2 instance
    });

    // Clear text cells
    newRow.find('td[class$="-cell"]').text('');

    // Append new row to the table
    $('#itemsTableBody11').append(newRow);

    // Reinitialize Select2 AFTER adding the row
    newRow.find('select').select2();

    // Initialize other events on the new row if needed
    initializeRowEvents(newRow);
}
function addNewRow2() {
    const newRow = $('#itemsTableBody22 tr:first').clone(); // Clone the first row

    // Remove old Select2 container
    newRow.find('.select2-container').remove();

    // Find the select element
    newRow.find('select').each(function () {
        $(this).val($(this).find("option:first").val()).trigger('change'); // Reset to first option
    });

    // Clear text cells
    newRow.find('td[class$="-cell"]').text('');

    // Append new row to the table
    $('#itemsTableBody22').append(newRow);

    // Reinitialize Select2 AFTER adding the row
    newRow.find('select').select2();

    // Initialize other events on the new row if needed
    initializeRowEvents(newRow);
}


function removeRow(button) {
    // This removes the row that contains the button
    var row = button.closest('tr');

    row.remove();
    
    //setsrno()
}


function validateMaterialOutForm() {
    let isValid = true;
    const errors = [];

    // Validate Company
    if (!$('#CompanyId').val()) {
        errors.push('Please select a Company');
        $('#CompanyId').addClass('is-invalid');
        isValid = false;
    }

    // Validate Branch
    if (!$('#branchSelect').val()) {
        errors.push('Please select or enter a Branch');
        $('#branchSelect').addClass('is-invalid');
        isValid = false;
    }

    // Validate Employee
    if (!$('#employeeSelect').val()) {
        errors.push('Please select or enter an Employee ID');
        $('#employeeSelect').addClass('is-invalid');
        isValid = false;
    }

    // Validate Employee Details
    const requiredFields = [
        { id: 'EmployeeName', label: 'Employee Name' },
        { id: 'Department', label: 'Department' },
        { id: 'EmailId', label: 'Email ID' },
        { id: 'PhoneNo', label: 'Phone Number' }
    ];

    requiredFields.forEach(field => {
        const value = $(`#${field.id}`).val();
        if (!value || value.trim() === '') {
            errors.push(`${field.label} is required`);
            $(`#${field.id}`).addClass('is-invalid');
            isValid = false;
        }
    });

    // Validate Phone Number (10 digits)
    const phoneValue = $('#PhoneNo').val();
    if (phoneValue && phoneValue.length !== 10) {
        errors.push('Phone number must be 10 digits');
        $('#PhoneNo').addClass('is-invalid');
        isValid = false;
    } else {
        $('#PhoneNo').removeClass('is-invalid').addClass('is-valid');
    }

    // Validate Issuance Date
    if (!$('#IssuanceDate').val()) {
        errors.push('Please select an Issuance Date');
        $('#IssuanceDate').addClass('is-invalid');
        isValid = false;
    }

    // Validate Items Table
    let hasItems = false;
    $('#itemsTableBody11 tr').each(function () {
        const assetSelect = $(this).find('.asset-select');
        const serialNoSelect = $(this).find('.serialno-select');

        if (assetSelect.val() || serialNoSelect.val()) {
            hasItems = true;

            if (!assetSelect.val()) {
                errors.push('Please select an Asset for all rows');
                assetSelect.addClass('is-invalid');
                isValid = false;
            }

            if (!serialNoSelect.val()) {
                errors.push('Please select a Serial Number for all rows');
                serialNoSelect.addClass('is-invalid');
                isValid = false;
            }
        }
    });

    if (!hasItems) {
        errors.push('Please add at least one item');
        isValid = false;
    }

    // Show validation messages
    if (!isValid) {
        errors.forEach(error => toastr.error(error));
        return false;
    }

    // If form is valid, show confirmation and submit
    if (isValid) {
        Swal.fire({
            title: 'Processing...',
            text: 'Assigning assets to employee',
            icon: 'info',
            showConfirmButton: false,
            timer: 2000,
            willClose: () => {
                document.getElementById('materialOutForm').submit();
            }
        });
        return false; // Prevent default form submission
    }

    return false;
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
    //$(".serailno").each(function () {
    //    if ($(this).val() == 0) {

    //        msg += "Serial No Required !!\n";
    //    }
    //});
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

    //if ($("#branchSelect").val() == "0") {
    //    if ($("#employeeId").val() == "") {
    //        msg += "Branch Name Required!!\n";
    //    }

    //}
    //else {
    //    msg += "Branch Name Required!!\n";
    //}



    //if ($("#employeeSelect").val() == "0") {

    //    msg += "Employee Id Required!!\n";
    //}
    //else {
    //    msg += "Employee Id Required!!\n";
    //}

    return msg;
}

function Update() {
    let val = Validation()
    if (val == "") {
       



            var fdata = new FormData();


        fdata.append("Company", $("#Companyid").val());
        fdata.append("VendorName", $("#customVendorName").val());
        fdata.append("Manufacturer", $("#Manufacturer").val());
        fdata.append("BillDate", $("#BillDate").val());
        
        fdata.append("ReceivedDate", $("#ReceivedDate").val());
        fdata.append("materialid", $("#materialid").val());
                       

            var itemsArray = [];

            $("#itemsTableBody TR").each(function (index, row) {
                var Items = {};
                if ($(row).find('#SerialNo').val() != '') {
                    Items.matrialitemid = $(row).find("#matrialitemid").text();
                    Items.AssetItemId = $(row).find('#AssetItemId').val();
                    Items.ModelNo = $(row).find('#ModelNo').val();
                    Items.SerialNo = $(row).find('#SerialNo').val();
                    Items.Generation = $(row).find('#Generation').val();
                    Items.Processor = $(row).find('#Processor').val();
                    Items.RAMCapacity = $(row).find('#RAMCapacity').val();
                    Items.HardDisk = $(row).find('#HardDisk').val();
                    Items.SSDCapacity = $(row).find('#SSDCapacity').val();
                    Items.WarrantyDate = $(row).find('#WarrantyDate').val();
                    Items.Other = $(row).find('#Other').val();                   
                    itemsArray.push(Items);
                }

            });


            $("#itemsTableBody2 TR").each(function (index, row) {
                var Items = {};

                if ($(row).find('#SerialNo').val() != '') {

                    Items.matrialitemid = $(row).find("#matrialitemid").text();
                    Items.SerialNo = $(row).find('#SerialNo').val();
                    Items.AssetItemId = $(row).find('#AssetItemId').val();
                    Items.ModelNo = $(row).find('#ModelNo').val();
                    Items.ItemName = $(row).find('#ItemName').val();
                    Items.WarrantyDate = $(row).find('#WarrantyDate').val();
                    Items.Other = $(row).find('#Other').val();
                    Items.Generation = $(row).find('#Generation').val();
                    Items.Processor = $(row).find('#Processor').val();
                    Items.RAMCapacity = $(row).find('#RAMCapacity').val();
                    Items.HardDisk = $(row).find('#HardDisk').val();
                    Items.SSDCapacity = $(row).find('#SSDCapacity').val();
                   
                    itemsArray.push(Items);
                }
            });

            console.log(itemsArray);

            fdata.append("JsonData", JSON.stringify(itemsArray));

        var guestFileUpload = $("#invoiceUpload").get(0);
        if (guestFileUpload.files.length > 0) {
            fdata.append("file", guestFileUpload.files[0]);
            fdata.append("FileName", guestFileUpload);
        }

            if (itemsArray.length > 0) {


                $.ajax({

                    url: myurl + '/Material/UpdateIn',
                    type: 'post',
                    data: fdata,
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        if (data.message == 'Matrial In Updated SuccessFully') {
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
                alert("Please Select Any Item");
            }
        }


        
    
    else {
        alert(val);
    }
}

function uploadInvoice() {
    document.getElementById('invoiceUpload').click();
}



function DeleteInvoiceOrItems() {
    if ($("#InvoiceNo").val() != "0") {


        $.ajax({
            url: myurl + "/Material/DeleteInvoiceOrItems",
            type: "post",
            data: { invoiceNo: $("#InvoiceNo").val() },
            success: (data) => {
                var data = JSON.parse(data);
                if (data[0].Status == "Success") {
                    alert(data[0].Massage);
                    window.location.reload();
                } else {
                    alert(data[0].Massage);
                }
              
            }

        });
    } else {
        alert("Invoice No Required !!");
    }
}

function DeleteAllItems() {
    if ($("#InvoiceNo").val() != "0") {


        $.ajax({
            url: myurl + "/Material/DeleteAllItems",
            type: "post",
            data: { invoiceNo: $("#InvoiceNo").val() },
            success: (data) => {
                var data = JSON.parse(data);
                if (data[0].Status == "Success") {
                    alert(data[0].Massage);
                    window.location.reload();
                } else {
                    alert(data[0].Massage);
                }
            }

        });
    } else {
        alert("Invoice No Required !!");
    }
}


