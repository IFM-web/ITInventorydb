function updateTableRows() {
    const quantity = parseInt(document.getElementById('reqnQuantity').value);
    const assetType = $("#NewMaterial_AssetItemId option:selected").text();
    const assetTypeid = $("#NewMaterial_AssetItemId").val();
 
    // tbody.innerHTML = '';
    
    const isComputerAsset = assetType === 'Desktop' || assetType === 'Laptop' || assetType === 'Server';

    if (isComputerAsset) {
        const tbody = document.getElementById('itemsTableBody');
        for (let i = 0; i < 1; i++) {
            const row = document.createElement('tr');
            row.innerHTML = `
                    <td>${i + 1}</td>
                            <td>${assetType}</td>
                             <td class='d-none'>
                                      <input type="text"
                               name="NewMaterial.MaterialItems[${i}].AssetItemId"  value='${assetTypeid}'
                               class="form-control" 
                                />

                                  </td>
                    <td>
                        <input type="text" 
                               name="NewMaterial.MaterialItems[${i}].SerialNo" 
                               class="form-control" 
                               required />
                    </td>
                 
                    <td>
                        <div class="input-group">
                            <input type="text" 
                                   name="NewMaterial.MaterialItems[${i}].WarrantyDate" 
                                   class="form-control date-picker warranty-date" 
                                   placeholder="DD/MM/YYYY"
                                   autocomplete="off" required />
                            <span class="input-group-text">
                                <i class="fas fa-calendar"></i>
                            </span>
                        </div>
                    </td>
                    <td>
                        <input type="text" 
                               name="NewMaterial.MaterialItems[${i}].ModelNo" 
                               class="form-control" 
                               required />
                    </td>
                    <td id="tdGeneration${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="text" 
                               name="NewMaterial.MaterialItems[${i}].Generation" 
                               class="form-control"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdCPU${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="text" 
                               name="NewMaterial.MaterialItems[${i}].CPUCapacity" 
                               class="form-control"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdHDD${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="text" 
                               name="NewMaterial.MaterialItems[${i}].RAMCapacity" 
                               class="form-control"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdRAM${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="text" 
                               name="NewMaterial.MaterialItems[${i}].HardDisk" 
                               class="form-control"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdSSD${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="text" 
                               name="NewMaterial.MaterialItems[${i}].SSDCapacity" 
                               class="form-control" 
                               value="" />
                    </td>
                    <td>
                        <input type="text" 
                               name="NewMaterial.MaterialItems[${i}].Other" 
                               class="form-control" 
                               placeholder="Enter other details" />
                    </td>
                `;
            tbody.append(row);
        }
       
    }
    if (assetType=="Others") {
        const tbody = document.getElementById('itemsTableBody2');
        for (let i = 0; i < 1; i++) {
            const row = document.createElement('tr');
            row.innerHTML = `
                    <td>${i + 1}</td>
                            <td>${assetType}</td>
                                    <td class='d-none'>
                                      <input type="text" 
                               name="NewMaterial.MaterialItems[${i}].AssetItemId"  value='${assetTypeid}'
                               class="form-control" 
                                />

                                  </td>
                    <td>
                        <input type="text" 
                               name="NewMaterial.MaterialItems[${i}].SerialNo" 
                               class="form-control" 
                               required />
                    </td>
                    <td id="tdItemName${i}">
                        <input type="text" 
                               name="NewMaterial.MaterialItems[${i}].ItemName" 
                               class="form-control"
                               ${!isComputerAsset ? 'required' : ''} />
                    </td>
                    <td>
                        <div class="input-group">
                            <input type="text" 
                                   name="NewMaterial.MaterialItems[${i}].WarrantyDate" 
                                   class="form-control date-picker warranty-date" 
                                   placeholder="DD/MM/YYYY"
                                   autocomplete="off" />
                            <span class="input-group-text">
                                <i class="fas fa-calendar"></i>
                            </span>
                        </div>
                    </td>
                    <td>
                        <input type="text" 
                               name="NewMaterial.MaterialItems[${i}].ModelNo" 
                               class="form-control" 
                               required />
                    </td>
                    <td id="tdGeneration${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="text" 
                               name="NewMaterial.MaterialItems[${i}].Generation" 
                               class="form-control"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdCPU${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="number" 
                               name="NewMaterial.MaterialItems[${i}].CPUCapacity" 
                               class="form-control"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdHDD${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="number" 
                               name="NewMaterial.MaterialItems[${i}].HardDisk" 
                               class="form-control"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdRAM${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="number" 
                               name="NewMaterial.MaterialItems[${i}].RAMCapacity" 
                               class="form-control"
                               ${isComputerAsset ? 'required' : ''} />
                    </td>
                    <td id="tdSSD${i}" style="display:${isComputerAsset ? '' : 'none'}">
                        <input type="text" 
                               name="NewMaterial.MaterialItems[${i}].SSDCapacity" 
                               class="form-control" 
                               value="" />
                    </td>
                    <td>
                        <input type="text" 
                               name="NewMaterial.MaterialItems[${i}].Other" 
                               class="form-control" 
                               placeholder="Enter other details" />
                    </td>
                `;
            tbody.append(row);
        }
    }

    var dataRowCount = $('#itemsTableBody2  tr').length;
    var dataRowCount2 = $('#itemsTableBody  tr').length;

    $("#reqnQuantity").val(dataRowCount+dataRowCount2)

    

    // Initialize date pickers for warranty dates
    $('.warranty-date').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        todayHighlight: true,
        clearBtn: true,
        startDate: '01/01/2000',
        endDate: '31/12/2100'
    });

    // Click handler for calendar icons
    $('.input-group-text').click(function () {
        $(this).closest('.input-group').find('.date-picker').datepicker('show');
    });

    // Update validation when date changes
    $('.warranty-date').on('changeDate', function () {
        $(this).datepicker('hide');
        validateDate($(this));
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
    $('#NewMaterial_AssetItemId, #NewMaterial_VendorId, #NewMaterial_ManufacturerId').change(function () {
        const assetType = $("#NewMaterial_AssetItemId option:selected").text();
        const vendorType = $("#NewMaterial_VendorId option:selected").text();
        const manufacturerType = $("#NewMaterial_ManufacturerId option:selected").text();

        // Show/hide custom name fields
        $('#customAssetNameDiv').toggle(assetType === 'Others');
        $('#customVendorNameDiv').toggle(vendorType === 'Others');
        $('#customManufacturerNameDiv').toggle(manufacturerType === 'Others');

        // Clear and regenerate material items
        //updateTableRows();
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
    $(document).on('blur', '[name$="WarrantyDate"]', function () {
        const value = $(this).val();
        if (value && !validateDate($(this))) {
            $(this).addClass('is-invalid');
            $(this).next('.invalid-feedback').remove();
            $(this).after('<div class="invalid-feedback">Please enter a valid date in DD/MM/YYYY format</div>');
        }
    });
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

    // Click handler for calendar icons
    $('.input-group-text').click(function () {
        $(this).closest('.input-group').find('.date-picker').datepicker('show');
    });

    // Validate dates when changed
    $('.date-picker').on('changeDate', function () {
        $(this).datepicker('hide');
        validateDate($(this));
    });
});